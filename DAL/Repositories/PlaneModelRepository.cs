using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class PlaneModelRepository : IPlaneModelRepository
    {
        private readonly BoengServiceWebSiteContext _context;
        public PlaneModelRepository(BoengServiceWebSiteContext context)
        {
            _context = context;
        }
        public void CreatePlaneModel(PlaneModel planeModel)
        {
            _context.PlaneModels.Add(planeModel);
            Save();
        }

        public void DeletePlaneModel(PlaneModel planeModel)
        {
            _context.PlaneModels.Remove(planeModel);
            Save();
        }

        public IEnumerable<PlaneModel> GetAll()
        {
            return _context.PlaneModels
                .Include(p=>p.Users)
                .Include(p=>p.PlaneParts).AsNoTracking()
                .ToList();
        }

        public PlaneModel GetPlaneModel(int PlaneId)
        {
            return _context.PlaneModels.Find(PlaneId);
        }


        public void UpdatePlaneModel(PlaneModel planeModel)
        {
            _context.PlaneModels.Update(planeModel);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
