using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(APIClient.Startup))]
namespace APIClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
