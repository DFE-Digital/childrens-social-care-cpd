using Childrens_Social_Care_CPD.Constants;
using Childrens_Social_Care_CPD.Enums;
using Microsoft.Extensions.Primitives;

namespace Childrens_Social_Care_CPD
{
    public class CheckRequestHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        public static readonly string[] validHeaders = 
        {
            "https://www.develop-child-family-social-work-career.education.gov.uk/",
            "https://develop-child-family-social-work-career.education.gov.uk/",
            "https://www.test.develop-child-family-social-work-career.education.gov.uk/",
            "https://test.develop-child-family-social-work-career.education.gov.uk/",
            "https://www.pre-prod.develop-child-family-social-work-career.education.gov.uk/",
            "https://pre-prod.develop-child-family-social-work-career.education.gov.uk/"
        };

        public CheckRequestHeaderMiddleware(RequestDelegate next) => _next = next;
      
        public async Task InvokeAsync(HttpContext context)
        {
            const string hostHeaderKeyName = SiteConstants.HOSTHEADERKEYNAME;
            context.Request.Headers.TryGetValue(hostHeaderKeyName, out StringValues headerValue);
            if (!validHeaders.Contains<string>(headerValue))
            {
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("Bad request");
            }
        }
    }
}
