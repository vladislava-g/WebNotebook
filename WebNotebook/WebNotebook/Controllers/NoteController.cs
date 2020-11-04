using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebNotebook.Infrastructure;
using WebNotebook.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebNotebook.Controllers
{
    public class NoteController : Controller
    {
        IRepository<Note> noteRepository;
        IRepository<Notebook> notebookRepository;
        IRepository<Color> colorRepository;
        public NoteController(IRepository<Note> noteRepository, IRepository<Notebook> notebookRepository, IRepository<Color> colorRepository)
        {
            this.noteRepository = noteRepository;
            this.notebookRepository = notebookRepository;
            this.colorRepository = colorRepository;
        }
        public IActionResult GetByNotebook(int id = 0)
        {
            ViewBag.GoBackClass = "";
            ViewBag.UserId = notebookRepository.Get(id).CreatorId;
            ViewBag.NotebookId = id;
            ViewBag.Title = notebookRepository.Get(id).Name;
            ViewBag.Notes = noteRepository.GetAll().Where(x => x.NotebookId == id).ToList().OrderByDescending(x => x.Modified);
            return PartialView("~/Views/Home/_Notes.cshtml");
        }

        public IActionResult GetNotes(int? id) //by user
        {
            ViewBag.Notes = noteRepository.GetAll().Where(x => x.UserId == id).ToList().OrderByDescending(x => x.Modified);
            ViewBag.UserId = id;
            ViewBag.NotebookId = notebookRepository.GetAll().Where(x => x.CreatorId == id && x.IsDefault == 1).FirstOrDefault().Id;
            ViewBag.Title = "Notes";
            ViewBag.GoBackClass = "d-none";
            return PartialView("~/Views/Home/_Notes.cshtml");
        }

        public IActionResult Create(int notebookId = 0, int creatorId = 0)
        {
            var notebook = notebookRepository.GetAll().Where(x => x.CreatorId == creatorId && x.IsDefault == 1).FirstOrDefault();
            if (notebookId != 0)
                notebook = notebookRepository.Get(notebookId);
            else
            {
                if (notebook == null)
                {
                    notebook = notebookRepository.GetAll().FirstOrDefault();
                    notebook.IsDefault = 1;
                }

            }

            notebook.Modified = DateTime.Now;

            Random rnd = new Random();
            var num = rnd.Next(1, 11);
            var note = new Note()
            {
                UserId = notebook.CreatorId,
                NotebookId = notebook.Id,
                Content = "",
                Title = "Untitled",
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Hex = colorRepository.GetAll().Where(x => x.Id == num).FirstOrDefault().Hex,
                TypeId = 1
            };

            noteRepository.Create(note);
            notebookRepository.SaveChanges();
            return Json(note);

        }


        public ActionResult Update(int id = 0, string name = "", string content = "")
        {
            try
            {
                var note = noteRepository.Get(id);
                var notebook = notebookRepository.GetAll().Where(x => x.Id == note.NotebookId).FirstOrDefault();
                notebook.Modified = DateTime.Now;

                if (name != "")
                    note.Title = name;
                if (content != "")
                    note.Content = content;

                note.Modified = DateTime.Now;
                noteRepository.SaveChanges();
                notebookRepository.SaveChanges();
                return Json(new Data() { Success = true });
            }
            catch
            {
                return Json(new Data() { Success = false });

            }
        }

        public ActionResult Delete(int id = 0)
        {
            try
            {
                var note = noteRepository.Get(id);
                noteRepository.Delete(note);
                var notebook = notebookRepository.GetAll().Where(x => x.Id == note.NotebookId).FirstOrDefault();
                notebook.Modified = DateTime.Now;
                notebookRepository.SaveChanges();
                return Json(new Data() { Success = true });
            }
            catch
            {
                return Json(new Data() { Success = false });

            }
        }

        public ActionResult SortNotes(int? id = 0, int? notebook_id = 0, int? option = 2, int? direction = 2)
        {
            var notes = noteRepository.GetAll().Where(x => x.UserId == id);
            if (notebook_id != 0)
                notes = noteRepository.GetAll().Where(x => x.NotebookId == notebook_id);
            switch (option)
            {
                case 1:
                    if (direction == 2)
                        notes = notes.OrderBy(x => x.Title);
                    else
                        notes = notes.OrderByDescending(x => x.Title);
                    break;
                case 2:
                    if (direction == 2)
                        notes = notes.OrderBy(x => x.Modified);
                    else
                        notes = notes.OrderByDescending(x => x.Modified);
                    break;
                case 3:
                    if (direction == 2)
                        notes = notes.OrderBy(x => x.Created);
                    else
                        notes = notes.OrderByDescending(x => x.Created);
                    break;
            }

            return Json(new { Data = notes });
        }
    }
}
