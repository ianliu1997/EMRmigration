using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsBlockItemsVO : IValueObject, INotifyPropertyChanged
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

        private bool _IsChecked;
        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                if (_IsChecked != value)
                {
                    _IsChecked = value;
                    OnPropertyChanged("IsChecked");
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

        private string _ItemCode;
        public string ItemCode
        {
            get { return _ItemCode; }
            set
            {
                if (_ItemCode != value)
                {
                    _ItemCode = value;
                    OnPropertyChanged("ItemCode");
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

        private long _RackID;
        public long RackID
        {
            get { return _RackID; }
            set
            {
                if (_RackID != value)
                {
                    _RackID = value;
                    OnPropertyChanged("RackID");
                }
            }
        }

        private long _ShelfID;
        public long ShelfID
        {
            get { return _ShelfID; }
            set
            {
                if (_ShelfID != value)
                {
                    _ShelfID = value;
                    OnPropertyChanged("ShelfID");
                }
            }
        }

        private long _ContainerID;
        public long ContainerID
        {
            get { return _ContainerID; }
            set
            {
                if (_ContainerID != value)
                {
                    _ContainerID = value;
                    OnPropertyChanged("ContainerID");
                }
            }
        }

        private string _Store;
        public string Store
        {
            get { return _Store; }
            set
            {
                if (_Store != value)
                {
                    _Store = value;
                    OnPropertyChanged("Store");
                }
            }
        }

        private string _Rack;
        public string Rack
        {
            get { return _Rack; }
            set
            {
                if (_Rack != value)
                {
                    _Rack = value;
                    OnPropertyChanged("Rack");
                }
            }
        }

        private string _Shelf;
        public string Shelf
        {
            get { return _Shelf; }
            set
            {
                if (_Shelf != value)
                {
                    _Shelf = value;
                    OnPropertyChanged("Shelf");
                }
            }
        }

        private string _Container;
        public string Container
        {
            get { return _Container; }
            set
            {
                if (_Container != value)
                {
                    _Container = value;
                    OnPropertyChanged("Container");
                }
            }
        }

        private double _AvailableStock;
        public double AvailableStock
        {
            get { return _AvailableStock; }
            set
            {
                if (_AvailableStock != value)
                {
                    _AvailableStock = value;
                    OnPropertyChanged("AvailableStock");
                }
            }
        }

        private double _AvailableStockInBase;
        public double AvailableStockInBase
        {
            get { return _AvailableStockInBase; }
            set
            {
                if (_AvailableStockInBase != value)
                {
                    _AvailableStockInBase = value;
                    OnPropertyChanged("AvailableStockInBase");
                }
            }
        }
       
        private DateTime? _ExpiryDate;
        public DateTime? ExpiryDate
        {
            get { return _ExpiryDate; }
            set
            {
                if (_ExpiryDate != value)
                {
                    _ExpiryDate = value;
                    OnPropertyChanged("ExpiryDate");
                }
            }
        }



        private long _SellingUMID;
        public long SellingUMID
        {
            get { return _SellingUMID; }
            set
            {
                if (_SellingUMID != value)
                {
                    _SellingUMID = value;
                    OnPropertyChanged("SellingUMID");
                }
            }
        }
        private string _SellingUM;
        public string SellingUM
        {
            get { return _SellingUM; }
            set
            {
                if (_SellingUM != value)
                {
                    _SellingUM = value;
                    OnPropertyChanged("SellingUM");
                }
            }
        }

        private long _BaseUMID;
        public long BaseUMID
        {
            get { return _BaseUMID; }
            set
            {
                if (_BaseUMID != value)
                {
                    _BaseUMID = value;
                    OnPropertyChanged("BaseUMID");
                }
            }
        }
        private string _BaseUM;
        public string BaseUM
        {
            get { return _BaseUM; }
            set
            {
                if (_BaseUM != value)
                {
                    _BaseUM = value;
                    OnPropertyChanged("BaseUM");
                }
            }
        }

        private long _PurchaseUMID;
        public long PurchaseUMID
        {
            get { return _PurchaseUMID; }
            set
            {
                if (_PurchaseUMID != value)
                {
                    _PurchaseUMID = value;
                    OnPropertyChanged("PurchaseUMID");
                }
            }
        }
        private string _PurchaseUM;
        public string PurchaseUM
        {
            get { return _PurchaseUM; }
            set
            {
                if (_PurchaseUM != value)
                {
                    _PurchaseUM = value;
                    OnPropertyChanged("PurchaseUM");
                }
            }
        }

        private long _StockingUMID;
        public long StockingUMID
        {
            get { return _StockingUMID; }
            set
            {
                if (_StockingUMID != value)
                {
                    _StockingUMID = value;
                    OnPropertyChanged("StockingUMID");
                }
            }
        }
        private string _StockingUM;
        public string StockingUM
        {
            get { return _StockingUM; }
            set
            {
                if (_StockingUM != value)
                {
                    _StockingUM = value;
                    OnPropertyChanged("StockingUM");
                }
            }
        }

        private float _SellingCF;
        public float SellingCF
        {
            get { return _SellingCF; }
            set
            {
                if (_SellingCF != value)
                {
                    _SellingCF = value;
                    OnPropertyChanged("SellingCF");
                }
            }
        }
        private float _StockingCF;
        public float StockingCF
        {
            get { return _StockingCF; }
            set
            {
                if (_StockingCF != value)
                {
                    _StockingCF = value;
                    OnPropertyChanged("StockingCF");
                }
            }
        }


    }

    public class clsPhysicalItemsMainVO : IValueObject, INotifyPropertyChanged
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


      

        private bool _RadioStatusYes = true;
        public bool RadioStatusYes
        {
            get { return _RadioStatusYes; }
            set
            {
                if (_RadioStatusYes != value)
                {
                    _RadioStatusYes = value;
                    OnPropertyChanged("RadioStatusYes");
                }
            }

        }
        private bool _RadioStatusNo;
        public bool RadioStatusNo
        {
            get { return _RadioStatusNo; }
            set
            {
                if (_RadioStatusNo != value)
                {
                    _RadioStatusNo = value;
                    OnPropertyChanged("RadioStatusNo");
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

        private DateTime? _RequestDateTime;
        public DateTime? RequestDateTime
        {
            get { return _RequestDateTime; }
            set
            {
                if (_RequestDateTime != value)
                {
                    _RequestDateTime = value;
                    OnPropertyChanged("RequestDateTime");
                }
            }
        }

        private long _RequestedBy;
        public long RequestedBy
        {
            get { return _RequestedBy; }
            set
            {
                if (_RequestedBy != value)
                {
                    _RequestedBy = value;
                    OnPropertyChanged("RequestedBy");
                }
            }
        }

        private string _RequestedByName;
        public string RequestedByName
        {
            get { return _RequestedByName; }
            set
            {
                if (_RequestedByName != value)
                {
                    _RequestedByName = value;
                    OnPropertyChanged("RequestedByName");
                }
            }
        }

        //IsConvertedToSA meaning Ia converted to Stock Adjustment
        private bool _IsConvertedToSA;
        public bool IsConvertedToSA
        {
            get { return _IsConvertedToSA; }
            set
            {
                if (_IsConvertedToSA != value)
                {
                    _IsConvertedToSA = value;
                    OnPropertyChanged("IsConvertedToSA");
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

        private string _Store;
        public string Store
        {
            get { return _Store; }
            set
            {
                if (_Store != value)
                {
                    _Store = value;
                    OnPropertyChanged("Store");
                }
            }
        }


        private string _PhysicalItemStockNo;
        public string PhysicalItemStockNo
        {
            get { return _PhysicalItemStockNo; }
            set
            {
                if (_PhysicalItemStockNo != value)
                {
                    _PhysicalItemStockNo = value;
                    OnPropertyChanged("PhysicalItemStockNo");
                }
            }
        }

      
        
    }

    public class clsPhysicalItemsVO : IValueObject, INotifyPropertyChanged
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


        

        private bool _RadioStatusYes ;
        public bool RadioStatusYes
        {
            get { return _RadioStatusYes; }
            set
            {
                if (_RadioStatusYes != value)
                {
                    _RadioStatusYes = value;
                    OnPropertyChanged("RadioStatusYes");
                    OnPropertyChanged("intOperationType");
                    OnPropertyChanged("PhysicalQuantity");
                }
            }

        }
        private bool _RadioStatusNo;
        public bool RadioStatusNo
        {
            get { return _RadioStatusNo; }
            set
            {
                if (_RadioStatusNo != value)
                {
                    _RadioStatusNo = value;
                    OnPropertyChanged("RadioStatusNo");
                    OnPropertyChanged("intOperationType");
                    OnPropertyChanged("PhysicalQuantity");
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

         private long _PhysicalItemID;
        public long PhysicalItemID
        {
            get { return _PhysicalItemID; }
            set
            {
                if (_PhysicalItemID != value)
                {
                    _PhysicalItemID = value;
                    OnPropertyChanged("PhysicalItemID");
                }
            }
        }

         private long _PhysicalItemUnitID;
        public long PhysicalItemUnitID
        {
            get { return _PhysicalItemUnitID; }
            set
            {
                if (_PhysicalItemUnitID != value)
                {
                    _PhysicalItemUnitID = value;
                    OnPropertyChanged("PhysicalItemUnitID");
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

        private string _ItemCode;
        public string ItemCode
        {
            get { return _ItemCode; }
            set
            {
                if (_ItemCode != value)
                {
                    _ItemCode = value;
                    OnPropertyChanged("ItemCode");
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

        private long _RackID;
        public long RackID
        {
            get { return _RackID; }
            set
            {
                if (_RackID != value)
                {
                    _RackID = value;
                    OnPropertyChanged("RackID");
                }
            }
        }

        private long _ShelfID;
        public long ShelfID
        {
            get { return _ShelfID; }
            set
            {
                if (_ShelfID != value)
                {
                    _ShelfID = value;
                    OnPropertyChanged("ShelfID");
                }
            }
        }

        private long _ContainerID;
        public long ContainerID
        {
            get { return _ContainerID; }
            set
            {
                if (_ContainerID != value)
                {
                    _ContainerID = value;
                    OnPropertyChanged("ContainerID");
                }
            }
        }

        private string _Store;
        public string Store
        {
            get { return _Store; }
            set
            {
                if (_Store != value)
                {
                    _Store = value;
                    OnPropertyChanged("Store");
                }
            }
        }

        private string _Rack;
        public string Rack
        {
            get { return _Rack; }
            set
            {
                if (_Rack != value)
                {
                    _Rack = value;
                    OnPropertyChanged("Rack");
                }
            }
        }

        private string _Shelf;
        public string Shelf
        {
            get { return _Shelf; }
            set
            {
                if (_Shelf != value)
                {
                    _Shelf = value;
                    OnPropertyChanged("Shelf");
                }
            }
        }

        private string _Container;
        public string Container
        {
            get { return _Container; }
            set
            {
                if (_Container != value)
                {
                    _Container = value;
                    OnPropertyChanged("Container");
                }
            }
        }

        private double _AvailableStock;
        public double AvailableStock
        {
            get { return _AvailableStock; }
            set
            {
                if (_AvailableStock != value)
                {
                    _AvailableStock = value;
                    OnPropertyChanged("AvailableStock");
                }
            }
        }

        private double _AvailableStockInBase;
        public double AvailableStockInBase
        {
            get { return _AvailableStockInBase; }
            set
            {
                if (_AvailableStockInBase != value)
                {
                    _AvailableStockInBase = value;
                    OnPropertyChanged("AvailableStockInBase");
                }
            }
        }

         private DateTime? _ExpiryDate;
        public DateTime? ExpiryDate
        {
            get { return _ExpiryDate; }
            set
            {
                if (_ExpiryDate != value)
                {
                    _ExpiryDate = value;
                    OnPropertyChanged("ExpiryDate");
                }
            }
        }
      
        //private int _intOperationType;
        //public int intOperationType
        //{
        //    get {
        //        if (RadioStatusYes == true)
        //            _intOperationType = 1;
        //        else if (RadioStatusNo == true)
        //            _intOperationType = 2;
                
        //        return _intOperationType;
        //    }
        //    set
        //    {
        //        if (_intOperationType != value)
        //        {
        //            _intOperationType = value;
                   
        //            OnPropertyChanged("intOperationType");
        //            OnPropertyChanged("PhysicalQuantity");
        //        }
        //    }
        //}

        private int _intOperationType;
        public int intOperationType
        {
            get
            {
                
                return _intOperationType;
            }
            set
            {
                if (_intOperationType != value)
                {
                    _intOperationType = value;

                    OnPropertyChanged("intOperationType");
                }
            }
        } 

        public string intOperationTypeName { get; set; }
          private InventoryStockOperationType _OperationType;
        public InventoryStockOperationType OperationType
        {
            get { return _OperationType; }
            set
            {
                if (_OperationType != value)
                {
                    _OperationType = value;
                    OnPropertyChanged("OperationType");
                }
            }
        }
        //private double _AdjustmentQunatity;
        //public double AdjustmentQunatity
        //{
        //    get { return _AdjustmentQunatity; }
        //    set
        //    {
        //        if (_AdjustmentQunatity != value)
        //        {
        //            _AdjustmentQunatity = value;
        //            OnPropertyChanged("AdjustmentQunatity");
        //            OnPropertyChanged("PhysicalQuantity");

        //        }
        //    }
        //}


        private double _AdjustmentQunatity;
        public double AdjustmentQunatity
        {
            get {
                //By Anjali..................
                //if (PhysicalQuantity > AvailableStock)
                //{
                //    intOperationType = (int)InventoryStockOperationType.Addition;
                //    _AdjustmentQunatity = PhysicalQuantity - AvailableStock;
                //}
                //else if (PhysicalQuantity < AvailableStock)
                //{
                //    intOperationType = (int)InventoryStockOperationType.Subtraction;
                //    _AdjustmentQunatity = AvailableStock - PhysicalQuantity;
                //}
                //else if (PhysicalQuantity == AvailableStock)
                //{
                //    intOperationType = (int)InventoryStockOperationType.None;
                //    _AdjustmentQunatity = 0;
                //}
                //return _AdjustmentQunatity;

                if (BaseQuantity > AvailableStockInBase)
                {
                    intOperationType = (int)InventoryStockOperationType.Addition;
                    _AdjustmentQunatity = (BaseQuantity - AvailableStockInBase) / BaseConversionFactor;

                }
                else if (BaseQuantity < AvailableStockInBase)
                {
                    intOperationType = (int)InventoryStockOperationType.Subtraction;
                    _AdjustmentQunatity = (AvailableStockInBase - BaseQuantity) / BaseConversionFactor;
                }
                else if (BaseQuantity == AvailableStockInBase)
                {
                    intOperationType = (int)InventoryStockOperationType.None;
                    _AdjustmentQunatity = 0;
                }
                return _AdjustmentQunatity;

                //.................................................
            }
            set
            {
                if (_AdjustmentQunatity != value)
                {
                    //_AdjustmentQunatity = value;
                    //if (PhysicalQuantity > AvailableStock)
                    //{
                    //    intOperationType =(int)InventoryStockOperationType.Addition;
                    //    _AdjustmentQunatity = PhysicalQuantity - AvailableStock;
                    //}
                    //else if (PhysicalQuantity < AvailableStock)
                    //{
                    //    intOperationType = (int)InventoryStockOperationType.Subtraction;
                    //    _AdjustmentQunatity =  AvailableStock -PhysicalQuantity;
                    //}
                    //else if (PhysicalQuantity == AvailableStock)
                    //{
                    //    intOperationType = (int)InventoryStockOperationType.None;
                    //    _AdjustmentQunatity = 0;
                    //}
                    _AdjustmentQunatity = value; 
                    OnPropertyChanged("AdjustmentQunatity");
                    OnPropertyChanged("intOperationType");

                }
            }
        }


        //private double _PhysicalQuantity;
        //public double PhysicalQuantity
        //{
        //    get { 
        //        //return _PhysicalQuantity; 
        //        if (intOperationType == 1)
        //        {
        //            _PhysicalQuantity = _AdjustmentQunatity + _AvailableStock;
        //        }
        //        else if (intOperationType == 2)
        //        {
        //            _PhysicalQuantity = _AvailableStock - _AdjustmentQunatity;
        //        }
        //        if (_PhysicalQuantity > 0)
        //            return _PhysicalQuantity;
        //        else
        //            return _PhysicalQuantity = 0;
        //    }
        //    set
        //    {
        //        if (_PhysicalQuantity != value)
        //        {
        //            _PhysicalQuantity = value;
        //            OnPropertyChanged("PhysicalQuantity");
        //        }
        //    }
        //}

        private double _PhysicalQuantity;
        public double PhysicalQuantity
        {
            get
            {
                return _PhysicalQuantity; 
               
            }
            set
            {
                if (_PhysicalQuantity != value)
                {
                    _PhysicalQuantity = value;
                    OnPropertyChanged("PhysicalQuantity");
                    OnPropertyChanged("AdjustmentQunatity");
                }
            }
        }

        private string _Remark;

        public string Remark
        {
            get { return _Remark; }
            set
            {
                if (value != _Remark)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }



        private long _SellingUMID;
        public long SellingUMID
        {
            get { return _SellingUMID; }
            set
            {
                if (_SellingUMID != value)
                {
                    _SellingUMID = value;
                    OnPropertyChanged("SellingUMID");
                }
            }
        }
        private string _SellingUM;
        public string SellingUM
        {
            get { return _SellingUM; }
            set
            {
                if (_SellingUM != value)
                {
                    _SellingUM = value;
                    OnPropertyChanged("SellingUM");
                }
            }
        }

        private long _BaseUMID;
        public long BaseUMID
        {
            get { return _BaseUMID; }
            set
            {
                if (_BaseUMID != value)
                {
                    _BaseUMID = value;
                    OnPropertyChanged("BaseUMID");
                }
            }
        }
        private string _BaseUM;
        public string BaseUM
        {
            get { return _BaseUM; }
            set
            {
                if (_BaseUM != value)
                {
                    _BaseUM = value;
                    OnPropertyChanged("BaseUM");
                }
            }
        }

        private long _PurchaseUMID;
        public long PurchaseUMID
        {
            get { return _PurchaseUMID; }
            set
            {
                if (_PurchaseUMID != value)
                {
                    _PurchaseUMID = value;
                    OnPropertyChanged("PurchaseUMID");
                }
            }
        }
        private string _PurchaseUM;
        public string PurchaseUM
        {
            get { return _PurchaseUM; }
            set
            {
                if (_PurchaseUM != value)
                {
                    _PurchaseUM = value;
                    OnPropertyChanged("PurchaseUM");
                }
            }
        }

        private long _StockingUMID;
        public long StockingUMID
        {
            get { return _StockingUMID; }
            set
            {
                if (_StockingUMID != value)
                {
                    _StockingUMID = value;
                    OnPropertyChanged("StockingUMID");
                }
            }
        }
        private string _StockingUM;
        public string StockingUM
        {
            get { return _StockingUM; }
            set
            {
                if (_StockingUM != value)
                {
                    _StockingUM = value;
                    OnPropertyChanged("StockingUM");
                }
            }
        }


        List<MasterListItem> _UOMList = new List<MasterListItem>();
        public List<MasterListItem> UOMList
        {
            get
            {
                return _UOMList;
            }
            set
            {
                if (value != _UOMList)
                {
                    _UOMList = value;

                }
            }

        }

        List<clsConversionsVO> _UOMConversionList = new List<clsConversionsVO>();
        public List<clsConversionsVO> UOMConversionList
        {
            get
            {
                return _UOMConversionList;
            }
            set
            {
                if (value != _UOMConversionList)
                {
                    _UOMConversionList = value;
                    OnPropertyChanged("UOMConversionList");
                }
            }

        }

        MasterListItem _SelectedUOM = new MasterListItem();
        public MasterListItem SelectedUOM
        {
            get
            {
                return _SelectedUOM;
            }
            set
            {
                if (value != _SelectedUOM)
                {
                    _SelectedUOM = value;
                    OnPropertyChanged("SelectedUOM");
                }
            }


        }
        private string _UOM;
        public string UOM
        {
            get { return _UOM; }
            set
            {
                if (_UOM != value)
                {
                    _UOM = value;
                    OnPropertyChanged("UOM");
                }
            }
        }
        private long _UOMID;
        public long UOMID
        {
            get { return _UOMID; }
            set
            {
                if (_UOMID != value)
                {
                    _UOMID = value;
                    OnPropertyChanged("UOMID");
                }
            }
        }
        
        

        private float _ConversionFactor = 1;
        public float ConversionFactor
        {
            get { return _ConversionFactor; }
            set
            {
                if (_ConversionFactor != value)
                {
                    _ConversionFactor = value;
                    OnPropertyChanged("ConversionFactor");
                }
            }
        }
        private float _BaseConversionFactor;
        public float BaseConversionFactor
        {
            get { return _BaseConversionFactor; }
            set
            {
                if (_BaseConversionFactor != value)
                {
                    _BaseConversionFactor = value;
                    OnPropertyChanged("BaseConversionFactor");
                }
            }
        }


        private float _BaseQuantity;
        public float BaseQuantity
        {
            get
            {

                return _BaseQuantity;
            }
            set
            {
                if (_BaseQuantity != value)
                {

                    _BaseQuantity = value;
                    OnPropertyChanged("BaseQuantity");
                    OnPropertyChanged("SingleQuantity");
                    OnPropertyChanged("Quantity");
                }
            }
        }

        private float _SingleQuantity;
        public float SingleQuantity
        {
            get
            {

                return _SingleQuantity;
            }
            set
            {
                if (_SingleQuantity != value)
                {

                    _SingleQuantity = value;
                    OnPropertyChanged("SingleQuantity");
                    OnPropertyChanged("Quantity");
                   



                }
            }
        }

        private float _Quantity;
        public float Quantity
        {
            get
            {

                return _Quantity;
            }
            set
            {
                if (_Quantity != value)
                {
                    //if (_Quantity == 0)
                    //    throw new ValidationException("Quantity should not be zero");

                    //else
                    //{
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                    OnPropertyChanged("Amount");
               
                    //}


                }
            }
        }

    }
}
