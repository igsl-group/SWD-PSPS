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
    public static class OrgTest
    {
        public class OrgContext : AutoRollbackContextSpecification
        {
            protected GridSettings grid;
            protected IOrganisationService _organisationService;

            public OrgContext()
            {
                _organisationService = EngineContext.Current.Resolve<IOrganisationService>();
            }
        }

        [TestClass]
        public class when_get_Org_default_page : OrgContext
        {
            [TestMethod]
            public void OrgGrid_test()
            {
                var page = _organisationService.GetPageByOrgMasterSearchView(grid, "", "", "", "", null, null, "", null,  "",  null, null, "", null, null, "", null, "", null, null);
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
                                 field = "registrationType1",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "pspIssuedNum",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "fdPermitIssuedNum",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "complaintReceivedNum",
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
                                 field = "addressProofIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "disableIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "simpChiOrgName",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "otherEngOrgName",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "otherChiOrgName",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "otherSimpChiOrgName",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "engRegisteredAddress1",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "chiRegisteredAddress1",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "engRegisteredAddress2",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "engRegisteredAddress3",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "engRegisteredAddress4",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "engRegisteredAddress5",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "chiRegisteredAddress1",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "chiRegisteredAddress2",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "chiRegisteredAddress3",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "chiRegisteredAddress4",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "chiRegisteredAddress5",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "engMailingAddress1",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "engMailingAddress2",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "engMailingAddress3",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "engMailingAddress4",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "engMailingAddress5",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "chiMailingAddress1",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "chiMailingAddress3",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "chiMailingAddress4",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "chiMailingAddress5",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "uRL",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "telNum",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "faxNum",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "emailAddress",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "applicantSalute",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "applicantFirstName",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "applicantLastName",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "applicantPosition",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "applicantTelNum",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "presidentName",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "secretaryName",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "treasurerName",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "orgObjective",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "section88StartDate",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "registrationOtherName1",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "registrationDate1",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "registrationDate2",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "registrationOtherName2",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "addressProofIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "addressProofDate",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "maaConstitutionIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "qualifiedOpinionIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "qualifiedOpinionRemark",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "otherSupportDocIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "otherSupportDocRemark",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "overallRemark",
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