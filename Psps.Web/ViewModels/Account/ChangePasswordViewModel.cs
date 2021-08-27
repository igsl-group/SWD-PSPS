using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace Psps.Web.ViewModels.Account
{
    [Validator(typeof(ChangePasswordViewModelValidator))]
    public class ChangePasswordViewModel : BaseViewModel
    {
        public string UserId { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ChangePassword_OldPassword")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ChangePassword_NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ChangePassword_ConfirmNewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
}