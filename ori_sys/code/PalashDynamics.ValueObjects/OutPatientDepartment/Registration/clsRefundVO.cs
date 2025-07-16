using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Registration
{
   public class clsRefundVO : IValueObject, INotifyPropertyChanged
    {
        #region Property Declaration Section

        private string _LinkServer;
        public string LinkServer
        {
            get { return _LinkServer; }
            set
            {
                if (_LinkServer != value)
                {
                    _LinkServer = value;
                    OnPropertyChanged("LinkServer");
                }
            }
        }

        private clsPaymentVO _PaymentDetails = new clsPaymentVO();
        public clsPaymentVO PaymentDetails
        {
            get { return _PaymentDetails; }
            set
            {

                _PaymentDetails = value;
                OnPropertyChanged("PaymentDetails");

            }
        }

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
        private long _ApprovalRequestID;
        public long ApprovalRequestID
        {
            get { return _ApprovalRequestID; }
            set
            {
                if (_ApprovalRequestID != value)
                {
                    _ApprovalRequestID = value;
                    OnPropertyChanged("ApprovalRequestID");
                }
            }
        }
        private long _ApprovalRequestUnitID;
        public long ApprovalRequestUnitID
        {
            get { return _ApprovalRequestUnitID; }
            set
            {
                if (_ApprovalRequestUnitID != value)
                {
                    _ApprovalRequestUnitID = value;
                    OnPropertyChanged("ApprovalRequestUnitID");
                }
            }
        }
        private long _RequestTypeID;
        public long RequestTypeID
        {
            get { return _RequestTypeID; }
            set
            {
                if (_RequestTypeID != value)
                {
                    _RequestTypeID = value;
                    OnPropertyChanged("RequestTypeID");
                }
            }
        }
        private string _RequestType;
        public string RequestType
        {
            get { return _RequestType; }
            set
            {
                if (_RequestType != value)
                {
                    _RequestType = value;
                    OnPropertyChanged("RequestType");
                }
            }
        }
        private bool _IsOPDBill;
        public bool IsOPDBill
        {
            get { return _IsOPDBill; }
            set
            {
                if (_IsOPDBill != value)
                {
                    _IsOPDBill = value;
                    OnPropertyChanged("IsOPDBill");
                }
            }
        }
        private long _BillUnitID;
        public long BillUnitID
        {
            get { return _BillUnitID; }
            set
            {
                if (_BillUnitID != value)
                {
                    _BillUnitID = value;
                    OnPropertyChanged("BillUnitID");
                }
            }
        }
        private long _ItemSaleReturnID;
        public long ItemSaleReturnID
        {
            get { return _ItemSaleReturnID; }
            set
            {
                if (_ItemSaleReturnID != value)
                {
                    _ItemSaleReturnID = value;
                    OnPropertyChanged("ItemSaleReturnID");
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

        private string _AdvanceNo;
        public string AdvanceNo
        {
            get { return _AdvanceNo; }
            set
            {
                if (_AdvanceNo != value)
                {
                    _AdvanceNo = value;
                    OnPropertyChanged("AdvanceNo");
                }
            }
        }

        private double _Amount;
        public double Amount
        {
            get { return _Amount; }
            set
            {
                if (_Amount != value)
                {
                    _Amount = value;
                    OnPropertyChanged("Amount");
                }
            }
        }
       
        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }
            }
        }


        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }

        private long _CompanyID;
        public long CompanyID
        {
            get { return _CompanyID; }
            set
            {
                if (_CompanyID != value)
                {
                    _CompanyID = value;
                    OnPropertyChanged("CompanyID");
                }
            }
        }


        private string _Remarks="";
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

        private string _ReceiptNo="";
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

        private bool _ReceiptPrinted;
        public bool ReceiptPrinted
        {
            get { return _ReceiptPrinted; }
            set
            {
                if (_ReceiptPrinted != value)
                {
                    _ReceiptPrinted = value;
                    OnPropertyChanged("ReceiptPrinted");
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

        private string _AddedOn="";
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
       
        private long? _UpdatedBy;
        public long? UpdatedBy
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

        private string _UpdatedOn="";
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
        private string _UnitName;
        public string UnitName
        {
            get
            {
                return _UnitName;
            }

            set
            {
                if (value != _UnitName)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
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

        private string _AddedWindowsLoginName="";
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

        private string _UpdateWindowsLoginName="";
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

        private long _AdvanceUnitID;

        public long AdvanceUnitID
        {
            get { return _AdvanceUnitID; }
            set
            {
                if (_AdvanceUnitID != value)
                {
                    _AdvanceUnitID = value;
                    OnPropertyChanged("AdvanceUnitID");
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
}
