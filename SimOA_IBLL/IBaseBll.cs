using SimOA_IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_IBLL
{
    public partial interface IBaseBll<T> where T:class
    {
        IDBSession CurrentDBSession { get; }
        IBaseDal<T> CurrentDal { get; set; }
        bool Add(T t);
        bool Remove(int id);
        bool Remove(int[] ids);
        bool Remove(T t);
        bool Edit(T t);
        bool Delete(T t);
        bool Delete(int id);
        bool Delete(List<int> idList);
        T GetById(long id);

        IQueryable<T> GetList<TKey>(Expression<Func<T, bool>> whereLambda, params Expression<Func<T, TKey>>[] orderLambda);
        IQueryable<T> GetPageList<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc, int pageIndex, int pageSize, out int total);

    }
}
