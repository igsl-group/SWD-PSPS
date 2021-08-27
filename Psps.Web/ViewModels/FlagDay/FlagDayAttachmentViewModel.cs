using FluentValidation.Attributes;
using Psps.Core.Helper;
using Psps.Models.Dto.Lookups;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Psps.Web.ViewModels.FlagDay
{
    //[Validator(typeof(FlagDayDocViewModelValidator))]
    public class FlagDayAttachmentViewModel : BaseViewModel
    {
        public int FdAttachmentId { get; set; }

        public int FdMasterId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FileName")]
        public string FileName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_FileDescription")]
        public string FileDescription { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "FD_AttachmentFile")]
        public HttpPostedFileBase AttachmentFile { get; set; }

        public byte[] RowVersion { get; set; }
    }
}