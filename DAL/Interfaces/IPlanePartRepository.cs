using DAL.Entities;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IPlanePartRepository
    {
        public void UpdatePlanePart(PlanePart planePart);
        public void DeletePlanePart(PlanePart planePart);
        public void CreatePlanePart(PlanePart planePart);
        public PlanePart GetPlanePart(int planePartId);
        public IEnumerable<PlanePart> GetAll();
        public void Save();
    }
}
