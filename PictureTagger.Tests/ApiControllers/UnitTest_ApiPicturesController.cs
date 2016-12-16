using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PictureTagger.Models;
using PictureTagger.Models.ApiModels;
using PictureTagger.Tests.Repositories;
using PictureTagger.ApiControllers;
using System.Linq;
using System.Web.Http.Results;

namespace PictureTagger.Tests.ApiControllers
{
    /// <summary>
    /// Summary description for ApiPicturesControllerTest
    /// </summary>
    [TestClass]
    public class UnitTest_ApiPicturesController
    {
        private static readonly byte[] TEST_BMP = new byte[] { 66, 77, 58, 0, 0, 0, 0, 0, 0, 0, 54, 0, 0, 0, 40, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 196, 14, 0, 0, 196, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255 };

        [TestMethod]
        public void ApiPicturesController_Index()
        {
            //Arrange
            var picturesRepo = new FakeRepository<Picture>(e => e.PictureID);
            picturesRepo.Create(new Picture()
            {
                PictureID = 1,
                OwnerID = "OwnerID",
                Name = "Testing",
                Hash = "Testing",
                ThumbnailData = TEST_BMP
            });

            //Act
            var controller = new PicturesController(picturesRepo);
            var result = controller.GetPictures() as IQueryable<PictureApi>;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ToList());
            Assert.IsTrue(result.ToList().Count() > 0);
        }

        [TestMethod]
        public void ApiPicturesController_Details()
        {
            //Arrange
            var picturesRepo = new FakeRepository<Picture>(e => e.PictureID);
            picturesRepo.Create(new Picture()
            {
                PictureID = 1,
                OwnerID = "OwnerID",
                Name = "Testing",
                Hash = "Testing",
                ThumbnailData = TEST_BMP
            });

            //Act
            var controller = new PicturesController(picturesRepo);
            var result = controller.GetPicture(1) as OkNegotiatedContentResult<PictureApi>;

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
