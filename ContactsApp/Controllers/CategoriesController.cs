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
    public class CategoriesController : Controller
    {
        private readonly ContactsAppDataContext _context;

        public CategoriesController(ContactsAppDataContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentSort"] = "Name";

            ViewData["CurrentFilter"] = searchString;


            var category = from c in _context.Categories
                           select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                category = category.Where(s => s.Description.Contains(searchString));
            }

            category = category.OrderBy(c => c.Description);

            return View(await PaginatedList<Category>.CreateAsync(category.AsNoTracking(), 1, 8));

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
            ViewData["DescriptionSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Description_desc" : "";
            ViewData["rowsOnEachPage"] = rowsOnEachPage;


            ViewData["CurrentFilter"] = searchString;


            var category = from c in _context.Categories
                           select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                category = category.Where(s => s.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Description_desc":
                    category = category.OrderByDescending(c => c.Description);
                    break;

                default:
                    category = category.OrderBy(c => c.Description);
                    break;
            }

            return View(await PaginatedList<Category>.CreateAsync(category.AsNoTracking(), pageNumber ?? 1, rowsOnEachPage ?? 8));
        }

        // GET: Categories/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var category = await _context.Categories
        //        .FirstOrDefaultAsync(m => m.CategoryId == id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(category);
        //}

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Description")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
