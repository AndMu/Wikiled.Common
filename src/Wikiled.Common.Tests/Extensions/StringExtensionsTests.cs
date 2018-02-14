using NUnit.Framework;
using Wikiled.Common.Extensions;

namespace Wikiled.Common.Tests.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void ContainWord()
        {
            const string text = "I like this book and #xtray with mouse#";
            Assert.IsTrue(text.ContainWord("like", false));
            Assert.IsTrue(text.ContainWord("xtray", true));
            Assert.IsFalse(text.ContainWord("xtray", false));
            Assert.IsFalse(text.ContainWord("mouse", false));
            Assert.IsTrue(text.ContainWord("mouse#", true));
        }

        [TestCase("test1)", "test1_")]
        [TestCase("test1 is cool", "test1_is_cool")]
        [TestCase("!2323test1", "_2323test1")]
        public void CreateLetterText(string text, string result)
        {
            Assert.AreEqual(result, text.CreateLetterText());
        }

        [TestCase("test1)", "test1.")]
        [TestCase("test1 is cool", "test1_is_cool")]
        [TestCase("!2323test1", ".2323test1")]
        public void CreatePureLetterText(string text, string result)
        {
            Assert.AreEqual(result, text.CreatePureLetterText());
        }

        [TestCase("(", "")]
        [TestCase("test1)", "test1")]
        [TestCase("test)", "test")]
        [TestCase("1test)", "1test")]
        [TestCase("test))", "test")]
        [TestCase("(test))", "(test")]
        public void RemoveLastNonLetters(string text, string result)
        {
            Assert.AreEqual(result, text.RemoveLastNonLetters());
        }

        [TestCase("(", "")]
        [TestCase("1test", "1test")]
        [TestCase("((!test", "test")]
        [TestCase("((test))", "test))")]
        [TestCase("((test)", "test)")]
        public void RemoveBeginingNonLetters(string text, string result)
        {
            Assert.AreEqual(result, text.RemoveBeginingNonLetters());
        }

        [TestCase("test", true)]
        [TestCase("1test", true)]
        [TestCase("(test)", true)]
        [TestCase("()", false)]
        [TestCase("23423432", false)]
        public void HasLetters(string text, bool result)
        {
            Assert.AreEqual(result, text.HasLetters());
        }

        [TestCase("test", false)]
        [TestCase("1test", true)]
        [TestCase("(test)", true)]
        [TestCase("()", true)]
        [TestCase("23423432", true)]
        public void HasNonLetters(string text, bool result)
        {
            Assert.AreEqual(result, text.HasNonLetters());
        }

        [TestCase("ing", "ing", true)]
        [TestCase("ingx", "int", false)]
        [TestCase("inging", "ing", true)]
        public void IsEnding(string text, string ending, bool result)
        {
            Assert.AreEqual(result, text.IsEnding(ending));
        }
    }
}
