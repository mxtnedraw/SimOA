
 using SimOA_IDAL;
using SimOA_Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_DAL
{
	
 	public partial class ActionInfoDal :BaseDal<ActionInfo>,IActionInfoDal
     {
		
     }   
  	
 	public partial class RoleInfoDal :BaseDal<RoleInfo>,IRoleInfoDal
     {
		
     }   
  	
 	public partial class UserActionInfoDal :BaseDal<UserActionInfo>,IUserActionInfoDal
     {
		
     }   
  	
 	public partial class UserInfoDal :BaseDal<UserInfo>,IUserInfoDal
     {
		
     }   
  	
 	public partial class WFInstanceDal :BaseDal<WFInstance>,IWFInstanceDal
     {
		
     }   
  	
 	public partial class WFModelDal :BaseDal<WFModel>,IWFModelDal
     {
		
     }   
  	
 	public partial class WFStepDal :BaseDal<WFStep>,IWFStepDal
     {
		
     }   
    }

  