using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.DoctorMaster
{
    public class clsGetDoctorAddressInfoByIdVO : IBizActionValueObject
    {
        public bool IsPagingEnabled { get; set; }
        public int StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public string SearchExpression { get; set; }
        public int TotalRows { get; set; }
        private List<clsDoctorAddressInfoVO> objPatient = null;
        public List<clsDoctorAddressInfoVO> DoctorAddressDetailList
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

        private long _ID;
        public long DoctorID
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

            return "PalashDynamics.BusinessLayer.Master.clsGetDoctorAddressInfoByIdBizAction";
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
