using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUtilities.Manipulations
{
    /// <summary>
    /// Defines a model that will be returned after api calls. 
    /// Contains the api reposne, as well as metadata like the success and message statuses
    /// </summary>
    /// <typeparam name="ResponseType">defines the return response model type</typeparam>
    public class ResponseModel<ResponseType>
    {
        public ResponseType Response { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

    public class APIManipulation
    {
        /// <summary>
        /// Use to send dynamic and generic get request to a server
        /// </summary>
        /// <typeparam name="ResponseType">defines the return response model type of the function call</typeparam>
        /// <param name="endpoint" type="string">defines the api endpoint to call</param>
        /// <param name="bearerToken" type="string">defines the authorization bearer header token</param>
        /// <param name="apiKey" type="string">defines the authorization header api key</param>
        /// <returns>ResponseModel<ResponseType> - api call response</returns>
        public static async Task<ResponseModel<ResponseType>> Get<ResponseType>(string endpoint, string bearerToken, string apiKey)
        {
            HttpClient client = new HttpClient();
            if (!string.IsNullOrEmpty(bearerToken)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            if (!string.IsNullOrEmpty(apiKey)) client.DefaultRequestHeaders.Add("Api-Key", apiKey);

            try
            {
                HttpResponseMessage responseData = await client.GetAsync(endpoint);
                if (responseData.IsSuccessStatusCode)
                {
                    ResponseType responseBody = JsonConvert.DeserializeObject<ResponseType>(await responseData.Content.ReadAsStringAsync());
                    return new ResponseModel<ResponseType>() { Response = responseBody, Success = true, Message = "Data fetched successfully", StatusCode = responseData.StatusCode };
                }
                else return new ResponseModel<ResponseType>() { Response = default, Success = false, Message = "Error while fetching the data", StatusCode = responseData.StatusCode };
            }
            catch (Exception ex)
            {
                return new ResponseModel<ResponseType>() { Response = default, Success = false, Message = "Error connecting to the source: " + ex.Message, StatusCode = HttpStatusCode.BadRequest };
            }
        }

        /// <summary>
        /// Use to send a dynamic and generic post request to a server
        /// </summary>
        /// <typeparam name="ResponseType">defines the return response model type of the function call</typeparam>
        /// <typeparam name="BodyType">defines the request body model type</typeparam>
        /// <param name="endpoint" type="string">defines the api endpoint to call</param>
        /// <param name="requestBody" type="string">defines the api request body</param>
        /// <param name="bearerToken" type="string">defines the authorization bearer header token</param>
        /// <param name="apiKey" type="string">defines the authorization header api key</param>
        /// <returns>ResponseModel<ResponseType> - api call response</returns>
        public static async Task<ResponseModel<ResponseType>> Post<ResponseType, BodyType>(string endpoint, BodyType requestBody, string bearerToken, string apiKey)
        {
            HttpClient client = new HttpClient();
            if (!string.IsNullOrEmpty(bearerToken)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            if (!string.IsNullOrEmpty(apiKey)) client.DefaultRequestHeaders.Add("Api-Key", apiKey);

            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(requestBody), null, "application/json");
                HttpResponseMessage responseData = await client.PostAsync(endpoint, content);
                if (responseData.IsSuccessStatusCode)
                {
                    ResponseType responseBody = JsonConvert.DeserializeObject<ResponseType>(await responseData.Content.ReadAsStringAsync());
                    return new ResponseModel<ResponseType>() { Response = responseBody, Success = true, Message = "Data forwarded successfully", StatusCode = responseData.StatusCode };
                }
                else return new ResponseModel<ResponseType>() { Response = default, Success = false, Message = "Error while forwarding the data", StatusCode = responseData.StatusCode };
            }
            catch (Exception ex)
            {
                return new ResponseModel<ResponseType>() { Response = default, Success = false, Message = "Error connecting to the source: " + ex.Message, StatusCode = HttpStatusCode.BadRequest };
            }
        }
    }
}
