
 using SimOA_IBLL;
using SimOA_IDAL;
using SimOA_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_BLL
{
	
 	public partial class ActionInfoBll :BaseBLL<ActionInfo>,IActionInfoBll
     {
     
 
 		 public override void SetCurrentDal()
         {
             CurrentDal = this.CurrentDBSession.ActionInfoDal;
         }
     }   
  	
 	public partial class RoleInfoBll :BaseBLL<RoleInfo>,IRoleInfoBll
     {
     
 
 		 public override void SetCurrentDal()
         {
             CurrentDal = this.CurrentDBSession.RoleInfoDal;
         }
     }   
  	
 	public partial class UserActionInfoBll :BaseBLL<UserActionInfo>,IUserActionInfoBll
     {
     
 
 		 public override void SetCurrentDal()
         {
             CurrentDal = this.CurrentDBSession.UserActionInfoDal;
         }
     }   
  	
 	public partial class UserInfoBll :BaseBLL<UserInfo>,IUserInfoBll
     {
     
 
 		 public override void SetCurrentDal()
         {
             CurrentDal = this.CurrentDBSession.UserInfoDal;
         }
     }   
  	
 	public partial class WFInstanceBll :BaseBLL<WFInstance>,IWFInstanceBll
     {
     
 
 		 public override void SetCurrentDal()
         {
             CurrentDal = this.CurrentDBSession.WFInstanceDal;
         }
     }   
  	
 	public partial class WFModelBll :BaseBLL<WFModel>,IWFModelBll
     {
     
 
 		 public override void SetCurrentDal()
         {
             CurrentDal = this.CurrentDBSession.WFModelDal;
         }
     }   
  	
 	public partial class WFStepBll :BaseBLL<WFStep>,IWFStepBll
     {
     
 
 		 public override void SetCurrentDal()
         {
             CurrentDal = this.CurrentDBSession.WFStepDal;
         }
     }   
    }

  