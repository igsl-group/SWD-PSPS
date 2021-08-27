using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psps.Core.Common;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Test.Data
{
    public static class EncryptorTest
    {
        public class EncryptorContext : ContextSpecification
        {
            public EncryptorContext()
            {
            }
        }

        [TestClass]
        public class when_encryte_password : EncryptorContext
        {
            private string password;
            private string hashedPassword;

            [TestMethod]
            public void length_of_hashed_password_should_be_40()
            {
                Assert.IsTrue(hashedPassword.Length == 40);
            }

            protected override void Context()
            {
                password = "123456789012345678901234567890123456789012345678901234567890";
            }

            protected override void BecauseOf()
            {
                hashedPassword = Encryptor.Hash(password);
            }
        }
    }
}