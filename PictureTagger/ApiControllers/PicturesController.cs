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
using Microsoft.AspNet.Identity;
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
		public IQueryable<PictureApi> GetPictures()
		{
			return dbPicturesRepository.Get().Select(p => new PictureApi()
			{
				PictureID = p.PictureID,
				Name = p.Name,
				Base64Data = $"data:image/{p.FileType};base64,{Convert.ToBase64String(p.Data)}",
				OwnerID = p.AspNetUser.UserName
			});
		}

		// GET: api/Pictures/5
		[ResponseType(typeof(PictureApi))]
		public IHttpActionResult GetPicture(int id)
		{
			Picture picture = dbPicturesRepository.Get(id);
			if (picture == null)
			{
				return NotFound();
			}

			PictureApi pictureApi = new PictureApi()
			{
				PictureID = picture.PictureID,
				Name = picture.Name,
				Base64Data = $"data:image/{picture.FileType};base64,{Convert.ToBase64String(picture.Data)}",
				OwnerID = picture.AspNetUser.UserName
			};

			return Ok(pictureApi);
		}

		// GET: api/Pictures/5/imgage
		[ResponseType(typeof(Bitmap))]
		public HttpResponseMessage GetPictureRaw(int id)
		{
			Picture picture = dbPicturesRepository.Get(id);
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
		public IHttpActionResult PutPicture(int id, Picture picture)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != picture.PictureID)
			{
				return BadRequest();
			}

			try
			{
				dbPicturesRepository.Update(picture);
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

		//TODO Fix Put.

		// POST: api/Pictures
		[ResponseType(typeof(PictureApi))]
		public IHttpActionResult PostPicture(PictureApi pictureApi)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			Picture picture = new Picture()
			{
				Name = pictureApi.Name,
				Data = pictureApi.Data,
				FileType = pictureApi.FileType,
				OwnerID = User.Identity.GetUserId(),
				Tags = dbTagsRepository.Get().Where(t => pictureApi.TagsIds.Contains(t.TagID)).ToList()
			};

			dbPicturesRepository.Create(picture);

			pictureApi = new PictureApi()
			{
				Base64Data = $"data:image/{picture.FileType};base64,{Convert.ToBase64String(picture.Data)}",
				OwnerID = User.Identity.GetUserName()
			};

			return CreatedAtRoute("DefaultApi", new { id = pictureApi.PictureID }, pictureApi);
		}

		// DELETE: api/Pictures/5
		[ResponseType(typeof(Picture))]
		public IHttpActionResult DeletePicture(int id)
		{
			Picture picture = dbPicturesRepository.Get(id);
			if (picture == null)
			{
				return NotFound();
			}

			picture.Tags.Clear();

			dbPicturesRepository.Delete(id);

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
			return dbPicturesRepository.Get().Count(e => e.PictureID == id) > 0;
		}
	}
}