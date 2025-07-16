using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Billing
{
   public class clsAccountsVO : IValueObject, INotifyPropertyChanged
    {

        public string ToXml()
        {
            //throw new NotImplementedException();
            return this.ToString();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        private long _StoreID;
        public long StoreID
        {
            get { return _StoreID; }
            set
            {
                if (_StoreID != value)
                {
                    _StoreID = value;
                    OnPropertyChanged("StoreID");
                }
            }
        }

        #region Property Declaration Section

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private DateTime _ExportDate = DateTime.Now;
        public DateTime ExportDate
        {
            get { return _ExportDate; }
            set
            {
                if (_ExportDate != value)
                {
                    _ExportDate = value;
                    OnPropertyChanged("ExportDate");
                }
            }
        }
        private List<clsLedgerVO> _SaleSelfReceiptLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> SaleSelfReceiptLedgers
        {
            get { return _SaleSelfReceiptLedgers; }
            set
            {
                if (_SaleSelfReceiptLedgers != value)
                {
                    _SaleSelfReceiptLedgers = value;
                    OnPropertyChanged("SaleSelfReceiptLedgers");
                }
            }
        }

        private List<clsLedgerVO> _SaleCompanyReceiptLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> SaleCompanyReceiptLedgers
        {
            get { return _SaleCompanyReceiptLedgers; }
            set
            {
                if (_SaleCompanyReceiptLedgers != value)
                {
                    _SaleCompanyReceiptLedgers = value;
                    OnPropertyChanged("SaleCompanyReceiptLedgers");
                }
            }
        }
        private DateTime _FromDate = DateTime.Now;
        public DateTime FromDate
        {
            get { return _FromDate; }
            set
            {
                if (_FromDate != value)
                {
                    _FromDate = value;
                    OnPropertyChanged("FromDate");
                }
            }
        }

        private DateTime _ToDate = DateTime.Now;
        public DateTime ToDate
        {
            get { return _ToDate; }
            set
            {
                if (_ToDate != value)
                {
                    _ToDate = value;
                    OnPropertyChanged("ToDate");
                }
            }
        }

        private List<clsLedgerVO> _SaleIncome = new List<clsLedgerVO>();
        public List<clsLedgerVO> SaleIncome
        {
            get { return _SaleIncome; }
            set
            {
                if (_SaleIncome != value)
                {
                    _SaleIncome = value;
                    OnPropertyChanged("SaleIncome");
                }
            }
        }

        private List<clsLedgerVO> _SaleCredit = new List<clsLedgerVO>();
        public List<clsLedgerVO> SaleCredit
        {
            get { return _SaleCredit; }
            set
            {
                if (_SaleCredit != value)
                {
                    _SaleCredit = value;
                    OnPropertyChanged("SaleCredit");
                }
            }
        }

        private List<clsLedgerVO> _SaleReturn = new List<clsLedgerVO>();
        public List<clsLedgerVO> SaleReturn
        {
            get { return _SaleReturn; }
            set
            {
                if (_SaleReturn != value)
                {
                    _SaleReturn = value;
                    OnPropertyChanged("SaleReturn");
                }
            }
        }

        private List<clsLedgerVO> _OPDSelfCreditBillAccount = new List<clsLedgerVO>();
        public List<clsLedgerVO> OPDSelfCreditBillAccount
        {
            get { return _OPDSelfCreditBillAccount; }
            set
            {
                if (_OPDSelfCreditBillAccount != value)
                {
                    _OPDSelfCreditBillAccount = value;
                    OnPropertyChanged("OPDSelfCreditBillAccount");
                }
            }
        }

        public double TotalOPDSelfBillDR { get; set; }
        public double TotalOPDSelfBillCR { get; set; }
        public double TotalOPDSelfRefundBillDR { get; set; }
        public double TotalOPDSelfRefundBillCR { get; set; }

        public double TotalItemSaleDR { get; set; }
        public double TotalItemSaleCR { get; set; }

        public double TotalItemSaleReturnDR { get; set; }
        public double TotalItemSaleReturnCR { get; set; }

        public double TotalPurchaseCashDR { get; set; }
        public double TotalPurchaseCashCR { get; set; }

        public double TotalPurchaseCreditDR { get; set; }
        public double TotalPurchaseCreditCR { get; set; }
       
        public double TotalPurchaseReturnCashDR { get; set; }
        public double TotalPurchaseReturnCashCR { get; set; }

        public double TotalPurchaseReturnCreditDR { get; set; }
        public double TotalPurchaseReturnCreditCR { get; set; }

        public double TotalScrapDR { get; set; }
        public double TotalScrapCR { get; set; }

        public double TotalConsumptionDR { get; set; }
        public double TotalConsumptionCR { get; set; }

        private List<clsLedgerVO> _OPDBillsLedgerAccount = new List<clsLedgerVO>();
        public List<clsLedgerVO> OPDBillsLedgerAccount
        {
            get { return _OPDBillsLedgerAccount; }
            set
            {
                if (_OPDBillsLedgerAccount != value)
                {
                    _OPDBillsLedgerAccount = value;
                    OnPropertyChanged("OPDBillsLedgerAccount");
                }
            }
        }

       private List<clsLedgerVO> _OPDSelfBillLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> OPDSelfBillLedgers
        {
            get { return _OPDSelfBillLedgers; }
            set
            {
                if (_OPDSelfBillLedgers != value)
                {
                    _OPDSelfBillLedgers = value;
                    OnPropertyChanged("OPDSelfBillLedgers");
                }
            }
        }
        private List<clsLedgerVO> _RefDoctorLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> RefDoctorLedger
        {
            get { return _RefDoctorLedgers; }
            set
            {
                if (_RefDoctorLedgers != value)
                {
                    _RefDoctorLedgers = value;
                    OnPropertyChanged("RefDoctorLedger");
                }
            }
        }

        private List<clsLedgerVO> _RefDoctorPaymentLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> RefDoctorPaymentLedger
        {
            get { return _RefDoctorPaymentLedgers; }
            set
            {
                if (_RefDoctorPaymentLedgers != value)
                {
                    _RefDoctorPaymentLedgers = value;
                    OnPropertyChanged("RefDoctorPaymentLedger");
                }
            }
        }
        private List<clsLedgerVO> _OPDRefundOfBillLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> OPDRefundOfBillLedgers
        {
            get { return _OPDRefundOfBillLedgers; }
            set
            {
                if (_OPDRefundOfBillLedgers != value)
                {
                    _OPDRefundOfBillLedgers = value;
                    OnPropertyChanged("OPDRefundOfBillLedgers");
                }
            }
        }
        private List<clsLedgerVO> _ItemSaleLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> ItemSaleLedgers
        {
            get { return _ItemSaleLedgers; }
            set
            {
                if (_ItemSaleLedgers != value)
                {
                    _ItemSaleLedgers = value;
                    OnPropertyChanged("ItemSaleLedgers");
                }
            }
        }

        private List<clsLedgerVO> _ItemSaleReturnLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> ItemSaleReturnLedgers
        {
            get { return _ItemSaleReturnLedgers; }
            set
            {
                if (_ItemSaleReturnLedgers != value)
                {
                    _ItemSaleReturnLedgers = value;
                    OnPropertyChanged("ItemSaleReturnLedgers");
                }
            }
        }

        private List<clsLedgerVO> _PurchaseCashLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> PurchaseCashLedgers
        {
            get { return _PurchaseCashLedgers; }
            set
            {
                if (_PurchaseCashLedgers != value)
                {
                    _PurchaseCashLedgers = value;
                    OnPropertyChanged("PurchaseCashLedgers");
                }
            }
        }

        private List<clsLedgerVO> _PurchaseCreditLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> PurchaseCreditLedgers
        {
            get { return _PurchaseCreditLedgers; }
            set
            {
                if (_PurchaseCreditLedgers != value)
                {
                    _PurchaseCreditLedgers = value;
                    OnPropertyChanged("PurchaseCreditLedgers");
                }
            }
        }


        private List<clsLedgerVO> _PurchaseReturnCashLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> PurchaseReturnCashLedgers
        {
            get { return _PurchaseReturnCashLedgers; }
            set
            {
                if (_PurchaseReturnCashLedgers != value)
                {
                    _PurchaseReturnCashLedgers = value;
                    OnPropertyChanged("PurchaseReturnCashLedgers");
                }
            }
        }

        private List<clsLedgerVO> _PurchaseReturnCreditLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> PurchaseReturnCreditLedgers
        {
            get { return _PurchaseReturnCreditLedgers; }
            set
            {
                if (_PurchaseReturnCreditLedgers != value)
                {
                    _PurchaseReturnCreditLedgers = value;
                    OnPropertyChanged("PurchaseReturnCreditLedgers");
                }
            }
        }

        private List<clsLedgerVO> _ScrapLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> ScrapLedgers
        {
            get { return _ScrapLedgers; }
            set
            {
                if (_ScrapLedgers != value)
                {
                    _ScrapLedgers = value;
                    OnPropertyChanged("ScrapLedgers");
                }
            }
        }

        private List<clsLedgerVO> _ConsumptionLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> ConsumptionLedgers
        {
            get { return _ConsumptionLedgers; }
            set
            {
                if (_ConsumptionLedgers != value)
                {
                    _ConsumptionLedgers = value;
                    OnPropertyChanged("ConsumptionLedgers");
                }
            }
        }



        private List<clsLedgerVO> _BillsLedgerAccountList = new List<clsLedgerVO>();
        public List<clsLedgerVO> BillsLedgerAccountList
        {
            get { return _BillsLedgerAccountList; }
            set
            {
                if (_BillsLedgerAccountList != value)
                {
                    _BillsLedgerAccountList = value;
                    OnPropertyChanged("BillsLedgerAccountList");
                }
            }
        }

        
        private List<clsLedgerVO> _OPDReceiptLedgerAccount = new List<clsLedgerVO>();
        public List<clsLedgerVO> OPDReceiptLedgerAccount
        {
            get { return _OPDReceiptLedgerAccount; }
            set
            {
                if (_OPDReceiptLedgerAccount != value)
                {
                    _OPDReceiptLedgerAccount = value;
                    OnPropertyChanged("OPDReceiptLedgerAccount");
                }
            }
        }

        private List<clsLedgerVO> _OPDCompanyCreditBillAccount = new List<clsLedgerVO>();
        public List<clsLedgerVO> OPDCompanyCreditBillAccount
        {
            get { return _OPDCompanyCreditBillAccount; }
            set
            {
                if (_OPDCompanyCreditBillAccount != value)
                {
                    _OPDCompanyCreditBillAccount = value;
                    OnPropertyChanged("OPDCompanyCreditBillAccount");
                }
            }
        }

        private List<clsLedgerVO> _OPDCompanyReceiptBillAccount = new List<clsLedgerVO>();
        public List<clsLedgerVO> OPDCompanyReceiptBillAccount
        {
            get { return _OPDCompanyReceiptBillAccount; }
            set
            {
                if (_OPDCompanyReceiptBillAccount != value)
                {
                    _OPDCompanyReceiptBillAccount = value;
                    OnPropertyChanged("OPDCompanyReceiptBillAccount");
                }
            }
        }

        private List<clsLedgerVO> _OPDAdvanceLedgerAccount = new List<clsLedgerVO>();
        public List<clsLedgerVO> OPDAdvanceLedgerAccount
        {
            get { return _OPDAdvanceLedgerAccount; }
            set
            {
                if (_OPDAdvanceLedgerAccount != value)
                {
                    _OPDAdvanceLedgerAccount = value;
                    OnPropertyChanged("OPDAdvanceLedgerAccount");
                }
            }
        }

        private List<clsLedgerVO> _OPDRefundBillLedgerAccount = new List<clsLedgerVO>();
        public List<clsLedgerVO> OPDRefundBillLedgerAccount
        {
            get { return _OPDRefundBillLedgerAccount; }
            set
            {
                if (_OPDRefundBillLedgerAccount != value)
                {
                    _OPDRefundBillLedgerAccount = value;
                    OnPropertyChanged("OPDRefundBillLedgerAccount");
                }
            }
        }

        private List<clsLedgerVO> _OPDAdvanceRefundLedgerAccount = new List<clsLedgerVO>();
        public List<clsLedgerVO> OPDAdvanceRefundLedgerAccount
        {
            get { return _OPDAdvanceRefundLedgerAccount; }
            set
            {
                if (_OPDAdvanceRefundLedgerAccount != value)
                {
                    _OPDAdvanceRefundLedgerAccount = value;
                    OnPropertyChanged("OPDAdvanceRefundLedgerAccount");
                }
            }
        }

        private List<clsLedgerVO> _IPDSelfBillAccount = new List<clsLedgerVO>();
        public List<clsLedgerVO> IPDSelfBillAccount
        {
            get { return _IPDSelfBillAccount; }
            set
            {
                if (_IPDSelfBillAccount != value)
                {
                    _IPDSelfBillAccount = value;
                    OnPropertyChanged("IPDSelfBillAccount");
                }
            }
        }

        private List<clsLedgerVO> _IPDReceiptLedgerAccount = new List<clsLedgerVO>();
        public List<clsLedgerVO> IPDReceiptLedgerAccount
        {
            get { return _IPDReceiptLedgerAccount; }
            set
            {
                if (_IPDReceiptLedgerAccount != value)
                {
                    _IPDReceiptLedgerAccount = value;
                    OnPropertyChanged("IPDReceiptLedgerAccount");
                }
            }
        }

        private List<clsLedgerVO> _IPDCompanyBillAccount = new List<clsLedgerVO>();
        public List<clsLedgerVO> IPDCompanyBillAccount
        {
            get { return _IPDCompanyBillAccount; }
            set
            {
                if (_IPDCompanyBillAccount != value)
                {
                    _IPDCompanyBillAccount = value;
                    OnPropertyChanged("IPDCompanyBillAccount");
                }
            }
        }

        private List<clsLedgerVO> _IPDCompanyReceiptBillAccount = new List<clsLedgerVO>();
        public List<clsLedgerVO> IPDCompanyReceiptBillAccount
        {
            get { return _IPDCompanyReceiptBillAccount; }
            set
            {
                if (_IPDCompanyReceiptBillAccount != value)
                {
                    _IPDCompanyReceiptBillAccount = value;
                    OnPropertyChanged("IPDCompanyReceiptBillAccount");
                }
            }
        }

        private List<clsLedgerVO> _IPDAdvanceLedgerAccount = new List<clsLedgerVO>();
        public List<clsLedgerVO> IPDAdvanceLedgerAccount
        {
            get { return _IPDAdvanceLedgerAccount; }
            set
            {
                if (_IPDAdvanceLedgerAccount != value)
                {
                    _IPDAdvanceLedgerAccount = value;
                    OnPropertyChanged("IPDAdvanceLedgerAccount");
                }
            }
        }

        private List<clsLedgerVO> _IPDRefundBillLedgerAccount = new List<clsLedgerVO>();
        public List<clsLedgerVO> IPDRefundBillLedgerAccount
        {
            get { return _IPDRefundBillLedgerAccount; }
            set
            {
                if (_IPDRefundBillLedgerAccount != value)
                {
                    _IPDRefundBillLedgerAccount = value;
                    OnPropertyChanged("IPDRefundBillLedgerAccount");
                }
            }
        }

        private List<clsLedgerVO> _IPDAdvanceRefundLedgerAccount = new List<clsLedgerVO>();
        public List<clsLedgerVO> IPDAdvanceRefundLedgerAccount
        {
            get { return _IPDAdvanceRefundLedgerAccount; }
            set
            {
                if (_IPDAdvanceRefundLedgerAccount != value)
                {
                    _IPDAdvanceRefundLedgerAccount = value;
                    OnPropertyChanged("IPDAdvanceRefundLedgerAccount");
                }
            }
        }

        private List<clsLedgerVO> _PurchaseLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> PurchaseLedger
        {
            get { return _PurchaseLedgers; }
            set
            {
                if (_PurchaseLedgers != value)
                {
                    _PurchaseLedgers = value;
                    OnPropertyChanged("PurchaseLedger");
                }
            }
        }

        private List<clsLedgerVO> _MiscExpenseLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> MiscExpenseLedgers
        {
            get { return _MiscExpenseLedgers; }
            set
            {
                if (_MiscExpenseLedgers != value)
                {
                    _MiscExpenseLedgers = value;
                    OnPropertyChanged("MiscExpenseLedgers");
                }
            }
        }

        private List<clsLedgerVO> _MiscIncomeLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> MiscIncomeLedger
        {
            get { return _MiscIncomeLedgers; }
            set
            {
                if (_MiscIncomeLedgers != value)
                {
                    _MiscIncomeLedgers = value;
                    OnPropertyChanged("MiscIncomeLedger");
                }
            }
        }

        private List<clsLedgerVO> _PurchaseReturnLedger = new List<clsLedgerVO>();
        public List<clsLedgerVO> PurchaseReturnLedger
        {
            get { return _PurchaseReturnLedger; }
            set
            {
                if (_PurchaseReturnLedger != value)
                {
                    _PurchaseReturnLedger = value;
                    OnPropertyChanged("PurchaseReturnLedger");
                }
            }
        }

        private List<clsLedgerVO> _DoctorLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> DoctorLedgers
        {
            get { return _DoctorLedgers; }
            set
            {
                if (_DoctorLedgers != value)
                {
                    _DoctorLedgers = value;
                    OnPropertyChanged("DoctorLedgers");
                }
            }
        }

        private List<clsLedgerVO> _DoctorPaymentLedgers = new List<clsLedgerVO>();
        public List<clsLedgerVO> DoctorPaymentLedgers
        {
            get { return _DoctorPaymentLedgers; }
            set
            {
                if (_DoctorPaymentLedgers != value)
                {
                    _DoctorPaymentLedgers = value;
                    OnPropertyChanged("DoctorPaymentLedgers");
                }
            }
        }

        private List<clsLedgerVO> _IPDSelfCreditForTallyInterface; //Added By Bhushanp

        public List<clsLedgerVO> IPDSelfCreditForTallyInterface
        {
            get { return _IPDSelfCreditForTallyInterface; }
            set { _IPDSelfCreditForTallyInterface = value; }
        }

       


        #endregion 



    }

   public class clsLedgerVO: IValueObject, INotifyPropertyChanged
    {

        public string ToXml()
        {
            //throw new NotImplementedException();
            return this.ToString();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        public long ID { get; set; }
        public long AdvanceTypeId { get; set; }
        public long RowID { get; set; }
        public long OrderByID { get; set; }
        public long PayModeID { get; set; }
        public long UnitID { get; set; }
        public string Clinic { get; set; }
        public string LedgerName { get; set; }
        public double? CR { get; set; }
        public double? DR { get; set; }
        public double? BalanceAmount { get; set; }
        public string Narration { get; set; }
        public string Reference { get; set; }
        public string TempNarration { get; set; }
        public double? TempCR { get; set; }
        public double? ConcessionAmount { get; set; }
        public string VoucherType { get; set; }

        public bool IsCredit { get; set; }

        public string _Mrno;
        public string Mrno
        {
            get { return _Mrno; }
            set { _Mrno = value; }
        }

        private string _GRNNo;
        public string GRNNo
        {
            get { return _GRNNo; }
            set { _GRNNo = value; }
        }

        private string _GRNReturnNo;
        public string GRNReturnNo
        {
            get { return _GRNReturnNo; }
            set { _GRNReturnNo = value; }
        }

        private string _SupplierLedger;
        public string SupplierLedger
        {
            get { return _SupplierLedger; }
            set { _SupplierLedger = value; }
        }

        private long _SupplierLedgerID;
        public long SupplierLedgerID
        {
            get { return _SupplierLedgerID; }
            set { _SupplierLedgerID = value; }
        }

        private string _CompanyLedger;
        public string CompanyLedger
        {
            get { return _CompanyLedger; }
            set { _CompanyLedger = value; }
        }

        private long _CompanyLedgerID;
        public long CompanyLedgerID
        {
            get { return _CompanyLedgerID; }
            set { _CompanyLedgerID = value; }
        }

        private string _NewReferance;
        public string NewReferance
        {
            get { return _NewReferance; }
            set { _NewReferance = value; }
        }

        private double _SettledBillAmount;
        public double SettledBillAmount
        {
            get { return _SettledBillAmount; }
            set { _SettledBillAmount = value; }
        }

        private DateTime? _TransactionDate;
        public DateTime? TransactionDate
        {
            get { return _TransactionDate; }
            set { _TransactionDate = value; }
        }

        private long _VoucherId;
        public long VoucherId
        {
            get { return _VoucherId; }
            set { _VoucherId = value; }
        }

        private long _VoucherUnitId;
        public long VoucherUnitId
        {
            get { return _VoucherUnitId; }
            set { _VoucherUnitId = value; }
        }

        private long _VoucherDetailId;
        public long VoucherDetailId
        {
            get { return _VoucherDetailId; }
            set { _VoucherDetailId = value; }
        }

        private long _VoucherDetailUnitId;
        public long VoucherDetailUnitId
        {
            get { return _VoucherDetailUnitId; }
            set { _VoucherDetailUnitId = value; }
        }

        private string _Particular;
        public string Particular
        {
            get { return _Particular; }
            set { _Particular = value; }
        }

        private long _SubLedgerId;
        public long SubLedgerId
        {
            get { return _SubLedgerId; }
            set { _SubLedgerId = value; }
        }

        private DateTime? _EntryDate;
        public DateTime? EntryDate
        {
            get { return _EntryDate; }
            set { _EntryDate = value; }
        }

        private string _TransactionType;
        public string TransactionType
        {
            get { return _TransactionType; }
            set { _TransactionType = value; }
        }

        private string _GPVoucherType;
        public string GPVoucherType
        {
            get { return _GPVoucherType; }
            set { _GPVoucherType = value; }
        }

        private string _PatientType;
        public string PatientType
        {
            get { return _PatientType; }
            set { _PatientType = value; }
        }

        private string _Sponsor;
        public string Sponsor
        {
            get { return _Sponsor; }
            set { _Sponsor = value; }
        }

        private string _VoucherMode;
        public string VoucherMode
        {
            get { return _VoucherMode; }
            set { _VoucherMode = value; }
        }

        private string _PatientNo;
        public string PatientNo
        {
            get { return _PatientNo; }
            set { _PatientNo = value; }
        }

        private long _PatientUnitId;
        public long PatientUnitId
        {
            get { return _PatientUnitId; }
            set { _PatientUnitId = value; }
        }

        private string _Pat_Comp_Name;
        public string Pat_Comp_Name
        {
            get { return _Pat_Comp_Name; }
            set { _Pat_Comp_Name = value; }
        }

        private string _TransactionGroup;
        public string TransactionGroup
        {
            get { return _TransactionGroup; }
            set { _TransactionGroup = value; }
        }

        private long _TransactionId;
        public long TransactionId
        {
            get { return _TransactionId; }
            set { _TransactionId = value; }
        }

        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }

        private DateTime? _PostingDate;
        public DateTime? PostingDate
        {
            get { return _PostingDate; }
            set { _PostingDate = value; }
        }

        private double? _VatAmount;
        public double? VatAmount
        {
            get { return _VatAmount; }
            set { _VatAmount = value; }
        }

        private double? _TotalAmount;
        public double? TotalAmount
        {
            get { return _TotalAmount; }
            set { _TotalAmount = value; }
        }

        private double? _NetAmount;
        public double? NetAmount
        {
            get { return _NetAmount; }
            set { _NetAmount = value; }
        }

        private long _VendorId;
        public long VendorId
        {
            get { return _VendorId; }
            set { _VendorId = value; }
        }

        private string _PurchaseInvoiceNo;
        public string PurchaseInvoiceNo
        {
            get { return _PurchaseInvoiceNo; }
            set { _PurchaseInvoiceNo = value; }
        }

        private string _TransactionNo;
        public string TransactionNo
        {
            get { return _TransactionNo; }
            set { _TransactionNo = value; }
        }
        

   }

   public class clsGetTotalBillAccountsLedgersVO : IBizActionValueObject
   {
       #region IBizActionValueObject Members

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Billing.clsGetTotalBillAccountsLedgers";
       }

       #endregion

       #region IValueObject Members

       public string ToXml()
       {
           return this.ToString();
       }

       #endregion
       private bool _IsGenrateXML = false;
       /// <summary>
       /// Output Property.
       /// This property states the outcome of BizAction Process.
       /// </summary>
       public bool IsGenrateXML
       {
           get { return _IsGenrateXML; }
           set { _IsGenrateXML = value; }
       }

       private bool _IsDeleteFile = false;
       /// <summary>
       /// Output Property.
       /// This property states the outcome of BizAction Process.
       /// </summary>
       public bool IsDeleteFiles
       {
           get { return _IsDeleteFile; }
           set { _IsDeleteFile = value; }
       }

       private string _strGenrateXML;
       /// <summary>
       /// Output Property.
       /// This property states the outcome of BizAction Process.
       /// </summary>
       public string strGenrateXMLName
       {
           get { return _strGenrateXML; }
           set { _strGenrateXML = value; }
       }
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
       public List<ENVELOPE> ENVELOPEList { get; set; }
       //public long UnitID { get; set; }
       //public DateTime ExportDate { get; set; }

       private clsAccountsVO obj = null;
       /// <summary>
       /// Output Property.
       /// This Property Contains OPDPatient Details Which is Added.
       /// </summary>
       public clsAccountsVO Details
       {
           get { return obj; }
           set { obj = value; }
       }
   }

   public class clsAddVoucherHeaderBizActionVO : IBizActionValueObject
   {
       #region IBizActionValueObject Members

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Billing.clsAddVoucherHeaderBizAction";
       }

       #endregion

       private bool _IsGenrateXML = false;
       /// <summary>
       /// Output Property.
       /// This property states the outcome of BizAction Process.
       /// </summary>
       public bool IsGenrateXML
       {
           get { return _IsGenrateXML; }
           set { _IsGenrateXML = value; }
       }

       //List<ENVELOPE>

       public List<ENVELOPE> ENVELOPEList { get; set; }

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

       //public long UnitID { get; set; }
       //public DateTime ExportDate { get; set; }

       private clsAccountsVO obj = null;
       /// <summary>
       /// Output Property.
       /// This Property Contains OPDPatient Details Which is Added.
       /// </summary>
       public clsAccountsVO Details
       {
           get { return obj; }
           set { obj = value; }
       }
   }
}
