using System;
using System.ComponentModel.DataAnnotations;

namespace HealthInsurance.Models
{
    public class EmpRegistrationViewModel
    {
        [Required(ErrorMessage = "Designation is required.")]
        [StringLength(100, ErrorMessage = "Designation can't be longer than 100 characters.")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Join date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime JoinDate { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username can't be longer than 50 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string Password { get; set; }

        [StringLength(250, ErrorMessage = "Address can't be longer than 250 characters.")]
        public string Address { get; set; }

        [StringLength(20, ErrorMessage = "Contact number can't be longer than 20 characters.")]
        public string ContactNo { get; set; }

        [StringLength(50, ErrorMessage = "State can't be longer than 50 characters.")]
        public string State { get; set; }

        [StringLength(50, ErrorMessage = "Country can't be longer than 50 characters.")]
        public string Country { get; set; }

        [StringLength(50, ErrorMessage = "City can't be longer than 50 characters.")]
        public string City { get; set; }

        [StringLength(50, ErrorMessage = "Policy status can't be longer than 50 characters.")]
        public string PolicyStatus { get; set; }

        [StringLength(50, ErrorMessage = "Policy ID can't be longer than 50 characters.")]
        public string PolicyId { get; set; }
    }
}
