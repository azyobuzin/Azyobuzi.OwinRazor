using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace Azyobuzi.OwinRazor
{
    public abstract class TemplateBase
    {
        public ExecutionContext Context { get; set; }

        public void Write(object value)
        {
            this.WriteTo(this.Context.Output, value);
        }

        public void WriteTo(TextWriter writer, object value)
        {
            var htmlString = value as IHtmlString;
            this.WriteLiteralTo(writer, htmlString != null ? htmlString.ToHtmlString() : value.ToString());
        }

        public void WriteLiteral(string value)
        {
            this.WriteLiteralTo(this.Context.Output, value);
        }

        public void WriteLiteralTo(TextWriter writer, string value)
        {
            if (value == null) return;
            writer.Write(value);
        }

        public void DefineSection(string name, Func<TextWriter, Task> action)
        {
            this.Context.Sections[name] = action;
        }

        public abstract Task ExecuteAsync();
    }

    public abstract class TemplateBase<T>
    {
        public T Model { get; set; }
    }
}
