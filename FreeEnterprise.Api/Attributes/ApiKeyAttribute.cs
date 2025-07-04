﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace FreeEnterprise.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string APIKEYNAME = "Api-Key";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = "No provided Api-Key header"
                };
                return;
            }

            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var apiKey = appSettings.GetValue<string>(APIKEYNAME);

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 500,
                    Content = "Server Error: No Api Key Configured"
                };
                return;
            }

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = "Api Key is not valid"
                };
                return;
            }

            await next();
        }
    }
}
