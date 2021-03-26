using AutoMapper;
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
    public class PlaneModelServiceTest
    {
        private UnitOfWork uow;
        private PlaneModelService planeService;
        private Mock<IPlanePartRepository> partRepository;
        private Mock<IUserRepository> userRepository;
        private PlaneModel plane;
        private PlaneModelDto planeDto;
        private Mapper mapper;

        [SetUp]
        public void SetUp()
        {
            mapper = new DtoProfile().GetMapper();
            planeDto = new PlaneModelDto() { Id = 1, Name = "Boeing 777", Description = "Good Plane", YearOfProd = DateTime.UtcNow };
            plane = mapper.Map<PlaneModel>(planeDto);
            partRepository = new Mock<IPlanePartRepository>();
            userRepository = new Mock<IUserRepository>();
        }

        [TearDown]
        public void TearDown()
        {
            mapper = null;
            partRepository = null;
            plane = null;
            planeDto = null;
            userRepository = null;
            planeService = null;
        }

        [Test]
        public void PlaneService_GetPlanes()
        {
            //Arrange
            var planeModelRepository = new Mock<IPlaneModelRepository>();
            planeModelRepository.Setup(a => a.GetAll()).Returns(new List<PlaneModel>() { plane });
            uow = new UnitOfWork(planeModelRepository.Object, userRepository.Object, partRepository.Object);
            planeService = new PlaneModelService(uow, new DtoProfile());
            //Act
            var planes = planeService.GetPlaneModels();
            //Assert
            Assert.NotNull(planes);
            Assert.IsNotEmpty(planes);
            Assert.AreEqual(plane.Id, planes.FirstOrDefault().Id);
            planeModelRepository.Verify(h => h.GetAll(), Times.Once());
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(11)]
        [TestCase(14)]
        [TestCase(22)]
        [TestCase(103)]
        public void PlaneService_GetPlane(int id)
        {
            //Arrange
            var planeModelRepository = new Mock<IPlaneModelRepository>();
            planeModelRepository.Setup(a => a.GetPlaneModel(It.Is<int>(x => x == id))).Returns(plane);
            uow = new UnitOfWork(planeModelRepository.Object, userRepository.Object, partRepository.Object);
            planeService = new PlaneModelService(uow, new DtoProfile());
            //Act
            var getPlane = planeService.GetPlaneModel(id);
            //Assert
            Assert.NotNull(getPlane);
            Assert.AreEqual(plane.Name, getPlane.Name);
            planeModelRepository.Verify(h => h.GetPlaneModel(It.Is<int>(x => x == id)), Times.Once());
        }

        [Test]
        public void PlaneService_CreatePlane_IsInvoked()
        {
            //Arrange
            var planeModelRepository = new Mock<IPlaneModelRepository>();
            planeModelRepository.Setup(a => a.CreatePlaneModel(It.Is<PlaneModel>(x => x == plane))).Verifiable();
            uow = new UnitOfWork(planeModelRepository.Object, userRepository.Object, partRepository.Object);
            planeService = new PlaneModelService(uow, new DtoProfile());
            //Act
            planeService.CreatePlaneModel(planeDto);

            //Assert
            planeModelRepository.Verify(h => h.CreatePlaneModel(It.Is<PlaneModel>(r => r != null)), Times.Once());
        }

        [Test]
        public void PlaneService_DeletePlane_IsInvoked()
        {
            //Arrange
            var planeModelRepository = new Mock<IPlaneModelRepository>();
            planeModelRepository.Setup(a => a.DeletePlaneModel(It.Is<PlaneModel>(x => x == plane))).Verifiable();
            uow = new UnitOfWork(planeModelRepository.Object, userRepository.Object, partRepository.Object);
            planeService = new PlaneModelService(uow, new DtoProfile());
            //Act
            planeService.DeletePlaneModel(planeDto);
            //Assert
            planeModelRepository.Verify(h => h.DeletePlaneModel(It.Is<PlaneModel>(r => r != null)), Times.Once());

        }

        [Test]
        public void PlaneService_UpdatePlane_IsInvoked()
        {
            //Arrange
            var planeModelRepository = new Mock<IPlaneModelRepository>();
            planeModelRepository.Setup(a => a.UpdatePlaneModel(It.Is<PlaneModel>(x => x == plane))).Verifiable();
            uow = new UnitOfWork(planeModelRepository.Object, userRepository.Object, partRepository.Object);
            planeService = new PlaneModelService(uow, new DtoProfile());
            //Act
            planeService.UpdatePlaneModel(planeDto);
            //Assert
            planeModelRepository.Verify(h => h.UpdatePlaneModel(It.Is<PlaneModel>(r => r != null)), Times.Once());
        }

        [Test]
        public void PlaneService_UpdatePlane_Exception()
        {
            //Arrange
            var planeModelRepository = new Mock<IPlaneModelRepository>();
            planeModelRepository.Setup(a => a.UpdatePlaneModel(null)).Throws(new ArgumentNullException("part", "Null argument was passed as a parameter")).Verifiable();
            uow = new UnitOfWork(planeModelRepository.Object, userRepository.Object, partRepository.Object);
            planeService = new PlaneModelService(uow, new DtoProfile());
            //Act

            planeService.UpdatePlaneModel(null);

            //Assert
            
            var ex = Assert.Throws<ArgumentNullException>(() => planeModelRepository.Object.UpdatePlaneModel(null));
            Assert.That(ex.Message, Is.EqualTo("Null argument was passed as a parameter (Parameter 'part')"));
        }

        [Test]
        public void PlaneService_CreateEntity_Exception()
        {
            //Arrange
            var planeModelRepository = new Mock<IPlaneModelRepository>();
            planeModelRepository.Setup(a => a.CreatePlaneModel(null)).Throws(new ArgumentNullException("part", "Null argument was passed as a parameter")).Verifiable();
            uow = new UnitOfWork(planeModelRepository.Object, userRepository.Object, partRepository.Object);
            planeService = new PlaneModelService(uow, new DtoProfile());
            //Act

            planeService.CreatePlaneModel(null);

            //Assert

            var ex = Assert.Throws<ArgumentNullException>(() => planeModelRepository.Object.CreatePlaneModel(null));
            Assert.That(ex.Message, Is.EqualTo("Null argument was passed as a parameter (Parameter 'part')"));
        }


        [TestCase(1)]
        [TestCase(5)]
        [TestCase(11)]
        [TestCase(14)]
        [TestCase(22)]
        [TestCase(103)]
        public void PlaneService_GetEntity_Null(int id)
        {
            //Arrange
            var planeModelRepository = new Mock<IPlaneModelRepository>();
            planeModelRepository.Setup(a => a.GetPlaneModel(id)).Throws(new ArgumentNullException("part", "Null argument was passed as a parameter")).Verifiable();
            uow = new UnitOfWork(planeModelRepository.Object, userRepository.Object, partRepository.Object);
            planeService = new PlaneModelService(uow, new DtoProfile());
            //Act

            var plane = planeService.GetPlaneModel(id);

            //Assert
            Assert.IsNull(plane);
            planeModelRepository.Verify(h => h.GetPlaneModel(It.Is<int>(x => x == id)), Times.Once());
        }

        [Test]
        public void PlaneService_GetPlanes_IsEmpty()
        {
            //Arrange
            var planeModelRepository = new Mock<IPlaneModelRepository>();
            planeModelRepository.Setup(a => a.GetAll()).Throws(new ArgumentNullException("plane", "Null argument was passed as a parameter")).Verifiable();
            uow = new UnitOfWork(planeModelRepository.Object, userRepository.Object, partRepository.Object);
            planeService = new PlaneModelService(uow, new DtoProfile());
            //Act
            var planes = planeService.GetPlaneModels();
            //Assert
            Assert.NotNull(planes);
            Assert.IsEmpty(planes);
            planeModelRepository.Verify(h => h.GetAll(), Times.Once());
        }

        [Test]
        public void PlaneService_DeletePlane_Exception()
        {
            //Arrange
            var planeModelRepository = new Mock<IPlaneModelRepository>();
            planeModelRepository.Setup(a => a.DeletePlaneModel(It.Is<PlaneModel>(x => x == null))).Throws(new ArgumentNullException("plane", "Null argument was passed as a parameter")).Verifiable();
            uow = new UnitOfWork(planeModelRepository.Object, userRepository.Object, partRepository.Object);
            planeService = new PlaneModelService(uow, new DtoProfile());
            //Act
            planeService.DeletePlaneModel(null);
            //Assert
            
            var ex = Assert.Throws<ArgumentNullException>(() => planeModelRepository.Object.DeletePlaneModel(null));
            Assert.That(ex.Message, Is.EqualTo("Null argument was passed as a parameter (Parameter 'plane')"));

        }
    }
}
