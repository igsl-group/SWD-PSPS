using System;

namespace Psps.Services.OGCIO
{
    public interface IOrganisationApi : IBaseApi
    {
        Psps.Models.Dto.OGCIO.Result Create(Psps.Models.Dto.OGCIO.Organisation organisation);

        Psps.Models.Dto.OGCIO.Result Delete(long organisationId);

        Psps.Models.Dto.OGCIO.Result Update(Psps.Models.Dto.OGCIO.Organisation organisation);
    }
}