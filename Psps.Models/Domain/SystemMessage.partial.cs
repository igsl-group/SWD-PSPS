namespace Psps.Models.Domain
{
    public partial class SystemMessage
    {
        public static class Error
        {
            public static readonly string AlreadyExists = "Error.AlreadyExists";
            public static readonly string ConcurrentUpdate = "Error.ConcurrentUpdate";
            public static readonly string ConflictSolicitDate = "Error.ConflictSolictDate";
            public static readonly string ConfltSoltDtRng = "Error.ConfltSoltDtRng";
            public static readonly string ConfltFdDtRng = "Error.ConfltFdDtRng";
            public static readonly string FromDateLaterThanToDate = "Error.FromDateLaterThanToDate";
            public static readonly string InvalidDate = "Error.InvalidDate";
            public static readonly string InvalidEmail = "Error.InvalidEmail";
            public static readonly string InvalidXlsx = "Error.InvalidXlsx";
            public static readonly string Length = "Error.Length";
            public static readonly string Mandatory = "Error.Mandatory";
            public static readonly string NotFound = "Error.NotFound";
            public static readonly string Numeric = "Error.Numeric";
            public static readonly string NumberNotWithinRange = "Error.ValueRange";
            public static readonly string Unique = "Error.Unique";
            public static readonly string MustBeEarlierThan = "Error.MustBeEarlierThan";

            public static class User
            {
                public static readonly string PasswordCompositionInvalid = "Error.User.PasswordCompositionInvalid";
                public static readonly string PasswordHistoryCrash = "Error.User.PasswordHistoryCrash";
                public static readonly string PostHeldByOthers = "Error.User.PostHeldByOthers";
                public static readonly string NotMatchBothPasswords = "Error.User.NotMatchBothPasswords";
                public static readonly string OldPasswordIncorrect = "Error.User.OldPasswordIncorrect";
                public static readonly string InvalidUserError = "Error.User.Invalid";
                public static readonly string SamePostError = "Error.User.SamePost";
                public static readonly string UserIsAssignedError = "Error.User.IsAssigned";
                public static readonly string WrongCredentials = "Error.User.WrongCredentials";
                public static readonly string NotActive = "Error.User.NotActive";
                public static readonly string PasswordExpire = "Error.User.PasswordExpire";
            }

            public static class Lookup
            {
                public static readonly string Unique = "Error.Lookup.Unqiue";
                public static readonly string NotThanOrEqualOthers = "Error.Lookup.NotThanOrEqualOthers";
                public static readonly string ShouldThanOrEqualOthers = "Error.Lookup.ShouldThanOrEqualOthers";
                public static readonly string NotFound = "Error.Lookup.NotFound";
                public static readonly string ValueMustNumeric = "Error.Lookup.ValueMustNumeric";
            }

            public static class Post
            {
                public static readonly string OwnerHeldByOthers = "Error.Post.OwnerHeldByOthers";
                public static readonly string RankofSupervisor = "Error.Post.RankofSupervisor";
            }

            public static class Role
            {
                public static readonly string RecordDeleteError = "Error.Role.Delete";
                public static readonly string PostAlreadyExistedError = "Error.Role.PostAlreadyExisted";
            }

            public static class Letter
            {
                public static readonly string FileFormat = "Error.Template.FileFormat";
            }

            public static class Suggestion
            {
                public static readonly string FileFormat = "Error.Template.FileFormat";
                public static readonly string EnclNoMustBeInputted = "Error.Suggestion.EnclNoMustBeInputted";
                public static readonly string PartNoMustBeInputted = "Error.Suggestion.PartNoMustBeInputted";
            }

            public static class FlagDay
            {
                public static readonly string FileFormat = "Error.Template.FileFormat";
                public static readonly string fdDateError = "Error.FlagDay.FdDateError";
                public static readonly string fdDateDateofWeek = "Error.FlagDay.FdDateDateofWeek";
                public static readonly string fdDateImportError = "Error.FlagDay.FdDateImportError";
                public static readonly string fdDateDateofWeekImportError = "Error.FlagDay.FdDateDateofWeekImportError";
                public static readonly string FlagDayUniqueImportError = "Error.FlagDay.FlagDayUniqueImportError";
                public static readonly string FlagdayDayNotMatch = "Error.FlagDayList.FlagDayNotMatched";
                public static readonly string FlagdayDayNotAvaliable = "Error.FlagDay.FlagdayDayNotAvaliable";
                public static readonly string StartDayConflict = "Error.FlagDay.StartDayConflict";
                public static readonly string RegionalMustHaveRegion = "Error.FlagDay.RegionalMustHaveRegion";
                public static readonly string OnlyOneFdMasterIsAllowForEachOrg = "Error.FlagDay.OnlyOneFdMasterIsAllowForEachOrg";
                public static readonly string FlagDayConflictedRecords = "Error.FlagDay.FlagDayConflictedRecords";
                public static readonly string StartDtConFdDt = "Error.FlagDay.StartConflictFlagDayDt";
                public static readonly string EndDtConFdDt = "Error.FlagDay.EndConflictFlagDayDt";
            }

            public static class Psps
            {
                public static readonly string FileFormat = "Error.Template.FileFormat";
                public static readonly string approveTypeTwoBatch = "Error.Psps.TwBatchError";
                public static readonly string prevPspNotApprov = "Error.Psps.prevPspNotApprov";
                public static readonly string DupRecTimeOverLap = "Error.Psps.DupRecTimeOverLap";
                public static readonly string StartDtLessThanEndDt = "Error.Psps.StartDtLessThanEndDt";
                public static readonly string StartTimeSameAsEndTime = "Error.Psps.StartTimeSameAsEndTime";
                public static readonly string InValidTimeFmt = "Error.Psps.InValidTimeFmt";                
                //public static readonly string WithholdWarning = "Info.Psp.Withhold.Warning";
            }

            public static class Compliant
            {
                public static readonly string FileFormat = "Error.Template.FileFormat";
                public static readonly string OtherEnquiryDepartmentMustBeInputted = "Error.Compliant.OtherEnquiryDepartmentMustBeInputted";
                public static readonly string ContactDateMustEarlierThanIssueDate = "Error.Compliant.ContactDateMustEarlierThanIssueDate";
                public static readonly string NotAllowToSaveRecord = "Error.Compliant.ComplaintFollowUpAction.NotAllowToSaveRecord";
                public static readonly string NotCompResultDistinctMsg = "Error.Compliant.NotCompResultDistinctMsg";
            }

            public static class Organisation
            {
                public static readonly string FirstReminderIssuedMustEarlierThanFirstReminderDeadline = "Error.Organisation.FirstReminderIssuedMustEarlierThanFirstReminderDeadline";
                public static readonly string SecondReminderIssuedMustEarlierThanSecondReminderDeadline = "Error.Organisation.SecondReminderIssuedMustEarlierThanSecondReminderDeadline";
                public static readonly string ReplySlipDateMutual = "Error.Organisation.ReplySlipDateMutual";
                public static readonly string ReplySlipReceiveDateMutual = "Error.Organisation.ReplySlipReceiveDateMutual";
                public static readonly string MustBeEarlierOrEqual = "Error.Organisation.MustBeEarlierOrEqual";
                public static readonly string SendDateEarlierReplySlipDate = "Error.Organisation.SendDateEarlierReplySlipDate";
                public static readonly string SendDateEarlierReplySlipReceiveDate = "Error.Organisation.SendDateEarlierReplySlipReceiveDate";
                public static readonly string IncorrectImportRefGuideXlsSendDate = "Error.Organisation.ImportRefGuideXls.IncorrectSendDate";
                public static readonly string ImportRefGuideXlsRecordExisted = "Error.Organisation.ImportRefGuideXls.RecordExisted";
                public static readonly string OrgRefRecordNotExisted = "Error.Organisation.ImportRefGuideXls.OrgRefRecordNotExisted";
                public static readonly string SendDateIsEmpty = "Error.Organisation.ImportRefGuideXls.SendDateIsEmpty";
            }

            public static class EnquiryComplaint
            {
                public static readonly string ComplaintDateEqualOrEarlierThanFirstComplaintDate = "Error.EnquiryComplaint.ComplaintDateEqualOrEarlierThanFirstComplaintDate";
                public static readonly string OtherActivityConcernedMustBeInputted = "Error.EnquiryComplaint.OtherActivityConcernedMustBeInputted";
                public static readonly string WithholdingStartDateMustBeInputted = "Error.EnquiryComplaint.WithholdingStartDateMustBeInputted";
                public static readonly string WithholdingEndDateMustBeInputted = "Error.EnquiryComplaint.WithholdingEndDateMustBeInputted";
                public static readonly string WithholdinEndDateMustAfterStartDate = "Error.EnquiryComplaint.WithholdingEndDateMustAfterStartDate";
            }

            public static class DocumentLibrary
            {
                public static readonly string FolderCannotBeDeleted = "Error.DocumentLibrary.FolderCannotBeDeleted";
            }

            public static class DisasterStatistics
            {
                public static readonly string ReportingDateLaterThanToday = "Error.DisasterStatistics.ReportingDateLaterThanToday";
                public static readonly string ReportingDateWithinDisasterPeriod = "Error.DisasterStatistics.ReportingDateWithinDisasterPeriod";
                public static readonly string ReportingDateWithDisasterRecord = "Error.DisasterStatistics.ReportingDateWithDisasterRecord";
            }

            public static class DisasterMaster
            {
                public static readonly string DisasterNameUnique = "Error.DisasterMaster.DisasterNameUnique";
                public static readonly string BeginDateLaterThanEndDate = "Error.DisasterMaster.BeginDateLaterThanEndDate";
                public static readonly string BeginDateLaterThanToday = "Error.DisasterMaster.BeginDateLaterThanToday";
                public static readonly string DisasterDateLaterThanBeginDate = "Error.DisasterMaster.DisasterDateLaterThanBeginDate";
                public static readonly string DisasterDateLaterThanToday = "Error.DisasterMaster.DisasterDateLaterThanToday";
            }

            public static class OGCIO
            {
                public static readonly string AlreadySubmitted = "Error.Ogcio.AlreadySubmitted";
            }
        }

        public static class Info
        {
            public static readonly string RecordCreated = "Info.RecordCreated";
            public static readonly string RecordUpdated = "Info.RecordUpdated";
            public static readonly string RecordDeleted = "Info.RecordDeleted";
            public static readonly string FileUploaded = "Info.FileUploaded";

            public static readonly string GovHK = "Info.GovHK";

            public static readonly string PrintProforma = "Info.Psp.PrintEvent";
        }

        public static class Warning
        {
            public static class Psp
            {
                public static readonly string OrgDisabledOrWithheld = "Warning.Psp.OrgDisabledOrWithheld";
                public static readonly string DiffSection88 = "Warning.Psp.DiffSection88";
                public static readonly string NotAllEventsApproved = "Warning.Psp.NotAllEventsApproved";
            }
        }
    }
}