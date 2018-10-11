using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_Common
{
    /// <summary>
    /// JSON序列化帮助类
    /// </summary>
   public class SerializeHelper
    {
       /// <summary>
       /// 对数据进行序列化
       /// </summary>
       /// <param name="value">要序列化的数据</param>
       /// <returns></returns>
       public static string  SerializeToString(object value)
       {
          return JsonConvert.SerializeObject(value);
       }
       /// <summary>
       /// 反序列化操作
       /// </summary>
       /// <typeparam name="T">反序列化后的对象类型</typeparam>
       /// <param name="str">要反序列化的JSON字符串</param>
       /// <returns>反序列化后的对象</returns>
       public static T DeserializeToObject<T>(string str)
       {
          return JsonConvert.DeserializeObject<T>(str);
       }
    }
}
