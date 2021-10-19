using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CSharpUtilities.Manipulations
{
    public class PhoneManipulation
    {
        private static readonly int _digitThreshold = 7;
        private static readonly string _validPhoneNumberRegexPattern = @"\(?\d{3}\)?-? *\d{3}-? *-?\d{3}";

        /// <summary>
        /// Returns the information of: are two phone numbers the same base on a certain amount of threshold digits.
        /// The digit threshold can be defined above, inside the private attribute.
        /// </summary>
        /// <param name="firstPhoneNumber" type="string"></param>
        /// <param name="secondPhoneNumber" type="string"></param>
        /// <returns>bool - are the phone numbers the same</returns>
        public static bool AreSamePhoneNumbers(string firstPhoneNumber, string secondPhoneNumber)
        {
            return
            firstPhoneNumber.Length >= _digitThreshold && secondPhoneNumber.Length >= _digitThreshold &&
            firstPhoneNumber.Substring(firstPhoneNumber.Length - _digitThreshold) == secondPhoneNumber.Substring(secondPhoneNumber.Length - _digitThreshold);
        }

        /// <summary>
        /// Will censor a phone number for the set digit threshold.
        /// The digit threshold can be defined above, inside the private attribute.
        /// </summary>
        /// <param name="originalPhoneNumber" type="string">the original phone number that needs to be censored</param>
        /// <returns>string - censored phone number</returns>
        public static string GetCensoredPhoneNumber(string originalPhoneNumber)
        {
            string censoredPhone = "*****";
            if (originalPhoneNumber.Length >= _digitThreshold)
            {
                StringBuilder builder = new StringBuilder(originalPhoneNumber);
                string filler = "";
                for (var i = 0; i < originalPhoneNumber.Length - _digitThreshold; i++) filler += "*";
                builder.Remove(_digitThreshold, originalPhoneNumber.Length - _digitThreshold);
                builder.Insert(_digitThreshold, filler);
                censoredPhone = builder.ToString();
            }
            return censoredPhone;
        }

        /// <summary>
        /// Will check if the passed in phone number has a valid format, based on a regular expression.
        /// The regular expression can be defined above, inside the private attribute.
        /// </summary>
        /// <param name="phoneNumber" type="string">phone number that needs to be checked</param>
        /// <returns>bool - if valid phone number</returns>
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (String.IsNullOrEmpty(phoneNumber)) return false;
            return new Regex(_validPhoneNumberRegexPattern).IsMatch(phoneNumber);
        }
    }
}
