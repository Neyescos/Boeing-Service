using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;
using System.Collections.Generic;

namespace BLL.Services
{
    public class PlanePartService : IPlanePartService
    {
        private readonly IUnitOfWork _unit;
        private readonly Mapper _mapper;

        public PlanePartService(IUnitOfWork uow, IMapperProfile mapperProfile)
        {
            _mapper = mapperProfile.GetMapper();
            _unit = uow;
        }

        public void CreatePlanePart(PlanePartDto planePart)
        {
            try
            {
                _unit.Parts.CreatePlanePart(_mapper.Map<PlanePart>(planePart));
            }
            catch
            {
                //catch just not to stop the program
            }
        }

        public void DeletePlanePart(PlanePartDto planePart)
        {
            try
            {
                _unit.Parts.DeletePlanePart(_mapper.Map<PlanePart>(planePart));
            }
            catch
            {
                //catch just not to stop the program
            }
        }

        public PlanePartDto GetPlanePart(int planePartId)
        {
            try
            {
                return _mapper.Map<PlanePartDto>(_unit.Parts.GetPlanePart(planePartId));
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<PlanePartDto> GetPlaneParts()
        {
            try
            {
                return _mapper.Map<IEnumerable<PlanePartDto>>(_unit.Parts.GetAll());
            }
            catch
            {
                return new List<PlanePartDto>();
            }
        }

        public void UpdatePlanePart(PlanePartDto planePart)
        {
            try
            {
                _unit.Parts.UpdatePlanePart(_mapper.Map<PlanePart>(planePart));
            }
            catch
            {
                //catch just not to stop the program
            }
        }
    }
}
