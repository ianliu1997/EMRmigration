using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
   public class clsAddDoctorMasterBizActionVO:IBizActionValueObject
    {
       private  clsDoctorVO _DoctorDetails;
       public clsDoctorVO DoctorDetails
       {
           get { return _DoctorDetails; }
           set { _DoctorDetails = value; }
       }

       private int _SuccessStatus;
       public int SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }

 
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddDoctorMasterBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }
}
