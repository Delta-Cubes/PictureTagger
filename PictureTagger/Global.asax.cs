using PictureTagger.ApiControllers;
using PictureTagger.Controllers;
using PictureTagger.Models;
using PictureTagger.Repositories;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace PictureTagger
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			// Register injectable things
			DI.RegisterTransient<IRepository<Picture>, DatabaseRepository<Picture>>();
			DI.RegisterTransient<IRepository<Tag>, DatabaseRepository<Tag>>();

			// Add DI to our controllers
			ControllerBuilder.Current.SetControllerFactory(typeof(InjectingControllerFactory)); // MVC controllers only, doing this for Web API sucks
		}

		protected void Application_PostAuthorizeRequest()
		{
			if (IsWebApiRequest())
			{
				HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
			}
		}

		protected void Application_BeginRequest()
		{
			HttpContext.Current.Items["Context"] = Guid.NewGuid();
			DI.RegisterScoped(HttpContext.Current.Items["Context"], new PictureTaggerContext());
		}

		protected void Application_EndRequest()
		{
			DI.UnregisterScoped(HttpContext.Current.Items["Context"]);
		}

		private bool IsWebApiRequest()
		{
			return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
		}
	}
}
