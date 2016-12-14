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

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Tags/Suggestions/{partialTag}")]
        public IQueryable<TagApi> Suggestions(string partialTag)
        {
            return _db.GetAll().ToList().RealCast<TagApi>().AsQueryable().Where(t => t.TagLabel.Contains(partialTag));
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