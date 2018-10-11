
 using SimOA_DAL;
using SimOA_IDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimOA_DalFactory
{
public partial class DBSession : IDBSession
    {
	
		private IActionInfoDal _ActionInfoDal;
        public IActionInfoDal ActionInfoDal
        {
            get
            {
                if(_ActionInfoDal == null)
                {
                    _ActionInfoDal = DalFactory.GetActionInfoDal();
                }
                return _ActionInfoDal;
            }
            set { _ActionInfoDal = value; }
        }
	
		private IRoleInfoDal _RoleInfoDal;
        public IRoleInfoDal RoleInfoDal
        {
            get
            {
                if(_RoleInfoDal == null)
                {
                    _RoleInfoDal = DalFactory.GetRoleInfoDal();
                }
                return _RoleInfoDal;
            }
            set { _RoleInfoDal = value; }
        }
	
		private IUserActionInfoDal _UserActionInfoDal;
        public IUserActionInfoDal UserActionInfoDal
        {
            get
            {
                if(_UserActionInfoDal == null)
                {
                    _UserActionInfoDal = DalFactory.GetUserActionInfoDal();
                }
                return _UserActionInfoDal;
            }
            set { _UserActionInfoDal = value; }
        }
	
		private IUserInfoDal _UserInfoDal;
        public IUserInfoDal UserInfoDal
        {
            get
            {
                if(_UserInfoDal == null)
                {
                    _UserInfoDal = DalFactory.GetUserInfoDal();
                }
                return _UserInfoDal;
            }
            set { _UserInfoDal = value; }
        }
	
		private IWFInstanceDal _WFInstanceDal;
        public IWFInstanceDal WFInstanceDal
        {
            get
            {
                if(_WFInstanceDal == null)
                {
                    _WFInstanceDal = DalFactory.GetWFInstanceDal();
                }
                return _WFInstanceDal;
            }
            set { _WFInstanceDal = value; }
        }
	
		private IWFModelDal _WFModelDal;
        public IWFModelDal WFModelDal
        {
            get
            {
                if(_WFModelDal == null)
                {
                    _WFModelDal = DalFactory.GetWFModelDal();
                }
                return _WFModelDal;
            }
            set { _WFModelDal = value; }
        }
	
		private IWFStepDal _WFStepDal;
        public IWFStepDal WFStepDal
        {
            get
            {
                if(_WFStepDal == null)
                {
                    _WFStepDal = DalFactory.GetWFStepDal();
                }
                return _WFStepDal;
            }
            set { _WFStepDal = value; }
        }
	}	
}

  