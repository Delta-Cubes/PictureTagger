using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PictureTagger.Models;
using PictureTagger.Models.ViewModels;
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
            var pictures = dbPicturesRepository.GetAll().Include(p => p.AspNetUser).Cast<PictureView>();
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
            Picture picture = dbPicturesRepository.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // GET: Pictures/Create
        public ActionResult Create()
        {
            ViewBag.OwnerID = new SelectList(dbAspNetUsersRepository.GetAll(), "Id", "Email");
            return View();
        }

        // POST: Pictures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Data,FileType,Name")] PictureView pictureView)
        {
            if (ModelState.IsValid)
            {
                pictureView.OwnerID = User.Identity.GetUserId();
                dbPicturesRepository.Create(pictureView);
                return RedirectToAction("Index");
            }

            ViewBag.OwnerID = new SelectList(dbAspNetUsersRepository.GetAll(), "Id", "Email", pictureView.OwnerID);
            return View(pictureView);
        }

        // GET: Pictures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = dbPicturesRepository.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnerID = new SelectList(dbAspNetUsersRepository.GetAll(), "Id", "Email", picture.OwnerID);
            return View(picture);
        }

        // POST: Pictures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PictureID,Data,FileType,Name")] PictureView pictureVie)
        {
            if (ModelState.IsValid)
            {
                dbPicturesRepository.Update(pictureVie);
                return RedirectToAction("Index");
            }
            ViewBag.OwnerID = new SelectList(dbAspNetUsersRepository.GetAll(), "Id", "Email", pictureVie.OwnerID);
            return View(pictureVie);
        }

        // GET: Pictures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = dbPicturesRepository.Find(id);
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
            Picture picture = dbPicturesRepository.Find(id);
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

