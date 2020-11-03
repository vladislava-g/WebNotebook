using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNotebook.Infrastructure;

namespace WebNotebook.Models.Repositories
{
    public class DefaultImageRepository : IRepository<DefaultImage>
    {
        DbContext db;
        public DefaultImageRepository(DbContext context)
        {
            db = context;
        }

        public void Create(DefaultImage obj)
        {
            db.DefaultImage.Add(obj);
            db.SaveChanges();
        }

        public void Delete(DefaultImage obj)
        {
            db.DefaultImage.Remove(obj);
            db.SaveChanges();
        }

        public DefaultImage Get(int id)
        {
            return db.DefaultImage.ToList().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<DefaultImage> GetAll()
        {
            return db.DefaultImage.ToList();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

    }
}
