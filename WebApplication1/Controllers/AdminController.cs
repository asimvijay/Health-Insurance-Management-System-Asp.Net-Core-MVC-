using Microsoft.AspNetCore.Mvc;

namespace HealthInsurance.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
