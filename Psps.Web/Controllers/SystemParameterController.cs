using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Models.Dto.Lookups;
using Psps.Services.Lookups;
using Psps.Services.SystemMessages;
using Psps.Services.SystemParameters;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Extensions;
using Psps.Web.Core.Mvc;
using Psps.Web.ViewModels.Lookup;
using Psps.Web.ViewModels.SystemParameters;
using System.Linq;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("SystemParameter"), Route("{action=index}")]
    public class SystemParameterController : BaseController
    {
        private readonly ICacheManager _cacheManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IParameterService _parameterServic;
        private readonly IMessageService _messageService;

        public SystemParameterController(ICacheManager cacheManager, IUnitOfWork unitOfWork,
            IParameterService parameterServic, IMessageService messageService)
        {
            this._cacheManager = cacheManager;
            this._unitOfWork = unitOfWork;
            this._parameterServic = parameterServic;
            this._messageService = messageService;
        }

        [PspsAuthorize(Allow.AdminFunction)]
        [HttpGet, Route]
        public ActionResult Index()
        {
            SystemParameterViewModel model = new SystemParameterViewModel();
            return View(model);
        }

        #region REST-like API

        [PspsAuthorize(Allow.AdminFunction)]
        [HttpGet, Route("~/api/systemParameter/list", Name = "ListSystemParameter")]
        public JsonResult List(GridSettings grid)
        {
            var systemParameter = _parameterServic.GetPage(grid);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = systemParameter.TotalPages,
                CurrentPageIndex = systemParameter.CurrentPageIndex,
                TotalCount = systemParameter.TotalCount,
                Data = (from m in systemParameter
                        select new SystemParameterViewModel
                        {
                            SystemParameterId = m.SystemParameterId,
                            Code = m.Code,
                            Description = m.Description,
                            Value = m.Value
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.AdminFunction)]
        [HttpGet, Route("~/api/systemParameter/{systemParameterId:int}", Name = "GetSystemParameter")]
        public JsonResult Get(int systemParameterId)
        {
            Ensure.Argument.NotNull(systemParameterId);

            var systemParameter = _parameterServic.GetParameterById(systemParameterId);

            if (systemParameter == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.AllowGet);
            }

            var model = new SystemParameterViewModel()
            {
                SystemParameterId = systemParameter.SystemParameterId,
                Code = systemParameter.Code,
                Description = systemParameter.Description,
                Value = systemParameter.Value,
                RowVersion = systemParameter.RowVersion
            };

            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.AdminFunction)]
        [HttpPost, Route("~/api/systemParameter/{systemParameterId:int}/edit", Name = "EditSystemParameter")]
        public JsonResult Edit(int systemParameterId, SystemParameterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var parameter = _parameterServic.GetParameterById(systemParameterId);

            Ensure.NotNull(parameter, "No message found with the specified id");

            parameter.Description = model.Description;
            parameter.Value = model.Value;
            parameter.RowVersion = model.RowVersion;

            using (_unitOfWork.BeginTransaction())
            {
                _parameterServic.UpdateParameter(parameter);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
                Data = parameter
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion REST-like API
    }
}