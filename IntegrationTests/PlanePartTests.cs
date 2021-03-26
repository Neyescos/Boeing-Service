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
    class PlanePartTests
    {
        private IPlanePartRepository repository;
        private PlanePart part;

        [SetUp]
        public void SetUp()
        {
            repository = new PlanePartRepository(new BoengServiceWebSiteContext());
            part = new PlanePart()
            {
                Name = "New Part",
                Description = "Part Description",
                Manufacturer = "Rolls-Royce",
                Price = 2000,
                SerialNumber = "0000000",
                PlaneModelId = 2002,
                ManufacturingDate = DateTime.UtcNow
            };
            PartRepository_CreatePlane(part);
        }
        
        public void PartRepository_CreatePlane(PlanePart part)
        {
            repository.CreatePlanePart(part);
            var createdPart = repository.GetAll().FirstOrDefault(u => u.Name == part.Name);
            part = createdPart;
        }

        [Test]
        public void PartRepository_GetPart()
        {
            //Arrange
            //Act
            var getPart =repository.GetPlanePart(part.Id);
            //Assert
            Assert.NotNull(getPart);
            Assert.AreEqual(part.Id, getPart.Id);
        }

        [Test]
        public void PartRepository_GetParts()
        {
            //Arrange
            //Act
            var parts = repository.GetAll();
            //Assert
            Assert.NotNull(parts);
            Assert.IsNotEmpty(parts);

        }
        
        [Test]
        public void PartRepository_DeletePart()
        {
            //Arrange
            //Act
            repository.DeletePlanePart(part);
            var getPart = repository.GetPlanePart(part.Id);
            //Assert
            Assert.Null(getPart);
        }
    }
}
