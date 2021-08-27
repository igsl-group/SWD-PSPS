using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
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
    public static class UserTest
    {
        public class UserContext : AutoRollbackContextSpecification
        {
            protected User user;
            protected GridSettings grid;
            protected IPagedList<User> page;
            protected IUserRepository _userRepository;

            public UserContext()
            {
                _userRepository = EngineContext.Current.Resolve<IUserRepository>();
            }
        }

        [TestClass]
        public class when_add_user : UserContext
        {
            [TestMethod]
            public void user_should_be_created()
            {
                var newlyAddedUser = _userRepository.GetById("UserId");

                Assert.AreEqual("UserId", newlyAddedUser.UserId);
                Assert.AreEqual("EngUserName", newlyAddedUser.EngUserName);
                Assert.AreEqual("ChiUserName", newlyAddedUser.ChiUserName);
                Assert.AreEqual("TelephoneNumber", newlyAddedUser.TelephoneNumber);
                Assert.AreEqual("Email", newlyAddedUser.Email);
                Assert.AreEqual(true, newlyAddedUser.IsActive);
                Assert.AreEqual(false, newlyAddedUser.IsSystemAdministrator);
            }

            protected override void Context()
            {
                user = new User
                {
                    UserId = "UserId",
                    EngUserName = "EngUserName",
                    ChiUserName = "ChiUserName",
                    TelephoneNumber = "TelephoneNumber",
                    Email = "Email",
                    IsActive = true,
                    IsSystemAdministrator = false
                };
            }

            protected override void BecauseOf()
            {
                _userRepository.Add(user);
            }
        }

        [TestClass]
        public class when_update_user : UserContext
        {
            [TestMethod]
            public void user_should_be_updated()
            {
                var updatedUser = _userRepository.GetById("UserId");

                Assert.AreEqual("EngUserName_changed", updatedUser.EngUserName);
                Assert.AreEqual("ChiUserName_changed", updatedUser.ChiUserName);
                Assert.AreEqual("TelephoneNumber_changed", updatedUser.TelephoneNumber);
                Assert.AreEqual("Email_changed", updatedUser.Email);
                Assert.AreEqual(false, updatedUser.IsActive);
                Assert.AreEqual(true, updatedUser.IsSystemAdministrator);
            }

            protected override void Context()
            {
                user = new User
                {
                    UserId = "UserId",
                    EngUserName = "EngUserName",
                    ChiUserName = "ChiUserName",
                    TelephoneNumber = "TelephoneNumber",
                    Email = "Email",
                    IsActive = true,
                    IsSystemAdministrator = false
                };
            }

            protected override void BecauseOf()
            {
                _userRepository.Add(user);

                var newlyAddedUser = _userRepository.GetById("UserId");
                newlyAddedUser.EngUserName = "EngUserName_changed";
                newlyAddedUser.ChiUserName = "ChiUserName_changed";
                newlyAddedUser.TelephoneNumber = "TelephoneNumber_changed";
                newlyAddedUser.Email = "Email_changed";
                newlyAddedUser.IsActive = false;
                newlyAddedUser.IsSystemAdministrator = true;

                _userRepository.Update(newlyAddedUser);
            }
        }

        [TestClass]
        public class when_get_default_page : UserContext
        {
            [TestMethod]
            public void page_should_be_with_data()
            {
                Assert.IsTrue(page.Count > 0);
            }

            protected override void Context()
            {
                grid = new GridSettings()
                {
                    IsSearch = false,
                    PageIndex = 1,
                    PageSize = 10
                };
            }

            protected override void BecauseOf()
            {
                page = _userRepository.GetPage(grid);
            }
        }

        [TestClass]
        public class when_get_filterd_page : UserContext
        {
            [TestMethod]
            public void page_should_be_with_filter_data()
            {
                Assert.IsTrue(page.Count > 0);
                Assert.IsTrue(page.Where(x => x.UserId == "testid1").Count() > 0);
                Assert.IsTrue(page.Where(x => x.UserId != "testid1").Count() == 0);
            }

            protected override void Context()
            {
                grid = new GridSettings()
                {
                    IsSearch = true,
                    PageSize = 10,
                    PageIndex = 1,
                    SortColumn = "UserId",
                    SortOrder = "asc",
                    Where = new Filter()
                    {
                        groupOp = "AND",
                        rules = new List<Psps.Core.JqGrid.Models.Rule>
                        {
                            new Psps.Core.JqGrid.Models.Rule {
                                 field = "UserId",
                                 op="eq",
                                 data="testid1"
                            }
                        }
                    }
                };
            }

            protected override void BecauseOf()
            {
                page = _userRepository.GetPage(grid);
            }
        }
    }
}