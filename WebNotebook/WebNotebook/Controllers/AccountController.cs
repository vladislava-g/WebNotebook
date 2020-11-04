using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebNotebook.Infrastructure;
using WebNotebook.Models;

namespace WebNotebook.Controllers
{
    public class AccountController : Controller
    {
        IRepository<User> repository;
        public AccountController(IRepository<User> repository)
        {
            this.repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Login
        [HttpPost]
        public async Task<ActionResult> Login([FromBody]User model)
        {
            User user = null;
            try
            {
                user = repository.GetAll().Where(x => x.Email == model.Email && x.Password == model.Password).Single();
            }
            catch { }

            var isExist = user != null ? true : false;

            if (isExist)
            {
                try
                {
                    await Authenticate(user);
                    return new JsonResult(new Data { Success = true, Message = "Connecting..." });
                }
                catch { }
            }

            return new JsonResult(new Data { Success = false, Message = "Incorrect Email or Password" });
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.UserData, user.Password) //hash!
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimTypes.Email, ClaimTypes.UserData);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [HttpPost]
        public IActionResult Logout()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, ""),
                new Claim(ClaimTypes.UserData, "") 
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimTypes.Email, ClaimTypes.UserData);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            return new JsonResult(new Data { Success = false, Message = "" });
        }
        #endregion

        #region Registration

        [HttpPost]

        public IActionResult Registration([FromBody] User user)
        {
            try
            {
                var users = repository.GetAll().Where(x => x.Email == user.Email).Count();
                if (users == 0)
                {
                    user.IsVerified = 1;
                    repository.Create(user);
                    //SendEmail(user);
                }
                else
                    return new JsonResult(new Data { Success = false, Message = "Email is already registered" });

            }
            catch
            {
                return new JsonResult(new Data { Success = false, Message = "Unknown Error" });
            }
            return new JsonResult(new Data { Success = true, Message = "Success" });
        }

        public void SendEmail(User user)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("webnotebook@gmail.com");
            msg.To.Add(user.Email);
            msg.Subject = "welcome to WebNotebook!";

            msg.Body = $"Dear {user.Name},\n" +
                $"Thank you for registering!";


            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com", 587);
            smtpServer.Credentials = new NetworkCredential("testing.stf", "testing12qw");
            smtpServer.EnableSsl = true;
            smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtpServer.Send(msg);
        }
        #endregion
    }
}