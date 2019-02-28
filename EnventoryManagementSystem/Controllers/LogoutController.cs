using DomainEntities;
using DomainInterface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using System;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace InventoryManagementSystem.Controllers
{

    public class LogoutController : Controller
    {
        private readonly ISettingsRepo settingsRepo;

        public LogoutController(ISettingsRepo _settingsRepo)
        {
            this.settingsRepo = _settingsRepo;
        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            //Session.Abandon();
            //string[] myCookies = Request.Cookies.AllKeys;
            try
            {
                //Settings settings = settingsRepo.GetSettingsBySettingsId("1016");
                //var settingValue = settings.SettingsValue;
                //if (settingValue == "1" && Request.Cookies["UniqueKey"] == null)
                //{
                //    Settings settings2 = settingsRepo.GetSettingsBySettingsId("1015");
                //    TempData["SettingValue"] = settings2;
                //    return RedirectToAction("PageUnderConstruction", "Login"/*, new { @settingValue = settings2.SettingsValue }*/);
                //}

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                //foreach (string cookie in myCookies)
                //{
                //    Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                //}
                //HttpCookie myCookie = new HttpCookie(Constant.TokenCookieName);
                //myCookie.Expires = DateTime.Now.AddDays(-1d);
                //Response.Cookies.Add(myCookie);
                //Request.GetOwinContext().Authentication.SignOut();
                //var Identity = new ClaimsIdentity(User.Identity);
                //Identity.RemoveClaim(Identity.FindFirst(ClaimTypes.NameIdentifier));


                return RedirectToAction("Index", "Login", new { isFromLogOut = "true" });
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}