using AutoMapper;
using Psps.Models.Domain;
using Psps.Models.Dto.Accounts;
using Psps.Web.ViewModels.Account;

namespace Psps.Web.Mappings
{
    public class AccountMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "AccountMappingProfile"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<LoginViewModel, ChangePasswordViewModel>();
        }
    }
}