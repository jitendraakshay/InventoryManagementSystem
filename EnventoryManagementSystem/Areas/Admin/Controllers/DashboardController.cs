using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DomainRepository;
using DomainInterface;
using InventoryManagementSystem.Helper;

namespace InventoryManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class DashboardController : Controller
    {
        #region Private Variables
        public readonly IRoleRepo _iRole;
        public readonly IDashboardRepo _iDashboardRepo;
        public readonly ISettingsRepo _SettingsRepo;
        public readonly ILoginUser _loginUser;
        public readonly IAuthorizeMenuHelper _authorizeMenuHelper;    
        private readonly IMenuRepository _menuRepository;
        #endregion

        public DashboardController(IRoleRepo rol, IDashboardRepo iDashboardRepo, ISettingsRepo iSettingsRepo, ILoginUser LoginUser, IAuthorizeMenuHelper AuthorizeMenuHelper, IMenuRepository MenuRepository)

        {
            this._iRole = rol;
            this._iDashboardRepo = iDashboardRepo;            
            this._SettingsRepo = iSettingsRepo;
            this._loginUser = LoginUser;
            this._authorizeMenuHelper = AuthorizeMenuHelper;
            this._menuRepository = MenuRepository;
        }
       
        public async Task<ActionResult> Index()
        {

            await Task.Run(() => {
                IEnumerable<UserMenu> allowedMenus = _menuRepository.GetMenuAccessBasedOnRole(_loginUser.GetCurrentUser());
                //authorize different data pannels
                ViewBag.Statistic = _authorizeMenuHelper.AuthorizeControlForButton("Statistic", allowedMenus);
                ViewBag.Graph = _authorizeMenuHelper.AuthorizeControlForButton("Graph", allowedMenus);
                ViewBag.QuickLinks = _authorizeMenuHelper.AuthorizeControlForButton("QuickLinks", allowedMenus);
            });
            return View(/*DashBoardMenu*/);
        }

        public IEnumerable<UserMenu> GetRole(int roleID)
        {            
            return _iRole.MenuGet(roleID);
        }

        #region MenuList
        public JsonResult GetMenuList()
        {
            List<UserMenu> DashBoardMenu = new List<UserMenu>();
            //var menuID = DomainEntities.Common.GetAllMenuIDs();

            string loggedInUserName = _loginUser.GetCurrentUser();
            var roles = _iRole.MenuGetBasedOnLoggedInUserRole(loggedInUserName);

            foreach (var role in roles)
            {
                //if (menuID.Contains(role.MenuID))
                //{
                    DashBoardMenu.Add(role);
                //}
            }
            return Json(DashBoardMenu);
        }
        #endregion

    }
}