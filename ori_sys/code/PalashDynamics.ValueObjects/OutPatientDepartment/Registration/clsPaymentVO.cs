using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Log;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Registration
{
    public class clsPaymentVO : IValueObject, INotifyPropertyChanged
    {

        public string LinkServer { get; set; }
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

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

        private List<clsPaymentDetailsVO> _PaymentDetails = new List<clsPaymentDetailsVO>();
        public List<clsPaymentDetailsVO> PaymentDetails
        {
            get { return _PaymentDetails; }
            set
            {

                _PaymentDetails = value;


            }
        }

        // For the Activity Log List
        private List<LogInfo> _LogInfoList = null;
        public List<LogInfo> LogInfoList
        {
            get { return _LogInfoList; }
            set { _LogInfoList = value; }
        }

        public BillPaymentTypes BillPaymentType { get; set; }

        private DateTime? _Date;
        public DateTime? Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    OnPropertyChanged("Date");
                }
            }
        }


        //Added by PMG

        private long _InvoiceID;
        public long InvoiceID
        {
            get { return _InvoiceID; }
            set
            {
                if (_InvoiceID != value)
                {
                    _InvoiceID = value;
                    OnPropertyChanged("_InvoiceID");
                }
            }
        }
        private long _InvoiceUnitID;
        public long InvoiceUnitID
        {
            get { return _InvoiceUnitID; }
            set
            {
                if (_InvoiceUnitID != value)
                {
                    _InvoiceUnitID = value;
                    OnPropertyChanged("InvoiceUnitID");
                }
            }
        }

        private string _ReceiptNo = "";
        public string ReceiptNo
        {
            get { return _ReceiptNo; }
            set
            {
                if (_ReceiptNo != value)
                {
                    _ReceiptNo = value;
                    OnPropertyChanged("ReceiptNo");
                }
            }
        }

        private bool _IsBillSettlement = false;
        public bool IsBillSettlement
        {
            get { return _IsBillSettlement; }
            set
            {
                if (_IsBillSettlement != value)
                {
                    _IsBillSettlement = value;
                    OnPropertyChanged("IsBillSettlement");
                }
            }
        }


        private long _BillID;
        public long BillID
        {
            get { return _BillID; }
            set
            {
                if (_BillID != value)
                {
                    _BillID = value;
                    OnPropertyChanged("BillID");
                }
            }
        }

        private double _BillAmount;
        public double BillAmount
        {
            get { return _BillAmount; }
            set
            {
                if (_BillAmount != value)
                {
                    _BillAmount = value;
                    OnPropertyChanged("BillAmount");
                }
            }
        }

        private double _PaidAmount;
        public double PaidAmount
        {
            get { return _PaidAmount; }
            set
            {
                if (_PaidAmount != value)
                {
                    _PaidAmount = value;
                    OnPropertyChanged("PaidAmount");
                }
            }
        }

        public double TDSAmt { get; set; }

        private double _BillBalanceAmount;
        public double BillBalanceAmount
        {
            get { return _BillBalanceAmount; }
            set
            {
                if (_BillBalanceAmount != value)
                {
                    _BillBalanceAmount = value;
                    OnPropertyChanged("BillBalanceAmount");
                }
            }
        }

        //Added by PMG
        private double _TempPaidAmount;
        public double TempPaidAmount
        {
            get { return _TempPaidAmount; }
            set
            {
                if (_TempPaidAmount != value)
                {
                    _TempPaidAmount = value;
                    OnPropertyChanged("TempPaidAmount");
                }
            }
        }


        private long _AdvanceID;
        public long AdvanceID
        {
            get { return _AdvanceID; }
            set
            {
                if (_AdvanceID != value)
                {
                    _AdvanceID = value;
                    OnPropertyChanged("AdvanceID");
                }
            }
        }

        private double _AdvanceAmount;
        public double AdvanceAmount
        {
            get { return _AdvanceAmount; }
            set
            {
                if (_AdvanceAmount != value)
                {
                    _AdvanceAmount = value;
                    OnPropertyChanged("AdvanceAmount");
                }
            }
        }

        private double _AdvanceUsed;
        public double AdvanceUsed
        {
            get { return _AdvanceUsed; }
            set
            {
                if (_AdvanceUsed != value)
                {
                    _AdvanceUsed = value;
                    OnPropertyChanged("AdvanceUsed");
                }
            }
        }

        private long _RefundID;
        public long RefundID
        {
            get { return _RefundID; }
            set
            {
                if (_RefundID != value)
                {
                    _RefundID = value;
                    OnPropertyChanged("RefundID");
                }
            }
        }

        private double _RefundAmount;
        public double RefundAmount
        {
            get { return _RefundAmount; }
            set
            {
                if (_RefundAmount != value)
                {
                    _RefundAmount = value;
                    OnPropertyChanged("RefundAmount");
                }
            }
        }



        private string _Remarks = "";
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                if (_Remarks != value)
                {
                    _Remarks = value;
                    OnPropertyChanged("Remarks");
                }
            }
        }

        private long _ChequeAuthorizedBy;
        public long ChequeAuthorizedBy
        {
            get { return _ChequeAuthorizedBy; }
            set
            {
                if (_ChequeAuthorizedBy != value)
                {
                    _ChequeAuthorizedBy = value;
                    OnPropertyChanged("ChequeAuthorizedBy");
                }
            }
        }

        private long _ComAdvAuthorizedBy;
        public long ComAdvAuthorizedBy
        {
            get { return _ComAdvAuthorizedBy; }
            set
            {
                if (_ComAdvAuthorizedBy != value)
                {
                    _ComAdvAuthorizedBy = value;
                    OnPropertyChanged("ComAdvAuthorizedBy");
                }
            }
        }
        private long _CreditAuthorizedBy;
        public long CreditAuthorizedBy
        {
            get { return _CreditAuthorizedBy; }
            set
            {
                if (_CreditAuthorizedBy != value)
                {
                    _CreditAuthorizedBy = value;
                    OnPropertyChanged("CreditAuthorizedBy");
                }
            }
        }

        private string _PayeeNarration = "";
        public string PayeeNarration
        {
            get { return _PayeeNarration; }
            set
            {
                if (_PayeeNarration != value)
                {
                    _PayeeNarration = value;
                    OnPropertyChanged("PayeeNarration");
                }
            }
        }

        private bool _IsPrinted;
        public bool IsPrinted
        {
            get { return _IsPrinted; }
            set
            {
                if (_IsPrinted != value)
                {
                    _IsPrinted = value;
                    OnPropertyChanged("IsPrinted");
                }
            }
        }

        private bool _IsCancelled;
        public bool IsCancelled
        {
            get { return _IsCancelled; }
            set
            {
                if (_IsCancelled != value)
                {
                    _IsCancelled = value;
                    OnPropertyChanged("IsCancelled");
                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        # region Costing Divisions for Clinical & Pharmacy Billing

        private long _CostingDivisionID;
        public long CostingDivisionID
        {
            get
            {
                return _CostingDivisionID;
            }

            set
            {
                if (value != _CostingDivisionID)
                {
                    _CostingDivisionID = value;
                    OnPropertyChanged("CostingDivisionID");
                }
            }
        }

        # endregion

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long _UpdatedUnitID;
        public long UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
                }
            }
        }

        private long _AddedBy;
        public long AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private long _UpdatedBy;
        public long UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private string _UpdateWindowsLoginName = "";
        public string UpdateWindowsLoginName
        {
            get { return _UpdateWindowsLoginName; }
            set
            {
                if (_UpdateWindowsLoginName != value)
                {
                    _UpdateWindowsLoginName = value;
                    OnPropertyChanged("UpdateWindowsLoginName");
                }
            }
        }

        #region For IPD Module

        private double _SettlementConcessionAmount;
        public double SettlementConcessionAmount
        {
            get
            {
                return _SettlementConcessionAmount;

            }
            set
            {
                if (_SettlementConcessionAmount != value)
                {
                    _SettlementConcessionAmount = value;
                    OnPropertyChanged("SettlementConcessionAmount");
                }
            }
        }

        #endregion


        #region INotifyPropertyChanged Members
        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }


    public class clsPaymentDetailsVO : IValueObject, INotifyPropertyChanged
    {

        public string LinkServer { get; set; }
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

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

        private long _PaymentID;
        public long PaymentID
        {
            get { return _PaymentID; }
            set
            {
                if (_PaymentID != value)
                {
                    _PaymentID = value;
                    OnPropertyChanged("PaymentID");
                }
            }
        }

        private long _PaymentModeID;
        public long PaymentModeID
        {
            get { return _PaymentModeID; }
            set
            {
                if (_PaymentModeID != value)
                {
                    _PaymentModeID = value;
                    OnPropertyChanged("PaymentModeID");
                }
            }
        }


        private string _Number = "";
        public string Number
        {
            get { return _Number; }
            set
            {
                if (_Number != value)
                {
                    _Number = value;
                    OnPropertyChanged("Number");
                }
            }
        }

        private DateTime? _Date;
        public DateTime? Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        private long? _BankID;
        public long? BankID
        {
            get { return _BankID; }
            set
            {
                if (_BankID != value)
                {
                    _BankID = value;
                    OnPropertyChanged("BankID");
                }
            }
        }

        private long? _AdvanceID;
        public long? AdvanceID
        {
            get { return _AdvanceID; }
            set
            {
                if (_AdvanceID != value)
                {
                    _AdvanceID = value;
                    OnPropertyChanged("AdvanceID");
                }
            }
        }

        private double _PaidAmount;
        public double PaidAmount
        {
            get { return _PaidAmount; }
            set
            {
                if (_PaidAmount != value)
                {
                    _PaidAmount = value;
                    OnPropertyChanged("PaidAmount");
                }
            }
        }


        private bool _ChequeCleared;
        public bool ChequeCleared
        {
            get { return _ChequeCleared; }
            set
            {
                if (_ChequeCleared != value)
                {
                    _ChequeCleared = value;
                    OnPropertyChanged("ChequeCleared");
                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long _UpdatedUnitID;
        public long UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
                }
            }
        }

        private long _AddedBy;
        public long AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private long _UpdatedBy;
        public long UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private string _UpdateWindowsLoginName = "";
        public string UpdateWindowsLoginName
        {
            get { return _UpdateWindowsLoginName; }
            set
            {
                if (_UpdateWindowsLoginName != value)
                {
                    _UpdateWindowsLoginName = value;
                    OnPropertyChanged("UpdateWindowsLoginName");
                }
            }
        }


        #region INotifyPropertyChanged Members
        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
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