using log4net;
using SimOA_UI.Models;
using Spring.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace SimOA_UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : SpringMvcApplication //System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            WriteLogToFile();
        }

        private static void WriteLogToFile()
        {
            ThreadPool.QueueUserWorkItem(c =>
            {
                while (true)
                {
                    if (GenErrorAttribute.exceptionQueue.Count() > 0)
                    {
                        string exMsg = GenErrorAttribute.exceptionQueue.Dequeue();
                        if (exMsg != null)
                        {
                            ILog logger = LogManager.GetLogger("WebLogger");
                            logger.Error(exMsg);
                        }
                        else
                        {
                            //如果队列中没有数据，休息
                            Thread.Sleep(3000);
                        }
                    }
                    else
                    {
                        //如果队列中没有数据，休息
                        Thread.Sleep(3000);
                    }
                }
            });
        }
    }
}