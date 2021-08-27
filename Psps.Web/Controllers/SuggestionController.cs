using CsvHelper;
using DocxGenerator.Library;
using FluentValidation.Mvc;
using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Models.Dto.Lookups;
using Psps.Models.Dto.Reports;
using Psps.Services.Lookups;
using Psps.Services.Report;
using Psps.Services.Suggestions;
using Psps.Services.SystemMessages;
using Psps.Services.SystemParameters;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Extensions;
using Psps.Web.Core.Mvc;
using Psps.Web.Mappings;
using Psps.Web.ViewModels.Lookup;
using Psps.Web.ViewModels.Suggestion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("Suggestion"), Route("{action=Search}")]
    public class SuggestionController : BaseController
    {
        #region "Fileds  & Page Load"

        private readonly static string SuggestionControllerSearchSessionName = "SuggestionControllerSearch";
        private readonly string DATE_FORMAT = "dd/MM/yyyy";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;
        private readonly ILookupService _lookupService;
        private readonly ISuggestionMasterService _suggestionMasterService;
        private readonly ISuggestionDocService _suggestionDocService;
        private readonly ISuggestionAttachmentService _suggestionAttachmentService;
        private readonly IParameterService _parameterService;
        private readonly IReportService _reportService;

        public SuggestionController(IUnitOfWork unitOfWork, IMessageService messageService, ISuggestionMasterService suggestionMasterService
            , ISuggestionDocService suggestionDocService, ILookupService lookupService, IParameterService parameterService
            , ISuggestionAttachmentService suggestionAttachmentService, IReportService reportService)
        {
            this._suggestionMasterService = suggestionMasterService;
            this._suggestionDocService = suggestionDocService;
            this._suggestionAttachmentService = suggestionAttachmentService;
            this._parameterService = parameterService;
            this._unitOfWork = unitOfWork;
            this._messageService = messageService;
            this._lookupService = lookupService;
            this._reportService = reportService;
        }

        #endregion "Fileds  & Page Load"

        //#region "Index"

        //[PspsAuthorize(Allow.SuggestionMaster)]
        //[HttpGet, Route]
        //public ActionResult Index()
        //{
        //    SuggestionMasterViewModel model = new SuggestionMasterViewModel();
        //    return View(model);
        //}

        //#endregion "Index"

        [RuleSetForClientSideMessagesAttribute("default", "Update")]
        public PartialViewResult RenderSuggestionTemplateModal()
        {
            SuggestionDocViewModel model = new SuggestionDocViewModel();
            return PartialView("_SuggestionTemplateModal", model);
        }

        #region "Search"

        [PspsAuthorize(Allow.SuggestionMaster)]
        [HttpGet, Route("Search")]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        public ActionResult Search()
        {
            SuggestionMasterViewModel model = new SuggestionMasterViewModel();
            model.SuggestionRefNumPR = DateTime.Now.Year.ToString();

            this.HttpContext.Session[SuggestionControllerSearchSessionName] = null;

            var SuggestionSource = _lookupService.getAllLkpInCodec(LookupType.SuggestionSource);
            model.SuggestionSources = SuggestionSource;

            var SuggestionActivityConcern = _lookupService.getAllLkpInCodec(LookupType.SuggestionActivityConcern);
            model.ActivityConcerns = SuggestionActivityConcern;

            var SuggestionNature = _lookupService.getAllLkpInCodec(LookupType.SuggestionNature);
            model.Natures = SuggestionNature;

            return View(model);
        }

        [PspsAuthorize(Allow.SuggestionMaster)]
        [HttpGet, Route("{id}/template/{suggestionDocId}/generate", Name = "GenerateSuggestionTemplate")]
        public ActionResult GenerateSuggestionTemplate(int id, int suggestionDocId)
        {
            Ensure.Argument.NotNull(suggestionDocId);
            Ensure.Argument.NotNull(id);

            var template = _suggestionDocService.GetSuggestionDocById(suggestionDocId);
            var suggestion = _suggestionDocService.GetSuggestionDocViewById(id);
            var sysParam = _parameterService.GetParameterByCode("OrganisationTemplatePath");
            var inputFilePath = Path.Combine(@sysParam.Value, template.FileLocation);

            if (!System.IO.File.Exists(inputFilePath))
                throw new HttpException((int)System.Net.HttpStatusCode.NotFound, "Template not found");

            SimpleDocumentGenerator<SuggestionDocView> docGenerator = new SimpleDocumentGenerator<SuggestionDocView>(new DocumentGenerationInfo
            {
                DataContext = suggestion,
                TemplateData = System.IO.File.ReadAllBytes(inputFilePath)
            });
            return File(docGenerator.GenerateDocument(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", template.DocName + ".docx");
        }

        #endregion "Search"

        #region "NewSuggestion"

        [PspsAuthorize(Allow.SuggestionMaster)]
        [HttpGet, Route("New", Name = "NewSuggestion")]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        public ActionResult New()
        {
            SuggestionMasterViewModel model = new SuggestionMasterViewModel();
            var SuggestionSource = _lookupService.getAllLkpInCodec(LookupType.SuggestionSource);
            model.SuggestionSources = SuggestionSource;

            var SuggestionActivityConcern = _lookupService.getAllLkpInCodec(LookupType.SuggestionActivityConcern);
            model.ActivityConcerns = SuggestionActivityConcern;

            var SuggestionNature = _lookupService.getAllLkpInCodec(LookupType.SuggestionNature);
            model.Natures = SuggestionNature;

            return View(model);
        }

        [PspsAuthorize(Allow.SuggestionMaster)]
        [HttpGet, Route("{code}/Edit/", Name = "NewSuggestionMaster")]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        public ActionResult Edit(string code)
        {
            SuggestionMasterViewModel model = new SuggestionMasterViewModel();
            var SuggestionSource = _lookupService.getAllLkpInCodec(LookupType.SuggestionSource);

            var SuggestionActivityConcern = _lookupService.getAllLkpInCodec(LookupType.SuggestionActivityConcern);

            var SuggestionNature = _lookupService.getAllLkpInCodec(LookupType.SuggestionNature);

            Ensure.Argument.NotNullOrEmpty(code);

            var suggestionMaster = _suggestionMasterService.GetSuggestionMasterById(Convert.ToInt32(code));

            var Data = new SuggestionMasterViewModel
              {
                  ActivityConcerns = SuggestionActivityConcern,
                  Natures = SuggestionNature,
                  SuggestionSources = SuggestionSource,
                  SuggestionMasterId = suggestionMaster.SuggestionMasterId.ToString()
              };

            return View("New", Data);
        }

        [RuleSetForClientSideMessagesAttribute("default", "CreateSuggestionAttachment", "UpdateSuggestionAttachment")]
        public PartialViewResult RenderSuggestionAttachmentDetailModal()
        {
            SuggestionMasterViewModel model = new SuggestionMasterViewModel();
            return PartialView("_SuggestionAttachmentDetailModal", model);
        }

        [PspsAuthorize(Allow.SuggestionMaster)]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        [HttpGet, Route("ReturnSearch", Name = "ReturnSearchSug")]
        public ActionResult ReturnSearchSug()
        {
            SuggestionMasterViewModel model = new SuggestionMasterViewModel();

            if (this.HttpContext.Session[SuggestionControllerSearchSessionName] != null)
                model = ((SuggestionMasterViewModel)(this.HttpContext.Session[SuggestionControllerSearchSessionName]));

            var SuggestionSource = _lookupService.getAllLkpInCodec(LookupType.SuggestionSource);
            model.SuggestionSources = SuggestionSource;

            var SuggestionActivityConcern = _lookupService.getAllLkpInCodec(LookupType.SuggestionActivityConcern);
            model.ActivityConcerns = SuggestionActivityConcern;

            var SuggestionNature = _lookupService.getAllLkpInCodec(LookupType.SuggestionNature);
            model.Natures = SuggestionNature;

            return View("Search", model);
        }

        [PspsAuthorize(Allow.SuggestionMaster)]
        [HttpGet, Route("~/api/suggestion/getsearchdatafromsessionsug", Name = "GetSearchDataFromSessionSug")]
        public JsonResult GetSearchDataFromSessionSug()
        {
            SuggestionMasterViewModel model = null;

            if (this.HttpContext.Session[SuggestionControllerSearchSessionName] != null)
                model = ((SuggestionMasterViewModel)(this.HttpContext.Session[SuggestionControllerSearchSessionName]));

            return Json(new JsonResponse(true)
            {
                //Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted),
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion "NewSuggestion"

        #region REST-like API

        //[PspsAuthorize(Allow.ListSuggestionMaster)]
        [HttpPost, Route("~/api/suggestion/listsuggestionmaster", Name = "ListSuggestionMaster")]
        //public JsonResult ListSuggestionMaster(GridSettings grid)
        public JsonResult ListSuggestionMaster(SuggestionMasterViewModel model, GridSettings grid)
        {
            GetSuggestionMasterSearchGrid(model, grid);
            var suggestionMasters = _suggestionMasterService.GetPage(grid);
            this.HttpContext.Session[SuggestionControllerSearchSessionName] = model;

            var gridResult = new GridResult
            {
                TotalPages = suggestionMasters.TotalPages,
                CurrentPageIndex = suggestionMasters.CurrentPageIndex,
                TotalCount = suggestionMasters.TotalCount,
                Data = (from s in suggestionMasters
                        select new
                        {
                            SuggestionMasterId = s.SuggestionMasterId,
                            SuggestionRefNum = s.SuggestionRefNum,
                            SuggestionSource = s.SuggestionSource != null ? _lookupService.GetDescription(LookupType.SuggestionSource, s.SuggestionSource) : "",
                            SuggestionActivityConcern = s.SuggestionActivityConcern != null ? _lookupService.GetDescription(LookupType.SuggestionActivityConcern, s.SuggestionActivityConcern) : "",
                            SuggestionNature = s.SuggestionNature != null ? _lookupService.GetDescription(LookupType.SuggestionNature, s.SuggestionNature) : "",
                            SuggestionDate = s.SuggestionDate,
                            PartNum = s.PartNum,
                            EnclosureNum = s.EnclosureNum,
                            SuggestionDescription = s.SuggestionDescription,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #region Tab

        #region Amount

        [PspsAuthorize(Allow.SuggestionMaster)]
        [HttpPost, Route("~/api/Suggestion/{code}/calRelevantRecords", Name = "CalRelevantRecords")]
        public JsonResult CalRelevantRecords(string code)
        {
            Ensure.Argument.NotNullOrEmpty(code);
            var TemplateAmount = _suggestionDocService.GetSuggestionDocAmount();
            var AttachmentAmount = _suggestionAttachmentService.GetSuggestionAttachmentAmountByCode(code);
            var map = new Hashtable();
            map.Add("templateAmount", TemplateAmount);
            map.Add("attachmentAmount", AttachmentAmount);
            return Json(new JsonResponse(true)
            {
                Data = map,
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion Amount

        #region Template

        [PspsAuthorize(Allow.SuggestionMaster)]
        [HttpGet, Route("~/api/suggestion/template/list/tab", Name = "ListSuggestionTemplateTab")]
        public JsonResult ListSuggestionTemplateTab(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            grid.AddDefaultRule(new Rule()
            {
                field = "DocStatus",
                data = "true",
                op = WhereOperation.Equal.ToEnumValue()
            });

            var template = _suggestionDocService.GetPage(grid);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = template.TotalPages,
                CurrentPageIndex = template.CurrentPageIndex,
                TotalCount = template.TotalCount,
                Data = (from l in template
                        //where l.DocStatus == true
                        select new
                        {
                            DocNum = l.DocNum,
                            DocName = l.DocName,
                            RowVersion = l.RowVersion,
                            SuggestionDocId = l.SuggestionDocId
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion Template

        #region SuggestionAttachment

        [PspsAuthorize(Allow.SuggestionMaster)]
        [HttpGet, Route("~/api/listSuggestionAttachment", Name = "ListSuggestionAttachment")]
        public JsonResult ListSuggestionAttachment(GridSettings grid, string code)
        {
            Ensure.Argument.NotNull(grid);
            Ensure.Argument.NotNullOrEmpty(code);

            grid.AddDefaultRule(new Rule()
            {
                field = "SuggestionMaster.SuggestionMasterId",
                data = code,
                op = WhereOperation.Equal.ToEnumValue()
            });

            var suggestionAttachment = _suggestionAttachmentService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = suggestionAttachment.TotalPages,
                CurrentPageIndex = suggestionAttachment.CurrentPageIndex,
                TotalCount = suggestionAttachment.TotalCount,
                Data = (from s in suggestionAttachment
                        select new
                        {
                            attachmentId = s.SuggestionAttachmentId,
                            fileName = s.FileName,
                            uploadedBy = s.CreatedByPost,
                            uploadedTime = s.CreatedOn,
                            remarks = s.FileDescription,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.SuggestionMaster)]
        [HttpPost, Route("~/api/suggestion/{code}/createSuggestionAttachment", Name = "CreateSuggestionAttachment")]
        public JsonResult CreateSuggestionAttachment([CustomizeValidator(RuleSet = "default,CreateSuggestionAttachment")] SuggestionMasterViewModel model, string code)
        {
            Ensure.Argument.NotNull(model);
            Ensure.Argument.NotNullOrEmpty(code, "code");
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the SuggestionMaster record by given the ID
            var suggestionMaster = _suggestionMasterService.GetSuggestionMasterById(Convert.ToInt32(code));

            // Get the root path of the Attachment from DB
            var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.SUGGESTION_ATTACHMENT_PATH);

            // Rename the file by adding current time
            var fileName = Path.GetFileName(model.AttachmentDocument.FileName);
            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            string generatedFileName = string.Format("{0}-{1}", time, fileName);

            // Set the root path by adding the SuggestionMasterId folder     ( [Root Folder of Attachment] \ [SuggestionMasterId Folder] )
            string rootPath = Path.Combine(attachmentPath.Value, suggestionMaster.SuggestionMasterId.ToString());
            // Form the Relative Path for storing in DB         ( [SuggestionMasterId Folder] \ [File Name] )
            // and Absolute Path for actually saving the file   ( [Root Folder of Attachment] \ [SuggestionMasterId Folder] \ [File Name] )
            string relativePath = Path.Combine(suggestionMaster.SuggestionMasterId.ToString(), generatedFileName);
            string absolutePath = Path.Combine(rootPath, generatedFileName);

            // Create new OrgAttachment row and fill the value
            var attachment = new SuggestionAttachment();
            attachment.SuggestionMaster = suggestionMaster;
            attachment.FileLocation = relativePath;
            attachment.FileName = model.AttachmentName;
            attachment.FileDescription = model.AttachmentRemark;

            using (_unitOfWork.BeginTransaction())
            {
                // Save the file to the Absolute Path
                if (CommonHelper.CreateFolderIfNeeded(rootPath))
                {
                    model.AttachmentDocument.SaveAs(absolutePath);
                }

                // Insert record to DB and commit
                _suggestionAttachmentService.CreateSuggestionAttachment(attachment);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated)
            }, "text/html", JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.SuggestionMaster)]
        [HttpPost, Route("~/api/suggestion/updateSuggestionAttachment", Name = "UpdateSuggestionAttachment")]
        public JsonResult UpdateSuggestionAttachment([CustomizeValidator(RuleSet = "default,UpdateSuggestionAttachment")] SuggestionMasterViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the SuggestionAttachment record by given the ID
            var AttachmentId = Convert.ToInt32(model.AttachmentId);
            var attachment = _suggestionAttachmentService.GetSuggestionAttachmentById(AttachmentId);

            // Fill the update values
            attachment.FileName = model.AttachmentName;
            attachment.FileDescription = model.AttachmentRemark;

            // Get the root path of the Attachment from DB
            var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.SUGGESTION_ATTACHMENT_PATH);

            using (_unitOfWork.BeginTransaction())
            {
                // If new file need to be upload
                if (model.AttachmentDocument != null)
                {
                    // Reforming the file name by adding current time
                    var fileName = Path.GetFileName(model.AttachmentDocument.FileName);
                    var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
                    string generatedFileName = string.Format("{0}-{1}", time, fileName);

                    // Set the root path by adding the SuggestionMasterId folder     ( [Root Folder of Attachment] \ [SuggestionMasterId Folder] )
                    string rootPath = Path.Combine(attachmentPath.Value, attachment.SuggestionMaster.SuggestionMasterId.ToString());
                    // Form the Relative Path for storing in DB         ( [SuggestionMasterId Folder] \ [File Name] )
                    // and Absolute Path for actually saving the file   ( [Root Folder of Attachment] \ [SuggestionMasterId Folder] \ [File Name] )
                    string relativePath = Path.Combine(attachment.SuggestionMaster.SuggestionMasterId.ToString(), generatedFileName);
                    string absolutePath = Path.Combine(rootPath, generatedFileName);

                    // Save the new file
                    if (CommonHelper.CreateFolderIfNeeded(rootPath))
                    {
                        model.AttachmentDocument.SaveAs(absolutePath);
                    }

                    // Form the path of the old file
                    string absolutePathOfOldFile = Path.Combine(attachmentPath.Value, attachment.FileLocation);

                    // Delete the old file
                    if (System.IO.File.Exists(absolutePathOfOldFile))
                    {
                        System.IO.File.Delete(absolutePathOfOldFile);
                    }

                    // Replace with the new path
                    attachment.FileLocation = relativePath;
                }
                // Update DB record and commit
                _suggestionAttachmentService.UpdateSuggestionAttachment(attachment);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, "text/html", JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.SuggestionMaster)]
        [HttpGet, Route("~/api/suggestion/{attachmentId:int}/downloadSuggestionAttachment", Name = "DownloadSuggestionAttachment")]
        public FileResult DownloadSuggestionAttachment(int attachmentId)
        {
            // Get the SuggestionAttachment record by given the ID
            var suggestionAttachment = _suggestionAttachmentService.GetSuggestionAttachmentById(attachmentId);
            Ensure.NotNull(suggestionAttachment, "No Suggestion Attachment found with the specified id");

            // Get the root path of the Attachment from DB
            // and combine with the FileLocation to get the Absolute Path that the file actually stored at
            var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.SUGGESTION_ATTACHMENT_PATH);
            string rootPath = attachmentPath.Value;
            var absolutePath = Path.Combine(rootPath, suggestionAttachment.FileLocation);
            // ( [Root Folder of Attachment] \ [SuggestionMasterId Folder] \ [File Name] )

            // Set the file name for saving
            string fileName = suggestionAttachment.FileName + Path.GetExtension(Path.GetFileName(suggestionAttachment.FileLocation));
            return FileDownload(absolutePath, fileName);
        }

        [PspsAuthorize(Allow.SuggestionMaster)]
        [HttpPost, Route("~/api/suggestion/{attachmentId:int}/deleteSuggestionAttachment", Name = "DeleteSuggestionAttachment")]
        public JsonResult DeleteSuggestionAttachment(int attachmentId)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            // Get the SuggestionAttachment record by given the ID
            var suggestionAttachment = _suggestionAttachmentService.GetSuggestionAttachmentById(attachmentId);
            Ensure.NotNull(suggestionAttachment, "No Suggestion Attachment found with the specified id");

            using (_unitOfWork.BeginTransaction())
            {
                // Get the root path of the Attachment file from DB
                // and combine with the FileLocation to get the Absolute Path that the file actually stored at
                var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.SUGGESTION_ATTACHMENT_PATH);
                var absolutePath = Path.Combine(attachmentPath.Value, suggestionAttachment.FileLocation);
                // ( [Root Folder of Attachment] \ [SuggestionMasterId Folder] \ [File Name] )

                // Delete the record in DB (set IsDeleted flag)
                _suggestionAttachmentService.DeleteSuggestionAttachment(suggestionAttachment);

                // Delete the Attachment file if exists
                if (System.IO.File.Exists(absolutePath))
                {
                    System.IO.File.Delete(absolutePath);
                }

                // Commit Delete
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted)
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion SuggestionAttachment

        #endregion Tab

        [PspsAuthorize(Allow.SuggestionMaster)]
        [HttpPost, Route("~/api/suggestion/new", Name = "CreateSuggestionMaster")]
        public JsonResult Create([CustomizeValidator(RuleSet = "default,Create")] SuggestionMasterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var suggestionMaster = new SuggestionMaster
            {
                SuggestionRefNum = _suggestionMasterService.CreateSuggestionRefNum(),
                SuggestionSource = model.SuggestionSourceId,
                SuggestionSourceOther = model.SuggestionSourceOther,
                SuggestionActivityConcern = model.ActivityConcernId,
                SuggestionActivityConcernOther = model.SuggestionActivityConcernOther,
                SuggestionNature = model.NatureId,
                SuggestionDate = model.SuggestionDate,
                SuggestionSenderName = model.SenderName,
                SuggestionDescription = model.SuggestionDescription,
                PartNum = model.PartNum,
                EnclosureNum = model.EnclosureNum,
                Remark = model.Remark,
                AcknowledgementSentDate = model.AcknowledgementSentDate
            };

            using (_unitOfWork.BeginTransaction())
            {
                _suggestionMasterService.CreateSuggestionMaster(suggestionMaster);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
                Data = suggestionMaster.SuggestionMasterId,
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.SuggestionMaster)]
        [HttpPost, Route("~/api/suggestion/updateSuggestionMaster", Name = "UpdateSuggestionMaster")]
        public JsonResult Update([CustomizeValidator(RuleSet = "default,Update")] SuggestionMasterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            var suggestionMaster = _suggestionMasterService.GetSuggestionMasterById(Convert.ToInt32(model.SuggestionMasterId));
            var oldSuggestionMaster = new SuggestionMaster();
            AutoMapper.Mapper.Map(suggestionMaster, oldSuggestionMaster);

            suggestionMaster.SuggestionRefNum = model.SuggestionRefNum;
            suggestionMaster.SuggestionSource = model.SuggestionSourceId;
            suggestionMaster.SuggestionSourceOther = model.SuggestionSourceOther;
            suggestionMaster.SuggestionActivityConcern = model.ActivityConcernId;
            suggestionMaster.SuggestionActivityConcernOther = model.SuggestionActivityConcernOther;
            suggestionMaster.SuggestionNature = model.NatureId;
            suggestionMaster.SuggestionDate = model.SuggestionDate;
            suggestionMaster.SuggestionSenderName = model.SenderName;
            suggestionMaster.SuggestionDescription = model.SuggestionDescription;
            suggestionMaster.AcknowledgementSentDate = model.AcknowledgementSentDate;
            suggestionMaster.PartNum = model.PartNum;
            suggestionMaster.EnclosureNum = model.EnclosureNum;
            suggestionMaster.Remark = model.Remark;

            using (_unitOfWork.BeginTransaction())
            {
                _suggestionMasterService.UpdateSuggestionMaster(oldSuggestionMaster, suggestionMaster);

                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.SuggestionMaster)]
        [HttpPost, Route("~/api/suggestion/{code}/getSuggestionMaster", Name = "GetSuggestionMaster")]
        public JsonResult GetSuggestion(string code)
        {
            Ensure.Argument.NotNullOrEmpty(code);

            var suggestionMaster = _suggestionMasterService.GetSuggestionMasterById(Convert.ToInt32(code));
            System.Globalization.DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";

            return Json(new JsonResponse(true)
            {
                Data = new SuggestionMasterViewModel
                {
                    SuggestionMasterId = suggestionMaster.SuggestionMasterId.ToString(),
                    SuggestionRefNum = suggestionMaster.SuggestionRefNum,
                    SuggestionSourceId = suggestionMaster.SuggestionSource,
                    ActivityConcernId = suggestionMaster.SuggestionActivityConcern,
                    SuggestionSourceOther = suggestionMaster.SuggestionSourceOther,
                    SuggestionActivityConcernOther = suggestionMaster.SuggestionActivityConcernOther,
                    NatureId = suggestionMaster.SuggestionNature,
                    SuggestionDate = suggestionMaster.SuggestionDate,
                    SenderName = suggestionMaster.SuggestionSenderName,
                    SuggestionDescription = suggestionMaster.SuggestionDescription,
                    PartNum = suggestionMaster.PartNum,
                    EnclosureNum = suggestionMaster.EnclosureNum,
                    Remark = suggestionMaster.Remark,
                    AcknowledgementSentDate = suggestionMaster.AcknowledgementSentDate
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.SuggestionMaster)]
        [HttpPost, Route("~/api/suggestion/export", Name = "ExportSuggestionMaster")]
        public JsonResult ExportSuggestionMaster(ExportSettings exportSettings)
        {
            SuggestionMasterViewModel model = ((SuggestionMasterViewModel)this.HttpContext.Session[SuggestionControllerSearchSessionName]);
            if (model != null)
            {
                exportSettings.GridSettings = GetSuggestionMasterSearchGrid(model, exportSettings.GridSettings);
            }
            var suggestionMaster = _suggestionMasterService.GetPage(exportSettings.GridSettings);

            var dataList = (from s in suggestionMaster
                            select new
                            {
                                SuggestionMasterId = s.SuggestionMasterId,
                                SuggestionRefNum = s.SuggestionRefNum,
                                SuggestionSource = s.SuggestionSource != null ? _lookupService.GetDescription(LookupType.SuggestionSource, s.SuggestionSource) : "",
                                SuggestionActivityConcern = s.SuggestionActivityConcern != null ? _lookupService.GetDescription(LookupType.SuggestionActivityConcern, s.SuggestionActivityConcern) : "",
                                SuggestionNature = s.SuggestionNature != null ? _lookupService.GetDescription(LookupType.SuggestionNature, s.SuggestionNature) : "",
                                SuggestionDate = s.SuggestionDate,
                                PartNum = s.PartNum,
                                EnclosureNum = s.EnclosureNum,
                                SuggestionDescription = s.SuggestionDescription,
                            }).ToArray();

            //------------------------------------------------------------------------Get Filtering List-----------------------------------------------------------------
            string tmpVal = "";
            List<string> filterCriterias = new List<string>();
            Dictionary<string, string> fieldNames = GetViewModelDescList<SuggestionMasterViewModel>();

            if (model.SuggestionRefNumPR.IsNotNullOrEmpty() || model.SuggestionRefNum.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["SuggestionRefNumPR"] + " : " + (model.SuggestionRefNumPR == null ? "" : model.SuggestionRefNumPR) + "S" + (model.SuggestionRefNum == null ? "" : model.SuggestionRefNum));
            }

            if (model.SuggestionDateStart != null && model.SuggestionDateEnd != null)
            {
                tmpVal = "From " + model.SuggestionDateStart.Value.ToString(DATE_FORMAT) + " to " + model.SuggestionDateEnd.Value.ToString(DATE_FORMAT);
            }
            else if (model.SuggestionDateStart != null)
            {
                tmpVal = "From " + model.SuggestionDateStart.Value.ToString(DATE_FORMAT);
            }
            else if (model.SuggestionDateEnd != null)
            {
                tmpVal = "To " + model.SuggestionDateEnd.Value.ToString(DATE_FORMAT);
            }
            if (tmpVal != "") filterCriterias.Add(fieldNames["SuggestionDateEnd"] + " : " + tmpVal);

            if (model.SuggestionSourceId != null)
            {
                var SuggestionSource = _lookupService.getAllLkpInCodec(LookupType.SuggestionSource);
                if (model.SuggestionSourceId == "Others")
                {
                    if (model.SuggestionSourceOther.IsNotNullOrEmpty())
                    {
                        filterCriterias.Add(fieldNames["SuggestionSources"] + " : " + SuggestionSource[model.SuggestionSourceId] + " ( " + model.SuggestionSourceOther + " ) ");
                    }
                    else
                    {
                        filterCriterias.Add(fieldNames["SuggestionSources"] + " : " + SuggestionSource[model.SuggestionSourceId]);
                    }
                }
                else
                {
                    filterCriterias.Add(fieldNames["SuggestionSources"] + " : " + SuggestionSource[model.SuggestionSourceId]);
                }
            }

            if (model.ActivityConcernId != null)
            {
                var SuggestionActivityConcern = _lookupService.getAllLkpInCodec(LookupType.SuggestionActivityConcern);

                if (model.ActivityConcernId == "Others")
                {
                    if (model.SuggestionActivityConcernOther.IsNotNullOrEmpty())
                    {
                        filterCriterias.Add(fieldNames["ActivityConcerns"] + " : " + SuggestionActivityConcern[model.ActivityConcernId] + " ( " + model.SuggestionActivityConcernOther + " ) ");
                    }
                    else
                    {
                        filterCriterias.Add(fieldNames["ActivityConcerns"] + " : " + SuggestionActivityConcern[model.ActivityConcernId]);
                    }
                }
                else
                {
                    filterCriterias.Add(fieldNames["ActivityConcerns"] + " : " + SuggestionActivityConcern[model.ActivityConcernId]);
                }
            }
            //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, filterCriterias, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        private GridSettings GetSuggestionMasterSearchGrid(SuggestionMasterViewModel model, GridSettings grid)
        {
            grid.AddDefaultRule(new Rule()
            {
                field = "IsDeleted",
                data = "false",
                op = WhereOperation.Equal.ToEnumValue()
            });

            if (!String.IsNullOrEmpty(model.SuggestionRefNumPR))
            {
                grid.Where.rules.Add(new Rule()
                {
                    field = "SuggestionRefNum",
                    data = model.SuggestionRefNumPR,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.SuggestionRefNum))
            {
                grid.Where.rules.Add(new Rule()
                {
                    field = "SuggestionRefNum",
                    data = model.SuggestionRefNum,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }

            if (model.SuggestionDateStart != null)
            {
                grid.Where.rules.Add(new Rule()
                {
                    field = "SuggestionDate",
                    data = model.SuggestionDateStart.Value.ToString("dd/MM/yyyy"),
                    op = WhereOperation.GreaterThanOrEqual.ToEnumValue()
                });
            }

            if (model.SuggestionDateEnd != null)
            {
                grid.Where.rules.Add(new Rule()
                {
                    field = "SuggestionDate",
                    data = model.SuggestionDateEnd.Value.ToString("dd/MM/yyyy"),
                    op = WhereOperation.LessThanOrEqual.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.SuggestionSourceId))
            {
                grid.Where.rules.Add(new Rule()
                {
                    field = "SuggestionSource",
                    data = model.SuggestionSourceId,
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.SuggestionSourceOther))
            {
                grid.Where.rules.Add(new Rule()
                {
                    field = "SuggestionSourceOther",
                    data = model.SuggestionSourceOther,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.ActivityConcernId))
            {
                grid.Where.rules.Add(new Rule()
                {
                    field = "SuggestionActivityConcern",
                    data = model.ActivityConcernId,
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.SuggestionActivityConcernOther))
            {
                grid.Where.rules.Add(new Rule()
                {
                    field = "SuggestionActivityConcernOther",
                    data = model.SuggestionActivityConcernOther,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }
            return grid;
        }

        #endregion REST-like API

        #region Report

        [PspsAuthorize(Allow.SuggestionReport)]
        [HttpGet, Route("Report")]
        public ActionResult Report()
        {
            SuggestionMasterViewModel model = new SuggestionMasterViewModel();
            return View(model);

            //SuggestionDocViewModel model = new SuggestionDocViewModel();
            //return View("Template", model);
        }

        [PspsAuthorize(Allow.SuggestionReport)]
        [HttpGet, Route("~/api/suggestion/report/list", Name = "ListSuggestionReport")]
        public JsonResult ListSuggestionReport(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            var suggestion = this._suggestionDocService.GetPage(grid);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = suggestion.TotalPages,
                CurrentPageIndex = suggestion.CurrentPageIndex,
                TotalCount = suggestion.TotalCount,
                Data = (from s in suggestion
                        select new
                        {
                            fileRefNum = s.Id,
                            description = s.DocName,
                            //parameters = s.SuggestionDate
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.SuggestionReport)]
        [HttpPost, Route("~/api/report/r20/generate", Name = "R20Generate")]
        public JsonResult R20Genrate(SuggestionMasterViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            String reportId = "R20";
            String templatePath = System.Web.HttpContext.Current.Server.MapPath("~/Reports/") + reportId + ".xlsx";
            //MemoryStream ms = _suggestionMasterService.GenerateR20PDF(templatePath, model.R20_DateFrom, model.R20_DateTo);
            MemoryStream ms = _reportService.GenerateR20Excel(templatePath, model.R20_DateFrom, model.R20_DateTo);
            return JsonReportResult(reportId, reportId + ".xlsx", ms);
        }

        #endregion Report

        #region Template

        [PspsAuthorize(Allow.SuggestionTemplate)]
        [HttpGet, Route("Template")]
        public ActionResult Template()
        {
            SuggestionDocViewModel model = new SuggestionDocViewModel();
            return View(model);
        }

        [PspsAuthorize(Allow.SuggestionTemplate)]
        [HttpGet, Route("~/api/suggestion/template/list", Name = "ListSuggestionTemplate")]
        public JsonResult ListSuggestionTemplate(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);
            var suggestion = _suggestionDocService.GetSuggestionDocSummaryViewPage(grid);
            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = suggestion.TotalPages,
                CurrentPageIndex = suggestion.CurrentPageIndex,
                TotalCount = suggestion.TotalCount,
                Data = (from s in suggestion
                        select new
                        {
                            DocNum = s.DocNum,
                            DocName = s.DocName,
                            VersionNum = s.VersionNum,
                            Enabled = s.Enabled,
                            SuggestionDocId = s.SuggestionDocId,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.SuggestionTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/suggestion/template/new", Name = "NewSuggesionTemplate")]
        public JsonResult NewSuggesionTemplate([CustomizeValidator(RuleSet = "default,Create,CreateSuggestionDoc")] SuggestionDocViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the root path of the Template file from DB
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.SUGGESTION_TEMPLATE_PATH);
            Ensure.NotNull(templatePath, "No letter found with the specified code");

            // Rename the file name by adding the current times
            var fileName = Path.GetFileName(model.File.FileName);
            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            string generatedFileName = string.Format("{0}-{1}", time, fileName);

            // Form the Relative Path for storing in DB
            // and Absolute Path for actually saving the file
            string rootPath = templatePath.Value;
            string relativePath = generatedFileName;
            string absolutePath = Path.Combine(rootPath, generatedFileName);

            // Create new SuggestionDoc row and fill the value
            var suggestionDoc = new SuggestionDoc();
            suggestionDoc.DocNum = model.DocNum;
            suggestionDoc.DocStatus = true;
            suggestionDoc.DocName = model.Description;
            suggestionDoc.VersionNum = model.Version;
            suggestionDoc.RowVersion = model.RowVersion;
            suggestionDoc.FileLocation = relativePath;

            using (_unitOfWork.BeginTransaction())
            {
                // Save the file to the Absolute Path
                if (CommonHelper.CreateFolderIfNeeded(rootPath))
                {
                    model.File.SaveAs(absolutePath);
                }

                // Insert record to DB and commit
                this._suggestionDocService.CreateSuggestionDoc(suggestionDoc);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
            }, "text/html", JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.SuggestionTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/suggestion/template/{suggestionDocId:int}/edit", Name = "EditSuggestionTemplate")]
        public JsonResult EditSuggestionTemplate(int suggestionDocId, [CustomizeValidator(RuleSet = "default,UpdateSuggestionDoc")]  SuggestionDocViewModel model)
        {
            Ensure.Argument.NotNullOrEmpty(suggestionDocId.ToString());
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            SuggestionDoc suggestion = new SuggestionDoc();
            suggestion = this._suggestionDocService.GetSuggestionDocById(suggestionDocId);
            suggestion.DocNum = model.DocNum;
            suggestion.DocName = model.Description;
            suggestion.VersionNum = model.Version;
            suggestion.DocStatus = model.IsActive;

            using (_unitOfWork.BeginTransaction())
            {
                this._suggestionDocService.UpdateSuggestionDoc(suggestion);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion Template

        #region "Suggestion Template Version"

        [PspsAuthorize(Allow.SuggestionTemplate)]
        [HttpGet, Route("Template/Version/{suggestionDocId:int}", Name = "SuggestionVersion")]
        //[RuleSetForClientSideMessagesAttribute("default", "Create", "NewVersion")]
        public ActionResult Version(int suggestionDocId)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            var suggestion = this._suggestionDocService.GetSuggestionDocById(suggestionDocId);

            Ensure.NotNull(suggestion, "No letter found with the specified id");
            SuggestionDocViewModel model = new SuggestionDocViewModel();
            model.DocNum = suggestion.DocNum;
            return View(model);
        }

        [PspsAuthorize(Allow.SuggestionTemplate)]
        [HttpGet, Route("~/api/suggestion/template/{docNum}/version/list", Name = "ListSuggestionVersion")]
        public JsonResult ListVersion(GridSettings grid, string docNum)
        {
            Ensure.Argument.NotNull(grid);
            var suggestion = this._suggestionDocService.GetPage(grid, docNum);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = suggestion.TotalPages,
                CurrentPageIndex = suggestion.CurrentPageIndex,
                TotalCount = suggestion.TotalCount,
                Data = (from s in suggestion
                        select new
                        {
                            DocNum = s.DocNum,
                            DocName = s.DocName,
                            VersionNum = s.VersionNum,
                            DocStatus = s.DocStatus,
                            RowVersion = s.RowVersion,
                            suggestionDocId = s.SuggestionDocId
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.SuggestionTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/suggestion/template/version/new", Name = "NewVersionLetter")]
        public JsonResult VersionNew([CustomizeValidator(RuleSet = "default,NewVersion")]SuggestionDocViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the root path of the Template from DB
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.SUGGESTION_TEMPLATE_PATH);

            // Rename the file name by adding the current time
            string fileName = Path.GetFileName(model.File.FileName);
            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            string generatedFileName = string.Format("{0}-{1}", time, fileName);

            // Set the root path
            string rootPath = templatePath.Value;
            // Form the Relative Path for storing in DB
            // and Absolute Path for actually saving the file
            string relativePath = generatedFileName;
            string absolutePath = Path.Combine(rootPath, generatedFileName);

            // Create new SuggestionDoc row
            var suggestionDoc = new SuggestionDoc();
            // Fill the values
            suggestionDoc.DocNum = model.DocNum;
            suggestionDoc.DocName = model.Description;
            suggestionDoc.DocStatus = model.IsActive;
            suggestionDoc.FileLocation = relativePath;
            suggestionDoc.VersionNum = model.Version;
            using (_unitOfWork.BeginTransaction())
            {
                try
                {
                    // Save the fill to the absolute path
                    if (CommonHelper.CreateFolderIfNeeded(rootPath))
                    {
                        model.File.SaveAs(absolutePath);
                    }
                    // Insert record to DB and commit
                    _suggestionDocService.CreateSuggestionDoc(suggestionDoc);
                    _unitOfWork.Commit();
                }
                catch (Exception)
                {
                    // Clear the saved file while exception
                    if (System.IO.File.Exists(absolutePath))
                        System.IO.File.Delete(absolutePath);

                    throw;
                }
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
            }, "text/html", JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.SuggestionTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/suggestion/template/version/{suggestionDocId:int}/edit", Name = "versionEditSuggestion")]
        public JsonResult VersionEdit(int suggestionDocId, [CustomizeValidator(RuleSet = "default,UpdateVersion")]SuggestionDocViewModel model)
        {
            Ensure.Argument.NotNull(suggestionDocId);
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the root path of the Template from DB
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.SUGGESTION_TEMPLATE_PATH);

            // Set the root path
            string rootPath = templatePath.Value;
            // Paths for new file if needed
            string relativePath = string.Empty;
            string absolutePath = string.Empty;

            using (_unitOfWork.BeginTransaction())
            {
                try
                {
                    // Get the SuggestionDoc record by given the ID
                    var suggestion = _suggestionDocService.GetSuggestionDocById(suggestionDocId);
                    Ensure.NotNull(suggestion, "No letter found with the specified id");

                    // If new file need to be upload
                    if (model.File != null)
                    {
                        // Rename the file name by adding the current time
                        string time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
                        string fileName = Path.GetFileName(model.File.FileName);
                        string generatedFileName = string.Format("{0}-{1}", time, fileName);

                        // Form the Relative Path for storing in DB
                        // and Absolute Path for actually saving the file
                        relativePath = generatedFileName;
                        absolutePath = Path.Combine(rootPath, generatedFileName);

                        // Save the new file
                        if (CommonHelper.CreateFolderIfNeeded(rootPath))
                        {
                            model.File.SaveAs(absolutePath);
                        }

                        // Form the path of the old file
                        string absolutePathOfOldFile = Path.Combine(rootPath, suggestion.FileLocation);

                        // Delete the old file
                        if (System.IO.File.Exists(absolutePathOfOldFile))
                            System.IO.File.Delete(absolutePathOfOldFile);

                        // Replace with the new path
                        suggestion.FileLocation = relativePath;
                    }

                    // Fill the update values
                    suggestion.DocStatus = model.IsActive;
                    suggestion.VersionNum = model.Version;
                    suggestion.DocName = model.Description;

                    // Update DB record and commit
                    _suggestionDocService.UpdateSuggestionDoc(suggestion);
                    _unitOfWork.Commit();
                }
                catch (Exception)
                {
                    // Delete uploaded file while exception
                    if (System.IO.File.Exists(absolutePath))
                        System.IO.File.Delete(absolutePath);

                    throw;
                }
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, "text/html", JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.SuggestionTemplate)]
        [HttpGet, Route("~/api/suggestion/template/version/{suggestionDocId:int}", Name = "GetSuggestionTemplate")]
        public JsonResult GetTemplate(int suggestionDocId)
        {
            Ensure.Argument.NotNullOrEmpty(suggestionDocId.ToString());
            var suggestionDoc = this._suggestionDocService.GetSuggestionDocById(suggestionDocId);
            if (suggestionDoc == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.AllowGet);
            }
            var model = new SuggestionDocViewModel()
            {
                DocNum = suggestionDoc.DocNum,
                Description = suggestionDoc.DocName,
                Version = suggestionDoc.VersionNum,
                IsActive = suggestionDoc.DocStatus,
                RowVersion = suggestionDoc.RowVersion,
                SuggestionDocId = suggestionDoc.Id
            };
            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.SuggestionTemplate)]
        [HttpPost, Route("~/api/suggestion/template/version/{suggestionDocId:int}/delete", Name = "DeteteSuggestion")]
        public JsonResult Delete(int suggestionDocId, byte[] rowVersion)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            // Get the SuggestionDoc record by given the ID
            var suggestionDoc = this._suggestionDocService.GetSuggestionDocById(suggestionDocId);
            Ensure.NotNull(suggestionDoc, "No Suggestion found with the specified id");

            // Get the root path of the Template file from DB
            // and combine with the FileLocation to get the Absolute Path that the file actually stored at
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.SUGGESTION_TEMPLATE_PATH);
            string rootPath = templatePath.Value;
            string absolutePath = Path.Combine(rootPath, suggestionDoc.FileLocation);

            using (_unitOfWork.BeginTransaction())
            {
                // Delete the record in DB
                suggestionDoc.RowVersion = rowVersion;
                _suggestionDocService.DeleteSuggestionDoc(suggestionDoc);

                // Delete the Template file if exists
                if (System.IO.File.Exists(absolutePath))
                {
                    System.IO.File.Delete(absolutePath);
                }

                // Commit Delete
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted),
                Data = suggestionDoc
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.SuggestionTemplate)]
        [HttpGet, Route("Template/Version/{suggestionDocId:int}/Download", Name = "DownloadFile")]
        public FileResult Download(int suggestionDocId)
        {
            // Get the SuggestionDoc record by given the ID
            var suggestion = _suggestionDocService.GetSuggestionDocById(suggestionDocId);

            // Get the root path of the Template from DB
            // and combine with the FileLocation to get the Absolute Path that the file actually stored at
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.SUGGESTION_TEMPLATE_PATH);
            string rootPath = templatePath.Value;
            var absolutePath = Path.Combine(rootPath, suggestion.FileLocation);

            // Set the file name for saving
            string fileName = suggestion.DocName + Path.GetExtension(Path.GetFileName(suggestion.FileLocation));
            return FileDownload(absolutePath, fileName);
        }

        #endregion "Suggestion Template Version"
    }
}