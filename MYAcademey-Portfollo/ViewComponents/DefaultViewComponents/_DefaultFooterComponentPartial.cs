using Microsoft.AspNetCore.Mvc;
using MYAcademey_Portfollo.Context;

namespace MYAcademey_Portfollo.ViewComponents.DefaultViewComponents
{
    public class _DefaultFooterComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultFooterComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.Name = _context.HeroSections.Select(x => x.Greeting).FirstOrDefault();
            var values = _context.SocialMedias.OrderBy(x => x.SocialMediaId).ToList();
            return View(values);
        }
    }
}
