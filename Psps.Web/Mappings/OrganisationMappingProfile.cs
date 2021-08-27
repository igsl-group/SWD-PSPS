using AutoMapper;
using Psps.Models.Domain;
using Psps.Models.Dto.Accounts;
using Psps.Models.Dto.Complaint;
using Psps.Models.Dto.Disaster;

namespace Psps.Web.Mappings
{
    public class OrganisationMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "OrganisationMappingProfile"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<OrgMaster, OrgMaster>()
                .ForMember(d => d.ReferenceGuideSearchViews, o => o.Ignore());
        }
    }
}