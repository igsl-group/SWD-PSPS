using AutoMapper;
using Psps.Models.Dto.Accounts;
using Psps.Models.Dto.Disaster;
using Psps.Web.ViewModels.Account;
using Psps.Web.ViewModels.Disaster;

namespace Psps.Web.Mappings
{
    public class ViewModelToDtoMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDtoMappingProfile"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<UserViewModel, UserInfoDto>();
            Mapper.CreateMap<DisasterViewModel, DisasterInfoDto>();
            //Mapper.CreateMap<UpdateUserViewModel, UserInfoDto>();
        }
    }
}