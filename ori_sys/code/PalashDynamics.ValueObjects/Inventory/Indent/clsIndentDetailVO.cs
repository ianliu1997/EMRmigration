using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsIndentDetailVO : IValueObject, INotifyPropertyChanged
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
        private bool _IsIndent;
        public bool IsIndent
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

        private long? _IndentID;
        public long? IndentID
        {
            get { return _IndentID; }
            set
            {
                if (_IndentID != value)
                {
                    _IndentID = value;
                    OnPropertyChanged("IndentID");
                }
            }
        }

        public double PurchaseRate { get; set; }
        public double Amount { get { return PurchaseRate * RequiredQuantity; } }


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



        private double _RequiredQuantity;
        public double RequiredQuantity
        {
            get { return _RequiredQuantity; }
            set
            {
                if (_RequiredQuantity != value)
                {
                    _RequiredQuantity = value;
                    OnPropertyChanged("RequiredQuantity");
                }
            }
        }

        private double _PendingQuantity;
        public double PendingQuantity
        {
            get { return _PendingQuantity; }
            set
            {
                if (_PendingQuantity != value)
                {
                    _PendingQuantity = value;
                    OnPropertyChanged("PendingQuantity");
                }
            }
        }


        private double _PurchaseOrderQuantity;
        public double PurchaseOrderQuantity
        {
            get { return _PurchaseOrderQuantity; }
            set
            {
                if (_PendingQuantity != value)
                {
                    _PendingQuantity = value;
                    OnPropertyChanged("PurchaseOrderQuantity");
                }
            }
        }


        private double _QtyOnHand;
        public double QtyOnHand
        {
            get { return _QtyOnHand; }
            set
            {
                if (_QtyOnHand != value)
                {
                    _QtyOnHand = value;
                    OnPropertyChanged("QtyOnHand");
                }
            }
        }

        private bool _IsItemBlock=false;
        public bool IsItemBlock
        {
            get
            {
                return _IsItemBlock;
            }
            set
            {
                if (_IsItemBlock != value)
                {
                    _IsItemBlock = value;
                    OnPropertyChanged("IsItemBlock");
                }
            }
        }

        private decimal _TotalBatchAvailableStock;
        public decimal TotalBatchAvailableStock
        {
            get { return _TotalBatchAvailableStock; }
            set
            {
                if (_TotalBatchAvailableStock != value)
                {
                    _TotalBatchAvailableStock = value;
                    OnPropertyChanged("TotalBatchAvailableStock");
                }
            }
        }

        # region Conversion Factor

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
                    OnPropertyChanged("RequiredQuantity");
                    //_SingleQuantity = Convert.ToSingle(System.Math.Round(value, 1));
                    ////var length = ((int)value).ToString().Length;
                    //if (((int)value).ToString().Length > 5)
                    //{
                    //    _SingleQuantity = 1;
                    //    throw new ValidationException("Indent Quantity Length Should Not Be Greater Than 5 Digits.");
                    //    OnPropertyChanged("SingleQuantity");
                    //}
                    ////if (value.ToString().Length > 5)
                    ////{
                    ////    _SingleQuantity = 0;
                    ////    throw new ValidationException("Indent Quantity Length Should Not Be Greater Than 5 Digits.");
                    ////}
                    //else
                    //{
                    //    _SingleQuantity = value;
                    //    OnPropertyChanged("SingleQuantity");
                    //    OnPropertyChanged("RequiredQuantity");
                    //}
                }
            }
        }

        //private float _SingleQuantity;
        //public float SingleQuantity
        //{
        //    get
        //    {

        //        return _SingleQuantity;
        //    }
        //    set
        //    {
        //        if (_SingleQuantity != value)
        //        {


        //            _SingleQuantity = value;
        //            OnPropertyChanged("SingleQuantity");
        //            OnPropertyChanged("RequiredQuantity");

        //        }
        //    }
        //}

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

        private float _StockCF = 1;
        public float StockCF
        {
            get { return _StockCF; }
            set
            {
                if (_StockCF != value)
                {
                    _StockCF = value;
                    OnPropertyChanged("StockCF");
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
                    OnPropertyChanged("SUOM");
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
                    OnPropertyChanged("SUOMID");
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
                    OnPropertyChanged("StockingQuantity");
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

        private long _PUOMID;
        public long PUOMID
        {
            get { return _PUOMID; }
            set
            {
                if (_PUOMID != value)
                {
                    _PUOMID = value;
                    OnPropertyChanged("PUOMID");
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
                    OnPropertyChanged("BaseUOMID");
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
                    OnPropertyChanged("SellingUOMID");
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
                    OnPropertyChanged("BaseUOM");
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
                    OnPropertyChanged("SellingUOM");
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
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");



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
                    OnPropertyChanged("BaseRate");
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
                    OnPropertyChanged("BaseMRP");
                }
            }
        }

        # endregion

        public double PendingQtyFP { get; set; } //for only Front Pannel on 12042018

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
