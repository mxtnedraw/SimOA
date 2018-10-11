using SimOA_Common;
using SimOA_Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_DAL
{
    /// <summary>
    /// 获取上下文对象的工厂类
    /// </summary>
    public partial class ContextFactory
    {
        /// <summary>
        /// 获取EF上下文对象
        /// </summary>
        /// <returns>EF上下文对象</returns>
        public static DbContext GetContext()
        {
            //利用数据槽CallContext获取上下文对象，实现上下文线程内唯一
            DbContext context = CallContext.GetData("OAContext") as DbContext;

            //如果数据槽中没有则创建上下文对象并将其加入到数据槽中
            if (context == null)
            {
                context = new SimOAContainer();
                CallContext.SetData("OAContext", context);
            }
            return context;
        }
    }
}
