using FluentValidation.Attributes;
using Psps.Core.Helper;
using Psps.Models.Dto.Lookups;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Psps.Web.ViewModels.Organisation
{
    [Validator(typeof(OrgNameChangeHistoryViewModelValidator))]
    public class OrgNameChangeHistoryViewModel : BaseViewModel
    {
        public string OrgNameChangeId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "OrgNameChangeHistory_ChangeDate")]
        public string OrgNameChangeDate { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "OrgNameChangeHistory_EngOrgName")]
        public string OrgNameChangeEngOrgName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "OrgNameChangeHistory_ChiOrgName")]
        public string OrgNameChangeChiOrgName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "OrgNameChangeHistory_Remarks")]
        public string OrgNameChangeRemarks { get; set; }
    }
}