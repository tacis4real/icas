using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ICAS.Startup))]
namespace ICAS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
