using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.DoctorPayment
{
    public class clsGetPaidDoctorPaymentDetailsBizActionVO : IBizActionValueObject
    {

        private List<clsDoctorPaymentVO> _DoctorDetails;
        public List<clsDoctorPaymentVO> DoctorDetails
        {
            get { return _DoctorDetails; }
            set { _DoctorDetails = value; }
        }
        private clsDoctorPaymentVO _DoctorInfo;
        public clsDoctorPaymentVO DoctorInfo
        {
            get { return _DoctorInfo; }
            set { _DoctorInfo = value; }
        }

        public long DoctorId { get; set; }

        public long DoctorPaymentId { get; set; }


        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {   _UnitID = value;}
        }

        private long _BillID;
        public long BillID
        {
            get { return _BillID; }
            set
            { _BillID = value; }
        }

        private long _BillUnitID;
        public long BillUnitID
        {
            get { return _BillUnitID; }
            set
            { _BillUnitID = value; }
        }

        private double _TotalBillAmount;
        public double TotalBillAmount
        {
            get { return _TotalBillAmount; }
            set
            {
                if (_TotalBillAmount != value)
                {
                    _TotalBillAmount = value;
                  
                }
            }
        }

        private double _TotalShareAmount;
        public double TotalShareAmount
        {
            get { return _TotalShareAmount; }
            set
            {
                if (_TotalShareAmount != value)
                {
                    _TotalShareAmount = value;

                }
            }
        }


        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set
            { _ServiceID = value; }
        }


        private double _ShareAmount;
        public double ShareAmount
        {
            get { return _ShareAmount; }
            set
            {
                if (_ShareAmount != value)
                {
                    _ShareAmount = value;

                }
            }
        }

        public string ServiceName { get; set; }



        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.DocPayment.clsGetPaidDoctorPaymentDetailsBizAction";
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
