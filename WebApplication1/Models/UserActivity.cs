namespace WebApplication1.Models
{
    public class UserActivity
    {
        public int? CreatedById { get; set; }

        public DateTime CreatedON { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime ModifiedON { get; set; }

    }

    public class ApprovalActivity: UserActivity
    {
        public int? ApprovedById { get; set; }

        public DateTime ApprovedON { get; set; }

       

    }
}
