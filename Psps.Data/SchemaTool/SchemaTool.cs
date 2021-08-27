using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Psps.Data.SchemaTool
{
    public static class SchemaTool
    {
        public static void ValidateSchema(string ConnString)
        {
            Psps.Data.Infrastructure.ConnectionHelper.GetConfiguration(ConnString).ExposeConfiguration(cfg =>
            {
                var schemaValidate = new NHibernate.Tool.hbm2ddl.SchemaValidator(cfg);
                schemaValidate.Validate();
            }).BuildConfiguration();
        }

        public static void CreatSchema(string OutputFile, string ConnString)
        {
            Psps.Data.Infrastructure.ConnectionHelper.GetConfiguration(ConnString).ExposeConfiguration(cfg =>
            {
                var schemaExport = new NHibernate.Tool.hbm2ddl.SchemaExport(cfg);
                if (!string.IsNullOrEmpty(OutputFile)) schemaExport.SetOutputFile(OutputFile);
                schemaExport.Drop(false, true);
                schemaExport.Create(false, true);
            }).BuildConfiguration();
        }

        public static void UpdateSchema(string ConnString)
        {
            Psps.Data.Infrastructure.ConnectionHelper.GetConfiguration(ConnString).ExposeConfiguration(cfg =>
            {
                var schemaUpdate = new NHibernate.Tool.hbm2ddl.SchemaUpdate(cfg);
                schemaUpdate.Execute(false, true);
            }).BuildConfiguration();
        }

        public static void ExportSchema(string OutputFile, string ConnString)
        {
            Psps.Data.Infrastructure.ConnectionHelper.GetConfiguration(ConnString).ExposeConfiguration(cfg =>
            {
                var schemaExport = new NHibernate.Tool.hbm2ddl.SchemaExport(cfg);
                if (!string.IsNullOrEmpty(OutputFile)) schemaExport.SetOutputFile(OutputFile);
                schemaExport.Execute(true, false, false);
            }).BuildConfiguration();
        }

        public static void ExportHbm(string OutputPath, string ConnString)
        {
            Psps.Data.Infrastructure.ConnectionHelper.GetConfiguration(ConnString).Mappings(m =>
            {
                if (!string.IsNullOrEmpty(OutputPath))
                {
                    if (!Directory.Exists(OutputPath))
                    {
                        Directory.CreateDirectory(OutputPath);
                    }

                    m.FluentMappings.ExportTo(OutputPath);
                }
            }).BuildConfiguration();
        }
    }
}