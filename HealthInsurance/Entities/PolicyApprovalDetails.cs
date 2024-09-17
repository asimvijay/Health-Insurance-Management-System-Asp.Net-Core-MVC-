using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsurance.Entities
{
    public class PolicyApprovalDetails
    {
        [Key]
        public int Id { get; set; }

        public int PolicyId { get; set; }
        [ForeignKey(nameof(PolicyId))]
        public Policy Policy { get; set; }

        public DateTime Date { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public bool Approved { get; set; }

        [StringLength(50)]
        public string Reason { get; set; }
    }
}
