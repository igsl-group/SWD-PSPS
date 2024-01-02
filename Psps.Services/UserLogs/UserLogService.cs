using log4net;
using Psps.Core;
using Psps.Core.Common;
using Psps.Core.Helper;

using Psps.Core.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Services.Events;
using Psps.Services.Posts;
using Psps.Services.SystemParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Psps.Services.UserLog
{
    public partial class UserLogService : IUserLogService
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string USERLOG_ALL_KEY = "Psps.userlog.all";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : code
        /// </remarks>
        private const string USERLOG_BY_CODE_KEY = "Psps.userlog.{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string USERLOG_PATTERN_KEY = "Psps.userlog.";        

        #endregion Constants

        #region Fields

        private readonly IEventPublisher _eventPublisher;

        private readonly IUserLogRepository _userlogRepository;

        private readonly IUserRepository _userRepository;

        private readonly IPostRepository _postRepository;

        private readonly IParameterService _parameterService;

        #endregion Fields

        private Dictionary<string, string> _logCodeDict;

        #region Ctor

        public UserLogService(IEventPublisher eventPublisher, IUserLogRepository UserLogRepository,
            IUserRepository userRepository, IPostRepository postRepository, IParameterService parameterService)
        {
            this._eventPublisher = eventPublisher;
            this._userlogRepository = UserLogRepository;
            this._userRepository = userRepository;
            this._postRepository = postRepository;
            this._parameterService = parameterService;
            _logCodeDict = LogCodeDictionary();
        }

        #endregion Ctor

        #region Methods

        //CR-005 01
        //For user ease to filter release permit action
        public IDictionary<String, String> GetActions()
        {
            return this._userlogRepository.Table
                     .OrderBy(l => l.Action)                     
                     .Select(l => new { Key = l.Action, Value = l.Action })
                     .Distinct()
                     .ToDictionary(k => k.Key, v => v.Value);
        }

        public ActivityLog LogOrganisationInformation(OrgMaster oldOrgMaster, OrgMaster newOrgMaster)
        {
            /*
                Please log those information related to:
                1) Organisation Information
             */

            var result = ObjectComparer.Compare(oldOrgMaster, newOrgMaster, new CompareSettings
            {
                IgnoreProperties = new string[] { "OrgMaster", "ComplaintMaster", "FdMaster", "OrgAttachment", "OrgNameChangeHistory",
                "OrgRefGuidePromulgations", "PspMasters", "BeneficiaryPspMasters", "ReferenceGuideSearchViews"}
            });

            if (!result.AreEqual)
            {
                var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
                var logBuilder = new StringBuilder();
                logBuilder.AppendFormat("Organisation Reference: {0}", oldOrgMaster.OrgRef).AppendLine();

                var log = new ActivityLog
                {
                    RecordKey = oldOrgMaster.OrgId.ToString(),
                    Activity = "Organisation",
                    Action = "Update",
                    ActionedOn = DateTime.Now,
                    User = _userRepository.GetById(currentUser.UserId),
                    Post = _postRepository.GetById(currentUser.PostId),
                    Remark = logBuilder.ToString()
                };
                this._userlogRepository.Add(log);
                _eventPublisher.EntityInserted<ActivityLog>(log);

                return log;
            }

            if (oldOrgMaster == null)
            {
                var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
                var logBuilder = new StringBuilder();
                logBuilder.AppendFormat("Organisation Reference: {0}", newOrgMaster.OrgRef).AppendLine();

                var log = new ActivityLog
                {
                    RecordKey = newOrgMaster.OrgId.ToString(),
                    Activity = "Organisation",
                    Action = "Create",
                    ActionedOn = DateTime.Now,
                    User = _userRepository.GetById(currentUser.UserId),
                    Post = _postRepository.GetById(currentUser.PostId),
                    Remark = logBuilder.ToString()
                };


                this._userlogRepository.Add(log);
                _eventPublisher.EntityInserted<ActivityLog>(log);

                return log;
            }

            return null;
        }

        public ActivityLog LogFlagDayInformation(FdMaster oldFdMaster, FdMaster newFdMaster)
        {
            /*
                Please log those information related to:
                1) Flag Day Information
             */

            var result = ObjectComparer.Compare(oldFdMaster, newFdMaster, new CompareSettings
            {
                IgnoreProperties = new string[] { "FdMaster", "OrgMaster", "ReferenceGuideSearchView", "FdAttachment", "FdEvent" }
            });

            if (CommonHelper.HasValueChanged(oldFdMaster.OrgMaster.OrgId, newFdMaster.OrgMaster.OrgId) || !result.AreEqual)
            {
                var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
                var logBuilder = new StringBuilder();
                logBuilder.AppendFormat("Flag Day Reference: {0}", oldFdMaster.FdRef).AppendLine();

                var log = new ActivityLog
                {
                    RecordKey = oldFdMaster.FdMasterId.ToString(),
                    Activity = "Flag Day",
                    Action = "Update",
                    ActionedOn = DateTime.Now,
                    User = _userRepository.GetById(currentUser.UserId),
                    Post = _postRepository.GetById(currentUser.PostId),
                    Remark = logBuilder.ToString()
                };
                this._userlogRepository.Add(log);
                _eventPublisher.EntityInserted<ActivityLog>(log);

                return log;
            }

            return null;
        }

        public ActivityLog LogPSPInformation(PspMaster oldPspMaster, PspMaster newPspMaster)
        {
            /*
                Please log those information related to:
                1) PSP Information
             */

            if (oldPspMaster != null)
            {
                //CR-005 01 Exclude the PermitNum
                var result = ObjectComparer.Compare(oldPspMaster, newPspMaster, new CompareSettings
                {
                    IgnoreProperties = new string[] { "PspMaster", "OrgMaster", "DisasterMaster", "ReferenceGuideSearchView",
                        "PspApprovalHistory", "PspAttachment", "PspEvent", "PermitNum"}
                });

                var oldDisasterMasterId = oldPspMaster.DisasterMaster == null ? 0 : oldPspMaster.DisasterMaster.DisasterMasterId;
                var newDisasterMasterId = newPspMaster.DisasterMaster == null ? 0 : newPspMaster.DisasterMaster.DisasterMasterId;

                if (CommonHelper.HasValueChanged(oldPspMaster.OrgMaster.OrgId, newPspMaster.OrgMaster.OrgId) ||
                    CommonHelper.HasValueChanged(oldDisasterMasterId, newDisasterMasterId) || !result.AreEqual)
                {
                    var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
                    var logBuilder = new StringBuilder();
                    logBuilder.AppendFormat("PSP Reference: {0}", oldPspMaster.PspRef).AppendLine();

                    var log = new ActivityLog
                    {
                        RecordKey = oldPspMaster.PspMasterId.ToString(),
                        Activity = "PSP",
                        Action = "Update",
                        ActionedOn = DateTime.Now,
                        User = _userRepository.GetById(currentUser.UserId),
                        Post = _postRepository.GetById(currentUser.PostId),
                        Remark = logBuilder.ToString()
                    };
                    this._userlogRepository.Add(log);
                    _eventPublisher.EntityInserted<ActivityLog>(log);

                    return log;
                }
                else if (oldPspMaster.PermitNum.IsNotNullOrEmpty() && !oldPspMaster.PermitNum.Equals(newPspMaster.PermitNum))
                {
                    //CR-005 release the permit num
                    var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
                    var logBuilder = new StringBuilder();
                    logBuilder.AppendFormat("PSP Reference: {0}  Permit Num.: {1}", oldPspMaster.PspRef, oldPspMaster.PermitNum);

                    var log = new ActivityLog
                    {
                        RecordKey = oldPspMaster.PspMasterId.ToString(),
                        Activity = "PSP",
                        Action = "Release Permit No.",
                        ActionedOn = DateTime.Now,
                        User = _userRepository.GetById(currentUser.UserId),
                        Post = _postRepository.GetById(currentUser.PostId),
                        Remark = logBuilder.ToString()
                    };
                    this._userlogRepository.Add(log);
                    _eventPublisher.EntityInserted<ActivityLog>(log);

                    return log;
                }
            }
            else
            {
                var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
                var logBuilder = new StringBuilder();
                logBuilder.AppendFormat("PSP Reference: {0}", newPspMaster.PspRef).AppendLine();

                var log = new ActivityLog
                {
                    RecordKey = newPspMaster.PspMasterId.ToString(),
                    Activity = "PSP",
                    Action = "Create",
                    ActionedOn = DateTime.Now,
                    User = _userRepository.GetById(currentUser.UserId),
                    Post = _postRepository.GetById(currentUser.PostId),
                    Remark = logBuilder.ToString()
                };
                ActivityLogToApplicationLog(log);
                return log;
            }

            return null;
        }

        public ActivityLog LogLoginInformation(int Mode, string IPAddress)
        {
            /*
                Please log those information related to:
                1) User Login / Logout Information - Mode : 0 - Login; 1 - Logout
             */

            var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
            var logBuilder = new StringBuilder();

            var user = _userRepository.GetById(currentUser.UserId);
            var LogTime = DateTime.Now;
            var LogType = Mode == 0 ? "Login" : "Logout";
            var LogCode = Mode == 0 ? 
                            _logCodeDict[Constant.SystemParameter.LOG_CODE_LOGIN_SUCCESS] 
                            : _logCodeDict[Constant.SystemParameter.LOG_CODE_LOGOUT];
            var LogMsg = Mode == 0 ?
                        $"Login succeeded by user \"{currentUser.UserId}\"" :
                        $"Logout succeeded by user \"{currentUser.UserId}\"";

            logBuilder.AppendFormat("(IP Address: {0});\"{1}\"", IPAddress, currentUser.UserId).AppendLine();

            var log = new ActivityLog
            {
                Activity = "Login / Logout",
                Action = LogType,
                ActionedOn = LogTime,
                User = user,
                Post = _postRepository.GetById(currentUser.PostId),
                Remark = logBuilder.ToString()
            };            

            this._userlogRepository.Add(log);
            _eventPublisher.EntityInserted<ActivityLog>(log);

            ActivityLogToApplicationLog(log, LogCode, LogMsg, log.Remark);
            return log;
        }

        public ActivityLog LogLoginWrongPassword(string UserId, string IPAddress)
        {
            /*
                Please log those information related to:
                1) User Login / Logout Information - Mode : 0 - Login; 1 - Logout
             */

            var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
            var logBuilder = new StringBuilder();

            var userID = _userRepository.GetById(UserId);            
            var LogTime = DateTime.Now;
            var LogType = "Wrong Password";           
                        
            logBuilder.AppendFormat("(IP Address: {0});\"{1}\"", IPAddress,UserId).AppendLine();
            
            var log = new ActivityLog
            {
                Activity = "Login / Logout",
                Action = LogType,
                ActionedOn = LogTime,
                User = userID,
                
                Post = _postRepository.Table.Where(p => p.Owner.UserId == UserId).ToList().First(),
                Remark = logBuilder.ToString()
            };
            this._userlogRepository.Add(log);
            _eventPublisher.EntityInserted<ActivityLog>(log);

            string logCode = _logCodeDict[Constant.SystemParameter.LOG_CODE_LOGIN_FAIL];
            string logMsg = $"Login failed by user \"{UserId}\", due to wrong password.";
            ActivityLogToApplicationLog(log, logCode, logMsg, log.Remark);
            return log;
        }

        public ActivityLog LogAccountLockedByInvalidAttempts(string IPAddress, string UserId, int Attempts, int AttemptsLimit) {
            var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
            var logBuilder = new StringBuilder();
            var userID = _userRepository.GetById(currentUser.UserId);

            logBuilder.AppendFormat("(IP Address: {0}, Wrong Attempts: {1})", IPAddress,Attempts).AppendLine();
            logBuilder.AppendFormat(";\"{0}\"", UserId);

            var log = new ActivityLog
            {
                Activity = "User",
                Action = "Locked",
                ActionedOn = DateTime.Now,
                User = _userRepository.GetById(currentUser.UserId),
                Post = _postRepository.GetById(currentUser.PostId),
                Remark = logBuilder.ToString(),
            };

            this._userlogRepository.Add(log);
            _eventPublisher.EntityInserted<ActivityLog>(log);

            string logCode = _logCodeDict[Constant.SystemParameter.LOG_CODE_LOGIN_LOCK];
            string logMsg = $"User account of user \"{UserId}\" lockout due to wrong password inputted for {Attempts} times.";
            ActivityLogToApplicationLog(log, logCode, logMsg, log.Remark);
            return log;
        }

        public ActivityLog LogChangePasswordAttempt(string IPAddress, string Message = "" ,bool Changed = false)
        {
            var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
            var logBuilder = new StringBuilder();
            var userID = _userRepository.GetById(currentUser.UserId);
            var ActionStatus = "Fail";
            var LogCode = _logCodeDict[Constant.SystemParameter.LOG_CODE_PASSWORD_CHANGE_FAIL];
            var LogMsg = $"Password change failed by user \"{currentUser.UserId}\".";

            if (Changed) {
                ActionStatus = "Success";
                LogCode = _logCodeDict[Constant.SystemParameter.LOG_CODE_PASSWORD_CHANGE_SUCCESS];
                logBuilder.AppendFormat("SUCCESS").AppendLine();
                LogMsg = $"Password change suceeded by user \"{currentUser.UserId}\".";
            }
            
            if (!string.IsNullOrEmpty(Message)) { logBuilder.AppendFormat("(IPAddress: {0},Message: {1})",IPAddress, Message).AppendLine(); }
            logBuilder.AppendFormat(";\"{0}\"", currentUser.UserId);

            var log = new ActivityLog
            {
                RecordKey = currentUser.UserId,
                Activity = "User",
                Action = $"Change Password Attempt ({ActionStatus})",
                ActionedOn = DateTime.Now,
                User = _userRepository.GetById(currentUser.UserId),
                Post = _postRepository.GetById(currentUser.PostId),
                Remark = logBuilder.ToString(),
            };
            ActivityLogToApplicationLog(log, LogCode ,LogMsg, log.Remark);           
            return log;
        }

        public ActivityLog LogCRUDUser(string UserId, string IPAddress, List<string> LogCodeList, string PostId = null)
        {
            var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
            var logBuilder = new StringBuilder();
            var user = _userRepository.GetById(currentUser.UserId);
            
            logBuilder.AppendFormat(";\"{0}\"", UserId).AppendLine();
            if (!string.IsNullOrEmpty(PostId)) {
                logBuilder.AppendFormat(";\"{0}\"", PostId).AppendLine();
            }
            logBuilder.AppendFormat(";\"{0}\"", currentUser.UserId).AppendLine();

            string LogMsg = "";
            string LogCode = LogCodeList != null && LogCodeList.Count > 0 ?
                    string.Join(",", LogCodeList.Select(x => _logCodeDict[x])) 
                    : _logCodeDict[Constant.SystemParameter.LOG_CODE_USER_UPDATE_GENERAL];


            var log = new ActivityLog
            {
                RecordKey = UserId,
                Activity = "User",
                Action = "",
                ActionedOn = DateTime.Now,
                User = _userRepository.GetById(currentUser.UserId),
                Post = _postRepository.GetById(currentUser.PostId),
                Remark = logBuilder.ToString(),
            };

            if (LogCodeList == null || LogCodeList.Count == 0)
            {
                LogMsg = $"The User Account \"{UserId}\" is updated by user \"{currentUser.UserId}\".";
                ActivityLogToApplicationLog(log, LogCode, LogMsg, log.Remark);
            }
            else 
            {
                if (LogCodeList.Any(x => x.ToUpper().Contains("CREATE")))
                {
                    log.Action = $"Created User ({UserId})";
                    LogMsg = $"A new User Account \"{UserId}\" is created by user \"{currentUser.UserId}\".";
                    ActivityLogToApplicationLog(log, LogCode, LogMsg, log.Remark);
                }
                else
                {
                    log.Action = $"Updated User ({UserId})";
                    log.Action += LogCodeList != null && LogCodeList.Count > 0 ? string.Join(",", LogCodeList).Replace("LogCodeUser", "").Replace("LogCodeUpdate", "") : "";

                    foreach (var l in LogCodeList)
                    {
                        LogCode = (_logCodeDict.ContainsKey(l)) ? _logCodeDict[l] : l;
                        if (l.ToUpper().Contains("ADMIN"))
                        {
                            if (l.ToUpper().Contains("ON")) { LogMsg = $"The User Account \"{UserId}\" is elevated to Administrator by user \"{currentUser.UserId}\"."; }
                            else
                            if (l.ToUpper().Contains("OFF")) { LogMsg = $"The User Account \"{UserId}\" is downgraded to a non-administrator by user \"{currentUser.UserId}\"."; }
                        }
                        else
                        if (l.ToUpper().Contains("ACTIVE"))
                        {
                            if (l.ToUpper().Contains("ON")) { LogMsg = $"The User Account \"{UserId}\" is enabled by user \"{currentUser.UserId}\"."; }
                            else
                            if (l.ToUpper().Contains("OFF")) { LogMsg = $"The User Account \"{UserId}\" is disabled by user \"{currentUser.UserId}\"."; }
                        }
                        else
                        if (l.ToUpper().Contains("POST"))
                        {
                            LogMsg = $"The post of User Account \"{UserId}\" is updated to \"{PostId}\" by user \"{currentUser.UserId}\".";
                        }
                        else
                        if (l.ToUpper().Contains("PASSWORD"))
                        {
                            LogMsg = $"The password of User Account \"{UserId}\" is updated by user \"{currentUser.UserId}\".";
                        }
                        ActivityLogToApplicationLog(log, LogCode, LogMsg, log.Remark);
                    }
                }
            }
            return log;
        }

        public ActivityLog LogCRUDActing(string Mode ,string IPAddress, string ActingId ,string AssignTo = "", string AssignedPost = "")
        {
            var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
            var logBuilder = new StringBuilder();
            var userID = _userRepository.GetById(currentUser.UserId);

            var DataStr = "";
            if (!string.IsNullOrEmpty(AssignTo)) 
            {
                DataStr+= String.Format("Assigned To: {0},", AssignTo);
            }
            if (!string.IsNullOrEmpty(AssignedPost))
            {
                DataStr += String.Format("Assigned Post: {0}", AssignedPost);
            }

            logBuilder.AppendFormat(";\"{0}\"", currentUser.UserId);
            logBuilder.AppendFormat(";\"{0}\"", currentUser.PostId);

            var log = new ActivityLog
            {
                RecordKey = ActingId,
                Activity = "Acting",
                Action = Mode,
                ActionedOn = DateTime.Now,
                User = _userRepository.GetById(currentUser.UserId),
                Post = _postRepository.GetById(currentUser.PostId),
                Remark = logBuilder.ToString(),
            };

            string LogCode =
                (Mode.ToUpper().Contains("CREATE")) ? _logCodeDict[Constant.SystemParameter.LOG_CODE_CREATE_ACTING] :
                (Mode.ToUpper().Contains("UPDATE")) ? _logCodeDict[Constant.SystemParameter.LOG_CODE_UPDATE_ACTING] :
                (Mode.ToUpper().Contains("DELETE")) ? _logCodeDict[Constant.SystemParameter.LOG_CODE_DELETE_ACTING] : "";
            string LogMsg = "";
            if (Mode.ToUpper().Contains("CREATE"))
            {
                LogMsg = $"The acting of post \"{currentUser.PostId}\" is created by user \"{currentUser.UserId}\".";
            }
            else
            if (Mode.ToUpper().Contains("UPDATE"))
            {
                LogMsg = $"The acting of post \"{currentUser.PostId}\" is updated by user \"{currentUser.UserId}\".";
            }
            else
            if (Mode.ToUpper().Contains("DELETE"))
            {
                LogMsg = $"The acting of post \"{currentUser.PostId}\" is deleted by user \"{currentUser.UserId}\".";
            }
            ActivityLogToApplicationLog(log, LogCode, LogMsg, log.Remark);
            return log;
        }

        public ActivityLog LogCRUDPost(string Mode, string IPAddress, string PostId,string DataStr, List<string> LogCodeList)
        {
            var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
            var logBuilder = new StringBuilder();
            var userID = _userRepository.GetById(currentUser.UserId);
            
            logBuilder.AppendFormat(";\"{0}\"", currentUser.UserId);
            logBuilder.AppendFormat(";\"{0}\"", PostId);


            var log = new ActivityLog
            {
                RecordKey = PostId,
                Activity = "Post",
                Action = Mode,
                ActionedOn = DateTime.Now,
                User = _userRepository.GetById(currentUser.UserId),
                Post = _postRepository.GetById(currentUser.PostId),
                Remark = logBuilder.ToString(),
            };

            string logCode = "";
            string logMsg = "";
            if (LogCodeList != null && LogCodeList.Count > 0)
            {
                foreach (var l in LogCodeList)
                {
                    logCode = _logCodeDict.ContainsKey(l) ? _logCodeDict[l] : l;
                    if (l.ToUpper().Contains("CREATE"))
                    {                        
                        logMsg = $"A new post \"{PostId}\" is created by user \"{currentUser.UserId}\".";
                    }
                    else
                    if (l.ToUpper().Contains("OWNER"))
                    {
                        logMsg = $"The post \"{PostId}\" Owner  is updated by user \"{currentUser.UserId}\".";
                    }
                    else
                    if (l.ToUpper().Contains("ROLE"))
                    {
                        logMsg = $"The post \"{PostId}\" Role  is updated by user \"{currentUser.UserId}\".";
                    }
                    else
                    if (l.ToUpper().Contains("RANK"))
                    {
                        logMsg = $"The post \"{PostId}\" Rank  is updated by user \"{currentUser.UserId}\".";
                    }
                    else
                    if (l.ToUpper().Contains("SUPERVISOR"))
                    {
                        logMsg = $"The post \"{PostId}\" Supervisor  is updated by user \"{currentUser.UserId}\".";
                    }

                    ActivityLogToApplicationLog(log, logCode, logMsg, log.Remark);
                }
            }
            
            return log;
        }

        public ActivityLog LogCRUDRole(string Mode, string IPAddress, string RoleId,string DataStr)
        {
            var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
            var logBuilder = new StringBuilder();
            var userID = _userRepository.GetById(currentUser.UserId);

            if (!string.IsNullOrEmpty(DataStr))
            {
                logBuilder.AppendFormat("({0})",DataStr).AppendLine();
            }

            logBuilder.AppendFormat(";\"{0}\"", currentUser.UserId);
            logBuilder.AppendFormat(";\"{0}\"", RoleId);

            var log = new ActivityLog
            {
                RecordKey = RoleId,
                Activity = "Role",
                Action = Mode,
                ActionedOn = DateTime.Now,
                User = _userRepository.GetById(currentUser.UserId),
                Post = _postRepository.GetById(currentUser.PostId),
                Remark = logBuilder.ToString(),
            };
            string LogCode = "";
            string LogMsg = "";
            if (Mode.ToUpper().Contains("CREATE"))
            {
                LogCode = _logCodeDict[Constant.SystemParameter.LOG_CODE_CREATE_ROLE];
                LogMsg = $"A new role \"{RoleId}\" is created by user \"{currentUser.UserId}\".";
            }
            else
            if (Mode.ToUpper().Contains("MEMBER"))
            {
                LogCode = _logCodeDict[Constant.SystemParameter.LOG_CODE_UPDATE_ROLE_MEMBER];
                LogMsg = $"The role \"{RoleId}\" has assigned role members updated by user \"{currentUser.UserId}\".";
            }
            else if (Mode.ToUpper().Contains("UPDATE"))
            {
                LogCode = _logCodeDict[Constant.SystemParameter.LOG_CODE_UPDATE_ROLE];
                LogMsg = $"The role \"{RoleId}\" has access rights updated by user \"{currentUser.UserId}\".";

            }
            else if (Mode.ToUpper().Contains("DELETE"))
            {
                LogCode = _logCodeDict[Constant.SystemParameter.LOG_CODE_DELETE_ROLE];
                LogMsg = $"The role \"{RoleId}\" was deleted by user \"{currentUser.UserId}\".";

            }
            ActivityLogToApplicationLog(log, LogCode, LogMsg, log.Remark);
            return log;
        }

        public int GetInvalidLoginAttempts(string UserId) {
            int result = 0;

            int check_range_day = Convert.ToInt32(_parameterService.GetParameterByCode("InvalidLoginAttempsCheckRangeDay").Value);
            int check_range_hour = Convert.ToInt32(_parameterService.GetParameterByCode("InvalidLoginAttempsCheckRangeHour").Value);
            int check_range_min = Convert.ToInt32(_parameterService.GetParameterByCode("InvalidLoginAttempsCheckRangeMinute").Value);
            var lookBackDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0)
                    .AddDays(check_range_day * -1)
                    .AddHours(check_range_hour * -1)
                    .AddMinutes(check_range_min * -1);
            var user = _userRepository.GetById(UserId);
            result  = _userlogRepository.Table.Where(
                l => 
                l.Action.ToUpper().Contains("WRONG PASSWORD") &&
                l.User == user &&
                l.ActionedOn >= lookBackDate
            ).Count();
            return result;
        }

        private DateTime GetInvalidLoginAttemptsStartDate(User user)
        {
            DateTime result = new DateTime();
            var last_login_result = _userlogRepository.Table.Where(
                l =>
                l.User == user &&
                l.Action.ToUpper().Contains("LOGIN")
            ).OrderByDescending(l => l.ActionedOn);


            DateTime last_login_date = (last_login_result != null && last_login_result.Count() > 0) ?
                last_login_result.FirstOrDefault().ActionedOn : new DateTime();
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            result = (last_login_date > today) ? last_login_date : today;
            return result;
        }

        public ActivityLog GetUserLogById(int userLogId)
        {
            if (userLogId == 0)
                return null;

            return _userlogRepository.GetById(userLogId);
        }

        public Core.Models.IPagedList<ActivityLog> GetPage(Core.JqGrid.Models.GridSettings grid)
        {
            return _userlogRepository.GetPage(grid);
        }

        private void ActivityLogToApplicationLog(ActivityLog log, bool noActivity = false, bool noRemark = false)
        {
            string msg = "";
            string Activity = (!noActivity) ? $" {log.Activity}" : "";
            string Remark = (!noRemark) ? $" ({log.Remark.Replace("\n", " ").Replace("\r", " ")})" : "";
            msg = $"[{log.User.UserId}/{log.Post.PostId}] {log.Action}{Activity}{Remark}";

            if (!string.IsNullOrEmpty(msg))
            {
                _logger.Info(msg);
            }
        }

        private Dictionary<string, string> LogCodeDictionary() {
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                result = new Dictionary<string, string>()
                {
                {Constant.SystemParameter.LOG_CODE_LOGIN_FAIL, GetLogCode(Constant.SystemParameter.LOG_CODE_LOGIN_FAIL) },
                {Constant.SystemParameter.LOG_CODE_LOGIN_SUCCESS ,GetLogCode(Constant.SystemParameter.LOG_CODE_LOGIN_SUCCESS)},
                {Constant.SystemParameter.LOG_CODE_LOGOUT ,GetLogCode(Constant.SystemParameter.LOG_CODE_LOGOUT)},
                {Constant.SystemParameter.LOG_CODE_LOGIN_LOCK ,GetLogCode(Constant.SystemParameter.LOG_CODE_LOGIN_LOCK)},

                {Constant.SystemParameter.LOG_CODE_PASSWORD_CHANGE_FAIL ,GetLogCode(Constant.SystemParameter.LOG_CODE_PASSWORD_CHANGE_FAIL)},
                {Constant.SystemParameter.LOG_CODE_PASSWORD_CHANGE_SUCCESS ,GetLogCode(Constant.SystemParameter.LOG_CODE_PASSWORD_CHANGE_SUCCESS)},

                {Constant.SystemParameter.LOG_CODE_CREATE_ROLE ,GetLogCode(Constant.SystemParameter.LOG_CODE_CREATE_ROLE)},
                {Constant.SystemParameter.LOG_CODE_UPDATE_ROLE ,GetLogCode(Constant.SystemParameter.LOG_CODE_UPDATE_ROLE)},

                {Constant.SystemParameter.LOG_CODE_UPDATE_ROLE_MEMBER ,GetLogCode(Constant.SystemParameter.LOG_CODE_UPDATE_ROLE_MEMBER)},
                {Constant.SystemParameter.LOG_CODE_DELETE_ROLE ,GetLogCode(Constant.SystemParameter.LOG_CODE_DELETE_ROLE)},

                {Constant.SystemParameter.LOG_CODE_CREATE_POST ,GetLogCode(Constant.SystemParameter.LOG_CODE_CREATE_POST)},
                {Constant.SystemParameter.LOG_CODE_UPDATE_POST ,GetLogCode(Constant.SystemParameter.LOG_CODE_UPDATE_POST)},

                {Constant.SystemParameter.LOG_CODE_UPDATE_POST_OWNER ,GetLogCode(Constant.SystemParameter.LOG_CODE_UPDATE_POST_OWNER)},
                {Constant.SystemParameter.LOG_CODE_UPDATE_POST_RANK ,GetLogCode(Constant.SystemParameter.LOG_CODE_UPDATE_POST_RANK)},
                {Constant.SystemParameter.LOG_CODE_UPDATE_POST_SUPERVISOR ,GetLogCode(Constant.SystemParameter.LOG_CODE_UPDATE_POST_SUPERVISOR)},
                {Constant.SystemParameter.LOG_CODE_UPDATE_POST_ROLE ,GetLogCode(Constant.SystemParameter.LOG_CODE_UPDATE_POST_ROLE)},

                {Constant.SystemParameter.LOG_CODE_CREATE_USER ,GetLogCode(Constant.SystemParameter.LOG_CODE_CREATE_USER)},
                {Constant.SystemParameter.LOG_CODE_UPDATE_USER_POST ,GetLogCode(Constant.SystemParameter.LOG_CODE_UPDATE_USER_POST)},
                {Constant.SystemParameter.LOG_CODE_UPDATE_USER_PASSWORD ,GetLogCode(Constant.SystemParameter.LOG_CODE_UPDATE_USER_PASSWORD)},

                {Constant.SystemParameter.LOG_CODE_USER_ADMIN_ON ,GetLogCode(Constant.SystemParameter.LOG_CODE_USER_ADMIN_ON)},
                {Constant.SystemParameter.LOG_CODE_USER_ADMIN_OFF ,GetLogCode(Constant.SystemParameter.LOG_CODE_USER_ADMIN_OFF)},

                {Constant.SystemParameter.LOG_CODE_USER_ACTIVE_ON ,GetLogCode(Constant.SystemParameter.LOG_CODE_USER_ACTIVE_ON)},
                {Constant.SystemParameter.LOG_CODE_USER_ACTIVE_OFF ,GetLogCode(Constant.SystemParameter.LOG_CODE_USER_ACTIVE_OFF)},

                {Constant.SystemParameter.LOG_CODE_USER_UPDATE_GENERAL ,GetLogCode(Constant.SystemParameter.LOG_CODE_USER_UPDATE_GENERAL)},

                {Constant.SystemParameter.LOG_CODE_UPDATE_ACTING_POST ,GetLogCode(Constant.SystemParameter.LOG_CODE_UPDATE_ACTING_POST)},
                {Constant.SystemParameter.LOG_CODE_CREATE_ACTING ,GetLogCode(Constant.SystemParameter.LOG_CODE_CREATE_ACTING)},
                {Constant.SystemParameter.LOG_CODE_UPDATE_ACTING ,GetLogCode(Constant.SystemParameter.LOG_CODE_UPDATE_ACTING)},
                {Constant.SystemParameter.LOG_CODE_DELETE_ACTING ,GetLogCode(Constant.SystemParameter.LOG_CODE_DELETE_ACTING)},
                };
            }
            catch (Exception ex) {
                string msg = "";
                msg += ex.Message;
                msg += "\r\n";
                msg += "******************";
                msg += "\r\n";
                msg += ex.StackTrace;
                msg += "\r\n";
                msg += "******************";
                msg += "\r\n";
                msg += ex.InnerException;
                ActivityLogToApplicationLog(null, "Error",msg);
            }
            
            return result;
        }

        private string GetLogCode(string ConstantName) {
            string LogCode = "";
            
            var param = _parameterService.GetParameterByCode(ConstantName);
            LogCode = (param != null) ? param.Value : ConstantName;
            return LogCode;
        }
        

        private void ActivityLogToApplicationLog(ActivityLog log, string LogCode, string Message, string InfoStr = null)
        {
            string msg = "";
            InfoStr = !string.IsNullOrEmpty(InfoStr) ? $"{InfoStr.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ")}" : "";
            msg = $"[{LogCode}];{Message}{InfoStr}";

            if (!string.IsNullOrEmpty(msg))
            {
                _logger.Info(msg);
            }
        }

        public void LogToFile(string msg)
        {   
            if (!string.IsNullOrEmpty(msg))
            {
                _logger.Info(msg);
            }
        }
        #endregion Methods
    }
}