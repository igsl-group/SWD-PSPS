using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Psp;
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
    public static class PSPTest
    {
        public class PSPContext : AutoRollbackContextSpecification
        {
            protected GridSettings grid;
            protected IPspService _pspService;

            public PSPContext()
            {
                _pspService = EngineContext.Current.Resolve<IPspService>();
            }
        }

        [TestClass]
        public class when_get_PSP_default_page : PSPContext
        {
            [TestMethod]
            public void PSPGrid_test()
            {
                var page = _pspService.GetPagePspSearchDto(grid);
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
                                 field = "orgRef",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "orgName",
                                 op="eq",
                                 data="2"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "subventedIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "section88Indicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "regType1",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "regType2",
                                 op="eq",
                                 data="2"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "pspRef",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "approvalStatus",
                                 op="eq",
                                 data="2"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "permitNum",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "processingOfficerPost",
                                 op="eq",
                                 data="2"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "totEvent",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "eventApprovedNum",
                                 op="eq",
                                 data="2"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "eventHeldNum",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "eventCancelledNum",
                                 op="eq",
                                 data="2"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "eventHeldPercent",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "overdueIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "lateIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "engOrgName",
                                 op="eq",
                                 data="2"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "chiOrgName",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "engRegisteredAddress1",
                                 op="eq",
                                 data="2"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "chiRegisteredAddress1",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "contactPerson",
                                 op="eq",
                                 data="2"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "contactPersonEmailAddress",
                                 op="eq",
                                 data="2"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "orgEmailAddress",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "applicationReceiveDate",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "applicationDisposalDate",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "disableIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "contactPersonName",
                                 op="eq",
                                 data="2"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "contactPersonChiName",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "regOtherName1",
                                 op="eq",
                                 data="2"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "regOtherName2",
                                 op="eq",
                                 data="2"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "eventStartDate",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "eventEndDate",
                                 op="eq",
                                 data="2000-01-01"
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