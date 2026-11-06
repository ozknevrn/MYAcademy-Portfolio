using Microsoft.AspNetCore.Mvc;
using MYAcademey_Portfollo.Context;

namespace MYAcademey_Portfollo.ViewComponents.DefaultViewComponents
{
    public class _DefaultExperienceComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultExperienceComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Pasif deneyimler sitede gösterilmez
            var values = _context.Experiences
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.ExperienceId)
                .ToList();
            return View(values);
        }
    }
}
