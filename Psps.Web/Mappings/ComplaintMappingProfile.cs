using AutoMapper;
using Psps.Models.Domain;
using Psps.Models.Dto.Accounts;
using Psps.Models.Dto.Complaint;
using Psps.Models.Dto.Disaster;

namespace Psps.Web.Mappings
{
    public class ComplaintMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ComplaintMappingProfile"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<ComplaintMaster, ComplaintMaster>();
        }
    }
}