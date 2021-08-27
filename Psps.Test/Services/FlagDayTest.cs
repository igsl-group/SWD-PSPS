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
    public static class FlagDayTest
    {
        public class FlagDayContext : AutoRollbackContextSpecification
        {
            protected GridSettings grid;
            protected IFlagDayService _flagDayService;

            public FlagDayContext()
            {
                _flagDayService = EngineContext.Current.Resolve<IFlagDayService>();
            }
        }

        [TestClass]
        public class when_get_FlagDay_default_page : FlagDayContext
        {
            [TestMethod]
            public void FlagDayGrid_test()
            {
                var page = _flagDayService.GetPageByFlagDaySearchDto(grid);
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
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "subventedIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "permitNum",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "flagDay",
                                 op="eq",
                                 data="2001-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "twr",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "twrDistrict",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "newApplicantIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "applicationResult",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "engOrgName",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "chiOrgName",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "engMailingAddress1",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "chiMailingAddress1",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "contactPersonSalute",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "contactPersonEmailAddress",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "emailAddress",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "applicationReceiveDate",
                                 op="eq",
                                 data="2001-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "disableIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "contactPersonName",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "contactPersonChiName",
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