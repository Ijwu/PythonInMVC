using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Python.Runtime;
using PythonInMyMVC.Templating;

namespace PythonInMyMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IntPtr PyThread;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            
            PythonEngine.Initialize();
            PyThread = PythonEngine.BeginAllowThreads();
            ViewEngines.Engines.Add(new JinjaViewEngine());
        }

        public override void Dispose()
        {
            base.Dispose();
            PythonEngine.EndAllowThreads(PyThread);
        }
    }
}
