using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimOA_UI.Models
{
    [Serializable]
    public class UserLoginViewModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
    }
}