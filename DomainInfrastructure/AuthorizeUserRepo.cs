using Dapper;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using DomainInterface;

namespace DomainRepository
{
    public class AuthorizeUserRepo: IAuthorizeUserRepo
    {
        IOptions<ReadConfig> connectionString;
        public AuthorizeUserRepo(IOptions<ReadConfig> _connectionString)
        {
            connectionString = _connectionString;
        }
        public bool SaveToken(string token, string userName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {

                    DynamicParameters param = new DynamicParameters();
                    param.Add("@tokenKey", token);
                    param.Add("@userName", userName);
                    connection.Execute("usp_TokenSave", param, commandType: CommandType.StoredProcedure);

                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public ReturnType CheckUser( string userName, string code)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {

                    DynamicParameters param = new DynamicParameters();
                    param.Add("@userName", userName);
                    param.Add("@password", code);
                    var returnType = SqlMapper.Query<ReturnType>(
                                      connection, "usp_UserCheck", param, commandType: CommandType.StoredProcedure).ToList().FirstOrDefault();
                    if (returnType == null)
                    {
                        returnType = new ReturnType();
                        returnType.Result = false;

                    }
                    if (string.IsNullOrEmpty(returnType.ProfileImage))
                    {
                        returnType.ProfileImage = "";
                    }
                    return returnType;
                }
            }
            catch(Exception ex)
            {
                ReturnType returnType = new ReturnType();
                returnType.Result = false;
                return null;
            }

        }
        //public IEnumerable<User> UserGet(string userName = null)
        //{
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
        //        {

        //            DynamicParameters param = new DynamicParameters();
        //            param.Add("@userName", userName);
        //            var returnType = SqlMapper.Query<User>(
        //                              connection, "usp_UserGet", param, commandType: CommandType.StoredProcedure).ToList();

        //            return returnType;
        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }

        //}
        //public bool CheckIfAdmin(string userName)
        //{
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
        //        {

        //            DynamicParameters userParam = new DynamicParameters();
        //            userParam.Add("@userName", userName);
        //            userParam.Add("@isAdmin", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //            connection.Execute("[dbo].[usp_UsersCheckIfAdmin]", userParam, commandType: CommandType.StoredProcedure);

        //            var isAdmin = userParam.Get<bool>("@isAdmin");

        //            return isAdmin;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public bool CheckIfSuperUser(string userName)
        //{
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
        //        {

        //            DynamicParameters userGroupParam = new DynamicParameters();
        //            userGroupParam.Add("@userName", userName);
        //            userGroupParam.Add("@isSuperUser", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //            connection.Execute("[dbo].[usp_UsersCheckIfSuperUser]", userGroupParam, commandType: CommandType.StoredProcedure);

        //            var isSuperUser = userGroupParam.Get<dynamic>("@isSuperUser");
        //            if (isSuperUser == null) {
        //                isSuperUser = false;
        //            }
        //            return isSuperUser;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public void SetLoginInfor(string UserName, string IPAddress)
        //{
        //    using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
        //    {

        //        DynamicParameters param = new DynamicParameters();
        //        param.Add("@UserName", UserName);
        //        param.Add("@IpAddress", IPAddress);
        //        //List<TicketStatistics> ticketStats = SqlMapper.Query<TicketStatistics>(connection, "[dbo].[usp_TicketStatisticsGetByFiscalYearCode]", param, commandType: CommandType.StoredProcedure).ToList();
        //        //ticketStats = SqlMapper.Query<TicketStatistics>(connection, "[dbo].[SetLoginInfo]", param, commandType: CommandType.StoredProcedure).ToList();
        //        connection.Execute("[dbo].[usp_UserLoginLogSave]", param, commandType: CommandType.StoredProcedure);
        //    }
        //}
        //public ReturnType CheckActiveUser(string UserName)
        //{
        //    ReturnType returnType = new ReturnType();
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
        //        {
        //            DynamicParameters param = new DynamicParameters();
        //            param.Add("@UserName", UserName);
        //            param.Add("@Result", dbType: DbType.Boolean, direction: ParameterDirection.Output, size: 554321);
        //            param.Add("@InactiveDateTime", dbType: DbType.String, direction: ParameterDirection.Output, size: 554321);
        //            connection.Execute("dbo.uspCheckActiveUser", param, commandType: CommandType.StoredProcedure);
        //            bool Response = param.Get<dynamic>("@Result");
        //            string Message = param.Get<dynamic>("@InactiveDateTime");
        //            returnType.Result = Response;
        //            returnType.Message = Message;
        //        }
        //        return returnType;
        //    }
        //    catch (Exception ex)
        //    {
        //        return returnType;
        //    }
        //}
        //public ReturnType ManageFailedLoginAttempt(string UserName, string IPAddress)
        //{
        //    ReturnType returnType = new ReturnType();
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
        //        {
        //            DynamicParameters param = new DynamicParameters();
        //            param.Add("@UserName", UserName);
        //            param.Add("@IPAddress", IPAddress);
        //            param.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 554321);
        //            connection.Execute("[dbo].[usp_ManageFailedLoginAttempt]", param, commandType: CommandType.StoredProcedure);
        //            string Response = param.Get<dynamic>("@Message");
        //            returnType.Message = Response;
        //        }
        //        return returnType;
        //    }
        //    catch (Exception ex)
        //    {
        //        return returnType;
        //    }
        //}

    }
}
