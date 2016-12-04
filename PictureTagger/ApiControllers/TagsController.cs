using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PictureTagger.Models;
using PictureTagger.Models.ApiModels;
using PictureTagger.Repositories;

namespace PictureTagger.ApiControllers
{
	[Authorize]
	public class TagsController : ApiController
	{
		private IRepository<Tag> dbTagsRepository;

		public TagsController() : this(new DatabaseRepository<Tag>(true))
		{
		}

		public TagsController(IRepository<Tag> dbTagsRepository)
		{
			this.dbTagsRepository = dbTagsRepository;
		}

		// GET: api/Tags
		[AllowAnonymous]
		public IQueryable<TagApi> GetTags()
		{
			return dbTagsRepository.Get().Select(t => new TagApi()
			{
				TagID = t.TagID,
				TagLabel = t.TagLabel
			});
		}

		// GET: api/Tags/5
		[ResponseType(typeof(TagApi))]
		[AllowAnonymous]
		public IHttpActionResult GetTag(int id)
		{
			Tag tag = dbTagsRepository.Get(id);
			if (tag == null)
			{
				return NotFound();
			}

			TagApi tagApi = new TagApi()
			{
				TagID = tag.TagID,
				TagLabel = tag.TagLabel
			};

			return Ok(tagApi);
		}

		// PUT: api/Tags/5
		[ResponseType(typeof(void))]
		public IHttpActionResult PutTag(int id, Tag tag)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != tag.TagID)
			{
				return BadRequest();
			}


			try
			{
				dbTagsRepository.Update(tag);
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!TagExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return StatusCode(HttpStatusCode.NoContent);
		}
		//TODO Fix Put

		// POST: api/Tags
		[ResponseType(typeof(TagApi))]
		public IHttpActionResult PostTag(TagApi tagApi)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			Tag tag = new Tag()
			{
				TagLabel = tagApi.TagLabel
			};

			dbTagsRepository.Create(tag);

			return CreatedAtRoute("DefaultApi", new { id = tagApi.TagID }, tagApi);
		}

		// DELETE: api/Tags/5
		[ResponseType(typeof(Tag))]
		public IHttpActionResult DeleteTag(int id)
		{
			Tag tag = dbTagsRepository.Get(id);
			if (tag == null)
			{
				return NotFound();
			}

			tag.Pictures.Clear();

			dbTagsRepository.Delete(id);

			return Ok(tag);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				dbTagsRepository.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool TagExists(int id)
		{
			return dbTagsRepository.Get().Count(e => e.TagID == id) > 0;
		}
	}
}