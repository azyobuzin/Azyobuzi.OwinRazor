using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Azyobuzi.OwinRazor
{
    public static class OwinResponseExtensions
    {
        public static async Task View(this IOwinResponse response, TemplateBase template, ViewDataDictionary viewData = null)
        {
            var context = response.Context;
            template.Context = new ExecutionContext(context, viewData);
            var bytes = Encoding.UTF8.GetBytes(await template.RunAsync().ConfigureAwait(false));
            response.ContentType = "text/html; charset=utf-8";
            response.ContentLength = bytes.LongLength;
            if (!string.Equals(context.Request.Method, "HEAD", StringComparison.OrdinalIgnoreCase))
                await response.WriteAsync(bytes, context.Request.CallCancelled).ConfigureAwait(false);
        }

        public static Task View<T>(this IOwinResponse response, TemplateBase<T> template, T model, ViewDataDictionary viewData = null)
        {
            template.Model = model;
            return View(response, template, viewData);
        }
    }
}
