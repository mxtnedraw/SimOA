using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_Common
{
    /// <summary>
    /// 工作流帮助类
    /// </summary>
    public class WorkFlowHelper
    {
        /// <summary>
        /// 预处理
        /// </summary>
        /// <param name="wfApp">WorkflowApplication对象</param>
        /// <returns>预处理后的WorkflowApplication对象</returns>
        private static WorkflowApplication PreInit(WorkflowApplication wfApp) 
        {
            //空闲卸载
            wfApp.PersistableIdle = a => { return PersistableIdleAction.Unload; };
            //持久化存储
            string conn = ConfigurationManager.ConnectionStrings["WFStore"].ConnectionString;
            wfApp.InstanceStore = new SqlWorkflowInstanceStore(conn);
            return wfApp;
        }
        /// <summary>
        /// 启动工作流
        /// </summary>
        /// <param name="activity">指定活动</param>
        /// <param name="dic">参数字典</param>
        /// <returns>工作流实例Guid</returns>
        public static Guid Run(Activity activity,IDictionary<string,object> dic) 
        {
            WorkflowApplication wfApp = PreInit(new WorkflowApplication(activity, dic));
            wfApp.Run();
            return wfApp.Id;
        }
        /// <summary>
        /// 继续工作流（书签）
        /// </summary>
        /// <param name="activity">指定活动</param>
        /// <param name="guid">工作流实例Guid</param>
        /// <param name="bookmarkName">书签名</param>
        /// <param name="value">工作流变量</param>
        public static void Resume(Activity activity, Guid guid, string bookmarkName, object value)
        {
            WorkflowApplication wfApp = PreInit(new WorkflowApplication(activity));
            wfApp.Load(guid);
            wfApp.ResumeBookmark(bookmarkName, value);
        }
    }
}
