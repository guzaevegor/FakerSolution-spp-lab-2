using System;
using System.Collections.Generic;

namespace FakerLib.Tests.TestEntities
{
    // 1. Простой класс с публичными свойствами и полями
    public class SimpleClass
    {
        public int NumberField;
        public string StringProperty { get; set; }
        public DateTime DateProperty { get; set; }

        // Приватное свойство не должно заполняться
        public double PrivateSetterProperty { get; private set; }
    }

    // 2. Класс с конструктором
    public class ConstructorClass
    {
        public string Name { get; }
        public int Age { get; }

        // Поле, которое не инициализируется в конструкторе
        public double Salary;

        public ConstructorClass(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }

    // 3. Классы с циклической зависимостью (A -> B -> C -> A)
    public class ClassA
    {
        public ClassB B { get; set; }
    }

    public class ClassB
    {
        public ClassC C { get; set; }
    }

    public class ClassC
    {
        public ClassA A { get; set; }
    }

    // 4. Структура (Value Type)
    public struct TestStruct
    {
        public int X;
        public int Y;
    }
}
