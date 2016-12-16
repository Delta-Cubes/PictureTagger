using PictureTagger.Infrastructure;
using PictureTagger.Models;
using PictureTagger.Models.ViewModels;
using PictureTagger.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace PictureTagger.Controllers
{
	[Authorize]
	public class PicturesController : Controller
	{
		private IRepository<Picture> _pictureRepo;
		private IRepository<Tag> _tagRepo;

		public PicturesController(IRepository<Picture> pictureRepo, IRepository<Tag> tagRepo)
		{
			_pictureRepo = pictureRepo;
			_tagRepo = tagRepo;
		}

		// GET: Pictures
		[AllowAnonymous]
		public ActionResult Index()
		{
			return View(_pictureRepo.GetAll().ToList().RealCast<PictureView>());
		}

		// GET: Pictures/Details/5
		[AllowAnonymous]
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Picture picture = _pictureRepo.Find(id);
			if (picture == null)
			{
				return HttpNotFound();
			}
			return View((PictureView)picture);
		}

		// GET: Pictures/Create
		public ActionResult Create()
		{
			return View(new PictureView());
		}

		// POST: Pictures/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(HttpPostedFileBase fileBase, string tags, string name)
		{
			var ownerId = User.Identity.GetUserId();

			if (ModelState.IsValid)
			{
				// Handle multiple files
				foreach (string key in Request.Files)
				{
					if (Request.Files[key]?.ContentLength == 0) continue;

					// Get file object
					var f = Request.Files[key] as HttpPostedFileBase;

					// Save image locally with hash as the filename
					string hash;
					using (var sha1 = new SHA1CryptoServiceProvider())
					{
						hash = Convert.ToBase64String(sha1.ComputeHash(f.InputStream));
					}

					hash = hash.SafeString();

					var filename = Server.MapPath(@"~\UserData\");

					// Save to UserData folder
					Directory.CreateDirectory(filename);
					f.SaveAs(filename + hash);

					// Create a new Picture from the file
					var picture = new Picture
					{
						OwnerID = ownerId,
						Name = name,
						Hash = hash,
						ThumbnailData = ThumbnailGenerator.Generate(f.InputStream),
						Tags = ResolveTags(tags)
					};

					// Add the picture
					_pictureRepo.Create(picture);
				}

				return RedirectToAction("Index");
			}

			return View();
		}

		private Tag ResolveTag(string tag)
		{
			var resolved = _tagRepo
				.GetAll()
				.FirstOrDefault(t => t.TagLabel.Equals(tag, StringComparison.OrdinalIgnoreCase));

			return resolved ?? new Tag
			{
				TagLabel = tag
			};
		}

		private ICollection<Tag> ResolveTags(string tags)
		{
			return tags
				.Split(',')
				.Select(t => t.Trim())
				.Where(t => t.Length > 0)
				.Distinct()
				.Select(t => ResolveTag(t))
				.ToList();
		}

		// GET: Pictures/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Picture picture = _pictureRepo.Find(id);
			if (picture == null)
			{
				return HttpNotFound();
			}

			if (picture.OwnerID != User.Identity.GetUserId())
			{
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
			}

			return View((PictureView)picture);
		}

		// POST: Pictures/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "PictureID,Name")] PictureView pictureView, string tags)
		{
			Picture picture = _pictureRepo.Find(pictureView.PictureID);

			if (picture.OwnerID != User.Identity.GetUserId())
			{
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
			}

			if (ModelState.IsValid)
			{
				picture.Tags.Clear();
				picture.Name = pictureView.Name;
				picture.Tags = ResolveTags(tags);
				_pictureRepo.Update(picture);
				return RedirectToAction("Index");
			}

			return View((PictureView)picture);
		}

		// GET: Pictures/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Picture picture = _pictureRepo.Find(id);
			if (picture == null)
			{
				return HttpNotFound();
			}

			if (picture.OwnerID != User.Identity.GetUserId())
			{
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
			}

			return View((PictureView)picture);
		}

		// POST: Pictures/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Picture picture = _pictureRepo.Find(id);
			if (picture.OwnerID != User.Identity.GetUserId())
			{
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
			}
			_pictureRepo.Delete(id);
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_pictureRepo.Dispose();
				_tagRepo.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}