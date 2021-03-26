using DAL.Entities;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IPlaneModelRepository
    {
        public void UpdatePlaneModel(PlaneModel planeModel);
        public void DeletePlaneModel(PlaneModel planeModel);
        public void CreatePlaneModel(PlaneModel planeModel);
        public PlaneModel GetPlaneModel(int PlaneId);
        public IEnumerable<PlaneModel> GetAll();
        public void Save();
    }
}

