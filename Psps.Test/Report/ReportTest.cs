using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psps.Core.Infrastructure;

//using Psps.Services.Reports;
using Psps.Services.Suggestions;
using Psps.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Test.Report
{
    public static class ReportTest
    {
        public class ReportContext : AutoRollbackContextSpecification
        {
            //public IReportService reportService;

            public ISuggestionMasterService suggestionMasterService;

            public ReportContext()
            {
                //reportService = EngineContext.Current.Resolve<IReportService>();
                suggestionMasterService = EngineContext.Current.Resolve<ISuggestionMasterService>();
            }
        }

        [TestClass]
        public class when_generate_r20 : ReportContext
        {
            [TestMethod]
            public void stream_should_not_be_empty()
            {
                //String templatePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\Reports\\R20.rpt";
                //String templatePath = "D:\\Project\\SWD\\Source\\Psps\\trunk\\Psps.Test\\Report\\R20.rpt";
                //String templatePath = "D:\\Project\\SWD\\Source\\Psps\\trunk\\Psps.Test\\bin\\Debug\\Reports\\R20.rpt";
                //String templatePath = Directory.GetCurrentDirectory() + "\\Reports\\R20.rpt";
                var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Reports\R20.rpt");
                //using (var memoryStream = suggestionMasterService.GenerateR20PDF(templatePath, Convert.ToDateTime("01/01/2007"), Convert.ToDateTime("12/01/2014")))
                using (var memoryStream = suggestionMasterService.GenerateR20PDF(templatePath, null, null))
                using (var fileStream = File.Open(@"C:\Users\Byron\Desktop\R20.pdf", FileMode.Create))
                {
                    Assert.IsNotNull(memoryStream, "Except memory stream has content");
                    Assert.IsTrue(memoryStream.Length > 0, "Except memory stream has content");
                    memoryStream.Position = 0;
                    memoryStream.CopyTo(fileStream);
                    Assert.IsTrue(fileStream.Length > 0, "Except file stream has content");
                }
            }

            protected override void Context()
            {
            }

            protected override void BecauseOf()
            {
            }
        }
    }
}