using Microsoft.AspNetCore.Mvc;

namespace MYAcademey_Portfollo.Areas.Admin.ViewComponents.AdminLayoutViewComponents
{
    public class _AdminLayoutStyleComponentPartial : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();  
        }
    }
}
