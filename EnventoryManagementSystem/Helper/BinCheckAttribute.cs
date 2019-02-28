//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Security.Claims;
//using System.Threading;
//using System.Web;
//using System.Web.Http.Controllers;
//using System.Web.Http.Filters;

//namespace TicketBookingSystem.Helper
//{
//    [AttributeUsage(AttributeTargets.Class |
//                    AttributeTargets.Constructor |
//                    AttributeTargets.Method
//       , AllowMultiple = true)
//]
//    public class BinCheckAttribute : ActionFilterAttribute
//    {
//        #region Properties
//        public string Type { get; set; }
//        #endregion
//        #region Methods

//        public override void OnActionExecuting(HttpActionContext actionContext)
//        {
//            HttpContext httpContext = HttpContext.Current;
//            ClaimsPrincipal currentClaimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
//            Claim nameClaim = currentClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
//            string key = String.Empty;
//            if (nameClaim != null)
//            {
//                key = nameClaim.Value;
//            }
//            if (!string.IsNullOrEmpty(key))
//            {
//                if (actionContext.Request.RequestUri.AbsolutePath.ToLower().Contains("adminapi"))
//                {
//                    Claim isAdminClaim = currentClaimsPrincipal.FindFirst("isAdmin");
//                    bool isAdmin = Convert.ToBoolean(isAdminClaim.Value);

//                    if (isAdmin == false)
//                    {
//                        actionContext.Response = new HttpResponseMessage()
//                        {
//                            StatusCode = System.Net.HttpStatusCode.Unauthorized,
//                            Content = new StringContent(JsonReturn.UnAuthorized())

//                        };
//                        actionContext.Response.Headers.Add("ContentType", " application/json; charset=utf-8");
//                        base.OnActionExecuting(actionContext);
//                    }
//                    else
//                    {
                      
//                    }
//                }
//                else if (actionContext.Request.RequestUri.AbsolutePath.ToLower().Contains("clientapi")) {


//                }

//            }
//            else
//                {
//                    actionContext.Response = new HttpResponseMessage()
//                    {
//                        StatusCode = System.Net.HttpStatusCode.Unauthorized,
//                        Content = new StringContent(JsonReturn.UnAuthorized())

//                        };
//                        actionContext.Response.Headers.Add("ContentType", " application/json; charset=utf-8");
//                        base.OnActionExecuting(actionContext);
//                    }

//        }
//        #endregion
//    }
//}