using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNotebook.Infrastructure;

namespace WebNotebook.Models
{
    public class UserRepository : IRepository<User>
    {
        DbContext db;
        public UserRepository(DbContext userContext)
        {
            db = userContext;
        }

        public void Create(User obj)
        {
            db.User.Add(obj);
            db.SaveChanges();
        }

        public void Delete(User obj)
        {
            db.Remove(obj);
            db.SaveChanges();
        }

        public User Get(int id)
        {
            return db.User.ToList().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<User> GetAll()
        {
            return db.User.ToList();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

    }
}
