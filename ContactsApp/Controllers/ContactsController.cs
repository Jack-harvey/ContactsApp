#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContactsApp.Data;
using ContactsApp.Models;
using FileSignatures;
using FileSignatures.Formats;

namespace ContactsApp.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ContactsAppDataContext _context;

        public ContactsController(ContactsAppDataContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentSort"] = "Name";
            ViewData["NameSortParm"] = "";
            ViewData["BirthDateSortParm"] = "Birthdate";
            ViewData["CompanySortParm"] = "Company";
            ViewData["CategoryIdSortParm"] = "Category";
            ViewData["rowsOnEachPage"] = 8;

            

            ViewData["CurrentFilter"] = searchString;


            var contacts = from c in _context.Contacts
                           .Include(cat => cat.Category)
                           .Include(c => c.Company)
                           select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                contacts = contacts.Where(s => s.Lastname.Contains(searchString)
                                       || s.Firstname.Contains(searchString));
            }

            contacts = contacts.OrderBy(c => c.Lastname);

            return View(await PaginatedList<Contact>.CreateAsync(contacts.AsNoTracking(),1,8));
            
        }

        [HttpPost]
        public async Task<IActionResult> Index(
            string sortOrder,
            //string currentFilter,
            string searchString,
            int? pageNumber,
            int? rowsOnEachPage)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewData["BirthDateSortParm"] = sortOrder == "Birthdate" ? "Birthdate_desc" : "Birthdate";
            ViewData["CompanySortParm"] = sortOrder == "Company" ? "Company_desc" : "Company";
            ViewData["CategoryIdSortParm"] = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewData["rowsOnEachPage"] = rowsOnEachPage;
            

            ViewData["CurrentFilter"] = searchString;


            var contacts = from c in _context.Contacts
                           .Include(cat => cat.Category)
                           .Include(c => c.Company)
                           select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                contacts = contacts.Where(s => s.Lastname.Contains(searchString)
                                       || s.Firstname.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    contacts = contacts.OrderByDescending(c => c.Lastname);
                    break;

                case "Birthdate":
                    contacts = contacts.OrderBy(c => c.Birthday);
                    break;
                case "Birthdate_desc":
                    contacts = contacts.OrderByDescending(c => c.Birthday);
                    break;
                case "Company":
                    contacts = contacts.OrderBy(c => c.Company.CompanyName);
                    break;
                case "Company_desc":
                    contacts = contacts.OrderByDescending(c => c.Company.CompanyName);
                    break;
                case "Category":
                    contacts = contacts.OrderBy(c => c.Category.Description);
                    break;
                case "Category_desc":
                    contacts = contacts.OrderByDescending(c => c.Category.Description);
                    break;
                default:
                    contacts = contacts.OrderBy(c => c.Lastname);
                    break;
            }

            return View(await PaginatedList<Contact>.CreateAsync(contacts.AsNoTracking(), pageNumber ?? 1, rowsOnEachPage ?? 8));
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(cat => cat.Category)
                .Include(c => c.Company)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            //ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId, CompanyId");
            ViewData["CategoryId"] = new SelectList(_context.Categories.OrderBy(o => o.Description).Select(s => new { s.CategoryId, s.Description }), "CategoryId", "Description");
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(o => o.CompanyName).Select(s => new { s.CompanyId, s.CompanyName }), "CompanyId", "CompanyName");
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Firstname,Lastname,CompanyId,Mobile,Phone,Email,Birthday,Picture,Notes,CategoryId")] Contact contact, IFormFile pictureUpload)
        {
            bool fileExtensionValid = false;
            var inspector = new FileFormatInspector();
            var fileTypeInspection= inspector.DetermineFileFormat(pictureUpload.OpenReadStream());

            if (fileTypeInspection is Image)
            {
                fileExtensionValid = true;
            }

            try
            {
                if (ModelState.IsValid && fileExtensionValid)
                {
                    contact.ContactId = Guid.NewGuid();
                    string fileExtension = Path.GetExtension(pictureUpload.FileName);
                    string fileName = $"{contact.Firstname}.{contact.Lastname}.{contact.ContactId}{fileExtension}";
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                    string filePathForDb = $"/images/{fileName}";
                    contact.Picture = filePathForDb;

                    _context.Add(contact);
                    await _context.SaveChangesAsync();

                    using (FileStream fileStream = new(filePath, FileMode.Create))
                    {
                        await pictureUpload.CopyToAsync(fileStream);
                    }
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (DbUpdateException dbUpdateExceptionHOWDOILOGTHIS)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", contact.CategoryId);
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories.OrderBy(o => o.Description).Select(s => new { s.CategoryId, s.Description}), "CategoryId", "Description", contact.CategoryId);
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(o => o.CompanyName).Select(s => new { s.CompanyId, s.CompanyName }), "CompanyId", "CompanyName", contact.CompanyId);
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ContactId,Firstname,Lastname,CompanyId,Mobile,Phone,Email,Birthday,Picture,Notes,CategoryId")] Contact contact, IFormFile pictureUpload)

        {

            bool fileExtensionValid = false;
            var inspector = new FileFormatInspector();
            var fileTypeInspection = inspector.DetermineFileFormat(pictureUpload.OpenReadStream());

            if (fileTypeInspection is Image)
            {
                fileExtensionValid = true;
            }

            if (ModelState.IsValid && fileExtensionValid)
            {
                try
                {
                    string fileExtension = Path.GetExtension(pictureUpload.FileName);
                    string fileName = $"{contact.Firstname}.{contact.Lastname}.{contact.ContactId}{fileExtension}";
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                    string filePathForDb = $"/images/{fileName}";
                    contact.Picture = filePathForDb;


                    _context.Update(contact);
                    await _context.SaveChangesAsync();

                    using (FileStream fileStream = new(filePath, FileMode.Create))
                    {
                        await pictureUpload.CopyToAsync(fileStream);
                    }
                    return RedirectToAction(nameof(Index));
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.ContactId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", contact.CategoryId);
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(Guid? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .AsNoTracking()
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contact == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool ContactExists(Guid id)
        {
            return _context.Contacts.Any(e => e.ContactId == id);
        }












    }
}
