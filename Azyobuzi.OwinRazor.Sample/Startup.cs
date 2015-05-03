using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;

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
            app.UseErrorPage().UseWelcomePage();
        }
    }
}
