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
    /// <summary>
    /// Summary description for TagsControllerTest
    /// </summary>
    [TestClass]
    public class UnitTest_TagsController
    {
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
            var tagsRepo = new FakeRepository<Tag>(e => e.TagID);

            //Act
            var controller = new TagsController(tagsRepo);
            controller.ControllerContext = new ControllerContext(Context.Object, new RouteData(), controller);
            var result = controller.Index() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TagsController_Details()
        {
            //Arrange
            var tagsRepo = new FakeRepository<Tag>(e => e.TagID);
            tagsRepo.Create(new Tag() {
                TagID = 1,
                TagLabel = "testing"
            });

            //Act
            var controller = new TagsController(tagsRepo);
            controller.ControllerContext = new ControllerContext(Context.Object, new RouteData(), controller);
            var result = controller.Details(1) as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
