using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.ValueObjects.CRM;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CRM;

namespace PalashDynamics.BusinessLayer.CRM
{
   internal class clsAddPROPatientBizAction:BizAction
    {
       private clsAddPROPatientBizAction()
       { 
       }
       private static clsAddPROPatientBizAction _Instance = null;
       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsAddPROPatientBizAction();

           return _Instance;
       }
        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
         bool CurrentMethodExecutionStatus = true;
             clsAddPROPatientBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddPROPatientBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    
                    clsBaseCampMasterDAL objBaseCampMasterDAL = clsBaseCampMasterDAL.GetInstance();
                    obj = (clsAddPROPatientBizActionVO)objBaseCampMasterDAL.AddPROPatient(obj, objUserVO);
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

