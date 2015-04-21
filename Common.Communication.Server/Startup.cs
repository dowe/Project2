using Microsoft.Owin.Cors;
using Owin;

namespace Common.Communication.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR<MyPersistentConnection>("/echo");
        }
    }
}
