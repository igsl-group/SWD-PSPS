using DocxGenerator.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordDocumentGenerator.Library;

namespace Psps.Test.Data
{
    public static class OrgMasterTest
    {
        [TestClass]
        public class when_merge : AutoRollbackContextSpecification
        {
            [TestMethod]
            public void docx_should_be_generated()
            {
            }

            protected override void BecauseOf()
            {
                OrgMaster orgMaster = new OrgMaster
                {
                    ChiOrgName = "中文",
                    EngOrgName = "ENG",
                    EmailAddress = "abc@gmail.com",
                    FdMaster = new List<FdMaster> {
                        new FdMaster {
                             Remark = "Remark 1"
                        }, new FdMaster {
                             Remark = "Remark 2"
                        }, new FdMaster {
                             Remark = "Remark 3",
                             FdEvent = new List<FdEvent> {
                                 new FdEvent { FdEventId = 1},
                                 new FdEvent { FdEventId = 2},
                                 new FdEvent { FdEventId = 3},
                            }
                        },
                    }
                };

                string filePath = @"MailMerge\Templates";
                string fileName = "Test_Template - 1.docx";
                RunDocumentGenerator<OrgMaster> doc = new RunDocumentGenerator<OrgMaster>(filePath, fileName);
                doc.generateDoc(orgMaster);
            }
        }
    }
}