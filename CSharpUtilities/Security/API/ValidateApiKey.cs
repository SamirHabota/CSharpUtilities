using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace CSharpUtilities.Security.API
{
    /// <summary>
    /// Use to validate an API key before allowing any connections to endpoints.
    /// Usage via filter: [TypeFilter(typeof(ValidateApiKey), Arguments = new object[] { })]
    /// Define filters order: [TypeFilter(typeof(ValidateApiKey), Arguments = new object[] { }, Order = 0)]
    /// Setup inside of Startup.cs: services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
    /// API key inside the header should be passed in as the key-value pair: Api-Key
    /// </summary>
    public class ValidateApiKey : TypeFilterAttribute
    {
        public ValidateApiKey(IActionContextAccessor actionContextAccessor) : base(typeof(ValidateIPAddressFilter))
        {
            this.Arguments = new object[] { actionContextAccessor };
        }

        private class ValidateIPAddressFilter : ActionFilterAttribute
        {
            private readonly IConfiguration _configuration;

            public ValidateIPAddressFilter(IConfiguration configuration)
            {
                _configuration = configuration;
            }

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
                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    BlankKey(context);
                    return;
                }
                // Validate Api-Key
                var storedApiKey = "GET_FROM_CONFIG"; /*_configuration.GetSection("ConfigSettings").GetSection("Api-Key").Value.ToString();*/

                if (apiKey != storedApiKey)
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
}
