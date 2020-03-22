using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PzaleszczykLib;
using FluentAssertions;

namespace PzaleszczykUnitTest
{
    /*
    będą zawierały najmniej 3 różne typy assercji (patrz)  - 2 punkty
        Assert.AreEqual, Assert.ThrowsException,
    będą zawierały najmniej jeden Data-Driven Unit Test (patrz)  - 2 punkty
        
    wykorzystają Microsoft Fakes (stubs & shims) - 2 punkty
        
    wykorzystać FluentAssertions w testach - 1 punkt
        testy z *Fluent()
    */


    [TestClass]
    public class UnitTest
    {
        public Cypher cypher;

        [TestInitialize()]
        public void Startup()
        {
            cypher = new Cypher();
        }

        [TestCleanup()]
        public void Cleanup()
        {
            cypher = null;
        }

        [TestMethod]
        public void CaesarEncrypt()
        {
            string text = "Osiem!";
            int key = 1;

            string actual = cypher.C_crypt(text, key);
            string expected = "Ptjfn!";

            Assert.AreEqual(expected, actual, "Ceasar encrypt is incorrect");
        }

        [TestMethod]
        public void CaesarEncryptFluent()
        {
            string text = "Osiem!";
            int key = 1;

            string actual = cypher.C_crypt(text, key);
            string expected = "Ptjfn!";

            //FluentAssertions example
            actual.Should().StartWith("P").And.EndWith("!").And.Contain("j");
        }

        [TestMethod]
        public void CaesarDecrypt()
        {
            string text = "Ptjfn!";
            int key = 1;

            string actual = cypher.C_decrypt(text, key);
            string expected = "Osiem!";

            Assert.AreEqual(expected, actual, "Ceasar decrypt is incorrect");
        }

        [TestMethod]
        public void AffineEncrypt()
        {
            string text = "Ptjfn!";
            int keya = 1;
            int keyb = 10;

            string actual = cypher.A_crypt(text, keya, keyb);
            string expected = "Zdtpx!";

            Assert.AreEqual(expected, actual, "Affine encrypt is incorrect");

        }
        [TestMethod]
        public void AffineDecrypt()
        {
            string text = "Zdtpx!";
            int keya = 1;
            int keyb = 10;

            string actual = cypher.A_decrypt(text, keya, keyb);
            string expected = "Ptjfn!";

            Assert.AreEqual(expected, actual, "Affine decrypt is incorrect");

        }

        [TestMethod]
        public void CaesarAnalyzeKey()
        {
            string extra = "sie" ;
            string encrypted_text = "Ptjfn!";
            string[] actual = cypher.C_analyze(extra, encrypted_text);
            string expected = 1+"";

            Assert.AreEqual(expected, actual[0], "Caesar analyze (key) is incorrect");
        }

        [TestMethod]
        public void CaesarAnalyzeText()
        {
            string extra = "sie";
            string encrypted_text = "Ptjfn!";
            string[] actual = cypher.C_analyze(extra, encrypted_text);
            string expected = "Osiem!";

            Assert.AreEqual(expected, actual[1], "Caesar analyze (text) is incorrect");
        }

        [TestMethod]
        public void AffineAnalyzeKeyA()
        {
            //encrypted = "Zdtpx!"   plain = "Ptjfn!"
            string extra = "tjf";
            string encrypted_text = "Zdtpx!";

            string[] actual = cypher.A_analyze(extra, encrypted_text);
            string expected = 1+"";

            Assert.AreEqual(expected, actual[0], "Affine analyze (keya) is incorrect");
        }
        [TestMethod]
        public void AffineAnalyzeKeyB()
        {
            //encrypted = "Zdtpx!"   plain = "Ptjfn!"
            string extra = "tjf";
            string encrypted_text = "Zdtpx!";

            string[] actual = cypher.A_analyze(extra, encrypted_text);
            string expected = 10+"";

            Assert.AreEqual(expected, actual[1], "Affine analyze (keyb) is incorrect");
        }
        [TestMethod]
        public void AffineAnalyzeText()
        {
            //encrypted = "Zdtpx!"   plain = "Ptjfn!"
            string extra = "tjf";
            string encrypted_text = "Zdtpx!";

            string[] actual = cypher.A_analyze(extra, encrypted_text);
            string expected = "Ptjfn!";

            Assert.AreEqual(expected, actual[2], "Affine analyze (text) is incorrect");
        }


        [TestMethod]
        public void AffineAnalyzeAllNull()
        {
            string[] expected = cypher.A_analyzeAll(null);
            Assert.IsNull(expected, "Does not return null");
        }
        [TestMethod]
        public void CaesarAnalyzeAllNull()
        {
            string[] expected = cypher.C_analyzeAll(null);
            Assert.IsNull(expected, "Does not return null");
        }

        [TestMethod]
        public void GCDTest()
        {
            int actual = cypher.GCD(17, 26);
            Assert.AreEqual(1, actual);
        }

        [TestMethod]
        public void InverseTest()
        {
            int expected = 9;
            int value = cypher.Inverse(expected);
            int actual = cypher.Inverse(value);

            Assert.AreEqual(expected, actual);
        }
    }
}

