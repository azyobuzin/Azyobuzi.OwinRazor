using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Azyobuzi.OwinRazor
{
    using SectionAction = Func<TextWriter, Task>;
    using PositionTagged = Tuple<string, int>;

    // Prefix(PositionTagged<string>), Value(PositionTagged<object>), Literal
    using AttributeValue = Tuple<Tuple<string, int>, Tuple<object, int>, bool>;

    public abstract class TemplateBase
    {
        protected TextWriter output;

        public ExecutionContext Context { get; set; }

        public TemplateBase Layout { get; set; }

        public virtual void Write(object value)
        {
            this.WriteTo(this.output, value);
        }

        public virtual void Write(RawString value)
        {
            this.WriteTo(this.output, value);
        }

        public virtual void WriteTo(TextWriter writer, object value)
        {
            if (value == null) return;
            var htmlString = value as RawString;
            if (htmlString != null)
                this.WriteLiteralTo(writer, htmlString.Value);
            else
                WebUtility.HtmlEncode(value.ToString(), writer);
        }

        public virtual void WriteTo(TextWriter writer, RawString value)
        {
            this.WriteLiteralTo(writer, value.Value);
        }

        public virtual void WriteLiteral(string value)
        {
            this.WriteLiteralTo(this.output, value);
        }

        public virtual void WriteLiteralTo(TextWriter writer, string value)
        {
            writer.Write(value);
        }

        public virtual void WriteAttribute(string name, PositionTagged prefix, PositionTagged suffix, params AttributeValue[] values)
        {
            this.WriteAttributeTo(this.output, name, prefix, suffix, values);
        }

        public virtual void WriteAttributeTo(TextWriter writer, string name, PositionTagged prefix, PositionTagged suffix, params AttributeValue[] values)
        {
            writer.Write(prefix.Item1);

            var first = true;
            foreach (var attr in values)
            {
                if (first)
                    first = false;
                else
                    writer.Write(attr.Item1.Item1); // Write prefix

                var value = attr.Item2.Item1;
                if (value is bool)
                {
                    if ((bool)value)
                        writer.Write(name);
                }
                else
                {
                    if (attr.Item3)
                        writer.Write((string)attr.Item2.Item1);
                    else
                        this.WriteTo(writer, attr.Item2.Item1);
                }
            }

            writer.Write(suffix.Item1);
        }

        public virtual void DefineSection(string name, SectionAction action)
        {
            this.Context.Sections[name] = action;
        }

        public virtual string Href(string contentPath)
        {
            return contentPath.StartsWith("~/")
                ? this.Context.OwinContext.Request.PathBase
                    .Add(new PathString(contentPath.Substring(1))).Value
                : contentPath;
        }

        public abstract Task ExecuteAsync();

        public virtual async Task<string> RunAsync()
        {
            this.output = new StringWriter();
            await this.ExecuteAsync().ConfigureAwait(false);
            var result = output.ToString();
            if (this.Layout == null)
                return result;
            this.Context.Body = result;
            this.Layout.Context = this.Context;
            return await this.Layout.RunAsync().ConfigureAwait(false);
        }

        public virtual RawString Raw(string value)
        {
            return new RawString(value);
        }

        public virtual RawString RenderBody()
        {
            return new RawString(this.Context.Body);
        }

        public virtual async Task<object> RenderSectionAsync(string name, bool required = true)
        {
            SectionAction action;
            if (this.Context.Sections.TryGetValue(name, out action))
            {
                await action(output).ConfigureAwait(false);
            }
            else
            {
                if (required)
                    throw new ArgumentException("Section not defined: " + name);
            }
            return null;
        }

        public virtual async Task<RawString> PartialAsync(TemplateBase template)
        {
            template.Context = this.Context;
            return new RawString(await template.RunAsync().ConfigureAwait(false));
        }

        public virtual async Task<RawString> PartialAsync<T>(TemplateBase<T> template, T model)
        {
            template.Context = this.Context;
            template.Model = model;
            return new RawString(await template.RunAsync().ConfigureAwait(false));
        }

        public virtual ViewDataDictionary ViewData
        {
            get
            {
                return this.Context.ViewData;
            }
        }

        public virtual dynamic ViewBag
        {
            get
            {
                return this.Context.ViewData;
            }
        }
    }

    public abstract class TemplateBase<T> : TemplateBase
    {
        public T Model { get; set; }
    }
}
