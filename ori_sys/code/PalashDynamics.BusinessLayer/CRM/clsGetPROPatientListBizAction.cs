using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.CRM;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CRM;

namespace PalashDynamics.BusinessLayer.CRM
{
   internal class clsGetPROPatientListBizAction:BizAction
    {
       private clsGetPROPatientListBizAction()
       {
       }
        private static clsGetPROPatientListBizAction  _Instance = null;
        public static BizAction GetInstance()
      {
         if (_Instance == null)
             _Instance = new clsGetPROPatientListBizAction();

         return _Instance;
     }
          
       
        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
       {
             bool CurrentMethodExecutionStatus = true;
             clsGetPROPatientListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetPROPatientListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    
                    clsBaseCampMasterDAL objBaseCampMasterDAL = clsBaseCampMasterDAL.GetInstance();
                    obj = (clsGetPROPatientListBizActionVO)objBaseCampMasterDAL.GetPROPatientList(obj, objUserVO);
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


