using DomainEntities;
using System.Collections.Generic;

namespace DomainInterface
{
    public interface IAuthorizeMenuHelper
    {
        bool AuthorizeAddUpdate(decimal id);
        bool AuthorizeControlForButton(string control, IEnumerable<UserMenu> allowedMenus);
        bool AuthorizeControl(string control);
    }
}
