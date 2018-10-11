using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimOA_UI.Models
{
    public class QuickEntryViewModel
    {
        public long InstanceId { get; set; }
        public string InstanceTitle { get; set; }
        public int InstanceState { get; set; }
        public string SubBy { get; set; }
    }
}