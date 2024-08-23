using Microsoft.AspNetCore.Mvc;
using ohms.Models.LandingPage;
using ohms.Models;
using System.Diagnostics;

namespace ohms.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Policies = new List<Policy>
    {
        new Policy { PolicyName = "Health Plus", Description = "Comprehensive health coverage.", Amount = 500 },
        // Add more policies
    };

            ViewBag.Companies = new List<Company>
    {
        new Company { CompanyName = "HealthCare Inc.", Address = "123 Main St", Phone = "123-456-7890", CompanyURL = "http://healthcare.com" },
        // Add more companies
    };

            ViewBag.Testimonials = new List<Testimonial>
    {
        new Testimonial { ClientName = "John Doe", Comment = "Excellent service and coverage!" },
        // Add more testimonials
    };

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
