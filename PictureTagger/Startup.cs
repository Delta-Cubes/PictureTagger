using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PictureTagger.Startup))]
namespace PictureTagger
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
