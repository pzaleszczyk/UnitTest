using NUnit.Framework;

namespace PzaleszczykNUnitTest
{
    /*
   będą zawierały najmniej 3 różne typy assercji (patrz)  - 2 punkty
       Assert.AreEqual, Assert.ThrowsException, Assert.IsNull
   będą zawierały najmniej jeden Data-Driven Unit Test (patrz)  - 2 punkty
       TODO
   wykorzystają Microsoft Fakes (stubs & shims) - 2 punkty
       TODO
   wykorzystać FluentAssertions w testach - 1 punkt
       testy z *Fluent()
   */

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void TearDown()
        {
            //_car1 = null;
            //_car2 = null;
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }


        [TestCase("11111111111")]
        [TestCase("22222222223")]
        [TestCase("33333333333")]
        [TestCase("12312312312")]
        public void test(string value)
        {

        }


    }
}