using Psps.Models.Domain;
using Psps.Models.Dto.Organisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Data.Mappings
{
    public partial class OrgAfsTrViewMap : BaseEntityMap<OrgAfsTrView, string>
    {
        protected override void MapId()
        {
            Id(x => x.FileRef).Column("FileRef");
        }

        protected override void MapEntity()
        {
            Map(x => x.OrgId).Column("OrgId");
            Map(x => x.OrgRef).Column("OrgRef");
            Map(x => x.PermitType).Column("PermitType");
            Map(x => x.AfsRecordStartDate).Column("AfsRecordStartDate");
            Map(x => x.AfsRecordEndDate).Column("AfsRecordEndDate");
            Map(x => x.AfsRecordDetails).Column("AfsRecordDetails");
            Map(x => x.TrackRecordStartDate).Column("TrackRecordStartDate");
            Map(x => x.TrackRecordEndDate).Column("TrackRecordEndDate");
            Map(x => x.TrackRecordDetails).Column("TrackRecordDetails");
            Map(x => x.RecordKey).Column("RecordKey");
            Map(x => x.CreatedOn).Column("CreatedOn");
            Map(x => x.ApplicationReceiveDate).Column("ApplicationReceiveDate");
        }
    }
}