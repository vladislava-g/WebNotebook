using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public HomeController(IRepository<User> repository)
        {
            this.repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
