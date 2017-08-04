using System;
using System.Linq;
using System.Text;

namespace Acme.Generic
{
    public static class TextHelper
    {
        /// <summary>
        /// Returns a copy of this string with the first letter of each word converted to Upper case.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String of words, space separated, first letters in Upper Case.</returns>
        public static string ToCamelCase(this string input)
        {
            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException("Input string cannot be empty");

            string[] words = input.Split(' ');

            foreach (string word in words)
            {
                sb.Append(word.First().ToString().ToUpper() + string.Join("", word.Skip(1)));
                sb.Append(" ");
            }

            string result = sb.ToString();
            return result;
        }

        /// <summary>
        /// Returns a copy of this string with each first letter uppercased, except for the first one
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string input)
        {
            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException("Input string cannot be empty");

            throw new  NotImplementedException();
        }

        /// <summary>
        /// Returns the first 3 letters of this string. Used to shorten the name of a day. Eg: Monday -> Mon.
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static string ToShortDayNotation(this string day)
        {
            string shortDay = day.Substring(0, 3);
            return shortDay;
        }
    }
}
