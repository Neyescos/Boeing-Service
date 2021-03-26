using System;
using System.Collections.Generic;

namespace PL.Models
{
    public class PlaneModelViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ProductionYear { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }

        public virtual ICollection<PlanePartViewModel> PlaneParts { get; set; }
        public virtual ICollection<UserViewModel> Users { get; set; }
    }
}
