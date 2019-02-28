using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using DomainInterface;
using System.Data;
using Dapper;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace DomainRepository
{
    public class UserRepo : IUserRepo
    {
        IOptions<ReadConfig> connectionString;
        public UserRepo(IOptions<ReadConfig> _connectionString)
        {
            connectionString = _connectionString;
        }


        #region SaveUser
        public ReturnType saveUser(User user)
        {
            try
            {
                int? operation = null;
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    if(user.UserID==null)
                    {
                        operation = 1;
                    }
                    else
                    {
                        operation = 3;
                    }
                    DynamicParameters param = new DynamicParameters();
                    
                    param.Add("@userID", user.UserID);
                    param.Add("@firstName", user.FirstName);
                    param.Add("@lastName", user.LastName);
                    param.Add("@userName", user.UserName);
                    param.Add("@email", user.Email);
                    param.Add("@phoneNo", user.PhoneNo);
                    param.Add("@address", user.Address);
                    param.Add("@mobileNo", user.MobileNo);
                    param.Add("@password", user.Password);
                    param.Add("@roleID", user.RoleID);
                    param.Add("@entryBy", user.EntryBy);
                    param.Add("@isActive", user.IsActive);
                    param.Add("@operation", operation);// 1 create, 2 read, 3 update, 4 delete                    
                    var returnType = SqlMapper.Query<ReturnType>(
                                     connection, "[dbo].[usp_CrudUser]", param, commandType: CommandType.StoredProcedure).ToList()?.FirstOrDefault() ?? null;
                    return returnType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get User List
        public List<User> getAllUsers()
        {
            using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@operation", 2);
                var returnType = SqlMapper.Query<User>(
                                  connection, "[dbo].[usp_CrudUser]", param, commandType: CommandType.StoredProcedure).ToList();
                return returnType;
            }
        }
        #endregion

        #region Delete User
        public ReturnType deleteUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@userID", user.UserID);
                param.Add("@isActive", user.IsActive);
                param.Add("@operation", 4);
                var returnType = SqlMapper.Query<ReturnType>(
                                  connection, "[dbo].[usp_CrudUser]", param, commandType: CommandType.StoredProcedure).ToList()?.FirstOrDefault() ?? null;
                return returnType;
            }
        }

        public ReturnType resetPassword(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@userID", user.UserID);
                param.Add("@password", user.Password);
                param.Add("@updatedBy", user.EntryBy);
                param.Add("@operation", 5);
                var returnType = SqlMapper.Query<ReturnType>(
                                  connection, "[dbo].[usp_CrudUser]", param, commandType: CommandType.StoredProcedure).ToList()?.FirstOrDefault() ?? null;
                return returnType;
            }
        }
        #endregion


    }



}
