using System;
using FluentAssertions;
using NUnit.Framework;
using PzaleszczykLib;

namespace PzaleszczykUnitTest
{

    public class UnitTestException
    {
        public Cypher cypher;

        [SetUp]
        public void Startup()
        {
            cypher = new Cypher();
        }

        [TearDown]
        public void Cleanup()
        {
            cypher = null;
        }

        [Test]
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

        [Test]
        public void AffineEncryptWrongKeyFluent()
        {
            Action action = () => cypher.A_crypt("exception", 13, 13);
            action.Should().Throw<Exception>().WithMessage("ERROR: Niepoprawny format klucza");
        }

        [Test]
        public void CaesarAnalyzeFluent()
        {
            Action action = () => cypher.C_analyze("?","!");
            action.Should().Throw<Exception>().WithMessage("ERROR: Nie da sie znalesc klucza!");
        }

        [Test]
        public void AffineAnalyze()
        {
            Assert.Throws<Exception>(
                delegate { cypher.A_analyze("?", "!"); });
        }

    }
}

