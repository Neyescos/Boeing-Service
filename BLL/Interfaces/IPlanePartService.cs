using BLL.Models;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IPlanePartService
    {
        public void CreatePlanePart(PlanePartDto planePart);
        public void DeletePlanePart(PlanePartDto planePart);
        public void UpdatePlanePart(PlanePartDto planePart);
        public PlanePartDto GetPlanePart(int planePartId);
        public IEnumerable<PlanePartDto> GetPlaneParts();
    }
}
