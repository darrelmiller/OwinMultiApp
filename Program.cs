// Step 1: Create a Console app to host the server  (or web app if you want to IIS host)
// Step 2: Pull in the Owin Server package - Microsoft.Owin.Host.*  (I picked HttpListener, use SystemWeb for IIS)
// Step 3: Pull in the Owin support for Web Api  - Microsoft.AspNet.WebApi.Owin
// Step 4: Pull in the Owin support for SignalR - Microsoft.AspNet.SignalR.Owin
// Step 5: Pull in Microsoft's implementation of Owin application building (aka Katana) - Microsoft.Owin.Hosting
// Step 5: Write the code to create the Owin application using WebApp.Start
// Step 6: Create your WebApi controllers
// Step 7: Create your signalR hubs


using System;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Hosting;
using Owin;

namespace OwinMultiApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new StartOptions();
            options.Urls.Add("http://localhost:999/");
            using (WebApp.Start(options, builder =>
            {
                var config = new HttpConfiguration();
                config.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
                builder.UseWebApi(config);
               
                
                builder.MapHubs("signalr", new HubConfiguration
                {
                    EnableCrossDomain = true,
                    EnableJavaScriptProxies = true,
                });


            }))
            {
                Console.ReadKey();
            }

        }
    }


    public class TestController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage() {Content = new StringContent("Hello World")};
        }
    }


    [HubName("hellohub")]
    public class MyHub : Hub
    {
        public void Send(string message)
        {
            Clients.All.addMessage(message);
        }
    }
}
