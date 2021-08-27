using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Models.Dto.Accounts;
using Psps.Models.Dto.Lookups;
using Psps.Models.Dto.Security;
using Psps.Services.Lookups;
using Psps.Services.Posts;
using Psps.Services.Security;
using Psps.Services.SystemMessages;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Extensions;
using Psps.Web.Core.Mvc;
using Psps.Web.ViewModels.Lookup;
using Psps.Web.ViewModels.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("Role"), Route("{action=index}")]
    public class RoleController : BaseController
    {
        private readonly ICacheManager _cacheManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;

        //private readonly ILookupService _lookupService;
        private readonly IRoleService _roleService;

        private readonly IFunctionService _functionService;
        private readonly IFunctionsInRolesService _functionsInRolesService;
        private readonly IPostsInRolesService _postsInRolesService;
        private readonly IPostService _postService;

        public RoleController(IRoleService roleService, IUnitOfWork unitOfWork, IMessageService messageService,
            ICacheManager cacheManager, IFunctionService functionService, IFunctionsInRolesService functionsInRolesService,
            IPostsInRolesService postsInRolesService, IPostService postService)
        {
            this._cacheManager = cacheManager;
            this._roleService = roleService;
            this._functionService = functionService;

            this._unitOfWork = unitOfWork;
            this._messageService = messageService;
            this._functionsInRolesService = functionsInRolesService;
            this._postsInRolesService = postsInRolesService;
            this._postService = postService;
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpGet, Route(Name = "RoleIndexUrl")]
        public ActionResult Index()
        {
            RoleViewModel model = new RoleViewModel();
            model.Roles = _roleService.GetAllRolesForDropdown();
            model.Posts = _postService.GetAllPostsForDropdown();
            return View(model);
        }

        #region REST-like API

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpPost, Route("~/api/role/new", Name = "NewRole")]
        public JsonResult New(CreateRoleViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var userId = EngineContext.Current.Resolve<IWorkContext>().CurrentUser.UserId;
            var postId = EngineContext.Current.Resolve<IWorkContext>().CurrentUser.PostId;

            var roleInfo = new RoleInfoDto
            {
                RoleId = model.RoleName,
                Description = "",
                IsDeleted = false,
                CreatedById = userId,
                CreatedByPost = postId
            };

            using (_unitOfWork.BeginTransaction())
            {
                _roleService.CreateRole(roleInfo);
                _unitOfWork.Commit();
            }

            var roles = _roleService.GetAllRolesForDropdown();
            //model.Roles = roles;

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
                Data = roles
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpPost, Route("~/api/role/function/list/{*roleId}", Name = "ListFunctions")]
        public JsonResult ListFunctions(string roleId)
        {
            IList<FunctionDto> list = _functionService.GetFunctionsByRoleId(roleId);
            return Json(new JsonResponse(true)
            {
                Data = list
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpPost, Route("~/api/role/accessright/edit/{*roleId}", Name = "UpdateAccessRights")]
        public JsonResult UpdateAccessRights(string roleId, string[] functionIds, string[] isEnabled)
        {
            IList<FunctionsInRolesDto> list = new List<FunctionsInRolesDto>();

            var currentDate = DateTime.Now;
            for (var i = 0; i < functionIds.Length; i++)
            {
                FunctionsInRolesDto funcInRoles = new FunctionsInRolesDto();
                funcInRoles.RoleId = roleId;
                funcInRoles.FunctionId = functionIds[i];
                funcInRoles.IsEnabled = Convert.ToBoolean(isEnabled[i]);

                list.Add(funcInRoles);
            }

            using (_unitOfWork.BeginTransaction())
            {
                _functionsInRolesService.CreateOrUpdateFunctionsInRoles(list);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpPost, Route("~/api/role/delete/{*roleId}", Name = "DeleteRole")]
        public JsonResult DeleteRole(string roleId)
        {
            var list = _postsInRolesService.GetByRoleId(roleId);
            if (list != null && list.Count() > 0)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.Role.RecordDeleteError),
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var role = _roleService.GetRoleById(roleId);

                using (_unitOfWork.BeginTransaction())
                {
                    _roleService.DeleteRole(role);
                    _unitOfWork.Commit();
                }

                var roles = _roleService.GetAllRolesForDropdown();

                return Json(new JsonResponse(true)
                {
                    Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted),
                    Data = roles
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpGet, Route("~/api/role/member/list/{*roleId}", Name = "ListRoleMembers")]
        public JsonResult ListRoleMembers(string roleId, GridSettings grid)
        {
            var role = new Role();
            role.RoleId = roleId;
            grid.AddDefaultRule(new Rule()
            {
                field = "RoleId",
                data = role.RoleId,
                op = WhereOperation.Equal.ToEnumValue()
            });
            var postsInRoles = _postsInRolesService.GetPage(grid);
            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = postsInRoles.TotalPages,
                CurrentPageIndex = postsInRoles.CurrentPageIndex,
                TotalCount = postsInRoles.TotalCount,
                Data = (from p in postsInRoles
                        orderby p.Post.Rank.RankLevel
                        select new
                        {
                            PostId = p.PostId,
                            PostsInRolesId = p.PostsInRolesId
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpPost, Route("~/api/role/member/createRoleMember", Name = "CreateRoleMember")]
        public JsonResult CreateRoleMember(CreatePostsInRolesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var postsInRoles = new PostsInRoles()
            {
                PostId = model.RolePostId,
                RoleId = model.RoleId,
            };

            using (_unitOfWork.BeginTransaction())
            {
                _postsInRolesService.CreatePostsInRoles(postsInRoles);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated)
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpPost, Route("~/api/role/member/{postsInRolesId:int}/delete", Name = "DeletePostsInRoles")]
        public JsonResult DeleteRoleMember(int postsInRolesId)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            var postsInRoles = _postsInRolesService.GetById(postsInRolesId);
            Ensure.NotNull(postsInRoles, "No postsInRoles found with the specified id");

            using (_unitOfWork.BeginTransaction())
            {
                _postsInRolesService.DeletePostsInRoles(postsInRoles);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted)
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion REST-like API
    }
}