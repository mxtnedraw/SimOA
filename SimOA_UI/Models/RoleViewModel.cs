using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimOA_UI.Models
{
    public class RoleViewModel
    {
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public string AddTime { get; set; }
        public string Remark { get; set; }
        public byte IsDeleted { get; set; }
        public string ModifiedTime { get; set; }
        public string SubBy { get; set; }
        public byte RoleLevel { get; set; }
    }
}