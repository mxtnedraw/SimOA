using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimOA_IBLL;
using SimOA_Common;
using SimOA_UI.Models;
using SimOA_Model;

namespace SimOA_UI.Controllers
{
    public class BaseController : Controller
    {
        //spring.net依赖注入属性
        protected IUserInfoBll UserInfoBll { get; set; }
        protected IActionInfoBll ActionInfoBll { get; set; }
        protected IUserActionInfoBll UserActionInfoBll { get; set; }

        //当前登录用户
        public UserLoginViewModel UserLogin { get; set; }
        //身份验证
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            bool isLogin =false;
            //判断Cookie中是否有对应数据
            if (Request.Cookies["LData"] != null)
            {
                //有
                string key = Request.Cookies["LData"].Value;
                object obj = MemcacheHelper.Get(key);
                if (obj != null)
                {
                    //反序列化
                    UserLogin = SerializeHelper.DeserializeToObject<UserLoginViewModel>(obj.ToString()) as UserLoginViewModel;
                }
                if (UserLogin != null)
                {
                    //登录信息正确
                    isLogin = true;
                }
            }

            if (!isLogin)
            {
                //未登录跳转到登录界面
                filterContext.Result = Redirect(Url.Action("Index", "Login"));
            }
        }
        //行为前过滤
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            #region 验证是否有访问权限
            ////留个后门，给管理方便，发布时删除
            //if (UserLogin.UserName.Equals("admin123"))
            //{
            //    return;
            //}

            //获取用户以及要访问的url
            UserInfo userInfo = UserInfoBll.GetById(UserLogin.UserId);
            string controllerName = RouteData.GetRequiredString("controller");
            string actionName = RouteData.GetRequiredString("action");
            if (actionName == "CheckExist" || actionName == "GetSelect" || actionName == "CheckPwdRight")
            {
                //默认所有人都有验证字段名是否存在、获取下拉列表数据和判断密码是否正确的权限
                return;
            }
            ActionInfo actionInfo = ActionInfoBll.GetList<int>(a =>
                (a.ControllerName.ToLower().Equals(controllerName.ToLower()))
                &&
                (a.ActionName.ToLower().Equals(actionName.ToLower()))
                &&
                a.IsDeleted == 0)
                .FirstOrDefault();
            if (actionInfo == null)
            {
                //访问url有误
                filterContext.Result = new RedirectResult("/Error.html");
                return;
            }

            //查询否决，看有无数据
            UserActionInfo userActionInfo = UserActionInfoBll.GetList<int>(ua =>
                (ua.UserId == userInfo.UserId)
                &&
                (ua.ActionId == actionInfo.ActionId)).FirstOrDefault();
            if (userActionInfo != null)
            {
                //否决表中有数据
                if (userActionInfo.IsAllow == 1)
                {
                    //允许
                    return;
                }
                else
                {
                    //拒绝，跳转到无权限页面
                    filterContext.Result = new RedirectResult("/NoAccess.html");
                }
            }
            else
            {
                //否决表中无数据，则通过用户找角色，通过角色找权限
                var result = from r in userInfo.RoleInfo
                             from a in r.ActionInfo
                             where a.ActionId == actionInfo.ActionId
                             select a;
                if (result.Count() > 0)
                {
                    //有权限
                    return;
                }
                else
                {
                    //无权限，跳转到无权限页面
                    filterContext.Result = new RedirectResult("/NoAccess.html");
                }
            }
            #endregion
        }

        //根据UserId获取用户名
        protected string GetUName(int uId)
        {
            return UserInfoBll.GetById(uId).Username;
        }
    }
}
