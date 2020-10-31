using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNotebook.Infrastructure;
using WebNotebook.Models;

namespace WebNotebook.Controllers
{
    [Authorize]
    public class NotebookController : Controller
    {

        IRepository<Notebook> notebookRepository;
        IRepository<DefaultImage> imagesRepository;

        public NotebookController(IRepository<Notebook> notebookRepository,
            IRepository<DefaultImage> imagesRepository)
        {
            this.notebookRepository = notebookRepository;
            this.imagesRepository = imagesRepository;
        }
        public ActionResult GetNotebooks(int? id = 0)
        {
            ViewBag.Notebooks = notebookRepository.GetAll().Where(x => x.CreatorId == id).OrderByDescending(x => x.Modified);
            ViewBag.UserId = id;
            return PartialView("~/Views/Home/_Notebooks.cshtml");
        }

        public ActionResult Create(int id = 0)
        {
            int? default_img = notebookRepository.GetAll().Select(x => x.DefaultImage).Max();
            var image = imagesRepository.GetAll().Where(x => x.Id == (default_img != null && default_img != 10 ? default_img + 1 : 1)).First();

            var notebook = new Notebook();
            notebook.CreatorId = id;
            notebook.Name = "Untitled";
            notebook.Created = DateTime.Now;
            notebook.Modified = DateTime.Now;
            notebook.IsDefault = 0;
            notebook.Cover = image.Url;
            notebook.DefaultImage = image.Id;

            notebookRepository.Create(notebook);

            return Json(notebook);
        }

        public ActionResult Delete(int id = 0)
        {
            try {
                notebookRepository.Delete(notebookRepository.Get(id));
                return Json(new Data() { Success = true} );
            }
            catch
            {
                return Json(new Data() { Success = false} );

            }
        }

        public ActionResult Update(int id = 0, string name = "", string cover = "")
        {
            try
            {
                var notebook = notebookRepository.Get(id);
                if (name != "")
                    notebook.Name = name;
                if (cover != "")
                    notebook.Cover = cover;

                notebookRepository.SaveChanges();
                return Json(new Data() { Success = true });
            }
            catch
            {
                return Json(new Data() { Success = false });

            }
        }

    }
}