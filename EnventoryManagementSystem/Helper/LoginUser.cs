using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace InventoryManagementSystem.Helper
{
    public class LoginUser:ILoginUser
    {        
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public string GetCurrentUser()
        {

            try
            {
              
                 return _httpContextAccessor.HttpContext.User.Identity.Name;

                //var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                //if (identity != null)
                //{
                //    // Get the claims values
                //    var name = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                //                       .Select(c => c.Value).SingleOrDefault();
                //    var sid = identity.Claims.Where(c => c.Type == ClaimTypes.Sid)
                //                       .Select(c => c.Value).SingleOrDefault();
                //}
                //return name;
                //return nameClaim.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }

    }
    public interface ILoginUser
    {
        string GetCurrentUser();
    }
}