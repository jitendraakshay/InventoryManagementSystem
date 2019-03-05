using DomainInterface;
using InventoryManagementSystem.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Areas.Admin.ViewComponents
{
    public class MenuItemsViewComponent : ViewComponent
    {
        private readonly ILoginUser _loginUser;
        private readonly IRoleRepo iRole;
        public MenuItemsViewComponent(IRoleRepo rol, ILoginUser LoginUser)
        {
            this.iRole = rol;
            this._loginUser = LoginUser;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = await Task.Run(() => iRole.MenuGetBasedOnLoggedInUserRole(_loginUser.GetCurrentUser()));
            return View(response);
        }
    }
}
