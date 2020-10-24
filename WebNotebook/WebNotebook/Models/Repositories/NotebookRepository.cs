using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNotebook.Infrastructure;

namespace WebNotebook.Models.Repositories
{
    public class NotebookRepository : IRepository<Notebook>
    {
        DbContext db;
        public NotebookRepository(DbContext context)
        {
            db = context;
        }

        public void Create(Notebook obj)
        {
            db.Notebook.Add(obj);
            db.SaveChanges();
        }

        public void Delete(Notebook obj)
        {
            db.Notebook.Remove(obj);
            db.SaveChanges();
        }

        public Notebook Get(int id)
        {
            return db.Notebook.ToList().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Notebook> GetAll()
        {
            return db.Notebook.ToList();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

    }
}
