using System;
using System.IO;
using System.Web.Mvc;
using Python.Runtime;

namespace PythonInMyMVC.Templating
{
    public class JinjaViewEngine : VirtualPathProviderViewEngine
    {
        private readonly PyObject _templateClass;

        public JinjaViewEngine()
        {
            var jinjaModule = PythonEngine.ImportModule("jinja2");
            _templateClass = jinjaModule.GetAttr("Template");

            ViewLocationFormats = new[]{ "~/Views/{0}.j2", "~/Views/Shared/{0}.j2" };
            PartialViewLocationFormats = new[] { "~/Views/{0}.j2", "~/Views/Shared/{0}.j2" };
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            var physicalpath = controllerContext.HttpContext.Server.MapPath(partialPath);
            return new JinjaView(CreateTemplateFromFile(physicalpath));
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            var physicalpath = controllerContext.HttpContext.Server.MapPath(viewPath);
            return new JinjaView(CreateTemplateFromFile(physicalpath));
        }

        private PyObject CreateTemplateFromFile(string physicalpath)
        {
            if (string.IsNullOrWhiteSpace(physicalpath))
            {
                throw new ArgumentException("Path string is null", nameof(physicalpath));
            }

            if (!File.Exists(physicalpath))
            {
                throw new FileNotFoundException("Template file not found");
            }

            var templateString = File.ReadAllText(physicalpath);
            PyObject templateInstance = _templateClass.Invoke(templateString.ToPython());
            return templateInstance;
        }
    }
}