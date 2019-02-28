using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Helper
{

    public class SessionExpireAttribute : IAsyncActionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoginUser _loginUser;
        public SessionExpireAttribute(IHttpContextAccessor HttpContextAccessor, ILoginUser LoginUser)
        {
            this._httpContextAccessor = HttpContextAccessor;
            this._loginUser = LoginUser;
        }
        public  async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Execute the rest of the MVC filter pipeline
            var resultContext = await next();

            string userid = _loginUser.GetCurrentUser();            
            var url = GetUrl.GetURL(_httpContextAccessor);
            var logOutUrl = url;
            if (url.IndexOf("?") >= 0)
            {
                url = url.Substring(0, url.IndexOf("?"));
            }
            if (!url.ToLower().Contains("login") && !url.ToLower().Contains("logout") && !url.ToLower().Contains("forgotpassword")
                && !url.ToLower().Contains("changepassword") )
            {
                if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {

                    if (!string.IsNullOrEmpty(_loginUser.GetCurrentUser()))
                    {
                        return;
                    }
                    else
                    {

                        context.Result = new JsonResult(new { status = 302 });
                        //xhr status code 401 to redirect
                        context.HttpContext.Response.StatusCode = 302;
                        _httpContextAccessor.HttpContext.Response.Redirect("/Login/Index?RedirectUrl=" + GetUrl.GetURL(_httpContextAccessor));
                        return;
                    }
                }

                // var session = filterContext.HttpContext.Session;
                if (!string.IsNullOrEmpty(_loginUser.GetCurrentUser()))
                    return;
               
                //Redirect to login page.
                //var redirectTarget = new RouteValueDictionary { { "action", "Index" }, { "controller", "Login" }, { "area", "" } };
                var redirectTarget = new RouteValueDictionary { { "action", "Index" }, { "controller", "/Login" }, { "area", "" } };
                context.Result = new RedirectToRouteResult(redirectTarget);
            }
                       
            else
            {
                
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}