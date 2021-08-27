using AutoMapper;
using Psps.Models.Dto.Accounts;
using Psps.Models.Dto.Disaster;
using Psps.Web.ViewModels.Account;
using Psps.Web.ViewModels.Disaster;

namespace Psps.Web.Mappings
{
    public class DtoToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DtoToViewModelMappingProfile"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<UserInfoDto, UserViewModel>();
            Mapper.CreateMap<DisasterInfoDto, DisasterViewModel>();
        }
    }
}