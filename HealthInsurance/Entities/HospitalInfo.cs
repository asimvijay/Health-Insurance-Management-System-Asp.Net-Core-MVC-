using System.ComponentModel.DataAnnotations;

namespace HealthInsurance.Entities
{
    public class HospitalInfo
    {
        [Key]
        public int HospitalId { get; set; }

        [Required]
        [StringLength(50)]
        public string HospitalName { get; set; }

        [StringLength(50)]
        public string PhoneNo { get; set; }

        [StringLength(50)]
        public string Location { get; set; }

        [StringLength(100)]
        public string URL { get; set; }
    }
}
