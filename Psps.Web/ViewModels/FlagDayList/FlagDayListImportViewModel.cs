using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Psps.Web.ViewModels.FlagDayList
{
    //[Validator(typeof(FlagDayListViewModelValidator))]
    public partial class FlagDayListImportViewModel : BaseViewModel
    {
 
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "ImportXlsFile")]
        public HttpPostedFileBase ImportFile { get; set; }

    }
}