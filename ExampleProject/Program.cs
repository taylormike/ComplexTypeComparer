using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueComparison;

namespace ExampleProject
{
    class Program
    {
        static void Main(string[] args)
        {

            var cat1 = new Cat() { Head = "head", Tail = "tail" };
            var cat2 = new Cat() { Head = "head", Tail = "tail" };

            var result = ValueComparer.AreEqual(cat1, cat2);

        }
    }
}
