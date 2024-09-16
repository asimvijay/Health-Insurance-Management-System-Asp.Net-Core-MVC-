using HealthInsurance.Entities;
using System.ComponentModel.DataAnnotations;

namespace HealthInsurance.Models
{
    public class AdminRegistrationViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username can't be longer than 50 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public Role Role { get; set; }
    }
}
