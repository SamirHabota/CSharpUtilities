using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUtilities.Manipulations
{
    public class HttpContextManipulation
    {
        /// <summary>
        /// Use to get specific item from the HttpContext
        /// Usage inside of controller endpoint: HttpContextManipulation.GetFromContext<string>("key", Request.HttpContext);
        /// </summary>
        public static T GetFromContext<T>(string key, HttpContext context)
        {
            if (context.Items.Keys.Contains(key)) return (T)context.Items[key];
            return default(T);
        }
    }
}
