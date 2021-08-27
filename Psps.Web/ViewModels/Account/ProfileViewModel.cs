using Autofac.Integration.Mvc;
using FluentValidation.Attributes;
using Psps.Core;
using Psps.Core.Helper;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Psps.Web.ViewModels.Account
{
    [Validator(typeof(ProfileViewModelValidator))]
    public class ProfileViewModel : BaseViewModel
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

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "User_Password")]
        public string Password { get; set; }

        public byte[] RowVersion { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Post_Role")]
        public IDictionary<string, string> Roles { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Post_Role")]
        public string[] Role { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Profile_AssignTo")]
        public IDictionary<string, string> AssignTos { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Profile_AssignTo")]
        public string AssignTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Effective_From")]
        public DateTime? EffectiveFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Effective_To")]
        public DateTime? EffectiveTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Action")]
        public string Action { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Acting_ActingId")]
        public int? ActingId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Profile_AssignTo_UserId")]
        public string AssignToUserId { get; set; }
    }
}