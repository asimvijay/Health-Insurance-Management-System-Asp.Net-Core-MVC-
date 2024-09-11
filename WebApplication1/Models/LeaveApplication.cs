namespace WebApplication1.Models
{
    public class LeaveApplication: ApprovalActivity
    {
        public int id { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public int NoOfDays { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int DurationID { get; set; }


        public SystemCodeDetail Duration { get; set; }

        public int LeaveTypeID { get; set; }

        public LeavesType leavesType { get; set; }


        public string Attachment { get; set; }

        public  string discription { get; set; }

        public int StatusId { get; set; }

        public SystemCodeDetail Status { get; set; }
    }
}
