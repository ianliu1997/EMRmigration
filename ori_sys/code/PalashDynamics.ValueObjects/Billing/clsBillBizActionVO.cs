using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Master.CompanyPayment;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.Log;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;

namespace PalashDynamics.ValueObjects.Billing
{
    public class clsAddBillBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsAddBillBizAction";
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

        private clsBillVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsBillVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
        private List<clsChargeVO> _objDeletedRadSerDetailsList = null;//Added By Yogesh K 1 7 16
        public List<clsChargeVO> DeletedRadSerDetailsList
        {
            get { return _objDeletedRadSerDetailsList; }
            set { _objDeletedRadSerDetailsList = value; }
        }
        public bool IsPackageBill { get; set; }

        private long _PackageID;
        public long PackageID
        {
            get { return _PackageID; }
            set
            {
                if (_PackageID != value)
                {
                    _PackageID = value;
                    //OnPropertyChanged("PackageID");
                }
            }
        }

        //***//----

        private string _PrescriptionDetailsDrugID;
        public string PrescriptionDetailsDrugID
        {
            get { return _PrescriptionDetailsDrugID; }
            set
            {
                if (_PrescriptionDetailsDrugID != value)

                {
                    _PrescriptionDetailsDrugID = value;
                    //OnPropertyChanged("PackageID");
                }
            }
        }

        public bool IsCouterSalesPackage { get; set; }

        private string _PrescriptionDetailsID;
        public string PrescriptionDetailsID
        {
            get { return _PrescriptionDetailsID; }
            set
            {
                if (_PrescriptionDetailsID != value)
                {
                    _PrescriptionDetailsID = value;
                    //OnPropertyChanged("PackageID");
                }
            }
        }

        private string _InvestigationDetailsID;
        public string InvestigationDetailsID
        {
            get { return _InvestigationDetailsID; }
            set
            {
                if (_InvestigationDetailsID != value)
                {
                    _InvestigationDetailsID = value;
                    //OnPropertyChanged("PackageID");
                }
            }
        }
        //
        //----------
      
        public bool IsFromIPDCancel { get; set; }

        //By Anjali.........................................


        public bool IsFromCounterSale { get; set; }
        private clsAddPatientBizActionVO objPatientVO = null;
        public clsAddPatientBizActionVO objPatientVODetails
        {
            get { return objPatientVO; }
            set { objPatientVO = value; }
        }
        private clsAddVisitBizActionVO objVisitVO = null;
        public clsAddVisitBizActionVO objVisitVODetails
        {
            get { return objVisitVO; }
            set { objVisitVO = value; }
        }

        //added by rohini dated 6.4.16

        public bool IsFromPathologyLab { get; set; }
        private clsAddPatientForPathologyBizActionVO objPathoPatientVO = null;
        public clsAddPatientForPathologyBizActionVO obPathoPatientVODetails
        {
            get { return objPathoPatientVO; }
            set { objPathoPatientVO = value; }
        }

        private clsAddVisitBizActionVO objPathoPatientVisitVO = null;
        public clsAddVisitBizActionVO obPathoPatientVisitVODetails
        {
            get { return objPathoPatientVisitVO; }
            set { objPathoPatientVisitVO = value; }
        }
        //..................................................

        #region For Package Advance & Bill Save in transaction : added on 16082018

        private clsAddAdvanceBizActionVO objPackageAdvanceVO = null;
        public clsAddAdvanceBizActionVO objPackageAdvanceVODetails
        {
            get { return objPackageAdvanceVO; }
            set { objPackageAdvanceVO = value; }
        }

        public bool IsFromSaveAndPackageBill { get; set; }

        #endregion

        // For the Activity Log List
        private List<LogInfo> _LogInfoList = null;
        public List<LogInfo> LogInfoList
        {
            get { return _LogInfoList; }
            set { _LogInfoList = value; }
        }
        public bool IsForPackageAdvanceCheck { get; set; }  // true for package service consumption advance check while saving.. frmBill.xaml.cs
        public decimal BalancePackageAdvance { get; set; }
        public decimal ServiceConsumptionAmount { get; set; }
        public decimal PharmacyConsumptionAmount { get; set; }

