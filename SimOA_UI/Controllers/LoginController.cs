using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimOA_Common;
using SimOA_IBLL;
using System.Web.Caching;
using SimOA_UI.Models;
using SimOA_Model;

namespace SimOA_UI.Controllers
{
    public class LoginController : Controller
    {
        //spring.net依赖注入属性
        IUserInfoBll UserInfoBll { get; set; }
        //登录界面
        public ActionResult Index()
        {
            return View();
        }
        //验证码
        public ActionResult ValidateCode()
        {
            string vCode = CommonHelper.CreateValidateCodeString(4);
            Session["ValidateCode"] = vCode.ToLower();
            byte[] vCodebuffer = CommonHelper.CreateValidateCodeImage(vCode);
            return File(vCodebuffer, "image/jpeg");
        }
        //验证登录
        public ActionResult UserLogin()
        {
            string userName = Request["LoginCode"] != null ? Request["LoginCode"] : string.Empty;
            string password = Request["Loginpwd"] != null ? Request["Loginpwd"] : string.Empty;
            string vCode = Request["vCode"] != null ? Request["vCode"].ToLower() : string.Empty;
            string validateCode = Session["ValidateCode"] != null ? Session["ValidateCode"].ToString() : string.Empty;
            if (vCode == string.Empty || !vCode.Equals(validateCode, StringComparison.InvariantCultureIgnoreCase))
            {
                Session["ValidateCode"] = string.Empty;
                return Content("验证码错误");
            }

            UserInfo user = UserInfoBll.GetList<long>(u => u.Username == userName).FirstOrDefault();
            if (user == null)
            {
                return Content("用户名或密码错误");
            }
            DateTime errTime;
            int restMin = 0;
            if (DateTime.TryParse(user.ErrorTime,out errTime))
            {
                restMin = 30 - Convert.ToInt32((DateTime.Now - errTime).TotalMinutes);
                if (user.ErrorCount > 4 && restMin > 0)
                {
                    return Content("超过最大错误次数，请" + restMin + "分钟后再试！");
                }
            }

            long uId;
            if (UserInfoBll.Login(new UserInfo
            {
                Username = userName,
                Password = password
            }, out uId))
            {
                //Session["userInfo"] = userName;
                //将用户登录信息存储在memcache服务器中
                string key = Guid.NewGuid().ToString();
                string value = SerializeHelper.SerializeToString(new UserLoginViewModel
                {
                    UserId = uId,
                    UserName = userName,
                    UserPwd = password
                });
                HttpCookie cookie = new HttpCookie("LData", key);
                cookie.Path = "/";
                MemcacheHelper.Set(key, value, DateTime.Now.AddDays(7));
                //自动登录：持久化cookie
                if (Request["AutoLogin"] != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(7);
                }
                Response.Cookies.Add(cookie);

                UserInfoBll.SetErrorInfo(uId, 0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return Content("ok");
            }
            else
            {
                Session["ValidateCode"] = string.Empty;
                string msg = "用户名或密码错误";
                //处理错误次数和时间
                if (user != null)
                {
                    //用户名存在
                    int errCount = 0;

                    if (user.ErrorCount < 5)
                    {
                        errCount = user.ErrorCount + 1;
                    }
                    else if (restMin <= 0)
                    {
                        errCount = 1;
                    }
                    if (errCount > 0)
                    {
                        UserInfoBll.SetErrorInfo(user.UserId, errCount, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        msg = errCount == 5 ? "超过最大错误次数，请" + restMin + "分钟后再试！" : "用户名或密码错误,您还有" + (5 - errCount) + "次尝试机会";
                    }
                }
                return Content(msg);
            }
        }
        //注销
        public ActionResult Logout()
        {
            string result = "no";
            HttpCookie ck = Request.Cookies["LData"];
            ck.Expires = DateTime.Now.AddDays(-1);
            if (MemcacheHelper.Delete(ck.Value))
            {
                result = "ok";
            }
            return Content(result);
        }
    }
}
