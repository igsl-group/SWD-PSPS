using Autofac;
using Autofac.Core;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Caching;
using Psps.Core.Caching;
using Psps.Core.Infrastructure;
using Psps.Core.Infrastructure.DependencyManagement;
using Psps.Services.OGCIO;
using Psps.Web.Controllers;
using Psps.Web.Infrastructure.Cache;
using Psps.Web.Infrastructure.DI.Autofac.Modules;

namespace Psps.Web.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get { return 2; }
        }

        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //we cache presentation models between requests
            // e.g. builder.RegisterType<NewsController>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("psps_cache_static"));

            builder.RegisterType<ModelCacheEventConsumer>().AsImplementedInterfaces().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("psps_cache_static"));

            builder.RegisterType<AccountController>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("psps_cache_static"));
            builder.RegisterType<LookupController>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("psps_cache_static"));
            builder.RegisterType<PostController>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("psps_cache_static"));

            builder.RegisterType<OrganisationApi>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("psps_cache_static"));

            //register for MvcSiteMapProvider
            builder.RegisterModule(new MvcSiteMapProviderModule());
            builder.RegisterType<ReservedAttributeNameProvider>()
                .As<IReservedAttributeNameProvider>()
                .WithParameter("attributesToIgnore", new string[] { "cssClass", "functionId" });
            builder.RegisterType<SiteMapNodeVisibilityProviderStrategy>()
                .As<ISiteMapNodeVisibilityProviderStrategy>()
                .WithParameter("defaultProviderName", "MvcSiteMapProvider.FilteredSiteMapNodeVisibilityProvider, MvcSiteMapProvider");
            builder.RegisterType<SiteMapBuilderSet>()
               .Named<ISiteMapBuilderSet>("builderSet1")
               .WithParameter("instanceName", "default")
               .WithParameter("securityTrimmingEnabled", true)
               .WithParameter("enableLocalization", true)
               .WithParameter("visibilityAffectsDescendants", true)
               .WithParameter("useTitleIfDescriptionNotProvided", true)
               .WithParameter(
                    (p, c) => p.Name == "siteMapBuilder",
                    (p, c) => c.ResolveNamed<ISiteMapBuilder>("siteMapBuilder1"))
               .WithParameter(
                    (p, c) => p.Name == "cacheDetails",
                    (p, c) => c.ResolveNamed<ICacheDetails>("cacheDetails1"));
        }
    }
}