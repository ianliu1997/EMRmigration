

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{
    
    public class clsGetItemListByReturnReceivedIdBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemListByReturnReceivedIdBizAction";
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

        public List<clsReceivedItemAgainstReturnDetailsVO> ItemList { get; set; }
        public long? ReceivedId { get; set; }
        public long? ReceivedUnitId { get; set; }
        public long? UnitId { get; set; }

    }

    public class clsReceivedItemAgainstReturnDetailsVO : INotifyPropertyChanged
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

        # region Conversion Factor

        private String _ReceivedUOM;
        public String ReceivedUOM
        {
            get
            {
                return _ReceivedUOM;
            }
            set
            {
                if (value != _ReceivedUOM)
                {
                    _ReceivedUOM = value;
                    OnPropertyChanged("ReceivedUOM");
                }
            }
        }

        private String _IssuedUOM;
        public String IssuedUOM
        {
            get
            {
                return _IssuedUOM;
            }
            set
            {
                if (value != _IssuedUOM)
                {
                    _IssuedUOM = value;
                    OnPropertyChanged("IssuedUOM");
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
                    OnPropertyChanged("RequiredQuantity");

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

        private string _ReturnUOM;
        public string ReturnUOM
        {
            get { return _ReturnUOM; }
            set
            {
                if (_ReturnUOM != value)
                {
                    _ReturnUOM = value;
                    OnPropertyChanged("ReturnUOM");
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

        private Boolean? _IsChecked = false;
        public Boolean? IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                if (value != _IsChecked)
                {
                    _IsChecked = value;
                    OnPropertyChanged("IsChecked");
                }
            }
        }

        private Boolean? _IsEnable;
        public Boolean? IsEnable
        {
            get
            {
               // if (_AvailableStock == 0 || _AvailableStock == null || _ExpiryDate < DateTime.Now || _ExpiryDate == null)
                    //_IsEnable = false;
               // else
                  //  _IsEnable = true;

                return _IsEnable;
            }
            set
            {
                if (value != _IsEnable)
                {
                    _IsEnable = value;
                    OnPropertyChanged("IsEnable");
                }
            }
        }





        private long? _ItemId;
        public long? ItemId
        {
            get
            {
                return _ItemId;
            }
            set
            {
                if (value != _ItemId)
                {
                    _ItemId = value;
                    OnPropertyChanged("ItemId");
                }
            }
        }

        private long? _ReceiveDetailsID;
        public long? ReceiveDetailsID
        {
            get
            {
                return _ReceiveDetailsID;
            }
            set
            {
                if (value != _ReceiveDetailsID)
                {
                    _ReceiveDetailsID = value;
                    OnPropertyChanged("ReceiveDetailsID");
                }
            }
        }    
        public bool IsIndent { get; set; }
        private float _ConversionFactor;
        public float ConversionFactor
        {
            get
            {
                return _ConversionFactor;
            }
            set
            {
                if (value != _ConversionFactor)
                {
                    _ConversionFactor = value;
                    OnPropertyChanged("ConversionFactor");
                }
            }
        }
        private long? _ReturnItemDetailsID;
        public long? ReturnItemDetailsID
        {
            get
            {
                return _ReturnItemDetailsID;
            }
            set
            {
                if (value != _ReturnItemDetailsID)
                {
                    _ReturnItemDetailsID = value;
                    OnPropertyChanged("ReturnItemDetailsID");
                }
            }
        }


        private String _ItemCode;
        public String ItemCode
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

        private String _ItemName;
        public String ItemName
        {
            get
            {
                return _ItemName;
            }
            set
            {
                if (value != _ItemName)
                {
                    _ItemName = value;
                    OnPropertyChanged("ItemName");
                }
            }
        }

        private long? _BatchId;
        public long? BatchId
        {
            get
            {
                return _BatchId;
            }
            set
            {
                if (value != _BatchId)
                {
                    _BatchId = value;
                    OnPropertyChanged("BatchId");
                }
            }
        }


        private String _BatchCode;
        public String BatchCode
        {
            get
            {
                return _BatchCode;
            }
            set
            {
                if (value != _BatchCode)
                {
                    _BatchCode = value;
                    OnPropertyChanged("BatchCode");
                }
            }
        }

        private DateTime? _ExpiryDate;
        public DateTime? ExpiryDate
        {
            get
            {
                return _ExpiryDate;
            }
            set
            {
                if (value != _ExpiryDate)
                {
                    _ExpiryDate = value;
                    OnPropertyChanged("ExpiryDate");
                    OnPropertyChanged("IsEnable");
                }
            }
        }


        private decimal? _ReturnQty;
        public decimal? ReturnQty
        {
            get
            {
                return _ReturnQty;
            }
            set
            {
                if (value != _ReturnQty)
                {
                    _ReturnQty = value;
                    OnPropertyChanged("ReturnQty");
                }
            }
        }

        private decimal? _BalanceQty;
        public decimal? BalanceQty
        {
            get
            {
                return _BalanceQty;
            }
            set
            {
                if (value != _BalanceQty)
                {
                    _BalanceQty = value;
                    OnPropertyChanged("BalanceQty");
                }
            }
        }

        private decimal _ReceivedQty;
        public decimal ReceivedQty
        {
            get
            {
                return _ReceivedQty;
            }
            set
            {
                if (value != _ReceivedQty)
                {
                    if (value < 0)
                        _ReceivedQty = 1;
                    else
                    _ReceivedQty = value;
                    OnPropertyChanged("ReceivedQty");
                    //ItemTotalAmount = ReceivedQty * PurchaseRate;
                    //ItemVATAmount = ItemTotalAmount * (ItemVATPercentage / 100);
                   // BalanceQty = ReceivedQty-ReceivedQty ;
                }
            }
        }


        private decimal? _PurchaseRate;
        public decimal? PurchaseRate
        {
            get
            {
                return _PurchaseRate;
            }
            set
            {
                if (value != _PurchaseRate)
                {
                    _PurchaseRate = value;
                    OnPropertyChanged("PurchaseRate");
                }
            }
        }


        private decimal? _MRP;
        public decimal? MRP
        {
            get
            {
                return _MRP;
            }
            set
            {
                if (value != _MRP)
                {
                    _MRP = value;
                    OnPropertyChanged("MRP");
                }
            }
        }


        private decimal? _ItemTotalAmount;
        public decimal? ItemTotalAmount
        {
            get
            {
                //decimal _temp = Convert.ToDecimal(BaseQuantity);
                //return _temp * PurchaseRate;
                return _ItemTotalAmount;// *PurchaseRate;
            }
            set
            {
                if (value != _ItemTotalAmount)
                {
                    _ItemTotalAmount = value;
                    OnPropertyChanged("ItemTotalAmount");
                }
            }
        }


      
        private decimal? _ItemVATPercentage;
        public decimal? ItemVATPercentage
        {
            get
            {
                return _ItemVATPercentage;
            }
            set
            {
                if (value != _ItemVATPercentage)
                {
                    _ItemVATPercentage = value;
                    OnPropertyChanged("ItemVATPercentage");
                }
            }
        }


        private decimal? _ItemVATAmount;
        public decimal? ItemVATAmount
        {
            get
            {
                return ItemTotalAmount * (ItemVATPercentage / 100);
            }
            set
            {
                if (value != _ItemVATAmount)
                {
                    _ItemVATAmount = value;
                    OnPropertyChanged("ItemVATAmount");
                }
            }
        }
        

    }
}

