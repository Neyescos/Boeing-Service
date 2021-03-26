using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;
using System.Collections.Generic;

namespace BLL.Services
{
    public class PlaneModelService : IPlaneModelService
    {
        private readonly IUnitOfWork _unit;
        private readonly Mapper _mapper;

        public PlaneModelService(IUnitOfWork unitOfWork, IMapperProfile mapperProfile)
        {
            _unit = unitOfWork;
            _mapper = mapperProfile.GetMapper();
        }

        public void CreatePlaneModel(PlaneModelDto planeModel)
        {
            try
            {
                _unit.Planes.CreatePlaneModel(_mapper.Map<PlaneModel>(planeModel));

            }catch
            {
                //catch just not to stop the program
            }
        }

        public void DeletePlaneModel(PlaneModelDto planeModel)
        {
            try
            {
                _unit.Planes.DeletePlaneModel(_mapper.Map<PlaneModel>(planeModel));
            }catch
            {
                //catch just not to stop the program
            }
        }

        public PlaneModelDto GetPlaneModel(int planeModelId)
        {
            try
            {
                return _mapper.Map<PlaneModelDto>(_unit.Planes.GetPlaneModel(planeModelId));
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<PlaneModelDto> GetPlaneModels()
        {
            try
            {
                return _mapper.Map<IEnumerable<PlaneModelDto>>(_unit.Planes.GetAll());
            }
            catch
            {
                return new List<PlaneModelDto>();
            }
        }

        public void UpdatePlaneModel(PlaneModelDto planeModel)
        {
            try
            {
                _unit.Planes.UpdatePlaneModel(_mapper.Map<PlaneModel>(planeModel));
            }
            catch
            {
                //catch just not to stop the program
            }
        }
    }
}
