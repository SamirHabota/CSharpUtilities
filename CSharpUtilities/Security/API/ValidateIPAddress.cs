using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUtilities.Security.API
{
    /// <summary>
    /// Use to block IP addresses from accessing the API
    /// Usage via filter: [TypeFilter(typeof(ValidateIPAddress), Arguments = new object[] { })]
    /// Define filters order: [TypeFilter(typeof(ValidateIPAddress), Arguments = new object[] { }, Order = 0)]
    /// Setup inside of Startup.cs: services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
    /// This mehtod will store the client IP address to the HttpContext
    /// </summary>
    public class ValidateIPAddress : TypeFilterAttribute
    {
        public ValidateIPAddress(IActionContextAccessor actionContextAccessor) : base(typeof(ValidateIPAddressFilter))
        {
            this.Arguments = new object[] { actionContextAccessor };
        }

        //INJECT SERVICE TO RECEIVE BLACKLISTED IPS
        private class ValidateIPAddressFilter : ActionFilterAttribute
        {
            private readonly IActionContextAccessor _actionContextAccessor;
            //private readonly IDataService _dataService;

            public ValidateIPAddressFilter(IActionContextAccessor actionContextAccessor/*, IDataService dataService*/)
            {
                _actionContextAccessor = actionContextAccessor;
                //_dataService = dataService;
            }

            public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var callerIp = _actionContextAccessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();

                if (!string.IsNullOrWhiteSpace(callerIp))
                {
                    var blacklistedIPs = /*await _dataService.GetAllIPs()*/ new List<string>();

                    if (null != blacklistedIPs && blacklistedIPs.Count != 0)
                    {
                        var isThereIp = blacklistedIPs.FirstOrDefault(i => i == callerIp);
                        if (isThereIp != null)
                        {
                            ForbiddenResponse(context);
                            return;
                        }
                    }

                    //Inject caller IP to request
                    context.HttpContext.Items.Add("CallerIP", callerIp);
                }

                // Proceed with the request pipeline
                var requestResult = await next();
            }

            #region PrivateErrorResponses
            private void ForbiddenResponse(ActionExecutingContext context)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                var jsonString = "{\"error\":\"blIP\"}";
                byte[] data = Encoding.UTF8.GetBytes(jsonString);
                context.HttpContext.Response.Body.WriteAsync(data, 0, data.Length);
            }
            #endregion
        }
    }
}
