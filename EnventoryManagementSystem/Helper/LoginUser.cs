﻿using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace InventoryManagementSystem.Helper
{
    public class LoginUser:ILoginUser
    {        
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public string GetCurrentUser()
        {

            try
            {              
                 return _httpContextAccessor.HttpContext.User.Identity.Name;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }

    }
    public interface ILoginUser
    {
        string GetCurrentUser();
    }
}