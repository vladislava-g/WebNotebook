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
            ViewBag.Images = imagesRepository.GetAll();
            return PartialView("~/Views/Home/_Notebooks.cshtml");
        }

        public ActionResult SortNotebooks(int? id = 0, int? option = 2, int? direction = 2)
        {
            var notebooks = notebookRepository.GetAll().Where(x => x.CreatorId == id);
            switch (option)
            {
                case 1:
                    if (direction == 2)
                        notebooks = notebooks.OrderBy(x => x.Name);
                    else
                        notebooks = notebooks.OrderByDescending(x => x.Name);
                    break;
                case 2:
                    if (direction == 2)
                        notebooks = notebooks.OrderBy(x => x.Modified);
                    else
                        notebooks = notebooks.OrderByDescending(x => x.Modified);
                    break;
                case 3:
                    if (direction == 2)
                        notebooks = notebooks.OrderBy(x => x.Created);
                    else
                        notebooks = notebooks.OrderByDescending(x => x.Created);
                    break;
            }

            return Json(new { Data = notebooks });
        }

        public ActionResult GetDefaultImages()
        {
            return Json(new { Data = imagesRepository.GetAll() });
        }

        public ActionResult Create(int id = 0)
        {
            Random rnd = new Random();
            var num = rnd.Next(1, 11);
            var image = imagesRepository.GetAll().Where(x => x.Id == num).FirstOrDefault();

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
            try
            {
                notebookRepository.Delete(notebookRepository.Get(id));
                return Json(new Data() { Success = true });
            }
            catch
            {
                return Json(new Data() { Success = false });

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
                {
                    notebook.Cover = cover;
                    int default_img = imagesRepository.GetAll().Where(x => x.Url == cover).Select(x => x.Id).FirstOrDefault();
                    notebook.DefaultImage = default_img;
                }

                notebook.Modified = DateTime.Now;
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