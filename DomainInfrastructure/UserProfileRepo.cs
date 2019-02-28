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
        public ReturnType UserProfileSave(UserProfile oUserProfile)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@image", oUserProfile.ProfileImage);
                    param.Add("@userName", oUserProfile.UserName);                   
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
        public ReturnType UserProfileChangePassword(UserProfile oUserProfile)
        {
            return new ReturnType();
        }
        
        public string GetPath()
        {


            return Path.Combine(
                     Directory.GetCurrentDirectory(), "wwwroot\\profileImage");
        }
    }
}
