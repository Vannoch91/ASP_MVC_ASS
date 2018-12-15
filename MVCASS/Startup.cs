using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCASS.Startup))]
namespace MVCASS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
