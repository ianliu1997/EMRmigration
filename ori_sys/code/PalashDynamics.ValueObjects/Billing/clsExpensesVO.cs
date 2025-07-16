using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Radiology;

namespace PalashDynamics.ValueObjects.Billing
{
    public class clsExpensesVO : IValueObject, INotifyPropertyChanged
    {
        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region Property Declaration Section

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

        public string UnitName { get; set; }
        public string ExpenseName { get; set; }

        private List<clsExpensesDetailsVO> _ExpenseDetails = new List<clsExpensesDetailsVO>();
        public List<clsExpensesDetailsVO> ExpenseDetails
        {
            get { return _ExpenseDetails; }
            set
            {

                _ExpenseDetails = value;


            }
        }

        //private clsExpensesVO _ExpenseDetails = new clsExpensesVO();
        //public clsExpensesVO ExpenseDetails
        //{
        //    get { return _ExpenseDetails; }
        //    set
        //    {
        //        _ExpenseDetails = value;
        //        OnPropertyChanged("ExpenseDetails");
        //    }
        //}

        private long _Expense;
        public long Expense
        {
            get { return _Expense; }
            set
            {
                if (_Expense != value)
                {
                    _Expense = value;
                    OnPropertyChanged("Expense");
                }
            }
        }

        private DateTime? _ExpenseDate;
        public DateTime? ExpenseDate
        {
            get { return _ExpenseDate; }
            set
            {
                if (_ExpenseDate != value)
                {
                    _ExpenseDate = value;
                    OnPropertyChanged("ExpenseDate");
                }
            }
        }

        private string _VoucherNo;
        public string VoucherNo
        {
            get { return _VoucherNo; }
            set
            {
                if (_VoucherNo != value)
                {
                    _VoucherNo = value;
                    OnPropertyChanged("VoucherNo");
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

        private double _BalanceAmountSelf;
        public double BalanceAmountSelf
        {
            get { return _BalanceAmountSelf; }
            set
            {
                if (_BalanceAmountSelf != value)
                {
                    _BalanceAmountSelf = value;
                    OnPropertyChanged("BalanceAmountSelf");
                }
            }
        }

        public double PaidAmountSelf
        {
            get
            {
                if (_IsFreezed)
                    return _Amount - _BalanceAmountSelf;
                else
                    return 0;

            }

        }
        private bool _IsSettled;
        public bool IsSettled
        {
            get { return _IsSettled; }
            set
            {
                if (_IsSettled != value)
                {
                    _IsSettled = value;
                    OnPropertyChanged("IsSettled");
                }
            }
        }

        private string _voucherCreatedby;
        public string voucherCreatedby
        {
            get { return _voucherCreatedby; }
            set
            {
                if (_voucherCreatedby != value)
                {
                    _voucherCreatedby = value;
                    //OnPropertyChanged("voucherCreatedby");
                }
            }
        }


        private bool _IsFreezed;
        public bool IsFreezed
        {
            get { return _IsFreezed; }
            set
            {
                if (_IsFreezed != value)
                {
                    _IsFreezed = value;
                    OnPropertyChanged("IsFreezed");
                }
            }
        }
        public bool VoStatus { get; set; }
        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set
            {
                if (_Remark != value)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }

        private string _Reasonofcancellation;
        public string Reasonofcancellation
        {
            get { return _Reasonofcancellation; }
            set
            {
                if (_Reasonofcancellation != value)
                {
                    _Reasonofcancellation = value;
                    OnPropertyChanged("Reasonofcancellation");
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

        private string _AddedOn;
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedOn;
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

        private string _UpdatedWindowsLoginName;
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }

        #endregion
    }

    public class clsExpensesDetailsVO : IValueObject, INotifyPropertyChanged
    {
        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region Property Declaration Section
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

        private long _ExpenseUnitID;
        public long ExpenseUnitID
        {
            get { return _ExpenseUnitID; }
            set
            {
                if (_ExpenseUnitID != value)
                {
                    _ExpenseUnitID = value;
                    OnPropertyChanged("ExpenseUnitID");
                }
            }
        }



        private long _ExpenseID;
        public long ExpenseID
        {
            get { return _ExpenseID; }
            set
            {
                if (_ExpenseID != value)
                {
                    _ExpenseID = value;
                    OnPropertyChanged("ExpenseID");
                }
            }
        }



        private long _PaymentMode;
        public long PaymentMode
        {
            get { return _PaymentMode; }
            set
            {
                if (_PaymentMode != value)
                {
                    _PaymentMode = value;
                    OnPropertyChanged("PaymentMode");
                }
            }
        }



        private string _ChequeNo = "";
        public string ChequeNo
        {
            get { return _ChequeNo; }
            set
            {
                if (_ChequeNo != value)
                {
                    _ChequeNo = value;
                    OnPropertyChanged("ChequeNo");
                }
            }
        }
        private DateTime? _ChequeDate;
        public DateTime? ChequeDate
        {
            get { return _ChequeDate; }
            set
            {
                if (_ChequeDate != value)
                {
                    _ChequeDate = value;
                    OnPropertyChanged("ChequeDate");
                }
            }
        }

        private string _Payto;
        public string Payto
        {
            get { return _Payto; }
            set
            {
                if (_Payto != value)
                {
                    _Payto = value;
                    OnPropertyChanged("Payto");
                }
            }
        }
        private string _Remarks;
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

        private string _ReasonOfCancellation;
        public string ReasonOfCancellation
        {
            get { return _ReasonOfCancellation; }
            set
            {
                if (_ReasonOfCancellation != value)
                {
                    _ReasonOfCancellation = value;
                    OnPropertyChanged("ReasonOfCancellation");
                }
            }
        }

        private double _PaymentAmount;
        public double PaymentAmount
        {
            get { return _PaymentAmount; }
            set
            {
                if (_PaymentAmount != value)
                {
                    _PaymentAmount = value;
                    OnPropertyChanged("PaymentAmount");
                }
            }
        }


        private long _TransferToBank;
        public long TransferToBank
        {
            get { return _TransferToBank; }
            set
            {
                if (_TransferToBank != value)
                {
                    _TransferToBank = value;
                    OnPropertyChanged("TransferToBank");
                }
            }
        }

        private long _TransferToAccountNo;
        public long TransferToAccountNo
        {
            get { return _TransferToAccountNo; }
            set
            {
                if (_TransferToAccountNo != value)
                {
                    _TransferToAccountNo = value;
                    OnPropertyChanged("TransferToAccountNo");
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

        private string _AddedOn;
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedOn;
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

        private string _UpdatedWindowsLoginName;
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }
        #endregion

    }
}
