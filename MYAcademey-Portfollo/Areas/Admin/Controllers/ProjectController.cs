using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MYAcademey_Portfollo.Context;
using MYAcademey_Portfollo.Entities;
using MYAcademey_Portfollo.Services;

namespace MYAcademey_Portfollo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProjectController : Controller
    {
        private const string ImageFolder = "projects";

        private readonly PortfolioContext _context;
        private readonly ImageUploadService _imageUploadService;

        public ProjectController(PortfolioContext context, ImageUploadService imageUploadService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
        }

        // Kategori filtresi, arama, durum filtresi ve sıralama destekli liste
        public IActionResult ProjectList(int? categoryId, string? search, string? status, string sort = "newest")
        {
            var query = _context.Projects.Include(x => x.Category).AsQueryable();

            if (categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.ProjectTitle.Contains(search) || (x.Technologies != null && x.Technologies.Contains(search)));
            }

            if (status == "active")
            {
                query = query.Where(x => x.IsActive);
            }
            else if (status == "passive")
            {
                query = query.Where(x => !x.IsActive);
            }

            query = sort switch
            {
                "oldest" => query.OrderBy(x => x.CreatedDate),
                "title" => query.OrderBy(x => x.ProjectTitle),
                _ => query.OrderByDescending(x => x.CreatedDate)   // varsayılan: en yeni proje en üstte
            };

            ViewBag.Categories = new SelectList(_context.Categories.OrderBy(x => x.CategoryName).ToList(), "CategoryId", "CategoryName", categoryId);
            ViewBag.CategoryId = categoryId;
            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.Sort = sort;

            ViewBag.TotalCount = _context.Projects.Count();
            ViewBag.ActiveCount = _context.Projects.Count(x => x.IsActive);
            ViewBag.PassiveCount = _context.Projects.Count(x => !x.IsActive);

            return View(query.ToList());
        }

        [HttpGet]
        public IActionResult CreateProject()
        {
            LoadCategoryDropdown(null);
            ViewBag.FormTitle = "Yeni Proje";
            return View("ProjectForm", new Project());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(Project project, IFormFile? imageFile)
        {
            if (imageFile != null)
            {
                var upload = await _imageUploadService.SaveAsync(imageFile, ImageFolder);
                if (upload.Success)
                {
                    project.ImageUrl = upload.Path;
                }
                else
                {
                    ModelState.AddModelError("imageFile", upload.ErrorMessage!);
                }
            }

            if (!ModelState.IsValid)
            {
                // Hata varsa yüklenen görseli geri sil; form tekrar gösterilir.
                _imageUploadService.Delete(project.ImageUrl);
                project.ImageUrl = null;
                LoadCategoryDropdown(project.CategoryId);
                ViewBag.FormTitle = "Yeni Proje";
                return View("ProjectForm", project);
            }

            project.CreatedDate = DateTime.Now;
            _context.Projects.Add(project);
            _context.SaveChanges();
            TempData["Success"] = "Proje eklendi.";
            return RedirectToAction("ProjectList");
        }

        [HttpGet]
        public IActionResult UpdateProject(int id)
        {
            var value = _context.Projects.Find(id);
            if (value == null)
            {
                return RedirectToAction("ProjectList");
            }

            LoadCategoryDropdown(value.CategoryId);
            ViewBag.FormTitle = "Projeyi Güncelle";
            return View("ProjectForm", value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProject(Project project, IFormFile? imageFile)
        {
            var existing = _context.Projects.Find(project.ProjectId);
            if (existing == null)
            {
                return RedirectToAction("ProjectList");
            }

            string? newImagePath = null;
            if (imageFile != null)
            {
                var upload = await _imageUploadService.SaveAsync(imageFile, ImageFolder);
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
                project.ImageUrl = existing.ImageUrl;
                LoadCategoryDropdown(project.CategoryId);
                ViewBag.FormTitle = "Projeyi Güncelle";
                return View("ProjectForm", project);
            }

            // Sadece formdan gelen alanlar güncellenir; eklenme tarihi korunur.
            existing.ProjectTitle = project.ProjectTitle;
            existing.Description = project.Description;
            existing.Technologies = project.Technologies;
            existing.CategoryId = project.CategoryId;
            existing.IsActive = project.IsActive;

            if (newImagePath != null)
            {
                _imageUploadService.Delete(existing.ImageUrl);   // eski görsel diskten silinir
                existing.ImageUrl = newImagePath;
            }

            _context.SaveChanges();
            TempData["Success"] = "Proje güncellendi.";
            return RedirectToAction("ProjectList");
        }

        public IActionResult DeleteProject(int id)
        {
            var value = _context.Projects.Find(id);
            if (value != null)
            {
                _imageUploadService.Delete(value.ImageUrl);
                _context.Projects.Remove(value);
                _context.SaveChanges();
                TempData["Success"] = "Proje silindi.";
            }
            return RedirectToAction("ProjectList");
        }

        // Aktif <-> Pasif. Pasif projeler sitede listelenmez.
        public IActionResult ToggleProjectStatus(int id)
        {
            var value = _context.Projects.Find(id);
            if (value != null)
            {
                value.IsActive = !value.IsActive;
                _context.SaveChanges();
                TempData["Success"] = value.IsActive ? "Proje aktif yapıldı, sitede görünecek." : "Proje pasif yapıldı, sitede gizlendi.";
            }
            return RedirectToAction("ProjectList");
        }

        private void LoadCategoryDropdown(int? selectedId)
        {
            ViewBag.Categories = new SelectList(_context.Categories.OrderBy(x => x.CategoryName).ToList(), "CategoryId", "CategoryName", selectedId);
        }
    }
}
