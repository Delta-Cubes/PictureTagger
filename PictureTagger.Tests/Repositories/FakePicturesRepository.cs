using PictureTagger.Models;

namespace PictureTagger.Tests.Repositories
{
    public class FakePicturesRepository : FakeRepository<Picture>
    {
        public override int EntityId(Picture _model)
        {
            return _model.PictureID;
        }
    }
}