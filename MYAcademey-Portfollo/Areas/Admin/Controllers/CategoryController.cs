using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademey_Portfollo.Context;
using MYAcademey_Portfollo.Entities;

namespace MYAcademey_Portfollo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly PortfolioContext _context;

        public CategoryController(PortfolioContext context)
        {
            _context = context;
        }

        public IActionResult CategoryList()
        {
            // Include ile her kategorinin projeleri de yüklenir; listede proje sayısı gösterilir.
            var values = _context.Categories.Include(x => x.Projects).OrderBy(x => x.CategoryName).ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.Categories.Add(category);
            _context.SaveChanges();
            TempData["Success"] = "Kategori başarıyla eklendi.";
            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public IActionResult UpdateCategory(int id)
        {
            var value = _context.Categories.Find(id);
            if (value == null)
            {
                return RedirectToAction("CategoryList");
            }
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.Categories.Update(category);
            _context.SaveChanges();
            TempData["Success"] = "Kategori başarıyla güncellendi.";
            return RedirectToAction("CategoryList");
        }

        public IActionResult DeleteCategory(int id)
        {
            var value = _context.Categories.Find(id);
            if (value == null)
            {
                return RedirectToAction("CategoryList");
            }

            bool hasProjects = _context.Projects.Any(x => x.CategoryId == id);
            if (hasProjects)
            {
                TempData["Error"] = "Bu kategoriye bağlı projeler olduğu için silinemez.";
                return RedirectToAction("CategoryList");
            }

            _context.Categories.Remove(value);
            _context.SaveChanges();
            TempData["Success"] = "Kategori başarıyla silindi.";
            return RedirectToAction("CategoryList");
        }
    }
}