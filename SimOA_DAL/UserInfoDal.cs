using SimOA_IDAL;
using SimOA_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_DAL
{
    public partial class UserInfoDal : BaseDal<UserInfo>, IUserInfoDal
    {
        /// <summary>
        /// 为用户分配角色
        /// </summary>
        /// <param name="uId">用户Id</param>
        /// <param name="rIds">要分配的角色Id数组</param>
        public void SetRole(int uId, int[] rIds)
        {
            UserInfo userInfo = GetById(uId);
            //设置前清空，防止重复设置
            userInfo.RoleInfo.Clear();

            if (rIds.Length > 0)
            {
                RoleInfoDal roleInfoDal = new RoleInfoDal();
                foreach (var rId in rIds)
                {
                    userInfo.RoleInfo.Add(roleInfoDal.GetById(rId));
                }
            }
        }

        /// <summary>
        /// 设置否决权限
        /// </summary>
        /// <param name="uId">用户Id</param>
        /// <param name="aId">要设置否决的权限</param>
        /// <param name="isAllow">是否允许</param>
        public void SetAction(long uId, long aId, int isAllow)
        {
            UserInfo userInfo = GetById(uId);

            //通过导航属性判断是否已经有这个权限
            UserActionInfo userAction = userInfo.UserActionInfo.Where(ua => ua.ActionId == aId).FirstOrDefault();

            //进行否决判断
            if (userAction != null)
            {
                //有此权限
                if (isAllow == 1)
                {
                    //允许
                    userAction.IsAllow = (byte)1;
                }
                else if (isAllow == -1)
                {
                    //拒绝
                    userAction.IsAllow = (byte)0;
                }
                else
                {
                    //不设置
                    userInfo.UserActionInfo.Remove(userAction);
                }
            }
            else
            {
                //无此权限
                if (isAllow != 0)
                {
                    //新创建一个userAction对象加入到数据库
                    userAction = new UserActionInfo()
                    {
                        UserId = uId,
                        ActionId = aId
                    };
                    if (isAllow == 1)
                    {
                        //允许
                        userAction.IsAllow = (byte)1;
                    }
                    else if (isAllow == -1)
                    {
                        //拒绝
                        userAction.IsAllow = (byte)0;
                    }
                    //加入到数据库
                    userInfo.UserActionInfo.Add(userAction);
                }
            }
        }

        /// <summary>
        /// 设置登录错误次数和时间
        /// </summary>
        /// <param name="uId">用户Id</param>
        /// <param name="errorCount">错误次数</param>
        /// <param name="errorTime">错误时间</param>
        public void SetErrorInfo(long uId, int errorCount, string errorTime)
        {
            UserInfo user = GetById(uId);
            user.ErrorCount = errorCount;
            user.ErrorTime = errorTime;
            Edit(user);
        }
    }
}
