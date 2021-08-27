using AutoMapper;
using Psps.Models.Domain;
using Psps.Models.Dto.Accounts;
using Psps.Models.Dto.Complaint;
using Psps.Models.Dto.Disaster;

namespace Psps.Web.Mappings
{
    public class FlagDayMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "FlagDayMappingProfile"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<FdMaster, FdMaster>()
                .ForMember(d => d.ReferenceGuideSearchView, o => o.Ignore());
        }
    }
}