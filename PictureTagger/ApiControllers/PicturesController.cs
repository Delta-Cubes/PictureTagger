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

namespace PictureTagger.ApiControllers
{
	[Authorize]
	public class PicturesController : ApiController
	{
		private IRepository<Picture> dbPicturesRepository;
		private IRepository<Tag> dbTagsRepository;

		public PicturesController() : this(new DatabaseRepository<Picture>(true), new DatabaseRepository<Tag>(true))
		{
		}

		public PicturesController(IRepository<Picture> dbPicturesRepository, IRepository<Tag> dbTagsRepository)
		{
			this.dbPicturesRepository = dbPicturesRepository;
			this.dbTagsRepository = dbTagsRepository;
		}

		// GET: api/Pictures
		[AllowAnonymous]
		public IQueryable<PictureApi> GetPictures()
		{
			return dbPicturesRepository.GetAll().RealCast<PictureApi>();
		}

		// GET: api/Pictures/5
		[AllowAnonymous]
		[ResponseType(typeof(PictureApi))]
		public IHttpActionResult GetPicture(int id)
		{
			Picture picture = dbPicturesRepository.Find(id);
			if (picture == null)
			{
				return NotFound();
			}

			return Ok(picture);
		}

		// GET: api/Pictures/5/
		[AllowAnonymous]
		[Route("api/Pictures/{id}/image")]
		[ResponseType(typeof(Bitmap))]
		public HttpResponseMessage GetPictureRaw(int id)
		{
			Picture picture = dbPicturesRepository.Find(id);
			if (picture == null)
			{
				return (new HttpResponseMessage(HttpStatusCode.NotFound));
			}

			var pictureRawResponse = new HttpResponseMessage()
			{
				Content = new ByteArrayContent(picture.Data)
			};
			pictureRawResponse.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
			{

			};
			pictureRawResponse.Content.Headers.ContentType = new MediaTypeHeaderValue($"image/{picture.FileType}");
			return pictureRawResponse;
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
				dbPicturesRepository.Update(pictureApi);
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

			dbPicturesRepository.Create(pictureApi);

			return CreatedAtRoute("DefaultApi", new { id = pictureApi.PictureID }, pictureApi);
		}

		// DELETE: api/Pictures/5
		[ResponseType(typeof(PictureApi))]
		public IHttpActionResult DeletePicture(int id)
		{
			Picture picture = dbPicturesRepository.Find(id);
			if (picture == null)
			{
				return NotFound();
			}

			dbPicturesRepository.Delete(picture);

			return Ok(picture);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				dbPicturesRepository.Dispose();
				dbTagsRepository.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool PictureExists(int id)
		{
			return dbPicturesRepository.GetAll().Count(e => e.PictureID == id) > 0;
		}
	}
}