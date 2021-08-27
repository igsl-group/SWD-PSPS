using FluentValidation.Mvc;
using ModelMetadataExtensions;
using MvcSiteMapProvider.Loader;
using Psps.Core;
using Psps.Core.Infrastructure;
using Psps.Core.Models;
using Psps.Models.Dto.Security;
using Psps.Services.Accounts;
using Psps.Services.Security;
using Psps.Web.Controllers;
using Psps.Web.Core.Authentication;
using Psps.Web.Core.Mvc;
using Psps.Web.Core.Mvc.ModelBinders;
using Psps.Web.Framework.Mvc;
using Psps.Web.Infrastructure.DI;
using Psps.Web.Mappings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Psps.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            this.PostAuthenticateRequest += this.PostAuthenticateRequestHandler;
            base.Init();
        }

        protected void Application_Start()
        {
#if !DEBUG
            //Protect connection strings section
            ProtectConnectionStringsSection();
#endif
            //initialize engine _dataContext
            EngineContext.Initialize(false);

            //Registering some regular mvc stuff
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //custome ModelMetadataProviders
            ModelMetadataProviders.Current = new ConventionalModelMetadataProvider(
              requireConventionAttribute: false,
              defaultResourceType: typeof(Psps.Resources.Labels)
            );

            //custom model binder
            //This allows interfaces etc to be provided as parameters to action methods
            ModelBinders.Binders.DefaultBinder = new DiModelBinder();

            var dateTimeModelBinder = new DateTimeModelBinder(new string[] { "d/M/yyyy", "d/M/yyyy H:m:s" });
            ModelBinders.Binders.Add(typeof(DateTime), dateTimeModelBinder);
            ModelBinders.Binders.Add(typeof(DateTime?), dateTimeModelBinder);

            var decimalModelBinder = new DecimalModelBinder();
            ModelBinders.Binders.Add(typeof(decimal), decimalModelBinder);
            ModelBinders.Binders.Add(typeof(decimal?), decimalModelBinder);

            //fluent validation
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new ModelValidatorFactory()));

            //setup log4net
            var log4NetPath = Server.MapPath("~/log4net.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(log4NetPath));

            //setup automapper
            AutoMapperConfiguration.Configure();

            // Setup global sitemap loader (required)
            MvcSiteMapProvider.SiteMaps.Loader = EngineContext.Current.Resolve<ISiteMapLoader>();
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            // Remove any special filtering especially GZip filtering
            //Response.Filter = null;

            ShowCustomErrorPage(Server.GetLastError());
        }

        private bool IsValidAuthCookie(HttpCookie authCookie)
        {
            return authCookie != null && !String.IsNullOrEmpty(authCookie.Value);
        }

        private DateTime CalculateTicketExpirationDate()
        {
            return DateTime.Now.Add(FormsAuthentication.Timeout);
        }

        private FormsAuthenticationTicket UpdateAuthInfo(FormsAuthenticationTicket ticket, IPspsUser user, Psps.Models.Domain.User dbUser)
        {
            user.Name = dbUser.EngUserName;
            user.IsSysAdmin = dbUser.IsSystemAdministrator;

            return new FormsAuthenticationTicket(ticket.Version,
                                              ticket.Name,
                                              ticket.IssueDate,
                                              CalculateTicketExpirationDate(),
                                              ticket.IsPersistent,
                                              user.GetUserInfo().ToString(),
                                              ticket.CookiePath);
        }

        private void PostAuthenticateRequestHandler(object sender, EventArgs e)
        {
            HttpCookie authCookie = this.Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (IsValidAuthCookie(authCookie))
            {
                var formsAuthentication = EngineContext.Current.Resolve<IFormsAuthentication>();
                var aclService = EngineContext.Current.Resolve<IAclService>();

                FormsAuthenticationTicket ticket = formsAuthentication.Decrypt(authCookie.Value);
                IPspsUser user = new PspsUser(ticket.Name, ticket.UserData);
                Psps.Models.Domain.User dbUser = null;
                List<string> allowedFunctions = new List<string>();

                if (aclService.ValidateUserIdAndPostId(user.UserId, user.PostId, out dbUser))
                {
                    allowedFunctions = aclService.GetAllowedFunctionsByPost(user.PostId);
                    if (dbUser.IsSystemAdministrator)
                        allowedFunctions.Add(Psps.Web.Core.ActionFilters.Allow.SysAdmin.GetName());

                    var newTicket = UpdateAuthInfo(ticket, user, dbUser);
                    this.Context.User = new GenericPrincipal(user, allowedFunctions.ToArray());
                    formsAuthentication.SetAuthCookie(this.Context, newTicket);
                }
                else
                {
                    formsAuthentication.Signout();
                }
            }
        }

        private void ShowCustomErrorPage(Exception exception)
        {
            HttpException httpException = exception as HttpException;
            if (httpException == null)
                httpException = new HttpException(500, "Internal Server Error", exception);

            Response.Clear();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", "Index");
            routeData.Values.Add("exception", exception);
            routeData.Values.Add("fromAppErrorEvent", true);

            Server.ClearError();

            using (Controller controller = new ErrorController())
            {
                ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
            }
        }

        private void ProtectConnectionStringsSection()
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");

            ConfigurationSection section = config.GetSection("connectionStrings");

            if (section != null && !section.SectionInformation.IsProtected)
            {
                section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                config.Save();
            }
        }
    }
}