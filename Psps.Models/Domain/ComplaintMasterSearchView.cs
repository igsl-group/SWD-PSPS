using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class ComplaintMasterSearchView : BaseEntity<int>
    {
        public ComplaintMasterSearchView()
        {
            ComplaintAttachment = new List<ComplaintAttachment>();
            ComplaintTelRecord = new List<ComplaintTelRecord>();
            ComplaintFollowUpAction = new List<ComplaintFollowUpAction>();
            ComplaintPoliceCase = new List<ComplaintPoliceCase>();
            ComplaintOtherDepartmentEnquiry = new List<ComplaintOtherDepartmentEnquiry>();
            ComplaintResult = new List<ComplaintResult>();
        }

        public virtual int ComplaintMasterId { get; set; }

        public virtual string OrgRef { get; set; }

        public virtual string PspRef { get; set; }

        public virtual string FdRef { get; set; }

        public virtual string EngOrgName { get; set; }

        public virtual string EngOrgNameSorting { get; set; }

        public virtual string CmOrgNameSorting { get; set; }

        public virtual string ChiOrgName { get; set; }

        public virtual bool? DisableIndicator { get; set; }

        public virtual bool? SubventedIndicator { get; set; }

        public virtual string RegistrationType1 { get; set; }

        public virtual string RegistrationType2 { get; set; }

        public virtual string RegistrationOtherName1 { get; set; }

        public virtual string RegistrationOtherName2 { get; set; }

        public virtual string ComplaintRef { get; set; }

        public virtual string ComplaintRecordType { get; set; }

        public virtual string ComplaintSource { get; set; }

        public virtual string ActivityConcern { get; set; }

        public virtual string OtherActivityConcern { get; set; }

        public virtual string NonComplianceNatureResult { get; set; }

        public virtual DateTime? ComplaintDate { get; set; }

        public virtual DateTime? FirstComplaintDate { get; set; }

        public virtual DateTime? ReplyDueDate { get; set; }

        public virtual string SwdUnit { get; set; }

        public virtual DateTime? LfpsReceiveDate { get; set; }

        public virtual string ConcernedOrgName { get; set; }

        public virtual string ComplainantName { get; set; }

        public virtual string DcLcContent { get; set; }

        public virtual string ComplaintEnclosureNum { get; set; }

        public virtual string ProcessStatus { get; set; }

        public virtual string ActionFileEnclosureNum { get; set; }

        public virtual string FundRaisingLocation { get; set; }

        public virtual string WithholdingRemark { get; set; }

        public virtual DateTime? WithholdingBeginDate { get; set; }

        public virtual DateTime? WithholdingEndDate { get; set; }

        public virtual bool? WithholdingListIndicator { get; set; }

        public virtual int TelRecordNum { get; set; }

        public virtual int FollowUpActionRecordNum { get; set; }

        public virtual int PoliceCaseNum { get; set; }

        public virtual int OtherDepartmentEnquiryNum { get; set; }

        public virtual bool PoliceCaseIndicator { get; set; }

        public virtual string PoliceCaseResult { get; set; }

        public virtual string FollowUpAction { get; set; }

        public virtual string PspPermitNum { get; set; }

        public virtual string FdPermitNum { get; set; }

        public virtual string ComplaintResultRemark { get; set; }

        public virtual string OtherWithholdingRemark { get; set; }

        public virtual DateTime? FundRaisingDate { get; set; }

        public virtual bool FollowUpIndicator { get; set; }

        public virtual bool ReportPoliceIndicator { get; set; }

        public virtual bool OtherFollowUpIndicator { get; set; }

        public virtual bool OrgRefIndicator { get; set; }

        public virtual IList<ComplaintResult> ComplaintResult { get; set; }

        public virtual IList<ComplaintAttachment> ComplaintAttachment { get; set; }

        public virtual IList<ComplaintTelRecord> ComplaintTelRecord { get; set; }

        public virtual IList<ComplaintFollowUpAction> ComplaintFollowUpAction { get; set; }

        public virtual IList<ComplaintPoliceCase> ComplaintPoliceCase { get; set; }

        public virtual IList<ComplaintOtherDepartmentEnquiry> ComplaintOtherDepartmentEnquiry { get; set; }

        public override int Id
        {
            get
            {
                return ComplaintMasterId;
            }
            set
            {
                ComplaintMasterId = value;
            }
        }
    }
}