using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Lfpis.Models.Domain;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Envers.Configuration;
using NHibernate.Event;
using Psps.Core;
using Psps.Core.Models;
using Psps.Data.DB.Interceptors;
using Psps.Data.DB.Listeners;
using Psps.Data.Mappings;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Psps.Data.Infrastructure
{
    static public class ConnectionHelper
    {
        public static ISessionFactory BuildSessionFactory(string ConnString)
        {
            return GetConfiguration(ConnString).BuildSessionFactory();
        }

        public static ISessionFactory BuildSessionFactory(FluentConfiguration config)
        {
            return config.BuildSessionFactory();
        }

        public static FluentConfiguration GetConfiguration(string ConnString)
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(c => c.FromConnectionStringWithKey(ConnString)))
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<UserMap>()
                        .Conventions.AddFromAssemblyOf<ManyToManyIdNameConvention>();
                })
                .ExposeConfiguration(cfg =>
                {
                    cfg.SetProperty("command_timeout", TimeSpan.FromMinutes(5).TotalSeconds.ToString());
                    //cfg.Interceptor = new AuditInterceptor(cfg.Interceptor ?? new EmptyInterceptor());
                    new AuditFlushEntityEventListener().Register(cfg);
                    cfg.SetProperty(NHibernate.Cfg.Environment.CurrentSessionContextClass, "web");
                    ConfigureEnvers(cfg);
                });
        }

        private static void ConfigureEnvers(NHibernate.Cfg.Configuration nhConf)
        {
            var enversConf = new NHibernate.Envers.Configuration.Fluent.FluentConfiguration();
            enversConf.SetRevisionEntity<RevInfo>(r => r.RevInfoId, r => r.RevisionedOn, new RevInfoListener());

            //get all classes derived from BaseAuditEntity<>
            var baseAuditEntityType = typeof(BaseAuditEntity<>);
            var auditableClasses = typeof(User).Assembly
                .GetTypes()
                .Where(t => t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == baseAuditEntityType)
                .ToList();
            var genericMethod = (from method in typeof(NHibernate.Envers.Configuration.Fluent.FluentConfiguration).GetMethods()
                                 where method.Name == "Audit" && method.IsGenericMethod && method.GetParameters().Length == 0
                                 select method).Single();

            foreach (System.Type auditableClass in auditableClasses)
            {
                var auditMethod = genericMethod.MakeGenericMethod(auditableClass);
                dynamic fluentAudit = auditMethod.Invoke(enversConf, null);
                var nonAuditableProperties = from property in auditableClass.GetProperties()
                                             where (property.PropertyType.IsGenericType
                                             && typeof(IList<>).IsAssignableFrom(property.PropertyType.GetGenericTypeDefinition())
                                             && !property.PropertyType.GetGenericArguments().FirstOrDefault().IsDerivedFromOpenGenericType(baseAuditEntityType))
                                             || (!property.PropertyType.IsFundamental() && !property.PropertyType.IsDerivedFromOpenGenericType(baseAuditEntityType))
                                             select property;

                foreach (var property in nonAuditableProperties)
                {
                    fluentAudit.Exclude(property.Name);
                }
            }

            nhConf.SetEnversProperty(ConfigurationKey.AuditTableSuffix, "Jnl");
            nhConf.SetEnversProperty(ConfigurationKey.RevisionFieldName, "RevInfoId");
            nhConf.SetEnversProperty(ConfigurationKey.RevisionTypeFieldName, "RevType");

            nhConf.IntegrateWithEnvers(enversConf);
        }
    }
}