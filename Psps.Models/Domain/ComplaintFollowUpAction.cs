using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class ComplaintFollowUpAction : BaseAuditEntity<int>
    {
        public virtual int ComplaintFollowUpActionId { get; set; }

        public virtual ComplaintMaster ComplaintMaster { get; set; }

        public virtual string EnclosureNum { get; set; }

        public virtual bool ReportPoliceIndicator { get; set; }

        public virtual DateTime? VerbalReportDate { get; set; }

        public virtual string PoliceStation { get; set; }

        public virtual string PoliceOfficerName { get; set; }

        public virtual string RnNum { get; set; }

        public virtual string RnRemark { get; set; }

        public virtual string RnRemarkHtml { get; set; }

        public virtual DateTime? WrittenReferralDate { get; set; }

        public virtual string ReferralPoliceStation { get; set; }

        public virtual string ActionFileRefEnclosureNum { get; set; }

        public virtual string ActionFileRefPartNum { get; set; }

        //
        public virtual string PoliceInvestigation { get; set; }

        public virtual string PoliceInvestigationResult { get; set; }

        public virtual string PoliceInvestigationResultHtml { get; set; }

        public virtual DateTime? PoliceReplyDate { get; set; }

        public virtual string ConvictedPersonName { get; set; }

        public virtual string ConvictedPersonPosition { get; set; }

        public virtual string OffenceDetail { get; set; }

        public virtual string OffenceDetailHtml { get; set; }

        public virtual string SentenceDetail { get; set; }

        public virtual string SentenceDetailHtml { get; set; }

        public virtual string CourtRefNum { get; set; }

        public virtual DateTime? CourtHearingDate { get; set; }

        public virtual string PoliceRemark { get; set; }

        public virtual string PoliceRemarkHtml { get; set; }

        public virtual string ReferralPoliceOfficerName { get; set; }

        public virtual string ReferralPoliceOfficerPosition { get; set; }

        //
        public virtual bool FollowUpIndicator { get; set; }

        public virtual string ContactOrgName { get; set; }

        public virtual string ContactPersonName { get; set; }

        public virtual DateTime? ContactDate { get; set; }

        public virtual string OrgRemark { get; set; }

        public virtual string OrgRemarkHtml { get; set; }

        public virtual string FollowUpLetterType { get; set; }

        public virtual string FollowUpLetterReceiver { get; set; }

        public virtual DateTime? FollowUpLetterIssueDate { get; set; }

        public virtual string FollowUpLetterRemark { get; set; }

        public virtual string FollowUpLetterRemarkHtml { get; set; }

        public virtual string FollowUpLetterActionFileRefEnclosureNum { get; set; }

        public virtual string FollowUpLetterActionFileRefPartNum { get; set; }

        public virtual string FollowUpLetterActionFileRefRemark { get; set; }

        public virtual string FollowUpLetterActionFileRefRemarkHtml { get; set; }

        public virtual string FollowUpOrgReply { get; set; }

        public virtual string FollowUpOrgReplyHtml { get; set; }

        public virtual DateTime? FollowUpOrgReplyDate { get; set; }

        public virtual string FollowUpOfficerName { get; set; }

        public virtual string FollowUpOfficerPosition { get; set; }

        public virtual bool OtherFollowUpIndicator { get; set; }

        public virtual string OtherFollowUpPartyName { get; set; }

        public virtual DateTime? OtherFollowUpContactDate { get; set; }

        public virtual string OtherFollowUpRemark { get; set; }

        public virtual string OtherFollowUpRemarkHtml { get; set; }

        public virtual string OtherFollowUpFileRefEnclosureNum { get; set; }

        public virtual string OtherFollowUpFileRefPartNum { get; set; }

        public virtual string OtherFollowUpOfficerName { get; set; }

        public virtual string OtherFollowUpOfficerPosition { get; set; }

        public virtual DisasterMaster DisasterMaster { get; set; }

        public override int Id
        {
            get
            {
                return ComplaintFollowUpActionId;
            }
            set
            {
                ComplaintFollowUpActionId = value;
            }
        }
    }
}