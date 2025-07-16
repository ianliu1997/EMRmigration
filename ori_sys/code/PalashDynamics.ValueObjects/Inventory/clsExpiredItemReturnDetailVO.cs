using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsExpiredItemReturnDetailVO : INotifyPropertyChanged, IValueObject
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

        private long? _ItemExpiryReturnID;
        public long? ItemExpiryReturnID
        {
            get { return _ItemExpiryReturnID; }
            set
            {
                if (_ItemExpiryReturnID != value)
                {
                    _ItemExpiryReturnID = value;
                    OnPropertyChanged("ItemExpiryReturnID");
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

        private long? _BatchID;
        public long? BatchID
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

        private long _StockUOMID;
        public long StockUOMID
        {
            get { return _StockUOMID; }
            set
            {
                if (_StockUOMID != value)
                {
                    _StockUOMID = value;
                    OnPropertyChanged("StockUOMID");
                }
            }
        }

        private long _TransactionUOMID;
        public long TransactionUOMID
        {
            get { return _TransactionUOMID; }
            set
            {
                if (_TransactionUOMID != value)
                {
                    _TransactionUOMID = value;
                    OnPropertyChanged("TransactionUOMID");
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

        private DateTime? _BatchExpiryDate;
        public DateTime? BatchExpiryDate
        {
            get { return _BatchExpiryDate; }
            set
            {
                if (_BatchExpiryDate != value)
                {
                    _BatchExpiryDate = value;
                    OnPropertyChanged("BatchExpiryDate");
                }
            }
        }

        private double? _Conversion;
        public double? Conversion
        {
            get { return _Conversion; }
            set
            {
                if (_Conversion != value)
                {
                    _Conversion = value;
                    OnPropertyChanged("Conversion");
                }
            }
        }

        private double _ReturnQty;
        public double ReturnQty
        {
            get { return _ReturnQty; }
            set
            {
                if (_ReturnQty != value)
                {
                    _ReturnQty = value;
                    OnPropertyChanged("ReturnQty");
                    OnPropertyChanged("PurchaseRate");
                    OnPropertyChanged("TotalPurchaseRate");
                    OnPropertyChanged("MRP");
                    OnPropertyChanged("MRPAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("NetAmount");

                    //OnPropertyChanged("Amount");
                    //OnPropertyChanged("VATPercentage");
                    //OnPropertyChanged("TaxPercentage");
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
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");



                }
            }
        }
        private float _MainRate;
        public float MainRate
        {
            get { return _MainRate; }
            set
            {
                if (_MainRate != value)
                {
                    _MainRate = value;
                    OnPropertyChanged("MainRate");
                }
            }
        }
        private float _MainMRP;
        public float MainMRP
        {
            get
            {
                _MainMRP = (float)Math.Round(_MainMRP, 2);
                return _MainMRP;
            }
            set
            {
                if (value != _MainMRP)
                {
                    _MainMRP = value;
                    OnPropertyChanged("MainMRP");
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

        private float _Re_Order;
        public float Re_Order
        {
            get { return _Re_Order; }
            set
            {
                if (_Re_Order != value)
                {
                    _Re_Order = value;
                    OnPropertyChanged("Re_Order");
                }
            }
        }

        private float _BaseCF;
        public float BaseCF
        {
            get { return _BaseCF; }
            set
            {
                if (_BaseCF != value)
                {
                    _BaseCF = value;
                    OnPropertyChanged("BaseCF");
                }
            }
        }

        private float _StockCF;
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
        private double _MRP;
        public double MRP
        {
            get { return _MRP; }
            set
            {
                if (_MRP != value)
                {
                    _MRP = value;
                    OnPropertyChanged("MRP");
                    OnPropertyChanged("MRPAmount");
                }
            }
        }
        private double _PurchaseRate;
        public double PurchaseRate
        {
            get { return _PurchaseRate; }
            set
            {
                if (_PurchaseRate != value)
                {
                    _PurchaseRate = value;
                    OnPropertyChanged("PurchaseRate");
                    OnPropertyChanged("TotalPurchaseRate");
                }
            }
        }
        private double _TotalPurchaseRate;
        public double TotalPurchaseRate
        {
            get { return _TotalPurchaseRate = _PurchaseRate * _ReturnQty; }
            set
            {
                if (_TotalPurchaseRate != value)
                {
                    _TotalPurchaseRate = value;
                    OnPropertyChanged("TotalPurchaseRate");
                }
            }
        }
        private double _ReturnRate;
        public double ReturnRate
        {
            get { return _ReturnRate; }
            set
            {
                if (_ReturnRate != value)
                {
                    _ReturnRate = value;
                    OnPropertyChanged("ReturnRate");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("VATPercentage");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("TaxPercentage");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _Amount;
        public double Amount
        {
            get { return _Amount = _ReturnRate * _ReturnQty; }
            set
            {
                if (_Amount != value)
                {
                    _Amount = value;
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("VATPercentage");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _MRPAmount;
        public double MRPAmount
        {
            get
            {
                //if (_GRNItemVatApplicationOn == 1)
                //{
                //    return _Amount = _Rate * _Quantity;
                //}
                //else
                //{
                return _MRPAmount = _MRP * _ReturnQty;
                //}
            }
            set
            {
                if (_MRPAmount != value)
                {
                    _MRPAmount = value;
                    OnPropertyChanged("MRPAmount");
                    //OnPropertyChanged("MRP");
                    ////#region Added by Pallavi
                    ////OnPropertyChanged("CDiscountAmount");
                    ////OnPropertyChanged("SchDiscountAmount");
                    //OnPropertyChanged("VATAmount");
                    //OnPropertyChanged("TaxAmount");
                    ////#endregion
                    //OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _DiscountPercentage;
        public double DiscountPercentage
        {
            get { return _DiscountPercentage; }
            set
            {
                if (_DiscountPercentage != value)
                {
                    _DiscountPercentage = value;
                    OnPropertyChanged("DiscountPercentage");
                }
            }
        }

        private double _DiscountAmount;
        public double DiscountAmount
        {
            get { return _DiscountAmount; }
            set
            {
                if (_DiscountAmount != value)
                {
                    _DiscountAmount = value;
                    OnPropertyChanged("DiscountAmount");
                }
            }
        }

        private double _OctroiAmount;
        public double OctroiAmount
        {
            get { return _OctroiAmount; }
            set
            {
                if (_OctroiAmount != value)
                {
                    _OctroiAmount = value;
                    OnPropertyChanged("OctroiAmount");
                }
            }
        }

        private double _VATPercentage;
        public double VATPercentage
        {
            get { return _VATPercentage; }
            set
            {
                if (_VATPercentage != value)
                {
                    _VATPercentage = value;
                    OnPropertyChanged("VATPercentage");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        //private double _VATAmount;
        //public double VATAmount
        //{
        //    get
        //    {
        //        if (_VATPercentage != 0)
        //        {
        //            return _VATAmount = ((_Amount * _VATPercentage) / 100);
        //        }
        //        return _VATAmount;
        //    }
        //    set
        //    {
        //        if (_VATAmount != value)
        //        {
        //            _VATAmount = value;
        //            OnPropertyChanged("VATAmount");
        //            OnPropertyChanged("VATPercentage");
        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}

        private double _VATAmount;
        public double VATAmount
        {
            get
            {
                ////if (_VATPercent != 0)
                ////{
                ////    return _VATAmount = ((_Amount * _VATPercent) / 100);
                ////}
                if (_VATPercentage != 0)
                {
                    if (_GRNItemVatType == 2)
                    {
                        if (_GRNItemVatApplicationOn == 1)
                        {
                            //_VATAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _VATPercent) / 100);
                            _VATAmount = (((_TotalPurchaseRate) * _VATPercentage) / 100);
                            return _VATAmount;
                        }
                        else
                        {
                            ////_VATAmount = (((_Amount) * _VATPercent) / 100);
                            //_VATAmount = (((_Amount - _CDiscountAmount - _SchDiscountAmount) * _VATPercent) / 100);
                            _VATAmount = (((_MRPAmount) * _VATPercentage) / 100);
                            return _VATAmount;
                        }

                    }
                    else if (_GRNItemVatType == 1)
                    {
                        if (_GRNItemVatApplicationOn == 1)
                        {
                            ////double calculation = _CostRate - _CDiscountAmount - _SchDiscountAmount;
                            double calculation = _TotalPurchaseRate;
                            _VATAmount = ((calculation) / (100 + _VATPercentage) * 100);
                            _VATAmount = calculation - _VATAmount;
                            return _VATAmount;
                        }
                        else
                        {
                            ////////_VATAmount = ((_Amount) / (100 + _VATPercent) * 100);
                            //////_VATAmount = ((_Amount - _CDiscountAmount - _SchDiscountAmount) / (100 + _VATPercent) * 100);
                            //////_VATAmount = _Amount - _VATAmount;
                            //////return _VATAmount;

                            //double calculation2 = ((_Amount) / (100 + _VATPercentage) * 100);
                            double calculation2 = ((_MRPAmount) / (100 + _VATPercentage) * 100);
                            _VATAmount = (calculation2 * (_VATPercentage / 100));  //_VATAmount = ((calculation2 - CDiscountAmount - _SchDiscountAmount) * (_VATPercent / 100));
                            //_VATAmount = _Amount - _VATAmount;
                            return _VATAmount;
                        }
                    }
                    else
                    {
                        //_VATAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _VATPercentage) / 100);
                        _VATAmount = (((_TotalPurchaseRate) * _VATPercentage) / 100);
                        return _VATAmount;
                    }
                }
                return _VATAmount = 0;
            }
            set
            {
                if (_VATAmount != value)
                {
                    if (value < 0)
                        value = 0;

                    _VATAmount = value;
                    //_VATPercentage = 0;
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("VATPercentage");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _TaxPercentage;  // Duplicate Prop.
        public double TaxPercentage
        {
            get { return _TaxPercentage; }
            set
            {
                if (_TaxPercentage != value)
                {
                    _TaxPercentage = value;
                    OnPropertyChanged("TaxPercentage");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }


        //private double _TaxAmount;
        //public double TaxAmount
        //{
        //    get
        //    {

        //        if (_TaxPercentage != 0)
        //        {
        //            return _TaxAmount = ((_Amount * _TaxPercentage) / 100);
        //        }
        //        else
        //            return _TaxAmount;

        //    }
        //    set
        //    {
        //        if (_TaxAmount != value)
        //        {
        //            _TaxAmount = value;
        //            OnPropertyChanged("TaxAmount");
        //        }
        //    }
        //}

        private double _ItemTax;
        public double ItemTax
        {
            get { return _ItemTax; }
            set
            {
                if (_ItemTax != value)
                {
                    if (value < 0)
                        value = 0;

                    _ItemTax = value;
                    OnPropertyChanged("ItemTax");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _TaxAmount;
        public double TaxAmount
        {
            get
            {
                if (_ReturnQty > 0)   //if (_Quantity > 0)
                {
                    if (_ItemTax > 0)
                    {
                        if (_OtherGRNItemTaxType == 2)
                        {
                            if (_OtherGRNItemTaxApplicationOn == 1)
                            {
                                _TaxAmount = (((_TotalPurchaseRate) * _ItemTax) / 100);
                                return _TaxAmount;
                            }
                            else
                            {
                                _TaxAmount = (((_MRPAmount) * _ItemTax) / 100);
                                return _TaxAmount;
                            }
                        }
                        else if (_OtherGRNItemTaxType == 1)
                        {
                            if (_OtherGRNItemTaxApplicationOn == 1)
                            {
                                double calculation = _TotalPurchaseRate;
                                _TaxAmount = ((calculation) / (100 + _ItemTax) * 100);
                                _TaxAmount = calculation - _TaxAmount;
                                return _TaxAmount;
                            }
                            else
                            {
                                double calculation2 = ((_MRPAmount) / (100 + _ItemTax) * 100);
                                _TaxAmount = (calculation2 * (_ItemTax / 100)); //_TaxAmount = ((calculation2 - CDiscountAmount - _SchDiscountAmount) * (_ItemTax / 100)); Commented by ashish Z. on dated 140416
                                return _TaxAmount;
                            }
                        }
                        else
                        {
                            _TaxAmount = (((_TotalPurchaseRate) * _ItemTax) / 100);
                            return _TaxAmount;
                        }
                    }
                    return _TaxAmount = 0;
                }
                return _TaxAmount;
            }
            set
            {
                if (_TaxAmount != value)
                {
                    if (value < 0)
                        value = 0;
                    _TaxAmount = value;
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

        //private double _NetAmount;
        //public double NetAmount
        //{
        //    get { return _NetAmount = _Amount + _VATAmount + TaxAmount; }
        //    set
        //    {
        //        if (_NetAmount != value)
        //        {
        //            _NetAmount = value;
        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}

        private double _NetAmount;
        public double NetAmount
        {
            get
            {
                if (_VATPercentage != 0)
                {
                    //check type inclusive or exclusive
                    if (_GRNItemVatType == 2)
                    {
                        ////if (_GRNItemVatApplicationOn == 1)
                        ////{
                        //// _NetAmount = _Amount + _VATAmount - _SchDiscountAmount - _CDiscountAmount;
                        #region Added By Pallavi
                        ////double itemTaxAmount = ((_Amount * ItemTax) / 100);
                        //_NetAmount = (_CostRate - _SchDiscountAmount - _CDiscountAmount) + _VATAmount + _TaxAmount;
                        _NetAmount = (_TotalPurchaseRate) + _VATAmount + _TaxAmount;
                        #endregion
                        return _NetAmount;
                        ////}
                        ////else
                        ////{
                        ////    _NetAmount = _Amount - _SchDiscountAmount - _CDiscountAmount + _VATAmount + TaxAmount;
                        ////    return _NetAmount;
                        ////}
                    }
                    else
                    {
                        ////if (_GRNItemVatApplicationOn == 1)
                        ////{
                        ////_NetAmount = _CostRate - _SchDiscountAmount - _CDiscountAmount;
                        ////return _NetAmount;
                        ////}
                        ////else
                        ////{
                        ////    _NetAmount = _Amount - _SchDiscountAmount - _CDiscountAmount;
                        ////    return _NetAmount;
                        ////}

                        if (_GRNItemVatApplicationOn == 1) //for Applicable on CP
                        {
                            //return _NetAmount = _CostRate - _SchDiscountAmount - _CDiscountAmount;
                            return _NetAmount = _TotalPurchaseRate;
                        }
                        else  //for Applicable on MRP
                        {
                            //return _NetAmount = (_CostRate - _SchDiscountAmount - _CDiscountAmount) + _VATAmount + _TaxAmount;
                            return _NetAmount = (_TotalPurchaseRate) + _VATAmount + _TaxAmount;
                        }

                        ////_NetAmount = (_CostRate - _SchDiscountAmount - _CDiscountAmount);
                        ////return _NetAmount;
                    }
                }
                else
                {
                    if (_OtherGRNItemTaxType == 2)
                    {
                        ////if (_OtherGRNItemTaxApplicationOn == 1)
                        ////{
                        //// _NetAmount = _Amount + _VATAmount - _SchDiscountAmount - _CDiscountAmount;
                        #region Added By Pallavi
                        ////double itemTaxAmount = ((_Amount * ItemTax) / 100);
                        //_NetAmount = (_CostRate - _SchDiscountAmount - _CDiscountAmount) + _VATAmount + _TaxAmount;
                        _NetAmount = (_Amount) + _VATAmount + _TaxAmount;
                        #endregion
                        return _NetAmount;
                        ////}
                        ////else
                        ////{
                        ////    _NetAmount = _Amount - _SchDiscountAmount - _CDiscountAmount + _VATAmount + TaxAmount;
                        ////    return _NetAmount;
                        ////}
                    }
                    else
                    {
                        ////if (_OtherGRNItemTaxApplicationOn == 1)
                        ////{
                        ////_NetAmount = _CostRate - _SchDiscountAmount - _CDiscountAmount;
                        ////return _NetAmount;
                        ////}
                        ////else
                        ////{
                        ////    _NetAmount = _Amount - _SchDiscountAmount - _CDiscountAmount;
                        ////    return _NetAmount;
                        ////}

                        if (_GRNItemVatApplicationOn == 1) //for Applicable on CP
                        {
                            //return _NetAmount = _CostRate - _SchDiscountAmount - _CDiscountAmount;
                            return _NetAmount = _TotalPurchaseRate;
                        }
                        else  //for Applicable on MRP
                        {
                            //return _NetAmount = (_CostRate - _SchDiscountAmount - _CDiscountAmount) + _VATAmount + _TaxAmount;
                            return _NetAmount = (_TotalPurchaseRate) + _VATAmount + _TaxAmount;
                        }
                        ////_NetAmount = (_CostRate - _SchDiscountAmount - _CDiscountAmount);
                        ////return _NetAmount;
                    }
                }
            }
            set
            {
                if (_NetAmount != value)
                {
                    _NetAmount = value;
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _AbatedMRP;
        public double AbatedMRP
        {
            get
            {
                if (_GRNItemVatType == 2)      // Exclusive 
                {
                    return _AbatedMRP = (_MRP) - ((_MRP * _VATPercentage) / 100);
                }
                else if (_GRNItemVatType == 1)  // Inclusive 
                {
                    return _AbatedMRP = ((_MRP) / (_VATPercentage + 100)) * 100;
                }
                else
                {
                    return _AbatedMRP = (_MRP) - ((_MRP * _VATPercentage) / 100);
                }
            }
            set
            {
                if (_AbatedMRP != value)
                {
                    _AbatedMRP = value;
                    OnPropertyChanged("AbatedMRP");
                }
            }
        }

        public string UOM { get; set; }
        public Boolean Status { get; set; }
        public long StoreId { get; set; }
        public string Store { get; set; }
        public string StockUOM { get; set; }
        public Boolean IsSelect { get; set; }
        public string BaseUOM { get; set; }
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
        private String _AvailableStockUOM;
        public String AvailableStockUOM
        {
            get
            {
                return _AvailableStockUOM;
            }
            set
            {
                if (value != _AvailableStockUOM)
                {
                    _AvailableStockUOM = value;
                    OnPropertyChanged("AvailableStockUOM");
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

        #region For Quarantine Items (Expired, DOS)
        // Use For Vat/Tax Calculations

        private int _GRNItemVatType;
        public int GRNItemVatType
        {
            get { return _GRNItemVatType; }
            set
            {
                if (_GRNItemVatType != value)
                {
                    _GRNItemVatType = value;
                    OnPropertyChanged("GRNItemVatType");
                }
            }
        }

        private int _GRNItemVatApplicationOn;
        public int GRNItemVatApplicationOn
        {
            get { return _GRNItemVatApplicationOn; }
            set
            {
                if (_GRNItemVatApplicationOn != value)
                {
                    _GRNItemVatApplicationOn = value;
                    OnPropertyChanged("GRNItemVatApplicationOn");
                }
            }
        }

        private float _GRNItemVATPer;
        public float GRNItemVATPer
        {
            get { return _GRNItemVATPer; }
            set
            {
                if (_GRNItemVATPer != value)
                {
                    _GRNItemVATPer = value;
                    OnPropertyChanged("GRNItemVATPer");
                }
            }
        }

        private int _OtherGRNItemTaxType;
        public int OtherGRNItemTaxType
        {
            get { return _OtherGRNItemTaxType; }
            set
            {
                if (_OtherGRNItemTaxType != value)
                {
                    _OtherGRNItemTaxType = value;
                    OnPropertyChanged("OtherGRNItemTaxType");
                }
            }
        }

        private float _OtherGRNItemTaxPer;
        public float OtherGRNItemTaxPer
        {
            get { return _OtherGRNItemTaxPer; }
            set
            {
                if (_OtherGRNItemTaxPer != value)
                {
                    _OtherGRNItemTaxPer = value;
                    OnPropertyChanged("OtherGRNItemTaxPer");
                }
            }
        }

        private int _OtherGRNItemTaxApplicationOn;
        public int OtherGRNItemTaxApplicationOn
        {
            get { return _OtherGRNItemTaxApplicationOn; }
            set
            {
                if (_OtherGRNItemTaxApplicationOn != value)
                {
                    _OtherGRNItemTaxApplicationOn = value;
                    OnPropertyChanged("OtherGRNItemTaxApplicationOn");
                }
            }
        }

        private Boolean _InclusiveOfTax;
        public Boolean InclusiveOfTax
        {
            get
            {
                return _InclusiveOfTax;
            }
            set
            {
                if (value != _InclusiveOfTax)
                {
                    _InclusiveOfTax = value;
                    OnPropertyChanged("InclusiveOfTax");
                }
            }
        }

        #endregion

        private string _ReceivedNo;
        public string ReceivedNo
        {
            get { return _ReceivedNo; }
            set
            {
                if (_ReceivedNo != value)
                {
                    _ReceivedNo = value;
                    OnPropertyChanged("ReceivedNo");
                }
            }
        }

        private double _ReceivedQty;
        public double ReceivedQty
        {
            get { return _ReceivedQty; }
            set
            {
                if (_ReceivedQty != value)
                {
                    _ReceivedQty = value;
                    OnPropertyChanged("ReceivedQty");
                }
            }
        }

        private double _PendingQty;
        public double PendingQty
        {
            get { return _PendingQty; }
            set
            {
                if (_PendingQty != value)
                {
                    _PendingQty = value;
                    OnPropertyChanged("PendingQty");
                }
            }
        }

        private string _ReceivedQtyUOM;
        public string ReceivedQtyUOM
        {
            get { return _ReceivedQtyUOM; }
            set
            {
                if (_ReceivedQtyUOM != value)
                {
                    _ReceivedQtyUOM = value;
                    OnPropertyChanged("ReceivedQtyUOM");
                }
            }
        }

        public long ReceivedID { get; set; }
        public long ReceivedUnitID { get; set; }
        public long ReceivedDetailID { get; set; }
        public long ReceivedDetailUnitID { get; set; }

        private DateTime? _ReceivedDate;
        public DateTime? ReceivedDate
        {
            get { return _ReceivedDate; }
            set
            {
                if (_ReceivedDate != value)
                {
                    _ReceivedDate = value;
                    OnPropertyChanged("ReceivedDate");
                }
            }
        }

        private Boolean _IsItemBlock;
        public Boolean IsItemBlock
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

        #region Add properties by AniketK on 26Oct2018 to show PO ,GRN , Supplier details on expiry dashboard

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

        private double _BaseCPDashBoard;
        public double BaseCPDashBoard
        {
            get { return _BaseCPDashBoard; }
            set
            {
                if (_BaseCPDashBoard != value)
                {
                    _BaseCPDashBoard = value;
                    OnPropertyChanged("BaseCPDashBoard");
                }
            }
        }

        private double _BaseMRPDashBoard;
        public double BaseMRPDashBoard
        {
            get { return _BaseMRPDashBoard; }
            set
            {
                if (_BaseMRPDashBoard != value)
                {
                    _BaseMRPDashBoard = value;
                    OnPropertyChanged("BaseMRPDashBoard");
                }
            }
        }

        #endregion


        #region Added by Prashant Channe on 07/12/2018, to show ToalCP and TotalMRP details on expiry dashboard

        private double _TotalCP;
        public double TotalCP
        {
            get { return _TotalCP; }

            set 
            {
                if (_TotalCP != value)
                {
                    _TotalCP = value;
                    OnPropertyChanged("TotalCP");
                }
            }
        }

        private double _TotalMRP;
        public double TotalMRP
        {
            get { return _TotalMRP; }

            set
            {
                if (_TotalMRP != value)
                {
                    _TotalMRP = value;
                    OnPropertyChanged("TotalMRP");
                }
            }
        }

        #endregion

    }
}
