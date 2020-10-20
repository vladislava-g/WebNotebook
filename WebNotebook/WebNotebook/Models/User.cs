using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebNotebook.Models
{
    public class User
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public int IsVerified { set; get; }
        public string Photo { set; get; }
    }
    public class UserView
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Photo { set; get; }
        public string Email { set; get; }
    }
}
