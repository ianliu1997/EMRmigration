using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.StaffMaster
{
    public class clsAddStaffBankInfoBizActionVO : IBizActionValueObject
    {
        private List<clsStaffBankInfoVO> objPatient = null;
        public List<clsStaffBankInfoVO> PatientServiceList
        {
            get { return objPatient; }
            set { objPatient = value; }
        }
        private clsStaffBankInfoVO _objStaffBankDetail = null;
        public clsStaffBankInfoVO objStaffBankDetail
        {
            get { return _objStaffBankDetail; }
            set { _objStaffBankDetail = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {

            return "PalashDynamics.BusinessLayer.Administration.StaffMaster.clsAddStaffBankInfoBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

    }

    public class clsUpdateStaffBankInfoVO : IBizActionValueObject
    {
        private List<clsStaffBankInfoVO> objPatient = null;
        public List<clsStaffBankInfoVO> PatientServiceList
        {
            get { return objPatient; }
            set { objPatient = value; }
        }
        private clsStaffBankInfoVO _objStaffBankDetail = null;
        public clsStaffBankInfoVO objStaffBankDetail
        {
            get { return _objStaffBankDetail; }
            set { _objStaffBankDetail = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {

            return "PalashDynamics.BusinessLayer.Administration.StaffMaster.clsUpdateStaffBankInfoBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetStaffBankInfoBizActionVO : IBizActionValueObject
    {
        public bool IsPagingEnabled { get; set; }
        public int StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public string SearchExpression { get; set; }
        public int TotalRows { get; set; }
        private long _ID;
        public long StaffID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private List<clsStaffBankInfoVO> objPatient = null;
        public List<clsStaffBankInfoVO> StaffBankDetailList
        {
            get { return objPatient; }
            set { objPatient = value; }
        }
        private clsStaffBankInfoVO _objStaffBankDetail = null;
        public clsStaffBankInfoVO objStaffBankDetail
        {
            get { return _objStaffBankDetail; }
            set { _objStaffBankDetail = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {

            return "PalashDynamics.BusinessLayer.Administration.StaffMaster.clsGetStaffBankInfoBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetStaffBankInfoByIdVO : IBizActionValueObject
    {
        public bool IsPagingEnabled { get; set; }
        public int StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public string SearchExpression { get; set; }
        public int TotalRows { get; set; }
        private List<clsStaffBankInfoVO> objPatient = null;
        public List<clsStaffBankInfoVO> StaffBankDetailList
        {
            get { return objPatient; }
            set { objPatient = value; }
        }
        private clsStaffBankInfoVO _objStaffBankDetail = null;
        public clsStaffBankInfoVO objStaffBankDetail
        {
            get { return _objStaffBankDetail; }
            set { _objStaffBankDetail = value; }
        }

        private long _ID;
        public long StaffID
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

            return "PalashDynamics.BusinessLayer.Administration.StaffMaster.clsGetStaffBankInfoByIdBizAction";
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
