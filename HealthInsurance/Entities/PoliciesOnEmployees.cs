using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsurance.Entities
{
    public class PoliciesOnEmployees
    {
        [Key]
        public int Id { get; set; }

        public int EmpNo { get; set; }
        [ForeignKey(nameof(EmpNo))]
        public EmpRegister Employee { get; set; }

        public int PolicyId { get; set; }
        [ForeignKey(nameof(PolicyId))]
        public Policy Policy { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal EMI { get; set; }
    }
}
