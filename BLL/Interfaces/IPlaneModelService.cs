using BLL.Models;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IPlaneModelService
    {
        public void CreatePlaneModel(PlaneModelDto planeModel);
        public void DeletePlaneModel(PlaneModelDto planeModel);
        public void UpdatePlaneModel(PlaneModelDto planeModel);
        public PlaneModelDto GetPlaneModel(int planeModelId);
        public IEnumerable<PlaneModelDto> GetPlaneModels();
    }
}
