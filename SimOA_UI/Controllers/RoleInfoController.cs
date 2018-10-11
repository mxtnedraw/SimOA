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
    public class RoleInfoController : BaseController
    {
        //spring.net依赖注入属性
        IRoleInfoBll RoleInfoBll { get; set; }

        #region 角色管理
        //角色列表
        public ActionResult Index()
        {
            //ViewData.Model = roleInfoBll.GetList(u => true);
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

            WhereHelper<RoleInfo> wh = new WhereHelper<RoleInfo>();
            wh.Equal("IsDeleted", (byte)0);
            if (isId)
            {
                wh.Equal("RoleId", searchId);
            }
            if (searchName != string.Empty)
            {
                wh.Contains("RoleName", searchName);
            }
            if (fromIsDate)
            {
                wh.StrGreater("AddTime", from.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (toIsDate)
            {
                wh.StrLess("AddTime", to.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            var roleList = RoleInfoBll.GetPageList<long>(wh.GetExpression(), u => u.RoleId, false, pageIndex, pageSize, out totalCount);
            //var roleList = RoleInfoBll.GetPageList<long>(r => (r.IsDeleted == 0) && (isId ? r.RoleId == searchId : true) && (searchName != string.Empty ? r.RoleName.Contains(searchName) : true) && (formIsDate ? from.CompareTo(r.AddTime) < 0 : true) && (toIsDate ? to.CompareTo(r.AddTime) > 0 : true), r => r.RoleId, false, pageIndex, pageSize, out totalCount);
            var uList = UserInfoBll.GetList<int>(us => true);
            var result = from r in roleList
                         from uu in uList
                         where r.SubBy == uu.UserId
                         select new RoleViewModel
                         {
                             RoleId = r.RoleId,
                             RoleName = r.RoleName,
                             Remark = r.Remark,
                             AddTime = r.AddTime,
                             ModifiedTime = r.ModifiedTime,
                             SubBy = uu.RealName != null && uu.RealName != "" ? uu.RealName : uu.Username
                         };
            return Json(new { total = totalCount, rows = result });
        }
        //获取下拉列表数据
        public ActionResult GetSelect()
        {
            var selects = RoleInfoBll.GetList(r => (r.IsDeleted == 0), r => r.RoleName);
            var result = from r in selects
                         select new { id = r.RoleId, text = r.RoleName };
            return Json(result);
        }
        //测试异常用
        public ActionResult Test()
        {
            int a = 20;
            int b = 0;
            int c = a / b;
            ViewData["1"] = c;
            return View();
        }
        #endregion

        #region 添加
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(RoleInfo roleInfo)
        {
            string result = "no";
            //roleInfo.ModifiedTime = DateTime.Now;
            roleInfo.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            roleInfo.IsDeleted = 0;
            roleInfo.SubBy = UserLogin.UserId;
            if (RoleInfoBll.Add(roleInfo))
            {
                result = "ok";
            }
            return Content(result);
        }
        #endregion

        #region 编辑
        public ActionResult Edit(int rId)
        {
            RoleInfo roleInfo = RoleInfoBll.GetById(rId);
            return View(roleInfo);
        }
        [HttpPost]
        public ActionResult Edit(RoleInfo roleInfo)
        {
            string result = "no";
            RoleInfo role = RoleInfoBll.GetById(roleInfo.RoleId);
            role.ModifiedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            role.RoleName = roleInfo.RoleName;
            role.RoleLevel = roleInfo.RoleLevel;
            role.Remark = roleInfo.Remark;
            role.SubBy = UserLogin.UserId;
            if (role.RoleName != string.Empty && RoleInfoBll.Edit(role))
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
            if (idList != null && RoleInfoBll.Delete(idList))
            {
                result = "ok";
            }
            return Content(result);
        }
        #endregion

        #region 检查角色名是否存在
        public ActionResult CheckExist(string rName)
        {
            string result = "no";
            //string rName = Request[]
            var temp = RoleInfoBll.GetList<int>(r => (r.IsDeleted == 0) && (r.RoleName == rName)).FirstOrDefault();
            if (temp == null)
            {
                result = "ok";
            }
            return Content(result);
        }
        #endregion

        #region 展示为角色设置权限表单
        public ActionResult SetRoleAction(int rId)
        {
            ViewBag.RoleInfo = RoleInfoBll.GetById(rId);
            ViewData.Model = ActionInfoBll.GetList(a => a.IsDeleted == 0, a => a.ActionTitle).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult SetRoleAction(int rId, string aIds)
        {
            string result = "no";
            if (RoleInfoBll.SetAction(rId, aIds))
            {
                result = "ok";
            }
            return Content(result);
        }
        #endregion
    }
}
