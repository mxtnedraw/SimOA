
 using SimOA_IDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_DalFactory
{
	public partial class DalFactory
	{
	 
	        public static IActionInfoDal GetActionInfoDal()
			{
				Assembly ass = Assembly.Load(strAssPath);
				return ass.CreateInstance(strNameSpace + ".ActionInfoDal") as IActionInfoDal;
			}
	     
	        public static IRoleInfoDal GetRoleInfoDal()
			{
				Assembly ass = Assembly.Load(strAssPath);
				return ass.CreateInstance(strNameSpace + ".RoleInfoDal") as IRoleInfoDal;
			}
	     
	        public static IUserActionInfoDal GetUserActionInfoDal()
			{
				Assembly ass = Assembly.Load(strAssPath);
				return ass.CreateInstance(strNameSpace + ".UserActionInfoDal") as IUserActionInfoDal;
			}
	     
	        public static IUserInfoDal GetUserInfoDal()
			{
				Assembly ass = Assembly.Load(strAssPath);
				return ass.CreateInstance(strNameSpace + ".UserInfoDal") as IUserInfoDal;
			}
	     
	        public static IWFInstanceDal GetWFInstanceDal()
			{
				Assembly ass = Assembly.Load(strAssPath);
				return ass.CreateInstance(strNameSpace + ".WFInstanceDal") as IWFInstanceDal;
			}
	     
	        public static IWFModelDal GetWFModelDal()
			{
				Assembly ass = Assembly.Load(strAssPath);
				return ass.CreateInstance(strNameSpace + ".WFModelDal") as IWFModelDal;
			}
	     
	        public static IWFStepDal GetWFStepDal()
			{
				Assembly ass = Assembly.Load(strAssPath);
				return ass.CreateInstance(strNameSpace + ".WFStepDal") as IWFStepDal;
			}
	         }   
  }

  