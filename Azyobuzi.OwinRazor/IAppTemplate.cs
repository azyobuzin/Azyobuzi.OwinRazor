using Microsoft.Owin;
using RazorEngine.Templating;

namespace Azyobuzi.OwinRazor
{
    public interface IAppTemplate : ITemplate
    {
        IOwinContext OwinContext { get; set; }
    }
}
