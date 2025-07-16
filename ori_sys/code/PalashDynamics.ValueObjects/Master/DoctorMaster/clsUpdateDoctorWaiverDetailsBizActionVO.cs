using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.DoctorMaster
{
    public class clsUpdateDoctorWaiverDetailsBizActionVO : IBizActionValueObject
    {
        private List<clsDoctorWaiverDetailVO> objPatient = null;
        public List<clsDoctorWaiverDetailVO> objDoctorWaiverDetailList
        {
            get { return objPatient; }
            set { objPatient = value; }
        }
        private clsDoctorWaiverDetailVO _objDoctorBankDetail = null;
        public clsDoctorWaiverDetailVO objDoctorWaiverDetail
        {
            get { return _objDoctorBankDetail; }
            set { _objDoctorBankDetail = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {

            return "PalashDynamics.BusinessLayer.Master.clsUpdateDoctorWaiverDetailsBizAction";
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
