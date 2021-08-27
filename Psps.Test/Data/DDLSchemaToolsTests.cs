using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psps.Data.SchemaTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Test.Data
{
    [TestClass]
    public class DDLSchemaToolsTests
    {
        public DDLSchemaToolsTests()
        {
            //HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize(); //NH Profiler
        }

        [TestMethod, Ignore]
        public void CreateSchema()
        {
            SchemaTool.CreatSchema("", "DefaultConnection");
        }

        [TestMethod, Ignore]
        public void UpdateSchema()
        {
            SchemaTool.UpdateSchema("DefaultConnection");
        }

        [TestMethod]
        public void ValidateSchema()
        {
            SchemaTool.ValidateSchema("DefaultConnection");
        }

        [TestMethod]
        public void ExportSchema()
        {
            SchemaTool.ExportSchema(@"c:\temp\psps.txt", "DefaultConnection");
        }

        [TestMethod]
        public void ExportHbm()
        {
            SchemaTool.ExportHbm(@"c:\temp\psps.hbm", "DefaultConnection");
        }
    }
}