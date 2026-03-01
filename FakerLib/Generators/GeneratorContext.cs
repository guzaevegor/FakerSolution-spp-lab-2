using System;

namespace FakerLib.Generators
{
    public class GeneratorContext
    {
        public Random Random { get; } = new Random();
        public Faker Faker { get; }

        public GeneratorContext(Faker faker)
        {
            Faker = faker;
        }
    }
}
