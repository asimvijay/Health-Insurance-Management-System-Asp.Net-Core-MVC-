using HealthInsurance.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HealthInsurance.Models
{
    public class PoliciesOnEmployeesDto
    {
        [Key]
        public int Id { get; set; }

        public int EmpNo { get; set; }

        public int PolicyId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal EMI { get; set; }
    }
}
