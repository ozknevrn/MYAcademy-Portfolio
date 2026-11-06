using Microsoft.AspNetCore.Mvc;
using MYAcademey_Portfollo.Context;

namespace MYAcademey_Portfollo.ViewComponents.DefaultViewComponents
{
    public class _DefaultHeroSectionComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultHeroSectionComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Kayıt yoksa (null) view boş değerlerle çalışır; sayfa hata vermez.
            var value = _context.HeroSections.FirstOrDefault();

            // Yazı makinesi efektinde dönecek unvanlar (sadece aktif olanlar, sıraya göre)
            ViewBag.HeroTitles = _context.HeroTitles
                .Where(x => x.IsActive)
                .OrderBy(x => x.Order)
                .Select(x => x.Title)
                .ToList();

            ViewBag.SocialMedias = _context.SocialMedias.OrderBy(x => x.SocialMediaId).ToList();

            return View(value);
        }
    }
}
