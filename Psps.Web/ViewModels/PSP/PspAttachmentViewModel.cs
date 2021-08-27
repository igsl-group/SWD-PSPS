using FluentValidation.Attributes;
using Psps.Core.Helper;
using Psps.Models.Dto.Lookups;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Psps.Web.ViewModels.PSP
{
    //[Validator(typeof(PSPViewModelValidator))]
    public class PspAttachmentViewModel : BaseViewModel
    {
        public int PspAttachmentId { get; set; }

        public int PspMasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FileName")]
        public string FileName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_FileDescription")]
        public string FileDescription { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "PSP_AttachmentFile")]
        public HttpPostedFileBase AttachmentFile { get; set; }

        public byte[] RowVersion { get; set; }
    }
}