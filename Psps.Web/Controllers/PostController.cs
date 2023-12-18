using FluentValidation.Mvc;
using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.Actings;
using Psps.Services.Posts;
using Psps.Services.Ranks;
using Psps.Services.Security;
using Psps.Services.SystemMessages;
using Psps.Services.UserLog;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Extensions;
using Psps.Web.Core.Mvc;
using Psps.Web.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("Post"), Route("{action=index}")]
    public class PostController : BaseController
    {
        private readonly ICacheManager _cacheManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly IRankService _rankService;
        private readonly IRoleService _roleService;
        private readonly IActingService _actingService;
        private readonly IPostsInRolesService _postsInRolesService;
        private readonly IUserLogService _userLogService;


        public PostController(ICacheManager cacheManager, IUnitOfWork unitOfWork,
            IMessageService messageService, IPostService postService, IUserService userService, IRankService rankService, IRoleService roleService, IActingService actingService, IPostsInRolesService postsInRolesService,IUserLogService userLogService)
        {
            this._cacheManager = cacheManager;
            this._unitOfWork = unitOfWork;
            this._messageService = messageService;
            this._postService = postService;
            this._userService = userService;
            this._rankService = rankService;
            this._roleService = roleService;
            this._actingService = actingService;
            this._postsInRolesService = postsInRolesService;
            this._userLogService = userLogService;
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpGet, Route(Name = "PostIndex")]
        public ActionResult Index()
        {
            var posts = _postService.GetAllPostsForDropdown();
            var users = _userService.getAllUserForDropdown();
            var ranks = _rankService.GetAllRanksForDropdown();
            var roles = _roleService.GetAllRolesForDropdown();
            var assignto = _userService.getAllUserForDropdown();

            PostActingViewModel model = new PostActingViewModel();
            model.Posts = posts;
            model.Users = users;
            model.Ranks = ranks;
            model.Roles = roles;
            model.AssignTos = assignto;
            return View(model);
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpGet, Route("New", Name = "NewPost")]
        public ActionResult NewPost(string postId)
        {
            var posts = _postService.GetAllPostsForDropdown();
            var users = _userService.getAllUserForDropdown();
            var ranks = _rankService.GetAllRanksForDropdown();
            var roles = _roleService.GetAllRolesForDropdown();
            var assignto = _userService.getAllUserForDropdown();

            var model = new PostActingViewModel
            {
                Posts = posts,
                Users = users,
                Ranks = ranks,
                Roles = roles,
                AssignTos = assignto
            };

            return View("Detail", model);
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpGet, Route("{postId}", Name = "GetPost")]
        public ActionResult Detail(string postId)
        {
            Ensure.Argument.NotNull(postId);

            var post = _postService.GetPostById(postId);
            var posts = _postService.GetAllPostsForDropdown();
            var users = _userService.getAllUserForDropdown();
            var ranks = _rankService.GetAllRanksForDropdown();
            var roles = _roleService.GetAllRolesForDropdown();
            var assignto = _userService.getAllUserForDropdown();
            var postsInRole = _postsInRolesService.GetByPostId(postId);
            if (post == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.AllowGet);
            }

            var model = new PostActingViewModel
            {
                PostId = post.PostId,

                Rank = post.Rank == null ? "" : post.Rank.RankId,
                Owner = post.Owner == null ? "" : post.Owner.UserId,
                Supervisor = post.Supervisor == null ? "" : post.Supervisor.PostId,
                RowVersion = post.RowVersion,
                Posts = posts,
                Users = users,
                Ranks = ranks,
                Roles = roles,
                AssignTos = assignto,
            };

            // model.Role = new string[postsInRole.Count];
            model.PostsInRoles = new string[postsInRole.Count];
            for (int i = 0; i < postsInRole.Count; i++)
            {
                model.PostsInRoles[i] = postsInRole[i].RoleId;
            }

            return View(model);
        }

        #region REST-like API

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpGet, Route("~/api/post/list", Name = "ListPost")]
        public JsonResult List(GridSettings grid)
        {
            var posts = _postService.GetPage(grid);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = posts.TotalPages,
                CurrentPageIndex = posts.CurrentPageIndex,
                TotalCount = posts.TotalCount,
                Data = (from l in posts
                        select new
                        {
                            PostId = l.PostId,
                            Rank = l.Rank == null ? "" : l.Rank.RankId,
                            Owner = l.Owner == null ? "" : l.Owner.EngUserName,
                            Supervisor = l.Supervisor == null ? "" : l.Supervisor.PostId,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpPost, Route("~/api/post/new", Name = "CreatePost")]
        public JsonResult New(CreatePostViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            Rank rank = null;
            if (model.Rank != null) 
            {
                rank = _rankService.GetRankById(model.Rank);
            }             
            User owner = null;
            if (model.Owner != null)
            {
                owner = _userService.GetUserById(model.Owner);
            }
            Post supervisor = null;
            if (model.Supervisor != null)
            {
                supervisor = _postService.GetPostById(model.Supervisor);
            }
            var post = new Post
            {
                PostId = model.PostId,
                Rank = rank,
                Owner = owner,
                Supervisor = supervisor,
            };

            if (model.Role != null)
            {
                foreach (string s in model.Role)
                {
                    PostsInRoles postsInRoles = new PostsInRoles();

                    var r = _roleService.GetRoleById(s);
                    postsInRoles.PostId = post.PostId;
                    postsInRoles.RoleId = r.RoleId;

                    post.PostsInRole.Add(postsInRoles);
                }
            }

            using (_unitOfWork.BeginTransaction())
            {
                _postService.CreatePost(post);

                List<string> logPostList = new List<string>();
                logPostList.Add($"PostId:{post.PostId}");
                logPostList.Add(post.Owner != null ? $"Owner:{post.Owner.UserId}" : "");
                logPostList.Add(post.Rank != null ? $"Rank:{post.Rank.RankId}" : "");
                logPostList.Add(post.Rank != null ? $"RankLvl:{post.Rank.RankLevel}" : "");
                logPostList.Add(post.Supervisor != null ? $"Supervisor:{post.Supervisor.PostId}" : "");


                string dataStr = string.Join(",", logPostList.Where(x => (!string.IsNullOrEmpty(x))));
                _userLogService.LogCRUDPost("Create", this.Request.UserHostAddress, post.PostId, dataStr, new List<string> { Constant.SystemParameter.LOG_CODE_CREATE_POST });

                foreach (var pr in post.PostsInRole)
                {
                    _postsInRolesService.CreatePostsInRoles(pr);
                }
                _unitOfWork.Commit();
            }

            

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpPost, Route("~/api/post/{postId}/edit", Name = "EditPost")]
        public JsonResult Edit(string postId, [CustomizeValidator(RuleSet = "default,EditPost")] UpdatePostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var post = _postService.GetPostById(postId);
            Ensure.NotNull(post, "No post found with the specified id");

            var rank = _rankService.GetRankById(model.Rank);
            var oldPost = _postService.GetPostById(post.PostId);

            List<string> postLogCode = new List<string>();
            if (model.Owner != null)
            {
                if (oldPost.Owner != null && oldPost.Owner.UserId != model.Owner)
                {
                    postLogCode.Add(Constant.SystemParameter.LOG_CODE_UPDATE_POST_OWNER);
                }
            }

            if (model.Rank != null)
            {
                if (oldPost.Rank != null && oldPost.Rank.RankId != model.Rank)
                {
                    postLogCode.Add(Constant.SystemParameter.LOG_CODE_UPDATE_POST_RANK);
                }
            }

            if (model.Supervisor != null)
            {
                if (oldPost.Supervisor != null && oldPost.Supervisor.PostId != model.Supervisor)
                {
                    postLogCode.Add(Constant.SystemParameter.LOG_CODE_UPDATE_POST_SUPERVISOR);
                }
            }

            
            if (oldPost.Roles != null && model.Roles ==null 
                || oldPost.Roles == null && model.Roles != null
                || oldPost.Roles!=null && model.Roles!=null && oldPost.Roles.Count != model.Roles.Count
                || oldPost.Roles!=null && oldPost.Roles.Count > 0 && model.Roles!=null && model.Roles.Count > 0 && oldPost.Roles.Select(x=>x.RoleId) != model.Roles)
            {
                postLogCode.Add(Constant.SystemParameter.LOG_CODE_UPDATE_POST_ROLE);
            }
            

            // if Owner is selected
            if (model.Owner != null)
            {
                // Get the owner by ID and fill as update value
                post.Owner = _userService.GetUserById(model.Owner);
            }
            else
            {
                // Clear
                post.Owner = null;
            }

            // if Supervisor is selected
            if (model.Supervisor != null)
            {
                // Get the supervisor by ID and fill as update value
                post.Supervisor = _postService.GetPostById(model.Supervisor);
            }
            else
            {
                // Clear
                post.Supervisor = null;
            }

            if (model.Role != null)
            {
                foreach (string s in model.Role)
                {
                    PostsInRoles postsInRoles = new PostsInRoles();
                    var r = _roleService.GetRoleById(s);
                    postsInRoles.PostId = post.PostId;
                    postsInRoles.RoleId = r.RoleId;
                    post.PostsInRole.Add(postsInRoles);
                }
            }

            post.Rank = rank;
            post.RowVersion = model.RowVersion;
            using (_unitOfWork.BeginTransaction())
            {
                var OldPostsInRoles = _postsInRolesService.GetByPostId(postId);
                foreach (var oldpr in OldPostsInRoles)
                {
                    _postsInRolesService.DeletePostsInRoles(oldpr);
                }

               

                _postService.UpdatePost(post);

                List<string> logPostList = new List<string>();
                logPostList.Add($"PostId:{post.PostId}");
                logPostList.Add(post.Owner != null ? $"Owner:{post.Owner.UserId}" : "");
                logPostList.Add(post.Rank != null ? $"Rank:{post.Rank.RankId}" : "");
                logPostList.Add(post.Rank != null ? $"RankLvl:{post.Rank.RankLevel}" : "");
                logPostList.Add(post.Supervisor != null ? $"Supervisor:{post.Supervisor.PostId}" : "");

                string dataStr = string.Join(",", logPostList.Where(x => (!string.IsNullOrEmpty(x))));
                _userLogService.LogCRUDPost("Update", this.Request.UserHostAddress, post.PostId , dataStr, postLogCode);

                foreach (var newpr in post.PostsInRole)
                {
                    _postsInRolesService.CreatePostsInRoles(newpr);
                }

                _unitOfWork.Commit();
            }

            var newpost = _postService.GetPostById(postId);
            var data = new PostActingViewModel()
            {
                RowVersion = newpost.RowVersion
            };
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
                Data = data
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpPost, Route("~/api/post/{postId}/new", Name = "CreateActing")]
        public JsonResult NewActing(string postId, CreatePostActingViewModel model)
        {
            Ensure.Argument.NotNullOrEmpty(postId);
            Ensure.Argument.NotNull(model);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var post = _postService.GetPostById(postId);

            Ensure.NotNull(post, "No post found with the specified id");

            var assignToUser = _userService.GetUserById(model.AssignTo);
            var acting = new Acting
            {
                User = assignToUser,
                Post = assignToUser.Post,
                PostToBeActed = post,
                EffectiveFrom = model.EffectiveFrom,
                EffectiveTo = model.EffectiveTo,
            };

            using (_unitOfWork.BeginTransaction())
            {
                _actingService.CreateActing(acting);
                _unitOfWork.Commit();
            }
            var newpost = _postService.GetPostById(postId);
            var data = new PostActingViewModel()
            {
                RowVersion = newpost.RowVersion
            };

            _userLogService.LogCRUDActing("Create", this.Request.UserHostAddress, acting.ActingId.ToString(), acting.User.UserId, acting.Post.PostId);

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
                Data = data
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpPost, Route("~/api/post/{actingId:int}/editacting", Name = "EditActing")]
        public JsonResult ActingEdit(int actingId, UpdatePostActingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var acting = _actingService.GetActingById(actingId);
            var assignToUser = _userService.GetUserById(model.AssignTo);
            acting.User = assignToUser;
            acting.Post = assignToUser.Post;
            acting.EffectiveFrom = model.EffectiveFrom;
            acting.EffectiveTo = model.EffectiveTo;
            acting.RowVersion = model.ActingRowVersion;

            using (_unitOfWork.BeginTransaction())
            {
                _actingService.UpdateActing(acting);
                _unitOfWork.Commit();
            }
            var newpost = _postService.GetPostById(model.PostId);
            var data = new PostActingViewModel()
            {
                RowVersion = newpost.RowVersion
            };

            _userLogService.LogCRUDActing("Update", this.Request.UserHostAddress, actingId.ToString(), acting.User.UserId, acting.Post.PostId);


            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
                Data = data
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpGet, Route("~/api/post/{postId}/acting", Name = "Acting")]
        public JsonResult ActingList(GridSettings grid, string postId)
        {
            grid.AddDefaultRule(new Rule()
            {
                field = "PostToBeActed.PostId",
                data = postId,
                op = WhereOperation.Equal.ToEnumValue()
            });
            var acting = _actingService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = acting.TotalPages,
                CurrentPageIndex = acting.CurrentPageIndex,
                TotalCount = acting.TotalCount,
                Data = (from a in acting
                        select new
                        {
                            EngUserName = a.User.EngUserName,
                            EffectiveFrom = a.EffectiveFrom,
                            EffectiveTo = a.EffectiveTo,
                            RowVersion = a.RowVersion,
                            ActingId = a.ActingId,
                            AssignToUserId = a.User.UserId
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpPost, Route("~/api/post/{actingId:int}/delete", Name = "DeteteActing")]
        public JsonResult Delete(int actingId, byte[] RowVersion)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            var acting = _actingService.GetActingById(actingId);

            Ensure.NotNull(acting, "No acting found with the specified id");

            acting.RowVersion = RowVersion;

            using (_unitOfWork.BeginTransaction())
            {
                _actingService.DeleteActing(acting);
                _unitOfWork.Commit();
            }

            _userLogService.LogCRUDActing("Delete", this.Request.UserHostAddress, actingId.ToString());

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted),
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion REST-like API
    }
}