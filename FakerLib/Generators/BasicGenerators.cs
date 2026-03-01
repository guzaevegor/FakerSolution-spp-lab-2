using System;

namespace FakerLib.Generators
{
    public class IntGenerator : IValueGenerator
    {
        public bool CanGenerate(Type type) => type == typeof(int);
        public object Generate(Type type, GeneratorContext context) => context.Random.Next(1, 1000);
    }

    public class DoubleGenerator : IValueGenerator
    {
        public bool CanGenerate(Type type) => type == typeof(double);
        public object Generate(Type type, GeneratorContext context) => context.Random.NextDouble() * 100;
    }

    public class StringGenerator : IValueGenerator
    {
        public bool CanGenerate(Type type) => type == typeof(string);
        public object Generate(Type type, GeneratorContext context) => Guid.NewGuid().ToString().Substring(0, 8);
    }

    public class DateTimeGenerator : IValueGenerator
    {
        public bool CanGenerate(Type type) => type == typeof(DateTime);
        public object Generate(Type type, GeneratorContext context)
            => DateTime.Now.AddDays(context.Random.Next(-1000, 1000));
    }
}
