using Microsoft.AspNetCore.Mvc;
using MYAcademey_Portfollo.Context;

namespace MYAcademey_Portfollo.Areas.Admin.ViewComponents.AdminLayoutViewComponents
{
    public class _AdminLayoutSidebarComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _AdminLayoutSidebarComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Menüdeki "Gelen Mesajlar" yanında gösterilecek okunmamış mesaj sayısı
            ViewBag.UnreadMessageCount = _context.Messages.Count(x => !x.IsRead);
            return View();
        }
    }
}
