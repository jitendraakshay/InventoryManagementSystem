using Dapper;
using DomainEntities;
using DomainInterface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace DomainRepository
{
    public class RoleRepo : IRoleRepo
    {
        IOptions<ReadConfig> connectionString;
        public RoleRepo(IOptions<ReadConfig> _connectionString)
        {
            connectionString = _connectionString;
        }
        #region RoleGetAllForDDL
        public List<Role> RoleGetAllForDDL()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {

                    List<Role> roleList = SqlMapper.Query<Role>(connection, "[dbo].[usp_GetAllUserRole]", commandType: CommandType.StoredProcedure).ToList();
                    return roleList;
                }
            }
            catch (Exception ex)
            {

              throw ex;
            }
        }
        #endregion

        #region RoleGet
        public IEnumerable<Role> RoleGet(int? roleID = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@roleID", roleID);
                    var returnType = SqlMapper.Query<Role>(
                                      connection, "[dbo].[usp_UserRoleGet]", param, commandType: CommandType.StoredProcedure).ToList();
                    return returnType;
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region RoleMenuGet
        public IEnumerable<MenuRole> RoleMenuGet(int? roleID = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@roleID", roleID);
                    string cond = roleID > 0 ? "where R.RoleID=" + roleID : string.Empty;
                    var returnType = connection.Query<MenuRole>(
                                      $@"SELECT Distinct R.RoleName,MR.Access Options,M.MenuName,R.RoleID,M.MenuID FROM dbo.tblUserRole R
                                            LEFT JOIN dbo.tblMenuRole MR ON R.RoleID = MR.RoleID
                                            LEFT JOIN dbo.tblUserMenu M ON M.MenuID = MR.MenuID {cond}").ToList();
                    return returnType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MenuRole> GetMenuByRole(int? roleID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@roleID", roleID);
                    string cond = roleID > 0 ? "where R.RoleID=" + roleID : string.Empty;
                    var returnType = connection.Query<MenuRole>(
                                      $@"SELECT distinct R.RoleName,MR.Access Options,M.MenuName,R.RoleID,M.MenuID FROM dbo.tblUserRole R
                                            LEFT JOIN dbo.tblMenuRole MR ON R.RoleID = MR.RoleID
                                            LEFT JOIN dbo.tblUserMenu M ON M.MenuID = MR.MenuID {cond}").ToList();
                    return returnType;
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion
        #region RoleMenuSave
        public ReturnType RoleMenuSave(Role oRole, string xml, string userName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@RoleID", oRole.RoleID);
                    param.Add("@RoleName", oRole.Name);
                    param.Add("@xml", xml);
                    param.Add("@userName", userName);
                    var returnType = SqlMapper.Query<ReturnType>(
                                     connection, "[dbo].[usp_UserRoleMenuSave]", param, commandType: CommandType.StoredProcedure).ToList()?.FirstOrDefault() ?? null;
                    return returnType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region RoleSave
        public ReturnType RoleSave(Role oRole, string userName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@name", oRole.Name);
                    param.Add("@remarks", oRole.Remarks);
                    param.Add("@userName", userName);
                    var returnType = SqlMapper.Query<ReturnType>(
                                     connection, "usp_RoleSave", param, commandType: CommandType.StoredProcedure).ToList()?.FirstOrDefault() ?? null;
                    return returnType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        public ReturnType UserRoleAddUpdateDelete(string roleName, string userName, int? roleID,bool isActive,int operation )
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@roleName", roleName);
                    param.Add("@userName", userName);
                    param.Add("@roleID", roleID);
                    param.Add("@isActive", isActive);
                    param.Add("@operation", operation);
                    var returnType = SqlMapper.Query<ReturnType>(
                                     connection, "[dbo].[usp_UserRoleAddUpdateDelete]", param, commandType: CommandType.StoredProcedure).ToList()?.FirstOrDefault() ?? null;
                    return returnType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region MenuGet
        public IEnumerable<UserMenu> MenuGet(bool isAdmin)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                  
                    var returnType = SqlMapper.Query<UserMenu>(                                   
                                   connection, string.Format("SELECT M.MenuName, M.Options, M.MenuID, M.ParentID, CASE WHEN M.ParentID = 0 THEN M.MenuID ELSE M.ParentID END AS testParentID FROM  dbo.tblUserMenu M  ORDER BY testParentID, M.ParentID",isAdmin));

                    return returnType;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<UserMenu> GetMenusByRole(bool isAdmin, int roleID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
            {
                List<UserMenu> returnType = SqlMapper.Query<UserMenu>(connection, string.Format("SELECT M.MenuName ,MR.Access Options,R.RoleID,M.MenuID,M.ParentID,CASE WHEN M.ParentID = 0 THEN M.MenuID ELSE M.ParentID END AS testParentID FROM dbo.tblUserRole R LEFT JOIN dbo.tblMenuRole MR ON R.RoleID = MR.RoleID LEFT JOIN dbo.tblUserMenu M ON M.MenuID = MR.MenuID LEFT JOIN dbo.tblUserMenu R2 ON M.ParentID = R2.MenuID where R.RoleID='{0}'  ORDER BY testParentID, M.ParentID ", roleID)).ToList();

                return returnType;
            }
        }

        public IEnumerable<UserMenu> MenuGet(int roleID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {                  
                    var returnType = SqlMapper.Query<UserMenu>(
                                    connection, string.Format("SELECT M.*,isnull(m1.Name,'') as ParentName FROM dbo.MenuRole MR LEFT JOIN Menu M ON MR.MenuID=M.MenuID left JOIN Menu m1 on m1.MenuID = m.ParentID WHERE MR.RoleID={0} order by OrderBy", roleID));
                    return returnType;

                }
            }
            catch
            {
                return null;
            }
        }
        #endregion
        public IEnumerable<UserMenu> MenuGetBasedOnLoggedInUserRole(string userName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@userName", userName);
                    var menusForUser = SqlMapper.Query<UserMenu>(
                                      connection, "[dbo].[usp_MenuGetBasedOnUser]", param, commandType: CommandType.StoredProcedure).ToList();

                    List<UserMenu> menulist = new List<UserMenu>();
                    List<string> name = new List<string>();
                    foreach (var item in menusForUser)
                    {
                        if (menusForUser.Where(x => x.MenuID == item.MenuID).Select(x => x.MenuName).ToList().Count > 1)
                        {
                            if (menulist.Where(x => x.MenuID == item.MenuID).ToList().Count < 1)
                            {
                                foreach (var r in menusForUser.Where(x => x.MenuID == item.MenuID).Select(x => x.Access).ToList())
                                {
                                    name.Add(r);
                                }
                                if (name.Count > 0)
                                {
                                    string names = string.Join(",", name);
                                    List<string> result = names.Split(',').Distinct().ToList();
                                    menulist.Add(new UserMenu() { MenuID = item.MenuID, MenuName = item.MenuName, ParentName = item.ParentName, MenuURI = item.MenuURI, IconClass = item.IconClass, Access = string.Join(",", result) });
                                    name.Clear();
                                }
                            }
                        }
                        else
                        {
                            menulist.Add(new UserMenu() { MenuID = item.MenuID, MenuName = item.MenuName, ParentName = item.ParentName, MenuURI = item.MenuURI, IconClass = item.IconClass, Access = item.Access });
                        }
                    }

                    return menulist;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
    }
}
