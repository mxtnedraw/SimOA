
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
		IActionInfoDal ActionInfoDal { get; set; }
     }   
  	
 	public partial interface IDBSession
     {
		IRoleInfoDal RoleInfoDal { get; set; }
     }   
  	
 	public partial interface IDBSession
     {
		IUserActionInfoDal UserActionInfoDal { get; set; }
     }   
  	
 	public partial interface IDBSession
     {
		IUserInfoDal UserInfoDal { get; set; }
     }   
  	
 	public partial interface IDBSession
     {
		IWFInstanceDal WFInstanceDal { get; set; }
     }   
  	
 	public partial interface IDBSession
     {
		IWFModelDal WFModelDal { get; set; }
     }   
  	
 	public partial interface IDBSession
     {
		IWFStepDal WFStepDal { get; set; }
     }   
    }

  