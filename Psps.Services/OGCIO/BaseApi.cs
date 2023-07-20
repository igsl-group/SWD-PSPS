using log4net;
using Newtonsoft.Json;
using Psps.Models.Dto.OGCIO;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.OGCIO
{
    public abstract class BaseApi : IBaseApi
    {
        protected Boolean randomized = false;

        protected string[] _baseUrls;

        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private void RandomUrl()
        {
            if (!randomized)
            {
                Random rnd = new Random();
                _baseUrls = _baseUrls.OrderBy(x => rnd.Next()).ToArray();
                randomized = true;
            }
        }

        public T Get<T>(RestRequest request) where T : new()
        {
            var exceptions = new List<Exception>();

            RandomUrl();

            for (var i = 0; i < _baseUrls.Length; i++)
            {
                try
                {
                    InitServicePointManager();
                    var client = new RestClient();
                    client.BaseUrl = new Uri(_baseUrls[i]);
                    client.Authenticator = new OGCIOHttpAuthenticator();
                    request.JsonSerializer = new RestSharpJsonNetSerializer();

                    var response = client.Execute<T>(request);
                    if (response.ErrorException != null)
                    {
                        const string message = "Error retrieving response. Check inner details for more info.";
                        throw new ApplicationException(message, response.ErrorException);
                    }
                    return response.Data;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(exceptions);
        }

        public Result Execute(RestRequest request)
        {
            var exceptions = new List<Exception>();

            RandomUrl();

            for (var i = 0; i < _baseUrls.Length; i++)
            {
                try
                {
                    InitServicePointManager();
                    var client = new RestClient();
                    client.BaseUrl = new Uri(_baseUrls[i]);
                    client.Authenticator = new OGCIOHttpAuthenticator();
                    _logger.Info(client.BaseUrl);
                    _logger.Info(request.Parameters);
                    _logger.Info("82");

                    var response = client.Execute(request);
                    _logger.Info(response.StatusCode);
                    _logger.Info(response.Content);
                    _logger.Info(response.ErrorMessage);
                    _logger.Info("87");

                    if (response.ErrorException != null)
                    {
                        const string message = "Error retrieving response. Check inner details for more info.";
                        var charityActivitiesException = new ApplicationException(message, response.ErrorException);
                        throw charityActivitiesException;
                    }

                    Result result = null;

                    if (new[] { System.Net.HttpStatusCode.OK, System.Net.HttpStatusCode.Created }.Contains(response.StatusCode))
                        result = new Result { StatusCode = (int)response.StatusCode, Content = response.Content };
                    else if (new[] { System.Net.HttpStatusCode.BadRequest, System.Net.HttpStatusCode.InternalServerError }.Contains(response.StatusCode))
                    {
                        result = JsonConvert.DeserializeObject<Result>(response.Content);
                        result.Content = string.Join(Environment.NewLine, result.ErrorList.ToArray());
                        result.StatusCode = (int)response.StatusCode;
                    }
                    else
                    {
                        throw new ApplicationException(String.Format("OGCIO FRAS API ERROR - Please contact technical support! {0} - {1}", (int)response.StatusCode, response.StatusDescription));
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            //throw exceptions.Last();
            throw new AggregateException(exceptions);
        }

        private void InitServicePointManager()
        {
            // trust all certificates
            ServicePointManager.ServerCertificateValidationCallback = ((sender, cert, chain, sslPolicyErrors) => true);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            /*// trust sender
            ServicePointManager.ServerCertificateValidationCallback = ((sender, cert, chain, errors) => true);

            // validate cert by calling a function
            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback((sender, cert, chain, sslPolicyErrors) => true);*/
        }
    }
}