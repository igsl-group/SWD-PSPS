using AutoMapper;
using FluentValidation.Internal;
using FluentValidation.Mvc;
using FluentValidation.Results;
using Psps.Core;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Accounts;
using Psps.Models.Dto.Security;
using Psps.Services.Accounts;
using Psps.Services.Actings;
using Psps.Services.Posts;
using Psps.Services.Security;
using Psps.Services.SystemMessages;
using Psps.Services.SystemParameters;
using Psps.Services.UserLog;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Mvc;
using Psps.Web.Validators;
using Psps.Web.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("Account"), Route("{action=index}")]
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly IRoleService _roleService;
        private readonly IActingService _actingService;
        private readonly IUserLogService _userLogService;
        private readonly IParameterService _parameterService;
        private readonly IUserRepository _userRepository;

        public AccountController(IUnitOfWork unitOfWork,
            IMessageService messageService,
            IAuthenticationService authenticationService,
            IUserService userService,
            IPostService postService,
            IRoleService roleService,
            IActingService actingService,
            IUserLogService userLogService,
            IParameterService parameterService,
            IUserRepository userRepository)
        {
            this._unitOfWork = unitOfWork;
            this._messageService = messageService;
            this._authenticationService = authenticationService;
            this._userService = userService;
            this._postService = postService;
            this._roleService = roleService;
            this._actingService = actingService;
            this._userLogService = userLogService;
            this._parameterService = parameterService;
            this._userRepository = userRepository;
        }

        #region Login / Logout

        [Route("~/Login", Name = "Login")]
        [AllowAnonymous]
        [RuleSetForClientSideMessagesAttribute("default", "Login")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToRoute("Home");
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost, Route("~/Login")]
        [AllowAnonymous]
        [PasswordExpire]
        [ValidateAntiForgeryToken]
        public ActionResult Login([CustomizeValidator(RuleSet = "default,Login")] LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                model.UserId = model.UserId.Trim();

                var loginResult = _userService.ValidateUser(model.UserId, model.Password);

                switch (loginResult)
                {
                    case UserLoginResults.Successful:
                        {
                            //sign in new user
                            _authenticationService.SignIn(model.UserId);

                            //Log Login Information
                            _userLogService.LogLoginInformation(0);

                            if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                return RedirectToRoute("Home");
                            }
                        }
                    case UserLoginResults.NotActive:
                        {
                            ModelState.AddModelError("", _messageService.GetMessage(SystemMessage.Error.User.NotActive));
                            break;
                        }

                    case UserLoginResults.WrongPassword:
                        {
                            _userLogService.LogLoginWrongPassword(model.UserId,this.Request.UserHostAddress);
                            if (AccountLockOut(model.UserId))
                            {
                                ModelState.AddModelError("", _messageService.GetMessage(SystemMessage.Error.User.TooManyLoginAttemps));
                                break;
                            }
                            ModelState.AddModelError("", _messageService.GetMessage(SystemMessage.Error.User.WrongCredentials));                            
                            break;
                        }

                    default:
                        ModelState.AddModelError("", _messageService.GetMessage(SystemMessage.Error.User.WrongCredentials));
                        break;
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [Route("~/Logout", Name = "Logout")]
        public ActionResult Logout()
        {
            _userLogService.LogLoginInformation(1);
            _authenticationService.SignOut();
            return RedirectToRoute("Home");
        }

        [AllowAnonymous]
        [Route("~/PasswordPolicy", Name = "PasswordPolicy")]
        public ActionResult PasswordPolicy() {
            return PartialView("_PasswordPolicyModal");
        }

        public bool AccountLockOut(string user_id) {
            bool locked_out = false;
            //Accordance to OGCIO IT Security Guidelines [G3]
            //Lock out account after 5 invalid logins
            var user = _userService.GetUserById(user_id);
            if (user != null) {
                string default_system_user_id = _parameterService.GetParameterByCode(Constant.SystemParameter.DEFAULT_SYSTEM_USER_ID).Value;
                int max_attempts = Convert.ToInt32(_parameterService.GetParameterByCode("MaxInvalidLoginAttemps").Value);
                
                int attemps = _userLogService.GetInvalidLoginAttemps(user_id);
                if (attemps >= max_attempts) {
                    locked_out = true;
                    //Set User Account to InActive
                    user.IsActive = false;
                    user.UpdatedById = default_system_user_id;
                    user.UpdatedByPost = _postService.GetPostByOwnerUserId(default_system_user_id).First().PostId;
                    user.UpdatedOn = DateTime.Now;                    
                    //Sign In System Account
                    //Logout Immediately After
                    _authenticationService.SignIn(default_system_user_id);                    
                    using (_unitOfWork.BeginTransaction())
                    {
                        _userService.Update(user);
                        _unitOfWork.Commit();
                    }                    
                    _authenticationService.SignOut();
                }
            }
            return locked_out;
        }

        #endregion Login / Logout

        #region Login Change Password

        [AllowAnonymous]
        [Route("~/PasswordExpired", Name = "PasswordExpired")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult PasswordExpired(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel();
            model.UserId = (string)TempData["UserId"];

            if (model.UserId.IsNullOrEmpty())
                return RedirectToRoute("Login", new { returnUrl = returnUrl });

            _authenticationService.SignOut();
            ModelState.AddModelError("", _messageService.GetMessage(SystemMessage.Error.User.PasswordExpire));
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost, Route("~/PasswordExpired")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult PasswordExpired(LoginViewModel model, string returnUrl)
        {
            var changePasswordViewModel = AutoMapper.Mapper.Map<LoginViewModel, ChangePasswordViewModel>(model);

            FluentValidation.IValidator validator = EngineContext.Current.ContainerManager.ResolveUnregistered<ChangePasswordViewModelValidator>();
            FluentValidation.Results.ValidationResult results;

            results = validator.Validate(new FluentValidation.ValidationContext(changePasswordViewModel, new PropertyChain(), new RulesetValidatorSelector("ChangePassword")));

            bool validationSucceeded = results.IsValid;
            IList<ValidationFailure> failures = results.Errors;

            if (validationSucceeded)
            {
                var loginResult = _userService.ValidateUser(model.UserId, model.OldPassword);

                if (loginResult == UserLoginResults.Successful)
                {
                    //sign in new user
                    _authenticationService.SignIn(model.UserId);
                    User u = _userService.GetUserById(model.UserId);
                    using (_unitOfWork.BeginTransaction())
                    {
                        _userService.ChangePassword(model.UserId, model.NewPassword);
                        _unitOfWork.Commit();
                    }

                    if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToRoute("Home");
                }
                else
                    ModelState.AddModelError("", _messageService.GetMessage(SystemMessage.Error.User.WrongCredentials));
            }
            else
            {
                foreach (var failure in results.Errors)
                {
                    ModelState.AddModelError("", failure.ErrorMessage);
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        #endregion Login Change Password

        #region My Profile

        [Route("~/Profile", Name = "ListProfile")]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "EditUser")]
        public ActionResult MyProfile()
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var userId = EngineContext.Current.Resolve<IWorkContext>().CurrentUser.UserId;
            var postId = EngineContext.Current.Resolve<IWorkContext>().CurrentUser.PostId;
            var post = _postService.GetPostById(postId);

            var loginResult = _userService.GetUserById(userId);
            var rolesResult = _roleService.GetAllRolesForDropdown();

            Ensure.Argument.NotNull(loginResult);

            if (rolesResult == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.AllowGet);
            }

            ProfileViewModel model = new ProfileViewModel();

            model.UserId = loginResult.UserId;
            model.EngUserName = loginResult.EngUserName;
            model.ChiUserName = loginResult.ChiUserName;
            model.TelephoneNumber = loginResult.TelephoneNumber;
            model.Email = loginResult.Email;
            model.PostId = loginResult.Post != null ? loginResult.Post.PostId : "";
            model.Roles = rolesResult;

            model.Role = new string[post.Roles.Count];

            for (int i = 0; i < post.Roles.Count; i++)
            {
                model.Role[i] = post.Roles[i].RoleId;
            }

            model.AssignTos = _userService.getAllUserForDropdown();

            return View(model);
        }

        #endregion My Profile

        #region ChangePassword

        [Route("~/ChangePassword", Name = "ChangePassword")]
        [RuleSetForClientSideMessagesAttribute("default", "ChangePassword")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost, Route("~/api/account/changepassword", Name = "ChangePasswordUrl")]
        public JsonResult ConfirmChangePassword([CustomizeValidator(RuleSet = "default,ChangePassword")] ChangePasswordViewModel model)
        {
            //Try to obtain the userId from CurrentUser and assign to model
            var userId = EngineContext.Current.Resolve<IWorkContext>().CurrentUser.UserId;
            model.UserId = userId;

            if (!ModelState.IsValid) return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);

            var loginResult = _userService.ValidateUser(userId, model.OldPassword);

            if (loginResult == UserLoginResults.Successful)
            {
                User u = _userService.GetUserById(userId);
                //u.Password = model.NewPassword;

                using (_unitOfWork.BeginTransaction())
                {
                    //_userService.ChangePassword(userId, u.Password);
                    _userService.ChangePassword(userId, model.NewPassword);
                    _unitOfWork.Commit();
                }

                return Json(new JsonResponse(true)
                {
                    Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
                }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound),
                }, JsonRequestBehavior.DenyGet);
            }
        }

        #endregion ChangePassword

        #region User Maintenance

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpGet]
        [RuleSetForClientSideMessagesAttribute("default", "CreateUser", "UpdateUser")]
        public ActionResult Index()
        {
            var posts = _postService.GetAllPostsForDropdown();

            UserViewModel model = new UserViewModel();
            model.Posts = posts;

            return View(model);
        }

        #region REST-like API

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpGet, Route("~/api/account/list", Name = "ListUser")]
        public JsonResult List(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            var users = _userService.GetPage(grid);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = users.TotalPages,
                CurrentPageIndex = users.CurrentPageIndex,
                TotalCount = users.TotalCount,
                Data = (from u in users
                        select new
                        {
                            UserId = u.UserId,
                            EngUserName = u.EngUserName,
                            ChiUserName = u.ChiUserName,
                            TelephoneNumber = u.TelephoneNumber,
                            Email = u.Email,
                            IsSystemAdministrator = u.IsSystemAdministrator,
                            IsActive = u.IsActive,
                            PostId = u.Post != null ? u.Post.PostId : "",
                            RoleId = u.Post != null ? GetAllRoles(u.Post.Roles) : ""
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpGet, Route("~/api/account/{userId}", Name = "GetUser")]
        public JsonResult Get(string userId)
        {
            Ensure.Argument.NotNullOrEmpty(userId);

            var user = _userService.GetUserById(userId);

            if (user == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.AllowGet);
            }

            var posts = _postService.GetAllPostsForDropdown();

            var model = new UserViewModel()
            {
                UserId = user.UserId,
                PostId = user.Post != null ? user.Post.PostId : "",
                EngUserName = user.EngUserName,
                ChiUserName = user.ChiUserName,
                TelephoneNumber = user.TelephoneNumber,
                Email = user.Email,
                IsSystemAdministrator = user.IsSystemAdministrator,
                IsActive = user.IsActive,
                RowVersion = user.RowVersion,
                Posts = posts
            };

            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [HttpGet, Route("~/api/account/new", Name = "NewUser")]
        public JsonResult New()
        {
            return Json(new JsonResponse(true)
            {
                Data = _parameterService.GetParameterByCode(Constant.SystemParameter.DEFAULT_PASSWORD).Value
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/account/new", Name = "CreateUser")]
        public JsonResult New([CustomizeValidator(RuleSet = "default,CreateUser")] UserViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var userInfo = AutoMapper.Mapper.Map<UserViewModel, UserInfoDto>(model);

            using (_unitOfWork.BeginTransaction())
            {
                _userService.CreateUser(userInfo);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.AccessAdmin)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/account/{userId}/edit", Name = "EditUser")]
        public JsonResult Edit(string userId, [CustomizeValidator(RuleSet = "default,UpdateUser")]  UserViewModel model)
        {
            Ensure.Argument.NotNullOrEmpty(userId);
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            using (_unitOfWork.BeginTransaction())
            {
                var userInfo = AutoMapper.Mapper.Map<UserViewModel, UserInfoDto>(model);

                _userService.UpdateUser(userInfo);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        //[ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/account/profileCreateActing", Name = "ProfileCreateActing")]
        public JsonResult CreateActing([CustomizeValidator(RuleSet = "default,Create")] ProfileViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var userId = EngineContext.Current.Resolve<IWorkContext>().CurrentUser.UserId;
            var postId = EngineContext.Current.Resolve<IWorkContext>().CurrentUser.PostId;
            var post = _postService.GetPostById(postId);

            var assignToUser = _userService.GetUserById(model.AssignTo);
            var loginUser = _userService.GetUserById(userId);

            var acting = new Acting()
            {
                User = assignToUser,
                Post = assignToUser.Post,
                IsDeleted = false,
                PostToBeActed = loginUser.Post,
                EffectiveFrom = model.EffectiveFrom.Value,
                EffectiveTo = model.EffectiveTo.Value,
            };
            using (_unitOfWork.BeginTransaction())
            {
                this._actingService.CreateActing(acting);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("~/api/account/{actingId:int}/profileUpdateActing", Name = "ProfileUpdateActing")]
        public JsonResult UpdateActing(int actingId, [CustomizeValidator(RuleSet = "default,Create")]  ProfileViewModel model)
        {
            Ensure.Argument.NotNull(actingId);
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var userId = EngineContext.Current.Resolve<IWorkContext>().CurrentUser.UserId;
            var postId = EngineContext.Current.Resolve<IWorkContext>().CurrentUser.PostId;

            var assignToUser = _userService.GetUserById(model.AssignTo);

            var acting = _actingService.GetActingById(actingId);
            acting.User = assignToUser;
            acting.Post = assignToUser.Post;
            acting.EffectiveFrom = model.EffectiveFrom.Value;
            acting.EffectiveTo = model.EffectiveTo.Value;

            using (_unitOfWork.BeginTransaction())
            {
                this._actingService.UpdateActing(acting);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("~/api/account/ProfileListActing", Name = "ProfileListActing")]
        public JsonResult ProfileListActing(GridSettings grid)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            var userId = CurrentUser.UserId;
            var postId = CurrentUser.OriginalPostIdIfActed ?? CurrentUser.PostId;

            grid.AddDefaultRule(new Rule()
            {
                field = "postToBeActed.postId",
                data = postId,
                op = WhereOperation.Equal.ToEnumValue()
            });
            var actings = _actingService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = actings.TotalPages,
                CurrentPageIndex = actings.CurrentPageIndex,
                TotalCount = actings.TotalCount,
                Data = (from a in actings
                        select new
                        {
                            EngUserName = a.User.EngUserName,
                            EffectiveFrom = a.EffectiveFrom,
                            EffectiveTo = a.EffectiveTo,
                            RowVersion = a.RowVersion,
                            actingId = a.ActingId,
                            assignToUserId = a.User.UserId
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/account/profile/{userId}/editUser", Name = "ProfileEditUser")]
        public JsonResult ProfileEditUser(string userId, [CustomizeValidator(RuleSet = "default,EditUser")]  ProfileViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var user = _userService.GetUserById(userId);

            using (_unitOfWork.BeginTransaction())
            {
                user.EngUserName = model.EngUserName;
                user.ChiUserName = model.ChiUserName;
                user.TelephoneNumber = model.TelephoneNumber;
                user.Email = model.Email;

                _userService.Update(user);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, Route("~/api/account/{actingId:int}/profileDeteteActing", Name = "ProfileDeteteActing")]
        public JsonResult Edit(int actingId, byte[] rowVersion)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var acting = _actingService.GetActingById(actingId);

            Ensure.NotNull(acting, "No acting found with the specified id");

            acting.RowVersion = rowVersion;

            using (_unitOfWork.BeginTransaction())
            {
                _actingService.DeleteActing(acting);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted)
            }, JsonRequestBehavior.DenyGet);
        }

        private string GetAllRoles(IList<Role> roles)
        {
            string strRole = "";
            foreach (var role in roles)
            {
                strRole = strRole + role.RoleId + ",";
            }
            strRole = strRole.Length > 0 ? strRole.Substring(0, strRole.Length - 1) : "";
            return strRole;
        }

        #endregion REST-like API

        #endregion User Maintenance
    }
}