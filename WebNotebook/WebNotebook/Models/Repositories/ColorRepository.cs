using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNotebook.Infrastructure;

namespace WebNotebook.Models.Repositories
{
    public class ColorRepository : IRepository<Color>
    {
        DbContext db;
        public ColorRepository(DbContext context)
        {
            db = context;
        }

        public void Create(Color obj)
        {
            db.Color.Add(obj);
            db.SaveChanges();
        }

        public void Delete(Color obj)
        {
            db.Color.Remove(obj);
            db.SaveChanges();
        }

        public Color Get(int id)
        {
            return db.Color.ToList().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Color> GetAll()
        {
            return db.Color.ToList();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

    }
}
