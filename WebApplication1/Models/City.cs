namespace WebApplication1.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }

        public Country Country { get; set; }


    }
}
