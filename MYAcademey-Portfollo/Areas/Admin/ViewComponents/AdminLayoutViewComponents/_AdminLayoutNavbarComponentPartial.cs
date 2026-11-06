using Microsoft.AspNetCore.Mvc;
using MYAcademey_Portfollo.Context;

namespace MYAcademey_Portfollo.Areas.Admin.ViewComponents.AdminLayoutViewComponents
{
    public class _AdminLayoutNavbarComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _AdminLayoutNavbarComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Bildirim zilinin yanındaki nokta sadece okunmamış mesaj varsa görünür
            ViewBag.UnreadMessageCount = _context.Messages.Count(x => !x.IsRead);
            return View();
        }
    }
}
