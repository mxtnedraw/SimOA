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
    public class HomeController : BaseController
    {
        //spring.net依赖注入属性
        IWFInstanceBll WFInstanceBll { get; set; }
        IWFStepBll WFStepBll { get; set; }

        //首页
        public ActionResult Index()
        {
            #region 查找待办事项信息并放在ViewData中
            List<QuickEntryViewModel> ltEntry = new List<QuickEntryViewModel>();
            ViewBag.CurrentUser = UserLogin.UserName;
            var aSteps = WFStepBll.GetList(s => (s.NextId == UserLogin.UserId) && s.IsEnd == 0, s => s.StepId);
            var aInstances = from s in aSteps
                             select s.WFInstance;
            AddEntryViewToList(ltEntry, aInstances);
            var rInstances = WFInstanceBll.GetList(i => (i.SubBy == UserLogin.UserId) && (i.InstanceState == 1), i => i.InstanceId);
            AddEntryViewToList(ltEntry, rInstances);
            ViewData["Entry"] = ltEntry; 
            #endregion

            #region 主菜单过滤
            //准备目标集合
            List<MenuViewModel> listMenu = new List<MenuViewModel>();
            //获取所有的桌面菜单
            List<ActionInfo> aList = ActionInfoBll.GetList(a => a.IsDeleted == 0 && a.IsMenu == 1, a => a.ActionTitle).ToList();
            //获取当前登录的用户的对象
            UserInfo userInfo = UserInfoBll.GetById(UserLogin.UserId);
            //遍历所有桌面菜单，逐个判断是否有权限
            foreach (var actionInfo in aList)
            {

                //根据当前数据，构造一个菜单对象
                MenuViewModel menu = new MenuViewModel()
                {
                    ActionTitle = actionInfo.ActionTitle,
                    ControllerName = actionInfo.ControllerName,
                    ActionName = actionInfo.ActionName,
                    MenuIcon = actionInfo.MenuIcon
                };
                //查找否决中是否允许，如果允许，直接加入目标集合
                if (UserActionInfoBll.GetList<int>(ua =>
                    (ua.ActionId == actionInfo.ActionId) &&
                    (ua.UserId == UserLogin.UserId) &&
                    (ua.IsAllow == 1)).Count() > 0)
                {
                    listMenu.Add(menu);
                    continue;
                }

                //如果否决没有允许，则查找角色-权限过程
                var raList = from r in userInfo.RoleInfo
                             from a in r.ActionInfo
                             where a.ActionId == actionInfo.ActionId
                             select a;
                if (raList.Count() > 0)
                {
                    listMenu.Add(menu);
                }

                //排除拒绝的特殊权限
                var forbidList = from ua in userInfo.UserActionInfo
                                 where ua.ActionId == actionInfo.ActionId
                                 &&
                                 ua.IsAllow == 0
                                 select ua;
                if (forbidList.Count() > 0)
                {
                    listMenu.Remove(menu);
                }
            }
            #endregion

            return View(listMenu);
        }
        //将待办事项数据加入到集合中
        private void AddEntryViewToList(List<QuickEntryViewModel> ltEntry, IQueryable<WFInstance> instances)
        {
            var uuList = UserInfoBll.GetList<long>(u => true).ToList();
            if (instances != null && instances.Count() > 0)
            {
                foreach (var item in instances)
                {
                    string rejectBy = item.SubBy + "";
                    var subBy = uuList.Where(uu => uu.UserId == long.Parse(rejectBy)).FirstOrDefault();
                    if (item.InstanceState == 2)
                    {
                        //已完成
                        continue;
                    }
                    else if (item.InstanceState == 1)
                    {
                        //驳回
                        rejectBy = item.WFStep.OrderByDescending(s => s.StepId).FirstOrDefault().SubBy.ToString();
                    }
                    ltEntry.Add(new QuickEntryViewModel()
                    {
                        InstanceId = item.InstanceId,
                        InstanceTitle = item.InstanceTitle,
                        InstanceState = item.InstanceState,
                        SubBy = subBy.RealName != null &&  subBy.RealName != "" ?  subBy.RealName :  subBy.Username
                    });
                }
            }
        }

    }
}
