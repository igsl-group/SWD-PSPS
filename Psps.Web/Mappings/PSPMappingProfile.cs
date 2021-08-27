using AutoMapper;
using Psps.Models.Domain;
using Psps.Models.Dto.Accounts;
using Psps.Models.Dto.Complaint;
using Psps.Models.Dto.Disaster;

namespace Psps.Web.Mappings
{
    public class PSPMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "PSPMappingProfile"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<PspMaster, PspMaster>()
                .ForMember(d => d.ReferenceGuideSearchView, o => o.Ignore());
        }
    }
}