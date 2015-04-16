using Microsoft.Owin;
using RazorEngine.Templating;

namespace Azyobuzi.OwinRazor
{
    public abstract class AppTemplateBase<T> : HtmlTemplateBase<T>, IAppTemplate
    {
        public IOwinContext OwinContext { get; set; }

        public override string ResolveUrl(string path)
        {
            var pathBase = this.OwinContext.Request.PathBase;
            return path.StartsWith("~/")
                ? pathBase.ToUriComponent() + path.Substring(1)
                : path;
        }

        public TemplateWriter Include<TModel>(string name, TModel model)
        {
            return this.Include(name, model, typeof(TModel));
        }
    }
}
