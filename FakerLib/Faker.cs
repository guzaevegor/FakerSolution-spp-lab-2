using FakerLib.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace FakerLib
{
    public class Faker
    {
        private readonly List<IValueGenerator> _generators;
        private readonly Stack<Type> _creationStack = new Stack<Type>();
        private readonly GeneratorContext _context;

        public Faker()
        {
            _context = new GeneratorContext(this);

            _generators = new List<IValueGenerator>
            {
                new IntGenerator(),
                new DoubleGenerator(),
                new StringGenerator(),
                new DateTimeGenerator(),
                new EnumerableGenerator()
            };
        }

        public T Create<T>() => (T)Create(typeof(T));

        public object Create(Type type)
        {
            // поиск готового генератора для базовых типов или коллекций
            var generator = _generators.FirstOrDefault(g => g.CanGenerate(type));
            if (generator != null)
            {
                return generator.Generate(type, _context);
            }

            // обработка циклических зависимостей
            if (_creationStack.Contains(type))
            {
                return type.IsValueType ? Activator.CreateInstance(type) : null;
            }

            _creationStack.Push(type);
            try
            {
                return CreateComplexObject(type);
            }
            finally
            {
                _creationStack.Pop();
            }
        }

        private object CreateComplexObject(Type type)
        {
            // получаем конструкторы и сортируем по убыванию параметров
            var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                   .OrderByDescending(c => c.GetParameters().Length);

            object instance = null;
            var initializedMembers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // пыттаемся создать через конструктор
            foreach (var ctor in constructors)
            {
                try
                {
                    var parameters = ctor.GetParameters();
                    var args = new object[parameters.Length];

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        args[i] = Create(parameters[i].ParameterType);
                        initializedMembers.Add(parameters[i].Name);
                    }

                    instance = ctor.Invoke(args);
                    break;
                }
                catch
                {
                    initializedMembers.Clear();
                }
            }

            // если нет явных конструкторов, но это структура (value type)
            if (instance == null && type.IsValueType)
            {
                instance = Activator.CreateInstance(type);
            }

            if (instance == null) return null;

            // заполняем публичные свойства (которые не были заполнены в конструкторе)
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                 .Where(p => p.CanWrite && p.SetMethod != null && p.SetMethod.IsPublic);

            foreach (var prop in properties)
            {
                if (initializedMembers.Contains(prop.Name)) continue;
                prop.SetValue(instance, Create(prop.PropertyType));
            }

            // заполняем публичные поля
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                             .Where(f => !f.IsInitOnly);

            foreach (var field in fields)
            {
                if (initializedMembers.Contains(field.Name)) continue;
                field.SetValue(instance, Create(field.FieldType));
            }

            return instance;
        }
    }
}
