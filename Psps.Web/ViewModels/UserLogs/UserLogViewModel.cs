using FluentValidation.Attributes;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Psps.Web.ViewModels.UserLogs
{
    [Validator(typeof(UserLogViewModelValidator))]
    public class UserLogViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "UserLog_UserLogId")]
        public int LogId { get; set; }

        public string RecordKey { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "UserLog_EngUserName")]
        public string EngUserName { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "UserLog_Activity")]
        public string Activity { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "UserLog_Action")]
        public string Action { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "UserLog_Remark")]
        public string Remark { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "UserLog_ActionedOn")]
        public DateTime ActionedOn { get; set; }

        public IDictionary<string, string> Actions { get; set; }

        public byte[] RowVersion { get; set; }
    }
}