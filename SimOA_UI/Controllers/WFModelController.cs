using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimOA_IBLL;
using SimOA_Model;
using SimOA_Common;
using SimOA_UI.Models;

namespace SimOA_UI.Controllers
{
    public class WFModelController : BaseController
    {
        //spring.net依赖注入属性
        IWFModelBll WFModelBll { get; set; }

        #region 模板管理
        //模板列表
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
            WhereHelper<WFModel> wh = new WhereHelper<WFModel>();
            wh.Equal("IsDeleted", (byte)0);
            if (isId)
            {
                wh.Equal("ModelId", searchId);
            }
            if (searchName != string.Empty)
            {
                wh.Contains("ModelTitle", searchName);
            }
            if (fromIsDate)
            {
                wh.StrGreater("AddTime", from.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (toIsDate)
            {
                wh.StrLess("AddTime", to.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            var modelList = WFModelBll.GetPageList<long>(wh.GetExpression(), m => m.ModelId, false, pageIndex, pageSize, out totalCount);
            var uList = UserInfoBll.GetList<int>(us => true);
            var result = from m in modelList
                         from uu in uList
                         where m.SubBy == uu.UserId
                         select new WFViewModel
                         {
                             ModelId = m.ModelId,
                             ModelTitle = m.ModelTitle,
                             ControllerName = m.ControllerName,
                             ActionName = m.ActionName,
                             Remark = m.Remark,
                             AddTime = m.AddTime,
                             ModifiedTime = m.ModifiedTime,
                             SubBy = uu.RealName != null && uu.RealName != "" ? uu.RealName : uu.Username
                         };
            return Json(new { total = totalCount, rows = result }, JsonRequestBehavior.AllowGet);
        }
        //获取下拉列表数据
        public ActionResult GetSelect()
        {
            var selects = WFModelBll.GetList(m => (m.IsDeleted == 0), r => r.ModelTitle);
            var result = from m in selects
                         select new { id = m.ModelId, text = m.ModelTitle };
            return Json(result);
        } 
        #endregion

        #region 添加
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(WFModel wFModel)
        {
            string result = "no";
            if (wFModel != null && !string.IsNullOrWhiteSpace(wFModel.ModelTitle) && !string.IsNullOrWhiteSpace(wFModel.ControllerName) && !string.IsNullOrWhiteSpace(wFModel.ActionName))
            {
                wFModel.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                wFModel.IsDeleted = 0;
                wFModel.SubBy = UserLogin.UserId;
                if (WFModelBll.Add(wFModel))
                {
                    result = "ok";
                }
            }

            return Content(result);
        }
        #endregion

        #region 编辑
        public ActionResult Edit(int mId)
        {
            WFModel wFModel = WFModelBll.GetById(mId);
            return View(wFModel);
        }
        [HttpPost]
        public ActionResult Edit(WFModel wFModel)
        {
            string result = "no";
            WFModel model = WFModelBll.GetById(wFModel.ModelId);
            model.ModifiedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            model.ModelTitle = wFModel.ModelTitle;
            model.ControllerName = wFModel.ControllerName;
            model.ActionName = wFModel.ActionName;
            model.Remark = wFModel.Remark;
            model.SubBy = UserLogin.UserId;
            if (model.ModelTitle != string.Empty && WFModelBll.Edit(model))
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
            if (idList != null && WFModelBll.Delete(idList))
            {
                result = "ok";
            }
            return Content(result);
        }
        #endregion

        #region 检查模板名称是否存在
        public ActionResult CheckExist(string mName)
        {
            string result = "no";
            var temp = WFModelBll.GetList<int>(m => (m.IsDeleted == 0) && (m.ModelTitle == mName)).FirstOrDefault();
            if (temp == null)
            {
                result = "ok";
            }
            return Content(result);
        }
        #endregion
    }
}
