using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.ValueObjects.Master.DoctorPayment
{
    public class clsGetDoctorPaymentBizActionVO : IBizActionValueObject
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

        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.DocPayment.clsGetDoctorPaymentBizAction";
        }

        #endregion


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    //R
    public class clsGetDoctorPaymentChildBizActionVO : IBizActionValueObject
    {
        //private List<clsGetDoctorPaymentShareDetailsBizActionVO> _DoctorDetails;
        //public List<clsGetDoctorPaymentShareDetailsBizActionVO> DoctorDetails
        //{
        //    get { return _DoctorDetails; }
        //    set { _DoctorDetails = value; }
        //}
        private clsDoctorPaymentVO _DoctorInfo;
        public clsDoctorPaymentVO DoctorInfo
        {
            get { return _DoctorInfo; }
            set { _DoctorInfo = value; }
        }
        private List<clsDoctorPaymentVO> _DoctorInfoList;
        public List<clsDoctorPaymentVO> DoctorInfoList
        {
            get { return _DoctorInfoList; }
            set { _DoctorInfoList = value; }
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }


        public long DoctorId { get; set; }

        public bool IsPagingEnabled { get; set; }

        public bool IsSettleBill { get; set; }

        public bool IsCreditBill { get; set; }

        public bool IsForBoth { get; set; }

        public long UnitID { get; set; }


        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.DocPayment.clsGetDoctorPaymentChildBizAction";
        }

        #endregion


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }


    //R
    public class clsGetDoctorPaymentShareDetailsBizActionVO : IBizActionValueObject
    {
        //private List<clsGetDoctorPaymentShareDetailsBizActionVO> _DoctorDetails;
        //public List<clsGetDoctorPaymentShareDetailsBizActionVO> DoctorDetails
        //{
        //    get { return _DoctorDetails; }
        //    set { _DoctorDetails = value; }
        //}
        private clsDoctorPaymentVO _DoctorInfo;
        public clsDoctorPaymentVO DoctorInfo
        {
            get { return _DoctorInfo; }
            set { _DoctorInfo = value; }
        }
        private List<clsDoctorPaymentVO> _DoctorInfoList;
        public List<clsDoctorPaymentVO> DoctorInfoList
        {
            get { return _DoctorInfoList; }
            set { _DoctorInfoList = value; }
        }

        public bool IsForAllData { get; set; }
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }


        public long DoctorId { get; set; }
        public string DoctorIds { get; set; }

        private long _TariffServiceID;
        public long TariffServiceID
        {
            get { return _TariffServiceID; }
            set
            {
                if (_TariffServiceID != value)
                {
                    _TariffServiceID = value;

                }
            }
        }

        public bool IsPagingEnabled { get; set; }

        public bool IsSettleBill { get; set; }

        public bool IsCreditBill { get; set; }

        public bool IsForBoth { get; set; }

        public long UnitID { get; set; }


        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.DocPayment.clsGetDoctorSharePaymentBizAction";
        }

        #endregion


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    //R
    public class clsGetDoctorPaymentDetailListBizActionVO : IBizActionValueObject
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



        public long DoctorID { get; set; }

        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.DocPayment.clsGetDoctorPaymentDetailListBizAction";
        }

        #endregion


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetDoctorShareRangeList : IBizActionValueObject
    {


        public List<clsDoctorShareRangeList> DoctorShareRangeList { get; set; }
        public clsDoctorShareRangeList DoctorShareRangeDetails { get; set; }
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //return "PalashDynamics.BusinessLayer.Master.DocPayment.clsGetDoctorShareRangeListBizAction";
            return "PalashDynamics.BusinessLayer.Administration.DoctorShareRange.clsGetDoctorShareRangeListBizAction";
        }

        #endregion


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }
        private List<clsDoctorShareRangeVO> _ShareRangeList = new List<clsDoctorShareRangeVO>();
        public List<clsDoctorShareRangeVO> ShareRangeList
        {
            get { return _ShareRangeList; }
            set { _ShareRangeList = value; }
        }
        #endregion
    }

    public class clsDoctorShareRangeList
    {
        public long ID { get; set; }
        public decimal LowerLimit { get; set; }
        public decimal UpperLimit { get; set; }
        public decimal SharePercentage { get; set; }
        public decimal ShareAmount { get; set; }
        public bool Status { get; set; }
    }
}
