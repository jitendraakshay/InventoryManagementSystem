using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Helper
{
    public static class GetUrl
    {
        //public static string GetURL(IHttpContextAccessor httpcontextaccessor)
        public static string GetURL(IHttpContextAccessor httpcontextaccessor)
        {
            var request = httpcontextaccessor.HttpContext.Request;

            var absoluteUri = string.Concat(
                        request.Scheme,
                        "://",
                        request.Host.ToUriComponent(),
                        request.PathBase.ToUriComponent(),
                        request.Path.ToUriComponent(),
                        request.QueryString.ToUriComponent());
            return absoluteUri;
        }
        public static string GetUrlTokenForResetPassword(IHttpContextAccessor httpContextAccessor)
        {
           var Token= httpContextAccessor.HttpContext.Request.Query["Token"].ToString();
            return Token;
        }
    }
}
