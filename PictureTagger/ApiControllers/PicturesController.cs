using System.Linq;
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
        private IRepository<Picture> _db;

		public PicturesController() : this(new DatabaseRepository<Picture>(new PictureTaggerContext(), false))
		{
		}

        public PicturesController(IRepository<Picture> dbPictures)
        {
			_db = dbPictures;
        }

        // GET: api/Pictures
        [AllowAnonymous]
        public IQueryable<PictureApi> GetPictures()
        {
            return _db.GetAll().ToList().RealCast<PictureApi>().AsQueryable();
        }

        // GET: api/Pictures/5
        [AllowAnonymous]
        [ResponseType(typeof(PictureApi))]
        public IHttpActionResult GetPicture(int id)
        {
            Picture picture = _db.Find(id);
            if (picture == null)
            {
                return NotFound();
            }

            return Ok((PictureApi)picture);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PictureExists(int id)
        {
            return _db.GetAll().Count(e => e.PictureID == id) > 0;
        }
    }
}