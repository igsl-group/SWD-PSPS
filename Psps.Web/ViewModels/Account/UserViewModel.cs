using FluentValidation.Attributes;
using Psps.Core.Helper;
using Psps.Models.Dto.Lookups;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Psps.Web.ViewModels.Account
{
    [Validator(typeof(UserViewModelValidator))]
    public partial class UserViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "User_UserId")]
        public string UserId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "User_Post")]
        public string PostId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "User_Post")]
        public IDictionary<string, string> Posts { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "User_EngUserName")]
        public string EngUserName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "User_ChiUserName")]
        public string ChiUserName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "User_TelephoneNumber")]
        public string TelephoneNumber { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "User_Email")]
        public string Email { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "User_IsSystemAdministrator")]
        public bool IsSystemAdministrator { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "User_IsActive")]
        public bool IsActive { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "User_Password")]
        public string Password { get; set; }

        public byte[] RowVersion { get; set; }
    }
}