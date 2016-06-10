using System;
using System.IO;
using System.Web.Mvc;
using Python.Runtime;

namespace PythonInMyMVC.Templating
{
    public class JinjaView : IView
    {
        public PyObject TemplateObject { get; set; }

        public JinjaView(PyObject templateObject)
        {
            TemplateObject = templateObject;
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            var template = ProcessTemplate(TemplateObject, viewContext.ViewData);

            writer.Write(template);
        }

        private string ProcessTemplate(PyObject templateObject, ViewDataDictionary model)
        {
            string result = templateObject.InvokeMethod("render", model.ToPython()).AsManagedObject(typeof(string)) as string;
            return result;
        }
    }
}