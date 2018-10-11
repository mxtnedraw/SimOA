using Memcached.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_Common
{
    /// <summary>
    /// Memcache帮助类
    /// </summary>
   public class MemcacheHelper
    {
       private static readonly MemcachedClient mc = null;

       static MemcacheHelper()
       {
           //读取配置文件获取Memcache服务器列表（IP:Port）
           string[] serverlist = System.Configuration.ConfigurationManager.AppSettings["MemcacheServers"].Split(',');

           //初始化池
           SockIOPool pool = SockIOPool.GetInstance();
           pool.SetServers(serverlist);

           pool.InitConnections = 3;
           pool.MinConnections = 3;
           pool.MaxConnections = 5;

           pool.SocketConnectTimeout = 1000;
           pool.SocketTimeout = 3000;

           pool.MaintenanceSleep = 30;
           pool.Failover = true;

           pool.Nagle = false;
           pool.Initialize();

           // 获得客户端实例
            mc = new MemcachedClient();
           //是否启用压缩
           mc.EnableCompression = false;
       }
       /// <summary>
       /// 存储数据
       /// </summary>
       /// <param name="key">键</param>
       /// <param name="value">值</param>
       /// <returns>是否成功</returns>
       public static bool Set(string key,object value)
       {
          return mc.Set(key, value);
       }
       /// <summary>
       /// 存储数据
       /// </summary>
       /// <param name="key">键</param>
       /// <param name="value">值</param>
       /// <param name="time">过期时间</param>
       /// <returns>是否成功</returns>
       public static bool Set(string key, object value,DateTime time)
       {
           return mc.Set(key, value,time);
       }
       /// <summary>
       /// 获取数据
       /// </summary>
       /// <param name="key">键</param>
       /// <returns>对应的值（object）</returns>
       public static object Get(string key)
       {
           return mc.Get(key);
       }
       /// <summary>
       /// 删除
       /// </summary>
       /// <param name="key">键</param>
       /// <returns>是否成功</returns>
       public static bool Delete(string key)
       {
           if (mc.KeyExists(key))
           {
               return mc.Delete(key);

           }
           return false;

       }
    }
}
