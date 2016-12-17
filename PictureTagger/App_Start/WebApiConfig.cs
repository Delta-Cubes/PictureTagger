using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PictureTagger
{
	public static class WebApiConfig
	{
		/// <summary>
		/// URL prefix used to access Web API controllers
		/// </summary>
		public static string UrlPrefix { get { return "api"; } }

		/// <summary>
		/// Relative path to the same UrlPrefix
		/// </summary>
		public static string UrlPrefixRelative { get { return "~/api"; } }

		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services
			config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: UrlPrefix + "/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
