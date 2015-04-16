using System.IO;
using RazorEngine.Templating;

namespace Azyobuzi.OwinRazor
{
    public sealed class AppTemplateManager : DelegateTemplateManager
    {
        private AppTemplateManager() : base(ResolveView) { }

        static AppTemplateManager()
        {
            TemplateDirectory = "Views";
        }

        public static string TemplateDirectory { get; set; }

        public static string ResolveView(string key)
        {
            return File.ReadAllText(Path.Combine(TemplateDirectory, key + ".cshtml"));
        }

        private static AppTemplateManager _default;
        public static AppTemplateManager Default
        {
            get
            {
                if (_default == null)
                    _default = new AppTemplateManager();
                return _default;
            }
        }
    }
}