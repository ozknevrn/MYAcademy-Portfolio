using Microsoft.AspNetCore.Mvc;
using MYAcademey_Portfollo.Context;
using MYAcademey_Portfollo.Entities;

namespace MYAcademey_Portfollo.Controllers
{
    public class DefaultController : Controller
    {
        private readonly PortfolioContext _context;

        public DefaultController(PortfolioContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendMessage(Message message)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                    .ToDictionary(x => x.Key, x => x.Value!.Errors.First().ErrorMessage);

                return Json(new { success = false, errors });
            }

            message.SendDate = DateTime.Now;
            message.IsRead = false;
            _context.Messages.Add(message);
            _context.SaveChanges();

            return Json(new { success = true, message = "Mesajınız alındı. En kısa sürede dönüş yapacağım." });
        }
    }
}
