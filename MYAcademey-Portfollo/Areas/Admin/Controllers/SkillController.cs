using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MYAcademey_Portfollo.Context;
using MYAcademey_Portfollo.Entities;

namespace MYAcademey_Portfollo.Areas.Admin.Controllers
{
    // Yetenek grupları (Skill) ve gruplara bağlı yetenekler (SkillItem) burada yönetilir.
    [Area("Admin")]
    public class SkillController : Controller
    {
        private readonly PortfolioContext _context;

        public SkillController(PortfolioContext context)
        {
            _context = context;
        }

        public IActionResult SkillList()
        {
            var values = _context.Skills
                .Include(x => x.SkillItems)
                .OrderBy(x => x.SkillId)
                .ToList();
            return View(values);
        }

        // ---------------- Yetenek Grubu (Skill) ----------------

        [HttpGet]
        public IActionResult CreateSkill()
        {
            ViewBag.FormTitle = "Yeni Yetenek Grubu";
            return View("SkillForm", new Skill());
        }

        [HttpPost]
        public IActionResult CreateSkill(Skill skill)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.FormTitle = "Yeni Yetenek Grubu";
                return View("SkillForm", skill);
            }

            _context.Skills.Add(skill);
            _context.SaveChanges();
            TempData["Success"] = "Yetenek grubu eklendi.";
            return RedirectToAction("SkillList");
        }

        [HttpGet]
        public IActionResult UpdateSkill(int id)
        {
            var value = _context.Skills.Find(id);
            if (value == null)
            {
                return RedirectToAction("SkillList");
            }

            ViewBag.FormTitle = "Yetenek Grubunu Güncelle";
            return View("SkillForm", value);
        }

        [HttpPost]
        public IActionResult UpdateSkill(Skill skill)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.FormTitle = "Yetenek Grubunu Güncelle";
                return View("SkillForm", skill);
            }

            _context.Skills.Update(skill);
            _context.SaveChanges();
            TempData["Success"] = "Yetenek grubu güncellendi.";
            return RedirectToAction("SkillList");
        }

        public IActionResult DeleteSkill(int id)
        {
            var value = _context.Skills.Find(id);
            if (value != null)
            {
                // Gruba bağlı yetenekler (SkillItem) cascade delete ile birlikte silinir.
                _context.Skills.Remove(value);
                _context.SaveChanges();
                TempData["Success"] = "Yetenek grubu ve bağlı yetenekleri silindi.";
            }
            return RedirectToAction("SkillList");
        }

        // ---------------- Yetenek (SkillItem) ----------------

        [HttpGet]
        public IActionResult CreateSkillItem(int? skillId)
        {
            LoadSkillDropdown(skillId);
            ViewBag.FormTitle = "Yeni Yetenek";
            return View("SkillItemForm", new SkillItem { SkillId = skillId ?? 0, Level = 50 });
        }

        [HttpPost]
        public IActionResult CreateSkillItem(SkillItem skillItem)
        {
            if (!ModelState.IsValid)
            {
                LoadSkillDropdown(skillItem.SkillId);
                ViewBag.FormTitle = "Yeni Yetenek";
                return View("SkillItemForm", skillItem);
            }

            _context.SkillItems.Add(skillItem);
            _context.SaveChanges();
            TempData["Success"] = "Yetenek eklendi.";
            return RedirectToAction("SkillList");
        }

        [HttpGet]
        public IActionResult UpdateSkillItem(int id)
        {
            var value = _context.SkillItems.Find(id);
            if (value == null)
            {
                return RedirectToAction("SkillList");
            }

            LoadSkillDropdown(value.SkillId);
            ViewBag.FormTitle = "Yeteneği Güncelle";
            return View("SkillItemForm", value);
        }

        [HttpPost]
        public IActionResult UpdateSkillItem(SkillItem skillItem)
        {
            if (!ModelState.IsValid)
            {
                LoadSkillDropdown(skillItem.SkillId);
                ViewBag.FormTitle = "Yeteneği Güncelle";
                return View("SkillItemForm", skillItem);
            }

            _context.SkillItems.Update(skillItem);
            _context.SaveChanges();
            TempData["Success"] = "Yetenek güncellendi.";
            return RedirectToAction("SkillList");
        }

        public IActionResult DeleteSkillItem(int id)
        {
            var value = _context.SkillItems.Find(id);
            if (value != null)
            {
                _context.SkillItems.Remove(value);
                _context.SaveChanges();
                TempData["Success"] = "Yetenek silindi.";
            }
            return RedirectToAction("SkillList");
        }

        // Dropdown için yetenek gruplarını veritabanından doldurur (SelectList + asp-items).
        private void LoadSkillDropdown(int? selectedId)
        {
            ViewBag.Skills = new SelectList(_context.Skills.OrderBy(x => x.Title).ToList(), "SkillId", "Title", selectedId);
        }
    }
}
