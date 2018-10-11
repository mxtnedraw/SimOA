using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimOA_UI.Models
{
    public class ActionViewModel
    {
        public long ActionId { get; set; }
        public string ActionTitle { get; set; }
        public string AddTime { get; set; }
        public string Remark { get; set; }
        public byte IsDeleted { get; set; }
        public string ModifiedTime { get; set; }
        public string SubBy { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public byte IsMenu { get; set; }
        public string MenuIcon { get; set; }
    }
}