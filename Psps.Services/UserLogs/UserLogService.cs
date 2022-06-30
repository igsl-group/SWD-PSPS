using Psps.Core;
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
using System.Text;

namespace Psps.Services.UserLog
{
    public partial class UserLogService : IUserLogService
    {
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

        #region Ctor

        public UserLogService(IEventPublisher eventPublisher, IUserLogRepository UserLogRepository,
            IUserRepository userRepository, IPostRepository postRepository, IParameterService parameterService)
        {
            this._eventPublisher = eventPublisher;
            this._userlogRepository = UserLogRepository;
            this._userRepository = userRepository;
            this._postRepository = postRepository;
            this._parameterService = parameterService;
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

                this._userlogRepository.Add(log);
                _eventPublisher.EntityInserted<ActivityLog>(log);

                return log;
            }

            return null;
        }

        public ActivityLog LogComplaintInformation(ComplaintMaster oldComplaintMaster, ComplaintMaster newComplaintMaster)
        {
            /*
                Please log those information related to:
                1) Complaint Information
             */

            var result = ObjectComparer.Compare(oldComplaintMaster, newComplaintMaster, new CompareSettings
            {
                IgnoreProperties = new string[] { "ComplaintMaster", "PspApprovalHistory", "FdEvent", "OrgMaster", "RelatedComplaintMaster",
                    "ComplaintAttachment", "ComplaintTelRecord", "ComplaintFollowUpAction", "ComplaintPoliceCase", "ComplaintOtherDepartmentEnquiry"}
            });

            var oldPspApprovalHistoryId = oldComplaintMaster.PspApprovalHistory == null ? 0 : oldComplaintMaster.PspApprovalHistory.PspApprovalHistoryId;
            var newPspApprovalHistoryId = newComplaintMaster.PspApprovalHistory == null ? 0 : newComplaintMaster.PspApprovalHistory.PspApprovalHistoryId;
            var oldFdEventId = oldComplaintMaster.FdEvent == null ? 0 : oldComplaintMaster.FdEvent.FdEventId;
            var newFdEventId = newComplaintMaster.FdEvent == null ? 0 : newComplaintMaster.FdEvent.FdEventId;
            var oldOrgId = oldComplaintMaster.OrgMaster == null ? 0 : oldComplaintMaster.OrgMaster.OrgId;
            var newOrgId = newComplaintMaster.OrgMaster == null ? 0 : newComplaintMaster.OrgMaster.OrgId;
            var oldComplaintMasterId = oldComplaintMaster.RelatedComplaintMaster == null ? 0 : oldComplaintMaster.RelatedComplaintMaster.ComplaintMasterId;
            var newComplaintMasterId = newComplaintMaster.RelatedComplaintMaster == null ? 0 : newComplaintMaster.RelatedComplaintMaster.ComplaintMasterId;

            if (CommonHelper.HasValueChanged(oldPspApprovalHistoryId, newPspApprovalHistoryId) ||
                CommonHelper.HasValueChanged(oldFdEventId, newFdEventId) ||
                CommonHelper.HasValueChanged(oldOrgId, newOrgId) ||
                CommonHelper.HasValueChanged(oldComplaintMasterId, newComplaintMasterId) ||
                !result.AreEqual)
            {
                var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
                var logBuilder = new StringBuilder();
                logBuilder.AppendFormat("Complaint Reference: {0}", oldComplaintMaster.ComplaintRef).AppendLine();

                var log = new ActivityLog
                {
                    RecordKey = oldComplaintMaster.ComplaintMasterId.ToString(),
                    Activity = "Complaint",
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

        public ActivityLog LogSuggestionMasterInformation(SuggestionMaster oldSuggestionMaster, SuggestionMaster newSuggestionMaster)
        {
            /*
                Please log those information related to:
                1) Suggestion Information
             */

            var result = ObjectComparer.Compare(oldSuggestionMaster, newSuggestionMaster, new CompareSettings
            {
                IgnoreProperties = new string[] { "SuggestionMaster", "SuggestionAttachment" }
            });

            if (!result.AreEqual)
            {
                var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
                var logBuilder = new StringBuilder();
                logBuilder.AppendFormat("Suggestion Reference: {0}", oldSuggestionMaster.SuggestionRefNum).AppendLine();

                var log = new ActivityLog
                {
                    RecordKey = oldSuggestionMaster.SuggestionMasterId.ToString(),
                    Activity = "Suggestion",
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

        public ActivityLog LogLoginInformation(int Mode)
        {
            /*
                Please log those information related to:
                1) User Login / Logout Information - Mode : 0 - Login; 1 - Logout
             */

            var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
            var logBuilder = new StringBuilder();

            var userID = _userRepository.GetById(currentUser.UserId);
            var LogTime = DateTime.Now;
            var LogType = Mode == 0 ? "Login" : "Logout";

            logBuilder.AppendFormat("User ID: {0}", currentUser.UserId).AppendLine();
            //logBuilder.AppendFormat("User ID: {0}; Time: {1}", currentUser.UserId, LogTime.ToString()).AppendLine();

            var log = new ActivityLog
            {
                Activity = "Login / Logout",
                Action = LogType,
                ActionedOn = LogTime,
                User = userID,
                Post = _postRepository.GetById(currentUser.PostId),
                Remark = logBuilder.ToString()
            };

            this._userlogRepository.Add(log);
            _eventPublisher.EntityInserted<ActivityLog>(log);

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

            logBuilder.AppendFormat("User ID: {0}", UserId).AppendLine();
            logBuilder.AppendFormat("IP Address: ({0})", IPAddress).AppendLine();
            //logBuilder.AppendFormat("User ID: {0}; Time: {1}", currentUser.UserId, LogTime.ToString()).AppendLine();

            
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

            return log;
        }

        public int GetInvalidLoginAttemps(string UserId) {
            int result = 0;

            int check_range_day = Convert.ToInt32(_parameterService.GetParameterByCode("InvalidLoginAttempsCheckRangeDay").Value);
            int check_range_hour = Convert.ToInt32(_parameterService.GetParameterByCode("InvalidLoginAttempsCheckRangeHour").Value);
            int check_range_min = Convert.ToInt32(_parameterService.GetParameterByCode("InvalidLoginAttempsCheckRangeMinute").Value);

            var user = _userRepository.GetById(UserId);
            result  = _userlogRepository.Table.Where(
                l => 
                l.Action.ToUpper().Contains("WRONG PASSWORD") &&
                l.User == user &&
                l.ActionedOn >= GetInvalidLoginAttempsStartDate(user)
                                .AddDays(check_range_day * -1)
                                .AddHours(check_range_hour * -1)
                                .AddMinutes(check_range_min * -1)
            ).Count();
            return result;
        }

        private DateTime GetInvalidLoginAttempsStartDate(User user)
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

        #endregion Methods
    }
}