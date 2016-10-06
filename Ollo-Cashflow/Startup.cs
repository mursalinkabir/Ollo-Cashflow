using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ollo_Cashflow.Startup))]
namespace Ollo_Cashflow
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
