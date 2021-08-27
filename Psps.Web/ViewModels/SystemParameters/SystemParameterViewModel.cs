using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System.ComponentModel.DataAnnotations;

namespace Psps.Web.ViewModels.SystemParameters
{
    [Validator(typeof(SystemParameterViewModelValidator))]
    public class SystemParameterViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "SystemParameter_SystemParameterId")]
        public int SystemParameterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "SystemParameter_Code")]
        public string Code { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "SystemParameter_Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "SystemParameter_Value")]
        public string Value { get; set; }

        public byte[] RowVersion { get; set; }
    }
}