using System;

namespace FakerLib.Generators
{
    public interface IValueGenerator
    {
        bool CanGenerate(Type type);
        object Generate(Type type, GeneratorContext context);
    }
}
