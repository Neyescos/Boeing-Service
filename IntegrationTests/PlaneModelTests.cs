using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntegrationTests
{
    [TestFixture]
    class PlaneModelTests
    {
        private IPlaneModelRepository repository;
        private PlaneModel plane;
        [SetUp]
        public void SetUp()
        {
            repository = new PlaneModelRepository(new BoengServiceWebSiteContext());
            plane = new PlaneModel() { Name = "Not Boeing", Description = "Totally not boeing", YearOfProd = DateTime.UtcNow };
            PlaneRepository_CreatePlane(plane);
        }


        public void PlaneRepository_CreatePlane(PlaneModel plane)
        {
            repository.CreatePlaneModel(plane);
            var createdPlane = repository.GetAll().FirstOrDefault(u => u.Name == plane.Name);
            plane = createdPlane;
        }

        [Test]
        public void PlaneRepository_GetPlane()
        {
            //Arrange
            //Act
            var getPlane = repository.GetPlaneModel(plane.Id);
            //Assert
            Assert.IsNotNull(getPlane);
            Assert.AreEqual(plane.Id, getPlane.Id);
        }

        [Test]
        public void PlaneRepository_GetPlanes()
        {
            //Arrange
            //Act
            var planes = repository.GetAll();
            //Assert
            Assert.IsNotNull(planes);
            Assert.IsNotEmpty(planes);
        }

        [Test]
        public void PlaneRepository_DeletePlane()
        {
            //Arrange
            //Act
            repository.DeletePlaneModel(plane);
            var getPlane = repository.GetPlaneModel(plane.Id);
            //Assert
            Assert.Null(getPlane);
        }
    }
}
