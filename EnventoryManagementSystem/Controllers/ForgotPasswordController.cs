using DomainEntities;
using DomainInterface;
using System;
using Microsoft.AspNetCore.Mvc;
using InventoryManagementSystem.Helper;
using Microsoft.AspNetCore.Authorization;

namespace TicketBookingSystem.Controllers
{
    [AllowAnonymous]
    public class ForgotPasswordController : Controller
    {
        //private readonly IUserPasswordManagementRepository userPasswordManagementRepo;
        //private readonly IUser userRepo;
        //private readonly ITemplateRepository _iTemplateRepo;
        //private readonly ISettingsRepository _iSettingsRepo;
        //private readonly string emailSettingGroup = SettingsGroupName.EmailGroup;
        //public ForgotPasswordController(ISettingsRepository settingRepo, 
        //    IUserPasswordManagementRepository userPasswordManagementRepo, 
        //    IUser userRepo, ITemplateRepository templateRepo)
        //{
        //    this.userPasswordManagementRepo = userPasswordManagementRepo;
        //    this.userRepo = userRepo;
        //    this._iTemplateRepo = templateRepo;
        //    this._iSettingsRepo = settingRepo;
        //}

        // GET: ForgotPassword
        public ActionResult Index()
        {
            ViewBag.Message = "";
            ViewBag.EmailSentStatus = false;
            return View();
        }

        //[HttpPost]
        //public ActionResult Index(string email)
        //{
        //    ViewBag.Message = "";
        //    ViewBag.EmailSentStatus = false;
        //    EmailAddress emailAddress = new EmailAddress();
        //    if (!String.IsNullOrEmpty(email))
        //    {
        //        bool emailExists = userRepo.CheckIfEmailExists(email);

        //        if (emailExists)
        //        {
        //            int passwordResetTime= Convert.ToInt16(_iSettingsRepo.GetSettingsBySettingsId("1022").SettingsValue);

        //            //Response response= userRepo.UserGetByEmail(email,passwordResetTime);

        //            string randomToken = userPasswordManagementRepo.GenerateRandomToken().ToString();

        //            userPasswordManagementRepo.InsertToForgotPassword(email, randomToken);

        //            Template template = _iTemplateRepo.TemplateGetByTemplateEventName("OnPasswordReset"); 

        //            template.Body = template.Body.Replace("##NAME##", "response.UserName").Replace("##RESETLINK##",
        //                String.Format("<a href=\'"+ _iSettingsRepo.GetSettingsBySettingsId("1012").SettingsValue + "/ChangePassword?Token=" + randomToken + "'>Reset Password Link</a>"));

        //            EmailSenderReceiverData emailSenderData = new EmailSenderReceiverData();
        //            emailSenderData.EmailTo = email;
        //            emailSenderData.SMTPHost = _iSettingsRepo.GetSettingsBySettingsId("1003").SettingsValue;
        //            emailSenderData.SMTPUserName = _iSettingsRepo.GetSettingsBySettingsId("1004").SettingsValue;
        //            emailSenderData.SMTPPassword = _iSettingsRepo.GetSettingsBySettingsId("1005").SettingsValue;

        //            bool emailSent = EmailHelper.SendEmail(emailSenderData, template);

        //            ViewBag.EmailSentStatus = emailSent;

        //            if (emailSent)
        //            {
        //                ModelState.Remove("Email");
        //                ViewBag.Message = "Password Reset link has been sent to your email address";
        //            }
        //            else
        //            {
        //                ViewBag.Message = "Error occured while sending Password Reset link. Try again!";
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("Email", "Email does not exists.");
        //        }

        //    }
        //    else
        //    {
        //        ModelState.AddModelError("Email", "Email required");
        //    }

        //    return View(emailAddress);
        //}
    } 
}