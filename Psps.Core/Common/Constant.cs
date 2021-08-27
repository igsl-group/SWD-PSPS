using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Core.Common
{
    public static class Constant
    {
        public static readonly string DISASTERMASTER_ALL_KEY = "Psps.Disaster.all-{0}";
        public static readonly string DISASTERMASTER_FOR_DROWDROP_KEY = "Psps.Disaster.dropdown.all";
        public static readonly string DISASTERMASTER_FOR_DROWDROP_GTSYSDATE_KEY = "Psps.DisasterName.GtSysdate.dropdown.all";
        public static readonly string DISASTERMASTER_FOR_DROWDROP_NAME = "Psps.Disaster.DisasterName.dropdown.all";
        public static readonly string DISASTERMASTER_PATTERN_KEY = "Psps.Disaster.";
        public static readonly string DISASTERMASTER_FOR_DROWDROP_PATTERN_KEY = "Psps.Disaster.dropdown.";

        public static readonly string DISASTERSTATISTICS_ALL_KEY = "Psps.DisasterStatistics.all-{0}";
        public static readonly string DISASTERSTATISTICS_PATTERN_KEY = "Psps.DisasterStatistics.";
        public static readonly string DISASTERSTATISTICS_FOR_DROWDROP_KEY = "Psps.DisasterStatistics.dropdown.all";
        public static readonly string DISASTERSTATISTICS_FOR_DROWDROP_PATTERN_KEY = "Psps.DisasterStatistics.dropdown.";

        public static readonly string LOOKUP_ALL_KEY = "Psps.lookup.all-{0}";
        public static readonly string LOOKUP_PATTERN_KEY = "Psps.lookup.";
        public static readonly string LOOKUP_FOR_DROWDROP_KEY = "Psps.lookup.dropdown.all-{0}";
        public static readonly string LOOKUP_BY_TYPE_AND_CODE_KEY = "Psps.lookup.{0}-{1}";
        public static readonly string LOOKUP_BY_TYPE_KEY = "Psps.lookup.all-{0}";

        public static readonly string FLAGDAYLISTTYPE_FOR_DROWDROP_KEY = "Psps.FdListTypes.dropdown.all";
        public static readonly string FLAGDAYLISTYEAR_FOR_DROWDROP_KEY = "Psps.FdListYears.dropdown.all";
        public static readonly string FLAGDAYTEMPLATE_FOR_DROWDROP_KEY = "Psps.FdTemplate.dropdown.all";

        public static readonly string ORGANISATION_FOR_DROWDROP_KEY = "Psps.OrgMaster.dropdown.all";

        public static readonly string LegalAdvice_FOR_DROWDROP_KEY = "Psps.LegalAdvice.dropdown.all";
        public static readonly string FLAGDAY_SEARCH_SESSION = "psps.FlagDay.Search.Session";
        public static readonly string PSP_SEARCH_SESSION = "psps.Psp.Search.Session";
        public static readonly string PSP_DEFAULT_SEARCH_SESSION = "psps.Psp.Default_Search.Session";
        public static readonly string PSP_DEFAULT_EXPORT_SESSION = "psps.Psp.Default_Export.Session";
        public static readonly string YEAROFPSPLIST_FOR_DROWDROP_KEY = "Psps.YearOfPspList.dropdown.all";
        public static readonly string EVENTYEARLIST_FOR_DROWDROP_KEY = "Psps.EventYearList.dropdown.all";

        public static readonly string SYSTEMPARAMETER_ALL_KEY = "Psps.systemparameter.all";
        public static readonly string SYSTEMPARAMETER_BY_CODE_KEY = "Psps.systemparameter.{0}";
        public static readonly string SYSTEMPARAMETER_PATTERN_KEY = "Psps.systemparameter.";


        public static readonly string POST_FOR_DROWDROP_KEY_PROCESSING_OFFICER = "Psps.post.dropdown.ProcessingOfficer";

        public static class Session
        {
            public const string ENQUIRY_COMPLAINT_SEARCH_SESSION = "EnquiryComplaintSearchSession";
        }

        public static class SystemParameter
        {
            //TODO: Add Enable, Admin flag
            public const string FLAGDAY_ENABLE_IMPORT = "FlagDayEnableImport";

            public const string PSP_REMINDER_DEADLINE = "PspReminderDeadline";
            public const string PSP_REMINDER_DEADLINE2 = "PspReminderDeadline2";
            public const string FD_DUEDATE = "FdDueDate";
            public const string FD_REMINDER_DEADLINE = "FdReminderDeadline";
            public const string FD_REMINDER_DEADLINE2 = "FdReminderDeadline2";
            public const string FD_BENCHMARK = "FdBenchmark";

            public const string FLAGDAY_EVENT_START_TIME = "FlagDayEventStartTime";
            public const string FLAGDAY_EVENT_END_TIME = "FlagDayEventEndTime";

            public const string PSP_TEMPLATE_PATH = "PspTemplatePath";
            public const string FLAGDAY_TEMPLATE_PATH = "FlagDayTemplatePath";
            public const string COMPLAINT_TEMPLATE_PATH = "ComplaintTemplatePath";
            public const string ORGANISATION_TEMPLATE_PATH = "OrganisationTemplatePath";
            public const string DOC_LIB_PATH = "DocLibPath";
            public const string SUGGESTION_TEMPLATE_PATH = "SuggestionTemplatePath";

            public const string ORG_ATTACHMENT_PATH = "OrgAttachmentPath";
            public const string PSP_ATTACHMENT_PATH = "PspAttachmentPath";
            public const string FD_ATTACHMENT_PATH = "FdAttachmentPath";
            public const string SUGGESTION_ATTACHMENT_PATH = "SuggestionAttachmentPath";
            public const string COMPLAINT_ATTACHMENT_PATH = "ComplaintAttachmentPath";

            public const string ORG_REF_GUIDE_FILE_NUM = "OrgRefGuideFileNum";

            public const string DEFAULT_PASSWORD = "DefaultPassword";
            public const string FRAS_URL = "FrasUrl";
            public const string FRAS_ENABLED = "FrasEnabled";
        }
    }
}