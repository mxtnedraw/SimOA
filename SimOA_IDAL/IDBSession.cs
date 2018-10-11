using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_IDAL
{
    public partial interface IDBSession
    {
        DbContext Db{get;}
        //IRoleInfoDal RoleInfoDal { get; set; }
        bool SaveChanges();
    }
}
