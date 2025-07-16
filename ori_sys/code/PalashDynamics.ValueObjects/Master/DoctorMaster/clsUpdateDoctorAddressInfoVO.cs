using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.DoctorMaster
{
    public class clsUpdateDoctorAddressInfoVO : IBizActionValueObject
    {

        private List<clsDoctorAddressInfoVO> objPatient = null;
        public List<clsDoctorAddressInfoVO> PatientServiceList
        {
            get { return objPatient; }
            set { objPatient = value; }
        }
        private clsDoctorAddressInfoVO _objDoctorAddressDetail = null;
        public clsDoctorAddressInfoVO objDoctorAddressDetail
        {
            get { return _objDoctorAddressDetail; }
            set { _objDoctorAddressDetail = value; }
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

            return "PalashDynamics.BusinessLayer.Master.clsUpdateDoctorAddressInfoBizAction";
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
