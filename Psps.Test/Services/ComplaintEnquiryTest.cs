using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Psp;
using Psps.Services.ComplaintMasters;
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
    public static class ComplaintEnquiryTest
    {
        public class ComplaintEnquiryContext : AutoRollbackContextSpecification
        {
            protected GridSettings grid;
            protected IComplaintMasterService _complaintMasterService;

            public ComplaintEnquiryContext()
            {
                _complaintMasterService = EngineContext.Current.Resolve<IComplaintMasterService>();
            }
        }

        [TestClass]
        public class when_get_Org_default_page : ComplaintEnquiryContext
        {
            [TestMethod]
            public void OrgGrid_test()
            {
                var page = _complaintMasterService.GetPageByComplaintMasterSearchView(grid, false, false, false, false, null, null);
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
                                 field = "subventedIndicator",
                                 op="eq",
                                 data="True"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "complaintRecordType",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "complaintRef",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "complaintSource",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "activityConcern",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "complaintDate",
                                 op="eq",
                                 data="2000-01-01"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "processStatus",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "followUpAction",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "pspRef",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "complaintResult",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "replyDueDate",
                                 op="eq",
                                 data="2000-01-01"
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
                                 field = "telRecordNum",
                                 op="eq",
                                 data="1"
                            },
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "otherDepartmentEnquiryNum",
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

    [TestClass]
    public class ComplaintEnquiryTest2 : BaseTest
    {
        protected static IComplaintMasterService _complaintMasterService;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _complaintMasterService = EngineContext.Current.Resolve<IComplaintMasterService>();
        }

        [ClassCleanup]
        public static void ClassCleanup() { }

        [TestInitialize]
        public void TestInitialize() { }

        [TestCleanup]
        public void TestCleanup() { }

        [TestMethod]
        public void Generate_RefNum()
        {
        }
    }
}