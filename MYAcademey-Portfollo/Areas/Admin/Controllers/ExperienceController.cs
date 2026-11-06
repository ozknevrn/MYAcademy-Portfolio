using Microsoft.AspNetCore.Mvc;
using MYAcademey_Portfollo.Context;
using MYAcademey_Portfollo.Entities;

namespace MYAcademey_Portfollo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExperienceController : Controller
    {
        private readonly PortfolioContext _context;

        public ExperienceController(PortfolioContext context)
        {
            _context = context;
        }

        public IActionResult ExperienceList(string? search, string? status)
        {
            var query = _context.Experiences.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Title.Contains(search) || x.CompanyName.Contains(search));
            }

            if (status == "active")
            {
                query = query.Where(x => x.IsActive);
            }
            else if (status == "passive")
            {
                query = query.Where(x => !x.IsActive);
            }

            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.TotalCount = _context.Experiences.Count();
            ViewBag.ActiveCount = _context.Experiences.Count(x => x.IsActive);
            ViewBag.PassiveCount = _context.Experiences.Count(x => !x.IsActive);

            return View(query.OrderByDescending(x => x.ExperienceId).ToList());
        }

        [HttpGet]
        public IActionResult CreateExperience()
        {
            ViewBag.FormTitle = "Yeni Deneyim";
            return View("ExperienceForm", new Experience());
        }

        [HttpPost]
        public IActionResult CreateExperience(Experience experience)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.FormTitle = "Yeni Deneyim";
                return View("ExperienceForm", experience);
            }

            _context.Experiences.Add(experience);
            _context.SaveChanges();
            TempData["Success"] = "Deneyim eklendi.";
            return RedirectToAction("ExperienceList");
        }

        [HttpGet]
        public IActionResult UpdateExperience(int id)
        {
            var value = _context.Experiences.Find(id);
            if (value == null)
            {
                return RedirectToAction("ExperienceList");
            }

            ViewBag.FormTitle = "Deneyimi Güncelle";
            return View("ExperienceForm", value);
        }

        [HttpPost]
        public IActionResult UpdateExperience(Experience experience)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.FormTitle = "Deneyimi Güncelle";
                return View("ExperienceForm", experience);
            }

            _context.Experiences.Update(experience);
            _context.SaveChanges();
            TempData["Success"] = "Deneyim güncellendi.";
            return RedirectToAction("ExperienceList");
        }

        public IActionResult DeleteExperience(int id)
        {
            var value = _context.Experiences.Find(id);
            if (value != null)
            {
                _context.Experiences.Remove(value);
                _context.SaveChanges();
                TempData["Success"] = "Deneyim silindi.";
            }
            return RedirectToAction("ExperienceList");
        }

        public IActionResult ToggleExperienceStatus(int id)
        {
            var value = _context.Experiences.Find(id);
            if (value != null)
            {
                value.IsActive = !value.IsActive;
                _context.SaveChanges();
                TempData["Success"] = value.IsActive ? "Deneyim aktif yapıldı." : "Deneyim pasif yapıldı, sitede gizlendi.";
            }
            return RedirectToAction("ExperienceList");
        }
    }
}
