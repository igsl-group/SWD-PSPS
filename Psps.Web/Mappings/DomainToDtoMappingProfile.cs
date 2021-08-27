using AutoMapper;
using Psps.Models.Domain;
using Psps.Models.Dto.Accounts;
using Psps.Models.Dto.Disaster;
using Psps.Models.Dto.Complaint;

namespace Psps.Web.Mappings
{
    public class DomainToDtoMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToDtoMappingProfile"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<User, UserInfoDto>();
            Mapper.CreateMap<DisasterMaster, DisasterInfoDto>();
            Mapper.CreateMap<ComplaintMaster, ComplaintMasterDto>();
            
        }
    }
}