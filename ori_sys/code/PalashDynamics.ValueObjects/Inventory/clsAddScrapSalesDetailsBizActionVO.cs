using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsAddScrapSalesDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return " PalashDynamics.BusinessLayer.Inventory.clsAddScrapSalesDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        public long ItemScrapSaleId { get; set; }
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private clsSrcapVO objItems = new clsSrcapVO();
        public clsSrcapVO ItemMatserDetail
        {
            get
            {
                return objItems;
            }
            set
            {
                objItems = value;

            }
        }
        

        private List<clsSrcapDetailsVO> objItemMaster = new List<clsSrcapDetailsVO>();
        public List<clsSrcapDetailsVO>  ItemsDetail
        {
            get
            {
                return objItemMaster;
            }
            set
            {
                objItemMaster = value;

            }
        }

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

        public bool IsForApproveClick { get; set; }

    }

    public class clsSrcapDetailsVO: INotifyPropertyChanged
    {
        
     public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        public long ScrapSalesItemID { get; set; }
        public long UnitId { get; set; }
     

        public string SUOM { get; set; }
        public string BatchCode { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }


        private double _SaleQty=1;
        public double SaleQty
        {
            get
            {
                //TotalAmount = _SaleQty * ScrapRate;
                 return _SaleQty;
            }
            set
            {
                if (value < 0)
                    _SaleQty = 1;
                else
                _SaleQty = value;

                OnPropertyChanged("SaleQty");
                OnPropertyChanged("Rate");
                OnPropertyChanged("TotalPurchaseRate");
                OnPropertyChanged("MRP");
                OnPropertyChanged("MRPAmount");
                OnPropertyChanged("VATAmount");
                OnPropertyChanged("TaxAmount");
                OnPropertyChanged("NetAmount");
            }
        }

        private double _AvailableStock = 1;
        public double AvailableStock
        {
            get
            {
                return _AvailableStock;
            }
            set
            {
                _AvailableStock = value;

                OnPropertyChanged("AvailableStock");

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

        

        private double _Rate;
        public double Rate
        {
            get
            {
                
                return _Rate;

            }
            set
            {
                _Rate = value;
                  OnPropertyChanged("Rate");
            }
        }

        private double _ScrapRate;
        public double ScrapRate
        {
            get
            {

                return _ScrapRate;

            }
            set
            {
                _ScrapRate = value;
                OnPropertyChanged("ScrapRate");
               // OnPropertyChanged("SaleQty");
                OnPropertyChanged("TotalAmount");
                OnPropertyChanged("Amount");
                OnPropertyChanged("VATPerc");
                OnPropertyChanged("VATAmount");
                OnPropertyChanged("TaxPercentage");
                OnPropertyChanged("TaxAmount");
                //OnPropertyChanged("TotalAmount");
                OnPropertyChanged("NetAmount");

                
            }

        }




        private double _TotalAmount;
        public double TotalAmount
        {
            get
            {
                _TotalAmount = _SaleQty * _ScrapRate;
                return _TotalAmount;

            }
            set
            {
                _TotalAmount = value;
                //VATAmount = (VATPerc * TotalAmount) / 100;
                //DiscAmt = (_DiscPerc * TotalAmount) / 100;
                ////TotalAmount = _SaleQty * ScrapRate;
                NetAmount = TotalAmount; //+ VATAmount;
                OnPropertyChanged("TotalAmount");
                //OnPropertyChanged("NetAmount");
            }

        }



        public double Amount { get; set; }

        //private double _Amount;
        //public double Amount
        //{
        //    get { return _Amount = _ScrapRate * _SaleQty; }
        //    set
        //    {
        //        if (_Amount != value)
        //        {
        //            _Amount = value;
        //            OnPropertyChanged("Amount");
        //            OnPropertyChanged("VATPercentage");
        //            OnPropertyChanged("VATAmount");
        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}

        //public double TaxPercentage { get; set; }

        private double _TaxPercentage;
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

        //public double TaxAmount { get; set; }

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
                if (_SaleQty > 0)  //if (_ReturnQty > 0) //if (_Quantity > 0)
                {
                    if (_ItemTax > 0)
                    {
                        if (_OtherGRNItemTaxType == 2)
                        {
                            if (_OtherGRNItemTaxApplicationOn == 1)
                            {
                                //_TaxAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _ItemTax) / 100);
                                _TaxAmount = (((_TotalPurchaseRate) * _ItemTax) / 100);
                                return _TaxAmount;
                            }
                            else
                            {
                                ////_TaxAmount = (((_Amount) * _ItemTax) / 100);
                                //_TaxAmount = (((_Amount - _CDiscountAmount - _SchDiscountAmount) * _ItemTax) / 100);
                                _TaxAmount = (((_MRPAmount) * _ItemTax) / 100);
                                return _TaxAmount;
                            }
                        }
                        else if (_OtherGRNItemTaxType == 1)
                        {
                            if (_OtherGRNItemTaxApplicationOn == 1)
                            {
                                //double calculation = _CostRate - _CDiscountAmount - _SchDiscountAmount;
                                double calculation = _TotalPurchaseRate;
                                _TaxAmount = ((calculation) / (100 + _ItemTax) * 100);
                                _TaxAmount = calculation - _TaxAmount;
                                return _TaxAmount;
                            }
                            else
                            {
                                //////_TaxAmount = ((_Amount) / (100 + _ItemTax) * 100);
                                ////_TaxAmount = ((_Amount - _CDiscountAmount - _SchDiscountAmount) / (100 + _ItemTax) * 100);
                                ////_TaxAmount = _Amount - _TaxAmount;
                                ////return _TaxAmount;

                                //double calculation2 = ((_Amount) / (100 + _ItemTax) * 100);
                                double calculation2 = ((_MRPAmount) / (100 + _ItemTax) * 100);
                                _TaxAmount = (calculation2 * (_ItemTax / 100)); //_TaxAmount = ((calculation2 - CDiscountAmount - _SchDiscountAmount) * (_ItemTax / 100)); Commented by ashish Z. on dated 140416
                                //_TaxAmount = _Amount - _TaxAmount;
                                return _TaxAmount;
                            }
                        }
                        else
                        {
                            //_TaxAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _ItemTax) / 100);
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

        //private double _NetAmount; 
        //public double NetAmount
        //{
        //    get
        //    {

        //        return _NetAmount;
        //    }
        //    set
        //    {
        //        _NetAmount = value;
        //        OnPropertyChanged("NetAmount");

        //    }

        //}


        private double _NetAmount;
        public double NetAmount
        {
            get
            {
                if (_VATPerc != 0)
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
                    return _AbatedMRP = (_MRP) - ((_MRP * _VATPerc) / 100);
                }
                else if (_GRNItemVatType == 1)  // Inclusive 
                {
                    return _AbatedMRP = ((_MRP) / (_VATPerc + 100)) * 100;
                }
                else
                {
                    return _AbatedMRP = (_MRP) - ((_MRP * _VATPerc) / 100);
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

        public string Remark { get; set; }
       
        public long ItemScrapSaleId { get; set; }
        public long ItemId { get; set; }
        public long BatchID { get; set; }
        public Double Currency { get; set; }

        private double _VATPerc;
        public double VATPerc 
        {
            get
            {
                return _VATPerc;

            }
            set
            {
                _VATPerc = value;
              // //VATAmount = (_VATPerc * TotalAmount) / 100;
              //// NetAmount = TotalAmount - DiscAmt + VATAmount;
              //  NetAmount = TotalAmount;
               OnPropertyChanged("VATPerc");
               OnPropertyChanged("VATAmount");
               OnPropertyChanged("NetAmount");
            }
        }


      
        //private double _VATAmount;
        //public double VATAmount
        //{
        //    get
        //    {

        //        return _VATAmount;

        //    }
        //    set
        //    {
        //        _VATAmount = value;
        //        OnPropertyChanged("VATAmount");
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
                if (_VATPerc != 0)
                {
                    if (_GRNItemVatType == 2)
                    {
                        if (_GRNItemVatApplicationOn == 1)
                        {
                            //_VATAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _VATPercent) / 100);
                            _VATAmount = (((_TotalPurchaseRate) * _VATPerc) / 100);
                            return _VATAmount;
                        }
                        else
                        {
                            ////_VATAmount = (((_Amount) * _VATPercent) / 100);
                            //_VATAmount = (((_Amount - _CDiscountAmount - _SchDiscountAmount) * _VATPercent) / 100);
                            _VATAmount = (((_MRPAmount) * _VATPerc) / 100);
                            return _VATAmount;
                        }

                    }
                    else if (_GRNItemVatType == 1)
                    {
                        if (_GRNItemVatApplicationOn == 1)
                        {
                            ////double calculation = _CostRate - _CDiscountAmount - _SchDiscountAmount;
                            double calculation = _TotalPurchaseRate;
                            _VATAmount = ((calculation) / (100 + _VATPerc) * 100);
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
                            double calculation2 = ((_MRPAmount) / (100 + _VATPerc) * 100);
                            _VATAmount = (calculation2 * (_VATPerc / 100));  //_VATAmount = ((calculation2 - CDiscountAmount - _SchDiscountAmount) * (_VATPercent / 100));
                            //_VATAmount = _Amount - _VATAmount;
                            return _VATAmount;
                        }
                    }
                    else
                    {
                        //_VATAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _VATPercentage) / 100);
                        _VATAmount = (((_TotalPurchaseRate) * _VATPerc) / 100);
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
                    OnPropertyChanged("VATPerc");
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
                return _MRPAmount = _MRP * _SaleQty;  //return _MRPAmount = _MRP * _ReturnQty;
                //}
            }
            set
            {
                if (_MRPAmount != value)
                {
                    _MRPAmount = value;
                    OnPropertyChanged("MRPAmount");
                    OnPropertyChanged("MRP");
                    //#region Added by Pallavi
                    //OnPropertyChanged("CDiscountAmount");
                    //OnPropertyChanged("SchDiscountAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("TaxAmount");
                    //#endregion
                    OnPropertyChanged("NetAmount");
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
                }
            }
        }

        private double _DiscAmt;
        public double DiscAmt
        {
            get
            {
                
                return _DiscAmt;

            }
            set
            {
                _DiscAmt = value;
                
                OnPropertyChanged("DiscAmt");
            }
        }

        private double _DiscPerc;
        public double DiscPerc
        {
            get
            {
               
                return _DiscPerc;

            }
            set
            {
                _DiscPerc = value;
                //DiscAmt = (_DiscPerc * TotalAmount) / 100;
                //NetAmount = TotalAmount + VATAmount - DiscAmt;
                OnPropertyChanged("DiscPerc");
            }
        }

        private float _AvailableStockInBase;
        public float AvailableStockInBase
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

        public Boolean PrimaryKeyViolationError { get; set; }
        public Boolean GeneralError { get; set; }
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

        public string StockUOM { get; set; }

        private double _TotalPurchaseRate;
        public double TotalPurchaseRate
        {
            get { return _TotalPurchaseRate = _Rate * _SaleQty; }
            set
            {
                if (_TotalPurchaseRate != value)
                {
                    _TotalPurchaseRate = value;
                    OnPropertyChanged("TotalPurchaseRate");
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

        private string _TransactionUOM;
        public string TransactionUOM
        {
            get { return _TransactionUOM; }
            set
            {
                if (_TransactionUOM != value)
                {
                    _TransactionUOM = value;
                    OnPropertyChanged("TransactionUOM");
                }
            }
        }

        private DateTime _ReceivedDate;
        public DateTime ReceivedDate
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

        public long StoreID { get; set; }

    }

    public class clsSrcapVO : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

     

        public long ScrapID { get; set; }

        public long SupplierID { get; set; }
        public string ScrapSaleNo { get; set; }
        public long StoreID { get; set; }
        public string StoreName { get; set; }
      
        public long UnitId { get; set; }
        public Boolean Status { get; set; }
        public long? AddUnitID { get; set; }

        public string SupplierName { get; set; }
        public string ModeOfTransport { get; set; }
        public string Remark { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Time { get; set; }
        public bool IsApproved { get; set; }

        public string IsApprovedStatus { get; set; }

        public Double TotalAmount { get; set; }
        public Double TotalTaxAmount { get; set; }
        public Double TotalVatAmount { get; set; }
        public Double TotalRate { get; set; }
        public Double TotalNetAmount { get; set; }
        
        public long? By { get; set; }
        public string On { get; set; }
        public DateTime? DateTime { get; set; }
        public string WindowsLoginName { get; set; }
        public int PaymentModeID { get; set; }


        //private PalashDynamics.ValueObjects.MaterPayModeList _PaymentModeID;
        //public PalashDynamics.ValueObjects.MaterPayModeList PaymentModeID
        //{
        //    get { return _PaymentModeID; }
        //    set
        //    {
        //        if (_PaymentModeID != value)
        //        {
        //            _PaymentModeID = value;
        //            OnPropertyChanged("PaymentModeID");
        //        }
        //    }
        //}

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

    }
}
