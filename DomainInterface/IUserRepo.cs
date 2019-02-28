using System;
using System.Collections.Generic;
using DomainEntities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IUserRepo
    {
        ReturnType saveUser(User user);
        List<User> getAllUsers();
        ReturnType deleteUser(User user);
        ReturnType resetPassword(User user);
    }
}
