using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUtilities.Manipulations
{
    public class StringManipulation
    {
        private static readonly int _maximumCensorshipLength = 4;
        private static readonly int _minimumCensorshipLength = 2;

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
    }
}
