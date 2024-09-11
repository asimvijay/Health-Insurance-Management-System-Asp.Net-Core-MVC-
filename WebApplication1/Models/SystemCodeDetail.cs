using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class SystemCodeDetail:UserActivity
    {
        [Key]
        public int id { get; set; }
        public int SystemCodeId { get; set; }
        public SystemCode SystemCode { get; set; }

        public string Code { get; set; }

        public string  Discription { get; set; }

        public int? OderNo { get; set; }
    }
}
