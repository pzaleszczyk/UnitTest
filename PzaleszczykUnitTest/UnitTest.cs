using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PzaleszczykLib;

namespace PzaleszczykUnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void CaesarEncrypt()
        {
            Cypher cypher = new Cypher();
            string text = "Osiem!";
            int key = 1;

            string actual = cypher.C_crypt(text, key);
            string expected = "Ptjfn!";

            Assert.AreEqual(expected, actual, "Ceasar encrypt is incorrect");
        }
        [TestMethod]
        public void CaesarDecrypt()
        {

        }
        [TestMethod]
        public void AffineEncrypt()
        {

        }
        [TestMethod]
        public void AffineDecrypt()
        {

        }
        [TestMethod]
        public void CaesarAnalyze()
        {

        }
        [TestMethod]
        public void AffineAnalyze()
        {

        }
        [TestMethod]
        public void AffineAnalyzeAll()
        {

        }
        [TestMethod]
        public void CaesarAnalyzeAll()
        {

        }

        [TestMethod]
        public void GCDTest()
        {

        }

        [TestMethod]
        public void InverseTest()
        {

        }
    }
}

