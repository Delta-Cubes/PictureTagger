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

namespace PictureTagger.ApiControllers
{
    [Authorize]
    public class TagsController : ApiController
    {
        private PictureTaggerContext db = new PictureTaggerContext();

        public TagsController()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
        }

        // GET: api/Tags
        [AllowAnonymous]
        public IQueryable<TagApiModel> GetTags()
        {
            return db.Tags.Select(t => new TagApiModel()
            {
                TagID = t.TagID,
                TagLabel = t.TagLabel
            });
        }

        // GET: api/Tags/5
        [ResponseType(typeof(TagApiModel))]
        [AllowAnonymous]
        public IHttpActionResult GetTag(int id)
        {
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }

            TagApiModel tagApi = new TagApiModel()
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

            db.Entry(tag).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
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
        [ResponseType(typeof(TagApiModel))]
        public IHttpActionResult PostTag(TagApiModel tagApi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Tag tag = new Tag()
            {
                 TagLabel = tagApi.TagLabel
            };

            db.Tags.Add(tag);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tagApi.TagID }, tagApi);
        }

        // DELETE: api/Tags/5
        [ResponseType(typeof(Tag))]
        public IHttpActionResult DeleteTag(int id)
        {
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }

            tag.Pictures.Clear();

            db.Tags.Remove(tag);
            db.SaveChanges();

            return Ok(tag);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TagExists(int id)
        {
            return db.Tags.Count(e => e.TagID == id) > 0;
        }
    }
}