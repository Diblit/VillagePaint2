using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VillagePaint.Startup))]
namespace VillagePaint
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
