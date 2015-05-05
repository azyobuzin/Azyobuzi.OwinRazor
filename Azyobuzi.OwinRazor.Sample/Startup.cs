using System;
using Azyobuzi.OwinRazor.Sample.Models;
using Microsoft.Owin.Hosting;
using Owin;

namespace Azyobuzi.OwinRazor.Sample
{
    class Startup
    {
        static void Main(string[] args)
        {
            const string url = "http://localhost:5000/";
            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine(url);
                Console.ReadLine();
            }
        }

        public void Configuration(IAppBuilder app)
        {
            app.UseErrorPage()
                .MapWhen(
                    ctx => ctx.Request.Path.Value == "/",
                    x => x.Run(ctx => ctx.Response.View(new Views.Index(), new IndexModel() { Name = "九条カレン" }))
                )
                .Map("/linktest", x => x.Run(ctx => ctx.Response.View(new Views.LinkTest())));
        }
    }
}
