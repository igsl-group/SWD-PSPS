using FluentValidation;
using Psps.Core;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Services.FlagDays;
using Psps.Services.Lookups;
using Psps.Services.Organisations;
using Psps.Services.PSPs;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.FlagDay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Psps.Web.Validators
{
    public class FlagDayViewModelValidator : AbstractValidator<FlagDayViewModel>
    {
        protected readonly IOrganisationService _organisationService;
        protected readonly IMessageService _messageService;
        protected readonly IFlagDayListService _flagDayListService;
        protected readonly IFlagDayService _flagDayService;
        protected readonly IFdEventService _fdEventService;
        protected readonly IFDMasterRepository _fDMasterRepository;
        protected readonly IPspEventService _pspEventService;
        protected readonly ILookupService _lookupService;

        public FlagDayViewModelValidator(IMessageService messageService, IOrganisationService organisationService, IFlagDayListService flagDayListService,
                                         IFlagDayService flagDayService, IFdEventService fdEventService, IFDMasterRepository fDMasterRepository, IPspEventService pspEventService, ILookupService lookupService)
        {
            _organisationService = organisationService;
            _messageService = messageService;
            _flagDayListService = flagDayListService;
            _flagDayService = flagDayService;
            _fdEventService = fdEventService;
            _fDMasterRepository = fDMasterRepository;
            _pspEventService = pspEventService;
            _lookupService = lookupService;

            string mandatoryMessage = _messageService.GetMessage(SystemMessage.Error.Mandatory);

            RuleSet("Create", () =>
            {
                SharedRules();
                RuleFor(m => m.CreateModelOrgRef).Must(ChkOneOrgToFdEvent).WithMessage(_messageService.GetMessage(SystemMessage.Error.FlagDay.OnlyOneFdMasterIsAllowForEachOrg));
            });

            RuleSet("Update", () =>
            {
                SharedRules();
            });

            RuleSet("CreateFdAttachment", () =>
            {
                RuleFor(m => m.FlagDayAttachmentViewModel.FileName).NotNull().WithMessage(mandatoryMessage);
                RuleFor(m => m.FlagDayAttachmentViewModel.FileDescription).NotNull().WithMessage(mandatoryMessage);
                RuleFor(m => m.FlagDayAttachmentViewModel.AttachmentFile).NotNull().WithMessage(mandatoryMessage);
            });

            RuleSet("UpdateFdAttachment", () =>
            {
                RuleFor(m => m.FlagDayAttachmentViewModel.FileName).NotNull().WithMessage(mandatoryMessage);
                RuleFor(m => m.FlagDayAttachmentViewModel.FileDescription).NotNull().WithMessage(mandatoryMessage);
            });

            RuleSet("CreateFdEvent", () =>
            {
                SharedEventRules();
            });

            RuleSet("UpdateFdEvent", () =>
            {
                SharedEventRules();
            });

            RuleSet("ImportFlagDay", () =>
            {
                RuleFor(x => x.ImportFile).NotNull().WithMessage(mandatoryMessage);
                RuleFor(x => x.ImportFile).Must(IsXlsxFile).WithMessage(_messageService.GetMessage(SystemMessage.Error.InvalidXlsx));
            });
        }

        #region Collection of Rules

        protected void SharedRules()
        {
            string alreadyExistsMsg = _messageService.GetMessage(SystemMessage.Error.AlreadyExists);

            RuleFor(x => x.YearofFlagDay).NotNull().WithMessage(_messageService.GetMessage(SystemMessage.Error.Mandatory));

            RuleFor(x => x.TWR).NotNull().WithMessage(_messageService.GetMessage(SystemMessage.Error.Mandatory));

            RuleFor(x => x.DateofReceivingApplication).NotNull().WithMessage(_messageService.GetMessage(SystemMessage.Error.Mandatory));

            RuleFor(x => x.CreateModelOrgRef).Must(IfOrgMasterExists).WithMessage(SystemMessage.Error.NotFound);

            RuleFor(x => x.TargetIncome).InclusiveBetween(0, 1000000000).WithMessage(SystemMessage.Error.NumberNotWithinRange);

            RuleFor(x => x.PercentageForGrouping).InclusiveBetween(0, 10000).WithMessage(SystemMessage.Error.NumberNotWithinRange);

            RuleFor(x => x.PledgedAmt).InclusiveBetween(0, 1000000000).WithMessage(SystemMessage.Error.NumberNotWithinRange);

            RuleFor(x => x.PledgingAmt).InclusiveBetween(0, 1000000000).WithMessage(SystemMessage.Error.NumberNotWithinRange);

            RuleFor(x => x.CreateModelOrgRef).NotNull().WithMessage(_messageService.GetMessage(SystemMessage.Error.Mandatory));

            RuleFor(x => x.DateofReceivingApplication).Must(IfDateGtToday).WithMessage(_messageService.GetMessage(SystemMessage.Error.FromDateLaterThanToDate));

            RuleFor(x => x.CreateModelNewApplicant).Must(ValidateNewApplicant).When(x => x.CreateModelNewApplicant).WithMessage(alreadyExistsMsg);
        }

        protected void SharedEventRules()
        {
            string mandatoryMessage = _messageService.GetMessage(SystemMessage.Error.Mandatory);
            string regionalMustHaveRegion = _messageService.GetMessage(SystemMessage.Error.FlagDay.RegionalMustHaveRegion);
            string inValidTimeFmtMsg = _messageService.GetMessage(SystemMessage.Error.Psps.InValidTimeFmt);
            string startDtLessThanEndDt = _messageService.GetMessage(SystemMessage.Error.Psps.StartDtLessThanEndDt);

            RuleFor(m => m.FlagDay).NotEmpty().WithMessage(mandatoryMessage)
                                   .DependentRules(d =>
                                   {
                                       d.RuleFor(m => m.FlagDay).Must(MatchFdList).WithMessage(_messageService.GetMessage(SystemMessage.Error.FlagDay.FlagdayDayNotMatch));
                                       d.RuleFor(m => m.FlagDay).Must(AvaliableFlagDay).WithMessage(_messageService.GetMessage(SystemMessage.Error.FlagDay.FlagdayDayNotAvaliable));
                                       d.RuleFor(m => m.FlagDay).Must(IfSolicitDateConflicted).WithMessage(_messageService.GetMessage(SystemMessage.Error.FlagDay.StartDayConflict));
                                   });

            RuleFor(m => m.FlagTimeFrom).Cascade(FluentValidation.CascadeMode.StopOnFirstFailure)
                                        .NotEmpty().WithMessage(mandatoryMessage)
                                        .Must(IsValidTimeFormat).WithMessage(_messageService.GetMessage(SystemMessage.Error.Psps.InValidTimeFmt));

            RuleFor(m => m.FlagTimeTo).Cascade(FluentValidation.CascadeMode.StopOnFirstFailure)
                                      .NotEmpty().WithMessage(mandatoryMessage)
                                      .Must(IsValidTimeFormat).WithMessage(_messageService.GetMessage(SystemMessage.Error.Psps.InValidTimeFmt));

            RuleFor(m => m.FlagTimeFrom).LessThan(m => m.FlagTimeTo).When(m => m.FlagTimeFrom.IsNotNullOrEmpty() && m.FlagTimeTo.IsNotNullOrEmpty()).WithMessage(startDtLessThanEndDt);

            RuleFor(m => m.EventTWR).NotEmpty().WithMessage(mandatoryMessage);
            RuleFor(m => m.District).Must(RegionalDistrictMandatory).When(x => x.EventTWR.IsNotNullOrEmpty()).WithMessage(regionalMustHaveRegion);
        }

        #endregion Collection of Rules

        #region Custom validation rules

        protected bool IsValidTimeFormat(string time)
        {
            return _pspEventService.IsValidHrAndMin(time);
        }

        protected bool ValidateNewApplicant(FlagDayViewModel model, bool newApplicant)
        {
            return !this._flagDayService.NewApplicantExists(model.CreateModelOrgRef, model.FdMasterId);
        }

        protected bool IfOrgMasterExists(string OrgRef)
        {
            var orgMaster = _organisationService.GetOrgByRef(OrgRef);

            if (orgMaster == null)
            { return false; }
            return true;
        }

        protected bool IfDateGtToday(DateTime? varDate)
        {
            if (varDate > DateTime.Today)
            {
                return false;
            }

            return true;
        }

        protected bool IsXlsxFile(HttpPostedFileBase ImportFile)
        {
            if (ImportFile != null)
            {
                if (ImportFile.FileName.Substring(ImportFile.FileName.Length - 4, 4) == "xlsx")
                {
                    return true;
                }
            }
            return false;
        }

        protected bool RegionalDistrictMandatory(FlagDayViewModel model, string district)
        {
            if (model.EventTWR == "2") //2 regional
            {
                if (string.IsNullOrEmpty(district))
                {
                    return false;
                }
            }
            return true;
        }

        protected bool MatchFdList(FlagDayViewModel model, DateTime? varDate)
        {
            if (string.IsNullOrEmpty(model.EventTWR))
                return true;
            else
                return _flagDayListService.MatchFlagDayInFdList(varDate.Value, model.EventTWR, model.YearofFlagDay); // true matched
        }

        protected bool AvaliableFlagDay(FlagDayViewModel model, DateTime? varDate)
        {
            if (string.IsNullOrEmpty(model.EventTWR))
                return true;
            else
            {
                string district = string.IsNullOrEmpty(model.District) ? "" : model.District;
                return _fdEventService.AvaliableFlagDay(varDate.Value, model.EventTWR, district, model.YearofFlagDay, model.FdEventId); // true avaliable
            }
        }

        protected bool ChkOneOrgToFdEvent(FlagDayViewModel model, string orgRef)
        {
            if (string.IsNullOrEmpty(orgRef))
                return true;
            else
            {
                return _fDMasterRepository.ifOrgMasterHasFdevent(orgRef, model.YearofFlagDay); // org one to one fdMaster
            }
        }

        protected bool IfSolicitDateConflicted(DateTime? EventStartDate)
        {
            if (EventStartDate != null)
            {
                return !_lookupService.IsSolicitDate(EventStartDate.Value); // check if start day is conflicted with flagday
            }
            else
            {
                return true;
            }
        }

        #endregion Custom validation rules
    }
}