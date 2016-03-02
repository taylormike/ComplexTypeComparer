using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using val = ValueComparison.ValueComparer;

namespace ValueComparer.UnitTests
{
    [TestFixture]
    public class VaueComparerTests
    {
        [TestCase]
        public void ObjectComparison()
        {
            var cat1 = new Cat()
            {
                Name = "Cinder",
                Type = "Tabby"
            };

            var cat2 = new Cat()
            {
                Name = "Cinder",
                Type = "Tabby2"
            };

            var cat3 = new Cat()
            {
                Name = "Cinder",
                Type = "Tabby"
            };

            Assert.False(val.AreEqual(cat1, cat2));
            Assert.True(val.AreEqual(cat1, cat3));
        }
    }
}
