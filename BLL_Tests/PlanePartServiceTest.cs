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
    public class PlanePartServiceTest
    {
        private UnitOfWork uow;
        private PlanePartService partService;
        private Mock<IPlaneModelRepository> planeRepository;
        private Mock<IUserRepository> userRepository;
        private PlanePart part;
        private PlanePartDto partDto;
        private Mapper mapper;

        [SetUp]
        public void SetUp()
        {
            partDto = new PlanePartDto()
            {
                Id = 1,
                Description = "Some discription",
                Manufacturer = "The Boeing Company",
                ManufacturingDate = DateTime.UtcNow,
                Name = "Boeing 777 MAX",
                Price = 300
            };
            mapper = new DtoProfile().GetMapper();
            part = mapper.Map<PlanePart>(partDto);
            planeRepository = new Mock<IPlaneModelRepository>();
            userRepository = new Mock<IUserRepository>();

        }

        [TearDown]
        public void TearDown()
        {
            mapper = null;
            planeRepository = null;
            part = null;
            partDto = null;
            userRepository = null;
            partService = null;
        }

        [Test]
        public void PlaneService_GetAll()
        {
            //Arrange
            var planePartRepository = new Mock<IPlanePartRepository>();
            planePartRepository.Setup(a => a.GetAll()).Returns(new List<PlanePart>() { part });
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            partService = new PlanePartService(uow, new DtoProfile());
            //Act
            var parts = partService.GetPlaneParts();
            //Assert
            Assert.NotNull(parts);
            Assert.IsNotEmpty(parts);
            Assert.AreEqual(part.Id, parts.FirstOrDefault().Id);
            planePartRepository.Verify(a => a.GetAll(), Times.Once());
        }


        [TestCase(1)]
        [TestCase(5)]
        [TestCase(11)]
        [TestCase(14)]
        [TestCase(22)]
        [TestCase(103)]
        public void PlaneService_GetEntity(int id)
        {
            //Arrange
            var planePartRepository = new Mock<IPlanePartRepository>();
            planePartRepository.Setup(a => a.GetPlanePart(It.Is<int>(x => x == id))).Returns(part);
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            partService = new PlanePartService(uow, new DtoProfile());
            //Act
            var getPart = partService.GetPlanePart(id);
            //Assert
            Assert.NotNull(getPart);
            Assert.AreEqual(partDto.Name, getPart.Name);
            planePartRepository.Verify(a => a.GetPlanePart(It.Is<int>(r => r == id)), Times.Once());
        }

        [Test]
        public void PlaneService_CreateEntity_IsInvoked()
        {
            //Arrange
            var planePartRepository = new Mock<IPlanePartRepository>();
            planePartRepository.Setup(a => a.CreatePlanePart(It.Is<PlanePart>(x => x == part))).Verifiable();
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            partService = new PlanePartService(uow, new DtoProfile());
            //Act
            partService.CreatePlanePart(partDto);

            //Assert
            planePartRepository.Verify(a => a.CreatePlanePart(It.Is<PlanePart>(r => r != null)), Times.Once());
        }

        [Test]
        public void PlaneService_DeleteEntity_IsInvoked()
        {

            //Arrange
            var planePartRepository = new Mock<IPlanePartRepository>();
            planePartRepository.Setup(a => a.DeletePlanePart(It.Is<PlanePart>(x => x == part))).Verifiable();
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            partService = new PlanePartService(uow, new DtoProfile());
            //Act
            partService.DeletePlanePart(partDto);
            //Assert
            planePartRepository.Verify(h => h.DeletePlanePart(It.Is<PlanePart>(r => r != null)));
        }

        [Test]
        public void PlaneService_UpdateEntity_IsInvoked()
        {

            //Arrange
            var planePartRepository = new Mock<IPlanePartRepository>();
            planePartRepository.Setup(a => a.UpdatePlanePart(It.Is<PlanePart>(x => x == part))).Verifiable();
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            partService = new PlanePartService(uow, new DtoProfile());
            //Act
            partService.UpdatePlanePart(partDto);
            //Assert
            planePartRepository.Verify(h => h.UpdatePlanePart(It.Is<PlanePart>(r => r != null)));
        }

        [Test]
        public void PlaneService_UpdateEntity_Exception()
        {
            //Arrange
            var planePartRepository = new Mock<IPlanePartRepository>();
            planePartRepository.Setup(a => a.UpdatePlanePart(null)).Throws(new ArgumentNullException("part", "Null argument was passed as a parameter")).Verifiable();
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            partService = new PlanePartService(uow, new DtoProfile());
            //Act

            partService.UpdatePlanePart(null);

            //Assert

            var ex = Assert.Throws<ArgumentNullException>(() => planePartRepository.Object.UpdatePlanePart(null));
            Assert.That(ex.Message, Is.EqualTo("Null argument was passed as a parameter (Parameter 'part')"));
        }

        [Test]
        public void PlaneService_CreateEntity_Exception()
        {
            //Arrange
            var planePartRepository = new Mock<IPlanePartRepository>();
            planePartRepository.Setup(a => a.CreatePlanePart(null)).Throws(new ArgumentNullException("part", "Null argument was passed as a parameter")).Verifiable();
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            partService = new PlanePartService(uow, new DtoProfile());
            //Act

            partService.CreatePlanePart(null);

            //Assert

            var ex = Assert.Throws<ArgumentNullException>(() => planePartRepository.Object.CreatePlanePart(null));
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
            var planePartRepository = new Mock<IPlanePartRepository>();
            planePartRepository.Setup(a => a.GetPlanePart(It.Is<int>(x => x == id))).Throws(new ArgumentNullException("part", "Null argument was passed as a parameter")).Verifiable();
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            partService = new PlanePartService(uow, new DtoProfile());
            //Act
            var getPart = partService.GetPlanePart(id);
            //Assert
            Assert.Null(getPart);
            planePartRepository.Verify(a => a.GetPlanePart(It.Is<int>(r => r == id)), Times.Once());
        }

        [Test]
        public void PlaneService_GetEntities_Exception()
        {
            //Arrange
            var planePartRepository = new Mock<IPlanePartRepository>();
            planePartRepository.Setup(a => a.GetAll()).Throws(new Exception()).Verifiable();
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            partService = new PlanePartService(uow, new DtoProfile());
            //Act

            var parts = partService.GetPlaneParts();

            //Assert
            Assert.NotNull(parts);
            Assert.IsEmpty(parts);
        }

        [Test]
        public void PlaneService_DeleteEntity_Exception()
        {
            //Arrange
            var planePartRepository = new Mock<IPlanePartRepository>();
            planePartRepository.Setup(a => a.DeletePlanePart(null)).Throws(new ArgumentNullException("part", "Null argument was passed as a parameter")).Verifiable();
            uow = new UnitOfWork(planeRepository.Object, userRepository.Object, planePartRepository.Object);
            partService = new PlanePartService(uow, new DtoProfile());
            //Act

            partService.DeletePlanePart(null);

            //Assert

            var ex = Assert.Throws<ArgumentNullException>(() => planePartRepository.Object.DeletePlanePart(null));
            Assert.That(ex.Message, Is.EqualTo("Null argument was passed as a parameter (Parameter 'part')"));
        }
    }
}
