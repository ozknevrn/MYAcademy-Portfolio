using Microsoft.AspNetCore.Mvc;

namespace MYAcademey_Portfollo.ViewComponents.DefaultViewComponents
{
    public class _DefaultNavbarComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();

        }
    }

}
