using SimOA_IDAL;
using SimOA_Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_DAL
{
    public partial class RoleInfoDal:BaseDal<RoleInfo>,IRoleInfoDal
    {
        //封装前代码
        //DbContext context = ContextFactory.GetContext();

        //public int Add(RoleInfo roleInfo)
        //{
        //    context.Set<RoleInfo>().Add(roleInfo);
        //    return context.SaveChanges();
        //}

        //public int Remove(int id)
        //{
        //    RoleInfo roleInfo = context.Set<RoleInfo>().Find(id);
        //    context.Set<RoleInfo>().Remove(roleInfo);
        //    return context.SaveChanges();
        //}

        //public int Remove(int[] ids)
        //{
        //    int counter = ids.Length;
        //    for (int i = 0; i < counter; i++)
        //    {
        //        RoleInfo roleInfo = context.Set<RoleInfo>().Find(ids[i]);
        //        context.Set<RoleInfo>().Remove(roleInfo);
        //    }
        //    return context.SaveChanges();
        //}

        //public int Remove(RoleInfo roleInfo)
        //{
        //    context.Set<RoleInfo>().Remove(roleInfo);
        //    return context.SaveChanges();
        //}

        //public int Edit(RoleInfo roleInfo)
        //{
        //    context.Entry(roleInfo).State = EntityState.Modified;
        //    return context.SaveChanges();
        //}

        //public RoleInfo GetById(int id)
        //{
        //    return context.Set<RoleInfo>().Find(id);
        //}

        //public IQueryable<RoleInfo> GetList(Expression<Func<RoleInfo, bool>> whereLambda)
        //{
        //    return context.Set<RoleInfo>().Where(whereLambda);
        //}

        //public IQueryable<RoleInfo> GetPageList<TKey>(Expression<Func<RoleInfo, bool>> whereLambda, Expression<Func<RoleInfo, TKey>> orderLambda, int pageIndex, int pageSize, out int total)
        //{
        //    total = context.Set<RoleInfo>().Count();
        //    return context.Set<RoleInfo>().Where(whereLambda).OrderByDescending(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        //}
        /// <summary>
        /// 为角色设置权限
        /// </summary>
        /// <param name="rId">角色Id</param>
        /// <param name="aIds">要设置的权限Id数组</param>
        public void SetAction(int rId, int[] aIds)
        {
            RoleInfo roleInfo = GetById(rId);
            //设置前清空，防止重复设置
            roleInfo.ActionInfo.Clear();

            if (aIds.Length > 0)
            {
                ActionInfoDal actionInfoDal = new ActionInfoDal();
                foreach (var aId in aIds)
                {
                    roleInfo.ActionInfo.Add(actionInfoDal.GetById(aId));
                }
            }
        }
    }
}
