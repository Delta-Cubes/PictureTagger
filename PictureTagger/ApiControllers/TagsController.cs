using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
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
        private IRepository<Tag> _db;

        public TagsController() : this(new DatabaseRepository<Tag>(false))
        {
        }

        public TagsController(IRepository<Tag> db)
        {
            this._db = db;
        }

        // GET: api/Tags
        [AllowAnonymous]
        public IQueryable<TagApi> GetTags()
        {
            return _db.GetAll().RealCast<TagApi>();
        }

        // GET: api/Tags/5
        [AllowAnonymous]
        [ResponseType(typeof(TagApi))]
        public IHttpActionResult GetTag(int id)
        {
            Tag tag = _db.Find(id);
            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag);
        }

        [AllowAnonymous]
        [Route("api/Tags/Suggestions/{partialTag}")]
        public IQueryable<TagApi> Suggestions(string partialTag)
        {
            return _db.GetAll().Where(t => t.TagLabel.Contains(partialTag)).RealCast<TagApi>();
        }

        // PUT: api/Tags/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTag(int id, TagApi tagApi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tagApi.TagID)
            {
                return BadRequest();
            }

            try
            {
                _db.Update(tagApi);
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

        // POST: api/Tags
        [ResponseType(typeof(Tag))]
        public IHttpActionResult PostTag(TagApi tagApi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Create(tagApi);

            return CreatedAtRoute("DefaultApi", new { id = tagApi.TagID }, tagApi);
        }

        // DELETE: api/Tags/5
        [ResponseType(typeof(Tag))]
        public IHttpActionResult DeleteTag(int id)
        {
            Tag tag = _db.Find(id);
            if (tag == null)
            {
                return NotFound();
            }

            _db.Delete(tag);

            return Ok(tag);
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