using SimOA_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_IBLL
{
    public partial interface IUserInfoBll
    {
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="userInfo">登录用户userInfo对象</param>
        /// <param name="uId">out参数：登录用户Id</param>
        /// <returns>返回是否成功的标识，受影响行数大于0则返回true，否则为false</returns>
        bool Login(UserInfo userInfo, out long uId);
        /// <summary>
        /// 为用户分配角色
        /// </summary>
        /// <param name="uId">用户Id</param>
        /// <param name="rIds">要分配的角色Id数组</param>
        /// <returns>返回是否成功的标识，受影响行数大于0则返回true，否则为false</returns>
        bool SetRole(int uId, string rIds);
        /// <summary>
        /// 设置否决权限
        /// </summary>
        /// <param name="uId">用户Id</param>
        /// <param name="aId">要设置否决的权限</param>
        /// <param name="isAllow">是否允许</param>
        /// <returns>返回是否成功的标识，受影响行数大于0则返回true，否则为false</returns>
        bool SetAction(long uId, long aId, int isAllow);
        /// <summary>
        /// 设置登录错误次数和时间
        /// </summary>
        /// <param name="uId">用户Id</param>
        /// <param name="errorCount">错误次数</param>
        /// <param name="errorTime">错误时间</param>
        /// <returns>返回是否成功的标识，受影响行数大于0则返回true，否则为false</returns>
        bool SetErrorInfo(long uId, int errorCount, string errorTime);
    }
}
