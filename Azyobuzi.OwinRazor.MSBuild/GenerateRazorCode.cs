using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNet.Razor;
using Microsoft.AspNet.Razor.Generator;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Azyobuzi.OwinRazor.MSBuild
{
    public class GenerateRazorCode : Task
    {
        [Required]
        public ITaskItem[] Sources { get; set; }

        [Required]
        public string OutputDirectory { get; set; }

        public string RootNamespace { get; set; }

        [Output]
        public ITaskItem[] OutputFiles { get; set; }

        private string GetNamespace(string itemSpec)
        {
            var s = itemSpec.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length == 0) throw new ArgumentException();
            if (s.Length == 1) return this.RootNamespace;

            var sb = new StringBuilder(this.RootNamespace);
            for (var i = 0; i < s.Length - 1; i++)
            {
                sb.Append('.');
                sb.Append(s[i]);
            }
            return sb.ToString();
        }

        public override bool Execute()
        {
            var success = true;
            var outputFiles = new List<string>();

            foreach (var source in this.Sources)
            {
                var file = new FileInfo(source.GetMetadata("FullPath"));
                if (!file.Exists)
                {
                    this.Log.LogError("{0} is not found.", source.ItemSpec);
                    success = false;
                    continue;
                }
                var lang = RazorCodeLanguage.GetLanguageByExtension(file.Extension);
                if (lang == null)
                {
                    this.Log.LogError("{0} extension is not supported.", file.Extension);
                    success = false;
                    continue;
                }

                try
                {
                    var host = new RazorEngineHost(lang);
                    host.DefaultBaseClass = "Azyobuzi.OwinRazor.TemplateBase";
                    host.NamespaceImports.Add("System");
                    host.GeneratedClassContext = new GeneratedClassContext(
                        GeneratedClassContext.DefaultExecuteMethodName,
                        GeneratedClassContext.DefaultWriteMethodName,
                        GeneratedClassContext.DefaultWriteLiteralMethodName,
                        "WriteTo",
                        "WriteLiteralTo",
                        null,
                        "DefineSection",
                        new GeneratedTagHelperContext()
                    );
                    var engine = new RazorTemplateEngine(host);
                    var result = engine.GenerateCode(
                        file.OpenRead(),
                        Path.GetFileNameWithoutExtension(file.Name),
                        GetNamespace(source.ItemSpec),
                        file.FullName
                    );
                    if (result.Success)
                    {
                        var outputFile = Path.Combine(this.OutputDirectory, source.ItemSpec);
                        outputFile = Path.ChangeExtension(outputFile, Path.GetExtension(outputFile).Substring(0, 3));
                        Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                        File.WriteAllText(outputFile, result.GeneratedCode);
                        outputFiles.Add(outputFile);
                    }
                    else
                    {
                        foreach (var error in result.ParserErrors)
                        {
                            this.Log.LogError(null, null, null, file.FullName,
                                error.Location.LineIndex, error.Location.CharacterIndex,
                                0, 0, "{0}", error.Message);
                        }
                        success = false;
                    }
                }
                catch (Exception ex)
                {
                    this.Log.LogErrorFromException(ex, true, true, file.FullName);
                    success = false;
                }
            }

            this.OutputFiles = outputFiles.Select(file => new TaskItem(file)).ToArray();

            return success;
        }
    }
}
