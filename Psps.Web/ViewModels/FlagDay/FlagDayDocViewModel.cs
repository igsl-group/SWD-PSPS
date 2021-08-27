using FluentValidation.Attributes;
using Psps.Core.Helper;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Psps.Web.ViewModels.FlagDay
{
    [Validator(typeof(FlagDayDocViewModelValidator))]
    public class FlagDayDocViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Letter_LetterId")]
        public int FlagDayDocId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Letter_LetterId")]
        public string DocNum { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Letter_Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Letter_Path")]
        public string Path { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Letter_Version")]
        public string Version { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Letter_IsActive")]
        public bool IsActive { get; set; }

        public byte[] RowVersion { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Letter_Path")]
        public HttpPostedFileBase File { get; set; }
    }
}