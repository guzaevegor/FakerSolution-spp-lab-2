using System;
using System.Collections;
using System.Collections.Generic;

namespace FakerLib.Generators
{
    public class EnumerableGenerator : IValueGenerator
    {
        public bool CanGenerate(Type type)
        {
            if (!type.IsGenericType) return false;
            var genericType = type.GetGenericTypeDefinition();

            return genericType == typeof(List<>) ||
                   genericType == typeof(IList<>) ||
                   genericType == typeof(IEnumerable<>) ||
                   genericType == typeof(ICollection<>);
        }

        public object Generate(Type type, GeneratorContext context)
        {
            var itemType = type.GetGenericArguments()[0];
            var listType = typeof(List<>).MakeGenericType(itemType);
            var list = (IList)Activator.CreateInstance(listType);


            int count = context.Random.Next(2, 6);
            for (int i = 0; i < count; i++)
            {
                list.Add(context.Faker.Create(itemType));
            }
            return list;
        }
    }
}
