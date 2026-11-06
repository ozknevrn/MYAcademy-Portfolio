using Microsoft.AspNetCore.Mvc;
using MYAcademey_Portfollo.Context;
using MYAcademey_Portfollo.Entities;
using MYAcademey_Portfollo.Services;

namespace MYAcademey_Portfollo.Areas.Admin.Controllers
{
    // Hakkımda bölümü tek kayıttan oluşur; bu yüzden sadece güncelleme ekranı vardır.
    [Area("Admin")]
    public class AboutController : Controller
    {
        private readonly PortfolioContext _context;
        private readonly ImageUploadService _imageUploadService;

        public AboutController(PortfolioContext context, ImageUploadService imageUploadService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
        }

        [HttpGet]
        public IActionResult UpdateAbout()
        {
            var value = _context.Abouts.FirstOrDefault() ?? new About();
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAbout(About about, IFormFile? imageFile)
        {
            string? newImagePath = null;
            if (imageFile != null)
            {
                var upload = await _imageUploadService.SaveAsync(imageFile, "about");
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
                return View(about);
            }

            if (newImagePath != null)
            {
                _imageUploadService.Delete(about.ImageUrl);
                about.ImageUrl = newImagePath;
            }

            if (about.AboutId == 0)
            {
                _context.Abouts.Add(about);
            }
            else
            {
                _context.Abouts.Update(about);
            }

            _context.SaveChanges();
            TempData["Success"] = "Hakkımda bölümü kaydedildi.";
            return RedirectToAction("UpdateAbout");
        }
    }
}
