using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsurance.Entities
{
    public class EmpRegister
    {
        [Key]
        public int EmpNo { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [StringLength(50)]
        public string Designation { get; set; }

        public DateTime JoinDate { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(20)]
        public string ContactNo { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(30)]
        public string PolicyStatus { get; set; }

        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public CompanyDetails Company { get; set; }

        public ICollection<PoliciesOnEmployees> Policies { get; set; }
    }
}
