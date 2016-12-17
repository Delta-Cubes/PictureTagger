using System.Web.Mvc;
using System.Web.Routing;

namespace PictureTagger.Controllers
{
	/// <summary>
	/// Automatically detect and inject registered entities for controllers
	/// </summary>
	public class InjectingControllerFactory : DefaultControllerFactory
	{
		public override IController CreateController(RequestContext requestContext, string controllerName)
		{
			// Thanks DefaultControllerFactory!
			var controllerType = GetControllerType(requestContext, controllerName);

			// Attempt to create an instance with injected parameters
			var controller = DI.Instantiate<IController>(controllerType, requestContext.HttpContext.Items["Context"]);

			// Return the injected instance or a regular parameterless controller
			if (controller != null) return controller;
			return GetControllerInstance(requestContext, controllerType);
		}
	}
}