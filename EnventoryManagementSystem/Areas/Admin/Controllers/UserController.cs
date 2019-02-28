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
        //User user = new User();
        //private readonly IMessageHandlerRepository messageRepo;
        private readonly IUserRepo userRepo;
        //private readonly IStateRepository _iStateRepo;
        //private readonly IStatusRepository _iStatusRepo;
        //private readonly ICityRepository _iCityRepo;
        //private readonly IDepartmentRepository _iDepartmentRepo;
        //private readonly IDesignationRepository _iDesignationRepo;
        private readonly IRoleRepo roleRepo;
        private readonly ILoginUser loginUser;
        //private readonly ISettingsRepository _iSettingsRepo;
        //private readonly INotificationRepository _iNotificationRepo;
        //private readonly ITemplateRepository _iTemplateRepo;
        //string imageName;

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


        [HttpGet]
        public object getAllUsers()
        {
            JsonResponse response = new JsonResponse();
            List<User> model = userRepo.getAllUsers();   
            response.ResponseData = model;
            return JsonConvert.SerializeObject(response);

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