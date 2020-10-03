using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Homework12
{
    class Person
    {
        public string Name;
        private int Age = 12;
    }

    class Car
    {
        [MyIgnore]
        public string Mark;
        [MyIgnore]
        public int HorsePowers;
    }
    class Program
    {
        static void Main(string[] args)
        {
            Person tom = new Person { Name = "Tom" };
            Console.WriteLine(SerializeToJson(tom));

            Car car = new Car() { Mark = "Toyota", HorsePowers = 600 };
            Console.WriteLine(SerializeToJson(car));

            Console.ReadKey();
        }

        static string SerializeToJson(object o)
        {
            StringBuilder result = new StringBuilder();
            var fields = o.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(x => x.GetCustomAttribute<MyIgnoreAttribute>() == null);
            if (!fields.Any())
            {
                throw new MyException();
            }
            result.Append("{");
            foreach(var f in fields)
            {
                result.Append("\"" + f.Name + "\":");
                if(f.GetValue(o).GetType() == "".GetType())
                {
                    result.Append("\"" + f.GetValue(o) + "\",");
                }
                else
                {
                    result.Append(f.GetValue(o) + ",");
                }
            }
            result[result.Length - 1] = '}';

            return result.ToString();
        }
    }
}
