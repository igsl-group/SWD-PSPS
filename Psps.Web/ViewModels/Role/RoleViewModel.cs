using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Psps.Web.ViewModels.Role
{
    public class RoleViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Role_RoleId")]
        public string RoleId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Role_RoleId")]
        public string DropdownRoleId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Role_RoleId")]
        public IDictionary<string, string> Roles { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Role_RoleId")]
        public string RoleName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Role_Post")]
        public IDictionary<string, string> Posts { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Role_Post")]
        public string RolePostId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Role_Action")]
        public string RoleAction { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Role_Module")]
        public string RoleModule { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Role_Description")]
        public string RoleDescription { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Role_Enabled")]
        public string RoleEnabled { get; set; }

        public byte[] RowVersion { get; set; }
    }

    [Validator(typeof(CreateRoleViewModelValidator))]
    public partial class CreateRoleViewModel : RoleViewModel
    {

    }

    [Validator(typeof(CreatePostsInRolesViewModelValidator))]
    public partial class CreatePostsInRolesViewModel : RoleViewModel
    {

    }

}