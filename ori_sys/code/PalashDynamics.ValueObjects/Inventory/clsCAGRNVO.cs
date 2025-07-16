using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsCAGRNVO : IValueObject, INotifyPropertyChanged
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

        #region Common Properties

       
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

       private List<clsCAGRNDetailsVO> _Items = new List<clsCAGRNDetailsVO>();

       
       public List<clsCAGRNDetailsVO> ItemsCAGRN
       {
           get { return _Items; }
           set
           {
               if (_Items != value)
               {
                   _Items = value;
                   OnPropertyChanged("ItemsCAGRN");
               }
           }
       }
        private long _Id;
        public long ID
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private DateTime _Date = DateTime.Now;
        public DateTime Date
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


        

        private DateTime _Time = DateTime.Now;
        public DateTime Time
        {
            get { return _Time; }
            set
            {
                if (_Time != value)
                {
                    _Time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        private string _GRNNO;
        public string GRNNO
        {
            get { return _GRNNO; }
            set
            {
                if (_GRNNO != value)
                {
                    _GRNNO = value;
                    OnPropertyChanged("GRNNO");
                }
            }
        }

        private InventoryGRNType _GRNType;
        public InventoryGRNType GRNType
        {
            get { return _GRNType; }
            set
            {
                if (_GRNType != value)
                {
                    _GRNType = value;
                    OnPropertyChanged("GRNType");
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

        public string SupplierName { get; set; }
        public string StoreName { get; set; }


        public string GRNTypeName
        {
            get { return _GRNType.ToString(); }

        }

            


        private double _AvailableQuantity;
        public double AvailableQuantity
        {
            get { return _AvailableQuantity; }
            set
            {
                if (_AvailableQuantity != value)
                {
                    _AvailableQuantity = value;
                    OnPropertyChanged("AvailableQuantity");
                }
            }
        }

       
        private Boolean _Finalized;
        public Boolean Finalized
        {
            get { return _Finalized; }
            set
            {
                if (_Finalized != value)
                {
                    _Finalized = value;
                    OnPropertyChanged("Finalized");
                    OnPropertyChanged("IsEnabledFinalized");
                }
            }

            
        }


        private Boolean _Freezed;
        public Boolean Freezed
        {
            get { return _Freezed; }
            set
            {
                if (_Freezed != value)
                {
                    _Freezed = value;
                    OnPropertyChanged("Freezed");
                    OnPropertyChanged("IsEnabledFreezed");
                }
            }
        }


        private string _ServiceAgent;
        public string ServiceAgent
        {
            get { return _ServiceAgent; }
            set
            {
                if (_ServiceAgent != value)
                {
                    _ServiceAgent = value;
                    OnPropertyChanged("ServiceAgent");
                }
            }
        }

        private DateTime? _ContractExpiryDate;
        public DateTime? ContractExpiryDate
        {
            get { return _ContractExpiryDate; }
            set
            {
                if (_ContractExpiryDate != value)
                {
                    _ContractExpiryDate = value;
                    OnPropertyChanged("ContractExpiryDate");
                }
            }
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
                    OnPropertyChanged("FromDate");
                }
            }
        }

        private DateTime? _EndDate;
        public DateTime? EndDate
        {
            get { return _EndDate; }
            set
            {
                if (_EndDate != value)
                {
                    _EndDate = value;
                    OnPropertyChanged("EndDate");
                }
            }
        }

        private string _SerialNo = "";
        public string SerialNo
        {
            get { return _SerialNo; }
            set
            {
                if (_SerialNo != value)
                {
                    _SerialNo = value;
                    OnPropertyChanged("SerialNo");
                }
            }
        }

       
    }


    public class clsCAGRNDetailsVO : IValueObject, INotifyPropertyChanged
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

        #region Common Properties

       

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

       


        #endregion

        public bool SelectItem { get; set; }

        
        private double _ItemQuantity;
        public double ItemQuantity
        {
            get { return _ItemQuantity; }
            set
            {
                if (_ItemQuantity != value)
                {
                    _ItemQuantity = value;
                    OnPropertyChanged("ItemQuantity");
                }
            }
        }
       
       
        private long _ItemCategory;
        public long ItemCategory
        {
            get { return _ItemCategory; }
            set
            {
                if (_ItemCategory != value)
                {
                    _ItemCategory = value;
                    OnPropertyChanged("ItemCategory");
                }
            }
        }
        private long _ItemGroup;
        public long ItemGroup
        {
            get { return _ItemGroup; }
            set
            {
                if (_ItemGroup != value)
                {
                    _ItemGroup = value;
                    OnPropertyChanged("ItemGroup");
                }
            }
        }

       
        private long _Id;
        public long ID
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        
        private long _GRNID;
        public long GRNID
        {
            get { return _GRNID; }
            set
            {
                if (_GRNID != value)
                {
                    _GRNID = value;
                    OnPropertyChanged("GRNID");
                }
            }
        }

        private long _GRNUnitID;
        public long GRNUnitID
        {
            get { return _GRNUnitID; }
            set
            {
                if (_GRNUnitID != value)
                {
                    _GRNUnitID = value;
                    OnPropertyChanged("GRNUnitID");
                }
            }
        }

        private long _ItemID;
        public long ItemID
        {
            get { return _ItemID; }
            set
            {
                if (_ItemID != value)
                {
                    _ItemID = value;
                    OnPropertyChanged("ItemID");
                }
            }
        }

        private bool _IsBatchRequired;
        public bool IsBatchRequired
        {
            get { return _IsBatchRequired; }
            set
            {
                if (_IsBatchRequired != value)
                {
                    _IsBatchRequired = value;
                    OnPropertyChanged("IsBatchRequired");
                }
            }
        }

        private string _ItemName;
        public string ItemName
        {
            get { return _ItemName; }
            set
            {
                if (_ItemName != value)
                {
                    _ItemName = value;
                    OnPropertyChanged("ItemName");
                }
            }
        }

        private long _BatchID;
        public long BatchID
        {
            get { return _BatchID; }
            set
            {
                if (_BatchID != value)
                {
                    _BatchID = value;
                    OnPropertyChanged("BatchID");
                }
            }
        }

        private string _BatchCode;
        public string BatchCode
        {
            get { return _BatchCode; }
            set
            {
                if (_BatchCode != value)
                {
                    _BatchCode = value;
                    OnPropertyChanged("BatchCode");
                }
            }
        }

        

        private string _ItemCode;
        public string ItemCode
        {
            get
            {
                return _ItemCode;
            }
            set
            {
                if (value != _ItemCode)
                {
                    _ItemCode = value;
                    OnPropertyChanged("ItemCode");
                }
            }
        }

        
        

    }

   
   
}
