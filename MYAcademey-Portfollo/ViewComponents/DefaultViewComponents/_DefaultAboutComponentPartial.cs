using Microsoft.AspNetCore.Mvc;
using MYAcademey_Portfollo.Context;

namespace MYAcademey_Portfollo.ViewComponents.DefaultViewComponents
{
    public class _DefaultAboutComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultAboutComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var value = _context.Abouts.FirstOrDefault();

            // İstatistik alanı: öne çıkan bilgiler HeroSection'dan, proje sayısı Projects tablosundan gelir.
            ViewBag.Hero = _context.HeroSections.FirstOrDefault();
            ViewBag.ActiveProjectCount = _context.Projects.Count(x => x.IsActive);

            return View(value);
        }
    }
}
