using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebNotebook.Infrastructure;
using WebNotebook.Models;

namespace WebNotebook.Controllers
{
    [Authorize]

    public class HomeController : Controller
    {
        IRepository<User> repository;
        User current_user = null;

        public HomeController(IRepository<User> repository)
        {
            this.repository = repository;
        }

        public IActionResult Index()
        {
            if(!Authorization())
                return RedirectToAction("Index", "Account");
            ViewBag.UserId = current_user.Id;

            return View();
        }

        private bool Authorization()
        {
            var claim_email = HttpContext.User.Claims.ToList().FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var claim_password = HttpContext.User.Claims.ToList().FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;

            User user = null;
            try
            {
                user = repository.GetAll().Where(x => x.Email == claim_email && x.Password == claim_password).Single();
                current_user = user;
            }
            catch { }

            return claim_email == null || claim_password == null || user == null ? false : true;
        }
   
        public ActionResult GetFavorites()
        {
            return PartialView("_Favorites");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
