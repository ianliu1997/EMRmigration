using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.ValueObjects.Billing
{
    public class clsPreviousPatientBillsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Billing.clsPreviousBillListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public bool IsPharmacyQueue { get; set; }
        public bool FromRefund { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public long Opd_Ipd_External_Id { get; set; }
        public Int32 Opd_Ipd_External { get; set; }
        public long? PatientID { get; set; }
        public long? PatientUnitID { get; set; }
        public string OPDNO { get; set; }
        public string BillNO { get; set; }
        public string MRNO { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool? IsFreeze { get; set; }

        public BillTypes? BillType { get; set; }

        public Int16 BillStatus { get; set; }

        public long UnitID { get; set; }

        public long NoUse { get; set; }

        private List<clsBillVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsBillVO> List
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

    }
}
