using Microsoft.AspNetCore.Mvc;

namespace MYAcademey_Portfollo.ViewComponents.DefaultViewComponents
{
    public class _DefaultHeaderComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
