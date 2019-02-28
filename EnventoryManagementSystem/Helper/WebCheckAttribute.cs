//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Net.Http;
//using System.Security.Claims;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;

//namespace TicketBookingSystem.Helper
//{
//    public class WebCheckAttribute : AuthorizeAttribute
//    {
//        #region Methods
//        #region AuthorizeCore
//        protected override bool AuthorizeCore(HttpContextBase httpContext)
//        {
//            Console.WriteLine("error occured in web check authorize core");
//            string key = string.Empty;
//            ClaimsPrincipal currentClaimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;

//            Claim nameClaim = currentClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
//            if (nameClaim != null)
//                key = nameClaim.Value;

//            if (!string.IsNullOrEmpty(key))
//            {

//                if (httpContext.Request.Url.AbsolutePath.ToLower().Contains(Constant.BackendPath))
//                {
//                    Claim isAdminClaim = currentClaimsPrincipal.FindFirst("isAdmin");
//                    bool isAdmin = Convert.ToBoolean(isAdminClaim.Value);
//                    if (isAdmin == true)
//                    {
//                        if (httpContext.Request.Url.AbsolutePath.ToLower().Contains("login"))
//                        { httpContext.Response.Redirect($"/{Constant.BackendPath}/dashboard"); }
//                        return true;
//                    }
//                    else
//                    {
//                        if (httpContext.Request.Url.AbsolutePath.ToLower().Contains("login"))
//                        { httpContext.Response.Redirect($"/{Constant.BackendPath}/dashboard"); }
//                        return true;
//                       // httpContext.Response.Redirect($"/{Constant.frontendPath}/dashboard");
//                       // return false;
//                    }
//                }
//                else
//                {

//                    return true;
//                }

//            }
            

//            if (httpContext.Request.Url.AbsolutePath.ToLower().Contains("login") || httpContext.Request.Url.AbsolutePath.ToLower().Contains("forgotpassword") || httpContext.Request.Url.AbsolutePath.ToLower().Contains("changepassword"))
//            {
//                return true;
//            }
//            else
//            {
//                var currentUrl = "";
//                if (!httpContext.Request.Url.AbsolutePath.Equals("/") && !httpContext.Request.Url.AbsolutePath.Equals("/ChangePassword"))
//                {
//                    currentUrl = httpContext.Request.Url.AbsolutePath;
//                    httpContext.Response.Redirect("/login?RedirectUrl=" + currentUrl);
//                }
//                else {
//                    httpContext.Response.Redirect("/login");
                    

//                }
//                return false;
//            }

//        }
//    }



//    #endregion
//    #endregion

//}
