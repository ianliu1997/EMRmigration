using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.EMR;
using System.Collections.ObjectModel;

namespace PalashDynamics.ValueObjects
{
   public class clsItembatchSearchVO
    {

       //for itemmaster
     
       public string ItemCode { get; set; }
       public string BrandName { get; set; }
       public string ItemName { get; set; }
       public Boolean BatchesRequired { get; set; }
       public Boolean Status { get; set; }
       public string SUOM { get; set; }
       public string PUOM { get; set; }
       //By Anjali..................
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
                  // OnPropertyChanged("PUM");
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
                  // OnPropertyChanged("SUM");
               }
           }
       }
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
                   //OnPropertyChanged("BaseUM");
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
                   //OnPropertyChanged("SellingUM");
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
                  // OnPropertyChanged("BaseUMString");
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
                   //OnPropertyChanged("SellingUMString");
               }
           }
       }
       //.....................................


       public long SUOMID { get; set; }
       public long BUOMID { get; set; }
       public Boolean InclusiveOfTax { get; set; }
       public string Manufacturer { get; set; }
       public string PreganancyClass { get; set; }
       public double TotalPerchaseTaxPercent { get; set; }
       public double TotalSalesTaxPercent { get; set; }
       public bool AssignSupplier { get; set; }

       public long CategoryId { get; set; }
       public long GroupId { get; set; }

       public decimal VatPer { get; set; }

       //By Anjali.....................

       public float AvailableStockInBase { get; set; }
       public float Re_Order { get; set; }
       public float ConversionFactor { get; set; }
       public float SellingCF { get; set; }
       public float StockingCF { get; set; }
       
       //..........................
      
       ////By Umesh
       //private float _StockingToBaseCF;
       //public float StockingToBaseCF
       //{
       //    get
       //    {
       //        return _StockingToBaseCF;
       //    }
       //    set
       //    {
       //        if (value != _StockingToBaseCF)
       //        {
       //            _StockingToBaseCF = value;
       //            OnPropertyChanged("StockingToBaseCF");
       //        }
       //    }
       //}
       //private float _PurchaseToBaseCF;
       //public float PurchaseToBaseCF
       //{
       //    get
       //    {
       //        return _PurchaseToBaseCF;
       //    }
       //    set
       //    {
       //        if (value != _PurchaseToBaseCF)
       //        {
       //            _PurchaseToBaseCF = value;
       //            OnPropertyChanged("PurchaseToBaseCF");
       //        }
       //    }
       //}

       ////

       //for bathmaster
       public long ID { get; set; } //For ItemStock
       public long? StoreID { get; set; }
       public long ItemID { get; set; }
       public long BatchID { get; set; }
       public double AvailableStock { get; set; }
       public string BatchCode { get; set; }
       public DateTime? ExpiryDate { get; set; }
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
               }
           }
       }
     

       public decimal SVatPer { get; set; }
      
       public double MRP { get; set; }
       public double PurchaseRate { get; set; }
        private DateTime _Date = DateTime.Now;
        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }
        public Double VATPerc { get; set; }
        public Double VATAmt { get; set; }
        public double DiscountOnSale { get; set; }

        public double StaffDiscount { get; set; } //***//
        public double RegisteredPatientsDiscount { get; set; } //***//
        public double WalkinDiscount { get; set; } //***//
        public bool IsApprovedDirect { get; set; } //***//     
      
        
        public double CurrentStock { get; set; }

        private clsPatientPrescriptionDetailVO _PrescribeDrugs = new clsPatientPrescriptionDetailVO();
        public clsPatientPrescriptionDetailVO PrescribeDrugs
        {
            get { return _PrescribeDrugs; }
            set
            {
                if (_PrescribeDrugs != value)
                {
                    _PrescribeDrugs = value;
                    // OnPropertyChanged("Items");
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
                    //OnPropertyChanged("Rackname");
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
                   // OnPropertyChanged("Shelfname");
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
                    //OnPropertyChanged("Containername");
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
                   // OnPropertyChanged("StockingToBaseCF");
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
                   // OnPropertyChanged("PurchaseToBaseCF");
                }
            }
        }

        private float _StockingToBaseCFForStkAdj;    //By Umesh
        public float StockingToBaseCFForStkAdj
        {
            get
            {
                return _StockingToBaseCFForStkAdj;
            }
            set
            {
                if (value != _StockingToBaseCFForStkAdj)
                {
                    _StockingToBaseCFForStkAdj = value;
                    // OnPropertyChanged("StockingToBaseCF");
                }
            }
        }    //By Umesh
        private float _PurchaseToBaseCFForStkAdj;    //By Umesh
        public float PurchaseToBaseCFForStkAdj
        {
            get
            {
                return _PurchaseToBaseCFForStkAdj;
            }
            set
            {
                if (value != _PurchaseToBaseCFForStkAdj)
                {
                    _PurchaseToBaseCFForStkAdj = value;
                    // OnPropertyChanged("PurchaseToBaseCF");
                }
            }
        }    //By Umesh

        #region For Quarantine Items (Expired, DOS)

        // Use For Vat/Tax Calculations

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

        private decimal _IssueVatPer;
        public decimal IssueVatPer
        {
            get
            {
                return _IssueVatPer;
            }
            set
            {
                if (value != _IssueVatPer)
                {
                    _IssueVatPer = value;
                    //OnPropertyChanged("IssueVatPer");
                }
            }
        }

        private decimal _IssueItemVatPer;
        public decimal IssueItemVatPer
        {
            get
            {
                return _IssueItemVatPer;
            }
            set
            {
                if (value != _IssueItemVatPer)
                {
                    _IssueItemVatPer = value;
                    //OnPropertyChanged("IssueItemVatPer");
                }
            }
        }

        private int _IssueItemVatType;
        public int IssueItemVatType
        {
            get
            {
                return _IssueItemVatType;
            }
            set
            {
                if (value != _IssueItemVatType)
                {
                    _IssueItemVatType = value;
                    //OnPropertyChanged("IssueItemVatType");
                }
            }
        }

        private int _IssueItemVatApplicationOn;
        public int IssueItemVatApplicationOn
        {
            get
            {
                return _IssueItemVatApplicationOn;
            }
            set
            {
                if (value != _IssueItemVatApplicationOn)
                {
                    _IssueItemVatApplicationOn = value;
                    //OnPropertyChanged("IssueItemVatApplicationOn");
                }
            }
        }

        #endregion

        #region Added By Bhushanp For GST 19062017

        private decimal _SGSTPercent;
        public decimal SGSTPercent
        {
            get { return _SGSTPercent; }
            set
            {
                if (_SGSTPercent != value)
                {
                    _SGSTPercent = value;                    
                }
            }
        }


        private decimal _CGSTPercent;
        public decimal CGSTPercent
        {
            get { return _CGSTPercent; }
            set
            {
                if (_CGSTPercent != value)
                {
                    _CGSTPercent = value;                    
                }
            }
        }

        private decimal _IGSTPercent;
        public decimal IGSTPercent
        {
            get { return _IGSTPercent; }
            set
            {
                if (_IGSTPercent != value)
                {
                    _IGSTPercent = value;                    
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
                    //OnPropertyChanged("SGSTtaxtype");
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
                    //OnPropertyChanged("SGSTapplicableon");
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
                    //OnPropertyChanged("CGSTtaxtype");
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
                    //OnPropertyChanged("CGSTapplicableon");
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
                    //OnPropertyChanged("IGSTtaxtype");
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
                    //OnPropertyChanged("IGSTapplicableon");
                }
            }
        }

        // For Sale 

        private decimal _SGSTPercentSale;
        public decimal SGSTPercentSale
        {
            get { return _SGSTPercentSale; }
            set
            {
                if (_SGSTPercentSale != value)
                {
                    _SGSTPercentSale = value;
                }
            }
        }


        private decimal _CGSTPercentSale;
        public decimal CGSTPercentSale
        {
            get { return _CGSTPercentSale; }
            set
            {
                if (_CGSTPercentSale != value)
                {
                    _CGSTPercentSale = value;
                }
            }
        }

        private decimal _IGSTPercentSale;
        public decimal IGSTPercentSale
        {
            get { return _IGSTPercentSale; }
            set
            {
                if (_IGSTPercentSale != value)
                {
                    _IGSTPercentSale = value;
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
                    //OnPropertyChanged("_SGSTtaxtypeSale");
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
                    //OnPropertyChanged("_SGSTapplicableonSale");
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
                    //OnPropertyChanged("_CGSTtaxtypeSale");
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
                    //OnPropertyChanged("_CGSTapplicableonSale");
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
                    //OnPropertyChanged("_IGSTtaxtypeSale");
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
                    //OnPropertyChanged("_IGSTapplicableonSale");
                }
            }
        }

       
        #endregion
        private decimal _DiscountOnPackageItem;

        public decimal DiscountOnPackageItem
        {
            get { return _DiscountOnPackageItem; }
            set { _DiscountOnPackageItem = value; }
        }
    }
}
