using Autofac;
using Autofac.Core;
using Autofac.Extras.DynamicProxy2;
using Autofac.Integration.Mvc;
using NHibernate;
using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Fakes;
using Psps.Core.Infrastructure;
using Psps.Core.Infrastructure.DependencyManagement;
using Psps.Data.Infrastructure;
using Psps.Data.Mappings;
using Psps.Data.Repositories;
using Psps.Services.Accounts;
using Psps.Services.Events;
using Psps.Services.Lookups;
using Psps.Services.OGCIO;
using Psps.Services.SystemMessages;
using Psps.Services.SystemParameters;
using Psps.Web.Core.Authentication;
using Psps.Web.Core.Autofac;
using System;
using System.Linq;
using System.Web;

namespace Psps.Web.Framework
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get { return 0; }
        }

        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.Register(c =>
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerLifetimeScope();

            //Register logger
            builder.RegisterModule(new LoggingModule());

            //Register cache manager
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("psps_cache_static").SingleInstance();
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("psps_cache_per_request").InstancePerLifetimeScope();

            //Register controllers
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

            //Register model binders
            builder.RegisterModelBinders(typeFinder.GetAssemblies().ToArray());
            builder.RegisterModelBinderProvider();

            //Register data layer
            builder.Register(c => Psps.Data.Infrastructure.ConnectionHelper.BuildSessionFactory("DefaultConnection")).As<ISessionFactory>().SingleInstance();
            builder.Register(c => c.Resolve<ISessionFactory>().OpenSession())
                .InstancePerLifetimeScope()
                .OnActivated(activatedArgs =>
                {
                    var session = activatedArgs.Instance;
                    session.EnableFilter(DeletedFilter.Name);
                });
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            //Register repositories
            builder.RegisterAssemblyTypes(typeof(UserRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(CallLogger));

            //Register services & interface api
            builder.RegisterAssemblyTypes(typeof(UserService).Assembly)
                .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Api"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(CallLogger));

            builder.RegisterType<DefaultFormsAuthentication>().As<IFormsAuthentication>().InstancePerLifetimeScope();
            //builder.RegisterFilterProvider();

            //Register work context
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();

            //Register event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>)))
                    .InstancePerLifetimeScope();
            }
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();

            //we cache some service between requests
            builder.RegisterType<LookupService>().As<ILookupService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("psps_cache_static")).InstancePerLifetimeScope();
            builder.RegisterType<MessageService>().As<IMessageService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("psps_cache_static")).InstancePerLifetimeScope();
            builder.RegisterType<ParameterService>().As<IParameterService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("psps_cache_static")).InstancePerLifetimeScope();

            // register CallLogger
            var interceptor = new CallLogger();
            builder.RegisterInstance(interceptor);
        }
    }
}