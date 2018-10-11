using SimOA_Common;
using SimOA_IBLL;
using SimOA_Model;
using SimOA_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimOA_UI.Controllers
{
    public class UserInfoController : BaseController
    {
        //spring.net依赖注入属性
        IRoleInfoBll RoleInfoBll { get; set; }

        #region 用户管理
        //用户列表
        public ActionResult Index()
        {
            return View();
        }
        //获取分页数据
        public ActionResult GetPageList()
        {
            long searchId;
            bool isId = long.TryParse(Request["searchId"], out searchId);
            string searchName = string.IsNullOrEmpty(Request["searchName"]) ? string.Empty : Request["searchName"];
            DateTime from, to;
            bool fromIsDate = DateTime.TryParse(Request["from"], out from);
            bool toIsDate = DateTime.TryParse(Request["to"], out to);

            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            int totalCount;

            WhereHelper<UserInfo> wh = new WhereHelper<UserInfo>();
            wh.Equal("IsDeleted", (byte)0);
            if (isId)
            {
                wh.Equal("UserId", searchId);
            }
            if (searchName != string.Empty)
            {
                wh.Contains("Username", searchName);
            }
            if (fromIsDate)
            {
                wh.StrGreater("AddTime", from.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (toIsDate)
            {
                wh.StrLess("AddTime", to.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            var userList = UserInfoBll.GetPageList<long>(wh.GetExpression(), u => u.UserId, false, pageIndex, pageSize, out totalCount).ToList();
            //var userList = UserInfoBll.GetPageList<int>(u => (u.IsDeleted == 0) && (isId ? u.UserId == searchId : true) && (searchName != string.Empty ? u.Username.Contains(searchName) : true), u => u.UserId, false, pageIndex, pageSize, out totalCount);
            var uList = UserInfoBll.GetList<int>(us => true);
            var result = from u in userList
                         from uu in uList
                         where u.SubBy == uu.UserId
                         select new UserViewModel
                         {
                             UserId = u.UserId,
                             Username = u.Username,
                             Remark = u.Remark,
                             AddTime = u.AddTime,
                             ModifiedTime = u.ModifiedTime,
                             SubBy = uu.RealName != null && uu.RealName != "" ? uu.RealName : uu.Username
                         };
            return Json(new { total = totalCount, rows = result });
        }
        #endregion

        #region 添加
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(UserInfo userInfo)
        {
            string result = "no";
            if (userInfo != null && !string.IsNullOrWhiteSpace(userInfo.Username) && !string.IsNullOrWhiteSpace(userInfo.Password))
            {
                userInfo.Password = CommonHelper.GetMD5String(userInfo.Password + CommonHelper.GetPasswordSalt());
                userInfo.ErrorCount = 0;
                userInfo.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                userInfo.IsDeleted = 0;
                userInfo.SubBy = UserLogin.UserId;
                if (UserInfoBll.Add(userInfo))
                {
                    result = "ok";
                }
            }

            return Content(result);
        }
        #endregion

        #region 编辑
        public ActionResult Edit(int uId)
        {
            UserInfo userInfo = UserInfoBll.GetById(uId);
            return View(userInfo);
        }
        [HttpPost]
        public ActionResult Edit(UserInfo userInfo)
        {
            string result = "no";
            UserInfo user = UserInfoBll.GetById(userInfo.UserId);
            user.ModifiedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            user.SubBy = UserLogin.UserId;
            user.Username = userInfo.Username;
            user.Remark = userInfo.Remark;
            if (user.Username != string.Empty && UserInfoBll.Edit(user))
            {
                result = "ok";
            }
            return Content(result);
        }
        #endregion

        #region 删除数据（软删除）
        public ActionResult Delete(string strId)
        {
            string result = "no";
            string[] strIds = strId.Split(',');
            List<int> idList = new List<int>();
            int temp;
            foreach (string item in strIds)
            {
                if (int.TryParse(item, out temp))
                {
                    idList.Add(temp);
                }
            }
            if (idList != null && UserInfoBll.Delete(idList))
            {
                result = "ok";
            }
            return Content(result);
        }
        #endregion

        #region 检查用户名是否存在
        public ActionResult CheckExist(string uName)
        {
            string result = "no";
            var temp = UserInfoBll.GetList<long>(u => (u.IsDeleted == 0) && (u.Username == uName)).FirstOrDefault();
            if (temp == null)
            {
                result = "ok";
            }
            return Content(result);
        }
        #endregion

        #region 为用户设置角色
        public ActionResult SetUserRole(int uId)
        {
            ViewBag.UserInfo = UserInfoBll.GetById(uId);
            ViewData.Model = RoleInfoBll.GetList(r => r.IsDeleted == 0, r => r.RoleName).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult SetUserRole(int uId, string rIds)
        {
            string result = "no";
            if (UserInfoBll.SetRole(uId, rIds))
            {
                result = "ok";
            }
            return Content(result);
        }
        #endregion

        #region 为用户设置否决
        public ActionResult SetUserAction(int uId)
        {
            UserInfo userInfo = UserInfoBll.GetById(uId);
            ViewBag.UserInfo = userInfo;
            ViewBag.UserActionInfo = userInfo.UserActionInfo.ToList();
            ViewData.Model = ActionInfoBll.GetList(a => a.IsDeleted == 0, a => a.ActionTitle).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult SetUserAction(long uId, long aId, int isAllow)
        {
            string result = "no";
            if (UserInfoBll.SetAction(uId, aId, isAllow))
            {
                result = "ok";
            }
            return Content(result);
        }
        #endregion

        #region 更新个人信息
        public ActionResult PersonalInfo()
        {
            var result = UserInfoBll.GetById(UserLogin.UserId);
            return View(result);
        }
        [HttpPost]
        public ActionResult PersonalInfo(UserInfo userInfo)
        {
            string result = "no";
            UserInfo user = UserInfoBll.GetById(userInfo.UserId);
            user.ModifiedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            user.Username = userInfo.Username;
            user.RealName = userInfo.RealName;
            user.Age = userInfo.Age;
            user.Gender = userInfo.Gender;
            user.PhoneNumber = userInfo.PhoneNumber;
            user.Email = userInfo.Email;
            user.Remark = userInfo.Remark;
            if (userInfo.Username != UserLogin.UserName)
            {
                UserLogin.UserName = userInfo.Username;
            }
            user.SubBy = UserLogin.UserId;
            if (user.Username != string.Empty && UserInfoBll.Edit(user))
            {
                result = "ok";
            }
            return Content(result);
        }
        #endregion

        #region 修改密码
        public ActionResult PasswordEdit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PasswordEdit(string pwd, string rpwd)
        {
            string result = "no";
            if (pwd != rpwd)
            {
                return Content("no");
            }
            UserInfo userInfo = UserInfoBll.GetById(UserLogin.UserId);
            userInfo.Password = CommonHelper.GetMD5String(pwd + CommonHelper.GetPasswordSalt());
            if (UserInfoBll.Edit(userInfo))
            {
                result = "ok";
            }
            return Content(result);
        }
        #endregion

        #region 检查密码是否正确
        public ActionResult CheckPwdRight(string uPwd)
        {
            string result = "no";
            string temp = UserInfoBll.GetById(UserLogin.UserId).Password;
            if (temp == CommonHelper.GetMD5String(uPwd + CommonHelper.GetPasswordSalt()))
            {
                result = "ok";
            }
            return Content(result);
        }
        #endregion
    }
}
