using Psps.Core.Common;
using Psps.Models.Domain;

namespace Psps.Data.Mappings
{
    public partial class UserMap : BaseAuditEntityMap<User, string>
    {
        protected override void MapId()
        {
            Id(x => x.UserId).GeneratedBy.Assigned().Column("UserId").Length(20);
        }

        protected override void MapEntity()
        {
            Map(x => x.EngUserName).Column("EngUserName").Not.Nullable().Length(50);
            Map(x => x.ChiUserName).Column("ChiUserName").Length(50);
            Map(x => x.Password).Column("Password").Not.Nullable().Length(40);
            Map(x => x.PrevPassword1).Column("PrevPassword1").Length(40);
            Map(x => x.PrevPassword2).Column("PrevPassword2").Length(40);
            Map(x => x.PrevPassword3).Column("PrevPassword3").Length(40);
            Map(x => x.PrevPassword4).Column("PrevPassword4").Length(40);
            Map(x => x.PrevPassword5).Column("PrevPassword5").Length(40);
            Map(x => x.PrevPassword6).Column("PrevPassword6").Length(40);
            Map(x => x.PrevPassword7).Column("PrevPassword7").Length(40);
            Map(x => x.PrevPassword8).Column("PrevPassword8").Length(40);
            Map(x => x.PasswordChangedDate).Column("PasswordChangedDate");
            Map(x => x.TelephoneNumber).Column("Tel").Length(15);
            Map(x => x.Email).Column("Email").Length(50);
            Map(x => x.IsActive).Column("IsActive").Not.Nullable();
            Map(x => x.IsSystemAdministrator).Column("IsSystemAdministrator").Not.Nullable();
            HasOne(x => x.Post).PropertyRef(x => x.Owner);
        }
    }
}