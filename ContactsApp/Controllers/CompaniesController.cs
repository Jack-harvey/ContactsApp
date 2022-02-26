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
    public class CompaniesController : Controller
    {
        private readonly ContactsAppDataContext _context;

        public CompaniesController(ContactsAppDataContext context)
        {
            _context = context;
        }

        // GET: Companies
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentSort"] = "Name";
            ViewData["NameSortParm"] = "";
            ViewData["FoundDateSortParm"] = "Founddate";
            ViewData["AbnSortParm"] = "Abn";
            ViewData["rowsOnEachPage"] = 8;



            ViewData["CurrentFilter"] = searchString;


            var companies = from c in _context.Companies
                           select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                companies = companies.Where(s => s.CompanyName.Contains(searchString)
                                       || s.Abn.Contains(searchString));
            }

            companies = companies.OrderBy(c => c.CompanyName);

            return View(await PaginatedList<Company>.CreateAsync(companies.AsNoTracking(), 1, 8));

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
            ViewData["FoundDateSortParm"] = sortOrder == "Founddate" ? "Founddate_desc" : "Founddate";
            ViewData["AbnSortParm"] = sortOrder == "Abn" ? "Abn_desc" : "Abn";
            ViewData["rowsOnEachPage"] = rowsOnEachPage;


            ViewData["CurrentFilter"] = searchString;


            var companies = from c in _context.Companies
                            select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                companies = companies.Where(s => s.CompanyName.Contains(searchString)
                                       || s.Abn.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    companies = companies.OrderByDescending(c => c.CompanyName);
                    break;
                case "Founddate":
                    companies = companies.OrderBy(c => c.FoundingDate);
                    break;
                case "Founddate_desc":
                    companies = companies.OrderByDescending(c => c.FoundingDate);
                    break;
                case "Abn":
                    companies = companies.OrderBy(c => c.Abn);
                    break;
                case "Abn_desc":
                    companies = companies.OrderByDescending(c => c.Abn);
                    break;
                default:
                    companies = companies.OrderBy(c => c.CompanyName);
                    break;
            }

            return View(await PaginatedList<Company>.CreateAsync(companies.AsNoTracking(), pageNumber ?? 1, rowsOnEachPage ?? 8));
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,CompanyName,Abn,Website,FoundingDate")] Company company)
        {
            if (ModelState.IsValid)
            {
                company.CompanyId = Guid.NewGuid();
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CompanyId,CompanyName,Abn,Website,FoundingDate")] Company company)
        {
            if (id != company.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.CompanyId))
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
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(Guid id)
        {
            return _context.Companies.Any(e => e.CompanyId == id);
        }
    }
}
