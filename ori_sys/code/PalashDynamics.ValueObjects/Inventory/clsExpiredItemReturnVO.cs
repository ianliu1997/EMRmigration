using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsExpiredItemReturnVO : INotifyPropertyChanged, IValueObject
    {
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



        private long _SupplierID;
        public long SupplierID
        {
            get { return _SupplierID; }
            set
            {
                if (_SupplierID != value)
                {
                    _SupplierID = value;
                    OnPropertyChanged("SupplierID");
                }
            }
        }


        private string _ExpiryReturnNo;
        public string ExpiryReturnNo
        {
            get { return _ExpiryReturnNo; }
            set
            {
                if (_ExpiryReturnNo != value)
                {
                    _ExpiryReturnNo = value;
                    OnPropertyChanged("ExpiryReturnNo");
                }
            }
        }

        private string _GateEntryNo;
        public string GateEntryNo
        {
            get { return _GateEntryNo; }
            set
            {
                if (_GateEntryNo != value)
                {
                    _GateEntryNo = value;
                    OnPropertyChanged("GateEntryNo");
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

        private double _TotalAmount;
        public double TotalAmount
        {
            get { return _TotalAmount; }
            set
            {
                if (_TotalAmount != value)
                {
                    _TotalAmount = value;
                    OnPropertyChanged("TotalAmount");
                }
            }
        }

        private double _TotalVATAmount;
        public double TotalVATAmount
        {
            get { return _TotalVATAmount; }
            set
            {
                if (_TotalVATAmount != value)
                {
                    _TotalVATAmount = value;
                    OnPropertyChanged("TotalVATAmount");
                }
            }
        }


        private double _TotalTaxAmount;
        public double TotalTaxAmount
        {
            get { return _TotalTaxAmount; }
            set
            {
                if (_TotalTaxAmount != value)
                {
                    _TotalTaxAmount = value;
                    OnPropertyChanged("TotalTaxAmount");
                }
            }
        }

        private double _TotalOctriAmount;
        public double TotalOctriAmount
        {
            get { return _TotalOctriAmount; }
            set
            {
                if (_TotalOctriAmount != value)
                {
                    _TotalOctriAmount = value;
                    OnPropertyChanged("TotalOctriAmount");
                }
            }
        }

        private double _OtherDeducution;
        public double OtherDeducution
        {
            get { return _OtherDeducution; }
            set
            {
                if (_OtherDeducution != value)
                {
                    _OtherDeducution = value;
                    OnPropertyChanged("OtherDeducution");
                }
            }
        }

        private double _NetAmount;
        public double NetAmount
        {
            get { return _NetAmount; }
            set
            {
                if (_NetAmount != value)
                {
                    _NetAmount = value;
                    OnPropertyChanged("NetAmount");
                }
            }
        }

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
        private string _SupplierName;
        public string SupplierName
        {
            get { return _SupplierName; }
            set
            {
                if (_SupplierName != value)
                {
                    _SupplierName = value;
                    OnPropertyChanged("SupplierName");
                }
            }
        }

        private string _StoreName;
        public string StoreName
        {
            get { return _StoreName; }
            set
            {
                if (_StoreName != value)
                {
                    _StoreName = value;
                    OnPropertyChanged("StoreName");
                }
            }
        }

        #region Approve Flow Variables

        private Boolean _Approve;
        public Boolean Approve
        {
            get { return _Approve; }
            set
            {
                if (_Approve != value)
                {
                    _Approve = value;
                    OnPropertyChanged("Approve");
                }
            }
        }

        private Boolean _Reject;
        public Boolean Reject
        {
            get { return _Reject; }
            set
            {
                if (_Reject != null)
                    _Reject = value;
                OnPropertyChanged("Reject");
            }
        }

        public bool EditForApprove { get; set; }  //Added for Save Expired Item Return In to  T_ItemExpiryReturnHistory  And  T_ItemExpiryReturnDetailsHistory

        private Boolean _DirectApprove = false;
        public Boolean DirectApprove
        {
            get { return _DirectApprove; }
            set
            {
                if (_DirectApprove != value)
                {
                    _DirectApprove = value;
                    OnPropertyChanged("DirectApprove");
                }
            }
        }

        private string _ApproveOrRejectByName;
        public string ApproveOrRejectByName
        {
            get { return _ApproveOrRejectByName; }
            set
            {
                if (_ApproveOrRejectByName != value)
                {
                    _ApproveOrRejectByName = value;
                    OnPropertyChanged("ApproveOrRejectByName");
                }
            }
        }

        #endregion

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

        private long _UnitId;
        public long UnitId
        {
            get { return _UnitId; }
            set
            {
                if (_UnitId != value)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
                }
            }
        }

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
        private clsItemStockVO _StockDetails = new clsItemStockVO();
        public clsItemStockVO StockDetails
        {
            get { return _StockDetails; }
            set
            {
                if (_StockDetails != value)
                {
                    _StockDetails = value;
                    OnPropertyChanged("StockDetails");
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



        #endregion
    }
}
