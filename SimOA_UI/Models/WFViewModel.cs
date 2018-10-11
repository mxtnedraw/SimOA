using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimOA_UI.Models
{
    public class WFViewModel
    {
        public long ModelId { get; set; }
        public string ModelTitle { get; set; }
        public string AddTime { get; set; }
        public string Remark { get; set; }
        public byte IsDeleted { get; set; }
        public string ModifiedTime { get; set; }
        public string SubBy { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}