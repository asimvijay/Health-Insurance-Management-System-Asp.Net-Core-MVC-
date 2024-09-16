using System.ComponentModel.DataAnnotations;

namespace HealthInsurance.Entities
{
    public class PolicyTotalDescription
    {
        [Key]
        public int PolicyId { get; set; }

        [StringLength(50)]
        public string PolicyName { get; set; }

        [StringLength(250)]
        public string PolicyDesc { get; set; }

        [DataType(DataType.Currency)]
        public decimal PolicyAmount { get; set; }

        [DataType(DataType.Currency)]
        public decimal EMI { get; set; }

        public int PolicyDurationInMonths { get; set; }

        [StringLength(50)]
        public string CompanyName { get; set; }

        [StringLength(50)]
        public string Medicaid { get; set; }
    }
}
