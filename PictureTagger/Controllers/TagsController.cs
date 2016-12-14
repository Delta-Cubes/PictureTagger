using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PictureTagger.Models;
using PictureTagger.Models.ViewModels;
using PictureTagger.Repositories;

namespace PictureTagger.Controllers
{
    [Authorize]
    public class TagsController : Controller
    {
        private IRepository<Tag> _db;

        public TagsController() : this(new DatabaseRepository<Tag>())
		{ }

        public TagsController(IRepository<Tag> db)
        {
            _db = db;
        }

        // GET: Tags
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(_db.GetAll().RealCast<TagView>().ToList());
        }

        // GET: Tags/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = _db.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View((TagView)tag);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
