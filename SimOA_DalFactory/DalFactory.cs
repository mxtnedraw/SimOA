using SimOA_IDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_DalFactory
{
    /// <summary>
    /// 数据层工厂类
    /// </summary>
    public partial class DalFactory
    {
        //读取配置文件获取程序集信息和命名空间
        private static readonly string strAssInfo = ConfigurationManager.AppSettings["AssPathAndNameSpace"];
        //程序集信息
        private static readonly string strAssPath = strAssInfo.Split(',')[0];
        //命名空间
        private static readonly string strNameSpace = strAssInfo.Split(',')[1];

        //已根据T4模板生成
        //public static IRoleInfoDal GetRoleInfoDal()
        //{
        //    Assembly ass = Assembly.Load(strAssPath);
        //    return ass.CreateInstance(strNameSpace + ".RoleInfoDal") as IRoleInfoDal;
        //}
    }
}
