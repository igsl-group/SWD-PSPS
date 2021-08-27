using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.FlagDays;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.FlagDayList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Psps.Web.Validators
{
    public class FlagDayListViewModelValidator : AbstractValidator<FlagDayListViewModel>
    {
        protected readonly IFlagDayListService _FdListService;

        public FlagDayListViewModelValidator(IMessageService messageService, IFlagDayListService fdListService)
        {
            _FdListService = fdListService;

            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            var uniqueMessage = messageService.GetMessage(SystemMessage.Error.Unique);

            //default rule
            //RuleFor(x => x.FlagDayListId).NotNull().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));
            RuleFor(x => x.FlagDayDate).NotNull().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));
            var fdDateErrorMsg = messageService.GetMessage(SystemMessage.Error.FlagDay.fdDateError);
            RuleFor(x => x.FlagDayDate).Must(ValidateFlagDayDate).When(x => x.FlagDayDate != null).WithMessage(fdDateErrorMsg);

            var fdDateofWeekMsg = messageService.GetMessage(SystemMessage.Error.FlagDay.fdDateDateofWeek);
            RuleFor(x => x.FlagDayDate).Must(ValidateFDDateWedSat).When(x => x.FlagDayDate != null).WithMessage(fdDateofWeekMsg);
            RuleFor(x => x.FlagDayDate).Must(UniqueFlagDayDate).When(x => x.FlagDayDate != null).WithMessage(messageService.GetMessage(SystemMessage.Error.Unique));
            RuleFor(x => x.FlagDayType).NotNull().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));
            //rule for create disaster
            RuleSet("CreateFdList", () =>
            {
            });

            //rulke for update disaster
            RuleSet("UpdateFdList", () =>
            {
            });
        }

        protected bool ValidateFlagDayDate(FlagDayListViewModel model, DateTime? date)
        {
            int yearPrv = Convert.ToInt32(model.FlagDayYear.Substring(0, 2));

            if (date == null)
            {
                date = DateTime.Now;
            }
            DateTime fromDate = new DateTime(2000 + yearPrv, 4, 1);//from 2000 year
            DateTime toDate = new DateTime(2001 + yearPrv, 3, 31);
            if (date < fromDate || date > toDate)
            {
                return false;
            }
            return true;
        }

        protected bool ValidateFDDateWedSat(DateTime? date)
        {
            DayOfWeek strWeek = date.Value.DayOfWeek;
            switch (strWeek)
            {
                case DayOfWeek.Wednesday:
                    return true;

                case DayOfWeek.Saturday:
                    return true;
            }
            return false;
        }

        private bool UniqueFlagDayDate(FlagDayListViewModel model, DateTime? FlagDayDate)
        {
            //bool flag = false;
            //if (!String.IsNullOrEmpty(model.FlagDayListId))
            //{
            //    var FdList = _FdListService.GetFdListByDateAndType(FlagDayDate,model.FlagDayType);
            //    if (FdList == null)
            //    {
            //        flag = true;
            //    }
            //    else
            //    {
            //        if (FdList.FlagDayListId == Convert.ToInt32(model.FlagDayListId))
            //        {
            //            flag = true;
            //        }
            //    }
            //}
            //else
            //{
            //    flag = _FdListService.IsUniqueFlagDayDate(FlagDayDate, model.FlagDayType);
            //}

            return _FdListService.IsUniqueFlagDayDate(FlagDayDate.Value, model.FlagDayListId);
        }
    }
}