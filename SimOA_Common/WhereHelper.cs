using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_Common
{
    /// <summary>
    /// 构造查询表达式（whereLambda）帮助类
    /// </summary>
    /// <typeparam name="T">要查询的实体对象类</typeparam>
    [Serializable]
    public class WhereHelper<T>
        where T : class
    {
        /// <summary>
        /// lambda表达式中的要查询的实体对象的标识
        /// </summary>
        private ParameterExpression param;
        /// <summary>
        /// 包含二元运算符的lambda表达式
        /// </summary>
        private BinaryExpression filter;
        
        public WhereHelper()
        {
            param = Expression.Parameter(typeof(T), "c");

            //1==1
            Expression left = Expression.Constant(1);
            filter = Expression.Equal(left, left);
        }
        /// <summary>
        /// 获取lambda表达式
        /// </summary>
        /// <returns>生成的lambda表达式</returns>
        public Expression<Func<T, bool>> GetExpression()
        {
            return Expression.Lambda<Func<T, bool>>(filter, param);
        }
        /// <summary>
        /// 此方法用于构造相等lambda表达式（e.g.:c=>c.name == "abc"）
        /// </summary>
        /// <param name="propertyName">对象属性名</param>
        /// <param name="value">要比较的值</param>
        public void Equal(string propertyName, object value)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.Equal(left, right);
            filter = Expression.And(filter, result);
        }
        /// <summary>
        /// 此方法用于构造大于等于（字符串）lambda表达式
        /// </summary>
        /// <param name="propertyName">对象属性名</param>
        /// <param name="value">要比较的字符串</param>
        public void StrGreater(string propertyName, string value)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.Call(left, typeof(string).GetMethod("CompareTo", new Type[] { typeof(string) }), right);
            result = Expression.GreaterThanOrEqual(result, Expression.Constant(0));
            filter = Expression.And(filter, result);
        }
        /// <summary>
        /// 此方法用于构造小于等于（字符串）lambda表达式
        /// </summary>
        /// <param name="propertyName">对象属性名</param>
        /// <param name="value">要比较的字符串</param>
        public void StrLess(string propertyName, string value)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.Call(left, typeof(string).GetMethod("CompareTo", new Type[] { typeof(string) }), right);
            result = Expression.LessThanOrEqual(result, Expression.Constant(0));
            filter = Expression.And(filter, result);
        }
        /// <summary>
        /// 此方法用于构造包含lambda表达式
        /// </summary>
        /// <param name="propertyName">对象属性名</param>
        /// <param name="value">要比较的值</param>
        public void Contains(string propertyName, string value)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.Call(left, typeof(string).GetMethod("Contains"), right);
            filter = Expression.And(filter, result);
        }
        /// <summary>
        /// 此方法用于构造不包含lambda表达式
        /// </summary>
        /// <param name="propertyName">对象属性名</param>
        /// <param name="value">要比较的值</param>
        public void NotContains(string propertyName, string value)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.Call(left, typeof(string).GetMethod("Contains"), right);
            result = Expression.Equal(result,Expression.Constant(false));
            filter = Expression.And(filter, result);
        }
        /// <summary>
        /// 此方法用于构造大于等于（日期）lambda表达式
        /// </summary>
        /// <param name="propertyName">对象属性名</param>
        /// <param name="value">要比较的DateTime值</param>
        public void Greater(string propertyName, DateTime value)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.Call(left, typeof(DateTime).GetMethod("CompareTo", new Type[] { typeof(DateTime)}), right);
            result = Expression.GreaterThanOrEqual(result, Expression.Constant(0));
            filter = Expression.And(filter, result);
        }
        /// <summary>
        /// 此方法用于构造小于（日期）lambda表达式
        /// </summary>
        /// <param name="propertyName">对象属性名</param>
        /// <param name="value">要比较的DateTime值</param>
        public void Less(string propertyName, DateTime value)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.Call(left, typeof(DateTime).GetMethod("CompareTo", new Type[] { typeof(DateTime) }), right);
            result = Expression.LessThan(result, Expression.Constant(0));
            filter = Expression.And(filter, result);
        }
        /// <summary>
        /// 此方法用于构造时间相等lambda表达式
        /// </summary>
        /// <param name="propertyName">对象属性名</param>
        /// <param name="value">要比较的TimeSpan值</param>
        public void TimeEq(string propertyName, TimeSpan value)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.Call(left, typeof(TimeSpan).GetMethod("CompareTo", new Type[] { typeof(TimeSpan) }), right);
            result = Expression.Equal(result, Expression.Constant(0));
            filter = Expression.And(filter, result);
        }
       /// <summary>
       /// 此方法用于构造小于指定时间量度（如：分钟数Minutes）lambda表达式
       /// </summary>
        /// <param name="propertyName">对象属性名</param>
        /// <param name="value">要比较的值</param>
        /// <param name="item">时间量度字符串（如：Minutes）</param>
        /// <param name="diff">指定的时间量度值</param>
        public void Diff(string propertyName, DateTime value,string item, double diff)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.Call(null, typeof(System.Data.Objects.EntityFunctions).GetMethod("Diff" + item, new Type[] { typeof(DateTime), typeof(DateTime) }), left, right);
            result = Expression.LessThan(result, Expression.Constant(diff));
            filter = Expression.And(filter, result);
        }
    }
}
