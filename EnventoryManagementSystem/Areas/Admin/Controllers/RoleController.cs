using DomainEntities;
using DomainInterface;
using InventoryManagementSystem.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace TicketBookingSystem.Areas.Admin.Controllers
{

    //public struct RoleObject
    //{
    //    public string Name { get; set; }
    //    public int RoleID { get; set; }
    //    public string Menus { get; set; }
    //}
    [Authorize]
    [Area("Admin")]
    public class RoleController : Controller
    {
        #region Private Variables
        IRoleRepo iRole;
        private readonly ILoginUser _loginUser;
        #region Constructor
        public RoleController(IRoleRepo rol, ILoginUser LoginUser)
        {
            iRole = rol;
            _loginUser = LoginUser;
        }
        #endregion
        public ActionResult Index()
        {
            
            ViewBag.AdminMenu = iRole.MenuGet(true);
            ViewBag.ClientMenu = iRole.MenuGet(false);
            

            return View();
        }
        #endregion

        #region SaveRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveRole(Role obj)
        {
            var xml = JsonConvert.DeserializeXmlNode("{\"Menu\":" + obj.Menus + "}", "root");
            Role oRole = new Role();
            oRole.RoleID = obj.RoleID;
            oRole.Name = obj.Name;
            //LoginUser oLoginUser = new LoginUser();

            return Json(iRole.RoleMenuSave(oRole, xml.InnerXml,_loginUser.GetCurrentUser() ));
        }
        #endregion

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public ReturnType SaveNewRole(string roleName)
        {
            
                ReturnType type = iRole.UserRoleAddUpdateDelete(roleName, _loginUser.GetCurrentUser(), null, true, 1);
                return type;
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public ReturnType DeleteRole(int? roleID)
        {

            ReturnType type = iRole.UserRoleAddUpdateDelete(null, _loginUser.GetCurrentUser(), roleID, true, 3);
            return type;
        }
        [HttpGet]
        public dynamic Get(int roleID)
        {
            return JsonReturn.True("role", iRole.RoleGet(roleID));
        }

        [HttpGet]
        public dynamic Get()
        {
            return JsonReturn.True("roleList", iRole.RoleGet());
        }

        [HttpGet]
        public dynamic GetMenu(bool isAdmin)
        {
            return JsonReturn.True("roleList", iRole.MenuGet(isAdmin));
        }


        #region Check For Allowed Menus
        private void AllowedMenus()
        {
            //IEnumerable<TBS.DomainModel.Menu> allowedMenus = MenuRepository.GetMenuAccessBasedOnRole(new LoginUser().UserName);
            //ViewBag.viewAccess = AuthorizeUser.AuthorizeControlForButton("View", allowedMenus);
            //ViewBag.editAccess = AuthorizeUser.AuthorizeControlForButton("Edit", allowedMenus);
            //ViewBag.createAccess = AuthorizeUser.AuthorizeControlForButton("Add", allowedMenus);
            //ViewBag.deleteAccess = AuthorizeUser.AuthorizeControlForButton("Delete", allowedMenus);

            ViewBag.viewAccess = true;
            ViewBag.editAccess =true;
            ViewBag.createAccess = true;
            ViewBag.deleteAccess = true;
        }
        #endregion

        [HttpGet]
        public object getRoles()
        {
            JsonResponse response = new JsonResponse();
            IEnumerable<MenuRole> model = iRole.RoleMenuGet();
            var roles = model.GroupBy(p => p.RoleID).Select(lst => lst.First())
                .Select(x => new { x.RoleName, x.RoleID, x.Options }).ToList();
            List<MenuRole> mnu = new List<MenuRole>();

            foreach (var role in roles)
            {
                var menu = model.Where(x => x.RoleID == role.RoleID && x.MenuID > 0).Select(lst => new { lst.MenuID, lst.Options }).ToList();
                MenuRole a = new MenuRole();
                a.RoleName = role.RoleName;
                a.RoleID = role.RoleID;
                mnu.Add(a);
            }
            List<MenuRole> roleList = mnu;
            response.ResponseData = roleList;
            return JsonConvert.SerializeObject(response);
            
        }


        [HttpGet]
        public JsonResult GetMenuByRole(int roleID)
        {
            List<UserMenu> roleMenuList = iRole.GetMenusByRole(true, roleID);
            return Json(new { data = roleMenuList });
        }

        [HttpGet]
        public JsonResult GetMenus()
        {
            IEnumerable<UserMenu> menu= iRole.MenuGet(true);
            return Json(new { data = menu });
        }
    }
}