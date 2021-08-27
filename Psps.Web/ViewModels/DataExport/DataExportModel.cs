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

namespace Psps.Web.ViewModels.DataExport
{
    //[Validator(typeof(DataExportViewModelValidator))]
    public partial class DataExportViewModel : BaseViewModel
    {
        /// <summary>
        /// dropdown
        /// </summary>
        public IDictionary<string, string> Tables { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "TablesToBeExport")]
        public string[] TablesToBeExport { get; set; }
    }
}