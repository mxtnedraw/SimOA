using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimOA_UI.Models
{
    public class UserViewModel
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int ErrorCount { get; set; }
        public string ErrorTime { get; set; }
        public byte IsDeleted { get; set; }
        public string RealName { get; set; }
        public Nullable<int> Age { get; set; }
        public Nullable<byte> Gender { get; set; }
        public string AddTime { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Remark { get; set; }
        public string ModifiedTime { get; set; }
        public string SubBy { get; set; }
    }
}