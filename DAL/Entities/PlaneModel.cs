using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Entities
{
    public partial class PlaneModel
    { 

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime YearOfProd { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }

        public virtual ICollection<PlanePart> PlaneParts { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
