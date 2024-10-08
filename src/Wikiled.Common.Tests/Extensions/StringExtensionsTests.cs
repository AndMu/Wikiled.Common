﻿using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.IsTrue(text.ContainWord("like", false));
            ClassicAssert.IsTrue(text.ContainWord("xtray", true));
            ClassicAssert.IsFalse(text.ContainWord("xtray", false));
            ClassicAssert.IsFalse(text.ContainWord("mouse", false));
            ClassicAssert.IsTrue(text.ContainWord("mouse#", true));
        }

        [TestCase("test1)", "test1_")]
        [TestCase("test1 is cool", "test1_is_cool")]
        [TestCase("!2323test1", "_2323test1")]
        public void CreateLetterText(string text, string result)
        {
            ClassicAssert.AreEqual(result, text.CreateLetterText());
        }

        [TestCase("test1)", "test1.")]
        [TestCase("test1 is cool", "test1_is_cool")]
        [TestCase("!2323test1", ".2323test1")]
        public void CreatePureLetterText(string text, string result)
        {
            ClassicAssert.AreEqual(result, text.CreatePureLetterText());
        }

        [TestCase("(", "")]
        [TestCase("test1)", "test1")]
        [TestCase("test)", "test")]
        [TestCase("1test)", "1test")]
        [TestCase("test))", "test")]
        [TestCase("(test))", "(test")]
        public void RemoveLastNonLetters(string text, string result)
        {
            ClassicAssert.AreEqual(result, text.RemoveLastNonLetters());
        }

        [TestCase("(", "")]
        [TestCase("1test", "1test")]
        [TestCase("((!test", "test")]
        [TestCase("((test))", "test))")]
        [TestCase("((test)", "test)")]
        public void RemoveBeginingNonLetters(string text, string result)
        {
            ClassicAssert.AreEqual(result, text.RemoveBeginingNonLetters());
        }

        [TestCase("test", true)]
        [TestCase("1test", true)]
        [TestCase("(test)", true)]
        [TestCase("()", false)]
        [TestCase("23423432", false)]
        public void HasLetters(string text, bool result)
        {
            ClassicAssert.AreEqual(result, text.HasLetters());
        }

        [TestCase("test", false)]
        [TestCase("1test", true)]
        [TestCase("(test)", true)]
        [TestCase("()", true)]
        [TestCase("23423432", true)]
        public void HasNonLetters(string text, bool result)
        {
            ClassicAssert.AreEqual(result, text.HasNonLetters());
        }

        [TestCase("ing", "ing", true)]
        [TestCase("ingx", "int", false)]
        [TestCase("inging", "ing", true)]
        public void IsEnding(string text, string ending, bool result)
        {
            ClassicAssert.AreEqual(result, text.IsEnding(ending));
        }

        [TestCase("My  whole life", " ", true, "My whole life")]
        [TestCase("My  whole life", " ", false, "Mywholelife")]
        [TestCase("My  whole life", " w", false, "Myholelife")]
        [TestCase("My  whole life", " wled", true, "My whole life")]
        public void RemoveCharacters(string text, string remove, bool duplicates, string expected)
        {
            var letters = remove.ToCharArray();
            var result = text.RemoveCharacters(duplicates, letters);
            ClassicAssert.AreEqual(expected, result);
        }

        [TestCase("sudarė valdančiąją koaliciją", "sudare valdanciaja koalicija")]
        public void RemoveDiacritics(string text, string expected)
        {
            var result = text.RemoveDiacritics();
            ClassicAssert.AreEqual(expected, result);
        }

        [TestCase("This is text", "is", "as", "This as text")]
        public void ReplaceString(string text, string replace, string with, string expected)
        {
            var result = text.ReplaceString(replace, with, ReplacementOption.IgnoreCase | ReplacementOption.WholeWord);
            ClassicAssert.AreEqual(expected, result);
        }
    }
}
