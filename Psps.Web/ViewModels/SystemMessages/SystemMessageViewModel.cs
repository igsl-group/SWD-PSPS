using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System.ComponentModel.DataAnnotations;

namespace Psps.Web.ViewModels.SystemMessages
{
    [Validator(typeof(SystemMessageViewModelValidator))]
    public class SystemMessageViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "SystemMessage_SystemMessageId")]
        public int SystemMessageId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "SystemParameter_Code")]
        public string Code { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "SystemMessage_Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "SystemMessage_Value")]
        public string Value { get; set; }

        public byte[] RowVersion { get; set; }
    }
}