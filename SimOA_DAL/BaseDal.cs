using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_DAL
{
    public partial class BaseDal<T> where T:class
    {
        //通过工厂创建EF上下文对象
        DbContext context = ContextFactory.GetContext();

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="t">要操作的Entity实体对象</param>
        public void Add(T t)
        {
            context.Set<T>().Add(t);
        }
        
        /// <summary>
        /// 根据id删除数据库中的记录
        /// </summary>
        /// <param name="id">主键</param>
        public void Remove(int id)
        {
            T t = context.Set<T>().Find(id);
            context.Set<T>().Remove(t);
        }
        /// <summary>
        /// 批量删除数据库中的记录
        /// </summary>
        /// <param name="ids">主键数组</param>
        public void Remove(int[] ids)
        {
            int counter = ids.Length;
            for (int i = 0; i < counter; i++)
            {
                T t = context.Set<T>().Find(ids[i]);
                context.Set<T>().Remove(t);
            }
        }
        /// <summary>
        /// 根据实体对象删除数据库中的记录
        /// </summary>
        /// <param name="t">要操作的Entity实体对象</param>
        public void Remove(T t)
        {
            context.Set<T>().Remove(t);
        }

        /// <summary>
        /// 根据实体对象软删除
        /// </summary>
        /// <param name="t">要操作的Entity实体对象</param>
        public void Delete(T t)
        {
            context.Set<T>().Attach(t);
            context.Entry(t).Property("IsDeleted").IsModified = true;
            context.Entry(t).Property("IsDeleted").CurrentValue = true;
        }
        /// <summary>
        /// 根据id软删除
        /// </summary>
        /// <param name="id">主键</param>
        public void Delete(int id)
        {
            T t = GetById(id);
            context.Entry(t).Property("IsDeleted").IsModified = true;
            context.Entry(t).Property("IsDeleted").CurrentValue = (byte)1;
        }
        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <param name="idList">id集合</param>
        public void Delete(List<int> idList) 
        {
            int counter = idList.Count();
            for (int i = 0; i < counter; i++)
            {
                Delete(idList[i]);
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="t">要操作的Entity实体对象</param>
        public void Edit(T t)
        {
            context.Entry(t).State = EntityState.Modified;
        }

        /// <summary>
        /// 根据id查找
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns>Entity实体对象</returns>
        public T GetById(long id)
        {
            return context.Set<T>().Find(id);
        }
        /// <summary>
        /// 根据给定的Expression表达式进行查找并排序
        /// </summary>
        /// <typeparam name="TKey">排序数据类型</typeparam>
        /// <param name="whereLambda">查找Expression表达式</param>
        /// <param name="orderLambda">排序Expression表达式</param>
        /// <returns>IQueryable<T>：可查询的实体对象序列</returns>
        public IQueryable<T> GetList<TKey>(Expression<Func<T, bool>> whereLambda,params Expression<Func<T, TKey>>[] orderLambda)
        {
            IQueryable<T> temp = context.Set<T>().Where(whereLambda);
            return orderLambda.Length == 0 ? temp : temp.OrderBy(orderLambda[0]);
        }
        /// <summary>
        /// 根据给定的Expression表达式进行分页查找并排序
        /// </summary>
        /// <typeparam name="TKey">排序数据类型</typeparam>
        /// <param name="whereLambda">查找Expression表达式</param>
        /// <param name="orderLambda">排序Expression表达式</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="pageIndex">分页页码（显示第几页）</param>
        /// <param name="pageSize">分页大小（一页显示的数据条数）</param>
        /// <param name="total">数据总条数</param>
        /// <returns>IQueryable<T>：可查询的实体对象序列</returns>
        public IQueryable<T> GetPageList<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda,bool isAsc, int pageIndex, int pageSize, out int total)
        {
            IQueryable<T> temp = context.Set<T>().Where(whereLambda);
            total = temp.Count();
            return isAsc ? temp.OrderBy(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize) : temp.OrderByDescending(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}
