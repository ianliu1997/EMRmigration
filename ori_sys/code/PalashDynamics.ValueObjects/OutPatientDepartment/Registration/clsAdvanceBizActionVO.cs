using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Registration
{
    public class clsAddAdvanceBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddAdvanceBizAction";
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

        private clsAdvanceVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsAdvanceVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        public bool IsRefundToAdvance { get; set; }     // Refund to Advance 20042017
        public long BillID { get; set; }     // Refund to Advance 20042017
    }
    
    public class clsGetAdvanceListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetAdvanceListBizAction";
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

        public long ID { get; set; }

        public long UnitID { get; set; }

        public long CostingDivisionID { get; set; }
        

        public long PatientID { get; set; }

        public bool AllCompanies { get; set; }

        public long CompanyID { get; set; }

        public long PatientUnitID { get; set; }
        private List<clsAdvanceVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsAdvanceVO> Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsFromApprovalRequest{ get; set; }
        public bool IsRequest { get; set; }
        public long RequestTypeID { get; set; }
        public long UserLevelID { get; set; }
        public long UserRightsTypeID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }


        private decimal _PatientDailyCashAmount;
        public decimal PatientDailyCashAmount
        {
            get { return _PatientDailyCashAmount; }
            set
            {
                if (_PatientDailyCashAmount != value)
                {
                    _PatientDailyCashAmount = value;
                }
            }
        }

    }
    
    public class clsDeleteAdvanceBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsDeleteAdvanceBizAction";
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

        public long ID { get; set; }
    }


    public class clsGetPatientAdvanceListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientAdvanceListBizAction";
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

        public long ID { get; set; }

        public long UnitID { get; set; }

        public long CostingDivisionID { get; set; }
        
        public long PatientUnitID { get; set; }

        public long PatientID { get; set; }

        public bool IsFromCompany { get; set; }

        public long CompanyID { get; set; }

        private List<clsAdvanceVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsAdvanceVO> Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        private clsAdvanceVO objAdvanceVO = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsAdvanceVO AdvanceDetails
        {
            get { return objAdvanceVO; }
            set { objAdvanceVO = value; }
        }

        private decimal _PatientDailyCashAmount;
        public decimal PatientDailyCashAmount
        {
            get { return _PatientDailyCashAmount; }
            set
            {
                if (_PatientDailyCashAmount != value)
                {
                    _PatientDailyCashAmount = value;
                }
            }
        }

        public bool IsShowBothAdvance { get; set; }     // For Package New Changes Added on 16062018

    }

    //public class clsGetAdvanceBizActionVO : IBizActionValueObject
    //{
    //    #region IBizActionValueObject Members

    //    public string GetBizAction()
    //    {
    //        //throw new NotImplementedException();
    //        return "PalashDynamics.BusinessLayer.clsGetAdvanceBizAction";
    //    }

    //    #endregion

    //    #region IValueObject Members

    //    public string ToXml()
    //    {
    //        return this.ToString();
    //    }

    //    #endregion

    //    private int _SuccessStatus;
    //    /// <summary>
    //    /// Output Property.
    //    /// This property states the outcome of BizAction Process.
    //    /// </summary>
    //    public int SuccessStatus
    //    {
    //        get { return _SuccessStatus; }
    //        set { _SuccessStatus = value; }
    //    }

    //    private clsAdvanceVO objDetails = null;
    //    /// <summary>
    //    /// Output Property.
    //    /// This Property Contains OPDPatient Details Which is Added.
    //    /// </summary>
    //    public clsAdvanceVO Details
    //    {
    //        get { return objDetails; }
    //        set { objDetails = value; }
    //    }
    //}
}
