using DomainEntities;
using System.Collections.Generic;

namespace DomainInterface
{
    public interface IUserProfile
    {
        ReturnType UserProfileSave(UserProfile oUserProfile);
        ReturnType UserProfileChangePassword(UserProfile oUserProfile);
        IEnumerable<UserProfile> GetUserProfile(string userName = null);
        string GetPath();
    }
}