        #region Package New Changes Added on 28042018
        // Set to True when call from Counter Sale to check whether Pharmacy Consumed Amount > Pharmacy  Component
        public bool IsPackagePharmacyConsumption;

        private clsGetPatientPackageInfoListBizActionVO objPatientPackInfoVO = null;
        public clsGetPatientPackageInfoListBizActionVO objPatientPackInfoVODetails
        {
            get { return objPatientPackInfoVO; }
            set { objPatientPackInfoVO = value; }
        }

        private int _SuccessStatusForCSConsumtion;  // Set to 1 when Pharmacy Consumed Amount > Pharmacy  Component
        public int SuccessStatusForCSConsumtion
        {
            get { return _SuccessStatusForCSConsumtion; }
            set { _SuccessStatusForCSConsumtion = value; }
        }

        #endregion

    }

    public class clsGetCompanyCreditDtlsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsGetCompanyCreditDtlsBizAction";
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

        private clsCompanyCreditDetailsVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsCompanyCreditDetailsVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
    }

    public class clsUpdatePaymentDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsUpdatePaymentDetailsBizAction";
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
        public long BillID { get; set; }
        public string BillNO { get; set; }
        public string MRNO { get; set; }
        public long UnitID { get; set; }
        public long BankID
        {
            get;
            set;

        }
        public string Number { get; set; }
        public string Bank { get; set; }
        public long PaymentId { get; set; }

        public long PaymentDetailId { get; set; }

        public long PaymentModeID { get; set; }
        public DateTime? Date
        {
            get;
            set;

        }
        public double PaidAmount { get; set; }


        private List<clsBillVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsBillVO> PendingBillList
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

    }
    public class clsMaintainPaymentModeLogBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsMaintainPaymentModeLogBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        //public DateTime? Date { get; set; }



        //public string BillNO { get; set; }
        //public string MRNO { get; set; }
        //public string FirstName { get; set; }
        //public string MiddleName { get; set; }
        //public string LastName { get; set; }
        //public long UnitID { get; set; }
        //public long BillID { get; set; }
        //public string Number { get; set; }
        //public string Bank { get; set; }
        //public double PaidAmount { get; set; }
        public long PaymentID { get; set; }
        public long PaymentDetailId { get; set; }
    }

    public class clsGetFreezedBillListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsGetFreezedBillListBizAction";
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
        public long PaymentId { get; set; }
        public string IPDNO { get; set; }
        public long PaymentDetailId { get; set; }


        // Added by CDS
        public long CostingDivisionID { get; set; }

        public BillTypes? BillType { get; set; }

        public Int16 BillStatus { get; set; }


        public long UnitID { get; set; }
        public long BillID { get; set; }
        public string Number { get; set; }
        public string Bank { get; set; }

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

        public List<clsBillVO> PendingBillList
        {
            get { return objDetails; }
            set { objDetails = value; }
        }


        #region For IPD Module

        public bool IsIPDBillList { get; set; }

        public bool? IsRefunded { get; set; }
        public string ContactNo { get; set; }
        public bool FromRefund { get; set; }

        #endregion

    }
    public class clsGetFreezedBillSearchListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsGetFreezedBillSearchListBizAction";
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

        public bool IsPaymentModeChange { get; set; }
        public string IPDNO { get; set; }

        // Added by CDS
        public long CostingDivisionID { get; set; }

        public BillTypes? BillType { get; set; }

        public Int16 BillStatus { get; set; }


        public long UnitID { get; set; }

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

        public List<clsBillVO> PendingBillList
        {
            get { return objDetails; }
            set { objDetails = value; }
        }


        #region For IPD Module

        public bool IsIPDBillList { get; set; }

        public bool? IsRefunded { get; set; }
        public string ContactNo { get; set; }
        public bool FromRefund { get; set; }

        #endregion


    }
    public class clsGetBillSearchListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsGetBillSearchListBizAction";
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

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

       


        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public long Opd_Ipd_External_Id { get; set; }
        public long Opd_Ipd_External_UnitId { get; set; } //Added by AJ Date 8/2/2017
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

        public string IPDNO { get; set; }

        // Added by CDS
        public long CostingDivisionID { get; set; }

        public BillTypes? BillType { get; set; }

        public Int16 BillStatus { get; set; }


        public long UnitID { get; set; }

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

        public List<clsBillVO> PendingBillList
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        //By Anjali.........
        public bool IsFromApprovalRequestWindow { get; set; }
        public bool IsRequest { get; set; }
        public long RequestTypeID { get; set; }
        public long UserLevelID { get; set; }
        public long UserRightsTypeID { get; set; }
        public bool IsConsumption { get; set; }
        //..............................
        #region For IPD Module

        public bool IsIPDBillList { get; set; }

        public bool? IsRefunded { get; set; }
        public string ContactNo { get; set; }
        public bool FromRefund { get; set; }
        public bool IsOPDBill { get; set; }


        #endregion

    }

    //***//----------------------------
    public class clsGetBillClearanceBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsGetBillClearanceBizAction";
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

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }




        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public long Opd_Ipd_External_Id { get; set; }
        public long Opd_Ipd_External_UnitId { get; set; } 
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

        public string IPDNO { get; set; }
      
        public long CostingDivisionID { get; set; }

        public BillTypes? BillType { get; set; }

        public Int16 BillStatus { get; set; }


        public long UnitID { get; set; }

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
      
        public bool IsFromApprovalRequestWindow { get; set; }
        public bool IsRequest { get; set; }
        public long RequestTypeID { get; set; }
        public long UserLevelID { get; set; }
        public long UserRightsTypeID { get; set; }
        public bool IsConsumption { get; set; }
      
        #region For IPD Module

        public bool IsIPDBillList { get; set; }

        public bool? IsRefunded { get; set; }
        public string ContactNo { get; set; }
        public bool FromRefund { get; set; }
        public bool IsOPDBill { get; set; }


        #endregion
      
        public long ScheduleID { get; set; }
        public long ScheduleUnitID { get; set; }
        public bool IsSaveBillClearance { get; set; }
        public long OTBillClearanceID { get; set; }
        public long BillClearanceID { get; set; }
        public long BillClearanceUnitID { get; set; }
        public bool IsBillClearanceList { get; set; }
    }
    //----------------------------------

    public class clsUpdateBillPaymentDtlsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsUpdateBillPaymentDtlsBizAction";
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



        private clsBillVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsBillVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }


        private clsCompanyPaymentDetailsVO objDetails1 = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsCompanyPaymentDetailsVO Details1
        {
            get { return objDetails1; }
            set { objDetails1 = value; }
        }

        #region For IPD Module

        public bool FromCompanyPayment = false;

        public List<clsChargeVO> objChargeDetails = null;
        public List<clsChargeVO> ChargeDetails
        {
            get { return objChargeDetails; }
            set { objChargeDetails = value; }
        }


        private clsCompanyPaymentDetailsVO _CompanyPaymentDetailsInfo = new clsCompanyPaymentDetailsVO();
        public clsCompanyPaymentDetailsVO CompanyPaymentDetailsInfo
        {
            get { return _CompanyPaymentDetailsInfo; }
            set { _CompanyPaymentDetailsInfo = value; }
        }

        #endregion

        //Added By Changdeo sase
        //# region Costing Divisions for Clinical & Pharmacy Billing

        //public long CostingDivisionID { get; set; }

        //# endregion

    }
    // BY BHUSHAN . . . . . . . . . . 
    public class clsGetBillSearch_IVF_List_DashBoardBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsGetBillSearch_IVF_List_DashBoardBizAction";
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

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string MRNO { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public long BillType { get; set; }
        public long UnitID { get; set; }

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
    // BY BHUSHAN . . . . . . . . . . 
    public class clsGetBillSearch_USG_List_DashBoardBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsGetBillSearch_USG_List_DashBoardBizAction";
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

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string MRNO { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public long BillType { get; set; }
        public long UnitID { get; set; }
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
    // . . . . . . . . . . 

    #region For IPD Module

    public class clsFillGrossDiscountReasonBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsFillGrossDiscountReasonBizAction";
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

        private List<MasterListItem> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<MasterListItem> MasterList
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
    }

    #endregion



}