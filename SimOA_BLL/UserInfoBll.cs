using SimOA_Model;
using SimOA_IBLL;
using SimOA_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimOA_IDAL;

namespace SimOA_BLL
{
    public partial class UserInfoBll : BaseBLL<UserInfo>, IUserInfoBll 
    {
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="userInfo">登录用户userInfo对象</param>
        /// <param name="uId">out参数：登录用户Id</param>
        /// <returns>返回是否成功的标识，受影响行数大于0则返回true，否则为false</returns>
        public bool Login(UserInfo userInfo,out long uId)
        {
            string UName = userInfo.Username;
            //密码MD5处理
            string UPwd = CommonHelper.GetMD5String(userInfo.Password+CommonHelper.GetPasswordSalt());
            var result = CurrentDal.GetList<int>(u => (u.IsDeleted == 0) && (u.Username.Equals(UName)) && (u.Password.Equals(UPwd))).FirstOrDefault();
            uId = 0;
            if (result != null)
            {
                uId = result.UserId;
            }
            return result != null;
        }

        /// <summary>
        /// 为用户分配角色
        /// </summary>
        /// <param name="uId">用户Id</param>
        /// <param name="rIds">要分配的角色Id数组</param>
        /// <returns>返回是否成功的标识，受影响行数大于0则返回true，否则为false</returns>
        public bool SetRole(int uId, string rIds)
        {
            //判断角色Id数组rIds是否有数据
            if (rIds.Trim() == string.Empty)
            {
                //有
                ((IUserInfoDal)CurrentDal).SetRole(uId,new int[0]);
            }
            else
            {
                //无
                List<int> list1 = new List<int>();
                string[] list2 = rIds.Split(',');
                foreach (var item in list2)
                {
                    list1.Add(int.Parse(item));
                }

                ((IUserInfoDal)CurrentDal).SetRole(uId, list1.ToArray());
            }

            return CurrentDBSession.SaveChanges();
        }

        /// <summary>
        /// 设置否决权限
        /// </summary>
        /// <param name="uId">用户Id</param>
        /// <param name="aId">要设置否决的权限</param>
        /// <param name="isAllow">是否允许</param>
        /// <returns>返回是否成功的标识，受影响行数大于0则返回true，否则为false</returns>
        public bool SetAction(long uId, long aId, int isAllow)
        {
            ((IUserInfoDal)CurrentDal).SetAction(uId, aId, isAllow);
            return CurrentDBSession.SaveChanges();
        }

        /// <summary>
        /// 设置登录错误次数和时间
        /// </summary>
        /// <param name="uId">用户Id</param>
        /// <param name="errorCount">错误次数</param>
        /// <param name="errorTime">错误时间</param>
        /// <returns>返回是否成功的标识，受影响行数大于0则返回true，否则为false</returns>
        public bool SetErrorInfo(long uId, int errorCount, string errorTime)
        {
            ((IUserInfoDal)CurrentDal).SetErrorInfo(uId, errorCount, errorTime);
            return CurrentDBSession.SaveChanges();
        }
    }
}
