using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DomainEntities;
using DomainInterface;
using Microsoft.AspNetCore.Authorization;
using InventoryManagementSystem.Helper;

namespace InventoryManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class MenuController : Controller
    {
        
        #region Private Variables
        IRoleRepo _role;
        private readonly ILoginUser _loginUser;
        private readonly IUserProfile _userProfile;
        #endregion

        public MenuController(IRoleRepo rol, ILoginUser LoginUser, IUserProfile userProfile)
        {
            this._role = rol;
            this._loginUser = LoginUser;
            this._userProfile = userProfile;
        }
        public async Task<IActionResult> AdminMenu()
        {
            return await Task.Run(() =>
            {
                return View(_role.MenuGetBasedOnLoggedInUserRole(_loginUser.GetCurrentUser()));
            });
            // string loggedInUserName = _loginUser.GetCurrentUser();
            //iRole.MenuGetBasedOnLoggedInUserRole(_loginUser.GetCurrentUser());            
            //return await Task.Run(() => View(iRole.MenuGetBasedOnLoggedInUserRole(_loginUser.GetCurrentUser())));



        }
        public async Task<ActionResult> UserName()
        {
            await Task.Run(() => {
                ViewBag.loggedInUserName = _loginUser.GetCurrentUser();
               
            });
            return View();
        }

        [HttpGet]
        public JsonResult GetUserName()
        {
            return Json(new { data = _loginUser.GetCurrentUser() });
        }
        [HttpGet]
        public JsonResult GetProfileImage()
        {    
            return Json(new { data = _userProfile.GetUserProfile(_loginUser.GetCurrentUser()) });
        }
        [HttpGet]
        public JsonResult GetAppPath()
        {
            return Json(new { data = _userProfile.GetPath() });
        }
    }
}