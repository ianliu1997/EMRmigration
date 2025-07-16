using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.ValueObjects.Billing
{
    public class clsAddDoctorProcedureLinkBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Billing.clsAddDoctorProcedureLinkBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

        private clsDoctorProcedureLinkVO _Details;
        public clsDoctorProcedureLinkVO LinkDetails
        {
            get { return _Details; }
            set { _Details = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

    }

    public class clsGetDoctorProcedureLinkBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Billing.clsGetDoctorProcedureLinkBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

        private clsDoctorProcedureLinkVO _Details;
        public clsDoctorProcedureLinkVO LinkDetails
        {
            get { return _Details; }
            set { _Details = value; }
        }
        private List<clsDoctorProcedureLinkVO> _DetailsList;
        public List<clsDoctorProcedureLinkVO> LinkDetailsList
        {
            get { return _DetailsList; }
            set { _DetailsList = value; }
        }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

    }
    public class clsDeleteDoctorProcedureLinkBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Billing.clsDeleteDoctorProcedureLinkBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

        private clsDoctorProcedureLinkVO _Details;
        public clsDoctorProcedureLinkVO LinkDetails
        {
            get { return _Details; }
            set { _Details = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
    }
}
