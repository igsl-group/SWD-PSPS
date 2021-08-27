using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class ComplaintPoliceCase : BaseAuditEntity<int>
    {
        public virtual int ComplaintPoliceCaseId { get; set; }

        public virtual ComplaintMaster ComplaintMaster { get; set; }

        public virtual string CaseInvestigateRefNum { get; set; }

        public virtual DateTime? ReferralDate { get; set; }

        public virtual DateTime? MemoDate { get; set; }

        public virtual int OrgId { get; set; }

        public virtual OrgMaster OrgMaster { get; set; }

        public virtual string CorrespondenceEnclosureNum { get; set; }

        public virtual string CorrespondencePartNum { get; set; }

        public virtual string InvestigationResult { get; set; }

        public virtual string PoliceRefNum { get; set; }

        public virtual string CaseNature { get; set; }

        public virtual string CaseNatureHtml { get; set; }

        public virtual string ResultDetail { get; set; }

        public virtual string ResultDetailHtml { get; set; }

        public virtual string EnclosureNum { get; set; }

        public virtual string PartNum { get; set; }

        public virtual DateTime? FundRaisingDate { get; set; }

        public virtual string FundRaisingTime { get; set; }

        public virtual string FundRaisingLocation { get; set; }

        public virtual string ConvictedPersonName { get; set; }

        public virtual string ConvictedPersonPosition { get; set; }

        public virtual string OffenceDetail { get; set; }

        public virtual string OffenceDetailHtml { get; set; }

        public virtual string SentenceDetail { get; set; }

        public virtual string SentenceDetailHtml { get; set; }

        public virtual string CourtRefNum { get; set; }

        public virtual DateTime? CourtHearingDate { get; set; }

        public virtual string Remark { get; set; }

        public virtual string RemarkHtml { get; set; }

        public override int Id
        {
            get
            {
                return ComplaintPoliceCaseId;
            }
            set
            {
                ComplaintPoliceCaseId = value;
            }
        }
    }
}