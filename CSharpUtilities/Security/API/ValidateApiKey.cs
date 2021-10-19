using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CSharpUtilities.Security.API
{
    /// <summary>
    /// Use to validate an API key before allowing any connections to endpoints.
    /// Usage via filter: [ValidateApiKey] 
    /// Define filters order: [ValidateApiKey(Order = 0)]
    /// Define the apps API key below, inside the validation section
    /// API key inside the header should be passed in as the key-value pair: Api-Key
    /// </summary>
    public class ValidateApiKey : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Check if the request contains header with valid API key
            if (!context.HttpContext.Request.Headers.ContainsKey("Api-Key"))
            {
                HeaderMissing(context);
                return;
            }

            // Grab Api-Key header value
            var apiKey = context.HttpContext.Request.Headers["Api-Key"];
            if (String.IsNullOrWhiteSpace(apiKey))
            {
                BlankKey(context);
                return;
            }

            // Validate Api-Key
            if (apiKey !="APP_API_KEY_FROM_CONFIGURATION")
            {
                InvalidKey(context);
                return;
            }

            // Proceed with the request pipeline
            var requestResult = await next();
        }

        #region PrivateErrorResponses
        private void HeaderMissing(ActionExecutingContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            var jsonString = "{\"error\":\"AKHmss\"}";
            byte[] data = Encoding.UTF8.GetBytes(jsonString);
            context.HttpContext.Response.Body.WriteAsync(data, 0, data.Length);
        }

        private void BlankKey(ActionExecutingContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            var jsonString = "{\"error\":\"AKblnk\"}";
            byte[] data = Encoding.UTF8.GetBytes(jsonString);
            context.HttpContext.Response.Body.WriteAsync(data, 0, data.Length);
        }

        private void InvalidKey(ActionExecutingContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            var jsonString = "{\"error\":\"AKmsmtch\"}";
            byte[] data = Encoding.UTF8.GetBytes(jsonString);
            context.HttpContext.Response.Body.WriteAsync(data, 0, data.Length);
        }
        #endregion
    }
}
