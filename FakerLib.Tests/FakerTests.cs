using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using FakerLib;
using FakerLib.Tests.TestEntities;

namespace FakerLib.Tests
{
    public class FakerTests
    {
        private readonly Faker _faker;

        public FakerTests()
        {
            _faker = new Faker();
        }

        [Fact]
        public void Create_ShouldGenerateBasicTypes()
        {
           
            var intVal = _faker.Create<int>();
            var stringVal = _faker.Create<string>();
            var doubleVal = _faker.Create<double>();
            var dateVal = _faker.Create<DateTime>();

            
            intVal.Should().NotBe(0); // Вероятность сгенерировать 0 стремится к нулю
            stringVal.Should().NotBeNullOrEmpty();
            doubleVal.Should().NotBe(0.0);
            dateVal.Should().NotBe(default(DateTime));
        }

        [Fact]
        public void Create_ShouldFillPublicPropertiesAndFields()
        {
           
            var result = _faker.Create<SimpleClass>();

            
            result.Should().NotBeNull();
            result.NumberField.Should().NotBe(0);
            result.StringProperty.Should().NotBeNullOrEmpty();
            result.DateProperty.Should().NotBe(default(DateTime));

            // Приватный сеттер не должен быть заполнен (остается default)
            result.PrivateSetterProperty.Should().Be(0.0);
        }

        [Fact]
        public void Create_ShouldUseConstructor_AndFillRemainingFields()
        {
           
            var result = _faker.Create<ConstructorClass>();

            
            result.Should().NotBeNull();

            // Заполнено через конструктор
            result.Name.Should().NotBeNullOrEmpty();
            result.Age.Should().NotBe(0);

            // Заполнено напрямую (так как не было в конструкторе)
            result.Salary.Should().NotBe(0.0);
        }

        [Fact]
        public void Create_ShouldHandleCircularDependencies_WithoutStackOverflow()
        {
           
            var result = _faker.Create<ClassA>();

            
            result.Should().NotBeNull();
            result.B.Should().NotBeNull();
            result.B.C.Should().NotBeNull();

            // Цикл должен прерваться, свойство A внутри C должно быть null
            result.B.C.A.Should().BeNull();
        }

        [Fact]
        public void Create_ShouldGenerateStructs()
        {
           
            var result = _faker.Create<TestStruct>();

            
            result.X.Should().NotBe(0);
            result.Y.Should().NotBe(0);
        }

        [Fact]
        public void Create_ShouldGenerateCollections_WithoutSwitchInsideFaker()
        {
           
            var listResult = _faker.Create<List<int>>();
            var ilistResult = _faker.Create<IList<string>>();
            var ienumerableResult = _faker.Create<IEnumerable<double>>();

            
            listResult.Should().NotBeNullOrEmpty();
            listResult.Count.Should().BeGreaterThan(0);

            ilistResult.Should().NotBeNullOrEmpty();

            ienumerableResult.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Create_ShouldGenerateCollectionsOfComplexObjects()
        {
           
            var result = _faker.Create<List<SimpleClass>>();

            
            result.Should().NotBeNullOrEmpty();
            result[0].Should().NotBeNull();
            result[0].StringProperty.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Create_ShouldGenerateNestedCollections()
        {

            var result = _faker.Create<List<List<int>>>();

            result.Should().NotBeNullOrEmpty();
            result[0].Should().NotBeNullOrEmpty(); // Внутренний список тоже должен быть сгенерирован
        }
    }
}
