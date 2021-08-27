using FluentValidation.Mvc;
using Psps.Core;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Services.FlagDays;
using Psps.Services.Security;
using Psps.Services.SystemMessages;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Extensions;
using Psps.Web.Core.Mvc;
using Psps.Web.ViewModels.FlagDayList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("FlagDayList"), Route("{action=index}")]
    public class FlagDayListController : BaseController
    {
        #region Var

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;
        private readonly IFlagDayListService _flagDayListService;

        #endregion Var

        #region Ctor

        public FlagDayListController(
            IMessageService messageService,
            IFlagDayListService FlagDayListService,
            IUnitOfWork unitOfWork
            )
        {
            this._unitOfWork = unitOfWork;
            this._messageService = messageService;
            this._flagDayListService = FlagDayListService;
        }

        #endregion Ctor

        [PspsAuthorize(Allow.FlagDayList)]
        [HttpGet]
        [RuleSetForClientSideMessagesAttribute("default", "CreateFdList", "UpdateFdList")]
        public ActionResult Index()
        {
            IDictionary<string, string> types = new Dictionary<string, string>();
            types.Add("R", "Regional");
            types.Add("T", "Territory-wide");
            //var flagDayListTypes = _flagDayListService.GetAllFlagDayListTypeForDropdown();
            var flagDayListYears = _flagDayListService.GetAllFlagDayListYearForDropdown();

            FlagDayListViewModel model = new FlagDayListViewModel();
            model.FlagDayTypes = types;
            model.FlagDayYears = flagDayListYears;
            model.IsFdApprover = HttpContext.User.GetPspsUser().IsSysAdmin || HttpContext.User.IsInRole(Allow.FdApprove.GetName());

            return View(model);
        }

        public PartialViewResult RenderImportFlagDayListXlsFileModal()
        {
            return PartialView("_ImportFlagDayListXlsFileModal");
        }

        #region REST-like API

        [PspsAuthorize(Allow.FlagDayList)]
        [HttpGet, Route("~/api/FlagDayList/list", Name = "ListFlagDayList")]
        public JsonResult List(string searchFlagDayType, string flagDayYear, GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            if (!String.IsNullOrEmpty(searchFlagDayType))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "flagDayType",
                    data = searchFlagDayType,
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(flagDayYear))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "flagDayYear",
                    data = flagDayYear.ToString(),
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            var FlagDayList = _flagDayListService.GetPage(grid);
            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = FlagDayList.TotalPages,
                CurrentPageIndex = FlagDayList.CurrentPageIndex,
                TotalCount = FlagDayList.TotalCount,
                Data = (from u in FlagDayList
                        select new
                        {
                            FlagDayDate = u.FlagDayDate,
                            FlagDayListId = u.FlagDayListId,
                            FlagDayType = u.FlagDayType == "R" ? "Regional" : "Territory-wide",
                            FlagDayYear = u.FlagDayYear,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.FlagDayList)]
        [HttpGet, Route("~/api/FlagDayList/{flagDayListId}", Name = "GetFlagDayList")]
        public JsonResult Get(int flagDayListId)
        {
            var flagDayList = _flagDayListService.GetFlagDayListById(flagDayListId);

            if (flagDayList == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.AllowGet);
            }

            var model = new FlagDayListViewModel
            {
                FlagDayListId = flagDayList.FlagDayListId,
                FlagDayYear = flagDayList.FlagDayYear,
                FlagDayType = flagDayList.FlagDayType,
                FlagDayDate = flagDayList.FlagDayDate,
                RowVersion = flagDayList.RowVersion
            };

            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.FlagDayList)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/FlagDayList/new", Name = "NewFlagDayList")]
        public JsonResult New(string recordPostId, [CustomizeValidator(RuleSet = "default,CreateFdList")] FlagDayListViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            //DateTime outFlagDayListDate = new DateTime();
            //CultureInfo enUS = new CultureInfo("en-US");
            //DateTime.TryParseExact(model.FlagDayDate, "d/M/yyyy", enUS, DateTimeStyles.None, out outFlagDayListDate);

            var FlagDayList = new FdList();

            if (model != null)
            {
                FlagDayList.FlagDayListId = Convert.ToInt32(model.FlagDayListId);
                FlagDayList.FlagDayDate = model.FlagDayDate;
                FlagDayList.FlagDayType = model.FlagDayType;
                FlagDayList.FlagDayYear = model.FlagDayYear;
            }

            using (_unitOfWork.BeginTransaction())
            {
                _flagDayListService.CreateFlagDayList(FlagDayList);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.FlagDayList)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/FlagDayList/{flagDayListId}/edit", Name = "EditFlagDayList")]
        public JsonResult Edit(int flagDayListId, [CustomizeValidator(RuleSet = "default,UpdateFdList")] FlagDayListViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var flagDayList = _flagDayListService.GetFlagDayListById(Convert.ToInt32(model.FlagDayListId));

            Ensure.NotNull(flagDayList, "No Flag Day List found with the specified id");

            //DateTime outFlagDayListDate = new DateTime();
            //CultureInfo enUS = new CultureInfo("en-US");
            //DateTime.TryParseExact(model.FlagDayDate, "d/M/yyyy", enUS, DateTimeStyles.None, out outFlagDayListDate);

            if (model != null)
            {
                flagDayList.FlagDayListId = Convert.ToInt32(model.FlagDayListId);
                flagDayList.FlagDayDate = model.FlagDayDate;
                flagDayList.FlagDayType = model.FlagDayType;
                flagDayList.FlagDayYear = model.FlagDayYear;
                flagDayList.RowVersion = model.RowVersion;
            }

            using (_unitOfWork.BeginTransaction())
            {
                _flagDayListService.UpdateFlagDayList(flagDayList);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.FlagDayList)]
        [HttpPost, Route("~/api/FlagDayList/importFlagDayList", Name = "ImportFlagDayList")]
        public JsonResult ImportFlagDayList(FlagDayListImportViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (model.ImportFile == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = "Import file can not be empty.",
                }, "text/html", JsonRequestBehavior.DenyGet);
            }
            var fileName = model.ImportFile.FileName;
            string type = fileName.Substring(fileName.LastIndexOf(".") + 1);
            if (!type.ToLower().Equals("xlsx"))
            {
                return Json(new JsonResponse(false)
                {
                    Message = "File is not correct.",
                }, "text/html", JsonRequestBehavior.DenyGet);
            }
            string errorMsg = "";
            using (_unitOfWork.BeginTransaction())
            {
                errorMsg = _flagDayListService.InsertFlagDayListByImportXls(model.ImportFile.InputStream);
                _unitOfWork.Commit();
            }
            if (!String.IsNullOrEmpty(errorMsg))
            {
                return Json(new JsonResponse(false)
                {
                    Message = CommonHelper.ConvertHtmlToString(errorMsg)
                }, "text/html", JsonRequestBehavior.DenyGet);
            }
            else
            {
                var flagDayListYears = _flagDayListService.GetAllFlagDayListYearForDropdown();

                return Json(new JsonResponse(true)
                {
                    Message = _messageService.GetMessage(SystemMessage.Info.FileUploaded),
                    Data = flagDayListYears,
                }, "text/html", JsonRequestBehavior.DenyGet);
            }
        }

        [PspsAuthorize(Allow.FlagDayList)]
        [HttpPost, Route("~/api/FlagDayList/{flagDayListId}/deleteFlagDayList", Name = "DeleteFlagDayList")]
        public JsonResult DeleteFlagDayList(int flagDayListId)
        {
            using (_unitOfWork.BeginTransaction())
            {
                _flagDayListService.DeleteFlagDayList(flagDayListId);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted)
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion REST-like API
    }
}