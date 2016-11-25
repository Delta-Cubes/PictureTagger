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

namespace PictureTagger.ApiControllers
{
    [Authorize]
    public class PicturesController : ApiController
    {
        private PictureTaggerContext db = new PictureTaggerContext();

        public PicturesController()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
        }

        // GET: api/Pictures
        public IQueryable<PictureAPIModel> GetPictures()
        {
            return db.Pictures.Select(p => new PictureAPIModel()
            {
                PictureID = p.PictureID,
                Name = p.Name,
                Base64Data = $"data:image/{p.FileType};base64,{Convert.ToBase64String(p.Data)}",
                OwnerID = p.AspNetUser.UserName
            });
        }

        // GET: api/Pictures/5
        [ResponseType(typeof(PictureAPIModel))]
        public IHttpActionResult GetPicture(int id)
        {
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return NotFound();
            }

            PictureAPIModel pictureApi = new PictureAPIModel()
            {
                PictureID = picture.PictureID,
                Name = picture.Name,
                Base64Data = $"data:image/{picture.FileType};base64,{Convert.ToBase64String(picture.Data)}",
                OwnerID = picture.AspNetUser.UserName
            };

            return Ok(pictureApi);
        }

        // GET: api/Pictures/5/imgage
        [Route("/api/Pictures/{id}/image")]
        [ResponseType(typeof(Bitmap))]
        public HttpResponseMessage GetPictureRaw(int id)
        {
            Picture picture = db.Pictures.Find(id);
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

            db.Entry(picture).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
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
        [ResponseType(typeof(PictureAPIModel))]
        public IHttpActionResult PostPicture(PictureAPIModel pictureApi)
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
                Tags = db.Tags.Where(t => pictureApi.TagsIds.Contains(t.TagID)).ToList()
            };

            db.Pictures.Add(picture);
            db.SaveChanges();

            pictureApi = new PictureAPIModel()
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
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return NotFound();
            }

            picture.Tags.Clear();

            db.Pictures.Remove(picture);
            db.SaveChanges();

            return Ok(picture);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PictureExists(int id)
        {
            return db.Pictures.Count(e => e.PictureID == id) > 0;
        }
    }
}