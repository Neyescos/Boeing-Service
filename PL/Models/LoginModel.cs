using System.ComponentModel.DataAnnotations;

namespace PL.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is empty")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Password is empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
