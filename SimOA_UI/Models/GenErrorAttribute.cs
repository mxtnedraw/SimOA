using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimOA_UI.Models
{
    public class GenErrorAttribute:HandleErrorAttribute
    {
        public static Queue<string> exceptionQueue = new Queue<string>();
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            Exception ex = filterContext.Exception;
            exceptionQueue.Enqueue(ex.ToString());
            filterContext.HttpContext.Response.Redirect("/Error.html");
        }
    }
}