using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Azyobuzi.OwinRazor.MSBuild
{
    public class RazorOutputs : Task
    {
        [Required]
        public ITaskItem[] Sources { get; set; }

        [Required]
        public string OutputDirectory { get; set; }

        [Output]
        public ITaskItem[] Outputs { get; set; }

        internal static string GetOutputFileName(string outputDirectory, string itemSpec)
        {
            var tmp = Path.Combine(outputDirectory, itemSpec);
            return Path.ChangeExtension(tmp, Path.GetExtension(tmp).Substring(0, 3));
        }

        public override bool Execute()
        {
            var len = this.Sources.Length;
            var result = new ITaskItem[len];
            for (var i = 0; i < len; i++)
                result[i] = new TaskItem(GetOutputFileName(this.OutputDirectory, this.Sources[i].ItemSpec));
            this.Outputs = result;
            return true;
        }
    }
}
