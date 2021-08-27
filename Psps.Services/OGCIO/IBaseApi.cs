using Psps.Models.Dto.OGCIO;
using System;

namespace Psps.Services.OGCIO
{
    public interface IBaseApi
    {
        Result Execute(RestSharp.RestRequest request);

        T Get<T>(RestSharp.RestRequest request) where T : new();
    }
}