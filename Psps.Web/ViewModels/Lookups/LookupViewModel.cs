using Autofac.Integration.Mvc;
using FluentValidation.Attributes;
using Psps.Core;
using Psps.Core.Helper;
using Psps.Models.Dto.Lookups;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Psps.Web.ViewModels.Lookup
{
    [Validator(typeof(LookupViewModelValidator))]
    public partial class LookupViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Lookup_Type")]
        public LookupType? SelectedType { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Lookup_Type")]
        public LookupType Type { get; set; }

        public int? LookupId { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Lookup_Code")]
        public string Code { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Lookup_EngDescription")]
        public string EngDescription { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Lookup_ChiDescription")]
        public string ChiDescription { get; set; }

        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Lookup_DisplayOrder")]
        public int DisplayOrder { get; set; }

        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Psps.Resources.Labels), Name = "Lookup_IsActive")]
        public bool IsActive { get; set; }

        // TIR#: PSUAT00035-3  Importing or amending “No Solicitation Date” is assigned to PSP Approver only.
        public bool IsPspApprover { get; set; }

        public byte[] RowVersion { get; set; }
    }

    [ModelBinderType(typeof(LookupViewModel))]
    public class LookupViewModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            var request = controllerContext.HttpContext.Request;
            var model = bindingContext.Model as LookupViewModel;

            if (model != null)
            {
                if (propertyDescriptor.Name == "SelectedType" && request["SelectedType"] != null)
                {
                    model.SelectedType = request["SelectedType"].ToString().ToEnum<LookupType>();
                }

                if (propertyDescriptor.Name == "Type" && request["Type"] != null)
                {
                    model.Type = request["Type"].ToString().ToEnum<LookupType>();
                }
            }

            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}