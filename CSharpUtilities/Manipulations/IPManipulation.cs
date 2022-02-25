using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUtilities.Manipulations
{
    /// <summary>
    /// This service is made in collaboration with https://ipfind.com
    /// Register an API key there, before using this service
    /// </summary>
    public class IPManipulation
    {
        #region PrivateClassMembers
        //Main url to the ipfind API server
        private static readonly string Url = "https://api.ipfind.com";

        //Insert the API key obtained from ipfind.com
        private static readonly string ApiKey = "INSERT_API_KEY_HERE";

        /// <summary>
        /// Use to create and fill in ipfind.com API endpoint for fetching ip details
        /// </summary>
        /// <param name="ipAddress" type="string">desired ip address</param>
        /// <returns>string - created and filled in endpoint</returns>
        private static string CreateApiEndpoint(string ipAddress)
        {
            return Url + "?auth=" + ApiKey + "&ip=" + ipAddress;
        }
        #endregion

        /// <summary>
        /// All the ip details returned from the ipfind.com API server
        /// </summary>
        public class IpDetails
        {
            public string Country { get; set; }

            public string Region { get; set; }

            public string Continent { get; set; }

            public string City { get; set; }

            public string Timezone { get; set; }

            public string Longitude { get; set; }

            public string Latitude { get; set; }
        }

        /// <summary>
        /// Use to receive in depth details about an ip address
        /// </summary>
        /// <param name="ipAddress" type="string">desired ip address</param>
        /// <returns>IpDetails - all ip address details</returns>
        public static async Task<IpDetails> GetIpDetails(string ipAddress)
        {
            var apiResponse = await APIManipulation.Get<IpDetails>(CreateApiEndpoint(ipAddress), string.Empty, string.Empty);
            if (apiResponse.Success) return apiResponse.Response;
            else return null;
        }


    }
}
