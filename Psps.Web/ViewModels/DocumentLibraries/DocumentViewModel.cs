using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Psps.Web.ViewModels.DocumentLibraries
{
    [Validator(typeof(DocumentViewModelValidator))]
    public class DocumentViewModel : BaseViewModel
    {
        public int? DocumentId { get; set; }

        public int DocumentLibraryId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Document_Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Document_Remark")]
        public string Remark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Document_UploadedBy")]
        public string UploadedBy { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Document_UploadedOn")]
        public DateTime UploadedOn { get; set; }

        public byte[] RowVersion { get; set; }

        public HttpPostedFileBase Document { get; set; }
    }
}