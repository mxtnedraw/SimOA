using SimOA_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_IDAL
{
    public partial interface IUserInfoDal : IBaseDal<UserInfo>
    {
        /// <summary>
        /// 为用户分配角色
        /// </summary>
        /// <param name="uId">用户Id</param>
        /// <param name="rIds">要分配的角色Id数组</param>
        void SetRole(int uId, int[] rIds);
        /// <summary>
        /// 设置否决权限
        /// </summary>
        /// <param name="uId">用户Id</param>
        /// <param name="aId">要设置否决的权限</param>
        /// <param name="isAllow">是否允许</param>
        void SetAction(long uId, long aId, int isAllow);
        /// <summary>
        /// 设置登录错误次数和时间
        /// </summary>
        /// <param name="uId">用户Id</param>
        /// <param name="errorCount">错误次数</param>
        /// <param name="errorTime">错误时间</param>
        void SetErrorInfo(long uId,int errorCount, string errorTime);
    }
}
