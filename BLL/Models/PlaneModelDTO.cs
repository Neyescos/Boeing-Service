using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class PlaneModelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime YearOfProd { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }

        public virtual ICollection<PlanePartDto> PlaneParts { get; set; }
        public virtual ICollection<UserDto> Users { get; set; }
    }
}
