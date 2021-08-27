using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class ComplaintMaster : BaseAuditEntity<int>
    {
        public ComplaintMaster()
        {
            ComplaintAttachment = new List<ComplaintAttachment>();
            ComplaintTelRecord = new List<ComplaintTelRecord>();
            ComplaintFollowUpAction = new List<ComplaintFollowUpAction>();
            ComplaintPoliceCase = new List<ComplaintPoliceCase>();
            ComplaintOtherDepartmentEnquiry = new List<ComplaintOtherDepartmentEnquiry>();
        }

        public virtual int ComplaintMasterId { get; set; }

        public virtual PspApprovalHistory PspApprovalHistory { get; set; }

        public virtual FdEvent FdEvent { get; set; }

        public virtual OrgMaster OrgMaster { get; set; }

        public virtual ComplaintMaster RelatedComplaintMaster { get; set; }

        public virtual string ComplaintRef { get; set; }

        public virtual string ComplaintRecordType { get; set; }

        public virtual string ComplaintSource { get; set; }

        public virtual string ComplaintSourceRemark { get; set; }

        public virtual string ActivityConcern { get; set; }

        public virtual string OtherActivityConcern { get; set; }

        public virtual DateTime? ComplaintDate { get; set; }

        public virtual DateTime? FirstComplaintDate { get; set; }

        public virtual DateTime? ReplyDueDate { get; set; }

        public virtual string SwdUnit { get; set; }

        public virtual DateTime? LfpsReceiveDate { get; set; }

        public virtual string ConcernedOrgName { get; set; }

        public virtual string ComplainantName { get; set; }

        public virtual string DcLcContent { get; set; }

        public virtual string DcLcContentHtml { get; set; }

        public virtual string NonComplianceNature { get; set; }

        public virtual string OtherNonComplianceNature { get; set; }

        public virtual string ComplaintPartNum { get; set; }

        public virtual string ComplaintEnclosureNum { get; set; }

        public virtual string ProcessStatus { get; set; }

        public virtual bool WithholdingListIndicator { get; set; }

        public virtual DateTime? WithholdingBeginDate { get; set; }

        public virtual DateTime? WithholdingEndDate { get; set; }

        public virtual string ActionFilePartNum { get; set; }

        public virtual string ActionFileEnclosureNum { get; set; }

        public virtual IList<ComplaintAttachment> ComplaintAttachment { get; set; }

        public virtual IList<ComplaintTelRecord> ComplaintTelRecord { get; set; }

        public virtual IList<ComplaintFollowUpAction> ComplaintFollowUpAction { get; set; }

        public virtual IList<ComplaintPoliceCase> ComplaintPoliceCase { get; set; }

        public virtual IList<ComplaintOtherDepartmentEnquiry> ComplaintOtherDepartmentEnquiry { get; set; }

        public virtual DateTime? FundRaisingDate { get; set; }

        public virtual decimal? FundRaiserInvolve { get; set; }

        public virtual string FundRaisingTime { get; set; }

        public virtual string FundRaisingLocation { get; set; }

        public virtual string CollectionMethod { get; set; }

        public virtual string OtherCollectionMethod { get; set; }

        public virtual string ComplaintResult { get; set; }

        public virtual string ComplaintResultRemark { get; set; }

        public virtual string ComplaintResultRemarkHtml { get; set; }

        public virtual string WithholdingRemark { get; set; }

        public virtual string OtherWithholdingRemark { get; set; }

        public virtual string OtherWithholdingRemarkHtml { get; set; }

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