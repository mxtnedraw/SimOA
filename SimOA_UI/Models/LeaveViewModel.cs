using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimOA_UI.Models
{
    public class LeaveViewModel
    {
        public string LeaveTitle { get; set; }
        public string Reason { get; set; }
        public int Days { get; set; }
        public int NextId { get; set; }
        public string Remark { get; set; }
    }
}