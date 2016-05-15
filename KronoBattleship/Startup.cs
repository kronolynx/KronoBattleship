using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KronoBattleship.Startup))]
namespace KronoBattleship
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
