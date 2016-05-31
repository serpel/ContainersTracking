using NLog.Common;
using NLog.LayoutRenderers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ContainersWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            NLog.Config.ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("mdlc", typeof(MdlcLayoutRenderer));

            string nlogPath = Server.MapPath("nlog-web.log");
            InternalLogger.LogFile = nlogPath;
            InternalLogger.LogLevel = NLog.LogLevel.Trace;
        }
    }
}
