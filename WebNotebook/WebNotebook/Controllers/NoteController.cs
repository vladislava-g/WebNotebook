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
            ViewBag.Notes = noteRepository.GetAll().Where(x => x.NotebookId == id).ToList();
            return PartialView("~/Views/Home/_Notes.cshtml");
        }

        public IActionResult GetNotes(int? id) //by user
        {
            ViewBag.Notes = noteRepository.GetAll().Where(x => x.UserId == id).ToList();
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
    }
}
