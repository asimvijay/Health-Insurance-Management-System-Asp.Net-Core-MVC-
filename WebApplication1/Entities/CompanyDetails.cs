using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthInsurance.Entities
{
    public class CompanyDetails
    {
        public CompanyDetails()
        {
            Employees = new List<EmpRegister>();
            Policies = new List<Policy>();
        }

        [Key]
        public int CompanyId { get; set; }

        [Required]
        [StringLength(50)]
        public string CompanyName { get; set; }

        [StringLength(150)]
        public string Address { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string CompanyURL { get; set; }

        public ICollection<EmpRegister> Employees { get; set; }
        public ICollection<Policy> Policies { get; set; }
    }
}
