using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Models.Domain
{
    public partial class ComplaintOtherDepartmentEnquiry : BaseAuditEntity<int>
    {
        public virtual int ComplaintOtherDeptEnquiryId { get; set; }

        public virtual ComplaintMaster ComplaintMaster { get; set; }

        public virtual string RefNum { get; set; }

        public virtual DateTime? ReferralDate { get; set; }

        public virtual DateTime? MemoDate { get; set; }

        public virtual DateTime? MemoFromPoliceDate { get; set; }

        public virtual string EnquiryDepartment { get; set; }

        public virtual string OtherEnquiryDepartment { get; set; }

        public virtual string OrgInvolved { get; set; }

        public virtual string EnquiryContent { get; set; }

        public virtual string EnquiryContentHtml { get; set; }

        public virtual string EnclosureNum { get; set; }

        public virtual string Remark { get; set; }

        public virtual string RemarkHtml { get; set; }

        public override int Id
        {
            get
            {
                return ComplaintOtherDeptEnquiryId;
            }
            set
            {
                ComplaintOtherDeptEnquiryId = value;
            }
        }
    }
}
