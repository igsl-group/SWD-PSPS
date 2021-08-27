using Newtonsoft.Json;
using Psps.Core.Common;
using Psps.Core.Infrastructure;
using Psps.Models.Dto.Lookups;
using Psps.Models.Dto.OGCIO;
using Psps.Services.Lookups;
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
    public class FundRaisingActivityApi : BaseApi, IFundRaisingActivityApi
    {
        private readonly IParameterService _parameterService;
        private readonly ILookupService _lookupService;

        public FundRaisingActivityApi(IParameterService parameterService, ILookupService lookupService)
        {
            _parameterService = parameterService;
            _lookupService = lookupService;

            _baseUrls = _parameterService.GetParameterByCode(Constant.SystemParameter.FRAS_URL).Value.Split(',');
        }

        public List<Activity> List(int year, int month)
        {
            var request = new RestRequest();
            request.Method = Method.GET;
            request.Resource = "swd/activity/?year={year}&month={month}";
            request.AddParameter("year", year, ParameterType.UrlSegment);
            request.AddParameter("month", month, ParameterType.UrlSegment);

            return Get<List<Activity>>(request);
        }

        public Result Create(ActivitySendParam activity)
        {
            //TODO: Remove in production
            var config = _parameterService.GetParameterByCode(Constant.SystemParameter.FRAS_ENABLED);

            if (config != null && "Y".Equals(config.Value, StringComparison.CurrentCultureIgnoreCase))
            {
                var request = new RestRequest();
                request.Method = Method.POST;
                request.RequestFormat = DataFormat.Json;
                request.JsonSerializer = new RestSharpJsonNetSerializer();
                request.Resource = "swd/activity";
                request.AddBody(activity);

                return Execute(request);
            }
            else
            {
                return new Result { StatusCode = 404, Content = "Fras API disabled", ErrorList = new List<string>() { "Fras API disabled" } };
            }
        }

        public Result Update(ActivitySendParam activity)
        {
            //TODO: Remove in production
            var config = _parameterService.GetParameterByCode(Constant.SystemParameter.FRAS_ENABLED);

            if (config != null && "Y".Equals(config.Value, StringComparison.CurrentCultureIgnoreCase))
            {
                var request = new RestRequest();
                request.Method = Method.PUT;
                request.RequestFormat = DataFormat.Json;
                request.JsonSerializer = new RestSharpJsonNetSerializer();
                request.Resource = "swd/activity";
                request.AddBody(activity);

                return Execute(request);
            }
            else
            {
                return new Result { StatusCode = 404, Content = "Fras API disabled", ErrorList = new List<string>() { "Fras API disabled" } };
            }
        }

        public Result Delete(string charityEventId)
        {
            //TODO: Remove in production
            var config = _parameterService.GetParameterByCode(Constant.SystemParameter.FRAS_ENABLED);

            if (config != null && "Y".Equals(config.Value, StringComparison.CurrentCultureIgnoreCase))
            {
                var request = new RestRequest();
                request.Method = Method.DELETE;
                request.Resource = "swd/activity/{charityEventId}/";
                request.AddParameter("charityEventId", charityEventId, ParameterType.UrlSegment);

                return Execute(request);
            }
            else
            {
                return new Result { StatusCode = 404, Content = "Fras API disabled", ErrorList = new List<string>() { "Fras API disabled" } };
            }
        }

        public int LookupDistrictId(string districtId)
        {
            var frasDistrict = _lookupService.GetAllLookupListByType(LookupType.FrasDistrict);

            return Convert.ToInt32(frasDistrict.Single(u => u.Code == districtId).EngDescription);
        }
    }
}