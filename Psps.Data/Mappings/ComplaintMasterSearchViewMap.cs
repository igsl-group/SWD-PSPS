using FluentNHibernate.Mapping;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psps.Data.Mappings
{
    public partial class ComplaintMasterSearchViewMap : BaseEntityMap<ComplaintMasterSearchView, int>
    {
        protected override void MapId()
        {
            Id(x => x.ComplaintMasterId).GeneratedBy.Identity().Column("ComplaintMasterId");
        }

        protected override void MapEntity()
        {
            Map(x => x.OrgRef).Column("OrgRef");
            Map(x => x.PspRef).Column("PspRef");
            Map(x => x.FdRef).Column("FdRef");
            Map(x => x.EngOrgNameSorting).Column("EngOrgNameSorting");
            Map(x => x.CmOrgNameSorting).Column("CmOrgNameSorting");
            Map(x => x.EngOrgName).Column("EngOrgName");
            Map(x => x.ChiOrgName).Column("ChiOrgName");           
            Map(x => x.DisableIndicator).Column("DisableIndicator");
            Map(x => x.SubventedIndicator).Column("SubventedIndicator");
            Map(x => x.RegistrationType1).Column("RegistrationType1");
            Map(x => x.RegistrationType2).Column("RegistrationType2");
            Map(x => x.RegistrationOtherName1).Column("RegistrationOtherName1");
            Map(x => x.RegistrationOtherName2).Column("RegistrationOtherName2");
            Map(x => x.ComplaintRef).Column("ComplaintRef");
            Map(x => x.ComplaintRecordType).Column("ComplaintRecordType");
            Map(x => x.ComplaintSource).Column("ComplaintSource");
            Map(x => x.ActivityConcern).Column("ActivityConcern");
            Map(x => x.OtherActivityConcern).Column("OtherActivityConcern");
            Map(x => x.ComplaintDate).Column("ComplaintDate");
            Map(x => x.FirstComplaintDate).Column("FirstComplaintDate");
            Map(x => x.ReplyDueDate).Column("ReplyDueDate");
            Map(x => x.SwdUnit).Column("SwdUnit");
            Map(x => x.LfpsReceiveDate).Column("LfpsReceiveDate");
            Map(x => x.ConcernedOrgName).Column("ConcernedOrgName");
            Map(x => x.ComplainantName).Column("ComplainantName");
            Map(x => x.DcLcContent).Column("DcLcContent");
            Map(x => x.ComplaintEnclosureNum).Column("ComplaintEnclosureNum");
            Map(x => x.ProcessStatus).Column("ProcessStatus");
            Map(x => x.ActionFileEnclosureNum).Column("ActionFileEnclosureNum");
            Map(x => x.FundRaisingLocation).Column("FundRaisingLocation");
            Map(x => x.WithholdingRemark).Column("WithholdingRemark");
            Map(x => x.WithholdingBeginDate).Column("WithholdingBeginDate");
            Map(x => x.WithholdingEndDate).Column("WithholdingEndDate");
            Map(x => x.WithholdingListIndicator).Column("WithholdingListIndicator");
            Map(x => x.TelRecordNum).Column("TelRecordNum");
            Map(x => x.FollowUpActionRecordNum).Column("FollowUpActionRecordNum");
            Map(x => x.PoliceCaseNum).Column("PoliceCaseNum");
            Map(x => x.OtherDepartmentEnquiryNum).Column("OtherDepartmentEnquiryNum");
            Map(x => x.PoliceCaseIndicator).Column("PoliceCaseIndicator");
            Map(x => x.PoliceCaseResult).Column("PoliceCaseResult");
            Map(x => x.FollowUpAction).Column("FollowUpAction");
            Map(x => x.PspPermitNum).Column("PspPermitNum");
            Map(x => x.FdPermitNum).Column("FdPermitNum");
            Map(x => x.ComplaintResultRemark).Column("ComplaintResultRemark");
            Map(x => x.OtherWithholdingRemark).Column("OtherWithholdingRemark");
            Map(x => x.FundRaisingDate).Column("FundRaisingDate");
            Map(x => x.FollowUpIndicator).Column("FollowUpIndicator");
            Map(x => x.ReportPoliceIndicator).Column("ReportPoliceIndicator");
            Map(x => x.OtherFollowUpIndicator).Column("OtherFollowUpIndicator");
            Map(x => x.OrgRefIndicator).Formula("CASE WHEN OrgRef IS NULL THEN 1 ELSE 0 END");
            Map(x => x.NonComplianceNatureResult).Column("NonComplianceNatureResult");

            HasMany(x => x.ComplaintResult).KeyColumn("ComplaintMasterId").Inverse();
            HasMany(x => x.ComplaintTelRecord).KeyColumn("ComplaintMasterId").Inverse();
            HasMany(x => x.ComplaintAttachment).KeyColumn("ComplaintMasterId").Inverse();
            HasMany(x => x.ComplaintFollowUpAction).KeyColumn("ComplaintMasterId").Inverse();
            HasMany(x => x.ComplaintPoliceCase).KeyColumn("ComplaintMasterId").Inverse();
            HasMany(x => x.ComplaintOtherDepartmentEnquiry).KeyColumn("ComplaintMasterId").Inverse();
        }
    }
}