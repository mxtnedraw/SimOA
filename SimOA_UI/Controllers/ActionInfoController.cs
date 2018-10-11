using SimOA_IBLL;
using SimOA_Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using SimOA_Common;

namespace SimOA_UI.Controllers
{
    public class ActionInfoController : BaseController
    {
        #region 权限管理
        //权限列表
        public ActionResult Index()
        {
            return View();
        }
        //获取分页数据
        public ActionResult GetPageList()
        {
            //预处理搜索条件
            long searchId;
            bool isId = long.TryParse(Request["searchId"], out searchId);
            string searchName = string.IsNullOrEmpty(Request["searchName"]) ? string.Empty : Request["searchName"];
            DateTime from, to;
            bool fromIsDate = DateTime.TryParse(Request["from"], out from);
            bool toIsDate = DateTime.TryParse(Request["to"], out to);

            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            //进行分页查询
            int totalCount;
            WhereHelper<ActionInfo> wh = new WhereHelper<ActionInfo>();
            wh.Equal("IsDeleted", (byte)0);
            if (isId)
            {
                wh.Equal("ActionId", searchId);
            }
            if (searchName != string.Empty)
            {
                wh.Contains("ActionTitle", searchName);
            }
            if (fromIsDate)
            {
                wh.StrGreater("AddTime", from.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (toIsDate)
            {
                wh.StrLess("AddTime", to.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            var actionList = ActionInfoBll.GetPageList<long>(wh.GetExpression(), a => a.ActionId, false, pageIndex, pageSize, out totalCount);
            var uList = UserInfoBll.GetList<int>(us => true);
            //构造新匿名对象防止导航属性循环引用
            var result = from a in actionList
                         from uu in uList
                         where a.SubBy == uu.UserId
                         select new
                         {
                             ActionId = a.ActionId,
                             ActionTitle = a.ActionTitle,
                             IsMenu = a.IsMenu,
                             ControllerName = a.ControllerName,
                             ActionName = a.ActionName,
                             Remark = a.Remark,
                             AddTime = a.AddTime,
                             ModifiedTime = a.ModifiedTime,
                             SubBy = uu.RealName != null && uu.RealName != "" ? uu.RealName : uu.Username
                         };
            return Json(new { total = totalCount, rows = result }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 上传文件
        public ActionResult UpLoadFile()
        {
            var requestFile = Request.Files["file"];
            var fileExt = Path.GetExtension(requestFile.FileName);
            //只允许上传指定类型的图片文件
            if (fileExt == ".jpg" || fileExt == ".jpeg" || fileExt == ".png" || fileExt == ".ico")
            {
                //存储路径为：指定上传文件夹/当前年/月/日/
                string dirName = ConfigurationManager.AppSettings["UpLoadMenuIconPath"] + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Request.MapPath(dirName)));
                }
                string fullName = dirName + Guid.NewGuid().ToString() + fileExt;

                requestFile.SaveAs(Request.MapPath(fullName));
                return Content(fullName);
            }
            else
            {
                return Content("no");
            }
        }
        #endregion

        #region 添加
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(ActionInfo actionInfo)
        {
            string result = "no";
            if (actionInfo != null && !string.IsNullOrWhiteSpace(actionInfo.ActionTitle) && !string.IsNullOrWhiteSpace(actionInfo.ControllerName) && !string.IsNullOrWhiteSpace(actionInfo.ActionName))
            {
                //判断是否桌面菜单
                if (actionInfo.IsMenu == 1)
                {
                    //是桌面菜单
                    actionInfo.MenuIcon = string.IsNullOrWhiteSpace(Request["viewMenuIcon"]) ? ConfigurationManager.AppSettings["DefaultMenuIcon"] : Request["viewMenuIcon"];
                }
                else
                {
                    //否
                    actionInfo.MenuIcon = null;
                }
                actionInfo.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                actionInfo.IsDeleted = 0;
                actionInfo.SubBy = UserLogin.UserId;
                if (ActionInfoBll.Add(actionInfo))
                {
                    result = "ok";
                }
            }

            return Content(result);
        }
        #endregion

        #region 编辑
        public ActionResult Edit(int aId)
        {
            ActionInfo actionInfo = ActionInfoBll.GetById(aId);
            return View(actionInfo);
        }
        [HttpPost]
        public ActionResult Edit(ActionInfo actionInfo)
        {
            string result = "no";
            //判断是否桌面菜单
            if (actionInfo.IsMenu == 1)
            {
                //是
                actionInfo.MenuIcon = string.IsNullOrWhiteSpace(Request["viewMenuIcon"]) ? ConfigurationManager.AppSettings["DefaultMenuIcon"] : Request["viewMenuIcon"];
            }
            else
            {
                //否
                actionInfo.MenuIcon = null;
            }
            ActionInfo action = ActionInfoBll.GetById(actionInfo.ActionId);
            action.ModifiedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            action.ActionTitle = actionInfo.ActionTitle;
            action.ControllerName = actionInfo.ControllerName;
            action.ActionName = actionInfo.ActionName;
            action.IsMenu = actionInfo.IsMenu;
            action.MenuIcon = actionInfo.MenuIcon;
            action.Remark = actionInfo.Remark;
            action.SubBy = UserLogin.UserId;
            if (action.ActionTitle != string.Empty && ActionInfoBll.Edit(action))
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
            if (idList != null && ActionInfoBll.Delete(idList))
            {
                result = "ok";
            }
            return Content(result);
        }
        #endregion

        #region 检查权限名是否存在
        public ActionResult CheckExist(string aName)
        {
            string result = "no";
            var temp = ActionInfoBll.GetList<int>(a => (a.IsDeleted == 0) && (a.ActionTitle == aName)).FirstOrDefault();
            if (temp == null)
            {
                result = "ok";
            }
            return Content(result);
        }
        #endregion
    }
}
