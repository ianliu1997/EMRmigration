using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.CompanyPayment
{
    public class clsGetCompanyPaymentDetailsBizActionVO:IBizActionValueObject
    {

        private List<clsCompanyPaymentDetailsVO> _CompanyPaymentDetails;
        public List<clsCompanyPaymentDetailsVO> CompanyPaymentDetails
        {
            get { return _CompanyPaymentDetails; }
            set { _CompanyPaymentDetails = value; }
        }
        private clsCompanyPaymentDetailsVO _CompanyPaymentDetailsInfo;
        public clsCompanyPaymentDetailsVO CompanyPaymentDetailsInfo
        {
            get { return _CompanyPaymentDetailsInfo; }
            set { _CompanyPaymentDetailsInfo = value; }
        }

        private string _FromDate;
        public string FromDate
        {
            get { return _FromDate; }
            set
            {
                if (_FromDate != value)
                {
                    _FromDate = value;
                
                }
            }
        }

        private string _ToDate;
        public string ToDate
        {
            get { return _ToDate; }
            set
            {
                if (_ToDate != value)
                {
                    _ToDate = value;

                }
            }
        }

        //Added PMG 19/12/2015
        public bool IsFromNewForm { get; set; }

        public bool IsChild{get;  set;  }
        public bool IsCompanyForm { get; set; }
        public bool ServiceWise { get; set; }
        public string PageName { get; set; }
        public long UnitID { get; set; }
        public long PatientSouceID { get; set; }
        public long DepartmentID { get; set; }
        public string DoctorID { get; set; }
        public string CompanyID { get; set; }
        public string AssCompanyID { get; set; }
        public long TariffID { get; set; }
        public string ServiceID { get; set; }
        public long TariffServiceID { get; set; }
        public long SpecializationID { get; set; }
        public long BillNo { get; set; }
        public bool IsPaidBill { get; set; }

        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetCompanyPaymentDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion






    }

    public class clsGetCompanyInvoiceDetailsBizActionVO : IBizActionValueObject
    {

        private List<clsCompanyPaymentDetailsVO> _CompanyPaymentDetails;
        public List<clsCompanyPaymentDetailsVO> CompanyPaymentDetails
        {
            get { return _CompanyPaymentDetails; }
            set { _CompanyPaymentDetails = value; }
        }
        private clsCompanyPaymentDetailsVO _CompanyPaymentDetailsInfo;
        public clsCompanyPaymentDetailsVO CompanyPaymentDetailsInfo
        {
            get { return _CompanyPaymentDetailsInfo; }
            set { _CompanyPaymentDetailsInfo = value; }
        }

        private DateTime? _FromDate;
        public DateTime? FromDate
        {
            get { return _FromDate; }
            set
            {
                if (_FromDate != value)
                {
                    _FromDate = value;

                }
            }
        }
        public bool InvoicePrint = false;

        public string InvoiceNo { get; set; }
        private DateTime? _ToDate;
        public DateTime? ToDate
        {
            get { return _ToDate; }
            set
            {
                if (_ToDate != value)
                {
                    _ToDate = value;

                }
            }
        }
        public string BillNO { get; set; }
        public string MRNO { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsChild { get; set; }
        public bool IsCompanyForm { get; set; }
        public bool ServiceWise { get; set; }
        public string PageName { get; set; }
        public long UnitID { get; set; }
        public long PatientSouceID { get; set; }
        public long DepartmentID { get; set; }
        public string DoctorID { get; set; }
        public string CompanyID { get; set; }
        public string AssCompanyID { get; set; }
        public long TariffID { get; set; }
        public string ServiceID { get; set; }
        public long TariffServiceID { get; set; }
        public long SpecializationID { get; set; }
        public string BillNo { get; set; }
        public bool IsPaidBill { get; set; }
        public bool FromInvoice { get; set; }
        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetCompanyInvoiceDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion






    }

    public class clsAddCompanyInvoiceBizActionVO : IBizActionValueObject
    {

        private List<clsCompanyPaymentDetailsVO> _BillDetails;
        public List<clsCompanyPaymentDetailsVO> BillDetails
        {
            get { return _BillDetails; }
            set { _BillDetails = value; }
        }

        private clsCompanyPaymentDetailsVO _CompanyDetails;
        public clsCompanyPaymentDetailsVO CompanyDetails
        {
            get { return _CompanyDetails; }
            set { _CompanyDetails = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsAddCompanyInvoiceBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion






    }



    public class clsGetBillAgainstInvoiceBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Master.clsGetBillAgainstInvoiceBizAction";
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

        public long InvoiceID { get; set; }
        public long UnitID { get; set; }

        private List<clsCompanyPaymentDetailsVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsCompanyPaymentDetailsVO> List
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
    }

    public class clsGetCompanyInvoiceForModifyVO : IBizActionValueObject
    {

        private List<clsCompanyPaymentDetailsVO> _CompanyPaymentDetails;
        public List<clsCompanyPaymentDetailsVO> CompanyPaymentDetails
        {
            get { return _CompanyPaymentDetails; }
            set { _CompanyPaymentDetails = value; }
        }
        private clsCompanyPaymentDetailsVO _CompanyPaymentDetailsInfo;
        public clsCompanyPaymentDetailsVO CompanyPaymentDetailsInfo
        {
            get { return _CompanyPaymentDetailsInfo; }
            set { _CompanyPaymentDetailsInfo = value; }
        }

        private string _FromDate;
        public string FromDate
        {
            get { return _FromDate; }
            set
            {
                if (_FromDate != value)
                {
                    _FromDate = value;

                }
            }
        }

        private string _ToDate;
        public string ToDate
        {
            get { return _ToDate; }
            set
            {
                if (_ToDate != value)
                {
                    _ToDate = value;

                }
            }
        }

        //Added PMG 19/12/2015
        public bool IsFromNewForm { get; set; }

        public long InvoiceID { get; set; }

        public bool IsChild { get; set; }
        public bool IsCompanyForm { get; set; }
        public bool ServiceWise { get; set; }
        public string PageName { get; set; }
        public long UnitID { get; set; }
        public long PatientSouceID { get; set; }
        public long DepartmentID { get; set; }
        public string DoctorID { get; set; }
        public string CompanyID { get; set; }
        public string AssCompanyID { get; set; }
        public long TariffID { get; set; }
        public string ServiceID { get; set; }
        public long TariffServiceID { get; set; }
        public long SpecializationID { get; set; }
        public long BillNo { get; set; }
        public bool IsPaidBill { get; set; }

        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetCompanyInvoiceForModify";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion






    }

    public class clsModifyCompanyInvoiceBizActionVO : IBizActionValueObject
    {

        public long InvoiceID { get; set; }

        public long CompanyID { get; set; }
        public long UnitID { get; set; }

        private List<clsCompanyPaymentDetailsVO> _BillDetails;
        public List<clsCompanyPaymentDetailsVO> BillDetails
        {
            get { return _BillDetails; }
            set { _BillDetails = value; }
        }

        private clsCompanyPaymentDetailsVO _CompanyDetails;
        public clsCompanyPaymentDetailsVO CompanyDetails
        {
            get { return _CompanyDetails; }
            set { _CompanyDetails = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsModifyCompanyInvoiceBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion






    }


    public class clsDeleteCompanyInvoiceBillBizActionVO : IBizActionValueObject
    {

        public long InvoiceID { get; set; }
        public long BillID { get; set; }


        public long CompanyID { get; set; }
        public long UnitID { get; set; }

        private List<clsCompanyPaymentDetailsVO> _BillDetails;
        public List<clsCompanyPaymentDetailsVO> BillDetails
        {
            get { return _BillDetails; }
            set { _BillDetails = value; }
        }

        private clsCompanyPaymentDetailsVO _CompanyDetails;
        public clsCompanyPaymentDetailsVO CompanyDetails
        {
            get { return _CompanyDetails; }
            set { _CompanyDetails = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsDeleteCompanyInvoiceBillBizAction";
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
