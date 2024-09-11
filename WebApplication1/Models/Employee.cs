namespace WebApplication1.Models
{
    public class Employee : UserActivity
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public string LastName { get; set; }

        public int PhoneNumber { get; set; }

        public string Email { get; set; }
        public string Department { get; set; }

        public string Designation { get; set; }


    }
}
