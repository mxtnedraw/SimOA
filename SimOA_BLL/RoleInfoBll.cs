using SimOA_IBLL;
using SimOA_IDAL;
using SimOA_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_BLL
{
    public partial class RoleInfoBll:BaseBLL<RoleInfo>,IRoleInfoBll
    {
        //public override void SetCurrentDal()
        //{
        //    CurrentDal = this.CurrentDBSession.RoleInfoDal;
        //}
        //IRoleInfoDal roleInfoDal = SimOA_DalFactory.DalFactory.GetRoleInfoDal();

        //public bool Add(RoleInfo roleInfo)
        //{
        //    return roleInfoDal.Add(roleInfo) > 0;
        //}
        //public bool Remove(int id)
        //{
        //    return roleInfoDal.Remove(id) > 0;
        //}

        //public bool Remove(int[] ids)
        //{
        //    return roleInfoDal.Remove(ids) > 0;
        //}

        //public bool Remove(RoleInfo roleInfo)
        //{
        //    return roleInfoDal.Remove(roleInfo) > 0;
        //}

        //public bool Edit(RoleInfo roleInfo)
        //{
        //    return roleInfoDal.Edit(roleInfo) > 0;
        //}

        //public RoleInfo GetById(int id)
        //{
        //    return roleInfoDal.GetById(id);
        //}

        //public IQueryable<RoleInfo> GetList(Expression<Func<RoleInfo, bool>> whereLambda)
        //{
        //    return roleInfoDal.GetList(whereLambda);
        //}

        //public IQueryable<RoleInfo> GetPageList<TKey>(Expression<Func<RoleInfo, bool>> whereLambda, Expression<Func<RoleInfo, TKey>> orderLambda,bool isAsc, int pageIndex, int pageSize, out int total)
        //{
        //    return roleInfoDal.GetPageList(whereLambda, orderLambda, isAsc, pageIndex, pageSize, out total);
        //}
        /// <summary>
        /// 为角色设置权限
        /// </summary>
        /// <param name="rId">角色Id</param>
        /// <param name="aIds">要设置的权限Id数组</param>
        /// <returns>返回是否成功的标识，受影响行数大于0则返回true，否则为false</returns>
        public bool SetAction(int rId, string aIds)
        {
            //判断权限Id数组aIds是否有数据
            if (aIds.Trim() == string.Empty)
            {
                //无
                ((IRoleInfoDal)CurrentDal).SetAction(rId, new int[0]);
            }
            else
            {
                //有
                List<int> list1 = new List<int>();
                string[] list2 = aIds.Split(',');
                foreach (var item in list2)
                {
                    list1.Add(int.Parse(item));
                }

                ((IRoleInfoDal)CurrentDal).SetAction(rId, list1.ToArray());
            }

            return CurrentDBSession.SaveChanges();
        }
    }
}
