using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DomainEntities
{
    public class UserProfile
    {

        public string UserName { get; set; }

        [DisplayName("Address")]
        //[Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }

        [DisplayName("City")]
        //[Required(ErrorMessage = "City is Required")]
        public int CityID { get; set; }

        [DisplayName("State")]
        //[Required(ErrorMessage = "State is Required")]
        public int StateID { get; set; }

        [DisplayName("Profile Image")]
        public string ProfileImage { get; set; }

        [DisplayName("Password")]
        //[Required(ErrorMessage = "Password Required")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[RegularExpression("^((?=.*[a-z])(?=.*[A-Z])(?=.*\\d)).+$", ErrorMessage = "Enter atleast one upper case ,one lower case and a number")]
        public string PassCode { get; set; }

        [DisplayName("Old Password")]
        //[Required(ErrorMessage = "Old Password is Required")]
        public string OldPassword { get; set; }

        [DisplayName("Confirm Password")]
        //[Required(ErrorMessage = "Confirm Password is Required")]
        //[Compare("PassCode")]
        public string ConfirmPassCode { get; set; }
    }
}