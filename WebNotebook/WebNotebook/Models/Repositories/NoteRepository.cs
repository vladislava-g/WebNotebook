using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNotebook.Infrastructure;

namespace WebNotebook.Models.Repositories
{
    public class NoteRepository : IRepository<Note>
    {
        DbContext db;
        public NoteRepository(DbContext context)
        {
            db = context;
        }

        public void Create(Note obj)
        {
            db.Note.Add(obj);
            db.SaveChanges();
        }

        public void Delete(Note obj)
        {
            db.Note.Remove(obj);
            db.SaveChanges();
        }

        public Note Get(int id)
        {
            return db.Note.ToList().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Note> GetAll()
        {
            return db.Note.ToList();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

    }
}
