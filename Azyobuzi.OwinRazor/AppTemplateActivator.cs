using Microsoft.Owin;
using RazorEngine.Templating;

namespace Azyobuzi.OwinRazor
{
    public sealed class AppTemplateActivator : IActivator
    {
        public AppTemplateActivator(IOwinContext owinContext)
        {
            this.owinContext = owinContext;
        }

        private readonly IOwinContext owinContext;

        public ITemplate CreateInstance(InstanceContext context)
        {
            var result = context.Loader.CreateInstance(context.TemplateType);
            var appTemplate = result as IAppTemplate;
            if (appTemplate != null)
                appTemplate.OwinContext = this.owinContext;
            return result;
        }
    }
}