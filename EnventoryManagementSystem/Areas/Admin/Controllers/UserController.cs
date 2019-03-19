using DomainEntities;
using InventoryManagementSystem.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using DomainInterface;

namespace InventoryManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class UserController : Controller
    {
       
        private readonly IUserRepo userRepo;        
        private readonly IRoleRepo roleRepo;
        private readonly ILoginUser loginUser;
        #region Constructor
        public UserController(IUserRepo UserRepo, IRoleRepo RoleRepo, ILoginUser LoginUser)
        {
            this.userRepo = UserRepo;
            this.roleRepo = RoleRepo;
            this.loginUser = LoginUser;

        }
        #endregion

        #region IndexAction
        // GET: Admin/Users

        public IActionResult Index()
        {
            int pageLength = 10;
            ViewBag.pageLength = pageLength;
            return View();
        }
        #endregion
    

        #region UserSave

        [HttpPost]
        
        public ReturnType saveUser(User User)
        {
            
            try
            {
                

                if (ModelState.IsValid)
                {                    
                    if (!string.IsNullOrEmpty(User.Password))
                    {
                        User.Password = Crypto.OneWayEncryter(User.Password);                        
                    }
                    User.EntryBy = loginUser.GetCurrentUser();
                    ReturnType type = userRepo.saveUser(User);
                    return type;
                }      
                else
                {
                    ReturnType type = new ReturnType();
                    type.Message = "Something went wrong...";
                    return type;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion


        [HttpPost]
        public JsonResult getAllUsers()
        {

            DataTableFilters filter = new DataTableFilters();
            filter.Offset = Convert.ToInt32(Request.Form["start"]);
            filter.Limit = Convert.ToInt32(Request.Form["length"]);
            List<User> UserList = userRepo.getAllUsers(filter);
            long totalRows = UserList.Count > 0 ? UserList[0].TotalCount : 0;
            return Json(new { data = UserList, recordsTotal = totalRows, recordsFiltered = totalRows });
        }
        [HttpGet]
        public ReturnType deleteUser(User user)
        {
            ReturnType type = userRepo.deleteUser(user);
            return type;

        }
        [HttpGet]
        public ReturnType resetPassword(User user)
        {
            user.Password = Crypto.OneWayEncryter(user.UserName);
            user.EntryBy = loginUser.GetCurrentUser();
            ReturnType type = userRepo.resetPassword(user);
            return type;

        }


    }


}