using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainEntities;
using DomainInterface;
using InventoryManagementSystem.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace EnventoryManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class UserProfileController : Controller
    {
        private readonly IUserProfile _userProfile;
        private readonly ILoginUser _loginUser;
        public UserProfileController(IUserProfile userProfile, ILoginUser loginUser)
        {
            this._userProfile = userProfile;
            this._loginUser = loginUser;
        }
        public IActionResult Index()
        {
            //GET DATA OF LOGGED IN USER 
            string loggedInUserName = _loginUser.GetCurrentUser();
            UserProfile userInfo = _userProfile.GetUserProfile(_loginUser.GetCurrentUser()).ToList().FirstOrDefault();           
            UserProfile userProfile = new UserProfile();
            userProfile.Address = "";
            userProfile.ProfileImage = userProfile.ProfileImage;
            userProfile.StateID = 1;
            userProfile.CityID = 2;
            return View(userProfile);
           
        }
        [HttpPost]
        public async Task<string> FileUpload(IFormFile file)
        {
           
            string image= System.DateTime.Now.ToString("yyyyMMddHHmmss").ToString() + file.FileName;
            string ImageName = image;
            
            UserProfile userProfile = new UserProfile();
            userProfile.ProfileImage = image;
            userProfile.UserName = _loginUser.GetCurrentUser();
            ReturnType type = _userProfile.UserProfileSave(userProfile);
            if(type.Message.Length>0)
            {
                var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\profileImage",
                        ImageName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }           

            return type.Message;
        }

        
    }
}