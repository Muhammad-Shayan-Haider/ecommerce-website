using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OLXproject.Startup))]
namespace OLXproject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
