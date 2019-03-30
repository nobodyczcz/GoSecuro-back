using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(gosafe_back.Startup))]
namespace gosafe_back
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
