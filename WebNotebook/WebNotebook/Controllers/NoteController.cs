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
            ViewBag.NotebookId = notebookRepository.Get(id).CreatorId;
            ViewBag.Title = notebookRepository.Get(id).Name;
            ViewBag.Notes = noteRepository.GetAll().Where(x => x.NotebookId == id).ToList();
            return PartialView("~/Views/Home/_Notes.cshtml");
        }

        public IActionResult GetNotes(int? id) //by user
        {
            ViewBag.Notes = noteRepository.GetAll().Where(x => x.UserId == id).ToList();
            ViewBag.UserId = id;
            ViewBag.NotebookId = notebookRepository.GetAll().Where(x => x.CreatorId == id && x.IsDefault == 1);
            ViewBag.Title = "Notes";
            ViewBag.GoBackClass = "d-none";
            return PartialView("~/Views/Home/_Notes.cshtml");
        }

       
    }
}
