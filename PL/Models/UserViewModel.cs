using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PL.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int Role { get; set; }
        public List<string> PlaneModelIds { get; set; }
        public virtual ICollection<PlaneModelViewModel> PlaneModels { get; set; }
    }
}
