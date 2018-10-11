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
    public class WorkFlowController : BaseController
    {
        //spring.net依赖注入属性
        IRoleInfoBll RoleInfoBll { get; set; }
        IWFInstanceBll WFInstanceBll { get; set; }
        IWFStepBll WFStepBll { get; set; }
        IWFModelBll WFModelBll { get; set; }

        #region 流程管理
        //流程列表
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
            bool showAll = !string.IsNullOrEmpty(Request["show"]);

            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            int totalCount;
            WhereHelper<WFInstance> wh = new WhereHelper<WFInstance>();
            //wh.Equal("IsDeleted", (byte)0);
            if (!showAll)
            {
                wh.Equal("SubBy", UserLogin.UserId);
            }
            if (isId)
            {
                wh.Equal("InstanceId", searchId);
            }
            if (searchName != string.Empty)
            {
                wh.Contains("InstanceTitle", searchName);
            }
            if (fromIsDate)
            {
                wh.StrGreater("SubTime", from.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (toIsDate)
            {
                wh.StrLess("SubTime", to.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            var InstanceList = WFInstanceBll.GetPageList<long>(wh.GetExpression(), i => i.InstanceId, false, pageIndex, pageSize, out totalCount);
            var uList = UserInfoBll.GetList<int>(us => true);
            var result = from i in InstanceList
                         from uu in uList
                         where i.SubBy == uu.UserId
                         select new
                         {
                             UserId = UserLogin.UserId,
                             SubId = i.SubBy,
                             InstanceId = i.InstanceId,
                             InstanceTitle = i.InstanceTitle,
                             InstanceState = i.InstanceState,
                             Details = i.Details,
                             Remark = i.Remark,
                             SubTime = i.SubTime,
                             SubBy = uu.RealName != null && uu.RealName != "" ? uu.RealName : uu.Username,
                             RejectMsg = i.RejectMsg
                         };
            return Json(new { total = totalCount, rows = result }, JsonRequestBehavior.AllowGet);
        }
        //获取审批人下拉列表数据
        private List<SelectListItem> GetNextIdList()
        {
            UserInfo userInfo = UserInfoBll.GetById(UserLogin.UserId);
            List<SelectListItem> ltResult = new List<SelectListItem> { new SelectListItem() };
            if (userInfo != null && userInfo.RoleInfo != null)
            {
                List<RoleInfo> ltRole = userInfo.RoleInfo.ToList();
                byte temp = 0;
                foreach (var item in ltRole)
                {
                    temp = item.RoleLevel > temp ? item.RoleLevel : temp;
                }
                IQueryable<RoleInfo> ltR = RoleInfoBll.GetList(r => (r.RoleLevel == temp + 1), r => r.RoleName);
                ltResult = (from r in ltR
                            from u in r.UserInfo
                            select new SelectListItem
                            {
                                Text = u.Username + "(" + r.RoleName + ")",
                                Value = u.UserId + ""
                            }).ToList();
                if (ltResult.Count > 0)
                {
                    ltResult.Add(new SelectListItem { Text = "---默认---", Value = ltResult[0].Value, Selected = true });
                }
            }
            return ltResult;
        } 
        #endregion

        #region 发起报销
        public ActionResult Expense()
        {
            var result = GetNextIdList();
            ViewData["SelectList"] = result;
            return View();
        }
        [HttpPost]
        public ActionResult Expense(ExpenseViewModel model)
        {
            string result = "no";
            //Guid instanceGuid = WorkFlowHelper.Run(new Expense(), new Dictionary<string, object>()
            //{
            //    {"R2",model.Reason},
            //    {"M2",model.Money}
            //});
            WFInstance instance = new WFInstance()
            {
                InstanceTitle = "报销--" + model.ExpenseTitle,
                Details = string.Format("事由：{0}，金额：{1}元。", model.Reason, model.Money),
                InstanceGuid = "",
                InstanceState = (short)InstanceState.Approving,
                Remark = model.Remark,
                SubBy = UserLogin.UserId,
                SubTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            instance.WFStep.Add(new WFStep()
            {
                NextId = model.NextId,
                SubBy = UserLogin.UserId,
                SubTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
            if (WFInstanceBll.Add(instance))
            {
                result = "ok";
            }
            return Content(result);
        } 
        #endregion

        #region 发起请假
        public ActionResult Leave()
        {
            var result = GetNextIdList();
            ViewData["SelectList"] = result;
            return View();
        }
        [HttpPost]
        public ActionResult Leave(LeaveViewModel model)
        {
            string result = "no";
            //Guid instanceGuid = WorkFlowHelper.Run(new Leave(), new Dictionary<string, object>()
            //{
            //    {"R2",model.Reason},
            //    {"D2",model.Days}
            //});
            string title = model.LeaveTitle == "1" ? "事假" : model.LeaveTitle == "2" ? "病假" : "休假";
            WFInstance instance = new WFInstance()
            {
                InstanceTitle = "请假--" + title,
                Details = string.Format("事由：{0}，天数：{1}天。", model.Reason, model.Days),
                InstanceGuid = "",
                InstanceState = (short)InstanceState.Approving,
                Remark = string.IsNullOrWhiteSpace(model.Remark) ? string.Format("{0}至{1}", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(model.Days).ToString("yyyy-MM-dd")) : model.Remark,
                SubBy = UserLogin.UserId,
                SubTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            instance.WFStep.Add(new WFStep()
            {
                NextId = model.NextId,
                SubBy = UserLogin.UserId,
                SubTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
            if (WFInstanceBll.Add(instance))
            {
                result = "ok";
            }
            return Content(result);
        } 
        #endregion

        #region 审批管理
        //审批列表
        public ActionResult ApproveList()
        {
            var result = GetNextIdList();
            ViewData["SelectList"] = result;
            return View();
        }
        //获取分页数据
        public ActionResult GetApprove()
        {
            long searchId;
            bool isId = long.TryParse(Request["searchId"], out searchId);
            string searchName = string.IsNullOrEmpty(Request["searchName"]) ? string.Empty : Request["searchName"];
            DateTime from, to;
            bool fromIsDate = DateTime.TryParse(Request["from"], out from);
            bool toIsDate = DateTime.TryParse(Request["to"], out to);
            string fromStr = from.ToString("yyyy-MM-dd HH:mm:ss");
            string toStr = to.ToString("yyyy-MM-dd") + " 23:59:59";

            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            int totalCount;
            var stepList = WFStepBll.GetPageList<long>(s => (s.NextId == UserLogin.UserId) && (s.IsEnd == 0) && (isId ? s.InstanceId == searchId : true) && (searchName != string.Empty ? s.WFInstance.InstanceTitle.Contains(searchName) : true) && (fromIsDate ? fromStr.CompareTo(s.WFInstance.SubTime) <= 0 : true) && (toIsDate ? toStr.CompareTo(s.WFInstance.SubTime) >= 0 : true), s => s.InstanceId, false, pageIndex, pageSize, out totalCount);
            var uList = UserInfoBll.GetList<int>(us => true);
            var result = from i in stepList
                         from uu in uList
                         where i.SubBy == uu.UserId
                         select new
                         {
                             StepId = i.StepId,
                             InstanceId = i.InstanceId,
                             InstanceTitle = i.WFInstance.InstanceTitle,
                             Details = i.WFInstance.Details,
                             Remark = i.WFInstance.Remark,
                             SubTime = i.SubTime,
                             SubBy = uu.RealName != null && uu.RealName != "" ? uu.RealName : uu.Username
                         };
            return Json(new { total = totalCount, rows = result }, JsonRequestBehavior.AllowGet);
        }
        //获取下拉列表数据
        public ActionResult GetSelect()
        {
            var selects = WFModelBll.GetList(r => (r.IsDeleted == 0), r => r.ModelTitle);
            var result = from r in selects
                         select new { id = r.ModelId, text = r.ModelTitle };
            return Json(result);
        } 
        #endregion

        #region 审批
        public ActionResult Approve(int stepId)
        {
            WFStep step = WFStepBll.GetById(stepId);
            var uu = UserInfoBll.GetById(step.WFInstance.SubBy);
            ApproveViewModel model = new ApproveViewModel
            {
                StepId = step.StepId,
                InstanceId = step.InstanceId,
                InstanceTitle = step.WFInstance.InstanceTitle,
                Details = step.WFInstance.Details,
                SubBy = uu.RealName != null && uu.RealName != "" ? uu.RealName : uu.Username,
                Approve = true //默认选中同意
            };
            ViewData["SelectList"] = GetNextIdList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Approve(ApproveViewModel model)
        {
            string result = "no";
            WFInstance wInstance = WFInstanceBll.GetById(model.InstanceId);
            //int status = !model.Approve ? -1 : model.Approve && model.NextId == 0 ? 1 : 0;
            //if (wInstance.InstanceTitle.StartsWith("报销"))
            //{
            //    WorkFlowHelper.Resume(new Expense(), Guid.Parse(wInstance.InstanceGuid), "Check", status);
            //}
            //else if (wInstance.InstanceTitle.StartsWith("请假"))
            //{
            //    WorkFlowHelper.Resume(new Leave(), Guid.Parse(wInstance.InstanceGuid), "LeaveCheck", status);
            //}
            //else
            //{
            //    return Content("no");
            //}
            wInstance.WFStep.Add(new WFStep
            {
                Tips = string.IsNullOrWhiteSpace(model.Tips) ? "" : model.Approve ? "同意,意见为:" + model.Tips : "驳回,意见为:" + model.Tips,
                NextId = model.NextId,
                SubBy = UserLogin.UserId,
                SubTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                IsEnd = model.NextId == 0 ? (byte)1 : (byte)0
            });
            if (model.Approve && model.NextId == 0)
            {
                wInstance.InstanceState = (int)InstanceState.Over;
            }
            else if (!model.Approve)
            {
                wInstance.InstanceState = (int)InstanceState.Reject;
            }
            wInstance.RejectMsg = model.Tips;
            WFStepBll.GetById(model.StepId).IsEnd = 1;
            if (WFInstanceBll.Edit(wInstance))
            {
                result = "ok";
            }
            return Content(result);
        } 
        #endregion

        #region 驳回后重新发起
        public ActionResult ReApply(int InstanceId)
        {
            WFStep lastStep = WFStepBll.GetList<int>(s => s.WFInstance.InstanceId == InstanceId).OrderByDescending(s => s.StepId).FirstOrDefault();
            WFInstance instance = lastStep.WFInstance;
            var uu = UserInfoBll.GetById(lastStep.SubBy);
            ReApplyViewModel raView = new ReApplyViewModel
            {
                InstanceId = instance.InstanceId,
                InstanceTitle = instance.InstanceTitle,
                Details = instance.Details,
                Tips = lastStep.Tips,
                RejectBy = uu.RealName != null && uu.RealName != "" ? uu.RealName : uu.Username,
                RejectTime = lastStep.SubTime
            };
            var result = GetNextIdList();
            ViewData["SelectList"] = result;
            return View(raView);
        }
        [HttpPost]
        public ActionResult ReApply(ReApplyViewModel model)
        {
            string result = "no";
            WFInstance instance = WFInstanceBll.GetById(model.InstanceId);
            if (instance.InstanceTitle.StartsWith("报销"))
            {
                //WorkFlowHelper.Resume(new Expense(), Guid.Parse(instance.InstanceGuid), "BackInput", new ExpenseModel()
                //{
                //    Reason = model.Reason,
                //    Money = model.Num
                //});
                instance.InstanceTitle = model.InstanceTitle.StartsWith("报销--") ? model.InstanceTitle : "报销--" + model.InstanceTitle;
                instance.Details = string.Format("事由：{0}，金额：{1}元。", model.Reason, model.Num);
            }
            else if (instance.InstanceTitle.StartsWith("请假"))
            {
                //WorkFlowHelper.Resume(new Leave(), Guid.Parse(instance.InstanceGuid), "ReInput", new LeaveModel()
                //{
                //    Reason = model.Reason,
                //    Days = model.Num
                //});
                string title = model.InstanceTitle == "1" ? "事假" : model.InstanceTitle == "2" ? "病假" : "休假";
                instance.InstanceTitle = "请假--" + title;
                instance.Details = string.Format("事由：{0}，天数：{1}天。", model.Reason, model.Num);
            }
            else
            {
                return Content("no");
            }
            instance.InstanceState = (int)InstanceState.Approving;
            instance.RejectMsg = "";
            instance.Remark = model.Remark;
            instance.WFStep.Add(new WFStep()
            {
                IsEnd = 0,
                NextId = model.NextId,
                SubBy = UserLogin.UserId,
                SubTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
            if (WFInstanceBll.Edit(instance))
            {
                result = "ok";
            }
            return Content(result);
        } 
        #endregion
    }
}
