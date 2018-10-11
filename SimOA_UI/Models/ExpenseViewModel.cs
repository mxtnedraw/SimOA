using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimOA_UI.Models
{
    public class ExpenseViewModel
    {
        public string ExpenseTitle { get; set; }
        public string Reason { get; set; }
        public int Money { get; set; }
        public int NextId { get; set; }
        public string Remark { get; set; }
    }
}