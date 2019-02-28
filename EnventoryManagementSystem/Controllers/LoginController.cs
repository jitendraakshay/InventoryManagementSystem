using DomainEntities;
using DomainInterface;
using InventoryManagementSystem.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using InventoryManagementSystem.Helper;

namespace InventoryManagementSystem.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        
        private readonly IAuthorizeUserRepo authorizeUserRepo;       
        private readonly IUserRepo iUserRepo;       
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoginUser _loginUser;


        public LoginController(IAuthorizeUserRepo authorizeUser, IUserRepo iUserRepo, ILoginUser loginUser, IHttpContextAccessor HttpContextAccessor)
        {
            this.authorizeUserRepo = authorizeUser;            
            this.iUserRepo = iUserRepo;           
            this._loginUser = loginUser;
            this._httpContextAccessor = HttpContextAccessor;

        }
        [HttpGet]
        public ActionResult Index(string RedirectUrl = "", string sessionKey = "", bool isFromLogOut = false)
        {            
            if (isFromLogOut == false)
            {
                               
                if (!string.IsNullOrEmpty(_loginUser.GetCurrentUser()))
                {
                    DomainRepository.AuthorizeUserRepo oAuthorizeUser = new DomainRepository.AuthorizeUserRepo(null);
                    return RedirectToAction("Index", "DashBoard", new { area = "Admin" });
                }
                else
                {
                    RedirectUrl = GetUrl.GetURL(_httpContextAccessor);                    
                    if (RedirectUrl.IndexOf("?") >= 0 && string.IsNullOrEmpty(_loginUser.GetCurrentUser()))
                    {
                        ViewBag.RedirectURL = RedirectUrl;
                        if (!RedirectUrl.Contains("logout"))
                        {
                            TempData["loginExpireCheck"] = true;
                        }
                        else
                        {
                            TempData["loginExpireCheck"] = false;
                        }
                    }

                    else
                    {
                        ViewBag.RedirectURL = "";
                        TempData["loginExpireCheck"] = false;
                    }

                }
            }
            return View();
        }

        

        [HttpPost]       
        public async Task<ActionResult> Index(GrantUser value)
        {

            string userName = value.UserName;
            string password = Crypto.OneWayEncryter(value.Password);
            //ModelState.Clear();
            if (ModelState.IsValid)
            {
                if (TryValidateModel(value))
                {

                    ReturnType returnData = authorizeUserRepo.CheckUser(userName, password);
                    if (returnData != null)
                    {
                        if (returnData.Result == true)
                        {
                            //authorizeUserRepo.SetLoginInfor(userName, value.IPAddress);
                        }
                        bool isSuperUser = returnData.isSuperUser;

                        bool allowToLogIn = false;
                        string profileImage = "";//default user image path

                        if (returnData.Result)
                        {
                            profileImage = returnData.ProfileImage;
                            allowToLogIn = true;
                        }
                        else
                        {
                            allowToLogIn = false;
                            ModelState.AddModelError("", "Invalid username or password");
                        }

                        if (allowToLogIn)
                        {
                            //LoadClaimData(isAdmin, userName, isSuperUser, profileImage);//, isLDAPUser);
                            if (Url.IsLocalUrl(value.RedirectUrl))
                            {
                                return Redirect(value.RedirectUrl);
                            }
                            else
                            {

                                //Settings settings = settingsRepo.GetSettingsBySettingsId("1002");
                                double cookieExpireTime = Convert.ToDouble(30);
                                var claims = new List<Claim>();
                                claims.Add(new Claim(ClaimTypes.Name, userName, ClaimValueTypes.String));
                                claims.Add(new Claim("isAdmin", true.ToString()));
                                var userIdentity = new ClaimsIdentity("isAdmin");
                                userIdentity.AddClaims(claims);
                                var userPrincipal = new ClaimsPrincipal(userIdentity);
                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal,
                                    new AuthenticationProperties
                                    {
                                        ExpiresUtc = DateTime.UtcNow.AddMinutes(cookieExpireTime),
                                        IsPersistent = true,
                                        AllowRefresh = true,


                                    });

                                return RedirectToAction("Index", "DashBoard", new { area = "Admin" });

                            }
                        }
                        return View();
                    }

                    else
                    {

                        //UserFailedLogin failedLoginParam = new UserFailedLogin();
                        //failedLoginParam.SessionID = value.SessionKey;
                        //failedLoginParam.userName = userName;
                        //failedLoginParam.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                        //ReturnType checkActiveUser = authorizeUserRepo.CheckActiveUser(userName);
                        //if (checkActiveUser.Result == true)//check user is active then only count failed login
                        //{
                        //    ReturnType failedMessage = authorizeUserRepo.ManageFailedLoginAttempt(userName, failedLoginParam.IPAddress);
                        //    ModelState.AddModelError("", failedMessage.Message);
                        //}
                        //else
                        //{
                        //userFailedLoginRepo.UserFailedLoginSave(failedLoginParam);
                        //ModelState.AddModelError("", "Invalid username or password");
                        //if (checkActiveUser.Message == "" || checkActiveUser.Message == null)
                        //{
                        ModelState.AddModelError("", "Invalid username or password");
                        //}
                        //else
                        //{
                        //    ModelState.AddModelError("", checkActiveUser.Message);
                        //}

                        //}



                        return View();

                    }

                }
                else
                {
                    return View();
                }

            }
            else
            {
               
                return View();
            }
    
        }       

        public void LoadUniqueKeyCookie(string uniqueKey)
        {
            if (HttpContext.Request.Cookies["UniqueKey"] != null)
            {
                Response.Cookies.Delete("UniqueKey");              
            }
            else
            {                
             
                Response.Cookies.Append("UniqueKey", uniqueKey);
            }
            
        }

        public void ClearUniqueKeyCookie()
        {
            if (HttpContext.Request.Cookies["UniqueKey"] != null)
                Response.Cookies.Delete("UniqueKey");            
        }
    }
}