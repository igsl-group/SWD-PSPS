using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.OGCIO
{
    public class OGCIOHttpAuthenticator : IAuthenticator
    {
        private readonly string CertSubject;
        private readonly X509Store _x509Store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
        private readonly X509Certificate2 _x509Cert;
        private readonly string _authHeader;

        public OGCIOHttpAuthenticator()
        {
            var config = Psps.Core.Infrastructure.EngineContext.Current.Resolve<Psps.Core.Configuration.PspsConfig>();
            CertSubject = String.Format("CN={0}, OU=Social Welfare Department", config.FrasCert);

            _x509Store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            _x509Cert = _x509Store.Certificates.OfType<X509Certificate2>().FirstOrDefault(cert => cert.Subject.StartsWith(CertSubject));

            if (_x509Cert == null)
            {
                string message = "Certificate with subject - [" + CertSubject + "] was not found.  Make sure the certificate is installed.";
                throw new ApplicationException(message);
            }

            var today = DateTime.Today;
            var authText = string.Format("{0}{1}{2}dws", today.ToString("dd").Reverse(), today.ToString("MM").Reverse(), today.ToString("yyyy").Reverse());
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes("user:" + authText));
            _authHeader = string.Format("Basic {0}", token);
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            client.ClientCertificates = new X509CertificateCollection(new X509Certificate[] { _x509Cert });
            request.AddHeader("AUTH_TYPE", "REST_SSL_CLIENT_X509_CERTIFICATE");
            // only add the Authorization parameter if it hasn't been added by a previous Execute
            if (!request.Parameters.Any(p => p.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase)))
            {
                request.AddHeader("Authorization", _authHeader);
            }
        }
    }
}