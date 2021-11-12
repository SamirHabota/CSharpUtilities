using Newtonsoft.Json;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUtilities.Manipulations
{
    /// <summary>
    /// This service is made in collaboration with https://gatewayapi.com/
    /// Register an API key there, before using this service
    /// This service uses two dependencies:
    /// Install deps from NuGet: http://nuget.org/packages/RestSharp
    /// Install deps from NuGet: http://nuget.org/packages/Newtonsoft.Json
    /// </summary>
    public class SmsManipulation
    {
        #region PrivateClassMembers
        //Main url to the gatewayapi API server
        private static readonly string Url = "https://gatewayapi.com/rest/";

        //Endpoint add-on for getting the current client credit
        private static readonly string CreditEndpointAddOn = "me";

        //Insert the API key obtained from gatewayapi.com
        private static readonly string ApiKey = "INSERT_API_KEY_HERE";

        //Define a landline for the phone numbers to be formated to
        private static readonly string PhoneLandline = "+387";

        /// <summary>
        /// Use to inject a landline as the first digits of a phone number
        /// </summary>
        /// <param name="number" type="string">the desired phone number</param>
        /// <returns>string - formatted phone number with the correct landline</returns>
        private static string InjectLandline(string number)
        {
            if (number[0] == '0') number = number.Remove(0, 1).Insert(0, PhoneLandline);
            return number;
        }

        /// <summary>
        /// Use to format a list of numbers to use the correct landline
        /// </summary>
        /// <param name="numbers" type="List<string>">the desired list of phone numbers</param>
        /// <returns>List<string> - a formatted with the correct landline list of numbers</returns>
        private static List<string> GetModifiedLandlineNumbers(List<string> numbers)
        {
            List<string> modifiedLandlineNumbers = new List<string>();
            foreach (string number in numbers) modifiedLandlineNumbers.Add(InjectLandline(number));
            return modifiedLandlineNumbers;
        }

        /// <summary>
        /// The expected format of a phone number for the gatewayapi server
        /// The desired phone number to receive the SMS is populated to the msisdn field
        /// </summary>
        private class FormattedPhoneNumber
        {
            public string msisdn { get; set; }
        }

        /// <summary>
        /// The request body that needs to be sent to the gatewayapi server
        /// The class defines the SMS sender, message, and a list of formatted phone numbers
        /// The formatted phone numbers will receive the message, from the sender
        /// </summary>
        private class SmsRequestBody
        {
            public string sender { get; set; }
            public string message { get; set; }
            public List<FormattedPhoneNumber> recipients { get; set; }
        }

        /// <summary>
        /// Use to format a list of phone numbers to a list of specified gatewayapi formatted phone objects
        /// </summary>
        /// <param name="numbers" type="List<string>">the desired list of phone numbers</param>
        /// <param name="validate" type="bool" default="false">if set to true, will validate and only allow and return correct phone numbers</param>
        /// <returns>List<FormattedPhoneNumber> - a list of specified gatewayapi formatted phone objects</returns>
        private static List<FormattedPhoneNumber> GetFormattedPhoneNumbers (List<string> numbers, bool validate = false)
        {
            List<FormattedPhoneNumber> formattedPhoneNumbers = new List<FormattedPhoneNumber>();
            foreach (string number in numbers)
            {
                if (validate && PhoneManipulation.IsValidPhoneNumber(number)) formattedPhoneNumbers.Add(new FormattedPhoneNumber() { msisdn = number });
                else if (!validate) formattedPhoneNumbers.Add(new FormattedPhoneNumber() { msisdn = number });
            }
            return formattedPhoneNumbers;
        }

        /// <summary>
        /// Creates a RestSharp rest client, using the above specified URL address and API key
        /// </summary>
        /// <returns>RestSharp.RestClient - a RestSharp rest client</returns>
        private static RestSharp.RestClient CreateClient()
        {
            return new RestSharp.RestClient(Url)
            {
                Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(ApiKey, "")
            };
        }

        /// <summary>
        /// Creates a RestSharp POST body request, with all the required SMS data
        /// </summary>
        /// <param name="numbers" type="List<string>">a list of phone numbers to receive the SMS message</param>
        /// <param name="sentFrom" type="string">use to define the SMS sender</param>
        /// <param name="message" type="string">use to define the SMS message</param>
        /// <returns>RestSharp.RestRequest - a RestSharp POST body request</returns>
        private static RestSharp.RestRequest CreatePostRequest(List<string> numbers, string sentFrom, string message)
        {
            RestSharp.RestRequest request = new RestSharp.RestRequest("mtsms", RestSharp.Method.POST);
            request.AddJsonBody(new SmsRequestBody()
            {
                sender = sentFrom,
                message = message,
                recipients = GetFormattedPhoneNumbers(numbers, true)
            });
            return request;
        }
        #endregion

        /// <summary>
        /// Object defined from the gatewayapi server which will be populated with the clients credit data
        /// </summary>
        public class SmsProviderCreditData
        {
            public double Credit { get; set; }
            public string Currency { get; set; }
            public int Id { get; set; }
        }

        /// <summary>
        /// Use to send a SMS message to a list of phone numbers
        /// </summary>
        /// <param name="numbers" type="List<string>">a list of phone numbers to receive the SMS message</param>
        /// <param name="sentFrom" type="string">use to define the SMS sender</param>
        /// <param name="message" type="string">use to define the SMS message</param>
        /// <returns>bool - result of if the SMS messages were sent successfully</returns>
        public static bool SendSms(List<string> numbers, string sentFrom, string message)
        {           
            RestSharp.RestClient client = CreateClient();
            RestSharp.RestRequest request = CreatePostRequest(GetModifiedLandlineNumbers(numbers), sentFrom, message);
            RestSharp.IRestResponse response = client.Execute(request);
            return (int)response.StatusCode == 200;
        }

        /// <summary>
        /// Use to get the clients SMS credit data from the SMS provider
        /// </summary>
        /// <returns>SmsProviderCreditData - the clients SMS credit data from provider</returns>
        public static SmsProviderCreditData GetSmsProviderCreditData()
        {
            RestSharp.RestClient client = CreateClient();
            RestSharp.RestRequest request = new RestSharp.RestRequest(CreditEndpointAddOn, RestSharp.Method.GET);
            return client.Execute<SmsProviderCreditData>(request).Data;
        }
    }
}
