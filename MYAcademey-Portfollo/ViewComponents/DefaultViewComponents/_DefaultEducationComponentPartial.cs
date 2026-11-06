using Microsoft.AspNetCore.Mvc;
using MYAcademey_Portfollo.Context;

namespace MYAcademey_Portfollo.ViewComponents.DefaultViewComponents
{
    public class _DefaultEducationComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultEducationComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = _context.Educations
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.EducationId)
                .ToList();
            return View(values);
        }
    }
}
