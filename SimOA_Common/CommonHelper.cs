using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Configuration;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace SimOA_Common
{
    public class CommonHelper
    {
        /// <summary>
        /// 将指定字符串编码为MD5字符串
        /// </summary>
        /// <param name="msg">要编码的字符串</param>
        /// <returns>MD5字符串</returns>
        public static string GetMD5String(string msg)
        {
            StringBuilder sb = new StringBuilder();
            byte[] buffer, bytes;
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                buffer = System.Text.Encoding.UTF8.GetBytes(msg);
                bytes = md5.ComputeHash(buffer);
                md5.Clear();
            }
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 读取config中的密码附加字符串（“密码盐”）
        /// </summary>
        /// <returns>密码附加字符串</returns>
        public static string GetPasswordSalt()
        {
            return ConfigurationManager.AppSettings["PwdAttach"];
        }

        /// <summary>
        /// 创建验证码字符串
        /// </summary>
        /// <returns>验证码字符串</returns>
        public static string CreateValidateCodeString(int length)
        {
            string crrCode = string.Empty;
            Random rdm = new Random();
            for (int i = 0; i < length; i++)
            {
                //大小写字母以及数字总共62个，利用随机数转换为对应的ASCII码实现验证码
                int tempNum = rdm.Next(62);
                crrCode += tempNum < 10 ? (char)(tempNum + 48) : tempNum < 36 ? (char)(tempNum + 55) : (char)(tempNum + 61);
            }
            return crrCode;
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary> 
        /// <param name="ifyCode">验证码字符串</param>
        /// <returns>验证码图片字节数组</returns>
        public static byte[] CreateValidateCodeImage(string ifyCode) 
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(ifyCode.Length * 20.0), 28);
            Graphics gph = Graphics.FromImage(image);
            try
            {
                Random rdm = new Random();
                gph.Clear(Color.FromArgb(238, 238, 238));
                //画图片的干扰线
                for (int i = 0; i < 20; i++)
                {
                    gph.DrawLine(new Pen(Color.FromArgb(rdm.Next())), rdm.Next(image.Width), rdm.Next(image.Height), rdm.Next(image.Width), rdm.Next(image.Height));
                }
                //画验证码字符串
                Font font = new Font("Arial", 16, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.Red, 1.2f, true);
                gph.DrawString(ifyCode, font, brush, 7, 1);
                //画图片的前景干扰点
                for (int i = 0; i < 99; i++)
                {
                    image.SetPixel(rdm.Next(image.Width), rdm.Next(image.Height), Color.FromArgb(rdm.Next()));
                }
                //画图片的边框线
                gph.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片到流
                MemoryStream stream = new MemoryStream();
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                //返回验证码图片字节数组
                return stream.ToArray();
            }
            finally
            {
                
                gph.Dispose();
                image.Dispose();
            }
        }
    }
}
