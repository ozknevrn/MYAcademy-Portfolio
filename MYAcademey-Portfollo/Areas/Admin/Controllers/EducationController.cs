using Microsoft.AspNetCore.Mvc;
using MYAcademey_Portfollo.Context;
using MYAcademey_Portfollo.Entities;

namespace MYAcademey_Portfollo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EducationController : Controller
    {
        private readonly PortfolioContext _context;

        public EducationController(PortfolioContext context)
        {
            _context = context;
        }

        public IActionResult EducationList()
        {
            var values = _context.Educations.OrderByDescending(x => x.EducationId).ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateEducation()
        {
            ViewBag.FormTitle = "Yeni Eğitim";
            return View("EducationForm", new Education());
        }

        [HttpPost]
        public IActionResult CreateEducation(Education education)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.FormTitle = "Yeni Eğitim";
                return View("EducationForm", education);
            }

            _context.Educations.Add(education);
            _context.SaveChanges();
            TempData["Success"] = "Eğitim bilgisi eklendi.";
            return RedirectToAction("EducationList");
        }

        [HttpGet]
        public IActionResult UpdateEducation(int id)
        {
            var value = _context.Educations.Find(id);
            if (value == null)
            {
                return RedirectToAction("EducationList");
            }

            ViewBag.FormTitle = "Eğitim Bilgisini Güncelle";
            return View("EducationForm", value);
        }

        [HttpPost]
        public IActionResult UpdateEducation(Education education)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.FormTitle = "Eğitim Bilgisini Güncelle";
                return View("EducationForm", education);
            }

            _context.Educations.Update(education);
            _context.SaveChanges();
            TempData["Success"] = "Eğitim bilgisi güncellendi.";
            return RedirectToAction("EducationList");
        }

        public IActionResult DeleteEducation(int id)
        {
            var value = _context.Educations.Find(id);
            if (value != null)
            {
                _context.Educations.Remove(value);
                _context.SaveChanges();
                TempData["Success"] = "Eğitim bilgisi silindi.";
            }
            return RedirectToAction("EducationList");
        }

        public IActionResult ToggleEducationStatus(int id)
        {
            var value = _context.Educations.Find(id);
            if (value != null)
            {
                value.IsActive = !value.IsActive;
                _context.SaveChanges();
                TempData["Success"] = "Eğitim durumu güncellendi.";
            }
            return RedirectToAction("EducationList");
        }
    }
}
