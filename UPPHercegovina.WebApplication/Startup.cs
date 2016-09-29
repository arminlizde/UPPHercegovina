using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UPPHercegovina.WebApplication.Startup))]
namespace UPPHercegovina.WebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
