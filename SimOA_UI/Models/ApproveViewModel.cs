using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimOA_UI.Models
{
    public class ApproveViewModel
    {
        public long StepId { get; set; }
        public long InstanceId { get; set; }
        public string InstanceTitle { get; set; }
        public string Details { get; set; }
        public bool Approve { get; set; }
        public string Tips { get; set; }
        public int NextId { get; set; }
        public string SubBy { get; set; }
    }
}