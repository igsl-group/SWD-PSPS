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
using Psps.Web.ViewModels.SystemMessages;
using System.Linq;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("SystemMessage"), Route("{action=index}")]
    public class SystemMessageController : BaseController
    {
        private readonly ICacheManager _cacheManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;

        public SystemMessageController(ICacheManager cacheManager, IUnitOfWork unitOfWork,
            IMessageService messageService)
        {
            this._cacheManager = cacheManager;
            this._unitOfWork = unitOfWork;
            this._messageService = messageService;
        }

        [PspsAuthorize(Allow.AdminFunction)]
        [HttpGet, Route]
        public ActionResult Index()
        {
            SystemMessageViewModel model = new SystemMessageViewModel();
            return View(model);
        }

        #region REST-like API

        [PspsAuthorize(Allow.AdminFunction)]
        [HttpGet, Route("~/api/systemMessage/list", Name = "ListSystemMessage")]
        public JsonResult List(GridSettings grid)
        {
            var systemMessages = _messageService.GetPage(grid);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = systemMessages.TotalPages,
                CurrentPageIndex = systemMessages.CurrentPageIndex,
                TotalCount = systemMessages.TotalCount,
                Data = (from m in systemMessages
                        select new SystemMessageViewModel
                        {
                            SystemMessageId = m.SystemMessageId,
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
        [HttpGet, Route("~/api/systemMessage/{systemMessageId:int}", Name = "GetSystemMessage")]
        public JsonResult Get(int systemMessageId)
        {
            Ensure.Argument.NotNull(systemMessageId);

            var systemMessage = _messageService.GetMessageById(systemMessageId);

            if (systemMessage == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.AllowGet);
            }

            var model = new SystemMessageViewModel()
            {
                SystemMessageId = systemMessage.SystemMessageId,
                Code = systemMessage.Code,
                Description = systemMessage.Description,
                Value = systemMessage.Value,
                RowVersion = systemMessage.RowVersion
            };

            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.AdminFunction)]
        [HttpPost, Route("~/api/systemMessage/{systemMessageId:int}/edit", Name = "EditSystemMessage")]
        public JsonResult Edit(int systemMessageId, SystemMessageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var message = _messageService.GetMessageById(systemMessageId);

            Ensure.NotNull(message, "No message found with the specified id");

            message.Description = model.Description;
            message.Value = model.Value;
            message.RowVersion = model.RowVersion;

            using (_unitOfWork.BeginTransaction())
            {
                _messageService.UpdateMessage(message);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
                Data = message
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion REST-like API
    }
}