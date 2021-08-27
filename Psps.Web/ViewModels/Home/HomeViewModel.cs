using FluentValidation.Attributes;
using Psps.Models.Dto.Complaint;
using Psps.Models.Dto.SuggestionMaster;
using Psps.Web.Core.Mvc;
using Psps.Web.Framework.Mvc;
using Psps.Web.Validators;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Psps.Web.ViewModels.Home
{
    [Validator(typeof(HomeViewModelValidator))]
    public class HomeViewModel : BaseViewModel
    {
        public int? Count { get; set; }

        public IDictionary<string, string> DynCompEnqHeaders { get; set; }

        public IList<CompEnqDto> CompEnqRowData { get; set; }

        public IDictionary<string, string> SuggestionHeaders { get; set; }

        public IList<SuggestDto> SuggestionRowData { get; set; }

        public IDictionary<string, string> PostIdToBeActed { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Home_ChangedPostId")]
        public string ChangedPostId { get; set; }
    }
}