using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.DoctorMaster
{
    public class clsGetDoctorBankInfoByIdVO : IBizActionValueObject
    {
        public bool IsPagingEnabled { get; set; }
        public int StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public string SearchExpression { get; set; }
        public int TotalRows { get; set; }
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

        private long _ID;
        public long DoctorID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private long _BankId;
        public long BankId
        {
            get { return _BankId; }
            set { _BankId = value; }
        }
        private long _DID;
        public long ID
        {
            get { return _DID; }
            set { _DID = value; }
        }
        
        #region IBizActionValueObject Members

        public string GetBizAction()
        {

            return "PalashDynamics.BusinessLayer.Master.clsGetDoctorBankInfoByIdBizAction";
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
