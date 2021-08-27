using FluentValidation;
using Psps.Core;
using Psps.Models.Domain;
using Psps.Services.FlagDays;
using Psps.Services.Lookups;
using Psps.Services.Organisations;
using Psps.Services.PSPs;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.PSP;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Psps.Web.Validators
{
    public class PSPViewModelValidator : AbstractValidator<PSPViewModel>
    {
        protected readonly IMessageService _messageService;
        private readonly IOrganisationService _organisationService;
        private readonly IFdEventService _fdEventService;
        private readonly ILookupService _lookupService;
        private readonly IFlagDayListService _flagDayListService;
        private readonly IPspService _pspService;
        private readonly IPspEventService _pspEventService;
        private readonly IPspApprovalHistoryService _pspApprovalHistoryService;

        public PSPViewModelValidator(IMessageService messageService, IOrganisationService organisationService, IFdEventService fdEventService, ILookupService lookupService, IFlagDayListService flagDayListService,
                                    IPspService pspService, IPspEventService pspEventService, IPspApprovalHistoryService pspApprovalHistoryService)
        {
            this._organisationService = organisationService;
            this._fdEventService = fdEventService;
            this._messageService = messageService;
            this._lookupService = lookupService;
            this._flagDayListService = flagDayListService;
            this._pspService = pspService;
            this._pspEventService = pspEventService;
            this._pspApprovalHistoryService = pspApprovalHistoryService;
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var notFoundMsg = _messageService.GetMessage(SystemMessage.Error.NotFound);
            var fromDateLaterThanToDateMsg = _messageService.GetMessage(SystemMessage.Error.FromDateLaterThanToDate);
            var numberNotWithinRangeMsg = _messageService.GetMessage(SystemMessage.Error.NumberNotWithinRange);
            var startDtConFdDtMsg = _messageService.GetMessage(SystemMessage.Error.FlagDay.StartDtConFdDt);
            var endDtConFdDtMsg = _messageService.GetMessage(SystemMessage.Error.FlagDay.EndDtConFdDt);
            var conflictSolicitDateMsg = _messageService.GetMessage(SystemMessage.Error.ConflictSolicitDate);
            var confltSoltDtRngMsg = _messageService.GetMessage(SystemMessage.Error.ConfltSoltDtRng);
            var confltFdDtRngMsg = _messageService.GetMessage(SystemMessage.Error.ConfltFdDtRng);
            var invalidXlsxMsg = _messageService.GetMessage(SystemMessage.Error.InvalidXlsx);
            var prevPspApprovedMsg = _messageService.GetMessage(SystemMessage.Error.Psps.prevPspNotApprov);
            var dupRecTimeOverLapMsg = _messageService.GetMessage(SystemMessage.Error.Psps.DupRecTimeOverLap);
            var startDtLessThanEndDtMsg = _messageService.GetMessage(SystemMessage.Error.Psps.StartDtLessThanEndDt);
            var startTimeEqualEndTimeMsg = _messageService.GetMessage(SystemMessage.Error.Psps.StartTimeSameAsEndTime);
            var mustBeEarlierThanMsg = _messageService.GetMessage(SystemMessage.Error.MustBeEarlierThan);

            var inValidTimeFmtMsg = _messageService.GetMessage(SystemMessage.Error.Psps.InValidTimeFmt);
            string alreadyExistsMsg = _messageService.GetMessage(SystemMessage.Error.AlreadyExists);

            RuleSet("Create", () =>
           {
               RuleFor(x => x.YearofPsp).NotNull().WithMessage(mandatoryMessage);

               RuleFor(x => x.DateofReceivingApplication).NotNull().WithMessage(mandatoryMessage);

               RuleFor(x => x.CreateModelOrgRef).NotNull().WithMessage(mandatoryMessage);

               RuleFor(x => x.CreateModelOrgRef).Must(IfOrgMasterExists).WithMessage(notFoundMsg);

               RuleFor(x => x.DateofEventPeriodFrom).Must(IfEventPeriodFromGtEventPeriodTo).WithMessage(fromDateLaterThanToDateMsg);

               RuleFor(x => x.NewApplicant).Must(ValidateNewApplicant).When(x => x.NewApplicant).WithMessage(alreadyExistsMsg);
           });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.YearofPsp).NotNull().WithMessage(mandatoryMessage);

                RuleFor(x => x.DateofReceivingApplication).NotNull().WithMessage(mandatoryMessage);

                RuleFor(x => x.CreateModelOrgRef).NotNull().WithMessage(mandatoryMessage);

                RuleFor(x => x.CreateModelOrgRef).Must(IfOrgMasterExists).WithMessage(notFoundMsg);

                RuleFor(x => x.GrossProceed).InclusiveBetween(0, 1000000000).WithMessage(numberNotWithinRangeMsg);

                RuleFor(x => x.Expenditure).InclusiveBetween(0, 1000000000).WithMessage(numberNotWithinRangeMsg);

                RuleFor(x => x.NetProceed).InclusiveBetween(-1000000000, 1000000000).WithMessage(numberNotWithinRangeMsg);

                RuleFor(x => x.OrgAnnualIncome).InclusiveBetween(0, 1000000000).WithMessage(numberNotWithinRangeMsg);

                RuleFor(x => x.DateofEventPeriodFrom).Must(IfEventPeriodFromGtEventPeriodTo).WithMessage(fromDateLaterThanToDateMsg);

                RuleFor(x => x.NewApplicant).Must(ValidateNewApplicant).When(x => x.NewApplicant).WithMessage(alreadyExistsMsg);
            });

            RuleSet("EditPSPAccountSummary", () =>
            {
            });

            RuleSet("UpdatePspEvent", () =>
            {
                //Mandatory
                RuleFor(x => x.PspEventViewModel.Location).NotEmpty().WithMessage(mandatoryMessage);

                RuleFor(x => x.PspEventViewModel.District).NotEmpty().WithMessage(mandatoryMessage);

                RuleFor(x => x.PspEventViewModel).SetValidator(new PspEventValidator(_messageService, _pspEventService))
                                                 .DependentRules(d =>
                                                 {
                                                     d.RuleFor(x => x.PspEventViewModel.EventStartTime)
                                                         .Must(IsStartDtLessThanEndDt).WithMessage(startDtLessThanEndDtMsg)
                                                         .NotEqual(x => x.PspEventViewModel.EventEndTime).WithMessage(startTimeEqualEndTimeMsg)
                                                         .DependentRules(c =>
                                                     {
                                                         c.RuleFor(x => x.PspEventViewModel.EventStartDate).Must(RngConfltSolChk).WithMessage("{0}", x => "{0}{1}".FormatWith(confltSoltDtRngMsg, x.BypassValidation ? " (Bypassed)" : ""));
                                                         c.RuleFor(x => x.PspEventViewModel.EventStartDate).Must(RngConfltFdChk).WithMessage("{0}", x => "{0}{1}".FormatWith(confltFdDtRngMsg, x.BypassValidation ? " (Bypassed)" : ""));
                                                         c.RuleFor(x => x.PspEventViewModel.EventStartDate).Must(HasEveDupAndTimeOverlap).WithMessage("{0}", x => "{0}{1}".FormatWith(dupRecTimeOverLapMsg, x.BypassValidation ? " (Bypassed)" : ""));
                                                          c.RuleFor(x => x.PspEventViewModel.EventEndDate).Must(x => false)
                                                             .When(m => {
                                                                 m.YearofPsp = this._pspService.GetPSPById(m.PspMasterId).PspYear;
                                                                 return m.PspEventViewModel.EventEndDate.Value.Year > int.Parse(m.YearofPsp) + 2;
                                                             })
                                                             .WithMessage(mustBeEarlierThanMsg, x => "{0}{1}".FormatWith(new DateTime(int.Parse(x.YearofPsp) + 2, 12, 31).AddDays(1).ToString("dd/MM/yyyy"), x.BypassValidation ? " (Bypassed)" : ""));
                                                         //c.RuleFor(x => x.PspEventViewModel.EventStartDate).Must(RngConfltSolChk).WithMessage("{0}{1}".FormatWith(confltSoltDtRngMsg));
                                                         //c.RuleFor(x => x.PspEventViewModel.EventStartDate).Must(RngConfltFdChk).When(x => !_pspService.getByPassVal(x.PspMasterId)).WithMessage(confltFdDtRngMsg);
                                                         //c.RuleFor(x => x.PspEventViewModel.EventStartDate).Must(HasEveDupAndTimeOverlap).When(x => !_pspService.getByPassVal(x.PspMasterId)).WithMessage(dupRecTimeOverLapMsg);

                                                         //// Bypass Cases
                                                         //c.RuleFor(x => x.PspEventViewModel.EventStartDate).Must(RngConfltSolChk).When(x => _pspService.getByPassVal(x.PspMasterId)).WithMessage(confltSoltDtRngMsg + " (Bypassed)");
                                                         //c.RuleFor(x => x.PspEventViewModel.EventStartDate).Must(RngConfltFdChk).When(x => _pspService.getByPassVal(x.PspMasterId)).WithMessage(confltFdDtRngMsg + " (Bypassed)");
                                                         //c.RuleFor(x => x.PspEventViewModel.EventStartDate).Must(HasEveDupAndTimeOverlap).When(x => _pspService.getByPassVal(x.PspMasterId)).WithMessage(dupRecTimeOverLapMsg + " (Bypassed)");
                                                     });
                                                 });
            });

            RuleSet("CreatePspEvent", () =>
            {
                RuleFor(x => x.PspEventViewModel.Location).NotEmpty().WithMessage(mandatoryMessage);

                RuleFor(x => x.PspEventViewModel.District).NotEmpty().WithMessage(mandatoryMessage);

                RuleFor(x => x.PspEventViewModel).SetValidator(new PspEventValidator(_messageService, _pspEventService))
                                                 .DependentRules(d =>
                                                 {
                                                     d.RuleFor(x => x.PspEventViewModel.EventStartTime)
                                                         .Must(IsStartDtLessThanEndDt).WithMessage(startDtLessThanEndDtMsg)
                                                         .NotEqual(x => x.PspEventViewModel.EventEndTime).WithMessage(startTimeEqualEndTimeMsg)
                                                         .DependentRules(c =>
                                                     {
                                                         c.RuleFor(x => x.PspEventViewModel.EventStartDate).Must(RngConfltSolChk).WithMessage("{0}", x => "{0}{1}".FormatWith(confltSoltDtRngMsg, x.BypassValidation ? " (Bypassed)" : ""));
                                                         c.RuleFor(x => x.PspEventViewModel.EventStartDate).Must(RngConfltFdChk).WithMessage("{0}", x => "{0}{1}".FormatWith(confltFdDtRngMsg, x.BypassValidation ? " (Bypassed)" : ""));
                                                         c.RuleFor(x => x.PspEventViewModel.EventStartDate).Must(HasEveDupAndTimeOverlap).WithMessage("{0}", x => "{0}{1}".FormatWith(dupRecTimeOverLapMsg, x.BypassValidation ? " (Bypassed)" : ""));
                                                         c.RuleFor(x => x.PspEventViewModel.EventEndDate).Must(x => false)
                                                             .When(m => {
                                                                 m.YearofPsp = this._pspService.GetPSPById(m.PspMasterId).PspYear;
                                                                 return m.PspEventViewModel.EventEndDate.Value.Year > int.Parse(m.YearofPsp) + 2;
                                                             })
                                                             .WithMessage(mustBeEarlierThanMsg, x => "{0}{1}".FormatWith(new DateTime(int.Parse(x.YearofPsp) + 2, 12, 31).AddDays(1).ToString("dd/MM/yyyy"), x.BypassValidation ? " (Bypassed)" : ""));
                                                         // Bypass Cases
                                                         //c.RuleFor(x => x.PspEventViewModel.EventStartDate).Must(RngConfltSolChk).When(x => _pspService.getByPassVal(x.PspMasterId)).WithMessage(confltSoltDtRngMsg + " (Bypassed)");
                                                         //c.RuleFor(x => x.PspEventViewModel.EventStartDate).Must(RngConfltFdChk).When(x => _pspService.getByPassVal(x.PspMasterId)).WithMessage(confltFdDtRngMsg + " (Bypassed)");
                                                         //c.RuleFor(x => x.PspEventViewModel.EventStartDate).Must(HasEveDupAndTimeOverlap).When(x => _pspService.getByPassVal(x.PspMasterId)).WithMessage(dupRecTimeOverLapMsg + " (Bypassed)");
                                                     });
                                                 });
            });

            RuleSet("SplitEvent", () =>
            {
                RuleFor(x => x.PspEventViewModel.EventEndDate).NotEmpty().WithMessage(mandatoryMessage);

                RuleFor(x => x.PspEventViewModel.EventEndDate).Must(ValidateEventDate).When(x => x.PspEventViewModel.EventEndDate != null).WithMessage("New Event End date must later or equal to original event start date and before original event end date");
            });

            RuleSet("ImportProforma", () =>
            {
                RuleFor(x => x.PspEventViewModel.ImportFile).NotNull().WithMessage(mandatoryMessage);
                RuleFor(x => x.PspEventViewModel.ImportFile).Must(IsXlsxFile).WithMessage(invalidXlsxMsg);
            });

            RuleSet("ImportPsp", () =>
            {
                RuleFor(x => x.ImportFile).NotNull().WithMessage(mandatoryMessage);
                RuleFor(x => x.ImportFile).Must(IsXlsxFile).WithMessage(invalidXlsxMsg);
            });

            RuleSet("CreatePspAttachment", () =>
            {
                RuleFor(m => m.PspAttachmentViewModel.FileName).NotNull().WithMessage(mandatoryMessage);
                RuleFor(m => m.PspAttachmentViewModel.FileDescription).NotNull().WithMessage(mandatoryMessage);
                RuleFor(m => m.PspAttachmentViewModel.AttachmentFile).NotNull().WithMessage(mandatoryMessage);
            });

            RuleSet("UpdatePspAttachment", () =>
            {
                RuleFor(m => m.PspAttachmentViewModel.FileName).NotNull().WithMessage(mandatoryMessage);
                RuleFor(m => m.PspAttachmentViewModel.FileDescription).NotNull().WithMessage(mandatoryMessage);
                //RuleFor(m => m.PspAttachmentViewModel.AttachmentFile).NotNull().WithMessage(mandatoryMessage);
            });

            RuleSet("RecommendToApprove", () =>
            {
                RuleFor(x => x.PrevPspMasterId).Must(PrevPspIsApproved).WithMessage(prevPspApprovedMsg);
            });
        }

        protected bool ValidateEventDate(PSPViewModel model, DateTime? eventEndDate)
        {
            var eve = _pspEventService.GetPspEventById((int)model.PspEventViewModel.PspEventId);

            if (eventEndDate.Value < eve.EventStartDate || eventEndDate.Value >= eve.EventEndDate)
                return false;

            return true;
        }

        protected bool ValidateNewApplicant(PSPViewModel model, bool newApplicant)
        {
            return !this._pspService.NewApplicantExists(model.CreateModelOrgRef, model.PspMasterId);
        }

        protected bool ValidateFirstReminderIssuedMustEarlierThanDeadline(PSPViewModel model, string firstReminderIssueDateStr)
        {
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime firstReminderIssueDate;
            DateTime.TryParseExact(firstReminderIssueDateStr, "d/M/yyyy", enUS, DateTimeStyles.None, out firstReminderIssueDate);
            DateTime firstReminderDeadline;
            DateTime.TryParseExact(model.FirstReminderDeadline.ToString(), "d/M/yyyy", enUS, DateTimeStyles.None, out firstReminderDeadline);
            if (firstReminderIssueDate > firstReminderDeadline)
            {
                return false;
            }
            return true;
        }

        protected bool ValidateSecondReminderIssuedMustEarlierThanDeadline(PSPViewModel model, string secondReminderIssueDateStr)
        {
            //model.SecondReminderIssueDate
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime secondReminderIssueDate;
            DateTime.TryParseExact(secondReminderIssueDateStr, "d/M/yyyy", enUS, DateTimeStyles.None, out secondReminderIssueDate);
            DateTime SecondReminderDeadline;
            DateTime.TryParseExact(model.SecondReminderDeadline.ToString(), "d/M/yyyy", enUS, DateTimeStyles.None, out SecondReminderDeadline);
            if (secondReminderIssueDate > SecondReminderDeadline)
            {
                return false;
            }
            return true;
        }

        protected bool IfOrgMasterExists(string OrgRef)
        {
            var orgMaster = _organisationService.GetOrgByRef(OrgRef);

            if (orgMaster == null)
            { return false; }
            return true;
        }

        protected bool ChkApproveTypeWhenRecomApproving(PSPViewModel model, bool TwoBatchEx)
        {
            if (model.TwoBatchEx || model.Amendment || model.CancelEvent)
            { return true; }
            else { return false; }
        }

        protected bool IfEventStartDateGtEventEndDate(PSPViewModel model, DateTime? EventEndDate)
        {
            if (EventEndDate < model.PspEventViewModel.EventStartDate)
            {
                return false;
            }

            return true;
        }

        protected bool IfEventPeriodFromGtEventPeriodTo(PSPViewModel model, DateTime? DateofEventPeriodFrom)
        {
            if (model.DateofEventPeriodTo < DateofEventPeriodFrom)
            {
                return false;
            }

            return true;
        }

        protected bool IfFlagDayConflicted(PSPViewModel model, DateTime? EventStartDate)
        {
            var byPass = false;
            if (model.PspMasterId == default(int))
            {
                var eve = _pspEventService.GetPspEventById((int)model.PspEventViewModel.PspEventId);
                byPass = _pspService.getByPassVal(eve.PspMaster.PspMasterId);
            }
            else
            {
                byPass = _pspService.getByPassVal(model.PspMasterId);
            }

            if (!byPass)
            {
                return _flagDayListService.IsFdDateAndTimeConflicit(EventStartDate.Value, model.PspEventViewModel.EventEndDate.Value, model.PspEventViewModel.EventStartTime, model.PspEventViewModel.EventEndTime); // check if start day is conflicted with flagday
            }
            else
            {
                return true;
            }
        }

        protected bool IfSolicitDateConflicted(PSPViewModel model, DateTime? targetDate)
        {
            //var eve = _pspEventService.GetPspEventById((int)model.PspEventViewModel.PspEventId);
            var byPass = _pspService.getByPassVal(model.PspMasterId);

            if ((model.PspEventViewModel.MethodOfCollection != null && model.PspEventViewModel.MethodOfCollection.Any(x => "2".Equals(x))) && targetDate != null && !byPass)
            {
                return !_lookupService.IsSolicitDate((DateTime)targetDate); // check if start day is conflicted with Solicit Date, true to fulfil must function
            }
            else
            {
                return true;
            }
        }

        protected bool RngConfltSolChk(PSPViewModel model, DateTime? eventStartDate)
        {//will check both solict
            //var byPass = false;
            //if (model.PspMasterId == default(int))
            //{
            //    var eve = _pspEventService.GetPspEventById((int)model.PspEventViewModel.PspEventId);
            //    byPass = _pspService.getByPassVal(eve.PspMaster.PspMasterId);
            //}
            //else
            //{
            //    byPass = _pspService.getByPassVal(model.PspMasterId);
            //}

            if ((model.PspEventViewModel.MethodOfCollection != null && model.PspEventViewModel.MethodOfCollection.Any(x => "2".Equals(x))) && eventStartDate != null && model.PspEventViewModel.EventEndDate != null)
            {
                var solicit = _lookupService.EveRngChk((DateTime)eventStartDate, (DateTime)model.PspEventViewModel.EventEndDate);
                return !solicit;
            }
            else
                return true;
        }

        protected bool RngConfltFdChk(PSPViewModel model, DateTime? eventStartDate)
        {//will check both fdlist
            //var byPass = false;
            //if (model.PspMasterId == default(int))
            //{
            //    var eve = _pspEventService.GetPspEventById((int)model.PspEventViewModel.PspEventId);
            //    byPass = _pspService.getByPassVal(eve.PspMaster.PspMasterId);
            //}
            //else
            //{
            //    byPass = _pspService.getByPassVal(model.PspMasterId);
            //}
            if (eventStartDate != null && model.PspEventViewModel.EventEndDate != null)
            {
                var fdResult = _flagDayListService.EveRngChk((DateTime)eventStartDate, model.PspEventViewModel.EventStartTime, (DateTime)model.PspEventViewModel.EventEndDate, model.PspEventViewModel.EventEndTime);
                return fdResult;
            }
            else
                return true;
        }

        protected bool ValidCutOffFromToDt(PSPViewModel model, DateTime? cutOffFrom)
        {
            if (cutOffFrom > model.PspEventViewModel.CutOffDateTo)
            {
                return false;
            }
            else
            {
                return true;
            }
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

        protected bool PrevPspIsApproved(int? prevPspMastId)
        {
            if (prevPspMastId == null)
            {
                return true;
            }
            else
            {
                return _pspApprovalHistoryService.ifPspMasterIsApproved(prevPspMastId);
            }
        }

        protected bool HasEveDupAndTimeOverlap(PSPViewModel model, DateTime? eventStartDate)
        {
            //var byPass = _pspService.getByPassVal(model.PspMasterId);
            //if (!byPass)
            //{
            var startDt = ((DateTime)eventStartDate).AddHours(Convert.ToInt32(model.PspEventViewModel.EventStartTime.Substring(0, 2))).AddMinutes(Convert.ToInt32(model.PspEventViewModel.EventStartTime.Substring(2, 2)));
            var endDt = ((DateTime)model.PspEventViewModel.EventEndDate).AddHours(Convert.ToInt32(model.PspEventViewModel.EventEndTime.Substring(0, 2))).AddMinutes(Convert.ToInt32(model.PspEventViewModel.EventEndTime.Substring(2, 2)));

            return !_pspEventService.HasEveDupAndTimeOverlap(model.PspMasterId, (DateTime)eventStartDate, (DateTime)model.PspEventViewModel.EventEndDate,
                                        startDt, endDt,
                                        model.PspEventViewModel.Location, model.PspEventViewModel.ChiLocation, model.PspEventViewModel.PspEventId);
            //}
            //else
            //    return true;
        }

        protected bool IsStartDtLessThanEndDt(PSPViewModel model, string inStartTime)
        {
            var startDt = ((DateTime)model.PspEventViewModel.EventStartDate).AddHours(Convert.ToInt32(model.PspEventViewModel.EventStartTime.Substring(0, 2))).AddMinutes(Convert.ToInt32(model.PspEventViewModel.EventStartTime.Substring(2, 2)));
            var endDt = ((DateTime)model.PspEventViewModel.EventStartDate).AddHours(Convert.ToInt32(model.PspEventViewModel.EventEndTime.Substring(0, 2))).AddMinutes(Convert.ToInt32(model.PspEventViewModel.EventEndTime.Substring(2, 2)));

            if (startDt > endDt)
            {
                return false;
            }
            else
                return true;
        }

        protected bool IsValidTimeFormat(PSPViewModel model, string time)
        {
            if (_pspEventService.IsValidHrAndMin(time))
                return true;
            return false;
        }
    }

    public class PspEventValidator : AbstractValidator<PspEventViewModel>
    {
        protected readonly IMessageService _messageService;
        protected readonly IPspEventService _pspEventService;

        public PspEventValidator(IMessageService messageService, IPspEventService pspEventService)
        {
            this._messageService = messageService;
            this._pspEventService = pspEventService;

            string mandatoryMessage = _messageService.GetMessage(SystemMessage.Error.Mandatory);
            string fromDateLaterThanToDateMsg = _messageService.GetMessage(SystemMessage.Error.FromDateLaterThanToDate);
            string inValidTimeFmtMsg = _messageService.GetMessage(SystemMessage.Error.Psps.InValidTimeFmt);

            RuleFor(x => x.EventStartDate).NotEmpty().WithMessage(mandatoryMessage);
            RuleFor(x => x.EventEndDate).NotEmpty().WithMessage(mandatoryMessage);

            RuleFor(x => x.EventEndDate).Cascade(FluentValidation.CascadeMode.StopOnFirstFailure)
                                        .NotEmpty().WithMessage(mandatoryMessage)
                                        .GreaterThanOrEqualTo(x => x.EventStartDate).When(x => x.EventStartDate.HasValue).WithMessage(fromDateLaterThanToDateMsg);

            RuleFor(x => x.EventStartTime).Cascade(FluentValidation.CascadeMode.StopOnFirstFailure)
                                          .NotEmpty().WithMessage(mandatoryMessage)
                                          .Must(IsValidTimeFormat).WithMessage(inValidTimeFmtMsg);

            RuleFor(x => x.EventEndTime).Cascade(FluentValidation.CascadeMode.StopOnFirstFailure)
                                        .NotEmpty().WithMessage(mandatoryMessage)
                                        .Must(IsValidTimeFormat).WithMessage(inValidTimeFmtMsg);
        }

        protected bool IsValidTimeFormat(string time)
        {
            if (_pspEventService.IsValidHrAndMin(time))
                return true;
            return false;
        }
    }
}