using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using PictureTagger.Models;
using PictureTagger.Models.ApiModels;
using PictureTagger.Repositories;
using Microsoft.AspNet.Identity;

namespace PictureTagger.ApiControllers
{
	[Authorize]
	public class PicturesController : ApiController
	{
		private IRepository<Picture> _dbPictures;
		private IRepository<Tag> _dbTags;

		public PicturesController() : this(new DatabaseRepository<Picture>(false), new DatabaseRepository<Tag>(false))
		{
		}

		public PicturesController(IRepository<Picture> dbPictures, IRepository<Tag> dbTags)
		{
			this._dbPictures = dbPictures;
			this._dbTags = dbTags;
		}

		// GET: api/Pictures
		[AllowAnonymous]
		public IQueryable<PictureApi> GetPictures()
		{
			return _dbPictures.GetAll().RealCast<PictureApi>();
		}

		// GET: api/Pictures/5
		[AllowAnonymous]
		[ResponseType(typeof(PictureApi))]
		public IHttpActionResult GetPicture(int id)
		{
			Picture picture = _dbPictures.Find(id);
			if (picture == null)
			{
				return NotFound();
			}

			return Ok(picture);
		}

		// PUT: api/Pictures/5
		[ResponseType(typeof(void))]
		public IHttpActionResult PutPicture(int id, PictureApi pictureApi)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != pictureApi.PictureID)
			{
				return BadRequest();
			}

			try
			{
				_dbPictures.Update((Picture)pictureApi);
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!PictureExists(id))
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

		// POST: api/Pictures
		[ResponseType(typeof(PictureApi))]
		public IHttpActionResult PostPicture(PictureApi pictureApi)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
            
            pictureApi.OwnerID = User.Identity.GetUserId();

			_dbPictures.Create((Picture)pictureApi);

			return CreatedAtRoute("DefaultApi", new { id = pictureApi.PictureID }, pictureApi);
		}

		// DELETE: api/Pictures/5
		[ResponseType(typeof(PictureApi))]
		public IHttpActionResult DeletePicture(int id)
		{
			Picture picture = _dbPictures.Find(id);
			if (picture == null)
			{
				return NotFound();
			}

			_dbPictures.Delete((Picture)picture);

			return Ok(picture);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_dbPictures.Dispose();
				_dbTags.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool PictureExists(int id)
		{
			return _dbPictures.GetAll().Count(e => e.PictureID == id) > 0;
		}
	}
}