using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace PalashDynamics.ValueObjects.Inventory.Quotation
{
    public class clsQuotaionVO:INotifyPropertyChanged,IValueObject
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

        #region MasterProperties

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

        private string _QuotationNo;
        public string QuotationNo
        {
            get { return _QuotationNo; }
            set
            {
                if (_QuotationNo != value)
                {
                    _QuotationNo = value;
                    OnPropertyChanged("QuotationNo");
                }
            }
        }

        private long _EnquiryID;
        public long EnquiryID
        {
            get { return _EnquiryID; }
            set
            {
                if (_EnquiryID != value)
                {
                    _EnquiryID = value;
                    OnPropertyChanged("EnquiryID");
                }
            }
        }


        private string _Notes;
        public string Notes
        {
            get { return _Notes; }
            set
            {
                if (_Notes != value)
                {
                    _Notes = value;
                    OnPropertyChanged("Notes");
                }
            }
        }


        private double _TotalAmount;
        public double TotalAmount
        {
            get { return _TotalAmount; }
            set
            {
                if (_TotalAmount != value)
                {
                    _TotalAmount = value;
                    OnPropertyChanged("TotalAmount");
                }
            }
        }

        private double _TotalNet;
        public double TotalNet
        {
            get { return _TotalNet; }
            set
            {
                if (_TotalNet != value)
                {
                    _TotalNet = value;
                    OnPropertyChanged("TotalNet");
                }
            }
        }


        private double _TotalConcession;
        public double TotalConcession
        {
            get { return _TotalConcession; }
            set
            {
                if (_TotalConcession != value)
                {
                    _TotalConcession = value;
                    OnPropertyChanged("TotalConcession");
                }
            }
        }


        private double _TotalTAX;
        public double TotalTAX
        {
            get { return _TotalTAX; }
            set
            {
                if (_TotalTAX != value)
                {
                    _TotalTAX = value;
                    OnPropertyChanged("TotalTAX");
                }
            }
        }
        private double _Other;
        public double Other
        {
            get { return _Other; }
            set
            {
                if (_Other != value)
                {
                    _Other = value;
                    OnPropertyChanged("Other");
                }
            }
        }
        private string _Remarks = "";
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                if (_Remarks != value)
                {
                    _Remarks = value;
                    OnPropertyChanged("Remarks");
                }
            }
        }

          
        #endregion

      

        #region Quotation Terms And Condition

                private long _TermsConditionID;
                public long TermsConditionID
            {
                get { return _TermsConditionID; }
                set
                {
                    if (_TermsConditionID != value)
                    {
                        _TermsConditionID = value;
                        OnPropertyChanged("TermsConditionID");
                    }
                }
            }
            #endregion


                private List<clsQuotationDetailsVO> _Items;
                public List<clsQuotationDetailsVO> Items 
                {
                    get
                    {
                        return _Items;
                    }
                    set
                    {
                        _Items = value;
                        OnPropertyChanged("Items");
                    }
                }

        #region Common Properties

         private string _LinkServer;
        public string LinkServer
        {
            get { return _LinkServer; }
            set
            {
                if (_LinkServer != value)
                {
                    _LinkServer = value;
                    OnPropertyChanged("LinkServer");
                }
            }
        }

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

        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
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

        public object IsPagingEnabled { get; set; }

        public object StartIndex { get; set; }

        public object MinRows { get; set; }

        public int TotalRows { get; set; }

        public string Supplier { get; set; }

        public DateTime ValidityDate { get; set; }

        public string QuotationFrom { get; set; }
        public long LeadTime { get; set; }

        #endregion

        public long ID { get; set; }

       
    }
    public class clsQuotationDetailsVO : INotifyPropertyChanged, IValueObject
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



        #region Detail Properties

        private long _QuotationID;
        public long QuotationID
        {
            get { return _QuotationID; }
            set
            {
                if (_QuotationID != value)
                {
                    _QuotationID = value;
                    OnPropertyChanged("QuotationID");
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


        private double _Quantity;
        public double Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {
                    //if (value < 0)
                    //    Quantity = 0;
                    //else
                    //    _Quantity = value;
                    //OnPropertyChanged("Quantity");
                    //OnPropertyChanged("Amount");
                    //OnPropertyChanged("ConcessionAmount");
                    //OnPropertyChanged("ExciseAmount");
                    //OnPropertyChanged("TAXAmount");
                    //OnPropertyChanged("NetAmount");


                    if (value > 0)
                    {
                        //if (value.ToString().Length > 5)
                        //{
                        //    throw new ValidationException("Quantity Length Should Not Be Greater Than 5 Digits.");
                        //}
                        //else
                        //{
                            _Quantity = value;
                            OnPropertyChanged("Quantity");
                            OnPropertyChanged("Amount");
                            OnPropertyChanged("ConcessionAmount");
                            OnPropertyChanged("ExciseAmount");
                            OnPropertyChanged("TAXAmount");
                            OnPropertyChanged("NetAmount");
                       // }
                    }
                    else
                        _Quantity = value;
                }
            }
        }

        private double _Rate;
        public double Rate
        {
            get { return _Rate; }
            set
            {
                if (_Rate != value)
                {
                    _Rate = value;
                    OnPropertyChanged("Rate");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("ExciseAmount");
                    OnPropertyChanged("TAXAmount");
                   OnPropertyChanged("NetAmount");

                }
            }
        }

        private double _Amount;
        public double Amount
        {
            get { return _Amount = _Rate * _Quantity; }
            set
            {
                if (_Amount != value)
                {
                    _Amount = value;
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("ExciseAmount");
                    OnPropertyChanged("TAXAmount");
                    OnPropertyChanged("NetAmount");
                    //OnPropertyChanged("TotalAmount");
                }
            }
        }

        private double _TAXPercent;
        public double TAXPercent
        {
            get { return _TAXPercent; }
            set
            {
                if (_TAXPercent != value)
                {
                    _TAXPercent = value;
                    OnPropertyChanged("TAXPercent");
                    OnPropertyChanged("TAXAmount");
                    //OnPropertyChanged("Amount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _TAXAmount;
        public double TAXAmount
        {
            get
            {
                //if (_TAXPercent != 0)
                //{
                    return _TAXAmount = ((_Amount * _TAXPercent) / 100);
                //}
                //else
                //    return _TAXAmount;
            }
            set
            {
                if (_TAXAmount != value)
                {

                    _TAXAmount = value;
                    //_TAXPercent = 0;
                    OnPropertyChanged("TAXAmount");
                    //OnPropertyChanged("Amount");
                    //OnPropertyChanged("VATPercent");
                    OnPropertyChanged("NetAmount");
                    //OnPropertyChanged("TotalTAX");
                }
            }
        }

        private double _ExcisePercent;
        public double ExcisePercent
        {
            get { return _ExcisePercent; }
            set
            {
                if (_ExcisePercent != value)
                {
                    _ExcisePercent = value;
                    OnPropertyChanged("ExcisePercent");
                    OnPropertyChanged("ExciseAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("NetAmount");
                    //OnPropertyChanged("TotalNet");
                    

                }
            }
        }

        private double _ExciseAmount;
        public double ExciseAmount
        {
            get
            {
                //if (_ExcisePercent != 0)
                //{
                    return _ExciseAmount = ((_Amount * _ExcisePercent) / 100);
                //}
                //else
                //    return _ExciseAmount;
            }
            set
            {
                if (_ExciseAmount != value)
                {

                    _ExciseAmount = value;
                    //_ExcisePercent = 0;
                    OnPropertyChanged("ExciseAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("NetAmount");
                    //OnPropertyChanged("TotalNet");
                    //OnPropertyChanged("TotalExcise");
                    
                }
            }
        }





        private double _ConPercent;
        public double ConPercent
        {
            get { return _ConPercent; }
            set
            {
                if (_ConPercent != value)
                {
                    _ConPercent = value;
                    //OnPropertyChanged("ConcessionPercent");
                    //OnPropertyChanged("ConcessionAmount");
                    //OnPropertyChanged("NetAmount");
                  
                }
            }
        }


        private double _TAXPer;
        public double TAXPer
        {
            get { return _TAXPer; }
            set
            {
                if (_TAXPer != value)
                {
                    _TAXPer = value;
                    //OnPropertyChanged("ConcessionPercent");
                    //OnPropertyChanged("ConcessionAmount");
                    //OnPropertyChanged("NetAmount");
                }
            }
        }


        private double _ConcessionPercent;
        public double ConcessionPercent
        {
            get { return _ConcessionPercent; }
            set
            {
                if (_ConcessionPercent != value)
                {
                    _ConcessionPercent = value;
                    OnPropertyChanged("ConcessionPercent");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("NetAmount");
                    //OnPropertyChanged("TotalNet");
                }
            }
        }

        private double _ConcessionAmount;
        public double ConcessionAmount
        {
            get
            {
                //if (_ConcessionPercent != 0)
                //{
                    return _ConcessionAmount = ((_Amount * _ConcessionPercent) / 100);
                //}
                //else
                //    return _ConcessionAmount;
            }
            set
            {
                if (_ConcessionAmount != value)
                {

                    _ConcessionAmount = value;
                    //_ConcessionPercent = 0;
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("ConcessionPercent");
                    OnPropertyChanged("NetAmount");
                    //OnPropertyChanged("TotalConcession");
                }
            }
        }


        private double _NetAmount;
        public double NetAmount
        {
            get
            {
                //Commented By Pallavi
               // return _NetAmount = (_Amount + _TAXAmount + _ExciseAmount) - _ConcessionAmount;

                return _NetAmount = (_Amount - _ConcessionAmount) + _TAXAmount + _ExciseAmount;
            }
            set
            {
                if (_NetAmount != value)
                {
                    _NetAmount = value;
                    OnPropertyChanged("NetAmount");
                    //OnPropertyChanged("TotalNet");
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



        private string _Specification = "";
        public string Specification
        {
            get { return _Specification; }
            set
            {
                if (_Specification != value)
                {
                    _Specification = value;
                    OnPropertyChanged("Specification");
                }
            }
        }


       

        private string _PUM = "";
        public string PUM
        {
            get { return _PUM; }
            set
            {
                if (_PUM != value)
                {
                    _PUM = value;
                    OnPropertyChanged("PUM");
                }
            }
        }


        

        #endregion
    }

    public class clsQuotationAttachmentsVO : IValueObject
    {
        //#region IValueObject Members

        //public string ToXml()
        //{
        //    return this.ToString();
        //}

        //#endregion
        public long ID { get; set; }
        public long QuotationID { get; set; }
        public long UnitID { get; set; }
        public bool Status{get;set;}
        public string Description { get; set; }
        public DateTime ValidityDate{get;set;}
        public long LeadTime{get;set;}
        public string AttachedFileName{get;set;}
        public byte[] AttachedFileContent{get;set;}
        public long AddedBy{get;set;}
        public string AddedOn{get;set;}
        public string ToXml()
        {
            throw new NotImplementedException();
        }
    }
}
