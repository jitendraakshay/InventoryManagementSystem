using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketBookingSystem;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Controllers
{
    public class GrantUser
    {
        [Required(ErrorMessage = "Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
        public string Key { get; set; }
        public string SessionKey { get; set; }
        public string RedirectUrl { get; set; }
        public string IPAddress { get; set; }
        public bool RememberMe { get; set; }
    }
    public class GenerateTokenController : Controller
    {
        [Route("api/GenerateToken")]
        
        public dynamic Get([FromBody]GrantUser value)
        {
            //HttpContext context = HttpContext.Request.Cookies;

            string userName = value.UserName;
            string password = value.Password;
            DomainRepository.AuthorizeUserRepo oAuthorizeUser = new DomainRepository.AuthorizeUserRepo(null);
            if (oAuthorizeUser.CheckUser(userName, password).Result)
            {
                string token = Guid.NewGuid().ToString();
                oAuthorizeUser.SaveToken(token, userName);
                //bool isAdmin = oAuthorizeUser.CheckIfAdmin(userName);
                //HttpCookie cookie = new HttpCookie(Constant.TokenCookieName, token);
                //HttpCookie cookieLoggedInUser = new HttpCookie(Constant.UserNameCookie, Crypto.Encrypt(userName));
                //HttpCookie cookieIsAdmin = new HttpCookie(Constant.isAdminCookie, Crypto.Encrypt(isAdmin.ToString()));
                //HttpContext.Current.Response.Cookies.Add(cookie);
                //HttpContext.Current.Response.Cookies.Add(cookieLoggedInUser);

                //HttpContext.Current.Response.Cookies.Add(cookieIsAdmin);
                return JsonReturn.True("token", token);
            }
            else
            {
                return JsonReturn.False("No valid User");
            }
        }
    }
}
