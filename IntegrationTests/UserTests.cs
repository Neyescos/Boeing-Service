using BLL.Services;
using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using NUnit.Framework;
using System;
using System.Linq;

namespace IntegrationTests
{
    [TestFixture]
    public class UserTests
    {
        private IUserRepository repository;
        private User user;
        [SetUp]
        public void SetUp()
        {
            repository = new UserRepository(new BoengServiceWebSiteContext());
            var encoder = new PasswordEncodingService();
            user = new User() { Name = "Colin", RegistrationDate = DateTime.UtcNow, Role = 2, Password = encoder.CalculateSHA256("qwerty")};
            UserRepository_AddUser(user);
        }


        public void UserRepository_AddUser(User user)
        {
            repository.CreateUser(user);
            var createdUser = repository.GetAll().FirstOrDefault(u => u.Name == user.Name);
            user = createdUser;
        }

        [Test]
        public void UserRepository_GetUsers()
        {
            //Arrange
            //Act
            var users = repository.GetAll();
            //Assert
            Assert.NotNull(users);
            Assert.IsNotEmpty(users);
            Assert.NotNull(users.FirstOrDefault(u => u.Id == user.Id));
        }

        [Test]
        public void UserRepository_GetUser()
        {
            //Arrange
            //Act
            var getUser = repository.GetUser(user.Id);
            //Assert
            Assert.NotNull(getUser);
            Assert.AreEqual(user.Id, getUser.Id);
        }

        [Test]
        public void UserRepository_DeleteUser()
        {
            //Arrange
            //Act
            repository.DeleteUser(user);
            var getUser = repository.GetUser(user.Id);
            //Assert
            Assert.Null(getUser);
        }
    }
}
