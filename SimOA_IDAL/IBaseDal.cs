using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_IDAL
{
    public partial interface IBaseDal<T>
    {
        void Add(T t);
        void Remove(T t);
        void Remove(int id);
        void Remove(int[] ids);
        void Delete(T t);
        void Delete(int id);
        void Delete(List<int> idList);
        void Edit(T t);

        T GetById(long id);
        IQueryable<T> GetList<TKey>(Expression<Func<T, bool>> whereLambda, params Expression<Func<T, TKey>>[] orderLambda);
        IQueryable<T> GetPageList<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda,bool isAsc, int pageIndex, int pageSize, out int total);

    }
}
