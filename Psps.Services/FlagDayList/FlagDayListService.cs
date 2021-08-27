using AutoMapper;
using OfficeOpenXml;
using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Lookups;
using Psps.Services.Accounts;
using Psps.Services.Events;
using Psps.Services.Lookups;
using Psps.Services.SystemMessages;
using Psps.Services.SystemParameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Psps.Services.FlagDays
{
    public partial class FlagDayListService : IFlagDayListService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;

        private readonly IEventPublisher _eventPublisher;

        private readonly IFlagDayListRepository _flagDayListRepository;

        private readonly IMessageService _messageService;

        private readonly ILookupService _lookupService;

        private readonly IParameterService _parameterService;

        private readonly IDictionary<string, string> twrs;

        #endregion Fields

        #region Ctor

        public FlagDayListService(IEventPublisher eventPublisher,
            IFlagDayListRepository flagDayListRepository,
            ICacheManager cacheManager, IMessageService messageService, ILookupService lookupService, IParameterService parameterService)
        {
            this._eventPublisher = eventPublisher;
            this._cacheManager = cacheManager;
            this._flagDayListRepository = flagDayListRepository;
            this._messageService = messageService;
            this._lookupService = lookupService;
            this._parameterService = parameterService;

            this.twrs = _lookupService.getAllLkpInCodec(LookupType.TWR);
            if (twrs.Count == 0) twrs.Add("", ""); ;
        }

        #endregion Ctor

        #region Methods

        public FdList GetFlagDayListById(int flagDayListId)
        {
            return _flagDayListRepository.Get(u => u.FlagDayListId == flagDayListId);
        }

        public IPagedList<FdList> GetPage(GridSettings grid)
        {
            return _flagDayListRepository.GetPage(grid);
        }

        public void CreateFlagDayList(FdList model)
        {
            _flagDayListRepository.Add(model);
            _eventPublisher.EntityInserted<FdList>(model);
        }

        public void UpdateFlagDayList(FdList model)
        {
            _flagDayListRepository.Update(model);
            _eventPublisher.EntityUpdated<FdList>(model);
        }

        public IDictionary<string, string> GetAllFlagDayListTypeForDropdown()
        {
            string key = Constant.FLAGDAYLISTTYPE_FOR_DROWDROP_KEY;
            return _cacheManager.Get(key, () =>
            {
                return this._flagDayListRepository.Table
                     .OrderBy(p => p.FlagDayType)
                     .Where(p => p.IsDeleted == false)
                     .Select(p => new { Key = p.FlagDayType, Value = p.FlagDayType })
                     .Distinct().ToDictionary(k => k.Key, v => v.Value);
            });
        }

        public IDictionary<string, string> GetAllFlagDayListYearForDropdown()
        {
            string key = Constant.FLAGDAYLISTYEAR_FOR_DROWDROP_KEY;
            return _cacheManager.Get(key, () =>
            {
                return this._flagDayListRepository.Table
                     .OrderBy(p => p.FlagDayYear)
                     .Where(p => p.IsDeleted == false)
                     .Select(p => new { Key = p.FlagDayYear, Value = p.FlagDayYear })
                     .Distinct().ToDictionary(k => k.Key, v => v.Value);
            });
        }

        public bool IsUniqueFlagDayDate(DateTime FlagDayDate, int? FlagDayListId)
        {
            Ensure.Argument.NotNull(FlagDayDate);

            return !_flagDayListRepository.Table.Any(f => f.FlagDayDate == FlagDayDate && f.FlagDayListId != (FlagDayListId.HasValue ? FlagDayListId : -1));
        }

        public FdList GetFdListByDateAndType(DateTime? FlagDayDate, string FlagDayType)
        {
            return _flagDayListRepository.Get(f => f.FlagDayDate == FlagDayDate && f.FlagDayType == FlagDayType);
        }

        public void DeleteFlagDayList(int Id)
        {
            var FdList = _flagDayListRepository.GetById(Id);
            _flagDayListRepository.Delete(FdList);
            _eventPublisher.EntityDeleted<FdList>(FdList);
        }

        public bool EveRngChk(DateTime eveStartDt, string startTime, DateTime eveEndDt, string endTime)
        {
            var d = _flagDayListRepository.Table.Any(x => x.FlagDayDate <= eveEndDt && x.FlagDayDate >= eveStartDt);

            var t = Convert.ToInt32(startTime) < 1300;

            return !(d && t);
        }

        public bool IsFdDateAndTimeConflicit(DateTime startDate, DateTime endDate, string startTime, string endTime)
        {
            int intStartTime = Convert.ToInt32(startTime);
            int intEndTime = Convert.ToInt32(endTime);

            //int fdStartTime = Convert.ToInt32(_parameterService.GetParameterByCode(Constant.SystemParameter.FLAGDAY_EVENT_START_TIME).Value);
            //int fdEndTime = Convert.ToInt32(_parameterService.GetParameterByCode(Constant.SystemParameter.FLAGDAY_EVENT_END_TIME).Value);

            if (intStartTime > 1300)
                return true;
            else
                return !_flagDayListRepository.Table.Any(u => startDate >= u.FlagDayDate && endDate <= u.FlagDayDate);
        }

        public string InsertFlagDayListByImportXls(Stream xlsxStream)
        {
            var package = new ExcelPackage(xlsxStream);

            ExcelWorksheet workSheet = null; // = package.Workbook.Worksheets[3];
            workSheet = package.Workbook.Worksheets[1];

            if (workSheet == null)
            {
                return "Worksheet is empty";
            }

            var target = workSheet.Cells[1, 1]; // starting from list header
            var start = target.Start;
            var end = workSheet.Dimension.End;
            var col = start.Column;
            var endCol = 0;

            for (int i = 1; i <= end.Column; i++) //use the column header to locate end of cloumns
            {
                if (!workSheet.Cells[start.Row, i].Text.Trim().Equals(""))
                {
                    endCol = i - 1;
                }
                else break;
            }

            IList<FdList> list = new List<FdList>();
            for (int i = start.Row + 1; i <= end.Row; i++)
            {
                if (workSheet.Cells[i, 1].Text.Trim().Equals("") || workSheet.Cells[i, 2].Text.Trim().Equals("") || workSheet.Cells[i, 3].Text.Trim().Equals(""))
                    continue;

                var fdList = new FdList();
                for (int j = 1; j <= endCol + 1; j++)
                {
                    var text = workSheet.Cells[i, j].Text;
                    if (!String.IsNullOrEmpty(text))
                    {
                        switch (j)
                        {
                            case 1:
                                fdList.FlagDayYear = text;
                                break;

                            case 2:
                                fdList.FlagDayType = text;
                                break;

                            case 3:
                                if (CommonHelper.IsValidDate(text))
                                {
                                    fdList.FlagDayDate = CommonHelper.ConvertStringToDateTime(text); ;
                                }
                                break;
                        }
                    }
                }
                list.Add(fdList);
            }
            string errorMsg = ValidateFdList(list);
            if (String.IsNullOrEmpty(errorMsg))
            {
                //save record
                foreach (var fdList in list)
                {
                    if (!String.IsNullOrEmpty(fdList.FlagDayType))
                    {
                        if (fdList.FlagDayType.ToLower().Equals("regional"))
                        {
                            fdList.FlagDayType = "R";
                        }
                        else if (fdList.FlagDayType.ToLower().Equals("territory-wide"))
                        {
                            fdList.FlagDayType = "T";
                        }
                    }
                    this.CreateFlagDayList(fdList);
                }
            }
            return errorMsg;
        }

        public bool MatchFlagDayInFdList(DateTime flagday, string type, string fdYear)
        {
            type = type == "1" ? "T" : "R";

            var count = _flagDayListRepository.Table.Where(u => u.FlagDayDate == flagday && u.FlagDayType == type && u.FlagDayYear == fdYear).ToList().Count();

            if (count > 0)
                return true; //matched
            else
                return false;
        }

        private string ValidateFdList(IList<FdList> list)
        {
            StringBuilder sb = new StringBuilder("");
            int index = 1;
            foreach (var fdList in list)
            {
                string error = GetValidateFlagDayDateMsg(fdList, index);
                if (!String.IsNullOrEmpty(error))
                {
                    sb.Append(error);
                }
                error = GetFlagDayMustUnique(fdList, index);
                if (!String.IsNullOrEmpty(error))
                {
                    sb.Append(error);
                }
                error = GetValidateFDDateWedSatMsg(fdList, index);
                if (!String.IsNullOrEmpty(error))
                {
                    sb.Append(error);
                }
                index++;
            }
            return sb.ToString();
        }

        private string GetFlagDayMustUnique(FdList fdList, int index)
        {
            string errorMsg = "";
            var count = _flagDayListRepository.Table.Count(f => f.FlagDayYear == fdList.FlagDayYear && f.FlagDayDate == fdList.FlagDayDate);
            if (count > 0)
            {
                errorMsg = String.Format(_messageService.GetMessage(SystemMessage.Error.FlagDay.FlagDayUniqueImportError), index, fdList.FlagDayYear, CommonHelper.ConvertDateTimeToString(fdList.FlagDayDate.Value), "dd/MM/yyyy");
            }
            return errorMsg;
        }

        private string GetValidateFlagDayDateMsg(FdList fdList, int index)
        {
            string errorMsg = "";
            int yearPrv = Convert.ToInt32(fdList.FlagDayYear.Substring(0, 2));
            DateTime date = DateTime.Now;
            if (fdList.FlagDayDate != null)
            {
                date = fdList.FlagDayDate.Value;
            }

            DateTime fromDate = new DateTime(2000 + yearPrv, 4, 1);//from 2000 year
            DateTime toDate = new DateTime(2001 + yearPrv, 3, 31);
            if (date < fromDate || date > toDate)
            {
                errorMsg = String.Format(_messageService.GetMessage(SystemMessage.Error.FlagDay.fdDateImportError), index);
                return errorMsg;
            }
            return errorMsg;
        }

        private string GetValidateFDDateWedSatMsg(FdList fdList, int index)
        {
            string errorMsg = "";
            DayOfWeek strWeek = fdList.FlagDayDate.Value.DayOfWeek;
            if (strWeek != DayOfWeek.Wednesday && strWeek != DayOfWeek.Saturday)
            {
                errorMsg = String.Format(_messageService.GetMessage(SystemMessage.Error.FlagDay.fdDateDateofWeekImportError), index);
            }
            return errorMsg;
        }

        #endregion Methods
    }
}