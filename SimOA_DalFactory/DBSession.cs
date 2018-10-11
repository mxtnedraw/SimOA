using SimOA_DAL;
using SimOA_IDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_DalFactory
{
    public partial class DBSession:IDBSession
    {
        //已用T4生成
        //private DbContext db;
        /// <summary>
        /// 使用上下文工厂得到上下文对象（属性）
        /// </summary>
        public DbContext Db
        {
            get{ return ContextFactory.GetContext(); }
        }
        //已用T4生成
        //private IRoleInfoDal roleInfoDal;

        //public IRoleInfoDal RoleInfoDal
        //{
        //    get 
        //    {
        //        if (roleInfoDal == null)
        //        {
        //            roleInfoDal = DalFactory.GetRoleInfoDal();
        //        }
        //        return roleInfoDal;
        //    }
        //    set { roleInfoDal = value; }
        //}
        /// <summary>
        /// 将改动保存到数据库
        /// </summary>
        /// <returns>返回是否成功的标识，受影响行数大于0则返回true，否则为false</returns>
        public bool SaveChanges()
        {
            return Db.SaveChanges() > 0;
        }
    }
}
