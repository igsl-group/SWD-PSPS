using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Models.Dto.Lookups;
using Psps.Services.Lookups;
using Psps.Services.SystemMessages;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Extensions;
using Psps.Web.Core.Mvc;
using Psps.Web.ViewModels.Lookup;
using System.Linq;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("Lookup"), Route("{action=index}")]
    public class LookupController : BaseController
    {
        private readonly ICacheManager _cacheManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;
        private readonly ILookupService _lookupService;

        public LookupController(ICacheManager cacheManager, IUnitOfWork unitOfWork,
            IMessageService messageService, ILookupService lookupService)
        {
            this._cacheManager = cacheManager;
            this._unitOfWork = unitOfWork;
            this._messageService = messageService;
            this._lookupService = lookupService;
        }

        [PspsAuthorize(Allow.Lookup)]
        [HttpGet, Route]
        public ActionResult Index()
        {
            LookupViewModel model = new LookupViewModel();
            model.IsPspApprover = HttpContext.User.GetPspsUser().IsSysAdmin || HttpContext.User.IsInRole(Allow.PspApprove.GetName());
            return View(model);
        }

        #region REST-like API

        [PspsAuthorize(Allow.Lookup)]
        [HttpGet, Route("~/api/lookup/{lookupType}/list", Name = "ListLookup")]
        public JsonResult List(string lookupType, GridSettings grid)
        {
            Ensure.Argument.NotNullOrEmpty(lookupType, "lookupType");

            var lookups = _lookupService.GetPage(lookupType, grid);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = lookups.TotalPages,
                CurrentPageIndex = lookups.CurrentPageIndex,
                TotalCount = lookups.TotalCount,
                Data = (from l in lookups
                        select new
                        {
                            LookupId = l.LookupId,
                            Code = l.Code,
                            EngDescription = l.EngDescription,
                            ChiDescription = l.ChiDescription,
                            DisplayOrder = l.DisplayOrder
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.Lookup)]
        [HttpGet, Route("~/api/lookup/{lookupId:int}", Name = "GetLookup")]
        public JsonResult Get(int lookupId)
        {
            Ensure.Argument.NotNull(lookupId);

            var lookup = _lookupService.GetLookupById(lookupId);

            if (lookup == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.AllowGet);
            }

            var model = new LookupViewModel
            {
                LookupId = lookup.LookupId,
                Code = lookup.Code,
                Type = lookup.Type.ToEnum<LookupType>(),
                EngDescription = lookup.EngDescription,
                ChiDescription = lookup.ChiDescription,
                DisplayOrder = lookup.DisplayOrder,
                IsActive = lookup.IsActive,
                RowVersion = lookup.RowVersion,
            };

            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.Lookup)]
        [HttpGet, Route("~/api/lookup/{lookupType}/new", Name = "NewLookup")]
        public JsonResult New(string lookupType)
        {
            Ensure.Argument.NotNullOrEmpty(lookupType, "lookupType");

            var selectedLookupType = lookupType.ToEnum<LookupType>();

            var defaultDisplayOrder = _lookupService.GetDefaultDisplayOrder(lookupType.ToEnum<LookupType>());

            return Json(new JsonResponse(true)
            {
                Data = new LookupViewModel
                {
                    SelectedType = selectedLookupType,
                    Type = selectedLookupType,
                    DisplayOrder = defaultDisplayOrder
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.Lookup)]
        [HttpPost, Route("~/api/lookup/{lookupType}/new")]
        public JsonResult New(string lookupType, LookupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var lookup = new Lookup
            {
                Code = model.Code,
                EngDescription = model.EngDescription,
                ChiDescription = model.ChiDescription,
                DisplayOrder = model.DisplayOrder,
                IsActive = model.IsActive,
                Type = model.Type.GetName()
            };

            using (_unitOfWork.BeginTransaction())
            {
                _lookupService.CreateLookup(lookup);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
                Data = lookup
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.Lookup)]
        [HttpPost, Route("~/api/lookup/{lookupId:int}/edit", Name = "EditLookup")]
        public JsonResult Edit(int lookupid, LookupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            //var lookup = _lookupService.GetLookupById(lookupid);

            //Ensure.NotNull(lookup, "No lookup found with the specified id");
            Lookup lookup = new Lookup();
            lookup.LookupId = lookupid;
            lookup.EngDescription = model.EngDescription;
            lookup.ChiDescription = model.ChiDescription;
            lookup.Code = model.Code;
            lookup.DisplayOrder = model.DisplayOrder;
            lookup.IsActive = model.IsActive;
            lookup.Type = model.Type.GetName();
            lookup.RowVersion = model.RowVersion;

            using (_unitOfWork.BeginTransaction())
            {
                _lookupService.UpdateLookup(lookup);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
                Data = lookup
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion REST-like API
    }
}