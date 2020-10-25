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
        IRepository<Notebook> notebookRepository;
        User current_user = null;

        public HomeController(IRepository<User> repository, IRepository<Notebook> notebookRepository)
        {
            this.repository = repository;
            this.notebookRepository = notebookRepository;
        }

        public IActionResult Index()
        {
            var claim_email = HttpContext.User.Claims.ToList().FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var claim_password = HttpContext.User.Claims.ToList().FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;

            if (claim_email == null || claim_password == null || !Authorization(claim_email, claim_password))
                return RedirectToAction("Index", "Account");

            return View();
        }

        private bool Authorization(string email, string password)
        {
            User user = null;
            try
            {
                user = repository.GetAll().Where(x => x.Email == email && x.Password == password).Single();
                current_user = user;
            }
            catch { }

            return user != null ? true : false;
        }

        public ActionResult GetNotebooks()
        {
            return PartialView("_Notebooks");
        }   
        
        public ActionResult GetNotes()
        {
            return PartialView("_Notes");
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
