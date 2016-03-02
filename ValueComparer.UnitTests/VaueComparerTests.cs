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



        [TestCase]
        public void IEnumerablePropertyComprasion()
        {
            var cat1A = new Cat()
            {
                Name = "Cinder",
                Type = "Tabby"
            };

            var cat2A = new Cat()
            {
                Name = "Cinder",
                Type = "Tabby2"
            };

            var cat3A = new Cat()
            {
                Name = "Cinder",
                Type = "Tabby"
            };

             var cat1B = new Cat()
            {
                Name = "Cinder",
                Type = "Tabby"
            };

            var cat2B = new Cat()
            {
                Name = "Cinder",
                Type = "Tabby2"
            };

            var cat3B = new Cat()
            {
                Name = "Cinder",
                Type = "Tabby"
            };


            List<Cat> cats1 = new List<Cat>(3);
            cats1.Add(cat1A);
            cats1.Add(cat2A);
            cats1.Add(cat3A);

            List<Cat> cats2 = new List<Cat>(3);
            cats2.Add(cat1B);
            cats2.Add(cat2B);
            cats2.Add(cat3B);


            CatOwner bob = new CatOwner()
            {
                Cats = cats1
            };

            CatOwner james = new CatOwner()
            {
                Cats = cats2
            };

            Assert.True(val.AreEqual(bob, james));
        }
    }
}
