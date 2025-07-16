using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.DoctorMaster
{
    public class clsUpdateDoctorBankInfoVO : IBizActionValueObject
    {
        private List<clsDoctorBankInfoVO> objPatient = null;
        public List<clsDoctorBankInfoVO> PatientServiceList
        {
            get { return objPatient; }
            set { objPatient = value; }
        }
        private clsDoctorBankInfoVO _objDoctorBankDetail = null;
        public clsDoctorBankInfoVO objDoctorBankDetail
        {
            get { return _objDoctorBankDetail; }
            set { _objDoctorBankDetail = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {

            return "PalashDynamics.BusinessLayer.Master.clsUpdateDoctorBankInfoBizAction";
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
