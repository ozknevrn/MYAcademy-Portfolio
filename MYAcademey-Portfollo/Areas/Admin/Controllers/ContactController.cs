using Microsoft.AspNetCore.Mvc;
using MYAcademey_Portfollo.Context;
using MYAcademey_Portfollo.Entities;

namespace MYAcademey_Portfollo.Areas.Admin.Controllers
{
    // Sitedeki iletişim bölümünde gösterilen bilgiler (e-posta, telefon, konum).
    // Sitede ilk kayıt gösterilir.
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly PortfolioContext _context;

        public ContactController(PortfolioContext context)
        {
            _context = context;
        }

        public IActionResult ContactList()
        {
            var values = _context.Contacts.OrderBy(x => x.ContactId).ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateContact()
        {
            ViewBag.FormTitle = "Yeni İletişim Bilgisi";
            return View("ContactForm", new Contact());
        }

        [HttpPost]
        public IActionResult CreateContact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.FormTitle = "Yeni İletişim Bilgisi";
                return View("ContactForm", contact);
            }

            _context.Contacts.Add(contact);
            _context.SaveChanges();
            TempData["Success"] = "İletişim bilgisi eklendi.";
            return RedirectToAction("ContactList");
        }

        [HttpGet]
        public IActionResult UpdateContact(int id)
        {
            var value = _context.Contacts.Find(id);
            if (value == null)
            {
                return RedirectToAction("ContactList");
            }

            ViewBag.FormTitle = "İletişim Bilgisini Güncelle";
            return View("ContactForm", value);
        }

        [HttpPost]
        public IActionResult UpdateContact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.FormTitle = "İletişim Bilgisini Güncelle";
                return View("ContactForm", contact);
            }

            _context.Contacts.Update(contact);
            _context.SaveChanges();
            TempData["Success"] = "İletişim bilgisi güncellendi.";
            return RedirectToAction("ContactList");
        }

        public IActionResult DeleteContact(int id)
        {
            var value = _context.Contacts.Find(id);
            if (value != null)
            {
                _context.Contacts.Remove(value);
                _context.SaveChanges();
                TempData["Success"] = "İletişim bilgisi silindi.";
            }
            return RedirectToAction("ContactList");
        }
    }
}
