using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.StaffMaster
{
       public class clsAddStaffAddressInfoBizActionVO : IBizActionValueObject
    {

        private List<clsStaffAddressInfoVO> objPatient = null;
        public List<clsStaffAddressInfoVO> PatientServiceList
        {
            get { return objPatient; }
            set { objPatient = value; }
        }
        private clsStaffAddressInfoVO _objStaffBankDetail = null;
        public clsStaffAddressInfoVO objStaffBankDetail
        {
            get { return _objStaffBankDetail; }
            set { _objStaffBankDetail = value; }
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

            return "PalashDynamics.BusinessLayer.Administration.StaffMaster.clsAddStaffAddressInfoBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

       public class clsGetStaffAddressInfoBizActionVO : IBizActionValueObject
       {
           public bool IsPagingEnabled { get; set; }
           public int StartRowIndex { get; set; }
           public int MaximumRows { get; set; }
           public string SearchExpression { get; set; }
           public bool IsfromMarketing = false;
           public int TotalRows { get; set; }
           private long _ID;
           public long StaffId
           {
               get { return _ID; }
               set { _ID = value; }
           }
           private List<clsStaffAddressInfoVO> objPatient = null;
           public List<clsStaffAddressInfoVO> StaffAddressDetailList
           {
               get { return objPatient; }
               set { objPatient = value; }
           }
           private clsStaffAddressInfoVO _objStaffAddressDetail = null;
           public clsStaffAddressInfoVO objStaffAddressDetail
           {
               get { return _objStaffAddressDetail; }
               set { _objStaffAddressDetail = value; }
           }

           #region IBizActionValueObject Members

           public string GetBizAction()
           {

               return "PalashDynamics.BusinessLayer.Administration.StaffMaster.clsGetStaffAddressInfoBizAction";
           }

           #endregion

           #region IValueObject Members

           public string ToXml()
           {
               return this.ToString();
           }

           #endregion
       }

       public class clsGetStaffAddressInfoByIdVO : IBizActionValueObject
       {
           public bool IsPagingEnabled { get; set; }
           public int StartRowIndex { get; set; }
           public bool IsfromMarketing = false;
           public int MaximumRows { get; set; }
           public string SearchExpression { get; set; }
           public int TotalRows { get; set; }
           private List<clsStaffAddressInfoVO> objPatient = null;
           public List<clsStaffAddressInfoVO> StaffAddressDetailList
           {
               get { return objPatient; }
               set { objPatient = value; }
           }
           private clsStaffAddressInfoVO _objStaffAddressDetail = null;
           public clsStaffAddressInfoVO objStaffAddressDetail
           {
               get { return _objStaffAddressDetail; }
               set { _objStaffAddressDetail = value; }
           }

           private long _ID;
           public long StaffId
           {
               get { return _ID; }
               set { _ID = value; }
           }
           private long _AddressTypeId;
           public long AddressTypeId
           {
               get { return _AddressTypeId; }
               set { _AddressTypeId = value; }
           }

           #region IBizActionValueObject Members

           public string GetBizAction()
           {

               return "PalashDynamics.BusinessLayer.Administration.StaffMaster.clsGetStaffAddressInfoByIdBizAction";
           }

           #endregion

           #region IValueObject Members

           public string ToXml()
           {
               return this.ToString();
           }

           #endregion
       }

       public class clsUpdateStaffAddressInfoVO : IBizActionValueObject
       {

           private List<clsStaffAddressInfoVO> objPatient = null;
           public List<clsStaffAddressInfoVO> PatientServiceList
           {
               get { return objPatient; }
               set { objPatient = value; }
           }
           private clsStaffAddressInfoVO _objStaffAddressDetail = null;
           public clsStaffAddressInfoVO objStaffAddressDetail
           {
               get { return _objStaffAddressDetail; }
               set { _objStaffAddressDetail = value; }
           }
           private long _SuccessStatus;
           public long SuccessStatus
           {
               get { return _SuccessStatus; }
               set { _SuccessStatus = value; }
           }

           #region IBizActionValueObject Members

           public string GetBizAction()
           {

               return "PalashDynamics.BusinessLayer.Administration.StaffMaster.clsUpdateStaffAddressInfoBizAction";
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
