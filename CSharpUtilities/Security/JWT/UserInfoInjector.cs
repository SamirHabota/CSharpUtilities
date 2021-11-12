using CSharpUtilities.Manipulations;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUtilities.Security.JWT
{
    /// <summary>
    /// Use to inject users info data to the HttpContext
    /// Usage via filter: [UserInfoInjector]
    /// Define filters order: [UserInfoInjector(Order = 3)]
    /// </summary>
    public class UserInfoInjector : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Get JWT token from the context (it should be injected into the context from the ValidateApiKey filter)
            var jwtToken = HttpContextManipulation.GetFromContext<Dictionary<string, string>>("JWTToken", context.HttpContext);

            // Check if there is an jwt token injected
            if (jwtToken != null && jwtToken.Keys.Count > 0)
            {
                // Inject user id info to context
                context.HttpContext.Items.Add("UserId", jwtToken["sub"]);
            }

            // Proceed with the pipeline
            await next();
        }
    }
}
