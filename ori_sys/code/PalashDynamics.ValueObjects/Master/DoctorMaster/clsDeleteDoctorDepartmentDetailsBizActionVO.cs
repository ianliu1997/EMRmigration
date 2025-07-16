using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.DoctorMaster
{
   public class clsDeleteDoctorDepartmentDetailsBizActionVO:IBizActionValueObject
    {
       private clsDoctorVO _Details;
       public clsDoctorVO Details
       {
           get { return _Details; }
           set { _Details = value; }
       }


       public long DoctorID { get; set;}

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsDeleteDoctorDepartmentDetailsBizAction";
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
