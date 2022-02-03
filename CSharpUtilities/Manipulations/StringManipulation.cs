using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUtilities.Manipulations
{
    public class StringManipulation
    {

        #region PrivateClassMembers
        private static readonly int _maximumCensorshipLength = 4;
        private static readonly int _minimumCensorshipLength = 2;
        private static List<char> _charactersWithHandle = new List<char>() { 'Č', 'č', 'Ć', 'ć', 'Š', 'š', 'Ž', 'ž', 'Đ', 'đ' };

        /// <summary>
        /// Tries to convert and return a character without its handle.
        /// Returns 'x' if original character has no handle
        /// </summary>
        /// <param name="character" type="char">desired character for handle removal</param>
        /// <returns>string - character without handle in string format</returns>
        private static string GetCharaceterWihoutHandle(char character)
        {
            return character switch
            {
                'Č' => "C",
                'č' => "c",
                'Ć' => "C",
                'ć' => "c",
                'Š' => "S",
                'š' => "s",
                'Ž' => "Z",
                'ž' => "z",
                'Đ' => "Dj",
                'đ' => "dj",
                _ => "x",
            };
        }

        /// <summary>
        /// This function is based on the "Levenshtein Distance" algorithm.
        /// The algorithm calculates the minimum number of single-character edits (insertions, deletions or substitutions) required to change one word into the other.
        /// Theory: https://en.wikipedia.org/wiki/Levenshtein_distance
        /// </summary>
        /// <param name="source" type="string">the source string, representing the start of the transformation count</param>
        /// <param name="target" type="string">the target string, representing the end of the transformation count</param>
        /// <returns>int - number of steps needed to transform the source string into the target string</returns>
        private static int ComputeStringDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++);
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++);

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }
        #endregion

        /// <summary>
        /// Will censor a string based on a set of censorship rules
        /// The censorship rules can be defined above, inside the private attributes
        /// </summary>
        /// <param name="originalString" type="string">string that needs to be censored</param>
        /// <param name="setSpaces" type="bool">defines if the censored string will have spaces</param>
        /// <returns>string - censored string</returns>
        public static string GetCensoredString(string originalString, bool setSpaces = false)
        {
            var censoredString = "*****";
            if (originalString.Length >= _maximumCensorshipLength)
            {
                int showOffset = (originalString.Length >= _maximumCensorshipLength) ? _maximumCensorshipLength : _minimumCensorshipLength;
                StringBuilder builder = new StringBuilder(originalString);
                string filler = setSpaces ? "* " : "*";
                for (var i = 0; i < originalString.Length - showOffset; i++) filler += "*";
                builder.Remove(showOffset, originalString.Length - showOffset);
                builder.Insert(showOffset, filler);
                censoredString = builder.ToString();
            }
            return censoredString;
        }

        /// <summary>
        /// Use to remove trailing characters from a string
        /// </summary>
        /// <param name="statement" type="string">the statement string from which the trailing characters need to be removed</param>
        /// <param name="trailingAmount" type="int">the amount of trailing characters from the back that need to be removed</param>
        /// <returns>string - the statement without any trailing characters</returns>
        public static string RemoveTrailingCharacters(string statement, int trailingAmount = 2)
        {
            if (!string.IsNullOrWhiteSpace(statement) && statement.Length >= trailingAmount) return statement.Remove(statement.Length - trailingAmount);
            else return statement;
        }

        /// <summary>
        /// Returns a string with all its handle characters removed
        /// </summary>
        /// <param name="originalString" type="string">the desired string that needs handle characters removal</param>
        /// <returns>stirng - string without any handle characters</returns>
        public static string GetStringWihoutHandleCharacters(string originalString)
        {
            string stringWihoutHandleCharacters = string.Empty;
            for (var i = 0; i < originalString.Length; i++)
            {
                if (_charactersWithHandle.Contains(originalString[i])) stringWihoutHandleCharacters += GetCharaceterWihoutHandle(originalString[i]);
                else stringWihoutHandleCharacters += originalString[i];
            }
            return stringWihoutHandleCharacters;
        }

        /// <summary>
        /// Returns a decimal calculated number that represents how similar the source and target strings forwarded as parameters are
        /// </summary>
        /// <param name="source" type="string">the source string, representing the start of the similarity calculation</param>
        /// <param name="target" type="string">the target string, representing the end of the similarity calculation</param>
        /// <returns>double - a precise decimal number from 0 to 1, representing the two strings similarity, 1 meaning identical</returns>
        public static double CalculateStringSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            return (1.0 - ((double)ComputeStringDistance(source, target) / (double)Math.Max(source.Length, target.Length)));
        }
    }
}
