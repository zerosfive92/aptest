using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(APTEST.Startup))]
namespace APTEST
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
