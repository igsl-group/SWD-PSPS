using OfficeOpenXml;
using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Complaint;
using Psps.Models.Dto.OGCIO;
using Psps.Models.Dto.Organisation;
using Psps.Services.Events;
using Psps.Services.OGCIO;
using Psps.Services.SystemMessages;
using Psps.Services.SystemParameters;
using Psps.Services.UserLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Organisations
{
    public partial class OrganisationService : IOrganisationService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;

        private readonly IOrgMasterRepository _orgMasterRepository;

        private readonly ICacheManager _cacheManager;

        private readonly IOrgEditPspViewRepository _orgEditPspViewRepository;

        private readonly IOrgEditComplaintViewRepository _orgEditComplaintViewRepository;

        private readonly IOrgMasterSearchViewRepository _orgMasterSearchViewRepository;

        private readonly IPspApprovalHistoryRepository _pspApprovalHistoryRepository;

        private readonly IOrgEditEnquiryViewRepository _orgEditEnquiryViewRepository;

        private readonly IReferenceGuideSearchViewRepository _referenceGuideSearchViewRepository;

        private readonly IMessageService _messageService;

        private readonly IOrgRefGuidePromulgationRepository _orgRefGuidePromulgationRepository;

        private readonly IUserLogService _userLogService;

        private readonly IOrganisationApi _organisationApi;

        #endregion Fields

        #region Ctor

        public OrganisationService(IEventPublisher eventPublisher, IOrgMasterRepository orgMasterRepository, ICacheManager cacheManager,
            IOrgEditPspViewRepository orgEditPspViewRepository, IOrgEditComplaintViewRepository orgEditComplaintViewRepository,
            IOrgMasterSearchViewRepository orgMasterSearchViewRepository,
            IPspApprovalHistoryRepository pspApprovalHistoryRepository, IOrgEditEnquiryViewRepository orgEditEnquiryViewRepository,
            IReferenceGuideSearchViewRepository referenceGuideSearchViewRepository, IMessageService messageService, IOrgRefGuidePromulgationRepository orgRefGuidePromulgationRepository,
            IUserLogService userLogService, IOrganisationApi organisationApi)
        {
            this._eventPublisher = eventPublisher;
            this._orgMasterRepository = orgMasterRepository;
            this._cacheManager = cacheManager;
            this._orgEditPspViewRepository = orgEditPspViewRepository;
            this._orgEditComplaintViewRepository = orgEditComplaintViewRepository;
            this._orgMasterSearchViewRepository = orgMasterSearchViewRepository;
            this._pspApprovalHistoryRepository = pspApprovalHistoryRepository;
            this._orgEditEnquiryViewRepository = orgEditEnquiryViewRepository;
            this._referenceGuideSearchViewRepository = referenceGuideSearchViewRepository;
            this._messageService = messageService;
            this._orgRefGuidePromulgationRepository = orgRefGuidePromulgationRepository;
            this._userLogService = userLogService;
            this._organisationApi = organisationApi;
        }

        #endregion Ctor

        #region Methods

        public OrgMaster GetOrgById(int orgId)
        {
            return _orgMasterRepository.GetById(orgId);
        }

        public OrgMaster GetOrgByRef(string orgRef)
        {
            return _orgMasterRepository.GetOrgByRef(orgRef);
        }

        public IPagedList<OrgMaster> GetPage(GridSettings grid)
        {
            return _orgMasterRepository.GetPage(grid);
        }

        public void CreateOrgMaster(OrgMaster model)
        {
            Ensure.Argument.NotNull(model, "New Organisation");
            
            _orgMasterRepository.Add(model);

            _userLogService.LogOrganisationInformation(null, model);

            //TODO: Remove in production
            IParameterService _parameterService = EngineContext.Current.Resolve<IParameterService>();
            var config = _parameterService.GetParameterByCode(Constant.SystemParameter.FRAS_ENABLED);

            if (config != null && "Y".Equals(config.Value, StringComparison.CurrentCultureIgnoreCase))
            {
                var result = _organisationApi.Create(new Organisation
                            {
                                OrganisationNameEnglish = model.EngOrgName,
                                OrganisationNameTChinese = model.ChiOrgName,
                                OrganisationNameSChinese = model.SimpChiOrgName,
                                OrganisationUrlEnglish = model.URL,
                                OrganisationAbbreviation = "Dummy",
                                S88 = model.Section88Indicator.Value ? 1 : 0,
                                Disable = model.DisableIndicator ? 1 : 0
                            });

                if (result.HasError)
                {
                    throw new ApplicationException(result.Content);
                }

                model.FrasOrganisationId = Convert.ToInt32(result.Content);
            }

            _orgMasterRepository.Update(model);

            _eventPublisher.EntityInserted<OrgMaster>(model);
        }

        public void UpdateOrgMaster(OrgMaster orgMaster)
        {
            _orgMasterRepository.Update(orgMaster);
            _eventPublisher.EntityUpdated<OrgMaster>(orgMaster);
        }

        public void UpdateOrgMaster(OrgMaster oldOrgMaster, OrgMaster newOrgMaster)
        {
            Ensure.Argument.NotNull(oldOrgMaster, "Old Organisation");
            Ensure.Argument.NotNull(newOrgMaster, "New Organisation");

            _userLogService.LogOrganisationInformation(oldOrgMaster, newOrgMaster);

            _orgMasterRepository.Update(newOrgMaster);

            //TODO: Remove in production
            IParameterService _parameterService = EngineContext.Current.Resolve<IParameterService>();
            var config = _parameterService.GetParameterByCode(Constant.SystemParameter.FRAS_ENABLED);

            if (config != null && "Y".Equals(config.Value, StringComparison.CurrentCultureIgnoreCase))
            {
                var result = _organisationApi.Update(new Organisation
                {
                    OrganisationId = newOrgMaster.FrasOrganisationId,
                    OrganisationNameEnglish = newOrgMaster.EngOrgName,
                    OrganisationNameTChinese = newOrgMaster.ChiOrgName,
                    OrganisationNameSChinese = newOrgMaster.SimpChiOrgName,
                    OrganisationUrlEnglish = newOrgMaster.URL,
                    OrganisationAbbreviation = "Dummy",
                    S88 = newOrgMaster.Section88Indicator.Value ? 1 : 0,
                    Disable = newOrgMaster.DisableIndicator ? 1 : 0
                });

                if (result.HasError)
                {
                    throw new ApplicationException(result.Content);
                }
            }

            _eventPublisher.EntityUpdated<OrgMaster>(newOrgMaster);
        }

        public string CreateOrgRef()
        {
            return _orgMasterRepository.CreateOrgRef();
        }

        public IPagedList<OrgFlagDaySearchDto> getPageByOrgFlagDaySearchDto(GridSettings grid)
        { return _orgMasterRepository.getPageByOrgFlagDaySearchDto(grid); }

        public IPagedList<OrgEditPspView> getPageByOrgEditPspView(GridSettings grid)
        {
            return _orgEditPspViewRepository.GetPage(grid);
        }

        public IPagedList<OrgEditComplaintView> getPageByOrgEditComplaintView(GridSettings grid)
        {
            return _orgEditComplaintViewRepository.GetPage(grid);
        }

        public int GetOrgEditPspViewAmountByOrgId(string OrgId, bool IsSSAF = false)
        {
            Ensure.Argument.NotNullOrEmpty(OrgId, "OrgId");
            return _orgEditPspViewRepository.Table.Count(a => a.OrgId == Convert.ToInt32(OrgId) && a.IsSsaf == IsSSAF);
        }

        public int GetOrgEditComplaintViewAmountByOrgId(string OrgId)
        {
            Ensure.Argument.NotNullOrEmpty(OrgId, "OrgId");
            return _orgEditComplaintViewRepository.Table.Count(a => a.OrgId == Convert.ToInt32(OrgId));
        }

        public int GetOrgEditEnquiryViewAmountByOrgId(string OrgId)
        {
            Ensure.Argument.NotNullOrEmpty(OrgId, "OrgId");
            return _orgEditEnquiryViewRepository.Table.Count(a => a.OrgId == Convert.ToInt32(OrgId));
        }

        public int GetOrgEditReferenceGuideViewAmountByOrgId(string OrgId)
        {
            Ensure.Argument.NotNullOrEmpty(OrgId, "OrgId");
            // return _referenceGuideSearchViewRepository.Table.Count(a => a.OrgId == Convert.ToInt32(OrgId));
            return _orgRefGuidePromulgationRepository.Table.Count(a => a.OrgMaster.OrgId == Convert.ToInt32(OrgId));
        }

        public IPagedList<OrgMasterSearchView> GetPageByOrgMasterSearchView(GridSettings grid, String withHoldInd, String receivedComplaintBefore, String receivedEnquiryBefore,
                                                                            String appliedPspBefore, DateTime? fromPspAppRecDate, DateTime? toPspAppRecDate, 
                                                                            String appliedFdBefore, IList<string> appliedFDBeforeFdYears,
                                                                            String appliedSSAFBefore, DateTime? fromSSAFAppRecDate, DateTime? toSSAFAppRecDate,
                                                                            String pspIssuedBfore, DateTime? fromPspPermitIssueDate, DateTime? toPspPermitIssueDate, 
                                                                            String fdIssuedBefore, IList<string> fdIssuedBeforeFdYears,
                                                                            String ssafFIssuedBfore, DateTime? fromSSAFPermitIssueDate, DateTime? toSSAFPermitIssueDate)
        {
            return _orgMasterSearchViewRepository.GetPageByOrgMasterSearchView(grid, withHoldInd, receivedComplaintBefore, receivedEnquiryBefore, 
                                                                               appliedPspBefore, fromPspAppRecDate, toPspAppRecDate,
                                                                               appliedFdBefore, appliedFDBeforeFdYears, 
                                                                               appliedSSAFBefore, fromSSAFAppRecDate, toSSAFAppRecDate,
                                                                               pspIssuedBfore, fromPspPermitIssueDate, toPspPermitIssueDate, 
                                                                               fdIssuedBefore, fdIssuedBeforeFdYears,
                                                                               ssafFIssuedBfore, fromSSAFPermitIssueDate, toSSAFPermitIssueDate);
        }

        public void Delete(OrgMaster orgMaster)
        {
            orgMaster.IsDeleted = true;
            _orgMasterRepository.Update(orgMaster);

            var result = _organisationApi.Delete(orgMaster.FrasOrganisationId.Value);
            if (result.HasError)
            {
                throw new ApplicationException(result.Content);
            }

            _eventPublisher.EntityUpdated<OrgMaster>(orgMaster);
        }

        public IPagedList<ComplaintPspMasterDto> getPageComplaintByPspMasterId(GridSettings grid, int pspMasterId)
        {
            return _orgMasterRepository.getPageComplaintByPspMasterId(grid, pspMasterId);
        }

        public IPagedList<ComplaintPspMasterDto> getPageEnquiryByPspMasterId(GridSettings grid, int pspMasterId)
        {
            return _orgMasterRepository.getPageEnquiryByPspMasterId(grid, pspMasterId);
        }

        public IPagedList<ComplaintFdMasterDto> getPageComplaintByFdMasterId(GridSettings grid, int fdMasterId)
        {
            return _orgMasterRepository.getPageComplaintByFdMasterId(grid, fdMasterId);
        }

        public IPagedList<ComplaintFdMasterDto> getPageEnquiryByFdMasterId(GridSettings grid, int fdMasterId)
        {
            return _orgMasterRepository.getPageEnquiryByFdMasterId(grid, fdMasterId);
        }

        public int GetLeastOrgMaster()
        {
            return _orgMasterRepository.Table.Max(x => x.Id);
        }

        public IPagedList<OrgEditEnquiryView> getPageByOrgEditEnquiryView(GridSettings grid)
        {
            return this._orgEditEnquiryViewRepository.GetPage(grid);
        }

        public IPagedList<ReferenceGuideSearchView> GetPageByReferenceGuideSearchView(GridSettings grid)
        {
            return _referenceGuideSearchViewRepository.GetPage(grid);
        }

        public IPagedList<ReferenceGuideSearchView> GetPageByReferenceGuideSearchView(GridSettings grid, String appliedPspBefore, DateTime? fromPspAppRecDate, DateTime? toPspAppRecDate, 
                                                                                      String pspIssuedBfore, DateTime? fromPspPermitIssueDate, DateTime? toPspPermitIssueDate,
                                                                                      List<string> appliedFDBeforeFdYears, IList<string> fdIssuedBeforeFdYears, string appliedFDBeforeId, string fdIssuedBeforeId,
                                                                                      String appliedSSAFBefore, DateTime? fromSSAFApplicationDate, DateTime? toSSAFApplicationDate,
                                                                                      String ssafIssuedBefore, DateTime? fromSSAFPermitIssueDate, DateTime? toSSAFPermitIssueDate,
                                                                                      String referenceGuideActivityConcern)
        {
            return _referenceGuideSearchViewRepository.GetPageByReferenceGuideSearchView(grid, 
                appliedPspBefore, fromPspAppRecDate, toPspAppRecDate, pspIssuedBfore, fromPspPermitIssueDate, toPspPermitIssueDate, 
                appliedFDBeforeFdYears, fdIssuedBeforeFdYears, appliedFDBeforeId, fdIssuedBeforeId,
                appliedSSAFBefore, fromSSAFApplicationDate, toSSAFApplicationDate,
                ssafIssuedBefore, fromSSAFPermitIssueDate, toSSAFPermitIssueDate,
                referenceGuideActivityConcern);
        }

        public bool IsExistedOrgName(string engOrgName, string chiOrgName)
        {
            if (!String.IsNullOrEmpty(chiOrgName) && chiOrgName.Equals("沒有中文註冊名稱"))
            {
                return _orgMasterRepository.Table.Count(x => x.EngOrgName == engOrgName && x.IsDeleted == false) > 0;
            }
            return _orgMasterRepository.Table.Count(x => x.EngOrgName == engOrgName || x.ChiOrgName == chiOrgName && x.IsDeleted == false) > 0;
        }

        public string ImportRefGuideXlsFile(Stream ms)
        {
            var package = new ExcelPackage(ms);

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

            // Define a Dictonary to store the column position
            Dictionary<String, int> columnPosition = new Dictionary<String, int>();

            columnPosition.Add("OrgRef", 0);
            columnPosition.Add("SendDate", 0);
            columnPosition.Add("PartNum", 0);
            columnPosition.Add("EnclosureNum", 0);

            // Loop the Column
            for (int i = 1; i <= end.Column; i++)
            {
                // Check the Column Header and record the column position
                switch (workSheet.Cells[start.Row, i].Text)
                {
                    case "Org. Reference":
                        columnPosition["OrgRef"] = i;
                        break;

                    case "Date of Letter Sent":
                        columnPosition["SendDate"] = i;
                        break;

                    case "Part No.":
                        columnPosition["PartNum"] = i;
                        break;

                    case "Encl. No.":
                        columnPosition["EnclosureNum"] = i;
                        break;
                }

                //use the column header to locate end of cloumns
                if (!workSheet.Cells[start.Row, i].Text.Equals(""))
                {
                    endCol = i - 1;
                }
                else break;
            }

            IList<OrgRefGuidePromulgationXlsFileDto> list = new List<OrgRefGuidePromulgationXlsFileDto>();
            for (int i = start.Row + 1; i <= end.Row; i++)
            {
                if (workSheet.Cells[i, 1].Text.Equals(""))
                    continue;

                var dto = new OrgRefGuidePromulgationXlsFileDto();
                for (int j = 1; j <= endCol + 1; j++)
                {
                    var text = workSheet.Cells[i, j].Text;
                    if (!String.IsNullOrEmpty(text))
                    {
                        // Store to the corresponding field by checking the column position
                        if (columnPosition["OrgRef"] == j)
                        {
                            dto.OrgRef = text;
                        }
                        else if ((columnPosition["SendDate"] == j))
                        {
                            dto.SendDate = text;
                            //dto.SendDate = DateTime.ParseExact(text, new string[] { "dd/MM/yyyy","d/M/yyyy", "M/d/yyyy" }, new CultureInfo("en-US"), DateTimeStyles.None);
                        }
                        else if ((columnPosition["PartNum"] == j))
                        {
                            dto.PartNum = text;
                        }
                        else if ((columnPosition["EnclosureNum"] == j))
                        {
                            dto.EnclosureNum = text;
                        }
                        else
                        {
                            // Skip
                        }
                    }
                }
                list.Add(dto);
            }
            string errorMsg = ValidateRefGuideXlsData(list);
            return errorMsg;
        }

        private string ValidateRefGuideXlsData(IList<OrgRefGuidePromulgationXlsFileDto> list)
        {
            StringBuilder sb = new StringBuilder("");
            int index = 1;
            string IncorrectSendDateMsg = _messageService.GetMessage(SystemMessage.Error.Organisation.IncorrectImportRefGuideXlsSendDate);
            string RecordExistedMsg = _messageService.GetMessage(SystemMessage.Error.Organisation.ImportRefGuideXlsRecordExisted);
            string OrgRefRecordNotExisted = _messageService.GetMessage(SystemMessage.Error.Organisation.OrgRefRecordNotExisted);
            string sendDtIsEmpty = _messageService.GetMessage(SystemMessage.Error.Organisation.SendDateIsEmpty);

            foreach (var dto in list)
            {
                if (string.IsNullOrEmpty(dto.SendDate))
                {
                    sb.Append(String.Format(sendDtIsEmpty, index));
                }
                else if (!CommonHelper.IsValidDate(dto.SendDate))
                {
                    sb.Append(String.Format(IncorrectSendDateMsg, index));
                }
                else
                {
                    var org = GetOrgByRef(dto.OrgRef);
                    if (org != null)
                    {
                        dto.OrgId = org.OrgId;
                        dto.OrgMaster = org;
                        string errorMsg = "";
                        if (!string.IsNullOrEmpty(dto.SendDate))
                            errorMsg = ValidateRecordExistedOrNot(dto, RecordExistedMsg, index);

                        if (!String.IsNullOrEmpty(errorMsg))
                        {
                            sb.Append(errorMsg);
                        }
                    }
                    else
                    {
                        sb.Append(String.Format(OrgRefRecordNotExisted, index));
                    }
                }

                index++;
            }

            if (String.IsNullOrEmpty(sb.ToString()))
            {
                foreach (var dto in list)
                {
                    var sendDt = (DateTime?)null;
                    sendDt = DateTime.ParseExact(dto.SendDate, new string[] { "dd/MM/yyyy", "d/M/yyyy", "M/d/yyyy" }, new CultureInfo("en-US"), DateTimeStyles.None);
                    var exs = _orgRefGuidePromulgationRepository.Table.Any(x => x.OrgMaster.OrgId == dto.OrgMaster.OrgId && x.SendDate == sendDt && sendDt != null);
                    if (!exs)
                    {
                        OrgRefGuidePromulgation o = _orgRefGuidePromulgationRepository.Table.FirstOrDefault(x => x.OrgMaster.OrgId == dto.OrgMaster.OrgId && x.SendDate == null && x.OrgReply == "5");

                        if (o == null)
                        {
                            o = new OrgRefGuidePromulgation();
                            o.OrgMaster = dto.OrgMaster;
                            
                            o.SendDate = sendDt;
                            o.PartNum = dto.PartNum;
                            o.EnclosureNum = dto.EnclosureNum;

                            _orgRefGuidePromulgationRepository.Add(o);
                            _eventPublisher.EntityInserted<OrgRefGuidePromulgation>(o);
                        }
                        else
                        {
                            o.OrgReply = null;
                            o.SendDate = sendDt;
                            o.PartNum = dto.PartNum;
                            o.EnclosureNum = dto.EnclosureNum;
                            _orgRefGuidePromulgationRepository.Update(o);
                            _eventPublisher.EntityUpdated<OrgRefGuidePromulgation>(o);
                        }
                    }
                }
            }
            return sb.ToString();
        }

        private string ValidateRecordExistedOrNot(OrgRefGuidePromulgationXlsFileDto dto, string RecordExistedMsg, int index)
        {
            string errorMsg = "";
            var count = _orgRefGuidePromulgationRepository.Table.Where(o => o.OrgMaster.OrgId == dto.OrgId && o.SendDate == CommonHelper.ConvertStringToDateTime(dto.SendDate) && o.IsDeleted == false).Count();
            if (count > 0)
            {
                errorMsg = String.Format(RecordExistedMsg, index);
            }
            return errorMsg;
        }

        #endregion Methods
    }
}