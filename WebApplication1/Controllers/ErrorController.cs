using Microsoft.AspNetCore.Mvc;

namespace HealthInsurance.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NotFound()
        {
            return View(); // Returns Views/Error/NotFound.cshtml
        }
    }
}
