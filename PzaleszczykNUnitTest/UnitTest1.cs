using NUnit.Framework;
using PzaleszczykLib;
using Pose;
using FluentAssertions;
using System;

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
        Cypher cypher;

        [SetUp]
        public void Setup()
        {
            cypher = new Cypher();
        }

        [TearDown]
        public void TearDown()
        {
            cypher = null;
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void shimtest1() {
            Action action = new Action(() => { });


            Shim shim = Shim.Replace(() => cypher.return2()).With(
                  delegate (Cypher @this)
                  {
                      Console.WriteLine("Returning 1 instead of 2");
                      return 1;
                  }
             );

            // PoseContext.Isolate(() => {result = cypher.A_crypt("Text", 15, 10); }, shim);
            //result.Should().Be("Osiem");
        }

        [Test]
        public void shimtestwithstub()
        {
            StubCypher stub = new StubCypher();
            int inverse = 0;

            var cshim = Shim.Replace(() => new StubChecker())
                .With(() => new StubChecker(stub));

            PoseContext.Isolate(() =>
            {
                StubChecker nowy = new StubChecker();
                inverse = nowy.Inverse(1);
            }, cshim);

            inverse.Should().Be(9);
        }


        [TestCase("1")]
        [TestCase("2")]
        [TestCase("3")]
        [TestCase("4")]
        public void GCDStubTest(int value)
        {
            StubCypher stub = new StubCypher();
            StubChecker checker = new StubChecker(stub);

            //Normalnie 1 powinna byc tylko dla liczb { 1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 23, 25 }
            //Stubujac zawsze jest

            int result = checker.GCD(value,13);
            result.Should().Be(1);
        }

        [TestCase("1")]
        [TestCase("2")]
        [TestCase("3")]
        [TestCase("4")]
        public void InverseStubTest(int value)
        {
            StubCypher stub = new StubCypher();
            StubChecker checker = new StubChecker(stub);
            //StubChecker actual = new StubChecker();

            //Normalnie 9 powinna byc tylko dla 3.
            //Stubujac zawsze jest

            int result = checker.Inverse(value);
            result.Should().Be(9);
            //result.Should().NotBe(actual.Inverse(value));
        }

    }
}