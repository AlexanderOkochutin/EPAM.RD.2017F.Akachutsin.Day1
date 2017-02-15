using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UserServiceLibrary.CustomExceptions;
using UserServiceLibrary.Entities;
using UserServiceLibrary.Interfaces;
using UserServiceLibrary.Services;

namespace UserServiceLibrary.Tests
{
    [TestFixture()]
    public class UserServiceTests
    {
        [Test]
        public void Add_NotInitialized_User()
        {
            IUserService userService = new UserService();
            User user = new User();
            Assert.Throws<NotInitializedException>(() => userService.Add(user));
        }

        [Test]
        public void Add_exist_user()
        {
            IUserService userService = new UserService();
            User user = new User()
            {
                LastName = "test",
                FirstName = "test",
                BirthDate = DateTime.Now
            };
            userService.Add(user);
            Assert.Throws<AlreadyExistException>(() => userService.Add(user));
        }

        [Test]
        public void Add_null_User()
        {
            IUserService userService = new UserService();
            User user = null;
            Assert.Throws<ArgumentNullException>(() => userService.Add(user));
        }

        [Test]
        public void Add_valid_user()
        {
            IUserService userService = new UserService();
            User user = new User()
            {
                LastName = "test",
                FirstName = "test",
                BirthDate = DateTime.Now
            };
            userService.Add(user);
            IEnumerable<User> users = userService.Search(u => u.FirstName == "test");
            Assert.AreEqual(users.Count(),1);
        }

        [Test]
        public void Remove_NotExist_User()
        {
            IUserService userService = new UserService();
            User user = new User()
            {
                LastName = "test",
                FirstName = "test",
                BirthDate = DateTime.Now
            };
            Assert.Throws<NotExistException>(() => userService.Delete(user));
        }
    }
}
