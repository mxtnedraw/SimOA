using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimOA_Common;
using SimOA_IBLL;
using SimOA_Model;
using SimOA_UI.Models;

namespace SimOA_UI.Controllers
{
    public class RegisterController : Controller
    {
        //spring.net依赖注入属性
        IUserInfoBll UserInfoBll { get; set; }
        IRoleInfoBll RoleInfoBll { get; set; }
        //注册页面
        public ActionResult Index()
        {
            return View();
        }
        //验证码
        public ActionResult ValidateCode()
        {
            string vCode = CommonHelper.CreateValidateCodeString(4);
            Session["RegValidCode"] = vCode.ToLower();
            byte[] vCodebuffer = CommonHelper.CreateValidateCodeImage(vCode);
            return File(vCodebuffer, "image/jpeg");
        }
        //验证输入合法性
        public ActionResult CheckInput()
        {

            string result = "no";
            if (string.IsNullOrEmpty(Request["regId"]))
            {
                string ifyCode = Request["ifyCode"] == null ? string.Empty : Request["ifyCode"].ToLower();
                if (ifyCode.Equals(Session["RegValidCode"].ToString(), StringComparison.InvariantCultureIgnoreCase))
                { result = "ok"; }
            }
            else if (string.IsNullOrEmpty(Request["ifyCode"]))
            {
                string regId = Request["regId"];
                if (UserInfoBll.GetList<int>(u => (u.IsDeleted == 0) && (u.Username == regId)).FirstOrDefault() == null)
                { result = "ok"; }
            }
            return Content(result);
        }
        //注册处理
        public ActionResult UserReg()
        {
            var result = "no";

            if (Request["regId"]!=null && Request["regPwd"]!=null && Request["ifyCode"]!=null && Request["ifyCode"].ToLower().Equals(Session["RegValidCode"].ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                UserInfo userinfo = new UserInfo
                {
                    Username = Request["regId"],
                    Password = CommonHelper.GetMD5String(Request["regPwd"] + CommonHelper.GetPasswordSalt()),
                    ErrorCount = 0,
                    IsDeleted = 0,
                    AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    SubBy = 19,
                    RoleInfo = new List<RoleInfo>{RoleInfoBll.GetById(6)}
                };
                if (UserInfoBll.Add(userinfo))
                {
                    //Session["UserInfo"] = userinfo.Username;
                    string key = Guid.NewGuid().ToString();
                    string value = SerializeHelper.SerializeToString(new UserLoginViewModel
                    {
                        UserId = userinfo.UserId,
                        UserName = userinfo.Username,
                        UserPwd = Request["regPwd"]
                    });
                    HttpCookie cookie = new HttpCookie("LData", key);
                    cookie.Path = "/";
                    MemcacheHelper.Set(key, value, DateTime.Now.AddDays(7));
                    Response.Cookies.Add(cookie);
                    result = "ok";
                }
            }

            return Content(result);
        }
    }
}
