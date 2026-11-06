using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademey_Portfollo.Context;

namespace MYAcademey_Portfollo.ViewComponents.DefaultViewComponents
{
    public class _DefaultSkillComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultSkillComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = _context.Skills
                .Include(x => x.SkillItems)
                .OrderBy(x => x.SkillId)
                .ToList();
            return View(values);
        }
    }
}
