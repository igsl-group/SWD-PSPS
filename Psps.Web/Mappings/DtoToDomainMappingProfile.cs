using AutoMapper;
using Psps.Models.Domain;
using Psps.Models.Dto.Accounts;
using Psps.Models.Dto.Disaster;

namespace Psps.Web.Mappings
{
    public class DtoToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DtoToDomainMappingProfile"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<UserInfoDto, User>()
                .ForMember(dest => dest.Password, opt => opt.Condition(src => !src.IsSourceValueNull));
            Mapper.CreateMap<DisasterInfoDto, DisasterMaster>();
        }
    }
}