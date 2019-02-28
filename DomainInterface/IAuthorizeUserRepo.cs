using System;
using System.Collections.Generic;
using System.Text;
using DomainEntities;


namespace DomainInterface
{
    public interface IAuthorizeUserRepo
    {
        bool SaveToken(string token, string userName);
        ReturnType CheckUser(string userName, string code);
        //IEnumerable<User> UserGet(string userName = null);
        //bool CheckIfAdmin(string userName);
        //bool CheckIfSuperUser(string userName);
        //void SetLoginInfor(string UserName, string IPAddress);
        //ReturnType CheckActiveUser(string UserName);
        //ReturnType ManageFailedLoginAttempt(string UserName, string IPAddress);
    }
}
