using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Electronica.Startup))]
namespace Electronica
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
