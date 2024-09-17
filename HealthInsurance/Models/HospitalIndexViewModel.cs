using System.Collections.Generic;

namespace HealthInsurance.Entities { 
public class HospitalIndexViewModel
    {
        public IEnumerable<HospitalInfo> Hospitals { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchString { get; set; }
        public string SortOrder { get; set; }
    }
}