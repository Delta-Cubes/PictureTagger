using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PictureTagger.Models;
using PictureTagger.Repositories;

namespace PictureTagger.Controllers
{
    [Authorize]
    public class PicturesController : Controller
    {
        private IRepository<Picture> dbPicturesRepository;
        private IRepository<AspNetUser> dbAspNetUsersRepository;

        public PicturesController() : this(new DatabaseRepository<Picture>(), new DatabaseRepository<AspNetUser>())
        {
        }

        public PicturesController(IRepository<Picture> dbPicturesRepository, IRepository<AspNetUser> dbAspNetUsersRepository)
        {
            this.dbPicturesRepository = dbPicturesRepository;
            this.dbAspNetUsersRepository = dbAspNetUsersRepository;
        }

        // GET: Pictures
        [AllowAnonymous]
        public ActionResult Index()
        {
            var pictures = dbPicturesRepository.Get().Include(p => p.AspNetUser);
            return View(pictures.ToList());
        }

        // GET: Pictures/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = dbPicturesRepository.Get(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // GET: Pictures/Create
        public ActionResult Create()
        {
            ViewBag.OwnerID = new SelectList(dbAspNetUsersRepository.Get(), "Id", "Email");
            return View();
        }

        // POST: Pictures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PictureID,OwnerID,Data,FileType,Name")] Picture picture)
        {
            if (ModelState.IsValid)
            {
                dbPicturesRepository.Create(picture);
                return RedirectToAction("Index");
            }

            ViewBag.OwnerID = new SelectList(dbAspNetUsersRepository.Get(), "Id", "Email", picture.OwnerID);
            return View(picture);
        }

        // GET: Pictures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = dbPicturesRepository.Get(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnerID = new SelectList(dbAspNetUsersRepository.Get(), "Id", "Email", picture.OwnerID);
            return View(picture);
        }

        // POST: Pictures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PictureID,OwnerID,Data,FileType,Name")] Picture picture)
        {
            if (ModelState.IsValid)
            {
                dbPicturesRepository.Update(picture);
                return RedirectToAction("Index");
            }
            ViewBag.OwnerID = new SelectList(dbAspNetUsersRepository.Get(), "Id", "Email", picture.OwnerID);
            return View(picture);
        }

        // GET: Pictures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = dbPicturesRepository.Get(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // POST: Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Picture picture = dbPicturesRepository.Get(id);
            dbPicturesRepository.Delete(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbPicturesRepository.Dispose();
                dbAspNetUsersRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
