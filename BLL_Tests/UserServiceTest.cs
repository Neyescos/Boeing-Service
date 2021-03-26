using AutoMapper;
using BLL.Interfaces;
using BLL.MapperProfile;
using BLL.Models;
using BLL.Services;
using DAL.Entities;
using DAL.Interfaces;
using DAL.UnitOfWork;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL_Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private UnitOfWork uow;
        private UserService userService;
        private Mock<IPlaneModelRepository> planeRepository;
        private Mock<IPlanePartRepository> planePartRepository;
        private Mock<IEncodingService> passwordEncodingService;
        private Mapper mapper;
        private User newUser;
        private UserDto UserDto;

        [SetUp]
        public void SetUp()
        {
            mapper = new DtoProfile().GetMapper();
            UserDto = new UserDto() { Id = 1, Name = "Andrey", Password = "qwerty", Role = 1, RegistrationDate = DateTime.UtcNow };
            newUser = mapper.Map<User>(UserDto);
            planeRepository = new Mock<IPlaneModelRepository>();
            planePartRepository = new Mock<IPlanePartRepository>();
            passwordEncodingService = new Mock<IEncodingService>();
            passwordEncodingService.Setup(s => s.CalculateSHA256(It.Is<string>(x => x == UserDto.Password))).Returns(newUser.Password);
        }

        [TearDown]
        public void TearDown()
        {
            mapper = null;
            planeRepository = null;
            newUser = null;
            UserDto = null;
            planePartRepository = null;
            userService = null;
        }

        [Test]
        public void UserService_GetAll()
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(a => a.GetAll()).Returns(new List<User>() { newUser });
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            userService = new UserService(uow, new DtoProfile(), passwordEncodingService.Object);

            //Act

            var users = userService.GetUsers();

            //Assert

            Assert.NotNull(users);
            Assert.IsNotEmpty(users);
            Assert.AreEqual(newUser.Name, users.FirstOrDefault().Name);
            userRepository.Verify(a => a.GetAll(), Times.Once());
        }


        [TestCase(1)]
        [TestCase(5)]
        [TestCase(11)]
        [TestCase(14)]
        [TestCase(22)]
        [TestCase(103)]
        public void UserService_GetEntity_NotNull(int id)
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(a => a.GetUser(It.Is<int>(i => i == id))).Returns(newUser);
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            userService = new UserService(uow, new DtoProfile(), passwordEncodingService.Object);

            //Act
            var getUser = userService.GetUser(id);

            //Assert
            Assert.NotNull(getUser);
            Assert.AreEqual(UserDto.Name, getUser.Name);
            userRepository.Verify(a => a.GetUser(It.Is<int>(x => x == id)), Times.Once());
        }

        [Test]
        public void UserService_CreateEntity_IsInvoked()
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(a => a.CreateUser(newUser)).Verifiable();
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            userService = new UserService(uow, new DtoProfile(), passwordEncodingService.Object);
            //Act
            userService.Registration(UserDto);

            //Assert
            userRepository.Verify(a => a.CreateUser(It.Is<User>(r => r != null)), Times.Once());

        }

        [Test]
        public void UserService_DeleteEntity_IsInvoked()
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(a => a.DeleteUser(newUser)).Verifiable();
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            userService = new UserService(uow, new DtoProfile(), passwordEncodingService.Object);
            //Act
            userService.DeleteUser(UserDto);
            //Assert
            userRepository.Verify(a => a.DeleteUser(It.Is<User>(r => r != null)), Times.Once());
        }

        [Test]
        public void UserService_Login_IsNotNull()
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            var encoder = new PasswordEncodingService();

            userRepository.Setup(a => a.Login(It.Is<string>(s => s == UserDto.Name), It.Is<byte[]>(a => a == newUser.Password))).Returns(newUser);
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            userService = new UserService(uow, new DtoProfile(), passwordEncodingService.Object);
            //Act
            var loginUser = userService.Login(UserDto.Name, UserDto.Password);

            //Assert
            Assert.IsNotNull(loginUser);
            Assert.AreEqual(UserDto.Name, loginUser.Name);
            userRepository.Verify(a => a.Login(It.Is<string>(s => s == UserDto.Name), It.Is<byte[]>(a => a == newUser.Password)), Times.Once());

        }

        [Test]
        public void UserService_UpdateEntity_IsInvoked()
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();

            userRepository.Setup(a => a.UpdateUser(newUser)).Verifiable();
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            userService = new UserService(uow, new DtoProfile(), passwordEncodingService.Object);
            //Act
            userService.UpdateUser(UserDto);
            //Assert
            userRepository.Verify(a => a.UpdateUser(It.Is<User>(r => r != null)), Times.Once());
        }

        [Test]
        public void UserService_CreateUser_Exception()
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(a => a.CreateUser(null)).Throws(new ArgumentNullException("user", "Null argument was passed as a parameter")).Verifiable();
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            userService = new UserService(uow, new DtoProfile(), passwordEncodingService.Object);
            //Act
            userService.Registration(null);

            //Assert

            var ex = Assert.Throws<ArgumentNullException>(() => userRepository.Object.CreateUser(null));
            Assert.That(ex.Message, Is.EqualTo("Null argument was passed as a parameter (Parameter 'user')"));
        }

        [Test]
        public void UserService_UpdateUser_Exception()
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(a => a.UpdateUser(null)).Throws(new ArgumentNullException("user", "Null argument was passed as a parameter")).Verifiable();
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            userService = new UserService(uow, new DtoProfile(), passwordEncodingService.Object);
            //Act
            userService.UpdateUser(null);

            //Assert

            var ex = Assert.Throws<ArgumentNullException>(() => userRepository.Object.UpdateUser(null));
            Assert.That(ex.Message, Is.EqualTo("Null argument was passed as a parameter (Parameter 'user')"));
        }

        [Test]
        public void UserService_Login_Exception()
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(a => a.Login(null,null)).Throws(new ArgumentNullException("user", "Null argument was passed as a parameter")).Verifiable();
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            userService = new UserService(uow, new DtoProfile(), passwordEncodingService.Object);
            //Act
            var user =userService.Login(null,null);

            //Assert
            Assert.Null(user);
            var ex = Assert.Throws<ArgumentNullException>(() => userRepository.Object.Login(null, null));
            Assert.That(ex.Message, Is.EqualTo("Null argument was passed as a parameter (Parameter 'user')"));
        }

        [Test]
        public void UserService_GetUsers_Exception()
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(a => a.GetAll()).Throws(new Exception()).Verifiable();
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            userService = new UserService(uow, new DtoProfile(), passwordEncodingService.Object);

            //Act
            var users = userService.GetUsers();

            //Assert
            Assert.NotNull(users);
            Assert.IsEmpty(users);
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(11)]
        [TestCase(14)]
        [TestCase(22)]
        [TestCase(103)]
        public void UserService_GetEntity_Exception(int id)
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(a => a.GetUser(It.Is<int>(i => i == id))).Throws(new Exception());
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            userService = new UserService(uow, new DtoProfile(), passwordEncodingService.Object);

            //Act
            var getUser = userService.GetUser(id);

            //Assert
            Assert.Null(getUser);
            userRepository.Verify(a => a.GetUser(It.Is<int>(x => x == id)), Times.Once());
        }


        [Test]
        public void UserService_DeleteUser_Exception()
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(a => a.DeleteUser(null)).Throws(new ArgumentNullException("user", "Null argument was passed as a parameter")).Verifiable();
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            userService = new UserService(uow, new DtoProfile(), passwordEncodingService.Object);
            //Act
            userService.DeleteUser(null);

            //Assert

            var ex = Assert.Throws<ArgumentNullException>(() => userRepository.Object.DeleteUser(null));
            Assert.That(ex.Message, Is.EqualTo("Null argument was passed as a parameter (Parameter 'user')"));
        }
    }
}
