using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Models.Domain;
using Psps.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Psps.Test.Data
{
    public static class ReadXls
    {
        public class ReadXlsTest : AutoRollbackContextSpecification
        {
            protected PspEvent pspEvent;

            public ReadXlsTest()
            {
            }
        }

        [TestClass]
        public class test : ReadXlsTest
        {
            [TestMethod]
            public void StartReadXls()
            {
                Console.Write("test");

                var package = new ExcelPackage(new FileInfo("C:\\Users\\crchow\\Desktop\\test.xlsx"));

                ExcelWorksheet workSheet = package.Workbook.Worksheets[2];
                Console.Write(package.Workbook.Worksheets.Count);
                //var start = workSheet.Dimension.Start;
                var target = workSheet.Cells[16, 1];
                var start = target.Start;
                var end = workSheet.Dimension.End;
                var endCol = 0;

                for (int i = 1; i <= end.Column; i++) // use title row to find end column.
                { if (!workSheet.Cells[start.Row, i].Equals("")) { endCol = i - 1; } }

                List<List<string>> resultList = new List<List<string>>();

                for (int i = start.Row + 1; i <= end.Row; i++)
                { // Row by row...
                    if (!workSheet.Cells[i, 1].Text.Equals(""))
                    {
                        List<string> list = new List<string>();
                        for (int j = start.Column; j <= endCol; j++)
                        { // ... Cell by cell...
                            //object cellValue = workSheet.Cells[i, j].Text; // This got me the actual value I needed.
                            Console.Write(" Reading ---" + workSheet.Cells[i, j].Text);

                            list.Add(workSheet.Cells[i, j].Text);
                        }
                        list.Add(i.ToString());

                        resultList.Add(list);
                    }
                    else break;
                }

                Dictionary<int, string> collectionMethod = new Dictionary<int, string>();

                collectionMethod.Add(1, workSheet.Cells[4, 2].Text);
                collectionMethod.Add(2, workSheet.Cells[6, 2].Text);
                collectionMethod.Add(3, workSheet.Cells[8, 2].Text);
                collectionMethod.Add(4, workSheet.Cells[10, 2].Text);
                collectionMethod.Add(5, workSheet.Cells[12, 2].Text);
                collectionMethod.Add(6, workSheet.Cells[14, 2].Text);

                workSheet = null;
                package.Dispose();

                DateTime startDate;
                DateTime endDate;

                foreach (var rec in resultList)
                {
                    Regex regex = new Regex(",");
                    Match match = regex.Match(rec[2]);

                    if (match.Success)
                    {
                        Array dates = rec[2].Split(',');
                        PspEvent pspEvent = createPspEve(rec, collectionMethod);
                    }
                    else
                    {
                        regex = new Regex("-");
                        match = regex.Match(rec[2]);

                        if (match.Success)
                        {
                            Array dates = rec[2].Split('-');
                            foreach (var d in dates)
                            {
                                PspEvent pspEvent = createPspEve(rec, collectionMethod);
                            }
                        }
                        else
                        {
                            PspEvent pspEvent = createPspEve(rec, collectionMethod);

                            startDate = new DateTime(Convert.ToInt32(rec[0]), Convert.ToInt32(rec[1]), Convert.ToInt32(rec[2]), Convert.ToInt32(rec[3].Substring(0, 2)), Convert.ToInt32(rec[3].Substring(2, 2)), 0);
                            endDate = new DateTime(Convert.ToInt32(rec[0]), Convert.ToInt32(rec[1]), Convert.ToInt32(rec[2]), Convert.ToInt32(rec[3].Substring(0, 2)), Convert.ToInt32(rec[3].Substring(2, 2)), 0);
                            //startTime = new TimeSpan(Convert.ToInt32(rec[3].Substring(0, 2)), Convert.ToInt32(rec[3].Substring(2, 2)), 0);
                            //endTime = new TimeSpan(Convert.ToInt32(rec[4].Substring(0, 2)), Convert.ToInt32(rec[4].Substring(2, 2)), 0);

                            pspEvent.EventStartDate = startDate;
                            pspEvent.EventEndDate = endDate;
                            pspEvent.EventStartTime = startDate;
                            pspEvent.EventEndTime = endDate;
                        }
                    }
                }
            }

            public PspEvent createPspEve(List<string> list, Dictionary<int, string> collectionMethod)
            {
                PspEvent pspEvent = new PspEvent();
                pspEvent.Location = list[5];
                pspEvent.ChiLocation = list[6];
                //pspEvent.SimpChiLocation = Microsoft.VisualBasic.Strings.StrConv(list[6],
                //                            Microsoft.VisualBasic.VbStrConv.SimplifiedChinese,
                //                            0);
                pspEvent.SimpChiLocation = LcMap.ToSimplified(list[6]);
                pspEvent.District = list[7];
                pspEvent.CollectionMethod = list[8].Replace("1", collectionMethod[1])
                                                    .Replace("2", collectionMethod[2])
                                                    .Replace("3", collectionMethod[3])
                                                    .Replace("4", collectionMethod[4])
                                                    .Replace("5", collectionMethod[5])
                                                    .Replace("6", collectionMethod[6]);
                return pspEvent;
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