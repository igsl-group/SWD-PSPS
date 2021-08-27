using FluentValidation.Mvc;
using Psps.Core;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Models.Dto.Lookups;
using Psps.Services.LegalAdvices;
using Psps.Services.Lookups;
using Psps.Services.Report;
using Psps.Services.SystemMessages;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Mvc;
using Psps.Web.ViewModels.LegalAdvice;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("LegalAdvice"), Route("{action=index}")]
    public class LegalAdviceController : BaseController
    {
        private readonly static string LegalAdviceControllerSearchSessionName = "LegalAdviceControllerSearch";
        private readonly string DATE_FORMAT = "dd/MM/yyyy";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;
        private readonly ILookupService _lookupService;
        private readonly ILegalAdviceService _legalAdviceService;
        private readonly IReportService _reportService;

        public LegalAdviceController(IUnitOfWork unitOfWork, IMessageService messageService,
            ILegalAdviceService legalAdviceService, ILookupService lookupService,
            IReportService reportService)
        {
            this._unitOfWork = unitOfWork;
            this._messageService = messageService;
            this._legalAdviceService = legalAdviceService;
            this._lookupService = lookupService;
            this._reportService = reportService;
        }

        [PspsAuthorize(Allow.LegalAdvice)]
        [HttpGet, Route("Search", Name = "Search")]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        public ActionResult Search()
        {
            LegalAdviceViewModel model = new LegalAdviceViewModel();

            this.HttpContext.Session[LegalAdviceControllerSearchSessionName] = null;

            initSelectOptions(model);
            return View(model);
        }

        [PspsAuthorize(Allow.LegalAdvice)]
        [HttpGet, Route("ReturnSearch", Name = "ReturnSearch")]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        public ActionResult ReturnSearch()
        {
            LegalAdviceViewModel model = new LegalAdviceViewModel();

            if (this.HttpContext.Session[LegalAdviceControllerSearchSessionName] != null)
                model = ((LegalAdviceViewModel)(this.HttpContext.Session[LegalAdviceControllerSearchSessionName]));
            initSelectOptions(model);
            return View("Search", model);
        }

        [PspsAuthorize(Allow.LegalAdvice)]
        [HttpGet, Route("~/api/legalAdvice/getSearchDataFromSession", Name = "getSearchDataFromSessionUrl")]
        public JsonResult getSearchDataFromSession()
        {
            LegalAdviceViewModel model = null;

            if (this.HttpContext.Session[LegalAdviceControllerSearchSessionName] != null)
                model = ((LegalAdviceViewModel)(this.HttpContext.Session[LegalAdviceControllerSearchSessionName]));

            return Json(new JsonResponse(true)
            {
                //Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted),
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.LegalAdvice)]
        [HttpGet, Route("New/{code}", Name = "EditLegalAdviceMaster")]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        public ActionResult New(string code)
        {
            LegalAdviceViewModel model = new LegalAdviceViewModel();
            var LegalAdviceTypes = _lookupService.getAllLkpInCodec(LookupType.LegalAdviceType);
            model.LegalAdviceTypeHeads = LegalAdviceTypes;
            model.RelatedLegalAdviceTypes = LegalAdviceTypes;

            var VenueTypes = _lookupService.getAllLkpInCodec(LookupType.VenueType);
            model.LegalAdviceTypes = VenueTypes;
            model.RelatedVenueTypes = VenueTypes;

            var DescriptionForDropdownAll = _legalAdviceService.GetDescriptionForDropdownAll();
            model.RelatedLegalAdvices = DescriptionForDropdownAll;

            var PSPRequiredIndicators = _lookupService.getAllLkpInCodec(LookupType.PSPRequiredIndicator);
            model.PSPRequireds = PSPRequiredIndicators;

            Ensure.Argument.NotNullOrEmpty(code);

            var legalAdvice = _legalAdviceService.GetLegalAdviceMasterById(Convert.ToInt32(code));
            System.Globalization.DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";

            var Data = new LegalAdviceViewModel
             {
                 LegalAdviceTypeHeads = LegalAdviceTypes,
                 RelatedLegalAdviceTypes = LegalAdviceTypes,
                 LegalAdviceTypes = VenueTypes,
                 RelatedVenueTypes = VenueTypes,
                 PSPRequireds = PSPRequiredIndicators,
                 RelatedLegalAdvices = DescriptionForDropdownAll,
                 LegalAdviceMasterId = legalAdvice.LegalAdviceId.ToString(),
             };

            return View(Data);
        }

        [PspsAuthorize(Allow.LegalAdvice)]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        public ActionResult New()
        {
            LegalAdviceViewModel model = new LegalAdviceViewModel();
          
            var LegalAdviceTypes = _lookupService.getAllLkpInCodec(LookupType.LegalAdviceType);
            model.LegalAdviceTypeHeads = LegalAdviceTypes;
            model.RelatedLegalAdviceTypes = LegalAdviceTypes;

            var VenueTypes = _lookupService.getAllLkpInCodec(LookupType.VenueType);
            model.LegalAdviceTypes = VenueTypes;
            model.RelatedVenueTypes = VenueTypes;

            var DescriptionForDropdownAll = _legalAdviceService.GetDescriptionForDropdownAll();
            model.RelatedLegalAdvices = DescriptionForDropdownAll;
            
            var PSPRequiredIndicators = _lookupService.getAllLkpInCodec(LookupType.PSPRequiredIndicator);
            model.PSPRequireds = PSPRequiredIndicators;

            return View(model);
        }

        public PartialViewResult RenderRelatedLegalAdviceModal()
        {
            return PartialView("_RelatedLegalAdviceModal");
        }

        private void initSelectOptions(LegalAdviceViewModel model)
        {
            var LegalAdviceTypes = _lookupService.getAllLkpInCodec(LookupType.LegalAdviceType);
            model.LegalAdviceTypeHeads = LegalAdviceTypes;
            model.RelatedLegalAdviceTypes = LegalAdviceTypes;

            var VenueTypes = _lookupService.getAllLkpInCodec(LookupType.VenueType);
            model.LegalAdviceTypes = VenueTypes;
            model.RelatedVenueTypes = VenueTypes;

            var DescriptionForDropdownAll = _legalAdviceService.GetDescriptionForDropdownAll();
            model.RelatedLegalAdvices = DescriptionForDropdownAll;

            var PSPRequiredIndicators = _lookupService.getAllLkpInCodec(LookupType.PSPRequiredIndicator);
            model.PSPRequireds = PSPRequiredIndicators;
        }

        #region REST-like API

        [PspsAuthorize(Allow.LegalAdvice)]
        [HttpPost, Route("~/api/legalAdvice/listLegalAdviceMaster", Name = "ListLegalAdviceMaster")]
        public JsonResult ListLegalAdviceMaster(GridSettings grid, LegalAdviceViewModel model)
        {
            //AddDefaultRule
            if (!CommonHelper.AreNullOrEmpty(model.LegalAdviceCode))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "LegalAdviceCode",
                    data = model.LegalAdviceCode,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }

            if (model.LegalAdviceTypeHeadId != null && model.LegalAdviceTypeHeadId != "" && model.LegalAdviceTypeHeadId != "0")
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "LegalAdviceType",
                    data = model.LegalAdviceTypeHeadId,
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (model.LegalAdviceTypeId != null && model.LegalAdviceTypeId != "" && model.LegalAdviceTypeId != "0")
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "VenueType",
                    data = model.LegalAdviceTypeId,
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (model.LegalAdviceDescription != null && model.LegalAdviceDescription != "")
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "LegalAdviceDescription",
                    data = model.LegalAdviceDescription,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }

            if (!CommonHelper.AreNullOrEmpty(model.PartNum))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "PartNum",
                    data = model.PartNum,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }

            if (!CommonHelper.AreNullOrEmpty(model.EnclosureNum))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "EnclosureNum",
                    data = model.EnclosureNum,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }

            if (model.PSPRequiredId != null && model.PSPRequiredId != "" && model.PSPRequiredId != "0")
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "RequirePspIndicator",
                    data = model.PSPRequiredId,
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (model.EffectiveDate != null)
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "EffectiveDate",
                    data = model.EffectiveDateStart.Value.ToString("dd/MM/yyyy"),
                    op = WhereOperation.GreaterThanOrEqual.ToEnumValue()
                });
            }

            if (model.EffectiveDateEnd != null)
            {
                grid.Where.rules.Add(new Rule()
                {
                    field = "EffectiveDate",
                    data = model.EffectiveDateEnd.Value.ToString("dd/MM/yyyy"),
                    op = WhereOperation.LessThanOrEqual.ToEnumValue()
                });
            }

            var legalAdvice = _legalAdviceService.GetPage(grid);

            this.HttpContext.Session[LegalAdviceControllerSearchSessionName] = model;

            var gridResult = new GridResult
            {
                TotalPages = legalAdvice.TotalPages,
                CurrentPageIndex = legalAdvice.CurrentPageIndex,
                TotalCount = legalAdvice.TotalCount,
                Data = (from p in legalAdvice
                        select new
                        {
                            legalAdviceMasterId = p.Id,
                            legalAdviceType = (!String.IsNullOrEmpty(p.LegalAdviceType) ? _lookupService.GetDescription(LookupType.LegalAdviceType, p.LegalAdviceType) : "") + (!String.IsNullOrEmpty(p.VenueType) ? "-" + _lookupService.GetDescription(LookupType.VenueType, p.VenueType) : ""),
                            legalAdviceCode = p.LegalAdviceCode,
                            legalAdviceDescription = p.LegalAdviceDescription,
                            partNum = p.PartNum,
                            enclosureNum = p.EnclosureNum,
                            effectiveDate = p.EffectiveDate,
                            requirePspIndicator = !String.IsNullOrEmpty(p.RequirePspIndicator) ? _lookupService.GetDescription(LookupType.PSPRequiredIndicator, p.RequirePspIndicator) : "",
                            remarks = p.Remarks,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.LegalAdvice)]
        [HttpPost, Route("~/api/legalAdvice/ListRelatedLegalAdviceMaster", Name = "ListRelatedLegalAdviceMaster")]
        public JsonResult ListRelatedLegalAdviceMaster(LegalAdviceViewModel model, GridSettings grid)
        {
            if (!String.IsNullOrEmpty(model.LegalAdviceTypeHeadId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "LegalAdviceType",
                    data = model.LegalAdviceTypeHeadId,
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.LegalAdviceTypeId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "VenueType",
                    data = model.LegalAdviceTypeId,
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            var legalAdvice = _legalAdviceService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = legalAdvice.TotalPages,
                CurrentPageIndex = legalAdvice.CurrentPageIndex,
                TotalCount = legalAdvice.TotalCount,
                Data = (from p in legalAdvice
                        select new
                        {
                            legalAdviceId = p.LegalAdviceId,
                            legalAdviceType = (!String.IsNullOrEmpty(p.LegalAdviceType) ? _lookupService.GetDescription(LookupType.LegalAdviceType, p.LegalAdviceType) : "") + (!String.IsNullOrEmpty(p.VenueType) ? "-" + _lookupService.GetDescription(LookupType.VenueType, p.VenueType) : ""),
                            legalAdviceCode = p.LegalAdviceCode,
                            legalAdviceDescription = p.LegalAdviceDescription,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.LegalAdvice)]
        [HttpGet, Route("~/api/legalAdvice/{selectedLegalAdviceId:int}/listByRelatedLegalAdviceId", Name = "ListByRelatedLegalAdviceId")]
        public JsonResult ListByRelatedLegalAdviceId(int selectedLegalAdviceId)
        {
            Ensure.Argument.NotNull(selectedLegalAdviceId);
            var list = _legalAdviceService.ListByRelatedLegalAdviceId(selectedLegalAdviceId);

            return Json(new JsonResponse(true)
            {
                Data = list
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.LegalAdvice)]
        [HttpPost, Route("~/api/legalAdvice/{code}/calRelevantRecordNum", Name = "CalRelevantRecordNum")]
        public JsonResult CalRelevantRecordNum(string code)
        {
            Ensure.Argument.NotNullOrEmpty(code);
            var LegalAdviceMasterAmount = _legalAdviceService.GetRelatedLegalAdviceAmountByCode(code);
            var map = new Hashtable();
            map.Add("legalAdviceMasterAmount", LegalAdviceMasterAmount);
            return Json(new JsonResponse(true)
            {
                Data = map,
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.LegalAdvice)]
        [HttpPost, Route("~/api/legalAdvice/createLegalAdviceMaster", Name = "CreateLegalAdviceMaster")]
        public JsonResult Create([CustomizeValidator(RuleSet = "default,Create")] LegalAdviceViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var legalAdviceMaster = new LegalAdviceMaster
            {
                LegalAdviceCode = GetLegalAdviceCode(model),
                LegalAdviceType = model.LegalAdviceTypeHeadId,
                VenueType = model.LegalAdviceTypeId,
                LegalAdviceDescription = model.LegalAdviceDescription,
                PartNum = model.PartNum,
                EnclosureNum = model.EnclosureNum,
                EffectiveDate = model.EffectiveDate,
                RequirePspIndicator = model.PSPRequiredId,
                Remarks = model.Remarks,
                IsDeleted = !model.Active,
                RelatedLegalAdviceId = Convert.ToInt32(model.RelatedLegalAdviceId)
            };

            using (_unitOfWork.BeginTransaction())
            {
                _legalAdviceService.CreateLegalAdviceMaster(legalAdviceMaster);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
                Data = legalAdviceMaster.LegalAdviceId,
            }, JsonRequestBehavior.DenyGet);
        }

        public string GetLegalAdviceCode(LegalAdviceViewModel model)
        {
            string mValue = "";

            if (model.LegalAdviceTypeHeadId == "01")
                mValue = model.LegalAdviceTypeHeadId + "-" + model.LegalAdviceTypeId;
            else
                mValue = model.LegalAdviceTypeHeadId + "-" + "X";

            mValue = mValue + "-" + _legalAdviceService.GetLegalAdviceCodeSuffix(mValue + "-");

            return mValue;
        }

        [PspsAuthorize(Allow.LegalAdvice)]
        [HttpPost, Route("~/api/legalAdvice/updateLegalAdviceMaster", Name = "UpdateLegalAdviceMaster")]
        public JsonResult Update([CustomizeValidator(RuleSet = "default,Update")] LegalAdviceViewModel model, string code)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            var legalAdvice = _legalAdviceService.GetLegalAdviceMasterById(Convert.ToInt32(code));

            legalAdvice.LegalAdviceType = model.LegalAdviceTypeHeadId;
            legalAdvice.VenueType = model.LegalAdviceTypeId;
            legalAdvice.LegalAdviceDescription = model.LegalAdviceDescription;
            legalAdvice.PartNum = model.PartNum;
            legalAdvice.EnclosureNum = model.EnclosureNum;
            legalAdvice.EffectiveDate = model.EffectiveDate;
            legalAdvice.RequirePspIndicator = model.PSPRequiredId;
            legalAdvice.Remarks = model.Remarks;
            legalAdvice.IsDeleted = !model.Active;
            legalAdvice.RelatedLegalAdviceId = Convert.ToInt32(model.RelatedLegalAdviceId);

            using (_unitOfWork.BeginTransaction())
            {
                _legalAdviceService.UpdateLegalAdviceMaster(legalAdvice);

                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.LegalAdvice)]
        [HttpPost, Route("~/api/legalAdvice/new/{code}", Name = "GetLegalAdviceMaster")]
        public JsonResult GetLegalAdvice(string code)
        {
            Ensure.Argument.NotNullOrEmpty(code);

            var legalAdvice = _legalAdviceService.GetLegalAdviceMasterById(Convert.ToInt32(code));
            System.Globalization.DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";
            var relatedLegalAdvice = _legalAdviceService.GetLegalAdviceMasterById(legalAdvice.RelatedLegalAdviceId);
            return Json(new JsonResponse(true)
            {
                Data = new LegalAdviceViewModel
                {
                    LegalAdviceTypeHeadId = legalAdvice.LegalAdviceType,
                    LegalAdviceTypeId = legalAdvice.VenueType,
                    LegalAdviceDescription = legalAdvice.LegalAdviceDescription,
                    LegalAdviceCode = legalAdvice.LegalAdviceCode,
                    PartNum = legalAdvice.PartNum,
                    EnclosureNum = legalAdvice.EnclosureNum,
                    EffectiveDate = legalAdvice.EffectiveDate,
                    PSPRequiredId = legalAdvice.RequirePspIndicator,
                    Remarks = legalAdvice.Remarks,
                    Active = !legalAdvice.IsDeleted,
                    RelatedLegalAdviceCode = relatedLegalAdvice != null ? relatedLegalAdvice.LegalAdviceCode : "",
                    RelatedLegalAdviceId = legalAdvice.RelatedLegalAdviceId.ToString(),
                    RelatedLegalAdviceDescription = relatedLegalAdvice != null ? relatedLegalAdvice.LegalAdviceDescription : "",
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.LegalAdvice)]
        [HttpPost, Route("~/api/legalAdvice/{code}/getDesUrl", Name = "getDesUrl")]
        public JsonResult GetDescriptionForDropdown(string code, string code1)
        {
            var des = _legalAdviceService.GetDescriptionForDropdownByType(code, code1);
            return Json(new JsonResponse(true)
            {
                //Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted),
                Data = des
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.LegalAdvice)]
        [HttpPost, Route("~/api/legalAdvice/exportLegalAdviceMaster", Name = "ExportLegalAdviceMaster")]
        public JsonResult ExportLegalAdviceMaster(ExportSettings exportSettings)
        {
            LegalAdviceViewModel model = ((LegalAdviceViewModel)this.HttpContext.Session[LegalAdviceControllerSearchSessionName]);
            var list = _legalAdviceService.GetPage(exportSettings.GridSettings);
            var dataList = (from l in list
                            select new
                            {
                                LegalAdviceId = l.LegalAdviceId,
                                LegalAdviceType = (!String.IsNullOrEmpty(l.LegalAdviceType) ? _lookupService.GetDescription(LookupType.LegalAdviceType, l.LegalAdviceType) : "") + (!String.IsNullOrEmpty(l.VenueType) ? "-" + _lookupService.GetDescription(LookupType.VenueType, l.VenueType) : ""),
                                LegalAdviceCode = l.LegalAdviceCode,
                                LegalAdviceDescription = l.LegalAdviceDescription,
                                PartNum = l.PartNum,
                                EnclosureNum = l.EnclosureNum,
                                EffectiveDate = l.EffectiveDate,
                                RequirePspIndicator = !String.IsNullOrEmpty(l.RequirePspIndicator) ? _lookupService.GetDescription(LookupType.PSPRequiredIndicator, l.RequirePspIndicator) : "",
                                Remarks = l.Remarks
                            }).ToArray();

            //------------------------------------------------------------------------Get Filtering List-----------------------------------------------------------------
            string tmpVal = "";
            List<string> filterCriterias = new List<string>();
            Dictionary<string, string> fieldNames = GetViewModelDescList<LegalAdviceViewModel>();

            if (model.LegalAdviceCode.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["LegalAdviceCode"] + " : " + model.LegalAdviceCode);

            if (model.LegalAdviceTypeHeadId != null)
            {
                var LegalAdviceTypes = _lookupService.getAllLkpInCodec(LookupType.LegalAdviceType);
                if (model.LegalAdviceTypeHeadId == "Others")
                {
                    if (model.LegalAdviceTypeId.IsNotNullOrEmpty())
                    {
                        var VenueTypes = _lookupService.getAllLkpInCodec(LookupType.VenueType);
                        filterCriterias.Add(fieldNames["LegalAdviceTypeHeads"] + " : " + LegalAdviceTypes[model.LegalAdviceTypeHeadId] + " ( " + VenueTypes[model.LegalAdviceTypeId] + " ) ");
                    }
                    else
                    {
                        filterCriterias.Add(fieldNames["LegalAdviceTypeHeads"] + " : " + LegalAdviceTypes[model.LegalAdviceTypeHeadId]);
                    }
                }
                else
                {
                    filterCriterias.Add(fieldNames["LegalAdviceTypeHeads"] + " : " + LegalAdviceTypes[model.LegalAdviceTypeHeadId]);
                }
            }

            if (model.LegalAdviceDescription.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["SearchLegalAdviceDescription"] + " : " + model.LegalAdviceDescription);

            if (model.EffectiveDateStart != null && model.EffectiveDateEnd != null)
            {
                tmpVal = "From " + model.EffectiveDateStart.Value.ToString(DATE_FORMAT) + " to " + model.EffectiveDateEnd.Value.ToString(DATE_FORMAT);
            }
            else if (model.EffectiveDateStart != null)
            {
                tmpVal = "From " + model.EffectiveDateStart.Value.ToString(DATE_FORMAT);
            }
            else if (model.EffectiveDateEnd != null)
            {
                tmpVal = "To " + model.EffectiveDateEnd.Value.ToString(DATE_FORMAT);
            }
            if (tmpVal != "") filterCriterias.Add(fieldNames["EffectiveDateEnd"] + " : " + tmpVal);

            if (model.PartNum.IsNotNullOrEmpty() || model.EnclosureNum.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["SearchEnclosureNum"] + " : " + (model.PartNum == null ? "" : model.PartNum) + " / " + (model.EnclosureNum == null ? "" : model.EnclosureNum));
            }

            if (model.PSPRequiredId != null)
            {
                var PSPRequiredIndicators = _lookupService.getAllLkpInCodec(LookupType.PSPRequiredIndicator);
                filterCriterias.Add(fieldNames["PSPRequireds"] + " : " + PSPRequiredIndicators[model.PSPRequiredId]);
            }
            //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, filterCriterias, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        #endregion REST-like API

        #region "Report"

        [PspsAuthorize(Allow.LegalAdviceReport)]
        public ActionResult Report()
        {
            LegalAdviceReportViewModel model = new LegalAdviceReportViewModel();
            return View(model);
        }

        [PspsAuthorize(Allow.LegalAdviceReport)]
        [HttpPost, Route("~/api/report/r26/generate", Name = "R26Generate")]
        public JsonResult R26Generate(LegalAdviceReportViewModel model)
        {
            Ensure.Argument.NotNull(model);
            var reportId = "R26";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var ms = _reportService.GenerateR26Excel(templatePath, model.EffectiveDateStart, model.EffectiveDateEnd);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        #endregion "Report"
    }
}