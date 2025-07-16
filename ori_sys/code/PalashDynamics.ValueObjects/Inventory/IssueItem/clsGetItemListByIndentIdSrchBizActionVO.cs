using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetItemListByIndentIdSrchBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemListByIndentIdSrchBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {

                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }

        public List<clsItemListByIndentId> ItemList { get; set; }
        public long? IndentId { get; set; }
        public long? UnitID { get; set; }

        public bool FromPO { get; set; }
        public InventoryTransactionType TransactionType { get; set; }

        //added by MMBABU
        private bool _IssueIndentFlag = false;
        public bool IssueIndentFlag
        {
            get
            {
                return _IssueIndentFlag;
            }
            set
            {
                _IssueIndentFlag = value;
            }
        }

    }
    public class clsGetIndentListBySupplierBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetIndentListBySupplierBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {

                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }

        public List<clsItemListByIndentId> ItemList { get; set; }
        public long? IndentId { get; set; }

        public bool FromPO { get; set; }
        public InventoryTransactionType TransactionType { get; set; }

        //added by MMBABU
        private bool _IssueIndentFlag = false;
        public bool IssueIndentFlag
        {
            get
            {
                return _IssueIndentFlag;
            }
            set
            {
                _IssueIndentFlag = value;
            }
        }


        public long? FromIndentStoreId { get; set; }
        public long? ToIndentStoreId { get; set; }
        public long? LoginUserUnitId { get; set; }
        public long UnitID { get; set; }
        public int StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool Freezed { get; set; }
        public bool PagingEnabled { get; set; }
        public Int32 TotalRowCount { get; set; }
        public string SIndentNumber { get; set; }
        public string SItemName { get; set; }
        public long SSupplierID { get; set; }

        // Added By CDS
        private bool _IsItemBlock;
        public bool IsItemBlock
        {
            get
            {
                return _IsItemBlock;
            }
            set
            {
                if (value != _IsItemBlock)
                {
                    _IsItemBlock = value;
                }
            }
        }
    }
    public class clsItemListByIndentId
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
        private Boolean? _IsChecked = false;
        public Boolean? IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                _IsChecked = value;
            }
        }
        public long? ItemId { get; set; }
        public String ItemName { get; set; }
        public String ItemCode { get; set; }
        public long? IndentId { get; set; }
        public String IndentNumber { get; set; }
        public String PurchaseRequisitionNumber { get; set; }
        public decimal? IndentQty { get; set; }

        private float _POApprItemQty;
        public float POApprItemQty
        {
            get
            {
                return _POApprItemQty;
            }
            set
            {
                if (_POApprItemQty != value)
                {
                    _POApprItemQty = value;
                    OnPropertyChanged("POApprItemQty");
                }
            }
        }

        private float _FinalPendindQty;  // showing the qty for Approved PO,PO. 
        public float FinalPendindQty
        {
            get
            {
                return _FinalPendindQty;
            }
            set
            {
                if (_FinalPendindQty != value)
                {
                    _FinalPendindQty = value;
                    OnPropertyChanged("FinalPendindQty");
                }
            }
        }

        private float _POPendingItemQty;
        public float POPendingItemQty
        {
            get
            {
                return _POPendingItemQty;
            }
            set
            {
                if (_POPendingItemQty != value)
                {
                    _POPendingItemQty = value;
                    OnPropertyChanged("POPendingItemQty");
                }
            }
        }

        public string PUM { get; set; }
        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;

                }
            }
        }
        //newly added
        public decimal? BalanceQty { get; set; }
        public decimal? IssuePendingQuantity { get; set; }
        // public long? ConversionFactor { get; set; }
        public float ConversionFactor { get; set; }
        public decimal? IssueQty { get; set; }
        public long IndentUnitID { get; set; }
        public long IndentDetailsID { get; set; }
        public long IndentDetailsUnitID { get; set; }
        public long ToStoreID { get; set; }
        public long FromStoreID { get; set; }
        public decimal? PurchaseRate { get; set; }
        public decimal? VATPer { get; set; }
        public decimal? MRP { get; set; }
        public string Supplier { get; set; }
        public long SupplierID { get; set; }
        public long ItemCategory { get; set; }
        public long ItemGroup { get; set; }
        public string PRNoWithStoreName { get; set; }

        //By Anjali..............................

        public int IsIndent { get; set; }
        //public bool IsItemBlock { get; set; }

        private bool _IsItemBlock;
        public bool IsItemBlock
        {
            get { return _IsItemBlock; }
            set
            {
                if (_IsItemBlock != value)
                {
                    _IsItemBlock = value;
                    OnPropertyChanged("IsItemBlock");
                }
            }
        }
        // public bool IsIndent { get; set; }

        //.................................

        # region Conversion Factor

        private decimal _ItemVatPer;
        public decimal ItemVatPer
        {
            get
            {
                return _ItemVatPer;
            }
            set
            {
                if (value != _ItemVatPer)
                {
                    _ItemVatPer = value;
                    // OnPropertyChanged("ItemVatPer");
                }
            }
        }

        private int _ItemVatType;
        public int ItemVatType
        {
            get
            {
                return _ItemVatType;
            }
            set
            {
                if (value != _ItemVatType)
                {
                    _ItemVatType = value;
                    //  OnPropertyChanged("ItemVatType");
                }
            }
        }

        private int _ItemVatApplicationOn;
        public int ItemVatApplicationOn
        {
            get
            {
                return _ItemVatApplicationOn;
            }
            set
            {
                if (value != _ItemVatApplicationOn)
                {
                    _ItemVatApplicationOn = value;
                    //OnPropertyChanged("ItemVatApplicationOn");
                }
            }
        }

        private int _ItemOtherTaxType;
        public int ItemOtherTaxType
        {
            get
            {
                return _ItemOtherTaxType;
            }
            set
            {
                if (value != _ItemOtherTaxType)
                {
                    _ItemOtherTaxType = value;
                    //OnPropertyChanged("ItemVatType");
                }
            }
        }

        private int _OtherItemApplicationOn;
        public int OtherItemApplicationOn
        {
            get
            {
                return _OtherItemApplicationOn;
            }
            set
            {
                if (value != _OtherItemApplicationOn)
                {
                    _OtherItemApplicationOn = value;
                    //OnPropertyChanged("ItemVatApplicationOn");
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
                    //OnPropertyChanged("SingleQuantity");
                    //OnPropertyChanged("RequiredQuantity");

                }
            }
        }

        //private float _ConversionFactor = 1;
        //public float ConversionFactor
        //{
        //    get { return _ConversionFactor; }
        //    set
        //    {
        //        if (_ConversionFactor != value)
        //        {
        //            _ConversionFactor = value;
        //           // OnPropertyChanged("ConversionFactor");
        //        }
        //    }
        //}

        private string _UOM;
        public string UOM
        {
            get { return _UOM; }
            set
            {
                if (_UOM != value)
                {
                    _UOM = value;
                    //OnPropertyChanged("UOM");
                }
            }
        }

        private string _TransUOM;
        public string TransUOM
        {
            get { return _TransUOM; }
            set
            {
                if (_TransUOM != value)
                {
                    _TransUOM = value;
                    //OnPropertyChanged("TransUOM");
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
                    //OnPropertyChanged("UOMID");
                }
            }
        }

        private string _SUOM;
        public string SUOM
        {
            get
            {
                return _SUOM;
            }
            set
            {
                if (value != _SUOM)
                {
                    _SUOM = value;
                    //OnPropertyChanged("SUOM");
                }
            }
        }

        private float _StockConversionFactor;
        public float StockConversionFactor
        {
            get { return _StockConversionFactor; }
            set
            {
                if (_StockConversionFactor != value)
                {
                    _StockConversionFactor = value;
                    //OnPropertyChanged("StockConversionFactor");
                }
            }
        }

        private long _SUOMID;
        public long SUOMID
        {
            get { return _SUOMID; }
            set
            {
                if (_SUOMID != value)
                {
                    _SUOMID = value;
                    //OnPropertyChanged("SUOMID");
                }
            }
        }

        private double _StockingQuantity;
        public double StockingQuantity
        {
            get { return _StockingQuantity; }
            set
            {
                if (_StockingQuantity != value)
                {
                    _StockingQuantity = value;
                    //OnPropertyChanged("StockingQuantity");
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
                    // OnPropertyChanged("SelectedUOM");
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
                    //  OnPropertyChanged("UOMConversionList");
                }
            }

        }

        private long _PUOMID;
        public long PUOMID
        {
            get { return _PUOMID; }
            set
            {
                if (_PUOMID != value)
                {
                    _PUOMID = value;
                    //OnPropertyChanged("PUOMID");
                }
            }
        }

        private long _BaseUOMID;
        public long BaseUOMID
        {
            get { return _BaseUOMID; }
            set
            {
                if (_BaseUOMID != value)
                {
                    _BaseUOMID = value;
                    //OnPropertyChanged("BaseUOMID");
                }
            }
        }


        private long _SellingUOMID;
        public long SellingUOMID
        {
            get { return _SellingUOMID; }
            set
            {
                if (_SellingUOMID != value)
                {
                    _SellingUOMID = value;
                    // OnPropertyChanged("SellingUOMID");
                }
            }
        }

        private string _BaseUOM;
        public string BaseUOM
        {
            get { return _BaseUOM; }
            set
            {
                if (_BaseUOM != value)
                {
                    _BaseUOM = value;
                    //OnPropertyChanged("BaseUOM");
                }
            }
        }


        private string _SellingUOM;
        public string SellingUOM
        {
            get { return _SellingUOM; }
            set
            {
                if (_SellingUOM != value)
                {
                    _SellingUOM = value;
                    //OnPropertyChanged("SellingUOM");
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
                    //OnPropertyChanged("BaseConversionFactor");
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
                    //OnPropertyChanged("BaseQuantity");
                    //OnPropertyChanged("SingleQuantity");
                    //OnPropertyChanged("Quantity");
                    //OnPropertyChanged("Amount");
                    //OnPropertyChanged("VATAmount");
                    //OnPropertyChanged("NetAmount");



                }
            }
        }

        private float _BaseRate;
        public float BaseRate
        {
            get { return _BaseRate; }
            set
            {
                if (_BaseRate != value)
                {
                    _BaseRate = value;
                    //OnPropertyChanged("BaseRate");
                }
            }
        }


        private float _BaseMRP;
        public float BaseMRP
        {
            get
            {
                _BaseMRP = (float)Math.Round((decimal)_BaseMRP, 2);
                return _BaseMRP;
            }
            set
            {
                if (value != _BaseMRP)
                {
                    _BaseMRP = value;
                    // OnPropertyChanged("BaseMRP");
                }
            }
        }

        private float _StockingToBaseCF;
        public float StockingToBaseCF
        {
            get
            {
                return _StockingToBaseCF;
            }
            set
            {
                if (value != _StockingToBaseCF)
                {
                    _StockingToBaseCF = value;
                    //OnPropertyChanged("StockingToBaseCF");
                }
            }
        }
        private float _PurchaseToBaseCF;
        public float PurchaseToBaseCF
        {
            get
            {
                return _PurchaseToBaseCF;
            }
            set
            {
                if (value != _PurchaseToBaseCF)
                {
                    _PurchaseToBaseCF = value;
                    //OnPropertyChanged("PurchaseToBaseCF");
                }
            }
        }

        # endregion


        public decimal ItemVATPercentage { get; set; }
        public int GRNItemVatApplicationOn { get; set; }
        public int GRNItemVatType { get; set; }

        public decimal ItemTaxPercentage { get; set; }
        public int OtherGRNItemTaxApplicationOn { get; set; }
        public int OtherGRNItemTaxType { get; set; }
        //Addde By Bhushanp For GST 22062017
        private decimal _SGSTPercent;
        public decimal SGSTPercent
        {
            get
            {
                return _SGSTPercent;
            }
            set
            {
                if (value != _SGSTPercent)
                {
                    _SGSTPercent = value;
                    OnPropertyChanged("SGSTPercent");
                }
            }
        }
        private decimal _CGSTPercent;
        public decimal CGSTPercent
        {
            get
            {
                return _CGSTPercent;
            }
            set
            {
                if (value != _CGSTPercent)
                {
                    _CGSTPercent = value;
                    OnPropertyChanged("CGSTPercent");
                }
            }
        }
        private decimal _IGSTPercent;
        public decimal IGSTPercent
        {
            get
            {
                return _IGSTPercent;
            }
            set
            {
                if (value != _IGSTPercent)
                {
                    _IGSTPercent = value;
                    OnPropertyChanged("IGSTPercent");
                }
            }
        }

        private string _HSNCodes;

        public string HSNCodes
        {
            get { return _HSNCodes; }
            set { _HSNCodes = value; }
        }

        private int _SGSTVatType;
        public int SGSTVatType
        {
            get
            {
                return _SGSTVatType;
            }
            set
            {
                if (value != _SGSTVatType)
                {
                    _SGSTVatType = value;
                    //  OnPropertyChanged("ItemVatType");
                }
            }
        }

        private int _SGSTVatApplicationOn;
        public int SGSTVatApplicationOn
        {
            get
            {
                return _SGSTVatApplicationOn;
            }
            set
            {
                if (value != _SGSTVatApplicationOn)
                {
                    _SGSTVatApplicationOn = value;
                    //OnPropertyChanged("ItemVatApplicationOn");
                }
            }
        }

        private int _CGSTVatType;
        public int CGSTVatType
        {
            get
            {
                return _CGSTVatType;
            }
            set
            {
                if (value != _CGSTVatType)
                {
                    _CGSTVatType = value;
                    //  OnPropertyChanged("ItemVatType");
                }
            }
        }

        private int _CGSTVatApplicationOn;
        public int CGSTVatApplicationOn
        {
            get
            {
                return _CGSTVatApplicationOn;
            }
            set
            {
                if (value != _CGSTVatApplicationOn)
                {
                    _CGSTVatApplicationOn = value;
                    //OnPropertyChanged("ItemVatApplicationOn");
                }
            }
        }

        private int _IGSTVatType;
        public int IGSTVatType
        {
            get
            {
                return _IGSTVatType;
            }
            set
            {
                if (value != _IGSTVatType)
                {
                    _IGSTVatType = value;
                    //  OnPropertyChanged("ItemVatType");
                }
            }
        }

        private int _IGSTVatApplicationOn;
        public int IGSTVatApplicationOn
        {
            get
            {
                return _IGSTVatApplicationOn;
            }
            set
            {
                if (value != _IGSTVatApplicationOn)
                {
                    _IGSTVatApplicationOn = value;
                    //OnPropertyChanged("ItemVatApplicationOn");
                }
            }
        }

        private decimal _TotalBatchAvailableStock;
        public decimal TotalBatchAvailableStock
        {
            get
            {
                return _TotalBatchAvailableStock;
            }
            set
            {
                if (value != _TotalBatchAvailableStock)
                {
                    _TotalBatchAvailableStock = value;
                    //OnPropertyChanged("ItemVatApplicationOn");
                }
            }
        }
    }
}
