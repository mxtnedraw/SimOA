using SimOA_DalFactory;
using SimOA_IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_BLL
{
    public abstract class BaseBLL<T> where T : class
    {
        /// <summary>
        /// 使用DBSession工厂得到DBSession对象（属性）
        /// </summary>
        public IDBSession CurrentDBSession
        {
            get { return DBSessionFactory.CreateDBSession(); }
        }
        /// <summary>
        /// 当前操作的实体类T对应的数据层Dal对象（属性）
        /// </summary>
        public IBaseDal<T> CurrentDal { get; set; }
        /// <summary>
        /// 抽象方法，在子类实现，并将对应的数据层Dal对象赋给CurrentDal属性
        /// </summary>
        public abstract void SetCurrentDal();
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public BaseBLL()
        {
            SetCurrentDal();
        }
        /// <summary>
        /// 增加（调用数据层对应方法）
        /// </summary>
        /// <param name="t">要操作的Entity实体对象</param>
        /// <returns>返回是否成功的标识，受影响行数大于0则返回true，否则为false</returns>
        public bool Add(T t)
        {
            CurrentDal.Add(t);
            return CurrentDBSession.SaveChanges();
        }
        /// <summary>
        /// 根据Id删除（调用数据层对应方法）
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns>返回是否成功的标识，受影响行数大于0则返回true，否则为false</returns>
        public bool Remove(int id)
        {
            CurrentDal.Remove(id);
            return CurrentDBSession.SaveChanges();
        }
        /// <summary>
        /// 批量删除（调用数据层对应方法）
        /// </summary>
        /// <param name="ids">Id数组</param>
        /// <returns>返回是否成功的标识，受影响行数大于0则返回true，否则为false</returns>
        public bool Remove(int[] ids)
        {
            CurrentDal.Remove(ids);
            return CurrentDBSession.SaveChanges();
        }
        /// <summary>
        /// 根据实体对象删除（调用数据层对应方法）
        /// </summary>
        /// <param name="t">要操作的Entity实体对象</param>
        /// <returns>返回是否成功的标识，受影响行数大于0则返回true，否则为false</returns>
        public bool Remove(T t)
        {
            CurrentDal.Remove(t);
            return CurrentDBSession.SaveChanges();
        }
        /// <summary>
        /// 根据实体对象软删除（调用数据层对应方法）
        /// </summary>
        /// <param name="t">要操作的Entity实体对象</param>
        /// <returns>返回是否成功的标识，受影响行数大于0则返回true，否则为false</returns>
        public bool Delete(T t)
        {
            CurrentDal.Delete(t);
            return CurrentDBSession.SaveChanges();
        }
        /// <summary>
        /// 根据Id软删除（调用数据层对应方法）
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns>返回是否成功的标识，受影响行数大于0则返回true，否则为false</returns>
        public bool Delete(int id)
        {
            CurrentDal.Delete(id);
            return CurrentDBSession.SaveChanges();
        }
        /// <summary>
        /// 批量软删除（调用数据层对应方法）
        /// </summary>
        /// <param name="idList">Id集合</param>
        /// <returns>返回是否成功的标识，受影响行数大于0则返回true，否则为false</returns>
        public bool Delete(List<int> idList)
        {
            CurrentDal.Delete(idList);
            return CurrentDBSession.SaveChanges();
        }
        /// <summary>
        /// 编辑（调用数据层对应方法）
        /// </summary>
        /// <param name="t">要操作的Entity实体对象</param>
        /// <returns>返回是否成功的标识，受影响行数大于0则返回true，否则为false</returns>
        public bool Edit(T t)
        {
            CurrentDal.Edit(t);
            return CurrentDBSession.SaveChanges();
        }

        #region 软删除old
        //public bool Delete(List<int> idList)
        //{
        //    var prop = typeof(T).GetProperty("IsDeleted");
        //    T t;
        //    foreach (var id in idList)
        //    {
        //        t = GetById(id);
        //        if (t != null)
        //        {
        //            prop.SetValue(t, true);
        //            CurrentDal.Edit(t);
        //        }
        //    }
        //    return CurrentDBSession.SaveChanges();
        //} 
        #endregion
        /// <summary>
        /// 根据id查找（调用数据层对应方法）
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns>Entity实体对象</returns>
        public T GetById(long id)
        {
            return CurrentDal.GetById(id);
        }
        /// <summary>
        /// 根据给定的Expression表达式进行查找并排序（调用数据层对应方法）
        /// </summary>
        /// <typeparam name="TKey">排序数据类型</typeparam>
        /// <param name="whereLambda">查找Expression表达式</param>
        /// <param name="orderLambda">排序Expression表达式</param>
        /// <returns>IQueryable<T>：可查询的实体对象序列</returns>
        public IQueryable<T> GetList<TKey>(Expression<Func<T, bool>> whereLambda, params Expression<Func<T, TKey>>[] orderLambda)
        {
            return CurrentDal.GetList(whereLambda,orderLambda);
        }
        /// <summary>
        /// 根据给定的Expression表达式进行分页查找并排序（调用数据层对应方法）
        /// </summary>
        /// <typeparam name="TKey">排序数据类型</typeparam>
        /// <param name="whereLambda">查找Expression表达式</param>
        /// <param name="orderLambda">排序Expression表达式</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="pageIndex">分页页码（显示第几页）</param>
        /// <param name="pageSize">分页大小（一页显示的数据条数）</param>
        /// <param name="total">数据总条数</param>
        /// <returns>IQueryable<T>：可查询的实体对象序列</returns>
        public IQueryable<T> GetPageList<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc, int pageIndex, int pageSize, out int total)
        {
            return CurrentDal.GetPageList<TKey>(whereLambda, orderLambda, isAsc, pageIndex, pageSize, out total);
        }
    }
}
