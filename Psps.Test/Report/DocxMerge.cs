using DocxGenerator.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.FdMaster;
using Psps.Services.FlagDays;
using Psps.Services.Report;

//using Psps.Services.Reports;
using Psps.Services.Suggestions;
using Psps.Services.SystemParameters;
using Psps.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Test.Report
{
    public static class DocxMerge
    {
        public class DocxMergeContext : AutoRollbackContextSpecification
        {
            //public IReportService reportService;

            public IReportService reportService;

            public DocxMergeContext()
            {
                //reportService = EngineContext.Current.Resolve<IReportService>();
                reportService = EngineContext.Current.Resolve<IReportService>();
            }
        }

        [TestClass]
        public class when_mergeDocx : DocxMergeContext
        {
            [TestMethod]
            public void merge_all_fd_docx()
            {
                IFlagDayService flagDayService = EngineContext.Current.Resolve<IFlagDayService>();
                IFlagDayDocService flagDayDocService = EngineContext.Current.Resolve<IFlagDayDocService>();
                IParameterService parameterService = EngineContext.Current.Resolve<IParameterService>();

                string targetDirectory = @"C:\Temp\merge_all_fd_docx";

                //Create target folder
                CommonHelper.CreateFolderIfNeeded(targetDirectory);

                GridSettings grid = new GridSettings()
                {
                    IsSearch = true,
                    PageSize = 999,
                    PageIndex = 1,
                    Where = new Filter()
                    {
                        groupOp = "AND",
                        rules = new List<Rule>
                        {
                            new Rule {
                                 field = "fdLotResult",
                                 op = "eq",
                                 data = "1"
                            }
                        }
                    }
                };

                string tempFolderPath = "";
                string tempFilePath = "";
                string time = "";

                var sysParam = parameterService.GetParameterByCode("FlagDayTemplatePath");
                string inputFilePath = "";

                //FdMaster List
                var flagDays = flagDayService.GetPageByFlagDaySearchDto(grid);

                //Template List
                var templateList = flagDayDocService.getAllFdDocTemplateForDropdown();

                foreach (var templateId in templateList.Where(x => x.Value.Contains("T29")))
                //foreach (var templateId in templateList)
                {
                    var template = flagDayDocService.GetFdDocById(Convert.ToInt32(templateId.Key));
                    inputFilePath = Path.Combine(@sysParam.Value, template.FileLocation);

                    if (!System.IO.File.Exists(inputFilePath))
                        throw new Exception("Template not found");

                    tempFolderPath = Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());
                    //Create temp folder
                    CommonHelper.CreateFolderIfNeeded(tempFolderPath);

                    try
                    {
                        foreach (FlagDaySearchDto flagDay in flagDays)
                        {
                            FdDocView fd = flagDayDocService.GetFdDocViewById(flagDay.FdMasterId);

                            SimpleDocumentGenerator<FdDocView> docGenerator = new SimpleDocumentGenerator<FdDocView>(new DocumentGenerationInfo
                            {
                                DataContext = fd,
                                TemplateData = System.IO.File.ReadAllBytes(inputFilePath)
                            });

                            time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
                            tempFilePath = Path.Combine(tempFolderPath, time + ".docx");

                            docGenerator.ToFile(tempFilePath);
                        }

                        List<string> arrayList = Directory.GetFiles(tempFolderPath).ToList();

                        time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
                        tempFilePath = Path.Combine(targetDirectory, template.DocName + "_" + time + ".docx");

                        reportService.DocumentsMerge(tempFilePath, arrayList);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        //Delete Temp Folder
                        Directory.Delete(tempFolderPath, true);
                    }
                }
            }

            [TestMethod]
            public void merge_docx()
            {
                var targetDirectory = @"D:\Downloads\Word_Docx_Merge";
                //var targetDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Word_Docx_Merge");
                List<string> arrayList = Directory.GetFiles(targetDirectory).ToList();

                string outputFile = Path.Combine(targetDirectory, "MergeDocx.docx");
                reportService.DocumentsMerge(outputFile, arrayList);
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