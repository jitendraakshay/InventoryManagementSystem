using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;

namespace DomainInterface
{
  public  interface IRoleRepo
    {
        List<Role> RoleGetAllForDDL();

        IEnumerable<Role> RoleGet(int? roleID = null);
        IEnumerable<UserMenu> MenuGet(bool IsAdmin);
        ReturnType RoleSave(Role oUser, string userName);
        ReturnType UserRoleAddUpdateDelete(string roleName, string userName, int? roleID, bool isActive, int operation);
        IEnumerable<MenuRole> RoleMenuGet(int? roleID = null);
        ReturnType RoleMenuSave(Role oRole, string xml, string userName);
        IEnumerable<UserMenu> MenuGet(int roleID);
        IEnumerable<UserMenu> MenuGetBasedOnLoggedInUserRole(string userName);
        List<MenuRole> GetMenuByRole(int? roleID);
        List<UserMenu> GetMenusByRole(bool isAdmin, int roleID);
    }
}
