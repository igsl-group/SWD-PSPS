using Autofac.Integration.Mvc;
using FluentValidation.Attributes;
using Psps.Core;
using Psps.Core.Helper;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Psps.Web.ViewModels.Posts
{
    [Validator(typeof(PostViewModelValidator<PostViewModel>))]
    public partial class PostViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Post_Post")]
        public string PostId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Post_Rank")]
        public string Rank { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Post_Owner")]
        public string Owner { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Post_Supervisor")]
        public string Supervisor { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Post_Role")]
        public string[] Role { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Post_Role")]
        public string[] PostsInRoles { get; set; }

        /// <summary>
        /// dropdown
        /// </summary>
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Post_Rank")]
        public IDictionary<string, string> Ranks { get; set; }

        /// <summary>
        /// dropdown
        /// </summary>
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Post_Owner")]
        public IDictionary<string, string> Users { get; set; }

        /// <summary>
        /// dropdown
        /// </summary>
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Post_Supervisor")]
        public IDictionary<string, string> Posts { get; set; }

        /// <summary>
        /// dropdown
        /// </summary>
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Post_Role")]
        public IDictionary<string, string> Roles { get; set; }

        public byte[] RowVersion { get; set; }
    }

    [Validator(typeof(CreatePostViewModelValidator))]
    public partial class CreatePostViewModel : PostViewModel
    {
    }

    [Validator(typeof(UpdatePostViewModelValidator))]
    public partial class UpdatePostViewModel : PostViewModel
    {
    }
}