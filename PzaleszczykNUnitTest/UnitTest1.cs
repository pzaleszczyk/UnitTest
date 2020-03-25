using NUnit.Framework;
using PzaleszczykLib;
using Pose;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Globalization;

namespace PzaleszczykNUnitTest
{
    /*
   będą zawierały najmniej 3 różne typy assercji (patrz)  - 2 punkty
       Assert.AreEqual, Assert.ThrowsException, Assert.IsNull, CollectionAssert.AllItemsAreNotNull, StringAssert.Contains
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


        [Test, TestCaseSource("getAffineData")]
        public void TestEncryptValues(int keya,int keyb, string before, string after)
        {
            cypher.A_crypt(after,keya,keyb).Should().Be(before);
        }


        [TestCase(1, 10, "Ptjfn!", "Zdtpx!")]
        [TestCase(15, 19, "Łśżź!", "Łśżź!")]
        public void AffineEncrypt(int keya, int keyb, string text, string expected)
        {
            string actual = cypher.A_crypt(text, keya, keyb);

            Assert.AreEqual(expected, actual, "Affine encrypt is incorrect");

        }

        [Test, TestCaseSource("getAffineData")]
        public void TestDecryptValues(int keya, int keyb, string before, string after)
        {
            cypher.A_decrypt(before, keya, keyb).Should().Be(after);
        }

        private static IEnumerable<TestCaseData> getAffineData()
        {
            using var csv = new CsvReader(new StreamReader("../../../../PzaleszczykUnitTest/AffineDecrypt.csv"), CultureInfo.InvariantCulture);
            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                int keya = Int32.Parse(csv[0]);
                int keyb = Int32.Parse(csv[1]);
                string before = csv[2];
                string after = csv[3];

                yield return new TestCaseData(keya,keyb,before,after);
            }
        }




        [Test]
        public void shimtest1() {
            Action action = new Action(() => { });
            int val = 0;

            Shim shim = Shim.Replace(() => cypher.return2()).With(
                  delegate (Cypher @this)
                  {
                      Console.WriteLine("Returning 1 instead of 2");
                      return 1;
                  }
             );

            PoseContext.Isolate(() =>
            {
                StubChecker nowy = new StubChecker();
                val = cypher.return2();
            }, shim);

            //PoseContext.Isolate(() => {result = cypher.A_crypt("Text", 15, 10); }, shim);
            val.Should().NotBe(cypher.return2());
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


        [Test]
        public void AffineAnalyzeAll()
        {
            int expected = 312;
            int actual = cypher.A_analyzeAll("Bulwa").Length;
            Assert.AreEqual(expected, actual);

        }
        [Test]
        public void CaesarAnalyzeAll()
        {
            int expected = 25;
            int actual = cypher.C_analyzeAll("Bulwa").Length;
            Assert.AreEqual(expected, actual);

        }

        [TestCase(1, "Osiem!", "Ptjfn!")]
        [TestCase(2, "!S iedem.?!", "!U kgfgo.?!")]
        [TestCase(3, "DFSDFADFSASDF", "GIVGIDGIVDVGI")]
        [TestCase(4, "..//./....//.///.////", "..//./....//.///.////")]
        [TestCase(87, "łŁ", "łŁ")]
        public void CaesarDecrypt(int key, string expected, string input)
        {
            string actual = cypher.C_decrypt(input, key);
            Assert.AreEqual(expected, actual, "Ceasar decrypt is incorrect");
        }


        [Test, TestCaseSource("getAffineData")]
        public void AffineDecrypt(int keya,int keyb, string text, string expected)
        {
            string actual = cypher.A_decrypt(text, keya, keyb);


            Assert.AreEqual(expected, actual, "Affine decrypt is incorrect");

        }


        [Test]
        public void InverseTest()
        {
            int expected = 9;
            int value = cypher.Inverse(expected);
            int actual = cypher.Inverse(value);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void InverseTestNegative()
        {
            int expected = -1;
            int value = cypher.Inverse(10);

            Assert.AreEqual(expected, value);
        }

        [Test]
        public void AffineAnalyzeAllNull()
        {
            string[] expected = cypher.A_analyzeAll(null);
            Assert.IsNull(expected, "Does not return null");
        }
        [Test]
        public void CaesarAnalyzeAllNull()
        {
            string[] expected = cypher.C_analyzeAll(null);
            Assert.IsNull(expected, "Does not return null");
        }

        [Test]
        public void CaesarAnalyzeNoneNull()
        {
            //CollectionAssert.AllItemsAreNotNull(ICollection);
            string[] expected = cypher.C_analyzeAll("asdfadfadfasdf");
            CollectionAssert.AllItemsAreNotNull(expected);
            //Assert.IsNull(expected, "Does not return null");
        }

        [Test]
        public void AffineAnalyzeNoneNull()
        {
            string[] expected = cypher.A_analyzeAll("dasdasdasdas");
            CollectionAssert.AllItemsAreNotNull(expected);
            //Assert.IsNull(expected, "Does not return null");
        }

        [Test]
        public void CaesarAnalyzeKey()
        {
            string extra = "sie";
            string encrypted_text = "Ptjfn!";
            string[] actual = cypher.C_analyze(extra, encrypted_text);
            string expected = 1 + "";

            Assert.AreEqual(expected, actual[0], "Caesar analyze (key) is incorrect");
        }

        [Test]
        public void CaesarAnalyzeText()
        {
            string extra = "sie";
            string encrypted_text = "Ptjfn!";
            string[] actual = cypher.C_analyze(extra, encrypted_text);
            string expected = "Osiem!";

            Assert.AreEqual(expected, actual[1], "Caesar analyze (text) is incorrect");
        }


        [TestCase(1, "Osiem!", "Ptjfn!")]
        [TestCase(2, "!S iedem.?!", "!U kgfgo.?!")]
        [TestCase(3, "DFSDFADFSASDF", "GIVGIDGIVDVGI")]
        [TestCase(4, "..//./....//.///.////", "..//./....//.///.////")]
        [TestCase(7, "Łł", "Łł")]
        public void CaesarEncrypt(int key, string input, string output)
        {
            string actual = cypher.C_crypt(input, key);
            Assert.AreEqual(output, actual, "Ceasar encrypt is incorrect");
        }

        [Test]
        public void CaesarEncryptFluent()
        {
            string text = "Osiem!";
            int key = 1;

            string actual = cypher.C_crypt(text, key);

            //FluentAssertions example
            actual.Should().StartWith("P").And.EndWith("!").And.Contain("tjfn");
        }


        [Test]
        public void AffineAnalyzeKeyA()
        {
            //encrypted = "Zdtpx!"   plain = "Ptjfn!"
            string extra = "tjf";
            string encrypted_text = "Zdtpx!";

            string[] actual = cypher.A_analyze(extra, encrypted_text);
            string expected = 1 + "";

            Assert.AreEqual(expected, actual[0], "Affine analyze (keya) is incorrect");
        }
        [Test]
        public void AffineAnalyzeKeyB()
        {
            //encrypted = "Zdtpx!"   plain = "Ptjfn!"
            string extra = "tjf";
            string encrypted_text = "Zdtpx!";

            string[] actual = cypher.A_analyze(extra, encrypted_text);
            string expected = 10 + "";

            Assert.AreEqual(expected, actual[1], "Affine analyze (keyb) is incorrect");
        }
        [Test]
        public void AffineAnalyzeText()
        {
            //encrypted = "Zdtpx!"   plain = "Ptjfn!"
            string extra = "tjf";
            string encrypted_text = "Zdtpx!";

            string[] actual = cypher.A_analyze(extra, encrypted_text);
            string expected = "Ptjfn!";

            Assert.AreEqual(expected, actual[2], "Affine analyze (text) is incorrect");
        }
    }
}