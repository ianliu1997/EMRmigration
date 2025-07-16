using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsIssueItemVO : IValueObject, INotifyPropertyChanged
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


        #region Property

        public String LinkServer { get; set; }


        private bool? _IsToStoreQuarantine;
        public bool? IsToStoreQuarantine
        {
            get
            {
                return _IsToStoreQuarantine;
            }
            set
            {
                if (value != _IsToStoreQuarantine)
                {
                    _IsToStoreQuarantine = value;
                    OnPropertyChanged("IsToStoreQuarantine");
                }
            }
        }
        private bool? _IsFromStoreQuarantine;
        public bool? IsFromStoreQuarantine
        {
            get
            {
                return _IsFromStoreQuarantine;
            }
            set
            {
                if (value != _IsFromStoreQuarantine)
                {
                    _IsFromStoreQuarantine = value;
                    OnPropertyChanged("IsFromStoreQuarantine");
                }
            }
        }



        private long _IssueID;
        public long IssueID
        {
            get
            {
                return _IssueID;
            }
            set
            {
                if (value != _IssueID)
                {
                    _IssueID = value;
                    OnPropertyChanged("IssueID");
                }
            }
        }

        private long _IndentDetailsID;
        public long IndentDetailsID
        {
            get
            {
                return _IndentDetailsID;
            }
            set
            {
                if (value != _IndentDetailsID)
                {
                    _IndentDetailsID = value;
                    OnPropertyChanged("IndentDetailsID");
                }
            }
        }

        //Added By Pallavi
        private long? _IndentUnitID;
        public long? IndentUnitID
        {
            get
            {
                return _IndentUnitID;
            }
            set
            {
                if (value != _IndentUnitID)
                {
                    _IndentUnitID = value;
                    OnPropertyChanged("IndentUnitID");
                }
            }
        }


        private String _IssueNumber;
        public String IssueNumber
        {
            get
            {
                return _IssueNumber;
            }
            set
            {
                if (value != _IssueNumber)
                {
                    _IssueNumber = value;
                    OnPropertyChanged("IssueNumber");
                }
            }
        }

        private DateTime _IssueDate;
        public DateTime IssueDate
        {
            get
            {
                return _IssueDate;
            }
            set
            {
                if (value != _IssueDate)
                {
                    _IssueDate = value;
                    OnPropertyChanged("IssueDate");
                }
            }
        }

        private long? _IssueFromStoreId;
        public long? IssueFromStoreId
        {
            get
            {
                return _IssueFromStoreId;
            }
            set
            {
                if (value != _IssueFromStoreId)
                {
                    _IssueFromStoreId = value;
                    OnPropertyChanged("IssueFromStoreId");
                }
            }
        }


        private long? _IssueToStoreId;
        public long? IssueToStoreId
        {
            get
            {
                return _IssueToStoreId;
            }
            set
            {
                if (value != _IssueToStoreId)
                {
                    _IssueToStoreId = value;
                    OnPropertyChanged("IssueToStoreId");
                }
            }
        }


        private decimal? _TotalItems;
        public decimal? TotalItems
        {
            get
            {
                return _TotalItems;
            }
            set
            {
                if (value != _TotalItems)
                {
                    _TotalItems = value;
                    OnPropertyChanged("TotalItems");
                }
            }
        }
        //by Anjali..........................................
        //private bool _IsIndent;
        //public bool IsIndent
        //{
        //    get
        //    {
        //        return _IsIndent;
        //    }
        //    set
        //    {
        //        if (value != _IsIndent)
        //        {
        //            _IsIndent = value;
        //            OnPropertyChanged("IsIndent");

        //        }
        //    }
        //}
        private int _IsIndent;
        public int IsIndent
        {
            get
            {
                return _IsIndent;
            }
            set
            {
                if (value != _IsIndent)
                {
                    _IsIndent = value;
                    OnPropertyChanged("IsIndent");

                }
            }
        }
        private long _ReasonForIssue;
        public long ReasonForIssue
        {
            get
            {
                return _ReasonForIssue;
            }
            set
            {
                if (value != _ReasonForIssue)
                {
                    _ReasonForIssue = value;
                    OnPropertyChanged("ReasonForIssue");

                }
            }
        }

        public bool IsApprovedDirect { get; set; } //***//
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
        private decimal? _TotalVATAmount;
        public decimal? TotalVATAmount
        {
            get
            {
                return _TotalVATAmount;
            }
            set
            {
                if (value != _TotalVATAmount)
                {
                    _TotalVATAmount = value;
                    OnPropertyChanged("TotalVATAmount");
                }
            }
        }

        private decimal? _TotalTaxAmount;
        public decimal? TotalTaxAmount
        {
            get
            {
                return _TotalTaxAmount;
            }
            set
            {
                if (value != _TotalTaxAmount)
                {
                    _TotalTaxAmount = value;
                    OnPropertyChanged("TotalTaxAmount");
                }
            }
        }


        private decimal? _TotalAmount;
        public decimal? TotalAmount
        {
            get
            {
                return _TotalAmount;
            }
            set
            {
                if (value != _TotalAmount)
                {
                    _TotalAmount = value;
                    OnPropertyChanged("TotalAmount");
                }
            }
        }


        private String _Remark;
        public String Remark
        {
            get
            {
                return _Remark;
            }
            set
            {
                if (value != _Remark)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }


        private long? _ReceivedById;
        public long? ReceivedById
        {
            get
            {
                return _ReceivedById;
            }
            set
            {
                if (value != _ReceivedById)
                {
                    _ReceivedById = value;
                    OnPropertyChanged("ReceivedById");
                }
            }
        }


        private long? _IndentID;
        public long? IndentID
        {
            get
            {
                return _IndentID;
            }
            set
            {
                if (value != _IndentID)
                {
                    _IndentID = value;
                    OnPropertyChanged("IndentID");
                }
            }
        }


        private List<clsIssueItemDetailsVO> _IssueItemDetailsList = new List<clsIssueItemDetailsVO>();
        public List<clsIssueItemDetailsVO> IssueItemDetailsList
        {
            get
            {
                return _IssueItemDetailsList;
            }
            set
            {
                if (value != _IssueItemDetailsList)
                {
                    _IssueItemDetailsList = value;
                    OnPropertyChanged("IssueItemDetailsList");
                }
            }
        }


        #endregion

        #region PatientIndent
        private long _PatientID;
        public long PatientID
        {
            get
            {
                return _PatientID;
            }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get
            {
                return _PatientUnitID;
            }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }

        private bool _IsAgainstPatient;
        public bool IsAgainstPatient
        {
            get
            {
                return _IsAgainstPatient;
            }
            set
            {
                if (_IsAgainstPatient != value)
                {
                    _IsAgainstPatient = value;
                    OnPropertyChanged("IsAgainstPatient");
                }
            }
        }

        private string _PatientName;
        public string PatientName
        {
            get
            {
                return _PatientName;
            }
            set
            {
                if (_PatientName != value)
                {
                    _PatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }

        private string _MRNo;
        public string MRNo
        {
            get
            {
                return _MRNo;
            }
            set
            {
                if (_MRNo != value)
                {
                    _MRNo = value;
                    OnPropertyChanged("MRNo");
                }
            }
        }
        #endregion



    }

    public class clsIssueItemDetailsVO : INotifyPropertyChanged
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



        private float _Re_Order;
        public float Re_Order
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
        private long _ReasonForIssue;
        public long ReasonForIssue
        {
            get
            {
                return _ReasonForIssue;
            }
            set
            {
                if (value != _ReasonForIssue)
                {
                    _ReasonForIssue = value;
                    OnPropertyChanged("ReasonForIssue");
                }
            }
        }
        private string _ReasonIssueDescription;
        public string ReasonIssueDescription
        {
            get
            {
                return _ReasonIssueDescription;
            }
            set
            {
                if (value != _ReasonIssueDescription)
                {
                    _ReasonIssueDescription = value;
                    OnPropertyChanged("ReasonIssueDescription");
                }
            }
        }
        private List<MasterListItem> _ReasonForIssueList = new List<MasterListItem>();
        public List<MasterListItem> ReasonForIssueList
        {
            get
            {
                return _ReasonForIssueList;
            }
            set
            {
                _ReasonForIssueList = value;
            }
        }

        private MasterListItem _SelectedReasonForIssue = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedReasonForIssue
        {
            get
            {
                return _SelectedReasonForIssue;
            }
            set
            {
                _SelectedReasonForIssue = value;
            }
        }

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

        private Boolean? _IsEnable;
        public Boolean? IsEnable
        {
            get
            {
                if (_AvailableStock == 0 || _AvailableStock == null)//|| _ExpiryDate < DateTime.Now || _ExpiryDate == null
                    _IsEnable = false;
                else
                    _IsEnable = true;

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





        private long _ItemId;
        public long ItemId
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

        private long _IndentUnitID;
        public long IndentUnitID
        {
            get
            {
                return _IndentUnitID;
            }
            set
            {
                if (value != _IndentUnitID)
                {
                    _IndentUnitID = value;
                    OnPropertyChanged("IndentUnitID");
                }
            }
        }

        private long _IndentID;
        public long IndentID
        {
            get
            {
                return _IndentID;
            }
            set
            {
                if (value != _IndentID)
                {
                    _IndentID = value;
                    OnPropertyChanged("IndentID");
                }
            }
        }

        private long _IndentDetailsID;
        public long IndentDetailsID
        {
            get
            {
                return _IndentDetailsID;
            }
            set
            {
                if (value != _IndentDetailsID)
                {
                    _IndentDetailsID = value;
                    OnPropertyChanged("IndentDetailsID");
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

        private long _BatchId;
        public long BatchId
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

        private long _IssueID;
        public long IssueID
        {
            get
            {
                return _IssueID;
            }
            set
            {
                if (value != _IssueID)
                {
                    _IssueID = value;
                    OnPropertyChanged("IssueID");
                }
            }
        }

        private long _ToStoreID;
        public long ToStoreID
        {
            get
            {
                return _ToStoreID;
            }
            set
            {
                if (value != _ToStoreID)
                {
                    _ToStoreID = value;
                    OnPropertyChanged("ToStoreID");
                }
            }
        }

        private long _FromStoreID;
        public long FromStoreID
        {
            get
            {
                return _FromStoreID;
            }
            set
            {
                if (value != _FromStoreID)
                {
                    _FromStoreID = value;
                    OnPropertyChanged("FromStoreID");
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


        private decimal? _IndentQty;
        public decimal? IndentQty
        {
            get
            {
                return _IndentQty;
            }
            set
            {
                if (value != _IndentQty)
                {
                    _IndentQty = value;
                    OnPropertyChanged("IndentQty");
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
        private decimal? _IssuePendingQuantity;
        public decimal? IssuePendingQuantity
        {
            get
            {
                return _IssuePendingQuantity;
            }
            set
            {
                if (value != _IssuePendingQuantity)
                {
                    _IssuePendingQuantity = value;
                    OnPropertyChanged("IssuePendingQuantity");
                }
            }
        }
        private decimal _IssueQty;
        public decimal IssueQty
        {
            get
            {
                return _IssueQty;
            }
            set
            {
                if (value != _IssueQty)
                {
                    _IssueQty = value;
                    OnPropertyChanged("IssueQty");
                    OnPropertyChanged("PurchaseRate");
                    OnPropertyChanged("PurchaseRateAmt");
                    OnPropertyChanged("MRP");
                    OnPropertyChanged("MRPAmt");
                    OnPropertyChanged("ItemVATAmount");
                    OnPropertyChanged("ItemTaxAmount");
                    OnPropertyChanged("NetAmount");



                    OnPropertyChanged("ItemTotalAmount");
                    OnPropertyChanged("ItemVATAmount");


                    //if (value.ToString().Length > 5)
                    //{
                    //    _IssueQty = 0;
                    //    throw new ValidationException("Issued Quantity Length Should Not Be Greater Than 5 Digits.");
                    //}
                    //else if (value < 0)
                    //    _IssueQty = 1;
                    //else
                    //    _IssueQty = value;
                    //if (_IssueQty.HasValue)
                    //    _IssueQty = Math.Round(_IssueQty.Value, 3);

                    //OnPropertyChanged("IssueQty");
                    //OnPropertyChanged("ItemTotalAmount");
                    //OnPropertyChanged("ItemVATAmount");
                    ////ItemTotalAmount = IssueQty* PurchaseRate;
                    ////ItemVATAmount = ItemTotalAmount * (ItemVATPercentage / 100);
                    ////Added By Pallavi
                    ////if (IndentQty != null && IndentQty != 0)
                    ////    BalanceQty = IndentQty - IssueQty;
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
                    OnPropertyChanged("PurchaseRateAmt");
                    OnPropertyChanged("MRPAmt");
                    OnPropertyChanged("ItemVATAmount");
                    OnPropertyChanged("ItemTaxAmount");
                    OnPropertyChanged("NetAmount");

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
                    OnPropertyChanged("AbatedMRP");
                    OnPropertyChanged("MRPAmt");
                    OnPropertyChanged("PurchaseRateAmt");
                    OnPropertyChanged("ItemVATAmount");
                    OnPropertyChanged("ItemTaxAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }


        private decimal? _ItemTotalAmount;
        public decimal? ItemTotalAmount
        {
            get
            {
                //decimal _temp = Convert.ToDecimal(BaseQuantity);
                //return _temp * PurchaseRate;  //
                return _ItemTotalAmount; //* PurchaseRate;

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

        private decimal? _ItemTotalCost;
        public decimal? ItemTotalCost
        {
            get
            {
                return _ItemTotalCost;

            }
            set
            {
                if (value != _ItemTotalCost)
                {
                    _ItemTotalCost = value;
                    OnPropertyChanged("ItemTotalCost");
                }
            }
        }


        private decimal? _UnverifiedStock;
        public decimal? UnverifiedStock
        {
            get
            {
                return _UnverifiedStock;
            }
            set
            {
                if (value != _UnverifiedStock)
                {
                    _UnverifiedStock = value;
                    OnPropertyChanged("UnverifiedStock");
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
                    OnPropertyChanged("AbatedMRP");
                    OnPropertyChanged("ItemVATAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        #region ItemVATAmount Commented By Ashish Z. dated 25-05-2016
        //private decimal? _ItemVATAmount;
        //public decimal? ItemVATAmount
        //{
        //    get
        //    {
        //        return ItemTotalAmount;// return ItemTotalAmount * (ItemVATPercentage / 100);
        //    }
        //    set
        //    {
        //        if (value != _ItemVATAmount)
        //        {
        //            _ItemVATAmount = value;
        //            OnPropertyChanged("ItemVATAmount");
        //        }
        //    }
        //}
        #endregion

        private decimal? _ItemVATAmount;
        public decimal? ItemVATAmount
        {
            get
            {
                if (_ItemVATPercentage != 0)
                {
                    if (_GRNItemVatType == 2)
                    {
                        if (_GRNItemVatApplicationOn == 1)
                        {
                            _ItemVATAmount = ((_PurchaseRateAmt * _ItemVATPercentage) / 100);  //_PurchaseRateAmt- _CDiscountAmount - _SchDiscountAmount
                            return _ItemVATAmount;
                        }
                        else
                        {
                            _ItemVATAmount = ((_MRPAmt * _ItemVATPercentage) / 100);  //_MRPAmt - _CDiscountAmount - _SchDiscountAmount
                            return _ItemVATAmount;
                        }

                    }
                    else if (_GRNItemVatType == 1)
                    {
                        if (_GRNItemVatApplicationOn == 1)
                        {
                            decimal? calculation = _PurchaseRateAmt; //- _CDiscountAmount - _SchDiscountAmount
                            _ItemVATAmount = ((calculation) / (100 + _ItemVATPercentage) * 100);
                            _ItemVATAmount = calculation - _ItemVATAmount;
                            return _ItemVATAmount;
                        }
                        else
                        {
                            decimal? calculation2 = ((_MRPAmt) / (100 + _ItemVATPercentage) * 100);
                            _ItemVATAmount = (calculation2 * (_ItemVATPercentage / 100));
                            return _ItemVATAmount;
                        }
                    }
                    else
                    {
                        _ItemVATAmount = ((_PurchaseRateAmt * _ItemVATPercentage) / 100);  //_VATAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _VATPercent) / 100);
                        return _ItemVATAmount;
                    }
                }
                return _ItemVATAmount = 0;
            }
            set
            {
                if (value != _ItemVATAmount)
                {
                    _ItemVATAmount = value;
                    OnPropertyChanged("ItemVATAmount");
                    OnPropertyChanged("ItemVATPercentage");
                    OnPropertyChanged("NetAmount");
                }
            }
        }


        private float? _ConversionFactor;
        public float? ConversionFactor
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


        private decimal? _NetPayBeforeVATAmount;
        public decimal? NetPayBeforeVATAmount
        {
            get
            {
                return _NetPayBeforeVATAmount;
            }
            set
            {
                if (value != _NetPayBeforeVATAmount)
                {
                    _NetPayBeforeVATAmount = value;
                    OnPropertyChanged("NetPayBeforeVATAmount");
                }
            }
        }


        private decimal? _VATInclusive;
        public decimal? VATInclusive
        {
            get
            {
                return _VATInclusive;
            }
            set
            {
                if (value != _VATInclusive)
                {
                    _VATInclusive = value;
                    OnPropertyChanged("VATInclusive");
                }
            }
        }


        private decimal? _TotalForVAT;
        public decimal? TotalForVAT
        {
            get
            {
                return _TotalForVAT;
            }
            set
            {
                if (value != _TotalForVAT)
                {
                    _TotalForVAT = value;
                    OnPropertyChanged("TotalForVAT");
                }
            }
        }


        private decimal? _AvailableStock;
        public decimal? AvailableStock
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
                    OnPropertyChanged("IsEnable");
                }
            }
        }
        //By Anjli............................................................


        //private bool _IsIndent;
        //public bool IsIndent
        //{
        //    get
        //    {
        //        return _IsIndent;
        //    }
        //    set
        //    {
        //        if (value != _IsIndent)
        //        {
        //            _IsIndent = value;
        //            OnPropertyChanged("IsIndent");

        //        }
        //    }
        //}


        private int _IsIndent;
        public int IsIndent
        {
            get
            {
                return _IsIndent;
            }
            set
            {
                if (value != _IsIndent)
                {
                    _IsIndent = value;
                    OnPropertyChanged("IsIndent");

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
                    //OnPropertyChanged("UOM");
                }
            }
        }
        //IsIndent 0 for PR
        //IsIndent 1 For Indent
        //IsIndent 2 For Quarantine

        //...................................................................

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

        private String _IndentUOM;
        public String IndentUOM
        {
            get
            {
                return _IndentUOM;
            }
            set
            {
                if (value != _IndentUOM)
                {
                    _IndentUOM = value;
                    OnPropertyChanged("IndentUOM");
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

        # endregion


        private string _Rack;
        public string Rack
        {
            get
            {
                return _Rack;
            }
            set
            {
                if (value != _Rack)
                {
                    _Rack = value;
                    OnPropertyChanged("Rack");
                }
            }
        }

        private string _Shelf;
        public string Shelf
        {
            get
            {
                return _Shelf;
            }
            set
            {
                if (value != _Shelf)
                {
                    _Shelf = value;
                    OnPropertyChanged("Shelf");
                }
            }
        }

        private string _Bin;
        public string Bin
        {
            get
            {
                return _Bin;
            }
            set
            {
                if (value != _Bin)
                {
                    _Bin = value;
                    OnPropertyChanged("Bin");
                }
            }
        }

        private string _GRNNo;
        public string GRNNo
        {
            get
            {
                return _GRNNo;
            }
            set
            {
                if (value != _GRNNo)
                {
                    _GRNNo = value;
                    OnPropertyChanged("GRNNo");
                }
            }
        }

        private decimal? _GRNQty;
        public decimal? GRNQty
        {
            get
            {
                return _GRNQty;
            }
            set
            {
                if (value != _GRNQty)
                {
                    _GRNQty = value;
                    OnPropertyChanged("GRNQty");
                }
            }
        }

        private string _GRNQtyUOM;
        public string GRNQtyUOM
        {
            get
            {
                return _GRNQtyUOM;
            }
            set
            {
                if (value != _GRNQtyUOM)
                {
                    _GRNQtyUOM = value;
                    OnPropertyChanged("GRNQtyUOM");
                }
            }
        }

        private bool _IsGRNFreeItem;
        public bool IsGRNFreeItem
        {
            get
            {
                return _IsGRNFreeItem;
            }
            set
            {
                if (value != _IsGRNFreeItem)
                {
                    _IsGRNFreeItem = value;
                    OnPropertyChanged("IsGRNFreeItem");
                }
            }
        }


        public long GRNID { get; set; }
        public long GRNUnitID { get; set; }
        public long GRNDetailID { get; set; }
        public long GRNDetailUnitID { get; set; }

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

        private decimal? _ItemTaxPercentage;
        public decimal? ItemTaxPercentage
        {
            get
            {
                return _ItemTaxPercentage;
            }
            set
            {
                if (value != _ItemTaxPercentage)
                {
                    _ItemTaxPercentage = value;
                    OnPropertyChanged("ItemTaxPercentage");
                    OnPropertyChanged("ItemTaxAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }


        private decimal? _ItemTaxAmount;
        public decimal? ItemTaxAmount
        {
            get
            {
                if (_IssueQty > 0)
                {
                    if (_ItemTaxPercentage > 0)
                    {
                        if (_OtherGRNItemTaxType == 2)
                        {
                            if (_OtherGRNItemTaxApplicationOn == 1)
                            {
                                _ItemTaxAmount = ((_PurchaseRateAmt * _ItemTaxPercentage) / 100);  //(((_PurchaseRateAmt - _CDiscountAmount - _SchDiscountAmount) * _ItemTaxPercentage) / 100);
                                return _ItemTaxAmount;
                            }
                            else
                            {
                                _ItemTaxAmount = ((_MRPAmt * _ItemTaxPercentage) / 100); //(((_MRPAmt - _CDiscountAmount - _SchDiscountAmount) * _ItemTaxPercentage) / 100);
                                return _ItemTaxAmount;
                            }
                        }
                        else if (_OtherGRNItemTaxType == 1)
                        {
                            if (_OtherGRNItemTaxApplicationOn == 1)
                            {
                                decimal? calculation = _PurchaseRateAmt;//- _CDiscountAmount - _SchDiscountAmount;
                                _ItemTaxAmount = ((calculation) / (100 + _ItemTaxPercentage) * 100);
                                _ItemTaxAmount = calculation - _ItemTaxAmount;
                                return _ItemTaxAmount;
                            }
                            else
                            {
                                decimal? calculation2 = ((_MRPAmt) / (100 + _ItemTaxPercentage) * 100);
                                _ItemTaxAmount = (calculation2 * (_ItemTaxPercentage / 100));
                                return _ItemTaxAmount;
                            }
                        }
                        else
                        {
                            _ItemTaxAmount = ((_PurchaseRateAmt * _ItemTaxPercentage) / 100);  //(((_PurchaseRateAmt - _CDiscountAmount - _SchDiscountAmount) * _ItemTaxPercentage) / 100);
                            return _ItemTaxAmount;
                        }
                    }
                    return _ItemTaxAmount = 0;
                }
                return _ItemTaxAmount;
            }
            set
            {
                if (value != _ItemTaxAmount)
                {
                    _ItemTaxAmount = value;
                    OnPropertyChanged("ItemTaxAmount");
                }
            }
        }

        #endregion

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
            get { return _MainMRP; }
            set
            {
                if (_MainMRP != value)
                {
                    _MainMRP = value;
                    OnPropertyChanged("MainMRP");
                }
            }
        }

        private decimal? _PurchaseRateAmt;
        public decimal? PurchaseRateAmt
        {
            get
            {
                _PurchaseRateAmt = _PurchaseRate * _IssueQty;
                return _PurchaseRateAmt;
            }
            set
            {
                if (value != _PurchaseRateAmt)
                {
                    _PurchaseRateAmt = value;
                    OnPropertyChanged("PurchaseRateAmt");
                }
            }
        }

        private decimal? _MRPAmt;
        public decimal? MRPAmt
        {
            get
            {
                _MRPAmt = _MRP * _IssueQty;
                return _MRPAmt;
            }
            set
            {
                if (value != _MRPAmt)
                {
                    _MRPAmt = value;
                    OnPropertyChanged("MRPAmt");
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
                    _AbatedMRP = Convert.ToDouble((_MRP) - ((_MRP * _ItemVATPercentage) / 100));
                    return _AbatedMRP;
                }
                else if (_GRNItemVatType == 1)  // Inclusive 
                {
                    _AbatedMRP = Convert.ToDouble(((_MRP) / (_ItemVATPercentage + 100)) * 100);
                    return _AbatedMRP;
                }
                else
                {
                    _AbatedMRP = Convert.ToDouble((_MRP) - ((_MRP * _ItemVATPercentage) / 100));
                    return _AbatedMRP;
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

        private double _NetAmount;
        public double NetAmount
        {
            get
            {
                if (_ItemVATPercentage != 0)
                {
                    //check type inclusive or exclusive
                    if (_GRNItemVatType == 2)
                    {
                        _NetAmount = Convert.ToDouble(_PurchaseRateAmt + _ItemVATAmount + _ItemTaxAmount);  //(_PurchaseRateAmt - _SchDiscountAmount - _CDiscountAmount) + _ItemVATAmount + _ItemTaxAmount;
                        return _NetAmount;
                    }
                    else
                    {

                        if (_GRNItemVatApplicationOn == 1) //for Applicable on CP
                        {
                            _NetAmount = Convert.ToDouble(_PurchaseRateAmt);   // _PurchaseRateAmt - _SchDiscountAmount - _CDiscountAmount;
                            return _NetAmount;
                        }
                        else  //for Applicable on MRP
                        {
                            _NetAmount = Convert.ToDouble(_PurchaseRateAmt + _ItemVATAmount + _ItemTaxAmount);  //(_PurchaseRateAmt - _SchDiscountAmount - _CDiscountAmount) + _ItemVATAmount + _ItemTaxAmount;
                            return _NetAmount;
                        }

                    }
                }
                else
                {
                    if (_OtherGRNItemTaxType == 2)
                    {
                        _NetAmount = Convert.ToDouble(_PurchaseRateAmt + _ItemVATAmount + _ItemTaxAmount);   //(_PurchaseRateAmt - _SchDiscountAmount - _CDiscountAmount) + _ItemVATAmount + _ItemTaxAmount;
                        return _NetAmount;
                    }
                    else
                    {
                        if (_GRNItemVatApplicationOn == 1) //for Applicable on CP
                        {
                            _NetAmount = Convert.ToDouble(_PurchaseRateAmt);   //_PurchaseRateAmt - _SchDiscountAmount - _CDiscountAmount;
                            return _NetAmount;
                        }
                        else  //for Applicable on MRP
                        {
                            _NetAmount = Convert.ToDouble(_PurchaseRateAmt + _ItemVATAmount + _ItemTaxAmount);  //(_PurchaseRateAmt - _SchDiscountAmount - _CDiscountAmount) + _ItemVATAmount + _ItemTaxAmount;
                            return _NetAmount;
                        }
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

    }
}
