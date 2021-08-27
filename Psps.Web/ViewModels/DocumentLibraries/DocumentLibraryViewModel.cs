using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Psps.Web.ViewModels.DocumentLibraries
{
    [Validator(typeof(DocumentLibraryViewModelValidator))]
    public class DocumentLibraryViewModel : BaseViewModel
    {
        public DocumentLibraryViewModel()
        {
            DocumentLibraries = new Dictionary<int, string>();
            Document = new DocumentViewModel();
        }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "DocumentLibrary_Folder")]
        public int? SelectedDocumentLibraryId { get; set; }

        public Dictionary<int, string> DocumentLibraries { get; set; }

        public string Name { get; set; }

        public DocumentViewModel Document { get; set; }
    }
}