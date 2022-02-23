using ContactsApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ContactsApp.Data;
using ContactsApp.Models.CompanyViewModels;

namespace ContactsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ContactsAppDataContext _context;

        public HomeController(ILogger<HomeController> logger, ContactsAppDataContext context) 
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<ActionResult> About()
        {
            IQueryable<CompanyGroup> data =
                from contact in _context.Contacts
                join  company in _context.Companies on  contact.CompanyId equals company.CompanyId
                //.Include(i => i.Company)
                group company by company.CompanyName into companyGroup
                select new CompanyGroup()
                {
                    CompanyName = companyGroup.Key,
                    CompanyCount = companyGroup.Count(),
                };
            return View(await data.AsNoTracking().ToListAsync());
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}