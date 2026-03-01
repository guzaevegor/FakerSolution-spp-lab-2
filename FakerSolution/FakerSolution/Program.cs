using System;
using System.Collections.Generic;
using FakerLib; 

namespace FakerSolution
{
    class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class Program
    {
        static void Main()
        {
            var faker = new Faker();

            int i = faker.Create<int>();
            double d = faker.Create<double>();
            Console.WriteLine($"Generated Int: {i}, Double: {d:F3}");

            User user = faker.Create<User>();
            Console.WriteLine($"Generated User: Name='{user.Name}', Age={user.Age}");

            List<User> users = faker.Create<List<User>>();
            Console.WriteLine($"Generated List<User> with {users.Count} items.");
            foreach (var u in users)
            {
                Console.WriteLine($"  -> Name: {u.Name}, Age: {u.Age}");
            }

            List<List<int>> nestedList = faker.Create<List<List<int>>>();
            Console.WriteLine($"Generated nested List<List<int>> with {nestedList.Count} items.");
        }
    }
}
