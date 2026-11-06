using Microsoft.AspNetCore.Mvc;
using MYAcademey_Portfollo.Context;
using MYAcademey_Portfollo.Entities;
using MYAcademey_Portfollo.Services;

namespace MYAcademey_Portfollo.Areas.Admin.Controllers
{
    // Ana sayfanın üst bölümü (isim, unvan, tanıtım, fotoğraf, öne çıkan bilgiler)
    // ve yazı makinesi efektiyle dönen unvanlar (HeroTitle) burada yönetilir.
    // Tek kayıtlı bir alan olduğu için ekleme/silme yerine güncelleme odaklıdır.
    [Area("Admin")]
    public class HeroSectionController : Controller
    {
        private readonly PortfolioContext _context;
        private readonly ImageUploadService _imageUploadService;

        public HeroSectionController(PortfolioContext context, ImageUploadService imageUploadService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
        }

        [HttpGet]
        public IActionResult UpdateHeroSection()
        {
            // Kayıt yoksa boş bir form açılır; kaydedilince ilk kayıt oluşur.
            var value = _context.HeroSections.FirstOrDefault() ?? new HeroSection();
            LoadHeroTitles();
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateHeroSection(HeroSection heroSection, IFormFile? imageFile)
        {
            string? newImagePath = null;
            if (imageFile != null)
            {
                var upload = await _imageUploadService.SaveAsync(imageFile, "hero");
                if (upload.Success)
                {
                    newImagePath = upload.Path;
                }
                else
                {
                    ModelState.AddModelError("imageFile", upload.ErrorMessage!);
                }
            }

            if (!ModelState.IsValid)
            {
                _imageUploadService.Delete(newImagePath);
                LoadHeroTitles();
                return View(heroSection);
            }

            if (newImagePath != null)
            {
                _imageUploadService.Delete(heroSection.MainImageUrl);
                heroSection.MainImageUrl = newImagePath;
            }

            if (heroSection.HeroSectionId == 0)
            {
                _context.HeroSections.Add(heroSection);
            }
            else
            {
                _context.HeroSections.Update(heroSection);
            }

            _context.SaveChanges();
            TempData["Success"] = "Ana sayfa bilgileri kaydedildi.";
            return RedirectToAction("UpdateHeroSection");
        }

        // ---------------- Dönen unvanlar (HeroTitle) ----------------

        [HttpPost]
        public IActionResult AddHeroTitle(HeroTitle heroTitle)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = ModelState.Values.SelectMany(x => x.Errors).First().ErrorMessage;
                return RedirectToAction("UpdateHeroSection");
            }

            heroTitle.IsActive = true;
            _context.HeroTitles.Add(heroTitle);
            _context.SaveChanges();
            TempData["Success"] = "Unvan eklendi.";
            return RedirectToAction("UpdateHeroSection");
        }

        public IActionResult ToggleHeroTitle(int id)
        {
            var value = _context.HeroTitles.Find(id);
            if (value != null)
            {
                value.IsActive = !value.IsActive;
                _context.SaveChanges();
                TempData["Success"] = "Unvan durumu güncellendi.";
            }
            return RedirectToAction("UpdateHeroSection");
        }

        public IActionResult DeleteHeroTitle(int id)
        {
            var value = _context.HeroTitles.Find(id);
            if (value != null)
            {
                _context.HeroTitles.Remove(value);
                _context.SaveChanges();
                TempData["Success"] = "Unvan silindi.";
            }
            return RedirectToAction("UpdateHeroSection");
        }

        private void LoadHeroTitles()
        {
            ViewBag.HeroTitles = _context.HeroTitles.OrderBy(x => x.Order).ToList();
        }
    }
}
