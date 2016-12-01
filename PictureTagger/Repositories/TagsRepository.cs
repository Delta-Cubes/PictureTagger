using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PictureTagger.Models;

namespace PictureTagger.Repositories
{
    public class TagRepository : DatabaseRepository<Tag>
    {
        public TagRepository() {}

        public TagRepository(bool isApiController) : base(isApiController) {}

        public override IQueryable<Tag> Get()
        {
            return dbContext.Tags;
        }

        public override Tag Get(int? id)
        {
            return dbContext.Tags.Find(id);
        }

        public override void Post(Tag _model)
        {
            dbContext.Tags.Add(_model);
            dbContext.SaveChanges();
        }

        public override void Put(Tag _model)
        {
            dbContext.Entry(_model).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public override void Delete(int? id)
        {
            Tag _model = dbContext.Tags.Find(id);
            dbContext.Tags.Remove(_model);
            dbContext.SaveChanges();
        }
    }
}