using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhotoStudioApp.Startup))]
namespace PhotoStudioApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
