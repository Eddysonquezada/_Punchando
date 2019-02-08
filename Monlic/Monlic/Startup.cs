using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Monlic.Startup))]
namespace Monlic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
