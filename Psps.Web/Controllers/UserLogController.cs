using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Models.Dto.Lookups;
using Psps.Services.Lookups;
using Psps.Services.UserLog;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Extensions;
using Psps.Web.Core.Mvc;
using Psps.Web.ViewModels.Lookup;
using Psps.Web.ViewModels.UserLogs;
using System.Linq;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("UserLog"), Route("{action=index}")]
    public class UserLogController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserLogService _UserLogService;

        public UserLogController(IUnitOfWork unitOfWork,
            IUserLogService UserLogService)
        {
            this._unitOfWork = unitOfWork;
            this._UserLogService = UserLogService;
        }

        [PspsAuthorize(Allow.AdminFunction)]
        [HttpGet, Route]
        public ActionResult Index()
        {
            UserLogViewModel model = new UserLogViewModel();
            //CR-005 01
            //For user ease to filter release permit action
            model.Actions = this._UserLogService.GetActions();
            return View(model);
        }

        #region REST-like API

        [PspsAuthorize(Allow.AdminFunction)]
        [HttpGet, Route("~/api/userLog/list", Name = "ListUserLog")]
        public JsonResult List(GridSettings grid)
        {
            var userLogs = _UserLogService.GetPage(grid);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = userLogs.TotalPages,
                CurrentPageIndex = userLogs.CurrentPageIndex,
                TotalCount = userLogs.TotalCount,
                Data = (from m in userLogs
                        select new UserLogViewModel
                        {
                            LogId = m.LogId,
                            RecordKey = m.RecordKey,
                            Activity = m.Activity,
                            Action = m.Action,
                            Remark = m.Remark,
                            ActionedOn = m.ActionedOn,
                            EngUserName = m.User == null ? "" : m.User.EngUserName
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion REST-like API
    }
}