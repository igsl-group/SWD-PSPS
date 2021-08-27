using AutoMapper;
using Psps.Models.Domain;
using Psps.Models.Dto.Accounts;
using Psps.Models.Dto.Complaint;
using Psps.Models.Dto.Disaster;

namespace Psps.Web.Mappings
{
    public class SuggestionMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "SuggestionMappingProfile"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<SuggestionMaster, SuggestionMaster>();
        }
    }
}