using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PictureTagger.Models;

namespace PictureTagger.Repositories
{
    public class PicturesRepository : DatabaseRepository<Picture>
    {
        public PicturesRepository() {}

        public PicturesRepository(bool isApiController) : base(isApiController) {}

        public override IQueryable<Picture> Get()
        {
            return dbContext.Pictures;
        }

        public override Picture Get(int? id)
        {
            return dbContext.Pictures.Find(id);
        }

        public override void Post(Picture _model)
        {
            dbContext.Pictures.Add(_model);
            dbContext.SaveChanges();
        }

        public override void Put(Picture _model)
        {
            dbContext.Entry(_model).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public override void Delete(int? id)
        {
            Picture _model = dbContext.Pictures.Find(id);
            dbContext.Pictures.Remove(_model);
            dbContext.SaveChanges();
        }
    }
}