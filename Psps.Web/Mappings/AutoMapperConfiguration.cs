using AutoMapper;

namespace Psps.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DtoToViewModelMappingProfile>();
                x.AddProfile<ViewModelToDtoMappingProfile>();
                x.AddProfile<DtoToDomainMappingProfile>();
                x.AddProfile<DomainToDtoMappingProfile>();
                x.AddProfile<OrganisationMappingProfile>();
                x.AddProfile<FlagDayMappingProfile>();
                x.AddProfile<PSPMappingProfile>();
                x.AddProfile<ComplaintMappingProfile>();
                x.AddProfile<SuggestionMappingProfile>();
                x.AddProfile<AccountMappingProfile>();
            });
        }
    }
}