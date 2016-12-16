using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PictureTagger.Models;
using PictureTagger.Models.ApiModels;
using PictureTagger.Tests.Repositories;
using PictureTagger.ApiControllers;
using System.Web.Http.Results;

namespace PictureTagger.Tests.ApiControllers
{
    /// <summary>
    /// Summary description for ApiTagsControllerTest
    /// </summary>
    [TestClass]
    public class UnitTest_ApiTagsController
    {
        [TestMethod]
        public void ApiTagsController_Index()
        {
            //Arrange
            var tagsRepo = new FakeRepository<Tag>(e => e.TagID);
            tagsRepo.Create(new Tag()
            {
                TagID = 1,
                TagLabel = "testing"
            });

            //Act
            var controller = new TagsController(tagsRepo);
            var result = controller.GetTags() as IQueryable<TagApi>;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ToList());
            Assert.IsTrue(result.ToList().Count() > 0);
        }

        [TestMethod]
        public void ApiTagsController_Details()
        {
            //Arrange
            var tagsRepo = new FakeRepository<Tag>(e => e.TagID);
            tagsRepo.Create(new Tag()
            {
                TagID = 1,
                TagLabel = "testing"
            });

            //Act
            var controller = new TagsController(tagsRepo);
            var result = controller.GetTag(1) as OkNegotiatedContentResult<TagApi>;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ApiTagsController_Suggestions()
        {
            //Arrange
            var tagsRepo = new FakeRepository<Tag>(e => e.TagID);
            tagsRepo.Create(new Tag()
            {
                TagID = 1,
                TagLabel = "testing"
            });

            //Act
            var controller = new TagsController(tagsRepo);
            var result = controller.Suggestions("test") as IQueryable<TagApi>;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ToList());
            Assert.IsTrue(result.ToList().Count() > 0);
        }
    }
}
