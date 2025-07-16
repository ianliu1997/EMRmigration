using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsIndentMasterVO : INotifyPropertyChanged, IValueObject
    {
        #region IValueObject Members

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region INotifyPropertyChanged


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

        public override string ToString()
        {
            return IndentNumber + " ( " + Date.Value.ToString("dd/MMM/yyyy") + " )";
        }

        #region Properties


        private bool _isPurchaseRequisitionClosed = false;
        public bool isPurchaseRequisitionClosed
        {
            get
            {
                return _isPurchaseRequisitionClosed;
            }
            set
            {
                if (_isPurchaseRequisitionClosed != value)
                {
                    _isPurchaseRequisitionClosed = value;
                    OnPropertyChanged("isPurchaseRequisitionClosed");
                }
            }
        }





        private bool _isIndentClosed = false;
        public bool isIndentClosed
        {
            get
            {
                return _isIndentClosed;
            }
            set
            {
                if (_isIndentClosed != value)
                {
                    _isIndentClosed = value;
                    OnPropertyChanged("isIndentClosed");
                }
            }
        }
        private bool _IndentDashBordStatus;
        public bool IndentDashBordStatus
        {
            get
            {
                return _IndentDashBordStatus;
            }
            set
            {
                if (_IndentDashBordStatus != value)
                {
                    _IndentDashBordStatus = value;
                    OnPropertyChanged("IndentDashBordStatus");
                }
            }
        }

        private long? _ID;
        public long? ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }
        private long? _UnitID;
        public long? UnitID
        {
            get
            {
                return _UnitID;
            }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }
        private DateTime? _Date;
        public DateTime? Date
        {
            get
            {
                return _Date;
            }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    OnPropertyChanged("Date");
                }
            }
        }
        private DateTime? _Time;
        public DateTime? Time
        {
            get
            {
                return _Time;
            }
            set
            {
                if (_Time != value)
                {
                    _Time = value;
                    OnPropertyChanged("Time");
                }
            }
        }
        private String _IndentNumber;
        public String IndentNumber
        {
            get
            {
                return _IndentNumber;
            }
            set
            {
                if (_IndentNumber != value)
                {
                    _IndentNumber = value;
                    OnPropertyChanged("IndentNumber");
                }
            }
        }
        private int? _TransactionMovementID;
        public int? TransactionMovementID
        {
            get
            {
                return _TransactionMovementID;
            }
            set
            {
                if (_TransactionMovementID != value)
                {
                    _TransactionMovementID = value;
                    OnPropertyChanged("TransactionMovementID");
                }
            }
        }
        private long? _FromStoreID;
        public long? FromStoreID
        {
            get
            {
                return _FromStoreID;
            }
            set
            {
                if (_FromStoreID != value)
                {
                    _FromStoreID = value;
                    OnPropertyChanged("FromStoreID");
                }
            }
        }
        private String _FromStoreName;
        public String FromStoreName
        {
            get
            {
                return _FromStoreName;
            }
            set
            {
                if (_FromStoreName != value)
                {
                    _FromStoreName = value;
                    OnPropertyChanged("FromStoreName");
                }
            }
        }

        private long? _ToStoreID;
        public long? ToStoreID
        {
            get
            {
                return _ToStoreID;
            }
            set
            {
                if (_ToStoreID != value)
                {
                    _ToStoreID = value;
                    OnPropertyChanged("ToStoreID");
                }
            }
        }
        private String _ToStoreName;
        public String ToStoreName
        {
            get
            {
                return _ToStoreName;
            }
            set
            {
                if (_ToStoreName != value)
                {
                    _ToStoreName = value;
                    OnPropertyChanged("ToStoreName");
                }
            }
        }


        private DateTime? _DueDate;
        public DateTime? DueDate
        {
            get
            {
                return _DueDate;
            }
            set
            {
                if (_DueDate != value)
                {
                    _DueDate = value;
                    OnPropertyChanged("DueDate");
                }
            }
        }

        private long? _IndentCreatedByID;
        public long? IndentCreatedByID
        {
            get
            {
                return _IndentCreatedByID;
            }
            set
            {
                if (_IndentCreatedByID != value)
                {
                    _IndentCreatedByID = value;
                    OnPropertyChanged("IndentCreatedByID");
                }
            }
        }

        private String _IndentCreatedByName;
        public String IndentCreatedByName
        {
            get
            {
                return _IndentCreatedByName;
            }
            set
            {
                if (_IndentCreatedByName != value)
                {
                    _IndentCreatedByName = value;
                    OnPropertyChanged("IndentCreatedByName");
                }
            }
        }


        private Boolean? _IsAuthorized;
        public Boolean? IsAuthorized
        {
            get
            {
                return _IsAuthorized;
            }
            set
            {
                if (_IsAuthorized != value)
                {
                    _IsAuthorized = value;
                    OnPropertyChanged("IsAuthorized");
                }
            }
        }

        private long? _AuthorizedByID;
        public long? AuthorizedByID
        {
            get
            {
                return _AuthorizedByID;
            }
            set
            {
                if (_AuthorizedByID != value)
                {
                    _AuthorizedByID = value;
                    OnPropertyChanged("AuthorizedByID");
                }
            }
        }

        private String _AuthorizedByName;
        public String AuthorizedByName
        {
            get
            {
                return _AuthorizedByName;
            }
            set
            {
                if (_AuthorizedByName != value)
                {
                    _AuthorizedByName = value;
                    OnPropertyChanged("AuthorizedByName");
                }
            }
        }

        private DateTime? _AuthorizationDate;
        public DateTime? AuthorizationDate
        {
            get
            {
                return _AuthorizationDate;
            }
            set
            {
                if (_AuthorizationDate != value)
                {
                    _AuthorizationDate = value;
                    OnPropertyChanged("AuthorizationDate");
                }
            }
        }
        private String _Remark;
        public String Remark
        {
            get
            {
                return _Remark;
            }
            set
            {
                if (_Remark != value)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }

        //private int? _IndentStatus;
        //public int? IndentStatus
        //{
        //    get
        //    {
        //        return _IndentStatus;
        //    }
        //    set
        //    {
        //        if (_IndentStatus != value)
        //        {
        //            _IndentStatus = value;
        //            OnPropertyChanged("IndentStatus");
        //        }
        //    }
        //}
        public bool IndentType { get; set; }
        public string IndentTypeName 
        { 
            get 
            {
                if (IndentType == false)
                    return "Manual";
                else
                    return "Auto";
            } 
        }
        //By Anjali............................................................
        //private bool _IsIndent;
        //public bool IsIndent
        //{
        //    get
        //    {
        //        return _IsIndent;
        //    }
        //    set
        //    {
        //        if (_IsIndent != value)
        //        {
        //            _IsIndent = value;
        //            OnPropertyChanged("IsIndent");
        //        }
        //    }
        //}
        private int _IsIndent;
        public int IsIndent
        {
            get
            {
                return _IsIndent;
            }
            set
            {
                if (_IsIndent != value)
                {
                    _IsIndent = value;
                    OnPropertyChanged("IsIndent");
                }
            }
        }

        
        //...........................................................................

        private InventoryIndentType _InventoryIndentType;
        public InventoryIndentType InventoryIndentType
        {
            get
            {
                return _InventoryIndentType;
            }
            set
            {
                if (_InventoryIndentType != value)
                {
                    _InventoryIndentType = value;
                    OnPropertyChanged("InventoryIndentType");
                }
            }
        }

        private InventoryIndentStatus _IndentStatus;
        public InventoryIndentStatus IndentStatus
        {
            get
            {
                return _IndentStatus;
            }
            set
            {
                if (_IndentStatus != value)
                {
                    _IndentStatus = value;
                    OnPropertyChanged("IndentStatus");
                }
            }
        }

        public string IndentStatusName { get { return _IndentStatus.ToString(); } }

        private bool _IsApproved;
        public bool IsApproved
        {
            get
            {
                return _IsApproved;
            }
            set
            {
                if (_IsApproved != value)
                {
                    _IsApproved = value;
                    OnPropertyChanged("IsApproved");
                }
            }
        }

        private bool _IsFreezed;
        public bool IsFreezed
        {
            get
            {
                return _IsFreezed;
            }
            set
            {
                if (_IsFreezed != value)
                {
                    _IsFreezed = value;
                    OnPropertyChanged("IsFreezed");
                }
            }
        }

        private bool _IsForwarded;
        public bool IsForwarded
        {
            get
            {
                return _IsForwarded;
            }
            set
            {
                if (_IsForwarded != value)
                {
                    _IsForwarded = value;
                    OnPropertyChanged("IsForwarded");
                }
            }
        }

        public bool IsModify { get; set; }

//***//--------------------------
        private long? _IssueId;
        public long? IssueId
        {
            get
            {
                return _IssueId;
            }
            set
            {
                if (value != _IssueId)
                {
                    _IssueId = value;
                    OnPropertyChanged("IssueId");
                }
            }
        }
        private long _IssueUnitID;
        public long IssueUnitID
        {
            get
            {
                return _IssueUnitID;
            }
            set
            {
                if (value != _IssueUnitID)
                {
                    _IssueUnitID = value;
                    OnPropertyChanged("IssueUnitID");
                }
            }
        }
//-----------------------------------------
        #endregion

        #region PatientIndent
        private long _PatientID;
        public long PatientID
        {
            get
            {
                return _PatientID;
            }
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
            get
            {
                return _PatientUnitID;
            }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }

        private bool _IsAgainstPatient;
        public bool IsAgainstPatient
        {
            get
            {
                return _IsAgainstPatient;
            }
            set
            {
                if (_IsAgainstPatient != value)
                {
                    _IsAgainstPatient = value;
                    OnPropertyChanged("IsAgainstPatient");
                }
            }
        }

        private string _PatientName;
        public string PatientName
        {
            get
            {
                return _PatientName;
            }
            set
            {
                if (_PatientName != value)
                {
                    _PatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }

        private string _MRNo;
        public string MRNo
        {
            get
            {
                return _MRNo;
            }
            set
            {
                if (_MRNo != value)
                {
                    _MRNo = value;
                    OnPropertyChanged("MRNo");
                }
            }
        }
        #endregion

        public List<clsIndentDetailVO> IndentDetailsList { get; set; }

        private List<clsIndentDetailVO> _objDeletedIndentDetailsList = null;
        public List<clsIndentDetailVO> DeletedIndentDetailsList
        {
            get { return _objDeletedIndentDetailsList; }
            set { _objDeletedIndentDetailsList = value; }
        }

        public bool IsChangeAndApprove { get; set; }

        # region ConvertToPR Properties

        private bool _IsConvertToPR;
        public bool IsConvertToPR
        {
            get
            {
                return _IsConvertToPR;
            }
            set
            {
                if (_IsConvertToPR != value)
                {
                    _IsConvertToPR = value;
                    OnPropertyChanged("IsConvertToPR");
                }
            }
        }


        private long _ConvertToPRID;
        public long ConvertToPRID
        {
            get
            {
                return _ConvertToPRID;
            }
            set
            {
                if (_ConvertToPRID != value)
                {
                    _ConvertToPRID = value;
                    OnPropertyChanged("ConvertToPRID");
                }
            }
        }


        /*  x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x 
         * 
         * Designed By : Rex Mathew
         * 
         * The below declaration allows the system to provide Bulk Indent Close Option as well as Single Indent Close Option
         * 
          x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x */


        private bool _IndentCloseStatus;
        public bool IndentCloseStatus
        {
            get { return _IndentCloseStatus; }
            set
            {
                if (_IndentCloseStatus != value)
                {
                    _IndentCloseStatus = value;
                    OnPropertyChanged("IndentCloseStatus");
                }
            }
        }


        private long _IndentBaseItemQty;
        public long IndentBaseItemQty
        {
            get
            {
                return _IndentBaseItemQty;
            }
            set
            {
                if (_IndentBaseItemQty != value)
                {
                    _IndentBaseItemQty = value;
                    OnPropertyChanged("IndentBaseItemQty");
                }
            }
        }
        


        private long _IssueBaseItemQty;
        public long IssueBaseItemQty
        {
            get
            {
                return _IssueBaseItemQty;
            }
            set
            {
                if (_IssueBaseItemQty != value)
                {
                    _IssueBaseItemQty = value;
                    OnPropertyChanged("IssueBaseItemQty");
                }
            }
        }

        private long _ReceivedBaseItemQty;
        public long ReceivedBaseItemQty
        {
            get
            {
                return _ReceivedBaseItemQty;
            }
            set
            {
                if (_ReceivedBaseItemQty != value)
                {
                    _ReceivedBaseItemQty = value;
                    OnPropertyChanged("ReceivedBaseItemQty");
                }
            }
        }



        /*  x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x 
         * 
         * Designed By : Rex Mathew
         * 
         * The below declaration allows the system to provide Bulk Purchase Requisition Close Option as well as Single Purchase Requisition Close Option
         * 
          x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x */

        private bool _PurchaseRequisitionStatus;
        public bool PurchaseRequisitionStatus
        {
            get { return _PurchaseRequisitionStatus; }
            set
            {
                if (_PurchaseRequisitionStatus != value)
                {
                    _PurchaseRequisitionStatus = value;
                    OnPropertyChanged("PurchaseRequisitionStatus");
                }
            }
        }


        private long _PRBaseItemQty;
        public long PRBaseItemQty
        {
            get
            {
                return _PRBaseItemQty;
            }
            set
            {
                if (_PRBaseItemQty != value)
                {
                    _PRBaseItemQty = value;
                    OnPropertyChanged("PRBaseItemQty");
                }
            }
        }



        private long _POBaseItemQty;
        public long POBaseItemQty
        {
            get
            {
                return _POBaseItemQty;
            }
            set
            {
                if (_POBaseItemQty != value)
                {
                    _POBaseItemQty = value;
                    OnPropertyChanged("POBaseItemQty");
                }
            }
        }

        private long _GRNBaseItemQty;
        public long GRNBaseItemQty
        {
            get
            {
                return _GRNBaseItemQty;
            }
            set
            {
                if (_GRNBaseItemQty != value)
                {
                    _GRNBaseItemQty = value;
                    OnPropertyChanged("GRNBaseItemQty");
                }
            }
        }




        # endregion

        #region Common Properties

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
        private long? _IndentUnitID;
         public long? IndentUnitID
        {
            get { return _IndentUnitID; }
            set
            {
                if (_IndentUnitID != value)
                {
                    _IndentUnitID = value;
                    OnPropertyChanged("IndentUnitID");
                }
            }
        }
        //private long _UnitId;
        //public long UnitId
        //{
        //    get { return _UnitId; }
        //    set
        //    {
        //        if (_UnitId != value)
        //        {
        //            _UnitId = value;
        //            OnPropertyChanged("UnitId");
        //        }
        //    }
        //}

        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
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


        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
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

        private long? _AddedBy;
        public long? AddedBy
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

        private DateTime? _AddedDateTime = DateTime.Now;
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

        private decimal _TotalRequiredQuantity; 
        public decimal TotalRequiredQuantity
        {
            get { return _TotalRequiredQuantity; }
            set
            {
                if (_TotalRequiredQuantity != value)
                {
                    _TotalRequiredQuantity = value;
                    OnPropertyChanged("TotalRequiredQuantity");
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

        private DateTime? _UpdatedDateTime = DateTime.Now;
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

        private string _UpdatedWindowsLoginName = "";
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

        private string _GRNNowithDate;
        public string GRNNowithDate
        {
            get { return _GRNNowithDate; }
            set
            {
                if (_GRNNowithDate != value)
                {
                    _GRNNowithDate = value;
                    OnPropertyChanged("GRNNowithDate");
                }
            }
        }

        private string _PONowithDate;
        public string PONowithDate
        {
            get { return _PONowithDate; }
            set
            {
                if (_PONowithDate != value)
                {
                    _PONowithDate = value;
                    OnPropertyChanged("PONowithDate");
                }
            }
        }

        private string _IssueNowithDate;
        public string IssueNowithDate
        {
            get { return _IssueNowithDate; }
            set
            {
                if (_IssueNowithDate != value)
                {
                    _IssueNowithDate = value;
                    OnPropertyChanged("IssueNowithDate");
                }
            }
        }

        private string _PRNowithDate;
        public string PRNowithDate
        {
            get { return _PRNowithDate; }
            set
            {
                if (_PRNowithDate != value)
                {
                    _PRNowithDate = value;
                    OnPropertyChanged("PRNowithDate");
                }
            }
        }



        #endregion


    }
}
