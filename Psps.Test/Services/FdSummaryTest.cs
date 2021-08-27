using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Psp;
using Psps.Services.FlagDays;
using Psps.Services.Organisations;
using Psps.Services.PSPs;
using Psps.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Test.Data
{
    public static class FDSummaryTest
    {
        public class FDSummaryContext : AutoRollbackContextSpecification
        {
            protected GridSettings grid;
            protected IFlagDayService _flagDayService;

            public FDSummaryContext()
            {
                _flagDayService = EngineContext.Current.Resolve<IFlagDayService>();
            }
        }

        [TestClass]
        public class when_get_FDSummary_default_page : FDSummaryContext
        {
            [TestMethod]
            public void FDSummaryGrid_test()
            {
                var page = _flagDayService.GetPageByFdAcSummaryView(grid, "", "");
                Console.Write("Sucess");
            }

            protected override void Context()
            {
                grid = new GridSettings()
                {
                    IsSearch = true,
                    PageIndex = 1,
                    PageSize = 10,
                    Where = new Filter()
                    {
                        groupOp = "AND",
                        rules = new List<Psps.Core.JqGrid.Models.Rule>
                        {
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "flagDay",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "twr",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "permitNum",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "OrgMaster.EngOrgName",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "orgMaster.OrgRef",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "submissionDueDate",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "firstReminderIssueDate",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "firstReminderDeadline",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "secondReminderIssueDate",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "secondReminderDeadline",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "overdue",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "late",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "auditReportReceivedDate",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "publicationReceivedDate",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "acknowledgementReceiveDate",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "docRemark",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "streetCollection",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "grossProceed",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "expenditure",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "netProceed",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "fdPercent",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "newspaperPublishDate",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "pledgingAmt",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "newspaperPublishDate",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "afsReceiveIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "requestPermitteeIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "afsReSubmitIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "afsReminderIssueIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "withholdingListIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "withholdingBeginDate",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "withholdingEndDate",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "remark",
                                 op="eq",
                                 data="1"
                            }
                        }
                    }
                };
            }

            protected override void BecauseOf()
            {
            }
        }
    }
}