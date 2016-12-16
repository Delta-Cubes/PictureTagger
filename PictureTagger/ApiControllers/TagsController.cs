using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using PictureTagger.Models;
using PictureTagger.Models.ApiModels;
using PictureTagger.Repositories;
using System.Collections.Generic;

namespace PictureTagger.ApiControllers
{
	[Authorize]
	public class TagsController : ApiController
	{
		private IRepository<Tag> _db;

		public TagsController() : this(new DatabaseRepository<Tag>(new PictureTaggerContext(), false))
		{
		}

		public TagsController(IRepository<Tag> db)
		{
			_db = db;
		}

		// GET: api/Tags
		[AllowAnonymous]
		public IEnumerable<TagApi> GetTags()
		{
			return _db.GetAll().ToList().RealCast<TagApi>();
		}

		// GET: api/Tags/5
		[AllowAnonymous]
		[ResponseType(typeof(PictureApi))]
		public IHttpActionResult GetTag(int id)
		{
			Tag tag = _db.Find(id);
			if (tag == null)
			{
				return NotFound();
			}
			return Ok((TagApi)tag);
		}

		[AllowAnonymous]
		[HttpGet]
		[Route("api/Tags/Suggestions/{partialTag}")]
		public IEnumerable<TagApi> Suggestions(string partialTag)
		{
			return _db.GetAll().Where(t => t.TagLabel.Contains(partialTag)).ToList().RealCast<TagApi>();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_db.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool TagExists(int id)
		{
			return _db.GetAll().Count(e => e.TagID == id) > 0;
		}
	}
}