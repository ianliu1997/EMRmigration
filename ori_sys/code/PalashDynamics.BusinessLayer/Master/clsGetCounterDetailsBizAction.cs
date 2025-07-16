using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Masters;

namespace PalashDynamics.BusinessLayer.Master
{
    internal class clsGetCounterDetailsBizAction : BizAction
    {

        private static clsGetCounterDetailsBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetCounterDetailsBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetCounterDetailsBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetCounterDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseCounterDAL objBaseMasterDAL = clsBaseCounterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetCounterDetailsBizActionVO)objBaseMasterDAL.GetCounterListByUnitId(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //log error  

            }

            return valueObject;

        }
    }
}
