using Psps.Models.Dto.OGCIO;
using System;
using System.Collections.Generic;

namespace Psps.Services.OGCIO
{
    public interface IFundRaisingActivityApi : IBaseApi
    {
        List<Activity> List(int year, int month);

        Result Create(ActivitySendParam activity);

        Result Update(ActivitySendParam activity);

        Result Delete(string charityEventId);

        int LookupDistrictId(string districtId);
    }
}