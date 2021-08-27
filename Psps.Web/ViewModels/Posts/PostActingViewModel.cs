using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Psps.Web.ViewModels.Posts
{
    [Validator(typeof(PostViewModelValidator<PostViewModel>))]
    public class PostActingViewModel : PostViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "User_EngUserName")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Effective_From")]
        public DateTime EffectiveFrom { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Effective_To")]
        public DateTime EffectiveTo { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "User_EngUserName")]
        public string AssignTo { get; set; }

        public byte[] ActingRowVersion { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Acting_ActingId")]
        public int? ActingId { get; set; }

        /// <summary>
        /// dropdown
        /// </summary>
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Post_Assign_to")]
        public IDictionary<string, string> AssignTos { get; set; }
    }

    [Validator(typeof(CreatePostActingViewModelValidator))]
    public partial class CreatePostActingViewModel : PostActingViewModel
    {
    }

    [Validator(typeof(UpdatePostActingViewModelValidator))]
    public partial class UpdatePostActingViewModel : PostActingViewModel
    {
    }
}