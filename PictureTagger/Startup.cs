using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PictureTagger.Startup))]
namespace PictureTagger
{
	/// <summary>
	/// Startups are the hip new thing, right?
	/// </summary>
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			ConfigureAuth(app);
		}
	}
}
