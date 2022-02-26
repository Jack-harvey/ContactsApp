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

namespace ContactsApp.Controllers
{
    public class CompanyOfficesController : Controller
    {
        private readonly ContactsAppDataContext _context;

        public CompanyOfficesController(ContactsAppDataContext context)
        {
            _context = context;
        }

        // GET: CompanyOffices
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentSort"] = "Name";
            ViewData["NameSortParm"] = "";
            ViewData["AddressSortParm"] = "Address";
            ViewData["CitySortParm"] = "City";
            ViewData["PostcodeSortParm"] = "Postcode";
            ViewData["rowsOnEachPage"] = 8;



            ViewData["CurrentFilter"] = searchString;


            var Office = from c in _context.CompanyOffices
                           .Include(c => c.Company)
                         select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                Office = Office.Where(s => s.Company.CompanyName.Contains(searchString)
                                       | s.Address.Contains(searchString)
                                       | s.City.Contains(searchString)
                                       | s.PostCode.Contains(searchString));
                //CHECK -- is | the best way to write this?
            }

            Office = Office.OrderBy(c => c.Company.CompanyName);

            return View(await PaginatedList<CompanyOffice>.CreateAsync(Office.AsNoTracking(), 1, 8));

        }

        [HttpPost]
        public async Task<IActionResult> Index(
            string sortOrder,
            string searchString,
            int? pageNumber,
            int? rowsOnEachPage)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewData["AddressSortParm"] = sortOrder == "Address" ? "Address_desc" : "Address";
            ViewData["CitySortParm"] = sortOrder == "City" ? "City_desc" : "City";
            ViewData["PostcodeSortParm"] = sortOrder == "Postcode" ? "Postcode_desc" : "Postcode";
            ViewData["rowsOnEachPage"] = rowsOnEachPage;


            ViewData["CurrentFilter"] = searchString;


            var Office = from c in _context.CompanyOffices
                           .Include(c => c.Company)
                         select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                Office = Office.Where(s => s.Company.CompanyName.Contains(searchString)
                                       | s.Address.Contains(searchString)
                                       | s.City.Contains(searchString)
                                       | s.PostCode.Contains(searchString));
                //CHECK -- is | the best way to write this?
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    Office = Office.OrderByDescending(c => c.Company.CompanyName);
                    break;

                case "Address":
                    Office = Office.OrderBy(c => c.Address);
                    break;
                case "Address_desc":
                    Office = Office.OrderByDescending(c => c.Address);
                    break;
                case "City":
                    Office = Office.OrderBy(c => c.City);
                    break;
                case "City_desc":
                    Office = Office.OrderByDescending(c => c.City);
                    break;
                case "Postcode":
                    Office = Office.OrderBy(c => c.PostCode);
                    break;
                case "Postcode_desc":
                    Office = Office.OrderByDescending(c => c.PostCode);
                    break;
                default:
                    Office = Office.OrderBy(c => c.Company.CompanyName);
                    break;
            }

            return View(await PaginatedList<CompanyOffice>.CreateAsync(Office.AsNoTracking(), pageNumber ?? 1, rowsOnEachPage ?? 8));
        }

        // GET: CompanyOffices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyOffice = await _context.CompanyOffices
                .Include(c => c.Company)
                .FirstOrDefaultAsync(m => m.OfficeId == id);
            if (companyOffice == null)
            {
                return NotFound();
            }

            return View(companyOffice);
        }

        //GET: CompanyOffices/Create
        public IActionResult Create()
        {
            //ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyId");
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(o => o.CompanyName).Select(s => new { s.CompanyId, s.CompanyName }), "CompanyId", "CompanyName");
            return View();
        }




        // POST: CompanyOffices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OfficeId,CompanyId,Address,City,PostCode")] CompanyOffice companyOffice)
        {
            if (ModelState.IsValid)
            {
                companyOffice.OfficeId = Guid.NewGuid();
                _context.Add(companyOffice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyId", companyOffice.CompanyId);
            return View(companyOffice);
        }

        // GET: CompanyOffices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyOffice = await _context.CompanyOffices.FindAsync(id);
            if (companyOffice == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(o => o.CompanyName).Select(s => new { s.CompanyId, s.CompanyName }), "CompanyId", "CompanyName", companyOffice.CompanyId);
            return View(companyOffice);
        }

        // POST: CompanyOffices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("OfficeId,CompanyId,Address,City,PostCode")] CompanyOffice companyOffice)
        {
            if (id != companyOffice.OfficeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companyOffice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyOfficeExists(companyOffice.OfficeId))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyId", companyOffice.CompanyId);
            return View(companyOffice);
        }

        // GET: CompanyOffices/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyOffice = await _context.CompanyOffices
                .Include(c => c.Company)
                .FirstOrDefaultAsync(m => m.OfficeId == id);
            if (companyOffice == null)
            {
                return NotFound();
            }

            return View(companyOffice);
        }

        // POST: CompanyOffices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var companyOffice = await _context.CompanyOffices.FindAsync(id);
            _context.CompanyOffices.Remove(companyOffice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyOfficeExists(Guid id)
        {
            return _context.CompanyOffices.Any(e => e.OfficeId == id);
        }
    }
}
