using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimOA_UI.Models
{
    public enum InstanceState
    {
        /// <summary>
        /// 审批中
        /// </summary>
        Approving = 0,
        /// <summary>
        /// 被驳回
        /// </summary>
        Reject = 1,
        /// <summary>
        /// 已完成
        /// </summary>
        Over = 2 
    }
}