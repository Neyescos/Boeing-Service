using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories
{
    public class PlanePartRepository : IPlanePartRepository
    {
        private readonly BoengServiceWebSiteContext _context;
        public PlanePartRepository(BoengServiceWebSiteContext context)
        {
            _context = context;
        }
        public void CreatePlanePart(PlanePart planePart)
        {
            _context.PlaneParts.Add(planePart);
            Save();
        }

        public void DeletePlanePart(PlanePart planePart)
        {
            _context.PlaneParts.Remove(planePart);
            Save();
        }

        public IEnumerable<PlanePart> GetAll()
        {
            return _context.PlaneParts.Include(p=>p.PlaneModel).ToList();
        }

        public PlanePart GetPlanePart(int planePartId)
        {
            return _context.PlaneParts.Find(planePartId);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdatePlanePart(PlanePart planePart)
        {
            _context.PlaneParts.Update(planePart);
            Save();
        }
    }
}
