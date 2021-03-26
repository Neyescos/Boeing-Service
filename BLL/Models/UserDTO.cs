using System;
using System.Collections.Generic;

namespace BLL.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int Role { get; set; }
        public virtual ICollection<PlaneModelDto> PlaneModels { get; set; }
    }
}
