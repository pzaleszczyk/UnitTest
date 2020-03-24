using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PzaleszczykLib;
using FluentAssertions;
using System.Diagnostics;

namespace PzaleszczykUnitTest
{
    /*
    będą zawierały najmniej 3 różne typy assercji (patrz)  - 2 punkty
        Assert.AreEqual, Assert.ThrowsException, Assert.IsNull, Assert.Fail, StringAssert.Contains, CollectionAssert.AllItemsAreNotNull
    będą zawierały najmniej jeden Data-Driven Unit Test (patrz)  - 2 punkty
        [DataTestMethod/DataRow] oraz [DataSource] z pliku csv.
    wykorzystają Microsoft Fakes (stubs & shims) - 2 punkty
        TODO
    wykorzystać FluentAssertions w testach - 1 punkt
        testy z *Fluent()
    */

    [TestClass]
    public class UnitTest
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        public Cypher cypher;

        [TestInitialize()]
        public void Startup()
        {
            cypher = new Cypher();
            Debug.WriteLine(this.TestContext.ToString());
        }

        [TestCleanup()]
        public void Cleanup()
        {
            cypher = null;
        }

        [DataTestMethod]
        [DataRow(1,"Osiem!", "Ptjfn!")]
        [DataRow(2,"!S iedem.?!", "!U kgfgo.?!")]
        [DataRow(3,"DFSDFADFSASDF", "GIVGIDGIVDVGI")]
        [DataRow(4,"..//./....//.///.////", "..//./....//.///.////")]
        [DataRow(7, "Łł", "Łł")]
        public void CaesarEncrypt(int key, string input, string output)
        {
            string actual = cypher.C_crypt(input, key);
            Assert.AreEqual(output, actual, "Ceasar encrypt is incorrect");
        }

        [TestMethod]
        public void CaesarEncryptFluent()
        {
            string text = "Osiem!";
            int key = 1;

            string actual = cypher.C_crypt(text, key);

            //FluentAssertions example
            actual.Should().StartWith("P").And.EndWith("!").And.Contain("tjfn");
        }

        [DataTestMethod]
        [DataRow(1, "Osiem!", "Ptjfn!")]
        [DataRow(2, "!S iedem.?!", "!U kgfgo.?!")]
        [DataRow(3, "DFSDFADFSASDF", "GIVGIDGIVDVGI")]
        [DataRow(4, "..//./....//.///.////", "..//./....//.///.////")]
        [DataRow(87, "łŁ", "łŁ")]
        public void CaesarDecrypt(int key, string expected, string input)
        { 
            string actual = cypher.C_decrypt(input, key);
            Assert.AreEqual(expected, actual, "Ceasar decrypt is incorrect");
        }
        //Dane z database
        //[DataSource(
        //    "System.Data.SqlClient",
        //    "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DataSource;Integrated Security=True",
        //    "Encrypt",
        //    DataAccessMethod.Sequential), TestMethod]

        [DataTestMethod]
        [DataRow(1, 10, "Ptjfn!", "Zdtpx!")]
        [DataRow(15, 19, "Łśżź!", "Łśżź!")]
        public void AffineEncrypt(int keya, int keyb, string text, string expected)
        {
            string actual = cypher.A_crypt(text, keya, keyb);

            Assert.AreEqual(expected, actual, "Affine encrypt is incorrect");

        }

        //Dane z pliku csv
        [DeploymentItem("..\\..\\AffineDecrypt.csv")]
        [DataSource(
            "Microsoft.VisualStudio.TestTools.DataSource.CSV",
            "|DataDirectory|\\AffineDecrypt.csv",
            "AffineDecrypt#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void AffineDecrypt()
        {
            int keya = ((int)TestContext.DataRow[0]);
            int keyb = ((int)TestContext.DataRow[1]);
            string text = ((string)TestContext.DataRow[2]);
            string actual = cypher.A_decrypt(text, keya, keyb);
            string expected = ((string)TestContext.DataRow[3]);

            Assert.AreEqual(expected, actual, "Affine decrypt is incorrect");

        }

        [TestMethod]
        public void AffineDecryptPolish()
        {
            int keya = 19;
            int keyb = 2;
            string text = "łŁo";
            string expected = "łŁc";
            string actual = cypher.A_decrypt(text, keya, keyb);

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
        public void CaesarAnalyzeNoneNull()
        {
            //CollectionAssert.AllItemsAreNotNull(ICollection);
            string[] expected = cypher.C_analyzeAll("asdfadfadfasdf");
            CollectionAssert.AllItemsAreNotNull(expected);
            //Assert.IsNull(expected, "Does not return null");
        }

        [TestMethod]
        public void AffineAnalyzeNoneNull()
        {
            string[] expected = cypher.A_analyzeAll("dasdasdasdas");
            CollectionAssert.AllItemsAreNotNull(expected);
            //Assert.IsNull(expected, "Does not return null");
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

        [TestMethod]
        public void InverseTestNegative()
        {
            int expected = -1;
            int value = cypher.Inverse(10);

            Assert.AreEqual(expected, value);
        }

        [TestMethod]
        public void AffineAnalyzeAll()
        {
            int expected = 312;
            int actual = cypher.A_analyzeAll("Bulwa").Length;
            Assert.AreEqual(expected, actual);

        }
        [TestMethod]
        public void CaesarAnalyzeAll()
        {
            int expected = 25;
            int actual = cypher.C_analyzeAll("Bulwa").Length;
            Assert.AreEqual(expected, actual);

        }

       
    }

}
