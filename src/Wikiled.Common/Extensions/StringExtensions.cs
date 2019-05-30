using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Wikiled.Common.Extensions
{
    public static class StringExtensions
    {
        public static string AccumulateItems(this IEnumerable<string> itemsSource, string connector)
        {
            var items = itemsSource.ToArray();
            var total = items.Length;
            if (total == 0)
            {
                return string.Empty;
            }

            return total == 1 ? items[0] : items.Aggregate((a, b) => a + connector + b);
        }

        public static bool ContainWord(this string text, string word, bool specialSymbols)
        {
            var words = text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var currentWord in words)
            {
                int foundIndex = currentWord.IndexOf(word, StringComparison.OrdinalIgnoreCase);
                if (foundIndex == -1)
                {
                    continue;
                }

                if (foundIndex == 0)
                {
                    if (currentWord.Length == word.Length)
                    {
                        return true;
                    }

                    for (int i = word.Length; i < currentWord.Length; i++)
                    {
                        if (!char.IsLetterOrDigit(currentWord[i]) &&
                            specialSymbols)
                        {
                            continue;
                        }

                        return false;
                    }
                }
                else
                {
                    for (int i = 0; i < foundIndex; i++)
                    {
                        if (!char.IsLetterOrDigit(currentWord[i]) &&
                            specialSymbols)
                        {
                            continue;
                        }

                        return false;
                    }

                    return true;
                }
            }

            return false;
        }

        public static string CreateLetterText(this string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return word;
            }

            char[] result = word.ToCharArray();
            for (int i = 0; i < result.Length; i++)
            {
                if (!char.IsLetterOrDigit(result[i]))
                {
                    result[i] = '_';
                }
            }

            return new string(result);
        }

        public static string CreatePureLetterText(this string word)
        {
            var letters = word.ToCharArray();
            for (int i = 0; i < word.Length; i++)
            {
                var letter = word[i];
                if (letter == ' ')
                {
                    letters[i] = '_';
                }
                else if (letter != '_' && letter != '-' && !char.IsLetterOrDigit(letter))
                {
                    letters[i] = '.';
                }
            }

            return new string(letters);
        }

        public static DateTime? ExtractDate(this string value)
        {
            if (DateTime.TryParseExact(
                value,
                "MMMM d, yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces,
                out var dateTime))
            {
                return dateTime;
            }

            if (DateTime.TryParseExact(
                value,
                "MMMM dd, yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces,
                out dateTime))
            {
                return dateTime;
            }

            return null;
        }

        public static double? ExtractDouble(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            var sb = new StringBuilder(value.Length);
            foreach (var letter in value)
            {
                if (char.IsLetter(letter) &&
                    sb.Length > 0)
                {
                    break;
                }

                if (char.IsNumber(letter) ||
                    letter == '.' ||
                    letter == ',' ||
                    letter == '-')
                {
                    sb.Append(letter);
                }
            }

            if (!double.TryParse(sb.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var calculated))
            {
                return null;
            }

            return calculated;
        }

        public static int? ExtractInt(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            var sb = new StringBuilder(value.Length);
            foreach (var letter in value)
            {
                if (char.IsLetter(letter) &&
                    sb.Length > 0)
                {
                    break;
                }

                if (char.IsNumber(letter))
                {
                    sb.Append(letter);
                }
            }

            if (!int.TryParse(sb.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var calculated))
            {
                return null;
            }

            return calculated;
        }

        public static byte[] GetBytes(this string text)
        {
            var bytes = new byte[text.Length * sizeof(char)];
            Buffer.BlockCopy(text.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(this byte[] bytes)
        {
            var chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static bool HasLetters(this string word)
        {
            return word.Any(char.IsLetter);
        }

        public static bool HasNonLetters(this string word)
        {
            return word.Any(letter => !char.IsLetter(letter));
        }

        public static bool IsEnding(this string word, string end)
        {
            return end.Length <= word.Length && word.EndsWith(end, StringComparison.CurrentCultureIgnoreCase);
        }

        public static string RemoveBeginingNonLetters(this string word)
        {
            int pointer = 0;
            for (int i = 0; i < word.Length; i++)
            {
                if (char.IsLetterOrDigit(word[i]))
                {
                    break;
                }

                pointer++;
            }

            return word.Substring(pointer, word.Length - pointer);
        }

        public static string RemoveDiacritics(this string text)
        {
            return string.Concat(text.Normalize(NormalizationForm.FormD)
                         .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark))
                         .Normalize(NormalizationForm.FormC);
        }

        public static string RemoveLastNonLetters(this string word)
        {
            int pointer = word.Length;
            for (int i = word.Length - 1; i >= 0; i--)
            {
                if (char.IsLetterOrDigit(word[i]))
                {
                    break;
                }

                pointer--;
            }

            return word.Substring(0, pointer);
        }

        public static string ReplaceString(this string str, string oldValue, string newValue, ReplacementOption options)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(str));
            }

            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(oldValue));
            }

            if (string.IsNullOrEmpty(newValue))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(newValue));
            }

            var sb = new StringBuilder();
            int previousIndex = 0;
            var comparison = StringComparison.Ordinal;
            if ((options & ReplacementOption.IgnoreCase) > 0)
            {
                comparison = StringComparison.OrdinalIgnoreCase;
            }

            bool whole = (options & ReplacementOption.WholeWord) > 0;
            int index = str.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                bool add = true;
                if (whole)
                {
                    if (index > 0 &&
                        char.IsLetterOrDigit(str[index - 1]))
                    {
                        add = false;
                    }

                    int nextIndex = oldValue.Length + index;
                    if (nextIndex < str.Length &&
                        char.IsLetterOrDigit(str[nextIndex]))
                    {
                        add = false;
                    }
                }

                if (add)
                {
                    sb.Append(str.Substring(previousIndex, index - previousIndex));
                    sb.Append(newValue);
                }
                else
                {
                    sb.Append(str.Substring(previousIndex, index - previousIndex + oldValue.Length));
                }

                index += oldValue.Length;
                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }

            sb.Append(str.Substring(previousIndex));
            return sb.ToString();
        }

        public static string ReplaceStringWithRange(
            this string str,
            string oldValueBegin,
            string oldValueEnd,
            string newValue)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(str));
            }

            if (string.IsNullOrEmpty(oldValueBegin))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(oldValueBegin));
            }

            if (string.IsNullOrEmpty(oldValueEnd))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(oldValueEnd));
            }

            if (string.IsNullOrEmpty(newValue))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(newValue));
            }

            string pattern = $@"{RemoveSpecialSymbols(oldValueBegin)}.*?{RemoveSpecialSymbols(oldValueEnd)}";
            return Regex.Replace(str, pattern, newValue, RegexOptions.IgnoreCase);
        }

        private static string RemoveSpecialSymbols(this string text)
        {
            text = text.Replace(@"\", @"\\");
            text = text.Replace("[", @"\[");
            text = text.Replace("]", @"\]");
            text = text.Replace("(", @"\(");
            text = text.Replace(")", @"\)");
            text = text.Replace(".", @"\.");
            text = text.Replace(@"+", @"\+");
            text = text.Replace(@"*", @"\*");
            return text;
        }
    }
}
