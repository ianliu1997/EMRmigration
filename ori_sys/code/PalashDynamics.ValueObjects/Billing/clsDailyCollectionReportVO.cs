using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using PalashDynamics.ValueObjects.OutPatientDepartment;

namespace PalashDynamics.ValueObjects.Billing
{
    public class clsDailyCollectionReportVO : IValueObject
    {

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        public MaterPayModeList PaymentModeID { get; set; }
        public DateTime Date { get; set; }
        public double Collection { get; set; }

        public string Paymode
        {
            get
            {
                return PaymentModeID.ToString();
            }

        }

    }

    public class clsDailySalesReportVO : IValueObject
    {

        public string ToXml()
        {
            throw new NotImplementedException();
        }
        public DateTime Date { get; set; }
        public double Collection { get; set; }

        public double TotalAmount { get; set; }
        public string Specialization { get; set; }
    }



    public class clsGetDailyCollectionListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsGetDailyCollectionListBizAction";
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
        public long UnitID { get; set; }
        public bool FromCashClosing { get; set; }
        public bool ClosingPharmacy { get; set; }
        public bool ISAppointmentList { get; set; }
        public bool DailySales { get; set; }
        public bool IsVisit { get; set; }
        public DateTime? CollectionDate { get; set; }
        public DateTime? ToDate { get; set; }

        private List<clsDailySalesReportVO> objSalesList = null;
        public List<clsDailySalesReportVO> SalesList
        {
            get { return objSalesList; }
            set { objSalesList = value; }
        }

        private List<clsAppointmentVO> objAppointmentList = null;
        public List<clsAppointmentVO> AppointmentList
        {
            get { return objAppointmentList; }
            set { objAppointmentList = value; }
        }

        private List<clsVisitVO> objVisitList = null;
        public List<clsVisitVO> VisitList
        {
            get { return objVisitList; }
            set { objVisitList = value; }
        }

        private List<clsDailyCollectionReportVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsDailyCollectionReportVO> List
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
    }

}
