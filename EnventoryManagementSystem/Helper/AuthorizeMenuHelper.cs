using DomainEntities;
using DomainRepository;
using DomainInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Helper
{
    
    
    [Authorize]
    public class AuthorizeMenuHelper: IAuthorizeMenuHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoginUser _loginUser;
        private readonly IMenuRepository _menuRepository;

        public AuthorizeMenuHelper(IHttpContextAccessor HttpContextAccessor, ILoginUser LoginUser,IMenuRepository MenuRepository)
        {
            this._httpContextAccessor = HttpContextAccessor;
            this._loginUser = LoginUser;
            this._menuRepository = MenuRepository;
        }

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
    }

    
}
