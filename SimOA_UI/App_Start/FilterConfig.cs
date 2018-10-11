using SimOA_UI.Models;
using System.Web;
using System.Web.Mvc;

namespace SimOA_UI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            //使用自定义的异常处理Attribute来进行异常处理
            filters.Add(new GenErrorAttribute());
        }
    }
}