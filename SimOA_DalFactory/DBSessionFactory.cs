using SimOA_IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_DalFactory
{
    /// <summary>
    /// DBSession工厂类
    /// </summary>
    public partial class DBSessionFactory
    {
        public static IDBSession CreateDBSession()
        {
            //利用数据槽CallContext获取DBSession对象，实现DBSession线程内唯一
            IDBSession dbSession = CallContext.GetData("dbSession") as IDBSession;
            //如果数据槽中没有则创建DBSession对象并将其加入到数据槽中
            if (dbSession == null)
            {
                dbSession = new DBSession();
                CallContext.SetData("dbSession", dbSession);
            }
            return dbSession;
        }
    }
}
