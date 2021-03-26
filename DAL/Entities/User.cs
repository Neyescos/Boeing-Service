using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Entities
{
    public partial class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Password { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int Role { get; set; }
        public virtual ICollection<PlaneModel> PlaneModels { get; set; }
    }
}
