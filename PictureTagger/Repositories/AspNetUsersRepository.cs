using System.Data.Entity;
using System.Linq;
using PictureTagger.Models;

namespace PictureTagger.Repositories
{
    public class AspNetUsersRepository : DatabaseRepository<AspNetUser>
    {
        public AspNetUsersRepository() { }

        public AspNetUsersRepository(bool isApiController) : base(isApiController) { }

        public override IQueryable<AspNetUser> Get()
        {
            return dbContext.AspNetUsers;
        }

        public override AspNetUser Get(int? id)
        {
            return dbContext.AspNetUsers.Find(id);
        }

        public override void Post(AspNetUser _model)
        {
            dbContext.AspNetUsers.Add(_model);
            dbContext.SaveChanges();
        }

        public override void Put(AspNetUser _model)
        {
            dbContext.Entry(_model).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public override void Delete(int? id)
        {
            AspNetUser _model = dbContext.AspNetUsers.Find(id);
            dbContext.AspNetUsers.Remove(_model);
            dbContext.SaveChanges();
        }
    }
}