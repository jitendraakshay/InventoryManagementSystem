using DomainEntities;
using System.Collections.Generic;

namespace DomainInterface
{
    public interface IUserProfile
    {
        ReturnType UserProfileImageSave(UserProfile userProfile);
        List<UserProfile> GetUserProfile(UserProfile userProfile);
        ReturnType SaveUserProfile(UserProfile userProfile);
        ReturnType UserProfileChangePassword(string OldPassword, string NewPassword, string UserName);
        IEnumerable<UserProfile> GetUserProfile(string userName = null);
        string getProfileImageName(string userName);
    }
}
