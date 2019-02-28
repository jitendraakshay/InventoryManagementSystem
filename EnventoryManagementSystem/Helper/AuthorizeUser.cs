using DomainEntities;
using DomainRepository;
using DomainInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Helper
{

   
    public class AuthorizeUser : IAuthorizationHandler
    {
        public string Controls { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoginUser _loginUser;
        private readonly IMenuRepository _menuRepository;


        public AuthorizeUser(IHttpContextAccessor HttpContextAccessor, ILoginUser LoginUser, IMenuRepository MenuRepository)
        {
            this._httpContextAccessor = HttpContextAccessor;
            this._loginUser = LoginUser;
            this._menuRepository = MenuRepository;
        }

        //public Task OnAuthorization(AuthorizationHandlerContext context)
        //{
        //    try

        //    {
        //        var url = GetUrl.GetURL(_httpContextAccessor);
        //        if (url.IndexOf("?") >= 0)
        //        {
        //            url = url.Substring(0, url.IndexOf("?"));
        //        }

        //        string isAdminClaimValue = "false";
        //        if (!string.IsNullOrEmpty(_loginUser.GetCurrentUser()))
        //        {
        //            isAdminClaimValue = _loginUser.GetCurrentUser();
        //        }

        //        bool isAdmin = false;

        //        var controller = url.ToLower().Split('/').ToList();
        //        //IEnumerable<Menu> allowedMenus = MenuRepository.GetMenuAccessBasedOnRole(_loginUser.GetCurrentUser());
        //        IEnumerable<Menu> allowedMenus = MenuRepository.GetMenuAccessBasedOnRole(_loginUser.GetCurrentUser());
        //        if (controller.Contains("admin"))
        //        {

        //            if (!string.IsNullOrEmpty(isAdminClaimValue))
        //            {
        //                isAdmin = Convert.ToBoolean(isAdminClaimValue);
        //            }
        //            if (isAdmin)
        //            {
        //                allowedMenus = allowedMenus.Where(x => x.isAdmin == true).ToList();
        //            }
        //            else
        //            {
        //                allowedMenus = null;
        //            }
        //        }
        //        else
        //        {
        //            allowedMenus = allowedMenus.Where(x => x.isAdmin == false).ToList();
        //        }

        //        if (allowedMenus != null)
        //        {
        //            foreach (var menus in allowedMenus)
        //            {

        //                var menuurl = menus.URI.ToLower().Split('/').ToList();
        //                if (!string.IsNullOrEmpty(menuurl[0]))
        //                {
        //                    if (controller.Contains(menuurl[0]))
        //                    {
        //                        List<string> identifier = menus.Access.Split(',').ToList();

        //                        if (identifier.Contains(Controls))
        //                        {

        //                            return true;
        //                        }

        //                    }
        //                }

        //            }


        //            if ((url.ToLower().Split('/').ToList()).Contains("admin"))
        //            {
        //                _httpContextAccessor.HttpContext.Response.Redirect("/Admin/DashBoard/Index");
        //            }
        //            else
        //            {
        //                _httpContextAccessor.HttpContext.Response.Redirect("/Client/DashBoard/Index");
        //            }

        //            return false;
        //        }



        //        _httpContextAccessor.HttpContext.Response.Redirect("/Login/Index?RedirectUrl=" + GetUrl.GetURL(_httpContextAccessor));
        //        return false;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public  bool AuthorizeCore()
        //{
        //    try

        //    {

        //        var url = GetUrl.GetURL(_httpContextAccessor);
        //        if (url.IndexOf("?") >= 0)
        //        {
        //            url = url.Substring(0, url.IndexOf("?"));
        //        }



        //        string isAdminClaimValue = "false";
        //        if (!string.IsNullOrEmpty(_loginUser.GetCurrentUser()))
        //        {
        //            isAdminClaimValue = _loginUser.GetCurrentUser();
        //        }

        //        bool isAdmin = false;

        //        var controller = url.ToLower().Split('/').ToList();
        //        //IEnumerable<Menu> allowedMenus = MenuRepository.GetMenuAccessBasedOnRole(_loginUser.GetCurrentUser());
        //        IEnumerable<Menu> allowedMenus = MenuRepository.GetMenuAccessBasedOnRole(_loginUser.GetCurrentUser());
        //        if (controller.Contains("admin"))
        //        {

        //            if (!string.IsNullOrEmpty(isAdminClaimValue))
        //            {
        //                isAdmin = Convert.ToBoolean(isAdminClaimValue);
        //            }
        //            if (isAdmin)
        //            {
        //                allowedMenus = allowedMenus.Where(x => x.isAdmin == true).ToList();
        //            }
        //            else
        //            {
        //                allowedMenus = null;
        //            }
        //        }
        //        else
        //        {
        //            allowedMenus = allowedMenus.Where(x => x.isAdmin == false).ToList();
        //        }

        //        if (allowedMenus != null)
        //        {
        //            foreach (var menus in allowedMenus)
        //            {

        //                var menuurl = menus.URI.ToLower().Split('/').ToList();
        //                if (!string.IsNullOrEmpty(menuurl[0]))
        //                {
        //                    if (controller.Contains(menuurl[0]))
        //                    {
        //                        List<string> identifier = menus.Access.Split(',').ToList();

        //                        if (identifier.Contains(Controls))
        //                        {

        //                            return true;
        //                        }

        //                    }
        //                }

        //            }


        //            if ((url.ToLower().Split('/').ToList()).Contains("admin"))
        //            {
        //                _httpContextAccessor.HttpContext.Response.Redirect("/Admin/DashBoard/Index");
        //            }
        //            else
        //            {
        //                _httpContextAccessor.HttpContext.Response.Redirect("/Client/DashBoard/Index");
        //            }

        //            return false;
        //        }



        //        _httpContextAccessor.HttpContext.Response.Redirect("/Login/Index?RedirectUrl=" + GetUrl.GetURL(_httpContextAccessor));
        //        return false;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public bool AuthorizeAddUpdate(decimal id)
        {
            try
            {
                var control = string.Empty;
                if (id == 0)
                {
                    //ADD
                    control = "Add";
                }
                else
                {
                    //EDIT
                    control = "Edit";

                }
                var url = GetUrl.GetURL(_httpContextAccessor);

                var controller = url.ToLower().Split('/').ToList();

                IEnumerable<UserMenu> allowedMenus = _menuRepository.GetMenuAccessBasedOnRole(_loginUser.GetCurrentUser()).ToList();

                if (allowedMenus != null)
                {
                    foreach (var menu in allowedMenus)
                    {
                        var menuurl = menu.MenuURI.Split('/').ToList();
                        if (!string.IsNullOrEmpty(menuurl[0]))
                        {
                            if (controller.Contains(menuurl[0].ToLower()))
                            {
                                List<string> identifier = menu.Access.Split(',').ToList();

                                if (identifier.Contains(control))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AuthorizeControlForButton(string control, IEnumerable<UserMenu> allowedMenus)
        {
            try
            {

                var url = GetUrl.GetURL(_httpContextAccessor);

                var controller = url.ToLower().Split('/').ToList();

                if (allowedMenus != null)
                {

                    foreach (var menu in allowedMenus)
                    {
                        var menuurl = menu.MenuURI.Split('/').ToList();
                        if (!string.IsNullOrEmpty(menuurl[0]))
                        {
                            if (controller.Contains(menuurl[0].ToLower()))
                            {
                                List<string> identifier = menu.Access.Split(',').ToList();

                                if (identifier.Contains(control))
                                {

                                    return true;
                                }

                            }
                        }

                    }

                    return false;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AuthorizeControl(string control)
        {
            try
            {

                var url = GetUrl.GetURL(_httpContextAccessor);

                var controller = url.ToLower().Split('/').ToList();
                IEnumerable<UserMenu> allowedMenus = _menuRepository.GetMenuAccessBasedOnRole(_loginUser.GetCurrentUser()).ToList();

                if (allowedMenus != null)
                {

                    foreach (var menu in allowedMenus)
                    {
                        var menuurl = menu.MenuURI.Split('/').ToList();
                        if (!string.IsNullOrEmpty(menuurl[0]))
                        {
                            if (controller.Contains(menuurl[0].ToLower()))
                            {
                                List<string> identifier = menu.Access.Split(',').ToList();

                                if (identifier.Contains(control))
                                {

                                    return true;
                                }

                            }
                        }

                    }

                    return false;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            try

            {
                bool result = false;
                var url = GetUrl.GetURL(_httpContextAccessor);
                if (url.IndexOf("?") >= 0)
                {
                    url = url.Substring(0, url.IndexOf("?"));
                }

                Claim isAdminClaim = _httpContextAccessor.HttpContext.User.FindFirst("isAdmin");

                string isAdminClaimValue = "false";

                if (isAdminClaim != null)
                {
                    isAdminClaimValue = isAdminClaim.Value;
                }                               

                bool isAdmin = false;

                if (!string.IsNullOrEmpty(isAdminClaimValue))
                {
                    isAdmin = Convert.ToBoolean(isAdminClaimValue);
                }

                if (!string.IsNullOrEmpty(context.User.Identity.Name) && isAdmin==true)
                {

                }
                else
                {
                    _httpContextAccessor.HttpContext.Response.Redirect("/Login/Index?RedirectUrl=" + GetUrl.GetURL(_httpContextAccessor));
                }
                    //var controller = url.ToLower().Split('/').ToList();
                    ////IEnumerable<Menu> allowedMenus = MenuRepository.GetMenuAccessBasedOnRole(_loginUser.GetCurrentUser());

                    //IEnumerable<Menu> allowedMenus = MenuRepository.GetMenuAccessBasedOnRole(context.User.Identity.Name);
                    //Task.WhenAll().Wait();
                    //if (controller.Contains("admin"))
                    //{

                    //    if (!string.IsNullOrEmpty(isAdminClaimValue))
                    //    {
                    //        isAdmin = Convert.ToBoolean(isAdminClaimValue);
                    //    }
                    //    if (isAdmin)
                    //    {
                    //        allowedMenus = allowedMenus.Where(x => x.isAdmin == true).ToList();
                    //    }
                    //    else
                    //    {
                    //        allowedMenus = null;
                    //    }
                    //}
                    //else
                    //{
                    //    allowedMenus = allowedMenus.Where(x => x.isAdmin == false).ToList();
                    //}

                    //if (allowedMenus != null)
                    //{
                    //    foreach (var menus in allowedMenus)
                    //    {

                    //        var menuurl = menus.URI.ToLower().Split('/').ToList();
                    //        if (!string.IsNullOrEmpty(menuurl[0]))
                    //        {
                    //            if (controller.Contains(menuurl[0]))
                    //            {
                    //                List<string> identifier = menus.Access.Split(',').ToList();

                    //                if (identifier.Contains(Controls))
                    //                {

                    //                    result = true;
                    //                }

                    //            }
                    //        }

                    //    }


                    //    if ((url.ToLower().Split('/').ToList()).Contains("admin"))
                    //    {
                    //        _httpContextAccessor.HttpContext.Response.Redirect("/Admin/DashBoard/Index");
                    //    }
                    //    else
                    //    {
                    //        _httpContextAccessor.HttpContext.Response.Redirect("/Client/DashBoard/Index");
                    //    }

                    //    result = false;
                    //}



                   
                //result = false;
                                              
                return Task.FromResult(isAdmin);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}