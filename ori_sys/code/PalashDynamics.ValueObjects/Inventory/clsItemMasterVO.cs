using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsItemMasterVO : INotifyPropertyChanged, IValueObject
    {
        public List<clsItemTaxVO> ItemTaxList;
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
        #region IValueObject
        public string ToXml()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Property Declaration
        private Boolean _IsSelected = false;
        public Boolean IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }


        private Boolean _IsEnable = false;
        public Boolean IsEnable
        {
            get
            {
                return _IsEnable;
            }
            set
            {
                _IsEnable = value;
                OnPropertyChanged("IsEnable");
            }
        }

        private Boolean _IsReadOnly = false;
        public Boolean IsReadOnly
        {
            get
            {
                return _IsReadOnly;
            }
            set
            {
                _IsReadOnly = value;
                OnPropertyChanged("IsReadOnly");
            }
        }
        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }
            set { _ID = value; }
        }
        private bool _IsABC = false;
        public bool IsABC
        {
            get { return _IsABC; }
            set
            {
                _IsABC = value;
                OnPropertyChanged("IsABC");
            }
        }
        private bool _IsFNS = false;
        public bool IsFNS
        {
            get { return _IsFNS; }
            set
            {
                _IsFNS = value;
                OnPropertyChanged("IsFNS");
            }
        }
        private bool _IsVED = false;
        public bool IsVED
        {
            get { return _IsVED; }
            set
            {
                _IsVED = value;
                OnPropertyChanged("IsVED");
            }
        }
        private long _StrengthUnitTypeID;
        public long StrengthUnitTypeID
        {
            get
            {
                return _StrengthUnitTypeID;
            }
            set
            {
                if (value != _StrengthUnitTypeID)
                {
                    _StrengthUnitTypeID = value;
                    OnPropertyChanged("StrengthUnitTypeID");
                }
            }
        }
        private long _UnitID;
        public long UnitID
        {
            get
            {
                return _UnitID;
            }
            set
            {
                if (value != _UnitID)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
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

        private string _BrandName;
        public string BrandName
        {
            get
            {
                return _BrandName;
            }
            set
            {
                if (value != _BrandName)
                {
                    _BrandName = value;
                    OnPropertyChanged("BrandName");
                }
            }
        }



        private string _Strength;
        public string Strength
        {
            get
            {
                return _Strength;
            }
            set
            {
                if (value != _Strength)
                {
                    _Strength = value;
                    OnPropertyChanged("BrandName");
                }
            }
        }



        private string _ItemName;
        public string ItemName
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
                    OnPropertyChanged("IsItemBlock");
                }
            }
        }

        //public bool IsItemBlock { get; set; }

        public long ItemExpiredInDays { get; set; }

        //private long _ItemExpiredInDays;
        //public long ItemExpiredInDays
        //{
        //    get
        //    {
        //        return _ItemExpiredInDays;
        //    }
        //    set
        //    {
        //        if (value != _ItemExpiredInDays)
        //        {
        //            if (_ItemExpiredInDays >= 365)
        //            {
        //                throw new ValidationException("Days Should Be Less Than 365 Days");
        //            }
        //            else
        //            {
        //                _ItemExpiredInDays = value;
        //                OnPropertyChanged("ItemExpiredInDays");
        //            }
        //        }
        //    }
        //}

        #region   //Added by Somnath
        private string _BatchCode;
        public string BatchCode
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

        private DateTime _ExpiryDate;
        public DateTime ExpiryDate
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
                }
            }
        }


        #endregion

        private long _MoleculeName;
        public long MoleculeName
        {
            get
            {
                return _MoleculeName;
            }
            set
            {
                if (value != _MoleculeName)
                {
                    _MoleculeName = value;
                    OnPropertyChanged("MoleculeName");
                }
            }
        }

        //Added By Pallavi
        private string _MoleculeNameString;
        public string MoleculeNameString
        {
            get
            {
                return _MoleculeNameString;
            }
            set
            {
                if (value != _MoleculeNameString)
                {
                    _MoleculeNameString = value;
                    OnPropertyChanged("MoleculeNameString");
                }
            }
        }
        //

        private long _ItemGroup;
        public long ItemGroup
        {
            get
            {
                return _ItemGroup;
            }
            set
            {
                if (value != _ItemGroup)
                {
                    _ItemGroup = value;
                    OnPropertyChanged("ItemGroup");
                }
            }
        }



        private string _ItemGroupString;
        public string ItemGroupString
        {
            get
            {
                return _ItemGroupString;
            }
            set
            {
                if (value != _ItemGroupString)
                {
                    _ItemGroupString = value;
                    OnPropertyChanged("ItemGroupString");
                }
            }
        }


        private long _ItemCategory;
        public long ItemCategory
        {
            get
            {
                return _ItemCategory;
            }
            set
            {
                if (value != _ItemCategory)
                {
                    _ItemCategory = value;
                    OnPropertyChanged("ItemCategory");
                }
            }
        }

        private string _ItemCategoryString;
        public string ItemCategoryString
        {
            get
            {
                return _ItemCategoryString;
            }
            set
            {
                if (value != _ItemCategoryString)
                {
                    _ItemCategoryString = value;
                    OnPropertyChanged("ItemCategoryString");
                }
            }
        }



        private long _DispencingType;
        public long DispencingType
        {
            get
            {
                return _DispencingType;
            }
            set
            {
                if (value != _DispencingType)
                {
                    _DispencingType = value;
                    OnPropertyChanged("DispencingType");
                }
            }
        }

        private string _DispencingTypeString;
        public string DispencingTypeString
        {
            get
            {
                return _DispencingTypeString;
            }
            set
            {
                if (value != _DispencingTypeString)
                {
                    _DispencingTypeString = value;
                    OnPropertyChanged("DispencingTypeString");
                }
            }
        }



        private long _StoreageType;
        public long StoreageType
        {
            get
            {
                return _StoreageType;
            }
            set
            {
                if (value != _StoreageType)
                {
                    _StoreageType = value;
                    OnPropertyChanged("StoreageType");
                }
            }
        }

        private long _ItemMarginID;
        public long ItemMarginID
        {
            get
            {
                return _ItemMarginID;
            }
            set
            {
                if (value != _ItemMarginID)
                {
                    _ItemMarginID = value;
                    OnPropertyChanged("ItemMarginID");
                }
            }
        }

        private string _ItemMargin;
        public string ItemMargin
        {
            get
            {
                return _ItemMargin;
            }
            set
            {
                if (value != _ItemMargin)
                {
                    _ItemMargin = value;
                    OnPropertyChanged("ItemMargin");
                }
            }
        }

        private long _ItemMovementID;
        public long ItemMovementID
        {
            get
            {
                return _ItemMovementID;
            }
            set
            {
                if (value != _ItemMovementID)
                {
                    _ItemMovementID = value;
                    OnPropertyChanged("ItemMovementID");
                }
            }
        }


        private string _ItemMovement;
        public string ItemMovement
        {
            get
            {
                return _ItemMovement;
            }
            set
            {
                if (value != _ItemMovement)
                {
                    _ItemMovement = value;
                    OnPropertyChanged("ItemMovement");
                }
            }
        }
        private decimal _StorageDegree;
        public decimal StorageDegree
        {
            get
            {
                return _StorageDegree;
            }
            set
            {
                if (value != _StorageDegree)
                {
                    _StorageDegree = value;
                    OnPropertyChanged("StorageDegree");
                }
            }
        }

        private decimal _HighestRetailPrice;
        public decimal HighestRetailPrice
        {
            get
            {
                return _HighestRetailPrice;
            }
            set
            {
                if (value != _HighestRetailPrice)
                {
                    _HighestRetailPrice = value;
                    OnPropertyChanged("HighestRetailPrice");
                }
            }
        }

        private decimal _Margin;
        public decimal Margin
        {
            get
            {
                return _Margin;
            }
            set
            {
                if (value != _Margin)
                {
                    _Margin = value;
                    OnPropertyChanged("Margin");
                }
            }
        }

        private decimal _MinStock;
        public decimal MinStock
        {
            get
            {
                return _MinStock;
            }
            set
            {
                if (value != _MinStock)
                {
                    _MinStock = value;
                    OnPropertyChanged("MinStock");
                }
            }
        }
        private long _PregClass;
        public long PregClass
        {
            get
            {
                return _PregClass;
            }
            set
            {
                if (value != _PregClass)
                {
                    _PregClass = value;
                    OnPropertyChanged("PregClass");
                }
            }
        }



        private long _TherClass;
        public long TherClass
        {
            get
            {
                return _TherClass;
            }
            set
            {
                if (value != _TherClass)
                {
                    _TherClass = value;
                    OnPropertyChanged("TherClass");
                }
            }
        }




        private long _MfgBy;
        public long MfgBy
        {
            get
            {
                return _MfgBy;
            }
            set
            {
                if (value != _MfgBy)
                {
                    _MfgBy = value;
                    OnPropertyChanged("MfgBy");
                }
            }
        }


        private string _MfgByString;
        public string MfgByString
        {
            get
            {
                return _MfgByString;
            }
            set
            {
                if (value != _MfgByString)
                {
                    _MfgByString = value;
                    OnPropertyChanged("MfgByString");
                }
            }
        }



        private long _MrkBy;
        public long MrkBy
        {
            get
            {
                return _MrkBy;
            }
            set
            {
                if (value != _MrkBy)
                {
                    _MrkBy = value;
                    OnPropertyChanged("MrkBy");
                }
            }
        }


        private string _MrkByString;
        public string MrkByString
        {
            get
            {
                return _MrkByString;
            }
            set
            {
                if (value != _MrkByString)
                {
                    _MrkByString = value;
                    OnPropertyChanged("MrkByString");
                }
            }
        }






        private long _PUM;
        public long PUM
        {
            get
            {
                return _PUM;
            }
            set
            {
                if (value != _PUM)
                {
                    _PUM = value;
                    OnPropertyChanged("PUM");
                }
            }
        }

        private string _PUMString;
        public string PUMString
        {
            get
            {
                return _PUMString;
            }
            set
            {
                if (value != _PUMString)
                {
                    _PUMString = value;
                    OnPropertyChanged("PUMString");
                }
            }
        }


        private long _SUM;
        public long SUM
        {
            get
            {
                return _SUM;
            }
            set
            {
                if (value != _SUM)
                {
                    _SUM = value;
                    OnPropertyChanged("SUM");
                }
            }
        }


        private string _PUOM;
        public string PUOM
        {
            get
            {
                return _PUOM;
            }
            set
            {
                if (value != _PUOM)
                {
                    _PUOM = value;
                    OnPropertyChanged("PUOM");
                }
            }
        }

        //added by Ashish Z. for Base and Selling UOM.
        private long _BaseUM;
        public long BaseUM
        {
            get
            {
                return _BaseUM;
            }
            set
            {
                if (value != _BaseUM)
                {
                    _BaseUM = value;
                    OnPropertyChanged("BaseUM");
                }
            }
        }

        private long _SellingUM;
        public long SellingUM
        {
            get
            {
                return _SellingUM;
            }
            set
            {
                if (value != _SellingUM)
                {
                    _SellingUM = value;
                    OnPropertyChanged("SellingUM");
                }
            }
        }

        private string _BaseUMString;
        public string BaseUMString
        {
            get
            {
                return _BaseUMString;
            }
            set
            {
                if (value != _BaseUMString)
                {
                    _BaseUMString = value;
                    OnPropertyChanged("BaseUMString");
                }
            }
        }

        private string _SellingUMString;
        public string SellingUMString
        {
            get
            {
                return _SellingUMString;
            }
            set
            {
                if (value != _SellingUMString)
                {
                    _SellingUMString = value;
                    OnPropertyChanged("SellingUMString");
                }
            }
        }

        private string _ConvFactStockBase = "1";
        public string ConvFactStockBase
        {
            get
            {
                return _ConvFactStockBase;
            }
            set
            {
                if (value != _ConvFactStockBase)
                {
                    _ConvFactStockBase = value;
                    OnPropertyChanged("ConvFactStockBase");
                }
            }
        }

        private string _ConvFactBaseSale = "1";
        public string ConvFactBaseSale
        {
            get
            {
                return _ConvFactBaseSale;
            }
            set
            {
                if (value != _ConvFactBaseSale)
                {
                    _ConvFactBaseSale = value;
                    OnPropertyChanged("ConvFactBaseSale");
                }
            }
        }
        //

        private string _StockUOM;
        public string StockUOM
        {
            get
            {
                return _StockUOM;
            }
            set
            {
                if (value != _StockUOM)
                {
                    _StockUOM = value;
                    OnPropertyChanged("StockUOM");
                }
            }
        }

        private string _PurchaseUOM;
        public string PurchaseUOM
        {
            get
            {
                return _PurchaseUOM;
            }
            set
            {
                if (value != _PurchaseUOM)
                {
                    _PurchaseUOM = value;
                    OnPropertyChanged("PurchaseUOM");
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

        private string _ConversionFactor = "1";
        public string ConversionFactor
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


        private long _Route;
        public long Route
        {
            get
            {
                return _Route;
            }
            set
            {
                if (value != _Route)
                {
                    _Route = value;
                    OnPropertyChanged("Route");
                }
            }
        }
        private decimal _PurchaseRate;
        public decimal PurchaseRate
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



        private decimal _DiscountPerc;
        public decimal DiscountPerc
        {
            get
            {
                if (_DiscountPerc > 0)
                    _DiscountAmt = ((_MRP * _DiscountPerc) / 100);
                else
                    _DiscountPerc = 0;
                //_DiscountPerc = Math.Round(_DiscountPerc, 2);
                return _DiscountPerc;
            }
            set
            {
                if (_DiscountPerc != value)
                {
                    if (value < 0)
                        value = 0;

                    _DiscountPerc = value;
                    if (_DiscountPerc > 0)
                    {
                        _DiscountAmt = ((_MRP * _DiscountPerc) / 100);
                        _NetAmt = _MRP - DiscountAmt - _DeductibleAmt;
                        _NetGross = _MRP - DiscountAmt;
                    }
                    else
                        _DiscountPerc = 0;
                    OnPropertyChanged("DiscountPerc");
                    OnPropertyChanged("DiscountAmt");
                    OnPropertyChanged("NetGross");
                    OnPropertyChanged("DeductiblePerc");
                    OnPropertyChanged("DeductibleAmt");
                    OnPropertyChanged("NetAmt");

                }
            }
        }


        private decimal _DiscountAmt;
        public decimal DiscountAmt
        {
            get
            {
                //if (_DiscountAmt > 0)
                //    _DiscountPerc = (_DiscountAmt * 100) / _MRP;
                //else
                //    _DiscountAmt = 0;

                return _DiscountAmt;
            }
            set
            {
                if (_DiscountAmt != value)
                {
                    if (value < 0)
                        value = 0;
                    _DiscountAmt = value;
                    //if (_DiscountAmt > 0)
                    //{
                    //    _DiscountPerc = (_DiscountAmt * 100) / _MRP;
                    //    _NetGross = _MRP - DiscountAmt;
                    //    _NetAmt = _MRP - DiscountAmt - _DeductibleAmt;

                    //}
                    //else
                    //_DiscountPerc = 0;
                    OnPropertyChanged("DiscountPerc");
                    OnPropertyChanged("DiscountAmt");
                    OnPropertyChanged("NetGross");
                    OnPropertyChanged("DeductiblePerc");
                    OnPropertyChanged("DeductibleAmt");
                    OnPropertyChanged("NetAmt");

                }
            }
        }

        private decimal _NetGross;
        public decimal NetGross
        {
            get
            {

                _NetGross = _MRP - DiscountAmt;
                return _NetGross;
            }
            set
            {
                if (_NetGross != value)
                {
                    _NetGross = value;

                    _NetAmt = _MRP - DiscountAmt - _DeductibleAmt;

                    OnPropertyChanged("NetGross");
                    OnPropertyChanged("DeductiblePerc");
                    OnPropertyChanged("DeductibleAmt");
                    OnPropertyChanged("NetAmt");
                }
            }
        }


        private decimal _MRP;
        public decimal MRP
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

        private decimal _AbatedMRP;
        public decimal AbatedMRP
        {
            get { return _AbatedMRP; }
            set
            {
                if (_AbatedMRP != value)
                {
                    _AbatedMRP = value;
                    OnPropertyChanged("AbatedMRP");
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
                    OnPropertyChanged("ItemVatType");
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
                    OnPropertyChanged("ItemVatApplicationOn");
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
                    OnPropertyChanged("ItemVatType");
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
                    OnPropertyChanged("ItemVatApplicationOn");
                }
            }
        }

        private bool _IsBeforeDiscount;
        public bool IsBeforeDiscount
        {

            get { return _IsBeforeDiscount; }
            set
            {
                if (value != _IsBeforeDiscount)
                {
                    _IsBeforeDiscount = value;
                    OnPropertyChanged("IsBeforeDiscount");
                }
            }
        }
        private decimal _NetAmt;
        public decimal NetAmt
        {
            get
            {

                _NetAmt = _MRP - DiscountAmt - _DeductibleAmt;
                return _NetAmt;
            }
            set
            {
                if (_NetAmt != value)
                {
                    _NetAmt = value;
                    OnPropertyChanged("NetAmt");
                }
            }
        }
        private decimal _DeductiblePerc;
        public decimal DeductiblePerc
        {
            get
            {
                if (_DeductiblePerc > 0)
                {

                    _DeductibleAmt = ((_MRP * _DeductiblePerc) / 100);

                }
                else
                    _DeductibleAmt = 0;
                //_DeductiblePerc = Math.Round(_DeductiblePerc, 2);
                return _DeductiblePerc;
            }
            set
            {
                if (_DeductiblePerc != value)
                {
                    if (value < 0)
                        value = 0;
                    _DeductiblePerc = value;
                    if (_DeductiblePerc > 0)
                    {

                        _DeductibleAmt = ((_MRP * _DeductiblePerc) / 100);

                        _NetAmt = _MRP - DiscountAmt - _DeductibleAmt;
                    }
                    else
                        _DeductibleAmt = 0;
                    OnPropertyChanged("DeductiblePerc");
                    OnPropertyChanged("DeductibleAmt");
                    OnPropertyChanged("NetAmt");

                }
            }
        }

        private decimal _DeductibleAmt;
        public decimal DeductibleAmt
        {
            get
            {
                //if (_DeductiblePerc > 0)
                //{

                //        _DeductibleAmt = ((_MRP * _DeductiblePerc) / 100);

                //}
                //else
                //    _DeductibleAmt = 0;               
                return _DeductibleAmt;
            }
            set
            {
                if (_DeductibleAmt != value)
                {
                    if (value < 0)
                        value = 0;
                    _DeductibleAmt = value;
                    //if (_DeductibleAmt > 0)
                    //{

                    //        _DeductiblePerc = (_DeductibleAmt * 100) / _MRP;

                    //    _NetAmt = _MRP - DiscountAmt - _DeductibleAmt;
                    //}
                    //else
                    //    _DiscountPerc = 0;
                    OnPropertyChanged("DeductibleAmt");
                    OnPropertyChanged("DeductiblePerc");
                    OnPropertyChanged("NetAmt");

                }
            }
        }

        private decimal _AvailableStock;
        public decimal AvailableStock
        {
            get
            {
                return _AvailableStock;
            }
            set
            {
                if (value != _AvailableStock)
                {
                    _AvailableStock = value;
                    OnPropertyChanged("AvailableStock");
                }
            }
        }

        private decimal _TotalBatchAvailableStock;   //total stock for itemSearch window only 
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
                    OnPropertyChanged("TotalBatchAvailableStock");
                }
            }
        }

        private decimal _VatPer;
        public decimal VatPer
        {
            get
            {
                return _VatPer;
            }
            set
            {
                if (value != _VatPer)
                {
                    _VatPer = value;
                    OnPropertyChanged("VatPer");
                }
            }
        }

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
                    OnPropertyChanged("ItemVatPer");
                }
            }
        }

        private double _DiscountOnSale;
        public double DiscountOnSale
        {
            get
            {
                return _DiscountOnSale;
            }
            set
            {
                if (value != _DiscountOnSale)
                {
                    _DiscountOnSale = value;
                    OnPropertyChanged("DiscountOnSale");
                }
            }
        }

        //***//-----------
        private double _StaffDiscount;
        public double StaffDiscount
        {
            get
            {
                return _StaffDiscount;
            }
            set
            {
                if (value != _StaffDiscount)
                {
                    _StaffDiscount = value;
                    OnPropertyChanged("StaffDiscount");
                }
            }
        }

        private double _WalkinDiscount;
        public double WalkinDiscount
        {
            get
            {
                return _WalkinDiscount;
            }
            set
            {
                if (value != _WalkinDiscount)
                {
                    _WalkinDiscount = value;
                    OnPropertyChanged("WalkinDiscount");
                }
            }
        }

        private double _RegisteredPatientsDiscount;
        public double RegisteredPatientsDiscount
        {
            get
            {
                return _RegisteredPatientsDiscount;
            }
            set
            {
                if (value != _RegisteredPatientsDiscount)
                {
                    _RegisteredPatientsDiscount = value;
                    OnPropertyChanged("RegisteredPatientsDiscount");
                }
            }
        }


        public bool IsApprovedDirect { get; set; } 

        //---------------------


        private int _ReorderQnt;
        public int ReorderQnt
        {
            get
            {
                return _ReorderQnt;
            }
            set
            {
                if (value != _ReorderQnt)
                {
                    _ReorderQnt = value;
                    OnPropertyChanged("ReorderQnt");
                }
            }
        }
        private Boolean _BatchesRequired;
        public Boolean BatchesRequired
        {
            get
            {
                return _BatchesRequired;
            }
            set
            {
                if (value != _BatchesRequired)
                {
                    _BatchesRequired = value;
                    OnPropertyChanged("BatchesRequired");
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
        public int ApplicableOnForOB { get; set; }
        public int InclusiveForOB { get; set; }
        private Boolean _Status;
        public Boolean Status
        {
            get
            {
                return _Status;
            }
            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }



        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get
            {
                return _CreatedUnitID;
            }
            set
            {
                if (value != _CreatedUnitID)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }
        private long _UpdatedUnitID;
        public long UpdatedUnitID
        {
            get
            {
                return _UpdatedUnitID;
            }
            set
            {
                if (value != _UpdatedUnitID)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
                }
            }
        }


        private long _RowId;
        public long RowID
        {
            get { return _RowId; }
            set
            {
                if (_RowId != value)
                {
                    _RowId = value;
                    OnPropertyChanged("RowID");
                }
            }
        }
        private long _AddedBy;
        public long AddedBy
        {
            get
            {
                return _AddedBy;
            }
            set
            {
                if (value != _AddedBy)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }
        private string _AddedOn;
        public string AddedOn
        {
            get
            {
                return _AddedOn;
            }
            set
            {
                if (value != _AddedOn)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }
        private DateTime _AddedDateTime;
        public DateTime AddedDateTime
        {
            get
            {
                return _AddedDateTime;
            }
            set
            {
                if (value != _AddedDateTime)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }
        private long _UpdatedBy;
        public long UpdatedBy
        {
            get
            {
                return _UpdatedBy;
            }
            set
            {
                if (value != _UpdatedBy)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }
        private string _UpdatedOn;
        public string UpdateddOn
        {
            get
            {
                return _UpdatedOn;
            }
            set
            {
                if (value != _UpdatedOn)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }
        private DateTime _UpdatedDateTime;
        public DateTime UpdatedDateTime
        {
            get
            {
                return _UpdatedDateTime;
            }
            set
            {
                if (value != _UpdatedDateTime)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }
        private string _AddedWindowsLoginName;
        public string AddedWindowsLoginName
        {
            get
            {
                return _AddedWindowsLoginName;
            }
            set
            {
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }
        private string _UpdateWindowsLoginName;
        public string UpdateWindowsLoginName
        {
            get
            {
                return _UpdateWindowsLoginName;
            }
            set
            {
                if (value != _UpdateWindowsLoginName)
                {
                    _UpdateWindowsLoginName = value;
                    OnPropertyChanged("UpdateWindowsLoginName");
                }
            }
        }

        private Boolean _EditMode;
        public Boolean EditMode
        {
            get
            {
                return _EditMode;
            }
            set
            {
                if (value != _EditMode)
                {
                    _EditMode = value;
                    OnPropertyChanged("EditMode");
                }
            }
        }
        private Boolean _RetrieveDataFlag;
        public Boolean RetrieveDataFlag
        {
            get
            {
                return _RetrieveDataFlag;
            }
            set
            {
                if (value != _RetrieveDataFlag)
                {
                    _RetrieveDataFlag = value;
                    OnPropertyChanged("RetrieveDataFlag");
                }
            }
        }
        private long _ItemID;
        public long ItemID
        {
            get
            {
                return _ItemID;
            }
            set
            {
                if (value != _ItemID)
                {
                    _ItemID = value;
                    OnPropertyChanged("ItemID");
                }
            }
        }
        private long _ItemTaxID;
        public long ItemTaxID
        {
            get
            {
                return _ItemTaxID;
            }
            set
            {
                if (value != _ItemTaxID)
                {
                    _ItemTaxID = value;
                    OnPropertyChanged("ItemTaxID");
                }
            }
        }
        private decimal _ItemTaxPer;
        public decimal ItemTaxPer
        {
            get
            {
                return _ItemTaxPer;
            }
            set
            {
                if (value != _ItemTaxPer)
                {
                    _ItemTaxPer = value;
                    OnPropertyChanged("ItemTaxPer");
                }
            }
        }
        private int _ItemTaxApplicableOn;
        public int ItemTaxApplicableOn
        {
            get
            {
                return _ItemTaxApplicableOn;
            }
            set
            {
                if (value != _ItemTaxApplicableOn)
                {
                    _ItemTaxApplicableOn = value;
                    OnPropertyChanged("ItemTaxApplicableOn");
                }
            }
        }
        private string _TaxApplicableOn;
        public string TaxApplicableOn
        {
            get
            {
                return _TaxApplicableOn;
            }
            set
            {
                if (value != _TaxApplicableOn)
                {
                    _TaxApplicableOn = value;
                    OnPropertyChanged("TaxApplicableOn");
                }
            }
        }
        private int _ItemTaxApplicableFor;
        public int ItemTaxApplicableFor
        {
            get
            {
                return _ItemTaxApplicableFor;
            }
            set
            {
                if (value != _ItemTaxApplicableFor)
                {
                    _ItemTaxApplicableFor = value;
                    OnPropertyChanged("ItemTaxApplicableFor");
                }
            }
        }
        private string _TaxApplicableFor;
        public string TaxApplicableFor
        {
            get
            {
                return _TaxApplicableFor;
            }
            set
            {
                if (value != _TaxApplicableFor)
                {
                    _TaxApplicableFor = value;
                    OnPropertyChanged("TaxApplicableFor");
                }
            }
        }
        private string _TaxName;
        public string TaxName { get; set; }




        private Boolean _PrimaryKeyViolationError;
        public Boolean PrimaryKeyViolationError
        {
            get
            {
                return _PrimaryKeyViolationError;
            }
            set
            {
                _PrimaryKeyViolationError = value;
            }
        }
        private Boolean _GeneralError;
        public Boolean GeneralError
        {
            get
            {
                return _GeneralError;
            }
            set
            {
                _GeneralError = value;
            }
        }
        public long SupplierID
        { get; set; }
        private string _SupplierName;
        public string SupplierName
        {
            get
            {
                return _SupplierName;
            }
            set
            {
                if (value != _SupplierName)
                {
                    _SupplierName = value;
                    OnPropertyChanged("SupplierName");
                }
            }
        }
        private string _ClinicName;
        public string ClinicName
        {
            get
            {
                return _ClinicName;
            }
            set
            {
                if (value != _ClinicName)
                {
                    _ClinicName = value;
                    OnPropertyChanged("ClinicName");
                }
            }
        }
        private string _StoreName;
        public string StoreName
        {
            get
            {
                return _StoreName;
            }
            set
            {
                if (value != _StoreName)
                {
                    _StoreName = value;
                    OnPropertyChanged("StoreName");
                }
            }
        }
        private double _Min;
        public double Min
        {
            get
            {
                return _Min;
            }
            set
            {
                if (value != _Min)
                {
                    _Min = value;
                    OnPropertyChanged("Min");
                }
            }
        }
        private double _Max;
        public double Max
        {
            get
            {
                return _Max;
            }
            set
            {
                if (value != _Max)
                {
                    _Max = value;
                    OnPropertyChanged("Max");
                }
            }
        }
        private double _Re_Order;
        public double Re_Order
        {
            get
            {
                return _Re_Order;
            }
            set
            {
                if (value != _Re_Order)
                {
                    _Re_Order = value;
                    OnPropertyChanged("Re_Order");
                }
            }
        }
        private long _StoreID;
        public long StoreID
        {
            get
            {
                return _StoreID;
            }
            set
            {
                if (value != _StoreID)
                {
                    _StoreID = value;
                    OnPropertyChanged("StoreID");
                }
            }
        }
        private Boolean _StoreStatus;
        public Boolean StoreStatus
        {
            get
            {
                return _StoreStatus;
            }
            set
            {
                if (value != _StoreStatus)
                {
                    _StoreStatus = value;
                    OnPropertyChanged("StoreStatus");
                }
            }
        }
        public List<long> lstStoresList { get; set; }
        private long _OpeningBalance;
        public long OpeningBalance
        {
            get
            {
                return _OpeningBalance;
            }
            set
            {
                if (value != _OpeningBalance)
                {
                    _OpeningBalance = value;
                    OnPropertyChanged("OpeningBalance");
                }
            }
        }
        private string _BatchNumber;
        public string BatchNumber
        {
            get
            {
                return _BatchNumber;
            }
            set
            {
                if (value != _BatchNumber)
                {
                    _BatchNumber = value;
                    OnPropertyChanged("BatchNumber");
                }
            }
        }
        private string _BatchExpiryDate;
        public string BatchExpiryDate
        {
            get
            {
                return _BatchExpiryDate;
            }
            set
            {
                if (value != _BatchExpiryDate)
                {
                    _BatchExpiryDate = value;
                    OnPropertyChanged("BatchExpiryDate");
                }
            }
        }
        private string _LinkServerName;
        public string LinkServerName
        {
            get
            {
                return _LinkServerName;
            }
            set
            {
                if (value != _LinkServerName)
                {
                    _LinkServerName = value;
                    OnPropertyChanged("LinkServerName");
                }
            }
        }
        private string _LinkServerAlias;
        public string LinkServerAlias
        {
            get
            {
                return _LinkServerAlias;
            }
            set
            {
                if (value != _LinkServerAlias)
                {
                    _LinkServerAlias = value;
                    OnPropertyChanged("LinkServerAlias");
                }
            }
        }
        private string _LinkServerDBName;
        public string LinkServerDBName
        {
            get
            {
                return _LinkServerDBName;
            }
            set
            {
                if (value != _LinkServerDBName)
                {
                    _LinkServerDBName = value;
                    OnPropertyChanged("LinkServerDBName");
                }
            }
        }
        private long _ClinicID;
        public long ClinicID
        {
            get
            {
                return _ClinicID;
            }
            set
            {
                if (value != _ClinicID)
                {
                    _ClinicID = value;
                    OnPropertyChanged("ClinicID");
                }
            }
        }

        private long _ItemClinicDetailID; //***//19
        public long ItemClinicDetailID
        {
            get
            {
                return _ItemClinicDetailID;
            }
            set
            {
                if (value != _ItemClinicDetailID)
                {
                    _ItemClinicDetailID = value;
                    OnPropertyChanged("ItemClinicDetailID");
                }
            }
        }

        private long _DefaultClinicID;
        public long DefaultClinicID
        {
            get
            {
                return _DefaultClinicID;
            }
            set
            {
                if (value != _DefaultClinicID)
                {
                    _DefaultClinicID = value;
                    OnPropertyChanged("DefaultClinicID");
                }
            }
        }

        private long _RackID;
        public long RackID
        {
            get
            {
                return _RackID;
            }
            set
            {
                if (value != _RackID)
                {
                    _RackID = value;
                    OnPropertyChanged("RackID");
                }
            }
        }
        private long _ShelfID;
        public long ShelfID
        {
            get
            {
                return _ShelfID;
            }
            set
            {
                if (value != _ShelfID)
                {
                    _ShelfID = value;
                    OnPropertyChanged("ShelfID");
                }
            }
        }
        private long _ContainerID;
        public long ContainerID
        {
            get
            {
                return _ContainerID;
            }
            set
            {
                if (value != _ContainerID)
                {
                    _ContainerID = value;
                    OnPropertyChanged("ContainerID");
                }
            }
        }
        private string _Rackname;
        public string Rackname
        {
            get
            {
                return _Rackname;
            }
            set
            {
                if (value != _Rackname)
                {
                    _Rackname = value;
                    OnPropertyChanged("Rackname");
                }
            }
        }
        private string _Shelfname;
        public string Shelfname
        {
            get
            {
                return _Shelfname;
            }
            set
            {
                if (value != _Shelfname)
                {
                    _Shelfname = value;
                    OnPropertyChanged("Shelfname");
                }
            }
        }
        private string _Containername;
        public string Containername
        {
            get
            {
                return _Containername;
            }
            set
            {
                if (value != _Containername)
                {
                    _Containername = value;
                    OnPropertyChanged("Containername");
                }
            }
        }


        public string Manufacturer { get; set; }
        public string PreganancyClass { get; set; }
        public double TotalPerchaseTaxPercent { get; set; }
        public double TotalSalesTaxPercent { get; set; }
        public bool AssignSupplier { get; set; }
        private string _BarCode;
        public string BarCode
        {
            get
            {
                return _BarCode;
            }
            set
            {
                if (value != _BarCode)
                {
                    _BarCode = value;
                    OnPropertyChanged("BarCode");
                }
            }
        }




        //by Anjali........................

        private decimal _SVatPer;
        public decimal SVatPer
        {
            get
            {
                return _SVatPer;
            }
            set
            {
                if (value != _SVatPer)
                {
                    _SVatPer = value;
                    OnPropertyChanged("SVatPer");
                }
            }
        }

        private decimal _SItemVatPer;
        public decimal SItemVatPer
        {
            get
            {
                return _SItemVatPer;
            }
            set
            {
                if (value != _SItemVatPer)
                {
                    _SItemVatPer = value;
                    OnPropertyChanged("SItemVatPer");
                }
            }
        }

        private int _SItemVatType;
        public int SItemVatType
        {
            get
            {
                return _SItemVatType;
            }
            set
            {
                if (value != _SItemVatType)
                {
                    _SItemVatType = value;
                    OnPropertyChanged("SItemVatType");
                }
            }
        }
        private int _SItemVatApplicationOn;
        public int SItemVatApplicationOn
        {
            get
            {
                return _SItemVatApplicationOn;
            }
            set
            {
                if (value != _SItemVatApplicationOn)
                {
                    _SItemVatApplicationOn = value;
                    OnPropertyChanged("SItemVatApplicationOn");
                }
            }
        }

        private int _SItemOtherTaxType;
        public int SItemOtherTaxType
        {
            get
            {
                return _SItemOtherTaxType;
            }
            set
            {
                if (value != _SItemOtherTaxType)
                {
                    _SItemOtherTaxType = value;
                    OnPropertyChanged("SItemVatType");
                }
            }
        }
        private int _SOtherItemApplicationOn;
        public int SOtherItemApplicationOn
        {
            get
            {
                return _SOtherItemApplicationOn;
            }
            set
            {
                if (value != _SOtherItemApplicationOn)
                {
                    _SOtherItemApplicationOn = value;
                    OnPropertyChanged("SItemVatApplicationOn");
                }
            }
        }

        private float _StockingCF;
        public float StockingCF
        {
            get
            {
                return _StockingCF;
            }
            set
            {
                if (value != _StockingCF)
                {
                    _StockingCF = value;
                    OnPropertyChanged("StockingCF");
                }
            }
        }
        private float _SellingCF;
        public float SellingCF
        {
            get
            {
                return _SellingCF;
            }
            set
            {
                if (value != _SellingCF)
                {
                    _SellingCF = value;
                    OnPropertyChanged("SellingCF");
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
                    OnPropertyChanged("StockingToBaseCF");
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
                    OnPropertyChanged("PurchaseToBaseCF");
                }
            }
        }
        //  public int ItemExpiredInDays { get; set; }
        //....................................
        #endregion

        private float _Budget;
        public float Budget
        {
            get
            {
                return _Budget;
            }
            set
            {
                if (value != _Budget)
                {
                    _Budget = value;
                    OnPropertyChanged("Budget");
                }
            }
        }

        private float _TotalBudget;
        public float TotalBudget
        {
            get
            {
                return _TotalBudget;
            }
            set
            {
                if (value != _TotalBudget)
                {
                    _TotalBudget = value;
                    OnPropertyChanged("TotalBudget");
                }
            }
        }

        private float _DiscountedPercentage;
        public float DiscountedPercentage
        {
            get
            {
                return _DiscountedPercentage;
            }
            set
            {
                if (value != _DiscountedPercentage)
                {
                    _DiscountedPercentage = value;
                    OnPropertyChanged("DiscountedPercentage");
                }
            }
        }

        private float _ApplicableToAllDiscount;
        public float ApplicableToAllDiscount
        {
            get
            {
                return _ApplicableToAllDiscount;
            }
            set
            {
                if (value != _ApplicableToAllDiscount)
                {
                    _ApplicableToAllDiscount = value;
                    OnPropertyChanged("ApplicableToAllDiscount");
                }
            }
        }

        # region Added By Bhushanp For GST 19062017

        private long _HSNCodesID;

        public long HSNCodesID
        {
            get { return _HSNCodesID; }
            set
            {
                if (value != _HSNCodesID)
                {
                    _HSNCodesID = value;
                    OnPropertyChanged("HSNCodesID");
                }
            }
        }

        private string _HSNCodes;

        public string HSNCodes
        {
            get { return _HSNCodes; }
            set
            {
                if (value != _HSNCodes)
                {
                    _HSNCodes = value;
                    OnPropertyChanged("HSNCodes");
                }
            }
        }

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

        ///////////////////////////////////////////////////

        private int _SGSTtaxtype;
        public int SGSTtaxtype
        {
            get
            {
                return _SGSTtaxtype;
            }
            set
            {
                if (value != _SGSTtaxtype)
                {
                    _SGSTtaxtype = value;
                    OnPropertyChanged("SGSTtaxtype");
                }
            }
        }
        private int _SGSTapplicableon;
        public int SGSTapplicableon
        {
            get
            {
                return _SGSTapplicableon;
            }
            set
            {
                if (value != _SGSTapplicableon)
                {
                    _SGSTapplicableon = value;
                    OnPropertyChanged("SGSTapplicableon");
                }
            }
        }

        ///////////////////////////////////////////////////

        private int _CGSTtaxtype;
        public int CGSTtaxtype
        {
            get
            {
                return _CGSTtaxtype;
            }
            set
            {
                if (value != _CGSTtaxtype)
                {
                    _CGSTtaxtype = value;
                    OnPropertyChanged("CGSTtaxtype");
                }
            }
        }
        private int _CGSTapplicableon;
        public int CGSTapplicableon
        {
            get
            {
                return _CGSTapplicableon;
            }
            set
            {
                if (value != _CGSTapplicableon)
                {
                    _CGSTapplicableon = value;
                    OnPropertyChanged("CGSTapplicableon");
                }
            }
        }

        ///////////////////////////////////////////////////

        private int _IGSTtaxtype;
        public int IGSTtaxtype
        {
            get
            {
                return _IGSTtaxtype;
            }
            set
            {
                if (value != _IGSTtaxtype)
                {
                    _IGSTtaxtype = value;
                    OnPropertyChanged("IGSTtaxtype");
                }
            }
        }
        private int _IGSTapplicableon;
        public int IGSTapplicableon
        {
            get
            {
                return _IGSTapplicableon;
            }
            set
            {
                if (value != _IGSTapplicableon)
                {
                    _IGSTapplicableon = value;
                    OnPropertyChanged("IGSTapplicableon");
                }
            }
        }

        // For Sale 

        private decimal _SGSTPercentSale;
        public decimal SGSTPercentSale
        {
            get
            {
                return _SGSTPercentSale;
            }
            set
            {
                if (value != _SGSTPercentSale)
                {
                    _SGSTPercentSale = value;
                    OnPropertyChanged("SGSTPercentSale");
                }
            }
        }

        private decimal _CGSTPercentSale;
        public decimal CGSTPercentSale
        {
            get
            {
                return _CGSTPercentSale;
            }
            set
            {
                if (value != _CGSTPercentSale)
                {
                    _CGSTPercentSale = value;
                    OnPropertyChanged("CGSTPercentSale");
                }
            }
        }

        private decimal _IGSTPercentSale;
        public decimal IGSTPercentSale
        {
            get
            {
                return _IGSTPercentSale;
            }
            set
            {
                if (value != _IGSTPercentSale)
                {
                    _IGSTPercentSale = value;
                    OnPropertyChanged("IGSTPercentSale");
                }
            }
        }

        ///////////////////////////////////////////////////

        private int _SGSTtaxtypeSale;
        public int SGSTtaxtypeSale
        {
            get
            {
                return _SGSTtaxtypeSale;
            }
            set
            {
                if (value != _SGSTtaxtypeSale)
                {
                    _SGSTtaxtypeSale = value;
                    OnPropertyChanged("SGSTtaxtypeSale");
                }
            }
        }
        private int _SGSTapplicableonSale;
        public int SGSTapplicableonSale
        {
            get
            {
                return _SGSTapplicableonSale;
            }
            set
            {
                if (value != _SGSTapplicableonSale)
                {
                    _SGSTapplicableonSale = value;
                    OnPropertyChanged("SGSTapplicableonSale");
                }
            }
        }

        ///////////////////////////////////////////////////

        private int _CGSTtaxtypeSale;
        public int CGSTtaxtypeSale
        {
            get
            {
                return _CGSTtaxtypeSale;
            }
            set
            {
                if (value != _CGSTtaxtypeSale)
                {
                    _CGSTtaxtypeSale = value;
                    OnPropertyChanged("CGSTtaxtypeSale");
                }
            }
        }
        private int _CGSTapplicableonSale;
        public int CGSTapplicableonSale
        {
            get
            {
                return _CGSTapplicableonSale;
            }
            set
            {
                if (value != _CGSTapplicableonSale)
                {
                    _CGSTapplicableonSale = value;
                    OnPropertyChanged("CGSTapplicableonSale");
                }
            }
        }

        ///////////////////////////////////////////////////

        private int _IGSTtaxtypeSale;
        public int IGSTtaxtypeSale
        {
            get
            {
                return _IGSTtaxtypeSale;
            }
            set
            {
                if (value != _IGSTtaxtypeSale)
                {
                    _IGSTtaxtypeSale = value;
                    OnPropertyChanged("IGSTtaxtypeSale");
                }
            }
        }
        private int _IGSTapplicableonSale;
        public int IGSTapplicableonSale
        {
            get
            {
                return _IGSTapplicableonSale;
            }
            set
            {
                if (value != _IGSTapplicableonSale)
                {
                    _IGSTapplicableonSale = value;
                    OnPropertyChanged("IGSTapplicableonSale");
                }
            }
        }

        #endregion      

    }
    public class clsItemStoreVO : INotifyPropertyChanged, IValueObject
    {

        private Boolean _status;
        public Boolean status
        {
            get
            {
                return _status;
            }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged("status");
                }
            }
        }

        private long _StoreID;
        public long StoreID
        {
            get
            {
                return _StoreID;
            }
            set
            {
                if (value != _StoreID)
                {
                    _StoreID = value;
                    OnPropertyChanged("StoreID");
                }
            }
        }
        private double _Min;
        public double Min
        {
            get
            {
                return _Min;
            }
            set
            {
                if (value != _Min)
                {
                    _Min = value;
                    OnPropertyChanged("Min");
                }
            }
        }
        private double _Max;
        public double Max
        {
            get
            {
                return _Max;
            }
            set
            {
                if (value != _Max)
                {
                    _Max = value;
                    OnPropertyChanged("Max");
                }
            }
        }
        private double _Re_Order;
        public double Re_Order
        {
            get
            {
                return _Re_Order;
            }
            set
            {
                if (value != _Re_Order)
                {
                    _Re_Order = value;
                    OnPropertyChanged("Re_Order");
                }
            }
        }
        private long _ItemClinicID;
        public long ItemClinicID
        {
            get
            {
                return _ItemClinicID;
            }
            set
            {
                if (value != _ItemClinicID)
                {
                    _ItemClinicID = value;
                    OnPropertyChanged("ItemClinicID");
                }
            }
        }
        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }
        private long _UnitID;
        public long UnitID
        {
            get
            {
                return _UnitID;
            }
            set
            {
                if (value != _UnitID)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }
        private string _StoreName;
        public string StoreName
        {
            get
            {
                return _StoreName;
            }
            set
            {
                if (value != _StoreName)
                {
                    _StoreName = value;
                    OnPropertyChanged("StoreName");
                }
            }
        }
        private bool _StoreStatus;
        public bool StoreStatus
        {
            get
            {
                return _StoreStatus;
            }
            set
            {
                if (value != _StoreStatus)
                {
                    _StoreStatus = value;
                    OnPropertyChanged("StoreStatus");
                }
            }
        }
        private long _UnitId;
        public long UnitId
        {
            get
            {
                return _UnitId;
            }
            set
            {
                if (value != _UnitId)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }
        private long _UserId;
        public long UserId
        {
            get { return _UserId; }
            set
            {
                if (value != _UserId)
                {
                    _UserId = value;
                    OnPropertyChanged("UserId");
                }
            }
        }

        private long _UserStoreId;
        public long UserStoreId
        {
            get { return _UserStoreId; }
            set
            {
                if (value != _UserStoreId)
                {
                    _UserStoreId = value;
                    OnPropertyChanged("UserStoreId");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get
            {
                return _CreatedUnitID;
            }
            set
            {
                if (value != _CreatedUnitID)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long _UpdatedUnitID;
        public long UpdatedUnitID
        {
            get
            {
                return _UpdatedUnitID;
            }
            set
            {
                if (value != _UpdatedUnitID)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
                }
            }
        }

        private long _AddedBy;
        public long AddedBy
        {
            get
            {
                return _AddedBy;
            }
            set
            {
                if (value != _AddedBy)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn;
        public string AddedOn
        {
            get
            {
                return _AddedOn;
            }
            set
            {
                if (value != _AddedOn)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime _AddedDateTime;
        public DateTime AddedDateTime
        {
            get
            {
                return _AddedDateTime;
            }
            set
            {
                if (value != _AddedDateTime)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private long _UpdatedBy;
        public long UpdatedBy
        {
            get
            {
                return _UpdatedBy;
            }
            set
            {
                if (value != _UpdatedBy)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn;
        public string UpdateddOn
        {
            get
            {
                return _UpdatedOn;
            }
            set
            {
                if (value != _UpdatedOn)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime _UpdatedDateTime;
        public DateTime UpdatedDateTime
        {
            get
            {
                return _UpdatedDateTime;
            }
            set
            {
                if (value != _UpdatedDateTime)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName;
        public string AddedWindowsLoginName
        {
            get
            {
                return _AddedWindowsLoginName;
            }
            set
            {
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private string _UpdateWindowsLoginName;
        public string UpdateWindowsLoginName
        {
            get
            {
                return _UpdateWindowsLoginName;
            }
            set
            {
                if (value != _UpdateWindowsLoginName)
                {
                    _UpdateWindowsLoginName = value;
                    OnPropertyChanged("UpdateWindowsLoginName");
                }
            }
        }

        public long ItemID { get; set; }

        public List<clsItemStoreVO> StoreList { get; set; }

        public List<clsItemStoreVO> DeletedStoreList { get; set; }

        public List<clsItemTaxVO> objStoreTaxList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return this.StoreName.ToString();
        }
    }

    public class clsItemSupllierVO : INotifyPropertyChanged, IValueObject
    {
        private Boolean _status;
        public Boolean status
        {
            get
            {
                return _status;
            }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged("status");
                }
            }
        }
        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }
        private long _SupplierID;
        public long SupplierID
        {
            get
            {
                return _SupplierID;
            }
            set
            {
                if (value != _SupplierID)
                {
                    _SupplierID = value;
                    OnPropertyChanged("SupplierID");
                }
            }
        }
        private string _SupplierName;
        public string SupplierName
        {
            get
            {
                return _SupplierName;
            }
            set
            {
                if (value != _SupplierName)
                {
                    _SupplierName = value;
                    OnPropertyChanged("SupplierName");
                }
            }
        }
        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get
            {
                return _CreatedUnitID;
            }
            set
            {
                if (value != _CreatedUnitID)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }
        private long _UpdatedUnitID;
        public long UpdatedUnitID
        {
            get
            {
                return _UpdatedUnitID;
            }
            set
            {
                if (value != _UpdatedUnitID)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
                }
            }
        }
        private long _AddedBy;
        public long AddedBy
        {
            get
            {
                return _AddedBy;
            }
            set
            {
                if (value != _AddedBy)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }
        private string _AddedOn;
        public string AddedOn
        {
            get
            {
                return _AddedOn;
            }
            set
            {
                if (value != _AddedOn)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }
        private DateTime _AddedDateTime;
        public DateTime AddedDateTime
        {
            get
            {
                return _AddedDateTime;
            }
            set
            {
                if (value != _AddedDateTime)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }
        private long _UpdatedBy;
        public long UpdatedBy
        {
            get
            {
                return _UpdatedBy;
            }
            set
            {
                if (value != _UpdatedBy)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }
        private string _UpdatedOn;
        public string UpdateddOn
        {
            get
            {
                return _UpdatedOn;
            }
            set
            {
                if (value != _UpdatedOn)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }
        private DateTime _UpdatedDateTime;
        public DateTime UpdatedDateTime
        {
            get
            {
                return _UpdatedDateTime;
            }
            set
            {
                if (value != _UpdatedDateTime)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }
        private string _AddedWindowsLoginName;
        public string AddedWindowsLoginName
        {
            get
            {
                return _AddedWindowsLoginName;
            }
            set
            {
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }
        private string _UpdateWindowsLoginName;
        public string UpdateWindowsLoginName
        {
            get
            {
                return _UpdateWindowsLoginName;
            }
            set
            {
                if (value != _UpdateWindowsLoginName)
                {
                    _UpdateWindowsLoginName = value;
                    OnPropertyChanged("UpdateWindowsLoginName");
                }
            }
        }
        private long _UnitId;
        public long UnitId
        {
            get
            {
                return _UnitId;
            }
            set
            {
                if (value != _UnitId)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
                }
            }
        }
        public long ItemID { get; set; }
        public List<clsItemSupllierVO> SupplierList { get; set; }
        List<MasterListItem> _HPLevelList = new List<MasterListItem> 
        { 
            new MasterListItem{ ID=0,Description="--Select--"} ,
            new MasterListItem{ ID=1,Description="I"} ,
            new MasterListItem{ ID=2,Description="II"} ,
            new MasterListItem{ ID=3,Description="III"} ,
        };

        public List<MasterListItem> HPLevelList
        {
            get
            {
                return _HPLevelList;
            }
            set
            {
                if (value != _HPLevelList)
                {
                    _HPLevelList = value;
                }
            }

        }
        MasterListItem _SelectedHPLevel = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedHPLevel
        {
            get
            {
                return _SelectedHPLevel;
            }
            set
            {
                if (value != _SelectedHPLevel)
                {
                    _SelectedHPLevel = value;
                    OnPropertyChanged("SelectedHPLevel");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public string ToXml()
        {
            throw new NotImplementedException();
        }
    }

    public class clsItemTaxVO : INotifyPropertyChanged, IValueObject
    {
        private Boolean _status;
        public Boolean status
        {
            get
            {
                return _status;
            }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged("status");
                }
            }
        }
        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }
        private long _TaxID;
        public long TaxID
        {
            get
            {
                return _TaxID;
            }
            set
            {
                if (value != _TaxID)
                {
                    _TaxID = value;
                    OnPropertyChanged("TaxID");
                }
            }
        }
        private string _TaxName;
        public string TaxName
        {
            get
            {
                return _TaxName;
            }
            set
            {
                if (value != _TaxName)
                {
                    _TaxName = value;
                    OnPropertyChanged("TaxName");
                }
            }
        }
        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get
            {
                return _CreatedUnitID;
            }
            set
            {
                if (value != _CreatedUnitID)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }
        private long _UpdatedUnitID;
        public long UpdatedUnitID
        {
            get
            {
                return _UpdatedUnitID;
            }
            set
            {
                if (value != _UpdatedUnitID)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
                }
            }
        }
        private long _AddedBy;
        public long AddedBy
        {
            get
            {
                return _AddedBy;
            }
            set
            {
                if (value != _AddedBy)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }
        private string _AddedOn;
        public string AddedOn
        {
            get
            {
                return _AddedOn;
            }
            set
            {
                if (value != _AddedOn)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }
        private DateTime _AddedDateTime;
        public DateTime AddedDateTime
        {
            get
            {
                return _AddedDateTime;
            }
            set
            {
                if (value != _AddedDateTime)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }
        private long _UpdatedBy;
        public long UpdatedBy
        {
            get
            {
                return _UpdatedBy;
            }
            set
            {
                if (value != _UpdatedBy)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }




        private string _UpdatedOn;
        public string UpdateddOn
        {
            get
            {
                return _UpdatedOn;
            }
            set
            {
                if (value != _UpdatedOn)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }


        private DateTime _UpdatedDateTime;
        public DateTime UpdatedDateTime
        {
            get
            {
                return _UpdatedDateTime;
            }
            set
            {
                if (value != _UpdatedDateTime)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }
        private string _AddedWindowsLoginName;
        public string AddedWindowsLoginName
        {
            get
            {
                return _AddedWindowsLoginName;
            }
            set
            {
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }
        private string _UpdateWindowsLoginName;
        public string UpdateWindowsLoginName
        {
            get
            {
                return _UpdateWindowsLoginName;
            }
            set
            {
                if (value != _UpdateWindowsLoginName)
                {
                    _UpdateWindowsLoginName = value;
                    OnPropertyChanged("UpdateWindowsLoginName");
                }
            }
        }
        private long _UnitId;
        public long UnitId
        {
            get
            {
                return _UnitId;
            }
            set
            {
                if (value != _UnitId)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
                }
            }
        }
        private long _ItemClinicDetailId;
        public long ItemClinicDetailId
        {
            get { return _ItemClinicDetailId; }
            set { _ItemClinicDetailId = value; }
        }
        public long ItemID { get; set; }

        public List<clsItemTaxVO> TaxList { get; set; }
        List<MasterListItem> _ApplicableOnList = new List<MasterListItem> 
        { 
            new MasterListItem{ ID=0,Description="--Select--"} ,
            new MasterListItem{ ID=1,Description="Purchase Rate"} ,
            new MasterListItem{ ID=2,Description="MRP"} 
           
        };

        public List<MasterListItem> ApplicableOnList
        {
            get
            {
                return _ApplicableOnList;
            }
            set
            {
                if (value != _ApplicableOnList)
                {
                    _ApplicableOnList = value;
                }
            }

        }
        MasterListItem _ApplicableOn = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem ApplicableOn
        {
            get
            {
                return _ApplicableOn;
            }
            set
            {
                if (value != _ApplicableOn)
                {
                    _ApplicableOn = value;
                    OnPropertyChanged("ApplicableOn");
                }
            }
        }
        List<MasterListItem> _ApplicableForList = new List<MasterListItem> 
        { 
            new MasterListItem{ ID=0,Description="--Select--"} ,
            new MasterListItem{ ID=1,Description="Purchase"} ,
            new MasterListItem{ ID=2,Description="Sale"} 
           
        };

        public List<MasterListItem> ApplicableForList
        {
            get
            {
                return _ApplicableForList;
            }
            set
            {
                if (value != _ApplicableForList)
                {
                    _ApplicableForList = value;
                }
            }

        }


        private long _ApplicableForId;
        public long ApplicableForId
        {
            get
            {
                return _ApplicableForId;
            }
            set
            {
                if (value != _ApplicableForId)
                {
                    _ApplicableForId = value;
                    OnPropertyChanged("ApplicableForId");
                }
            }
        }

        private string _ApplicableForDesc;
        public string ApplicableForDesc
        {
            get
            {
                return _ApplicableForDesc;
            }
            set
            {
                if (value != _ApplicableForDesc)
                {
                    _ApplicableForDesc = value;
                    OnPropertyChanged("ApplicableForDesc");
                }
            }
        }
        private string _ApplicableOnDesc;
        public string ApplicableOnDesc
        {
            get
            {
                return _ApplicableOnDesc;
            }
            set
            {
                if (value != _ApplicableOnDesc)
                {
                    _ApplicableOnDesc = value;
                    OnPropertyChanged("ApplicableOnDesc");
                }
            }
        }
        private long _ApplicableOnId;
        public long ApplicableOnId
        {
            get
            {
                return _ApplicableOnId;
            }
            set
            {
                if (value != _ApplicableOnId)
                {
                    _ApplicableOnId = value;
                    OnPropertyChanged("ApplicableOnId");
                }
            }
        }


        MasterListItem _ApplicableFor = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem ApplicableFor
        {
            get
            {
                return _ApplicableFor;
            }
            set
            {
                if (value != _ApplicableFor)
                {
                    _ApplicableFor = value;
                    OnPropertyChanged("ApplicableFor");
                }
            }
        }

        private decimal _Percentage;
        public decimal Percentage
        {
            get
            {
                return _Percentage;
            }
            set
            {
                if (value != _Percentage)
                {
                    _Percentage = value;
                    OnPropertyChanged("Percentage");
                }
            }
        }

        private int _TaxType;
        public int TaxType
        {
            get
            {
                return _TaxType;
            }
            set
            {
                if (value != _TaxType)
                {
                    _TaxType = value;
                    //OnPropertyChanged("Percentage");
                }
            }
        }

        private string _TaxTypeName;
        public string TaxTypeName
        {
            get
            {
                return _TaxTypeName;
            }
            set
            {
                if (value != _TaxTypeName)
                {
                    _TaxTypeName = value;
                    //OnPropertyChanged("Percentage");
                }
            }
        }

        private long _ItemClinicId;
        public long ItemClinicId
        {
            get { return _ItemClinicId; }
            set { _ItemClinicId = value; }
        }

        private long _StoreID;
        public long StoreID
        {
            get { return _StoreID; }
            set { _StoreID = value; }
        }

        private DateTime _ApplicableFrom;
        public DateTime ApplicableFrom
        {
            get
            {
                return _ApplicableFrom;
            }
            set
            {
                if (value != _ApplicableFrom)
                {
                    _ApplicableFrom = value;
                    OnPropertyChanged("ApplicableFrom");
                }
            }
        }

        private DateTime _ApplicableTo;
        public DateTime ApplicableTo
        {
            get
            {
                return _ApplicableTo;
            }
            set
            {
                if (value != _ApplicableTo)
                {
                    _ApplicableTo = value;
                    OnPropertyChanged("ApplicableTo");
                }
            }
        }

        private bool _IsGST;

        public bool IsGST
        {
            get { return _IsGST; }
            set
            {
                if (value != _IsGST)
                {
                    _IsGST = value;
                    OnPropertyChanged("IsGST");
                };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }
    }

    public class clsItemMasterOtherDetailsVO : INotifyPropertyChanged, IValueObject
    {
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

        #region IValueObject
        public string ToXml()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Property Declaration

        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }

            set { _ID = value; }

        }

        private long _UnitID;
        public long UnitID
        {
            get
            {
                return _UnitID;
            }
            set
            {
                if (value != _UnitID)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }


        private long _ItemID;
        public long ItemID
        {
            get
            {
                return _ItemID;
            }
            set
            {
                if (value != _ItemID)
                {
                    _ItemID = value;
                    OnPropertyChanged("ItemID");
                }
            }
        }

        private string _ItemName;
        public string ItemName
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

        private string _Contradiction;
        public string Contradiction
        {
            get
            {
                return _Contradiction;
            }
            set
            {
                if (value != _Contradiction)
                {
                    _Contradiction = value;
                    OnPropertyChanged("Contradiction");
                }
            }
        }



        private string _SideEffect;
        public string SideEffect
        {
            get
            {
                return _SideEffect;
            }
            set
            {
                if (value != _SideEffect)
                {
                    _SideEffect = value;
                    OnPropertyChanged("SideEffect");
                }
            }
        }

        private string _URL;
        public string URL
        {
            get
            {
                return _URL;
            }
            set
            {
                if (value != _URL)
                {
                    _URL = value;
                    OnPropertyChanged("URL");
                }
            }
        }

        #endregion
    }


    public class clsGRNSaleTaxDetailsVO : IValueObject, INotifyPropertyChanged
    {

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

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

        private long _GRNDetailID;
        public long GRNDetailID
        {
            get { return _GRNDetailID; }
            set
            {
                if (_GRNDetailID != value)
                {
                    _GRNDetailID = value;
                    OnPropertyChanged("GRNDetailID");
                }
            }
        }

        private long _GRNDetailUnitID;
        public long GRNDetailUnitID
        {
            get { return _GRNDetailUnitID; }
            set
            {
                if (_GRNDetailUnitID != value)
                {
                    _GRNDetailUnitID = value;
                    OnPropertyChanged("GRNDetailUnitID");
                }
            }
        }

        private long _TaxID;
        public long TaxID
        {
            get { return _TaxID; }
            set
            {
                if (_TaxID != value)
                {
                    _TaxID = value;
                    OnPropertyChanged("TaxID");
                }
            }
        }

        private double _TaxAmount;
        public double TaxAmount
        {
            get { return _TaxAmount; }
            set
            {
                if (_TaxAmount != value)
                {
                    _TaxAmount = value;
                    OnPropertyChanged("TaxAmount");
                }
            }
        }

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
                }
            }
        }

        private bool _IsFromGRN;
        public bool IsFromGRN
        {
            get { return _IsFromGRN; }
            set
            {
                if (_IsFromGRN != value)
                {
                    _IsFromGRN = value;
                    OnPropertyChanged("IsFromGRN");
                }
            }
        }

        // added by santosh patil 31/05/2013
        List<MasterListItem> _ApplicableOnList = new List<MasterListItem> 
        { 
            new MasterListItem{ ID=0,Description="--Select--"} ,
            new MasterListItem{ ID=1,Description="Purchase Rate"} ,
            new MasterListItem{ ID=2,Description="MRP"}            
        };
        public List<MasterListItem> ApplicableOnList
        {
            get
            {
                return _ApplicableOnList;
            }
            set
            {
                if (value != _ApplicableOnList)
                {
                    _ApplicableOnList = value;
                }
            }

        }

        MasterListItem _ApplicableOn = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem ApplicableOn
        {
            get
            {
                return _ApplicableOn;
            }
            set
            {
                if (value != _ApplicableOn)
                {
                    _ApplicableOn = value;
                    OnPropertyChanged("ApplicableOn");
                }
            }


        }
        #endregion
    }
}
