using SimOA_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_IDAL
{
    public partial interface IRoleInfoDal:IBaseDal<RoleInfo>
    {
        void SetAction(int rId, int[] aIds);
    }
}
