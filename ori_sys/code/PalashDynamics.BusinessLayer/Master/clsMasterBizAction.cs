using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.BusinessLayer
{
    class clsAddUserRoleBizAction : BizAction
    {
      private clsAddUserRoleBizAction()
     {

     }
     
     private static clsAddUserRoleBizAction _Instance = null;
     public static BizAction GetInstance()
     {
         if (_Instance == null)
             _Instance = new clsAddUserRoleBizAction();

         return _Instance;
     }

     protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
     {
         bool CurrentMethodExecutionStatus = true;
         clsAddUserRoleBizActionVO obj = null;
         int ResultStatus = 0;
         try
         {
             obj = (clsAddUserRoleBizActionVO)valueObject;
             if (obj != null)
             {
                 clsBaseMasterDAL objBaseDAL = clsBaseMasterDAL.GetInstance();
                 obj = (clsAddUserRoleBizActionVO)objBaseDAL.AddUserRole(obj, objUserVO);
             }
         }
         catch (Exception ex)
         {
             CurrentMethodExecutionStatus = false;
         }
         finally
         {
             //
         }
         return obj;
      
     }

    
    }
    
    class clsGetUserDashBoard : BizAction
    {
        //This is to get the dash board assigned to the respective user.
        public clsGetUserDashBoard()
        {

        }

        private static clsGetUserDashBoard _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetUserDashBoard();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetUserDashBoardVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetUserDashBoardVO)valueObject;
                if (obj != null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsGetUserDashBoardVO)objBaseDAL.GetUserDashBoard(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;
        }
    }

    class clsGetDashBoardList : BizAction
    {

         private clsGetDashBoardList()
         {

         }

         private static clsGetDashBoardList _Instance = null;
         public static BizAction GetInstance()
         {
             if (_Instance == null)
                 _Instance = new clsGetDashBoardList();

             return _Instance;
         }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetDashBoardListVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetDashBoardListVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterDAL objBaseDAL = clsBaseMasterDAL.GetInstance();
                    obj = (clsGetDashBoardListVO)objBaseDAL.GetDashBoardList(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;
        }
    }
}
