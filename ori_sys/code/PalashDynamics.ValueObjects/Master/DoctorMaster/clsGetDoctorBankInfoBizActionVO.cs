using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.DoctorMaster
{
    public class clsGetDoctorBankInfoBizActionVO : IBizActionValueObject
    {
        public bool IsPagingEnabled { get; set; }
        public int StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public string SearchExpression { get; set; }
        public int TotalRows { get; set; }
        private long _ID;
        public long DoctorID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private List<clsDoctorBankInfoVO> objPatient = null;
        public List<clsDoctorBankInfoVO> DoctorBankDetailList
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

            return "PalashDynamics.BusinessLayer.Master.clsGetDoctorBankInfoBizAction";
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
