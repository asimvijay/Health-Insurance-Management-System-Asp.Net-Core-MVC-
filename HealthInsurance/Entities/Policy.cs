using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsurance.Entities
{
    public class Policy
    {
        // Constructor to initialize the collection
        public Policy()
        {
            PoliciesOnEmployees = new List<PoliciesOnEmployees>();
        }

        [Key]
        public int PolicyId { get; set; }

        [Required]
        [StringLength(50)]
        public string PolicyName { get; set; }

        [StringLength(250)]
        public string PolicyDesc { get; set; }

        [DataType(DataType.Currency)]
        public decimal PolicyAmount { get; set; }

        [DataType(DataType.Currency)]
        public decimal EMI { get; set; }

        public int CompanyId { get; set; }

        // Navigation property for the company
        [ForeignKey(nameof(CompanyId))]
        public CompanyDetails Company { get; set; }

        [StringLength(50)]
        public string Medicaid { get; set; }

        // Navigation property for policies on employees
        public ICollection<PoliciesOnEmployees> PoliciesOnEmployees { get; set; }
    }
}
