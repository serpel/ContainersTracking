using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ContainersWeb.Startup))]
namespace ContainersWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
