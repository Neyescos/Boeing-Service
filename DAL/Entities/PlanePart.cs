using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Entities
{
    public partial class PlanePart
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public string Manufacturer { get; set; }
        public int PlaneModelId { get; set; }

        public virtual PlaneModel PlaneModel { get; set; }
    }
}
