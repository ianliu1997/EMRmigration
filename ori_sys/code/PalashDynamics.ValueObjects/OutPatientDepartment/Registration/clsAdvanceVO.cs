using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Registration
{
    public class clsAdvanceVO : IValueObject, INotifyPropertyChanged
    {
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

        public enum AdvanceType
        {
            Select=0,
            Company =1,
            Patient = 2,          
            PatientCompany = 3

        }
           
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

        private string _AdvanceNO;
        public string AdvanceNO
        {
            get { return _AdvanceNO; }
            set
            {
                if (_AdvanceNO != value)
                {
                    _AdvanceNO = value;
                    OnPropertyChanged("AdvanceNO");
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

        private string _UnitName;
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                if (_UnitName != value)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
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
        
        private double _Total;
        public double Total
        {
            get { return _Total; }
            set
            {
                if (_Total != value)
                {
                    _Total = value;
                    OnPropertyChanged("Total");
                }
            }
        }

        private long _AdvanceTypeId;
        public long AdvanceTypeId
        {
            get { return _AdvanceTypeId; }
            set
            {
                if (_AdvanceTypeId != value)
                {
                    _AdvanceTypeId = value;
                    OnPropertyChanged("AdvanceTypeId");
                }
            }
        }

        private long _AdvanceAgainstId;
        public long AdvanceAgainstId
        {
            get { return _AdvanceAgainstId; }
            set
            {
                if (_AdvanceAgainstId != value)
                {
                    _AdvanceAgainstId = value;
                    OnPropertyChanged("AdvanceAgainstId");
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

        private double _Used;
        public double Used
        {
            get { return _Used; }
            set
            {
                if (value < 0)
                {
                    _Used = 0;
                    OnPropertyChanged("Used");
                }
                else if (_Used != value )
                {
                    _Used = value;
                    OnPropertyChanged("Used");
                }
            }
        }
        
        private double _Refund;
        public double Refund
        {
            get { return _Refund; }
            set
            {
                if (_Refund != value)
                {
                    _Refund = value;
                    OnPropertyChanged("Refund");
                }
            }
        }
        
        private double _Balance;
        public double Balance
        {
            get { return _Balance; }
            set
            {
                if (_Balance != value)
                {
                    _Balance = value;
                    OnPropertyChanged("Balance");
                }
            }
        }

        private bool _Status=true;
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
                    OnPropertyChanged(AddedWindowsLoginName);
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
                    OnPropertyChanged(UpdateWindowsLoginName);
                }
            }
        }

        public string Company { get; set; }
        public string AdvanceAgainst { get; set; }
                
        public string AdvanceTypeName
        {
            get { return ((AdvanceType)_AdvanceTypeId).ToString(); }
           
        }
        //Adde By Bhushanp 02062017
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


        private long _ApprovedRequestID;
        public long ApprovedRequestID
        {
            get { return _ApprovedRequestID; }
            set
            {
                if (_ApprovedRequestID != value)
                {
                    _ApprovedRequestID = value;
                    OnPropertyChanged("ApprovedRequestID");
                }
            }
        }
        private long _ApprovedRequestUnitID;
        public long ApprovedRequestUnitID
        {
            get { return _ApprovedRequestUnitID; }
            set
            {
                if (_ApprovedRequestUnitID != value)
                {
                    _ApprovedRequestUnitID = value;
                    OnPropertyChanged("ApprovedRequestUnitID");
                }
            }
        }

        private long _ApprovalRequestDetailsID;
        public long ApprovalRequestDetailsID
        {
            get { return _ApprovalRequestDetailsID; }
            set
            {
                if (_ApprovalRequestDetailsID != value)
                {
                    _ApprovalRequestDetailsID = value;
                    OnPropertyChanged("ApprovalRequestDetailsID");
                }
            }
        }

        private long _ApprovalRequestDetailsUnitID;
        public long ApprovalRequestDetailsUnitID
        {
            get { return _ApprovalRequestDetailsUnitID; }
            set
            {
                if (_ApprovalRequestDetailsUnitID != value)
                {
                    _ApprovalRequestDetailsUnitID = value;
                    OnPropertyChanged("ApprovalRequestDetailsUnitID");
                }
            }
        }

        //Added By Bhushanp For New Package Flow 18082017        
        private long _PackageID;
        public long PackageID
        {
            get { return _PackageID; }
            set
            {
                if (_PackageID != value)
                {
                    _PackageID = value;
                    OnPropertyChanged("PackageID");
                }
            }
        }

        private string _PackageName;
        public string PackageName
        {
            get { return _PackageName; }
            set
            {
                if (_PackageName != value)
                {
                    _PackageName = value;
                    OnPropertyChanged("PackageName");
                }
            }
        }

        private bool _IsPackageBillFreeze = false;
        public bool IsPackageBillFreeze
        {
            get
            {
                return _IsPackageBillFreeze;
            }
            set
            {
                _IsPackageBillFreeze = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsPackageBillFreeze"));
                }
            }
        }

        private bool _IsAutoCharge = false;
        public bool IsAutoCharge
        {
            get
            {
                return _IsAutoCharge;
            }
            set
            {
                _IsAutoCharge = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsAutoCharge"));
                }
            }
        }

        private string _ApprovalRemark;
        public string ApprovalRemark
        {
            get { return _ApprovalRemark; }
            set
            {
                if (_ApprovalRemark != value)
                {
                    _ApprovalRemark = value;
                    OnPropertyChanged("ApprovalRemark");
                }
            }
        }
        private bool _ApprovalStatus;
        public bool ApprovalStatus
        {
            get { return _ApprovalStatus; }
            set
            {
                if (_ApprovalStatus != value)
                {
                    _ApprovalStatus = value;
                    OnPropertyChanged("ApprovalStatus");
                }
            }
        }

        private bool _IsSendForApproval;
        public bool IsSendForApproval
        {
            get { return _IsSendForApproval; }
            set
            {
                if (_IsSendForApproval != value)
                {
                    _IsSendForApproval = value;
                    OnPropertyChanged("IsSendForApproval");
                }
            }
        }


        private long _LevelID;
        public long LevelID
        {
            get { return _LevelID; }
            set
            {
                if (_LevelID != value)
                {
                    _LevelID = value;
                    OnPropertyChanged("LevelID");
                }
            }
        }

        public bool SelectCharge { get; set; }


        private string _ApprovalRequestRemark;
        public string ApprovalRequestRemark
        {
            get { return _ApprovalRequestRemark; }
            set
            {
                if (_ApprovalRequestRemark != value)
                {
                    _ApprovalRequestRemark = value;
                    OnPropertyChanged("ApprovalRequestRemark");
                }
            }
        }

        private bool _IsRefund;

        public bool IsRefund
        {
            get { return _IsRefund; }
            set
            {
                if (_IsRefund != value)
                {
                    _IsRefund = value;
                    OnPropertyChanged("IsRefund");
                }
            }
        }

        private string _MRNO;

        public string MRNO
        {
            get { return _MRNO; }
            set { _MRNO = value; }
        }

        private string _ContactNo;

        public string ContactNo
        {
            get { return _ContactNo; }
            set { _ContactNo = value; }
        }

        private string _RequestType;

        public string RequestType
        {
            get { return _RequestType; }
            set { _RequestType = value; }
        }

        private long _RequestTypeID;

        public long RequestTypeID
        {
            get { return _RequestTypeID; }
            set { _RequestTypeID = value; }
        }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        #region  21042017 Refund To Advance

        private long _FromRefundID;
        public long FromRefundID
        {
            get { return _FromRefundID; }
            set
            {
                if (_FromRefundID != value)
                {
                    _FromRefundID = value;
                    OnPropertyChanged("FromRefundID");
                }
            }
        }

        #endregion

        public long PackageBillID { get; set; }
        public long PackageBillUnitID { get; set; }
        public bool IsForTotalAdvance { get; set; }   // this flag will set true from only frmBill.xaml GetAdvance.

        private double _PackageBillAmount;  // Added on 13062018 for Package New Changes
        public double PackageBillAmount
        {
            get { return _PackageBillAmount; }
            set
            {
                if (_PackageBillAmount != value)
                {
                    _PackageBillAmount = value;
                    OnPropertyChanged("PackageBillAmount");
                }
            }
        }

        private double _PackageAdvanceBalance;  // Added on 13062018 for Package New Changes
        public double PackageAdvanceBalance
        {
            get { return _PackageAdvanceBalance; }
            set
            {
                if (_PackageAdvanceBalance != value)
                {
                    _PackageAdvanceBalance = value;
                    OnPropertyChanged("PackageAdvanceBalance");
                }
            }
        }

    }
}
