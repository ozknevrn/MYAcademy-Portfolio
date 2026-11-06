using Microsoft.AspNetCore.Mvc;

namespace MYAcademey_Portfollo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
