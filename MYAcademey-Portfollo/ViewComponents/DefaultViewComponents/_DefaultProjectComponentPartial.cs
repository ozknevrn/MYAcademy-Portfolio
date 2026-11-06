using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademey_Portfollo.Context;

namespace MYAcademey_Portfollo.ViewComponents.DefaultViewComponents
{
    public class _DefaultProjectComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultProjectComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Sadece aktif projeler, en yeni proje en başta
            var values = _context.Projects
                .Include(x => x.Category)
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            // Filtre butonları: sadece aktif projesi olan kategoriler
            ViewBag.Categories = values
                .Where(x => x.Category != null)
                .Select(x => x.Category!)
                .GroupBy(x => x.CategoryId)
                .Select(g => g.First())
                .OrderBy(x => x.CategoryName)
                .ToList();

            return View(values);
        }
    }
}
