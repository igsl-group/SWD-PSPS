using Psps.Core.Infrastructure;
using Psps.Data.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Test.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Test.Data
{
    public static class AuditEventTest
    {
        public class AuditEventContext : AutoRollbackContextSpecification
        {
            protected User user;
            protected IUserRepository _userRepository;
        }

        [TestClass]
        public class when_add_entity : AuditEventContext
        {
            [TestMethod]
            public void created_info_should_be_filled()
            {
                var newlyAddedUser = _userRepository.GetById("unittest");

                Assert.IsNotNull(newlyAddedUser.CreatedOn);
                Assert.IsNotNull(newlyAddedUser.CreatedById);
                Assert.IsNotNull(newlyAddedUser.CreatedByPost);
            }

            protected override void Context()
            {
                _userRepository = EngineContext.Current.Resolve<IUserRepository>();
            }

            protected override void BecauseOf()
            {
                _userRepository.Add(new User()
                {
                    UserId = "unittest",
                    EngUserName = "unittest"
                });
            }

            protected override void Cleanup()
            {
            }
        }

        [TestClass]
        public class when_update_entity : AuditEventContext
        {
            [TestMethod]
            public void updated_on_should_be_filled()
            {
                var newlyAddedUser = _userRepository.GetById("unittest");

                Assert.IsNotNull(newlyAddedUser.UpdatedOn);
                //Assert.IsNotNull(newlyAddedUser.UpdatedById);
                //Assert.IsNotNull(newlyAddedUser.UpdatedByPost);

                Assert.AreNotEqual(newlyAddedUser.CreatedOn, newlyAddedUser.UpdatedOn);
                //Assert.AreNotEqual(newlyAddedUser.CreatedById, newlyAddedUser.UpdatedById);
                //Assert.AreNotEqual(newlyAddedUser.CreatedByPost, newlyAddedUser.UpdatedByPost);
            }

            protected override void Context()
            {
                _userRepository = EngineContext.Current.Resolve<IUserRepository>();
            }

            protected override void BecauseOf()
            {
                _userRepository.Add(new User()
                {
                    UserId = "unittest",
                    EngUserName = "unittest"
                });

                var newlyAddedUser = _userRepository.GetById("unittest");
                newlyAddedUser.EngUserName = "unittest_changed";

                _userRepository.Update(newlyAddedUser);
            }

            protected override void Cleanup()
            {
            }
        }
    }
}