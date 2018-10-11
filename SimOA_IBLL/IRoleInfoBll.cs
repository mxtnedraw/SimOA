using SimOA_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_IBLL
{
    public partial interface IRoleInfoBll:IBaseBll<RoleInfo>
    {
        bool SetAction(int rId, string aIds);
    }
}
