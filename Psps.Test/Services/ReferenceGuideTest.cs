using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Psp;
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
    public static class ReferenceGuideTest
    {
        public class ReferenceGuideContext : AutoRollbackContextSpecification
        {
            protected GridSettings grid;
            protected IOrganisationService _organisationService;

            public ReferenceGuideContext()
            {
                _organisationService = EngineContext.Current.Resolve<IOrganisationService>();
            }
        }

        [TestClass]
        public class when_get_ReferenceGuide_default_page : ReferenceGuideContext
        {
            [TestMethod]
            public void ReferenceGuideGrid_test()
            {
                var page = _organisationService.GetPageByReferenceGuideSearchView(grid, "", null, null, "", null, null, null, null, "", "", "", null, null, "", null, null, "");
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
                                 field = "engOrgNameSorting",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "sendDate",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "orgReply",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "replySlipReceiveDate",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "engMailingAddress1",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "emailAddress",
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
                                 field = "contactPerson",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "emailAddress",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "refPartEnclosureNum",
                                 op="eq",
                                 data="True"
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