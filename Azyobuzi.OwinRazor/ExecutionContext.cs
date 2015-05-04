using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Azyobuzi.OwinRazor
{
    public class ExecutionContext
    {
        public ExecutionContext(TextWriter output)
        {
            this.Output = output;
            this.Sections = new Dictionary<string, Func<TextWriter, Task>>();
        }

        public TextWriter Output { get; private set; }
        public IDictionary<string, Func<TextWriter, Task>> Sections { get; private set; }
    }
}
