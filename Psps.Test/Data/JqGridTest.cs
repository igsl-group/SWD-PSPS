using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Data.Repositories;
using Psps.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Test.Data
{
    [TestClass]
    public class JqGridTest : BaseTest
    {
        public static IPSPMasterRepository pspsMasterRepository;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            pspsMasterRepository = EngineContext.Current.Resolve<IPSPMasterRepository>();
        }

        [ClassCleanup]
        public static void ClassCleanup() { }

        [TestInitialize]
        public void TestInitialize() { }

        [TestCleanup]
        public void TestCleanup() { }

        [TestMethod]
        public void Query_on_single_property()
        {
            var grid = new GridSettings()
            {
                IsSearch = true,
                PageSize = 10,
                PageIndex = 1,
                Where = new Filter()
                {
                    groupOp = "AND",
                    rules = new List<Rule>
                        {
                            new Rule {
                                 field = "PspRef",
                                 op = "cn",
                                 data = "12"
                            }
                        }
                }
            };

            var page = pspsMasterRepository.GetPage(grid);
            Assert.IsTrue(page.Count == 1);
        }

        [TestMethod]
        public void Query_on_multi_property()
        {
            var grid = new GridSettings()
            {
                IsSearch = true,
                PageSize = 9999,
                PageIndex = 1,
                Where = new Filter()
                {
                    groupOp = "OR",
                    rules = new List<Rule>
                        {
                            new Rule {
                                 field = "OrgMaster.OrgRef,SpecialRemark",
                                 op = "cn",
                                 data = "580"
                            }, new Rule {
                                 field = "PspRef",
                                 op = "cn",
                                 data = "12"
                            }
                        }
                }
            };

            var page = pspsMasterRepository.GetPage(grid);
            Assert.IsTrue(page.Count == 1137);

            grid = new GridSettings()
            {
                IsSearch = true,
                PageSize = 9999,
                PageIndex = 1,
                Where = new Filter()
                {
                    groupOp = "AND",
                    rules = new List<Rule>
                        {
                            new Rule {
                                 field = "OrgMaster.OrgRef,SpecialRemark",
                                 op = "cn",
                                 data = "580"
                            }, new Rule {
                                 field = "PspRef",
                                 op = "cn",
                                 data = "12"
                            }
                        }
                }
            };

            page = pspsMasterRepository.GetPage(grid);
            Assert.IsTrue(page.Count == 4);
        }

        [TestMethod]
        public void Query_on_collection()
        {
            var grid = new GridSettings()
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
                                 field = "PspEvent>EventStatus",
                                 op = "eq",
                                 data = "AP"
                            }
                        }
                }
            };

            var page = pspsMasterRepository.GetPage(grid);
            Assert.IsTrue(page.Count == 44);
        }

        [TestMethod]
        public void Order_by_single_column()
        {
            var grid = new GridSettings()
            {
                IsSearch = false,
                PageSize = 10,
                PageIndex = 1,
                SortColumn = "PspRef",
                SortOrder = "asc"
            };

            var page = pspsMasterRepository.GetPage(grid);
            Assert.IsTrue(page.Count == 10);
        }

        [TestMethod]
        public void Order_by_multi_column()
        {
            var grid = new GridSettings()
            {
                IsSearch = false,
                PageSize = 10,
                PageIndex = 1,
                SortColumn = "ContactPersonFirstName desc, BeneficiaryOrg",
                SortOrder = "desc"
            };

            var page = pspsMasterRepository.GetPage(grid);
            Assert.IsTrue(page.Count == 10);
        }
    }
}