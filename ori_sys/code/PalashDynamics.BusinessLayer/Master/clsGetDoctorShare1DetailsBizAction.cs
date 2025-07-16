using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;

namespace PalashDynamics.BusinessLayer.Master
{
    internal class clsGetDoctorShare1DetailsBizAction : BizAction
    {

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetDoctorShare1DetailsBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetDoctorShare1DetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                    obj = (clsGetDoctorShare1DetailsBizActionVO)objBaseDoctorDAL.GetDoctorShares1DetailsList(obj, objUserVO);
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


        private static clsGetDoctorShare1DetailsBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsGetDoctorShare1DetailsBizAction();
            }

            return _Instance;
        }


    }
}
