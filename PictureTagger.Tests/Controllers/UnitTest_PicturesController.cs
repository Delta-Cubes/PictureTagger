using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PictureTagger.Models;
using PictureTagger.Models.ViewModels;
using System.Collections.Generic;
using Moq;
using System.Web;
using System.IO;
using PictureTagger.Controllers;
using System.Web.Mvc;
using PictureTagger.Tests.Repositories;
using System.Linq;
using System.Security.Principal;
using System.Web.Routing;

namespace PictureTagger.Tests.Controllers
{
	[TestClass]
	public class UnitTest_PicturesController
	{
		private static readonly byte[] TEST_BMP = new byte[] { 66, 77, 58, 0, 0, 0, 0, 0, 0, 0, 54, 0, 0, 0, 40, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 196, 14, 0, 0, 196, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255 };
		internal Mock<HttpContextBase> Context;
		internal Mock<HttpRequestBase> Request;

		[TestInitialize]
		public void Initialize()
		{
			// Set up a fake HTTP context for each test that requires it
			var fakeIdentity = new GenericIdentity("User");
			var principal = new GenericPrincipal(fakeIdentity, null);
			var context = new Mock<HttpContextBase>();
			var controllerContext = new Mock<ControllerContext>();
			var request = new Mock<HttpRequestBase>();
			context.Setup(t => t.User).Returns(principal);
			context.Setup(t => t.Server.MapPath(It.IsAny<string>())).Returns((string a) => a.Replace("~/", @"C:\Temp\"));
			context.Setup(ctx => ctx.Request).Returns(request.Object);
			controllerContext.Setup(t => t.HttpContext).Returns(context.Object);

			Context = context;
			Request = request;
		}

        [TestMethod]
        public void TagsController_Index()
        {
            //Arrange
            var pictureRepo = new FakeRepository<Picture>(p => p.PictureID);
            var tagsRepo = new FakeRepository<Tag>(e => e.TagID);

            //Act
            var controller = new PicturesController(pictureRepo, tagsRepo);
            controller.ControllerContext = new ControllerContext(Context.Object, new RouteData(), controller);
            var result = controller.Index() as ViewResult;

            //Assert
            Assert.IsNotNull(result);           
        }

        [TestMethod]
        public void TagsController_Details()
        {
            //Arrange
            var pictureRepo = new FakeRepository<Picture>(p => p.PictureID);
            var tagsRepo = new FakeRepository<Tag>(e => e.TagID);

            var postedfilesKeyCollection = new Mock<HttpFileCollectionBase>();
            var fakeFileKeys = new List<string>() { "file" };
            var postedfile = new Mock<HttpPostedFileBase>();

            Request.Setup(req => req.Files).Returns(postedfilesKeyCollection.Object);
            postedfilesKeyCollection.Setup(keys => keys.GetEnumerator()).Returns(fakeFileKeys.GetEnumerator());
            postedfilesKeyCollection.Setup(keys => keys["file"]).Returns(postedfile.Object);
            postedfile.Setup(e => e.InputStream).Returns(new MemoryStream(TEST_BMP));
            postedfile.Setup(e => e.FileName).Returns("filename");
            postedfile.Setup(e => e.ContentLength).Returns(TEST_BMP.Length);
            postedfile.Verify(f => f.SaveAs(It.IsAny<string>()), Times.AtMostOnce());

            //Act
            var controller = new PicturesController(pictureRepo, tagsRepo);
            controller.ControllerContext = new ControllerContext(Context.Object, new RouteData(), controller);
            var result = controller.Create(postedfile.Object, "tag1, tag2, tag3", "custom name") as RedirectToRouteResult;
            var inserted = pictureRepo.GetAll().FirstOrDefault();
            var detailsResult = controller.Details(inserted.PictureID) as ViewResult;

            //Assert
            Assert.IsNotNull(result);  // Check if the view returned a valid result
            Assert.IsNotNull(inserted);  // Check if the picture was inserted
            Assert.IsTrue(new string[] { "tag1", "tag2", "tag3" }.SequenceEqual(inserted.Tags.Select(t => t.TagLabel)));  // Check if the tags were set correctly
            Assert.IsNotNull(detailsResult);
        }

        [TestMethod]
        public void PictureController_Create()
        {
            //Arrange
            var pictureRepo = new FakeRepository<Picture>(p => p.PictureID);
            var tagsRepo = new FakeRepository<Tag>(e => e.TagID);

            var postedfilesKeyCollection = new Mock<HttpFileCollectionBase>();
            var fakeFileKeys = new List<string>() { "file" };
            var postedfile = new Mock<HttpPostedFileBase>();

            Request.Setup(req => req.Files).Returns(postedfilesKeyCollection.Object);
            postedfilesKeyCollection.Setup(keys => keys.GetEnumerator()).Returns(fakeFileKeys.GetEnumerator());
            postedfilesKeyCollection.Setup(keys => keys["file"]).Returns(postedfile.Object);
            postedfile.Setup(e => e.InputStream).Returns(new MemoryStream(TEST_BMP));
            postedfile.Setup(e => e.FileName).Returns("filename");
            postedfile.Setup(e => e.ContentLength).Returns(TEST_BMP.Length);
            postedfile.Verify(f => f.SaveAs(It.IsAny<string>()), Times.AtMostOnce());

            //Act
            var controller = new PicturesController(pictureRepo, tagsRepo);
            controller.ControllerContext = new ControllerContext(Context.Object, new RouteData(), controller);
            var result = controller.Create(postedfile.Object, "tag1, tag2, tag3", "custom name") as RedirectToRouteResult;
            var inserted = pictureRepo.GetAll().FirstOrDefault();

            //Assert
            Assert.IsNotNull(result);  // Check if the view returned a valid result
            Assert.IsNotNull(inserted);  // Check if the picture was inserted
            Assert.IsTrue(new string[] { "tag1", "tag2", "tag3" }.SequenceEqual(inserted.Tags.Select(t => t.TagLabel)));  // Check if the tags were set correctly
        }

        [TestMethod]
        public void PictureController_Update()
        {
            //Arrange
            var pictureRepo = new FakeRepository<Picture>(p => p.PictureID);
            var tagsRepo = new FakeRepository<Tag>(e => e.TagID);

            var postedfilesKeyCollection = new Mock<HttpFileCollectionBase>();
            var fakeFileKeys = new List<string>() { "file" };
            var postedfile = new Mock<HttpPostedFileBase>();

            Request.Setup(req => req.Files).Returns(postedfilesKeyCollection.Object);
            postedfilesKeyCollection.Setup(keys => keys.GetEnumerator()).Returns(fakeFileKeys.GetEnumerator());
            postedfilesKeyCollection.Setup(keys => keys["file"]).Returns(postedfile.Object);
            postedfile.Setup(e => e.InputStream).Returns(new MemoryStream(TEST_BMP));
            postedfile.Setup(e => e.FileName).Returns("filename");
            postedfile.Setup(e => e.ContentLength).Returns(TEST_BMP.Length);
            postedfile.Verify(f => f.SaveAs(It.IsAny<string>()), Times.AtMostOnce());

            //Act
            var controller = new PicturesController(pictureRepo, tagsRepo);
            controller.ControllerContext = new ControllerContext(Context.Object, new RouteData(), controller);
            var createResult = controller.Create(postedfile.Object, "tag1, tag2, tag3", "custom name") as RedirectToRouteResult;
            var inserted = pictureRepo.GetAll().FirstOrDefault();
            var updateResult = controller.Edit((PictureView)inserted, "tag1, tag2") as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(createResult);  // Check if the view returned a valid result
            Assert.IsNotNull(inserted);  // Check if the picture was inserted
            Assert.IsNotNull(updateResult); // Check if the view returned a valid result.
            Assert.IsTrue(new string[] { "tag1", "tag2" }.SequenceEqual(inserted.Tags.Select(t => t.TagLabel)));  // Check if the tags were set correctly
        }

        [TestMethod]
        public void PictureController_Delete()
        {
            //Arrange
            var pictureRepo = new FakeRepository<Picture>(p => p.PictureID);
            var tagsRepo = new FakeRepository<Tag>(e => e.TagID);

            var postedfilesKeyCollection = new Mock<HttpFileCollectionBase>();
            var fakeFileKeys = new List<string>() { "file" };
            var postedfile = new Mock<HttpPostedFileBase>();

            Request.Setup(req => req.Files).Returns(postedfilesKeyCollection.Object);
            postedfilesKeyCollection.Setup(keys => keys.GetEnumerator()).Returns(fakeFileKeys.GetEnumerator());
            postedfilesKeyCollection.Setup(keys => keys["file"]).Returns(postedfile.Object);
            postedfile.Setup(e => e.InputStream).Returns(new MemoryStream(TEST_BMP));
            postedfile.Setup(e => e.FileName).Returns("filename");
            postedfile.Setup(e => e.ContentLength).Returns(TEST_BMP.Length);
            postedfile.Verify(f => f.SaveAs(It.IsAny<string>()), Times.AtMostOnce());

            //Act
            var controller = new PicturesController(pictureRepo, tagsRepo);
            controller.ControllerContext = new ControllerContext(Context.Object, new RouteData(), controller);
            var createResult = controller.Create(postedfile.Object, "tag1, tag2, tag3", "custom name") as RedirectToRouteResult;
            var inserted = pictureRepo.GetAll().FirstOrDefault();
            var deleteResult = controller.DeleteConfirmed(inserted.PictureID) as RedirectToRouteResult;
            var insertedCount = pictureRepo.GetAll().Count();

            //Assert
            Assert.IsNotNull(createResult);  // Check if the view returned a valid result
            Assert.IsNotNull(inserted);  // Check if the picture was inserted
            Assert.IsNotNull(deleteResult); // Check if the view returned a valid result.
            Assert.IsTrue(insertedCount == 0); // Checks if the picture was deleted.
        }

    }
}
