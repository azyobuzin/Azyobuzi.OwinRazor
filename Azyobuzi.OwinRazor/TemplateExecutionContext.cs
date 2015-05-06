using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Azyobuzi.OwinRazor
{
    public class TemplateExecutionContext
    {
        public TemplateExecutionContext(IOwinContext owinContext, ViewDataDictionary viewData = null)
        {
            this.OwinContext = owinContext;
            this.ViewData = viewData ?? new ViewDataDictionary();
            this.Sections = new Dictionary<string, Func<TextWriter, Task>>();
        }

        public IOwinContext OwinContext { get; private set; }
        public ViewDataDictionary ViewData { get; private set; }
        public IDictionary<string, Func<TextWriter, Task>> Sections { get; private set; }
        public string Body { get; set; }
    }
}
