using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_Common
{
    public class PwdHelper
    {
        public static string Decode()
        {
            string connStr = ConfigurationManager.ConnectionStrings["SimOAContainer"].ConnectionString;
            string str = ConfigurationManager.AppSettings["dbPwd"];
            char[] chrs = str.ToCharArray();
            StringBuilder sb = new StringBuilder();
            int len = chrs.Length;
            for (int i = 0; i < len; i++)
            {
                //根据ASCII码表对密码进行64进制（0-9A-Za-z-_）简单解码(右移八位)
                int temp = chrs[i] + 8 > 47 && chrs[i] + 8 < 58 ? chrs[i] + 8 : chrs[i] + 15 > 64 && chrs[i] + 15 < 91 ? chrs[i] + 15 : chrs[i] + 21 > 96 && chrs[i] + 21 < 123 ? chrs[i] + 21 : chrs[i];
                sb.Append((char)temp);
            }
            connStr = connStr.Replace("user id=sa;", "user id=sa;password=" + sb.ToString() + ";");
            return connStr;
        }
    }
}
