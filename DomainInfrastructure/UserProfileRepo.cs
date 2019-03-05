using Dapper;
using DomainEntities;
using DomainInterface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DomainRepository
{
    public class UserProfileRepo:IUserProfile
    {
        IOptions<ReadConfig> connectionString;
        public UserProfileRepo(IOptions<ReadConfig> _connectionString)
        {
            connectionString = _connectionString;
        }
        public ReturnType UserProfileImageSave(UserProfile userProfile)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@image", userProfile.ProfileImage);
                    param.Add("@userName", userProfile.UserName);                   
                    var returnType = SqlMapper.Query<ReturnType>(
                                     connection, "[dbo].[usp_SaveUserProfile]", param, commandType: CommandType.StoredProcedure).ToList()?.FirstOrDefault() ?? null;
                    return returnType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserProfile> GetUserProfile(UserProfile userProfile)
        {
            try
            {
                
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@userID", userProfile.UserID);
                    param.Add("@firstName", userProfile.FirstName);
                    param.Add("@middleName", userProfile.MiddleName);
                    param.Add("@lastName", userProfile.LastName);
                    param.Add("@userName", userProfile.UserName);
                    param.Add("@email", userProfile.Email);
                    param.Add("@phoneNo", userProfile.PhoneNo);
                    param.Add("@address", userProfile.Address);
                    param.Add("@mobileNo", userProfile.MobileNo);
                    param.Add("@password", userProfile.Password);                   
                    param.Add("@entryBy", userProfile.EntryBy);                   
                    param.Add("@operation", 1);// 1 select, 2 update            
                    var returnType = SqlMapper.Query<UserProfile>(
                                     connection, "[dbo].[usp_CrudUserProfile]", param, commandType: CommandType.StoredProcedure).ToList();
                    return returnType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ReturnType SaveUserProfile(UserProfile userProfile)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@userID", userProfile.UserID);
                    param.Add("@firstName", userProfile.FirstName);
                    param.Add("@middleName", userProfile.MiddleName);
                    param.Add("@lastName", userProfile.LastName);
                    param.Add("@userName", userProfile.UserName);
                    param.Add("@email", userProfile.Email);
                    param.Add("@phoneNo", userProfile.PhoneNo);
                    param.Add("@address", userProfile.Address);
                    param.Add("@mobileNo", userProfile.MobileNo);
                    param.Add("@password", userProfile.Password);
                    param.Add("@entryBy", userProfile.EntryBy);
                    param.Add("@operation", 2);// 1 select, 2 update            
                    var returnType = SqlMapper.Query<ReturnType>(
                                     connection, "[dbo].[usp_CrudUserProfile]", param, commandType: CommandType.StoredProcedure).ToList()?.FirstOrDefault() ?? null;
                    return returnType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<UserProfile> GetUserProfile(string userName = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@userName", userName);
                    var returnType = SqlMapper.Query<UserProfile>(
                                      connection, "[dbo].[usp_GetUserProfile]", param, commandType: CommandType.StoredProcedure).ToList();
                    
                    return returnType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }        
        public string getProfileImageName(string userName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@userName", userName);
                var returnType = SqlMapper.Query(connection, "[dbo].[usp_GetProfileImageName]", param, commandType: CommandType.StoredProcedure).SingleOrDefault();

                return returnType.Image;
            }
        }

        public ReturnType UserProfileChangePassword(string OldPassword, string NewPassword, string UserName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@oldPassword", OldPassword);
                    param.Add("@newPassword", NewPassword);
                    param.Add("@userName", UserName);
                    var returnType = SqlMapper.Query<ReturnType>(
                                      connection, "[dbo].[usp_SaveUserPassword]", param, commandType: CommandType.StoredProcedure).ToList()?.FirstOrDefault() ?? null;

                    return returnType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
