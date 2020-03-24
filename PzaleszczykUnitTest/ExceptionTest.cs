using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using PzaleszczykLib;

namespace PzaleszczykUnitTest
{
    [TestClass]
    public class UnitTestException
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
            try
            {
                cypher.C_crypt("exception", -1);
            }
            catch (Exception e)
            {
                // Assert
                StringAssert.Contains(e.Message, "ERROR: Klucz nie jest z przedzialu (1-25)");
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void AffineEncryptWrongKeyFluent()
        {
            Action action = () => cypher.A_crypt("exception", 13, 13);
            action.Should().Throw<Exception>().WithMessage("ERROR: Niepoprawny format klucza");
        }

        [TestMethod]
        public void CaesarAnalyzeFluent()
        {
            Action action = () => cypher.C_analyze("?","!");
            action.Should().Throw<Exception>().WithMessage("ERROR: Nie da sie znalesc klucza!");
        }

        [TestMethod]
        public void AffineAnalyze()
        {
            Action action = () => cypher.A_analyze("?", "!");
            Assert.ThrowsException<Exception>(action);
            //action.Should().Throw<Exception>().WithMessage("ERROR: Nie da sie znalesc klucza!");

        }

    }
}

