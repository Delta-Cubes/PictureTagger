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
using System.Threading.Tasks;

namespace PictureTagger.Controllers
{
	[Authorize]
	public class TagsController : Controller
	{
		private IRepository<Tag> dbTagsRepository;

		public TagsController() : this(new DatabaseRepository<Tag>())
		{
		}

		public TagsController(IRepository<Tag> dbTagsRepository)
		{
			this.dbTagsRepository = dbTagsRepository;
		}

		// GET: Tags
		[AllowAnonymous]
		public ActionResult Index()
		{
			return View(dbTagsRepository.Get().ToList());
		}

		// GET: Tags/Details/5
		[AllowAnonymous]
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Tag tag = dbTagsRepository.Get(id);
			if (tag == null)
			{
				return HttpNotFound();
			}
			return View(tag);
		}

		// GET: Tags/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Tags/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "TagID,TagLabel")] Tag tag)
		{
			if (ModelState.IsValid)
			{
				dbTagsRepository.Create(tag);
				return RedirectToAction("Index");
			}

			return View(tag);
		}

		// GET: Tags/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Tag tag = dbTagsRepository.Get(id);
			if (tag == null)
			{
				return HttpNotFound();
			}
			return View(tag);
		}

		// POST: Tags/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "TagID,TagLabel")] Tag tag)
		{
			if (ModelState.IsValid)
			{
				dbTagsRepository.Update(tag);
				return RedirectToAction("Index");
			}
			return View(tag);
		}

		// GET: Tags/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Tag tag = dbTagsRepository.Get(id);
			if (tag == null)
			{
				return HttpNotFound();
			}
			return View(tag);
		}

		// POST: Tags/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Tag tag = dbTagsRepository.Get(id);
			dbTagsRepository.Delete(id);
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				dbTagsRepository.Dispose();
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Create a new tag or return the existing one
		/// </summary>
		internal static Tag ResolveTag(IRepository<Tag> repo, string tag)
		{
			tag = tag.Trim();

			var existingTag = repo
				.Get()
				.FirstOrDefault(t => t.TagLabel.Equals(tag, StringComparison.OrdinalIgnoreCase));

			return existingTag ?? new Tag
			{
				TagLabel = tag
			};
		}
	}
}
