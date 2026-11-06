using Microsoft.AspNetCore.Mvc;
using MYAcademey_Portfollo.Context;
using MYAcademey_Portfollo.Entities;

namespace MYAcademey_Portfollo.Areas.Admin.Controllers
{
    // Sitede ana sayfa, iletişim bölümü ve footer'da gösterilen sosyal medya bağlantıları
    [Area("Admin")]
    public class SocialMediaController : Controller
    {
        private readonly PortfolioContext _context;

        public SocialMediaController(PortfolioContext context)
        {
            _context = context;
        }

        public IActionResult SocialMediaList()
        {
            var values = _context.SocialMedias.OrderBy(x => x.SocialMediaId).ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateSocialMedia()
        {
            ViewBag.FormTitle = "Yeni Sosyal Medya Bağlantısı";
            return View("SocialMediaForm", new SocialMedia());
        }

        [HttpPost]
        public IActionResult CreateSocialMedia(SocialMedia socialMedia)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.FormTitle = "Yeni Sosyal Medya Bağlantısı";
                return View("SocialMediaForm", socialMedia);
            }

            _context.SocialMedias.Add(socialMedia);
            _context.SaveChanges();
            TempData["Success"] = "Bağlantı eklendi.";
            return RedirectToAction("SocialMediaList");
        }

        [HttpGet]
        public IActionResult UpdateSocialMedia(int id)
        {
            var value = _context.SocialMedias.Find(id);
            if (value == null)
            {
                return RedirectToAction("SocialMediaList");
            }

            ViewBag.FormTitle = "Bağlantıyı Güncelle";
            return View("SocialMediaForm", value);
        }

        [HttpPost]
        public IActionResult UpdateSocialMedia(SocialMedia socialMedia)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.FormTitle = "Bağlantıyı Güncelle";
                return View("SocialMediaForm", socialMedia);
            }

            _context.SocialMedias.Update(socialMedia);
            _context.SaveChanges();
            TempData["Success"] = "Bağlantı güncellendi.";
            return RedirectToAction("SocialMediaList");
        }

        public IActionResult DeleteSocialMedia(int id)
        {
            var value = _context.SocialMedias.Find(id);
            if (value != null)
            {
                _context.SocialMedias.Remove(value);
                _context.SaveChanges();
                TempData["Success"] = "Bağlantı silindi.";
            }
            return RedirectToAction("SocialMediaList");
        }
    }
}
