using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Context;
using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Fakes;
using Psps.Core.Infrastructure;
using Psps.Core.Infrastructure.DependencyManagement;
using Psps.Data.DB.Interceptors;
using Psps.Data.DB.Listeners;
using Psps.Data.Infrastructure;
using Psps.Data.Mappings;
using Psps.Data.Repositories;
using Psps.Services.Accounts;
using Psps.Services.Events;
using Psps.Web.Core.Authentication;
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
            //builder.RegisterModule(new LogRequestModule());

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
            //builder.RegisterModule(new NLogLoggerAutofacModule());

            //Register cache manager
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("psps_cache_static").SingleInstance();
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("psps_cache_per_request").InstancePerLifetimeScope();

            //Register data layer
            builder.Register(c => Psps.Data.Infrastructure.ConnectionHelper.BuildSessionFactory(
                Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(x => x.FromConnectionStringWithKey("DefaultConnection")))
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<UserMap>()
                        .Conventions.AddFromAssemblyOf<ManyToManyIdNameConvention>())
                .ExposeConfiguration(cfg =>
                {
                    cfg.Interceptor = new SqlStatementInterceptor(cfg.Interceptor ?? new EmptyInterceptor());
                    cfg.SetProperty(NHibernate.Cfg.Environment.CurrentSessionContextClass, "thread_static");
                    new AuditFlushEntityEventListener().Register(cfg);
                })
            )).As<ISessionFactory>().SingleInstance();
            builder.Register(c => c.Resolve<ISessionFactory>().OpenSession())
                .OnActivated(activatedArgs =>
                {
                    var session = activatedArgs.Instance;
                    session.EnableFilter(DeletedFilter.Name);
                    session.FlushMode = FlushMode.Always;

                    if (!ThreadStaticSessionContext.HasBind(session.SessionFactory))
                        ThreadStaticSessionContext.Bind(session);
                }).As<ISession>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            //Register repositories
            builder.RegisterAssemblyTypes(typeof(UserRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            //Register services
            builder.RegisterAssemblyTypes(typeof(UserService).Assembly)
                .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Api"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

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
        }
    }
}