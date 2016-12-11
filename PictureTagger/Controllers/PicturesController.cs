using PictureTagger.Models;
using PictureTagger.Models.ViewModels;
using PictureTagger.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace PictureTagger.Controllers
{
	[Authorize]
	public class PicturesController : Controller
	{
		private IRepository<Picture> _db;

		public PicturesController() : this(new DatabaseRepository<Picture>())
		{ }

		public PicturesController(IRepository<Picture> db)
		{
			_db = db;
		}

		// GET: Pictures/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Pictures/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(HttpPostedFileBase fileBase, string tags)
		{
			var claims = HttpContext.User.GetClaims();
			var ownerId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (ownerId == null) return View();

			if (ModelState.IsValid)
			{
				var tagRepo = new DatabaseRepository<Tag>();

				// Handle multiple files
				foreach (string key in Request.Files)
				{
					if (Request.Files[key]?.ContentLength == 0) continue;

					// Get file object
					var f = Request.Files[key] as HttpPostedFileBase;

					// Get binary data from file
					byte[] buffer = new byte[f.ContentLength];
					using (var reader = new BinaryReader(f.InputStream))
					{
						buffer = reader.ReadBytes(f.ContentLength);
					}

					// Create a new Picture from the file
					var picture = new Picture
					{
						OwnerID = ownerId,
						Name = f.FileName,
						Data = buffer,
						FileType = f.ContentType,
						Tags = tags
							.Split(',')
							.Select(t => ResolveTag(tagRepo, t))
							.ToList()
					};

					// Add the picture
					_db.Create(picture);
				}

				return RedirectToAction("Index");
			}

			return View();
		}

		private static Tag ResolveTag(IRepository<Tag> repo, string tag)
		{
			tag = tag.Trim();

			var resolved = repo
				.GetAll()
				.FirstOrDefault(t => t.TagLabel.Equals(tag, StringComparison.OrdinalIgnoreCase));

			return resolved ?? new Tag
			{
				TagLabel = tag
			};
		}

		// GET: Pictures/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Picture picture = _db.Find(id);
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
			Picture picture = _db.Find(id);
			_db.Delete(id);
			return RedirectToAction("Index");
		}

		// GET: Pictures/Details/5
		[AllowAnonymous]
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Picture picture = _db.Find(id);
			if (picture == null)
			{
				return HttpNotFound();
			}
			return View(picture);
		}

		// GET: Pictures/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Picture picture = _db.Find(id);
			if (picture == null)
			{
				return HttpNotFound();
			}

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
				_db.Update(picture);
				return RedirectToAction("Index");
			}

			return View(picture);
		}

		// GET: Pictures
		[AllowAnonymous]
		public ActionResult Index()
		{
			var pictures = _db.GetAll();
			return View(pictures.ToList().RealCast<PictureView>());
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