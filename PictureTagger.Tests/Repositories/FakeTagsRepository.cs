using PictureTagger.Models;

namespace PictureTagger.Tests.Repositories
{
    public class FakeTagsRepository : FakeRepository<Tag>
    {
        public override int EntityId(Tag _model)
        {
            return _model.TagID;
        }
    }
}