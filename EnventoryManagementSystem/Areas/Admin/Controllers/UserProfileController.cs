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
using Newtonsoft.Json;

namespace InventoryManagementSystem.Areas.Admin.Controllers
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
            //userProfile.StateID = 1;
            //userProfile.CityID = 2;
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
            ReturnType type = _userProfile.UserProfileImageSave(userProfile);
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

        [HttpGet]
        public ReturnType GetProfileImage()
        {
            try
            {                 
                ReturnType type = new ReturnType();
                string ImageName = Convert.ToString(_userProfile.getProfileImageName(_loginUser.GetCurrentUser()));
                if(!String.IsNullOrEmpty(ImageName))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\profileImage", ImageName);
                    string base64ImageRepresentation = "";
                    byte[] imageArray = System.IO.File.ReadAllBytes(path);
                    imageArray = System.IO.File.ReadAllBytes(path);
                    base64ImageRepresentation = Convert.ToBase64String(imageArray);
                    type.ProfileImage = base64ImageRepresentation;
                    type.ImageType = Path.GetExtension(ImageName);
                }
                else
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\user.png");
                    string base64ImageRepresentation = "";
                    byte[] imageArray = System.IO.File.ReadAllBytes(path);
                    imageArray = System.IO.File.ReadAllBytes(path);
                    base64ImageRepresentation = Convert.ToBase64String(imageArray);
                    type.ProfileImage = base64ImageRepresentation;
                    type.ImageType = Path.GetExtension(".PNG");
                }
                return type;
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

        [HttpPost]
        public object GetUserProfile(UserProfile userProfile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(userProfile.Password))
                    {
                        userProfile.Password = Crypto.OneWayEncryter(userProfile.Password);
                    }
                    userProfile.UserName = _loginUser.GetCurrentUser();
                    JsonResponse response = new JsonResponse();
                    List<UserProfile> model = _userProfile.GetUserProfile(userProfile);
                    response.ResponseData = model;
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    ReturnType type = new ReturnType();
                    type.Message = "Invalid data type or model...";
                    return type;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ReturnType SaveUserProfile(UserProfile userProfile)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(userProfile.Password))
                    {
                        userProfile.Password = Crypto.OneWayEncryter(userProfile.Password);
                    }
                    userProfile.EntryBy = _loginUser.GetCurrentUser();
                    ReturnType type = _userProfile.SaveUserProfile(userProfile);
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
       
        [HttpPost]
        public ReturnType SavePassword(string OldPassword, string NewPassword)
        {
            try
            {


                string oldPassword = Crypto.OneWayEncryter(OldPassword);
                string newPassword = Crypto.OneWayEncryter(NewPassword);
                string EntryBy = _loginUser.GetCurrentUser();
                ReturnType type = _userProfile.UserProfileChangePassword(oldPassword,newPassword,EntryBy);
                return type;
                
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}