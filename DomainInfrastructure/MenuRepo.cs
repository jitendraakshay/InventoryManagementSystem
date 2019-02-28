using System;
using System.Collections.Generic;
using DomainEntities;
using DomainInterface;
using System.Data;
using Dapper;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using DomainRepository.DBManager;

namespace DomainRepository
{
    public class MenuRepository: IMenuRepository
    {
        IOptions<ReadConfig> connectionString;
        public MenuRepository(IOptions<ReadConfig> _connectionString)
        {
            connectionString = _connectionString;
        }
        public IEnumerable<UserMenu> GetMenuAccessBasedOnRole(string userName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@userName", userName);
                    var menusForUser = SqlMapper.Query<UserMenu>(
                                      connection, "[dbo].[usp_MenuGetBasedOnUser]", param, commandType: CommandType.StoredProcedure);
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
                                    menulist.Add(new UserMenu() { MenuID = item.MenuID,MenuName = item.MenuName, ParentName = item.ParentName, MenuURI = item.MenuURI, IconClass = item.IconClass, Access = string.Join(",", result) });
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
