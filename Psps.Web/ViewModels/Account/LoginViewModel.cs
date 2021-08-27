using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Framework.Mvc;
using Psps.Web.Validators;
using System.ComponentModel.DataAnnotations;

namespace Psps.Web.ViewModels.Account
{
    [Validator(typeof(LoginViewModelValidator))]
    public class LoginViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "User_UserId")]
        public string UserId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "User_Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ChangePassword_OldPassword")]
        public string OldPassword { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ChangePassword_NewPassword")]
        public string NewPassword { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ChangePassword_ConfirmNewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
}