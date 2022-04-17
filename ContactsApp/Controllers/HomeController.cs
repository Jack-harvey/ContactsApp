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


        public JsonResult UsernameAndTheme(int userId)
        {
            var users = _context.Users;
            var userIdInDb = users.Where(s => s.UserId.Equals(userId));
            return Json(userIdInDb);
        }

        //public string ThemeNameFromDbForLayout()
        //{

        //    int fakeUserId = 2;
        //    var userIdInDb = _context.Users.Find(fakeUserId);

        //    if (userIdInDb == null)
        //        return "";
        //    if (userIdInDb.ThemeSelection == null)
        //        return "";
        //    return userIdInDb.ThemeSelection.ToString();
        //}

        public JsonResult SaveNewThemePreference(int userId, string themePreference)
        {
            var users = _context.Users;
            var userToUpdate = users.Find(userId);

            if (userToUpdate == null)
                return Json(new { result = 0, message = "Congrats, it's a baby Null... 🥰" });

            if (themePreference == "dracula-theme")
            {
                userToUpdate.ThemeSelection = "Dracula";
                _context.Update(userToUpdate);
                _context.SaveChanges();
                return Json(new { result = 1, theme = "dracula-theme", message = "It's dracula baby!" });
            }
            if (themePreference == "atom-one-dark-theme")
            {
                userToUpdate.ThemeSelection = "atomOneDark";
                _context.Update(userToUpdate);
                _context.SaveChanges();
                return Json(new { result = 1, theme = "atom-one-dark-theme", message = "Click change theme again, you did it wrong." });
            }
            else
            {
                return Json(new { result = 0, message = "Invalid theme"});
            }
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