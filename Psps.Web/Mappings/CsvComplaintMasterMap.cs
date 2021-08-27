using CsvHelper.Configuration;
using Psps.Models.Domain;
using Psps.Models.Dto.Complaint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Psps.Web.Mappings
{
    public class CsvComplaintMasterMap : CsvClassMap<ComplaintMaster>
    {
        public CsvComplaintMasterMap()
        {

            Map(m => m.ComplaintRef).Name("Organisation Reference");
            Map(m => m.ComplaintRecordType).Name("Record Type");
            Map(m => m.ComplaintSource).Name("Source");
            Map(m => m.ActivityConcern).Name("Activity Concerned");
            Map(m => m.ComplaintDate).Name("Date of Complaint / Enquiry").TypeConverter<NullableDMYDateConverter>();
            Map(m => m.FirstComplaintDate).Name("Date of Complaint / Enquiry 1st Received by SWD").TypeConverter<NullableDMYDateConverter>();
            Map(m => m.ReplyDueDate).Name("Due Date to Reply").TypeConverter<NullableDMYDateConverter>();
            Map(m => m.LfpsReceiveDate).Name("Date of Complaint / Enquiry Received by LFPS").TypeConverter<NullableDMYDateConverter>();
            Map(m => m.SwdUnit).Name("Unit of SWD");
            References<CsvComplaintMasterOrgMasterMap>(m => m.OrgMaster);
            Map(m => m.ComplainantName).Name("Name of Complainant / Enquirer");
            //Map(m => m.GovernmentHotlineIndicator).Name("Referred from 1823");
            References<CsvComplaintMasterPspApprovalHistoryMap>(m => m.PspApprovalHistory);
            References<CsvComplaintMasterFdEventMap>(m => m.FdEvent);
            Map(m => m.DcLcContent).Name("Content (for DC/LC/Mass Media only)");
            //Map(m => m.NonComplianceNature1).Name("NonComplianceNature1");
            //Map(m => m.OtherNonComplianceNature1).Name("OtherNonComplianceNature1");
            //Map(m => m.NonComplianceNature1).Name("NonComplianceNature2");
            //Map(m => m.OtherNonComplianceNature1).Name("OtherNonComplianceNature2");
            //Map(m => m.NonComplianceNature1).Name("NonComplianceNature3");
            //Map(m => m.OtherNonComplianceNature1).Name("OtherNonComplianceNature3");
            Map(m => m.ComplaintEnclosureNum).Name("ComplaintEnclosureNum");
            Map(m => m.ProcessStatus).Name("Processing Status");
            Map(m => m.ActionFileEnclosureNum).Name("Enclosure No. in Action File");
            Map(m => m.RelatedComplaintMaster.ComplaintRef).Name("Related Complaint / Enquiry Case");
            //Map(m => m.ComplaintRemarks).Name("Remarks");
        }

        public class CsvComplaintMasterOrgMasterMap : CsvClassMap<OrgMaster>
        {
            public CsvComplaintMasterOrgMasterMap()
            {
                Map(m => m.OrgRef).Name("Organisation Concerned");
            }
        }

        public class CsvComplaintMasterPspApprovalHistoryMap : CsvClassMap<PspApprovalHistory>
        {
            public CsvComplaintMasterPspApprovalHistoryMap()
            {
                Map(m => m.PermitNum).Name("(PSP)Permit Concerned");
            }
        }

        public class CsvComplaintMasterFdEventMap : CsvClassMap<FdEvent>
        {
            public CsvComplaintMasterFdEventMap()
            {
                Map(m => m.PermitNum).Name("(FD)Permit Concerned");
            }
        }
    }
}