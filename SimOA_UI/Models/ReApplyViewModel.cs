using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimOA_UI.Models
{
    public class ReApplyViewModel
    {
        public long InstanceId { get; set; }
        public string InstanceTitle { get; set; }
        public string Details { get; set; }
        public string Reason { get; set; }
        public int Num { get; set; }
        public string Tips { get; set; }
        public int NextId { get; set; }
        public string RejectBy { get; set; }
        public string RejectTime { get; set; }
        public string Remark { get; set; }
    }
}