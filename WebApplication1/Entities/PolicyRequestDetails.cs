using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsurance.Entities
{
    public class PolicyRequestDetails
    {
        [Key]
        public int RequestId { get; set; }

        public DateTime RequestDate { get; set; }

        public int EmpNo { get; set; }
        [ForeignKey(nameof(EmpNo))]
        public EmpRegister Employee { get; set; }

        public int PolicyId { get; set; }
        [ForeignKey(nameof(PolicyId))]
        public Policy Policy { get; set; }

        public decimal PolicyAmount { get; set; }
        public decimal EMI { get; set; }

        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public CompanyDetails Company { get; set; }

        [StringLength(50)]
        public string Status { get; set; }
    }
}
