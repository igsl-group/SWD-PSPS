using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Models.Dto.OGCIO;
using Psps.Services.SystemParameters;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.OGCIO
{
    public class OrganisationApi : BaseApi, IOrganisationApi
    {
        private readonly ICacheManager _cacheManager;
        private readonly IParameterService _parameterService;

        public OrganisationApi(ICacheManager cacheManager, IParameterService parameterService)
        {
            _cacheManager = cacheManager;
            _parameterService = parameterService;

            _baseUrls = this._cacheManager.Get(string.Format(Constant.SYSTEMPARAMETER_BY_CODE_KEY, "FrasUrl"), () =>
            {
                return _parameterService.GetParameterByCode(Constant.SystemParameter.FRAS_URL).Value.Split(',');
            });
        }

        /*
        public ListOrganisationResult List(int page = 1)
        {
            var request = new RestRequest();
            request.Method = Method.GET;
            request.Resource = "swd/organisation/?page={page}";
            request.AddParameter("page", page, ParameterType.UrlSegment);

            return Get<ListOrganisationResult>(request);
        }
        */

        public Result Create(Organisation organisation)
        {
            var request = new RestRequest();
            request.Method = Method.POST;
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new RestSharpJsonNetSerializer();
            request.Resource = "swd/organisation";
            request.AddBody(organisation);

            return Execute(request);
        }

        public Result Update(Organisation organisation)
        {
            var request = new RestRequest();
            request.Method = Method.PUT;
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new RestSharpJsonNetSerializer();
            request.Resource = "swd/organisation";
            request.AddBody(organisation);

            return Execute(request);
        }

        public Result Delete(long organisationId)
        {
            var request = new RestRequest();
            request.Method = Method.DELETE;
            request.RequestFormat = DataFormat.Json;
            request.Resource = "swd/organisation/{organisationId}/";
            request.AddParameter("organisationId", organisationId, ParameterType.UrlSegment);

            return Execute(request);
        }
    }
}