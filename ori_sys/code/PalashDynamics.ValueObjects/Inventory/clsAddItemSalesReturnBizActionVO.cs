using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.ValueObjects.Log;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsAddItemSalesReturnBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsAddIemSalesReturnBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        public long ItemSalesReturnID { get; set; }
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private clsItemSalesReturnVO objItems = new clsItemSalesReturnVO();
        public clsItemSalesReturnVO ItemMatserDetail
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

        private List<clsItemSalesReturnDetailsVO> objItemMaster = new List<clsItemSalesReturnDetailsVO>();
        public List<clsItemSalesReturnDetailsVO> ItemsDetail
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

        // For the Activity Log List by rohinee dated 26/9/16
        private List<LogInfo> _LogInfoList = null;
        public List<LogInfo> LogInfoList
        {
            get { return _LogInfoList; }
            set { _LogInfoList = value; }
        }

        #region Refund to Advance 22042017

        public bool IsRefundToAdvance { get; set; }
        public long RefundToAdvancePatientID { get; set; }
        public long RefundToAdvancePatientUnitID { get; set; }

        public bool IsExchangeMaterial { get; set; }

        #endregion
    }


    public class clsItemSalesReturnVO : IValueObject, INotifyPropertyChanged
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



        #endregion


        #region Property Declaration Section

        private List<clsItemSalesDetailsVO> _Items = new List<clsItemSalesDetailsVO>();
        public List<clsItemSalesDetailsVO> Items
        {
            get { return _Items; }
            set
            {
                if (_Items != value)
                {
                    _Items = value;
                    OnPropertyChanged("Items");
                }
            }
        }
        private clsRefundVO _RefundDetails = new clsRefundVO();
        public clsRefundVO RefundDetails
        {
            get { return _RefundDetails; }
            set
            {

                _RefundDetails = value;


            }
        }

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

        private long _ItemSalesID;
        public long ItemSalesID
        {
            get { return _ItemSalesID; }
            set
            {
                if (_ItemSalesID != value)
                {
                    _ItemSalesID = value;
                    OnPropertyChanged("ItemSalesID");
                }
            }
        }

        private long _CostingDivisionID;
        public long CostingDivisionID
        {
            get { return _CostingDivisionID; }
            set
            {
                if (_CostingDivisionID != value)
                {
                    _CostingDivisionID = value;
                    OnPropertyChanged("CostingDivisionID");
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


        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }
            }
        }

        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {
                if (_VisitID != value)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }

        private string _ItemSaleReturnNo;
        public string ItemSaleReturnNo
        {
            get { return _ItemSaleReturnNo; }
            set
            {
                if (_ItemSaleReturnNo != value)
                {
                    _ItemSaleReturnNo = value;
                    OnPropertyChanged("ItemSaleReturnNo");
                }
            }
        }

        private string _Remarks;
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

        private double _ConcessionPercentage;
        public double ConcessionPercentage
        {
            get { return _ConcessionPercentage; }
            set
            {
                if (_ConcessionPercentage != value)
                {
                    _ConcessionPercentage = value;
                    OnPropertyChanged("ConcessionPercentage");
                }
            }
        }

        private double _ConcessionAmount;
        public double ConcessionAmount
        {
            get { return _ConcessionAmount; }
            set
            {
                if (_ConcessionAmount != value)
                {
                    _ConcessionAmount = value;
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("NetAmount");
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

                }
            }
        }

        private double _VATAmount;
        public double VATAmount
        {
            get { return _VATAmount; }
            set
            {
                if (_VATAmount != value)
                {
                    _VATAmount = value;
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
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
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _NetAmount;
        public double NetAmount
        {
            get
            {
                // return _NetAmount = _TotalAmount + _VATAmount - _ConcessionAmount;
                return _NetAmount; ;
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


        private double _TotalReturnAmount;
        public double TotalReturnAmount
        {
            get { return _TotalReturnAmount; }
            set
            {
                if (_TotalReturnAmount != value)
                {
                    _TotalReturnAmount = value;
                    OnPropertyChanged("TotalReturnAmount");
                }
            }
        }

        //By Anjali........................
        private double _CalculatedNetAmount;
        public double CalculatedNetAmount
        {
            get
            {
                // return _NetAmount = _TotalAmount + _VATAmount - _ConcessionAmount;
                return _CalculatedNetAmount; ;
            }
            set
            {
                if (_CalculatedNetAmount != value)
                {
                    _CalculatedNetAmount = value;
                    OnPropertyChanged("CalculatedNetAmount");
                }
            }
        }

        //................................
        //Added By Bhushanp For GST 14072017
        private double _TotalSGST;
        public double TotalSGST
        {
            get { return _TotalSGST; }
            set
            {
                if (_TotalSGST != value)
                {
                    _TotalSGST = value;
                    OnPropertyChanged("TotalSGST");
                    OnPropertyChanged("NetAmount");
                }
            }
        }
        private double _TotalCGST;
        public double TotalCGST
        {
            get { return _TotalCGST; }
            set
            {
                if (_TotalCGST != value)
                {
                    _TotalCGST = value;
                    OnPropertyChanged("TotalCGST");
                    OnPropertyChanged("NetAmount");
                }
            }
        }
        #endregion
    }

    public class clsItemSalesReturnDetailsVO : IValueObject, INotifyPropertyChanged
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

        private long _ItemSaleId;
        public long ItemSaleId
        {
            get { return _ItemSaleId; }
            set
            {
                if (_ItemSaleId != value)
                {
                    _ItemSaleId = value;
                    OnPropertyChanged("ItemSaleId");
                }
            }
        }

        private long _ItemSaleReturnId;
        public long ItemSaleReturnId
        {
            get { return _ItemSaleReturnId; }
            set
            {
                if (_ItemSaleReturnId != value)
                {
                    _ItemSaleReturnId = value;
                    OnPropertyChanged("ItemSaleReturnId");
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


        public DateTime? ExpiryDate { get; set; }

        private double _Quantity;
        public double Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {
                    
                    
                    _Quantity = value;
                    //OnPropertyChanged("Quantity");
                    //OnPropertyChanged("Amount");
                    //OnPropertyChanged("NetAmount");+
                }
            }
        }

        private double _BaseSaleQuantity;
        public double BaseSaleQuantity
        {
            get { return _BaseSaleQuantity; }
            set
            {
                if (_BaseSaleQuantity != value)
                {


                    _BaseSaleQuantity = value;
                    //OnPropertyChanged("Quantity");
                    //OnPropertyChanged("Amount");
                    //OnPropertyChanged("NetAmount");
                }
            }
        }
      
        private double _SaleQuantity;
        public double SaleQuantity
        {
            get { return _SaleQuantity; }
            set
            {
                if (_SaleQuantity != value)
                {


                    _SaleQuantity = value;
                    //OnPropertyChanged("Quantity");
                    //OnPropertyChanged("Amount");
                    //OnPropertyChanged("NetAmount");
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
        
        //private double _MRP;
        //public double MRP
        //{
        //    get { return _MRP; }
        //    set
        //    {
        //        if (_MRP != value)
        //        {
        //            _MRP = value;
        //            OnPropertyChanged("MRP");
        //            OnPropertyChanged("Amount");
        //            OnPropertyChanged("NetAmount");

        //        }
        //    }
        //}


        private double _MRP;
        public double MRP
        {
            get { return _MRP; }
            set
            {
                if (_MRP != value)
                {
                    if (value < 0)
                        value = 0;

                    _MRP = value;
                    OnPropertyChanged("MRP");
                    ////
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("NetAmount");

                    /* OnPropertyChanged("Amount");
                     #region Added By pallavi
                     OnPropertyChanged("ConcessionPercentage");
                     OnPropertyChanged("ConcessionAmount");
                     OnPropertyChanged("VATPercent");
                     OnPropertyChanged("VATAmount");
                     #endregion
                     OnPropertyChanged("NetAmount");*/

                }
            }
        }


        //private double _Amount;
        //public double Amount
        //{
        //    get { return _Amount = _MRP * _ReturnQuantity; }
        //    set
        //    {
        //        if (_Amount != value)
        //        {
        //            _Amount = value;
        //            OnPropertyChanged("Amount");
        //            OnPropertyChanged("ConcessionAmount");
        //            OnPropertyChanged("VATAmount");
        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}

        private double _Amount;
        public double Amount
        {
            get { return _Amount = _MRP * _ReturnQuantity; }
            set
            {
                if (_Amount != value)
                {
                    _Amount = value;
                    OnPropertyChanged("Amount");
                    ////
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("NetAmount");

                    /* #region Added By Pallavi
                     OnPropertyChanged("ConcessionPercentage");
                     OnPropertyChanged("ConcessionAmount");
                     OnPropertyChanged("VATPercent");
                     OnPropertyChanged("VATAmount");
                     #endregion
                     OnPropertyChanged("NetAmount");*/
                }
            }
        }

        //private double _VATPercent;
        //public double VATPercent
        //{
        //    get { return _VATPercent; }
        //    set
        //    {
        //        if (_VATPercent != value)
        //        {
        //            _VATPercent = value;
        //            OnPropertyChanged("VATPercent");
        //            OnPropertyChanged("VATAmount");
        //            OnPropertyChanged("Amount");
        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}

        private double _VATPercent;
        public double VATPercent
        {
            get { return _VATPercent; }
            set
            {
                if (_VATPercent != value)
                {
                    if (value < 0)
                        value = 0;
                    if (value > 100)
                        value = 100;

                    _VATPercent = value;
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("ConcessionPercentage");
                }
            }
        }

        //private double _VATAmount;
        //public double VATAmount
        //{
        //    get
        //    {
        //        if (_VATPercent != 0)
        //        {
        //            return _VATAmount = ((_Amount - _ConcessionAmount) * _VATPercent / 100);
        //                //((_Amount * _VATPercent) / 100);
        //        }
        //        else
        //            return _VATAmount;
        //    }
        //    set
        //    {
        //        if (_VATAmount != value)
        //        {

        //            _VATAmount = value;
        //            OnPropertyChanged("VATAmount");
        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}
        public bool IsItemSaleReturn { get; set; }
        public bool InclusiveOfTax { get; set; }

        private double _VATAmount;
        //public double VATAmount
        //{
        //    get
        //    {
        //        if (_VATPercent > 0)
        //            if (InclusiveOfTax == true)
        //            {
        //                return _VATAmount = ((_Amount - _ConcessionAmount) * _VATPercent / 100);
        //                //((_Amount * _VATPercent) / 100);
        //            }
        //            else
        //            {
        //                if (_ConcessionPercentage > 0)
        //                {
        //                    return _VATAmount = (((((MRP / (100 + (_VATPercent))) * 100)) - (((MRP / (100 + (_VATPercent))) * 100) * (_ConcessionPercentage / 100))) * (_VATPercent / 100)) * _Quantity;
        //                }
        //                else
        //                {
        //                    return _VATAmount = (((((MRP / (100 + (_VATPercent))) * 100)) - (((MRP / (100 + (_VATPercent))) * 100) * _ConcessionPercentage)) * (_VATPercent / 100)) * _Quantity;
        //                }
        //            }
        //        else
        //            return _VATAmount = 0;

        //    }
        //    set
        //    {
        //        if (_VATAmount != value)
        //        {
        //            if (value < 0)
        //                value = 0;
        //            _VATAmount = value;
        //            //_VATPercent = (_Quantity * _MRP) / _VATAmount;

        //            if (_VATAmount > 0 && (_Amount - ConcessionAmount) > 0)
        //                if (InclusiveOfTax == true)
        //                {
        //                    if (IsItemSaleReturn == false)
        //                    {
        //                        _VATPercent = (_VATAmount * 100) / (_Amount - ConcessionAmount);
        //                    }
        //                }
        //                else
        //                {
        //                }
        //            else
        //                _VATPercent = 0;


        //            //commented by pallavi
        //            //_VATPercent = 0;
        //            OnPropertyChanged("VATAmount");
        //            OnPropertyChanged("VATPercent");
        //            OnPropertyChanged("Amount");
        //            OnPropertyChanged("NetAmount");
        //            OnPropertyChanged("ConcessionAmount");
        //            OnPropertyChanged("ConcessionPercentage");
        //        }
        //    }
        //}

        //private double _ConcessionPercentage;
        //public double ConcessionPercentage
        //{
        //    get { return _ConcessionPercentage; }
        //    set
        //    {
        //        if (_ConcessionPercentage != value)
        //        {
        //            _ConcessionPercentage = value;
        //            OnPropertyChanged("ConcessionPercentage");
        //            OnPropertyChanged("ConcessionAmount");
        //            OnPropertyChanged("VATAmount");
        //            OnPropertyChanged("Amount");
        //            OnPropertyChanged("NetAmount");
                 
        //        }
        //    }
        //}

        //private double _ConcessionAmount;
        //public double ConcessionAmount
        //{
        //    get
        //    {
        //        if (_ConcessionPercentage != 0)
        //        {
        //            return _ConcessionAmount = ((_Amount * _ConcessionPercentage) / 100);
                    
        //        }
        //        else
        //            return _ConcessionAmount;
        //    }
        //    set
        //    {
        //        if (_ConcessionAmount != value)
        //        {
        //            _ConcessionAmount = value;
        //            OnPropertyChanged("ConcessionAmount");
        //            OnPropertyChanged("VATAmount");
        //            OnPropertyChanged("Amount");
        //            OnPropertyChanged("NetAmount");

        //        }
        //    }
        //}

        public double VATAmount
        {
            get
            {
                if (_VATPercent > 0)
                {
                    if (ItemVatType == 2)
                    {
                        //return _VATAmount = ((_Amount - _ConcessionAmount) * _VATPercent / 100);
                        return _VATAmount = ((_Amount) * _VATPercent / 100);
                    }
                    else
                    {
                        //double TotAmount = _Amount - _ConcessionAmount;
                        //_VATAmount = ((_Amount - _ConcessionAmount) / (100 + _VATPercent) * 100);
                        //return _VATAmount = TotAmount - _VATAmount;

                        if (_ConcessionPercentage > 0)
                        {
                            //return _VATAmount = (((((MRP / (100 + (_VATPercent))) * 100)) - (((MRP / (100 + (_VATPercent))) * 100) * (_ConcessionPercentage / 100))) * (_VATPercent / 100)) * _Quantity;
                            //return _VATAmount = (((((MRP / (100 + (_VATPercent))) * 100)) - (((MRP / (100 + (_VATPercent))) * 100) )) * (_VATPercent / 100)) * _Quantity;
                            return _VATAmount = ((MRP * _ReturnQuantity) - ((((MRP / (100 + (_VATPercent))) * 100)) * _ReturnQuantity));

                        }
                        else
                        {
                            return _VATAmount = ((MRP * _ReturnQuantity) - ((((MRP / (100 + (_VATPercent))) * 100)) * _ReturnQuantity));
                            // return _VATAmount = (((((MRP / (100 + (_VATPercent))) * 100)) - (((MRP / (100 + (_VATPercent))) * 100))) * (_VATPercent / 100)) * _Quantity;

                            //return _VATAmount = (((((MRP / (100 + (_VATPercent))) * 100)) - (((MRP / (100 + (_VATPercent))) * 100) * _ConcessionPercentage)) * (_VATPercent / 100)) * _Quantity;
                        }
                    }
                }
                else
                {
                    return _VATAmount = 0;
                }

            }
            set
            {
                if (_VATAmount != value)
                {
                    if (value < 0)
                        value = 0;
                    _VATAmount = value;
                  
                    //if (_VATAmount > 0 && (_Amount - ConcessionAmount) > 0 && _VATPercent <= 0)
                    //    _VATPercent = (_VATAmount * 100) / (_Amount - ConcessionAmount);
                    //else if (_VATPercent <= 0)
                    //    _VATPercent = 0;


                    if (_VATAmount > 0 && (_Amount - ConcessionAmount) > 0)
                        if (ItemVatType == 2)
                        {
                            //if (IsItemSaleReturn == false)
                            //{
                            //    _VATPercent = (_VATAmount * 100) / (_Amount - ConcessionAmount);
                            //}
                        }
                        else
                        {
                        }
                    else
                        _VATPercent = 0;



                    //commented by pallavi
                    //_VATPercent = 0;
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("ConcessionPercentage");
                }
            }
        }

        private double _ConcessionPercentage;
        public double ConcessionPercentage
        {
            get { return _ConcessionPercentage; }
            set
            {
                if (_ConcessionPercentage != value)
                {
                    if (value < 0)
                        value = 0;

                    if (value > 100)
                        value = 100;
                    _ConcessionPercentage = value;
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _ConcessionAmount;


        public double ConcessionAmount
        {
            get
            {
                if (_ConcessionPercentage > 0)
                {
                    if (_SGSTtaxtype == 2 || _CGSTtaxtype == 2 || _IGSTtaxtype == 2)
                    {
                        _ConcessionAmount = (_MRP / ((_SGSTPercent + _CGSTPercent + 100)) * 100) * (_ConcessionPercentage / 100) * _ReturnQuantity;
                    }
                    else
                    {
                        _ConcessionAmount = (_MRP / ((_SGSTPercent + _CGSTPercent + 100)) * 100) * (_ConcessionPercentage / 100) * _ReturnQuantity;
                    }
                }
                else
                    _ConcessionAmount = 0;

                _ConcessionAmount = Math.Round(_ConcessionAmount, 2);

                return _ConcessionAmount;

                #region Commeted on 05042018
                //if (_ConcessionPercentage > 0)
                //{
                //    if (_VATPercent != 0)
                //    {
                //        if (ItemVatType == 2)
                //        {
                //            _ConcessionAmount = (((_MRP * _ReturnQuantity) * _ConcessionPercentage) / 100);
                //            _ConcessionAmount = _ConcessionAmount * _ReturnQuantity;
                //        }
                //        else
                //        {
                //            _ConcessionAmount = ((MRP / (100 + (_VATPercent))) * 100) * (_ConcessionPercentage / 100);
                //            _ConcessionAmount = _ConcessionAmount * _ReturnQuantity;
                //        }
                //    }
                //    else if (_SGSTPercent != 0 || _CGSTPercent != 0 || _IGSTPercent != 0)
                //    {
                //        if (_SGSTtaxtype == 1 || _CGSTtaxtype == 1 || _IGSTtaxtype == 1)
                //        {
                //            _ConcessionAmount = (((MRP * _ReturnQuantity) / (1 + (_SGSTPercent / 100) + (_CGSTPercent / 100))) * (_ConcessionPercentage / 100));
                //        }
                //        else
                //        {
                //            _ConcessionAmount = (((_MRP * _ReturnQuantity) * _ConcessionPercentage) / 100);
                //            // _ConcessionAmount = _ConcessionAmount * _Quantity;
                //        }
                //    }
                //}
                //else
                //    _ConcessionAmount = 0;

                //_ConcessionAmount = Math.Round(_ConcessionAmount, 2);

                //return _ConcessionAmount;
                #endregion
            }
            set
            {
                if (_ConcessionAmount != value)
                {
                    if (value < 0)
                        value = 0;
                    //if(_ConcessionAmount !=)
                    //_ConcessionAmount =value;
                    _ConcessionAmount = Math.Round(value, 2);

                    //if (_ConcessionAmount > 0)
                    //{
                    //    if (IsItemSaleReturn == false)
                    //    {
                    //        //_ConcessionPercentage = ((_ConcessionAmount / _Quantity) * 100) / _MRP;
                    //        if (InclusiveOfTax == true)
                    //        {
                    //            _ConcessionPercentage = ((_ConcessionAmount / _Quantity) * 100) / _MRP;
                    //        }
                    //        else
                    //        {
                    //            _ConcessionPercentage = (((_ConcessionAmount / ((_MRP / (100 + (_VATPercent))) * 100)) * 100)) / _Quantity;
                    //        }
                    //    }
                    //}
                    //else
                    //    _ConcessionPercentage = 0;
                        //_ConcessionPercentage = ((_ConcessionAmount / _ReturnQuantity) * 100) / _MRP;
                   // else
                        //_ConcessionPercentage = 0;
                    //_ConcessionPercentage =Convert.ToDouble(  string.Format("{0:00.00}", _ConcessionPercentage));
                  //  _ConcessionPercentage = Math.Round(_ConcessionPercentage, 2);
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("NetAmount");

                }
            }
        }
        
        private double _NetAmount;
        public double NetAmount
        {
            get
            {
                if (_ItemVatType == 2)
                {
                        _NetAmount = _Amount - _ConcessionAmount + _VATAmount;

                        if (_NetAmount < 0)
                            return 0;
                        else
                            return _NetAmount;
                }
                else
                {
                    if (_ConcessionPercentage > 0)
                    {
                        //double BP = ((((MRP * _ReturnQuantity) / (1 + (_SGSTPercent / 100) + (_CGSTPercent / 100))) - _ConcessionAmount));      //Nettxn Val  //Math.Round(((MRP / (100 + (_VATPercent)) * 100) * _Quantity), 2);       // Package Change 19042017
                        //double DBP = _ConcessionAmount;
                        //double VA = (_SGSTAmount + _CGSTAmount); //_VATAmount;
                        //_NetAmount = BP + VA;//BP - DBP + VA;

                        double BP = ((MRP / (100 + (_SGSTPercent + _CGSTPercent)) * 100) * _ReturnQuantity);
                        double DBP = _ConcessionAmount;
                        double VA = _VATAmount;
                        double GST = _SGSTAmount + _CGSTAmount;
                        _NetAmount = BP - DBP + VA + GST;
                    }
                    else
                    {
                        //double BP = ((((MRP * _ReturnQuantity) / (1 + (_SGSTPercent / 100) + (_CGSTPercent / 100))) - _ConcessionAmount));  //Math.Round(((MRP / (100 + (_VATPercent)) * 100) * _Quantity), 2);      // Package Change 19042017
                        //double DBP = _ConcessionAmount;
                        //double VA = (_SGSTAmount + _CGSTAmount);//_VATAmount;
                        //_NetAmount = BP + VA;// BP - DBP + VA;

                        double BP = ((MRP / (100 + (_SGSTPercent + _CGSTPercent)) * 100) * _ReturnQuantity);
                        double DBP = _ConcessionAmount;
                        double VA = _VATAmount;
                        double GST = _SGSTAmount + _CGSTAmount;
                        _NetAmount = BP - DBP + VA + GST;
                     }

                    if (_NetAmount < 0)
                        return 0;
                    else
                        return _NetAmount;
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

        private double _TotalSalesAmount;
        public double TotalSalesAmount
        {
            get { return _TotalSalesAmount; }
            set
            {
                if (_TotalSalesAmount != value)
                {
                    _TotalSalesAmount = value;
                    OnPropertyChanged("TotalSalesAmount");
                }
            }
        }
        private double _ReturnQuantity; 
        public double ReturnQuantity 
        {
            get
            {
                return _ReturnQuantity;
            }
            set
            {
                if (_ReturnQuantity != value)
                {
                    //if (value <= 0)
                    //    _ReturnQuantity = 1;
                    //else
                    _ReturnQuantity = value;
                    OnPropertyChanged("ReturnQuantity");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("NetAmount");
                    //By Anjali........................
                    OnPropertyChanged("ConcessionPercentage");
                }
            }
        }

        public string Manufacture { get; set; }

        public string PregnancyClass { get; set; }

        public string DrugSchedule { get; set; }

        private bool _SelectStatus;
        public bool SelectStatus
        {
            get { return _SelectStatus; }
            set
            {
                if (_SelectStatus != value)
                {
                    _SelectStatus = value;
                    OnPropertyChanged("SelectStatus");
                }
            }
        }


        //By Anjali...........................

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
                    OnPropertyChanged("ReturnQuantity");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");



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
                    OnPropertyChanged("ReturnQuantity");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
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
        private long _SaleUOMID;
        public long SaleUOMID
        {
            get { return _SaleUOMID; }
            set
            {
                if (_SaleUOMID != value)
                {
                    _SaleUOMID = value;
                    OnPropertyChanged("SaleUOMID");
                }
            }
        }
        private string _SaleUOM;
        public string SaleUOM
        {
            get { return _SaleUOM; }
            set
            {
                if (_SaleUOM != value)
                {
                    _SaleUOM = value;
                    OnPropertyChanged("SaleUOM");
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
         private double _NetAmtCalculation;
        public double NetAmtCalculation
        {
            get { return _NetAmtCalculation ; }
            set
            {
                if (_NetAmtCalculation != value)
                {
                    _NetAmtCalculation = value;
                    OnPropertyChanged("NetAmtCalculation");
                    OnPropertyChanged("NetAmount");
                  
                }
            }
        }
       
        //......................................
        //Added By Bhushanp For GST 14072017
        private double _SGSTPercent;
        public double SGSTPercent
        {
            get { return _SGSTPercent; }
            set
            {
                if (_SGSTPercent != value)
                {
                    if (value < 0)
                        value = 0;
                    if (value > 100)
                        value = 100;
                    _SGSTPercent = value;
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("ActualNetAmt");
                }
            }
        }


        private double _CGSTPercent;
        public double CGSTPercent
        {
            get { return _CGSTPercent; }
            set
            {
                if (_CGSTPercent != value)
                {
                    if (value < 0)
                        value = 0;
                    if (value > 100)
                        value = 100;
                    _CGSTPercent = value;
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("ActualNetAmt");
                }
            }
        }

        private double _IGSTPercent;
        public double IGSTPercent
        {
            get { return _IGSTPercent; }
            set
            {
                if (_IGSTPercent != value)
                {
                    if (value < 0)
                        value = 0;
                    if (value > 100)
                        value = 100;
                    _IGSTPercent = value;
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("ActualNetAmt");
                }
            }
        }

        private double _SGSTAmount;
        public double SGSTAmount
        {
            get
            {
                if (_SGSTPercent > 0)
                {
                    if (_SGSTtaxtype == 2)
                    {
                        return _SGSTAmount = ((_Amount) * _SGSTPercent / 100);
                    }
                    else
                    {
                        if (_ConcessionPercentage > 0)
                        {
                           // return _SGSTAmount = (((((MRP * _ReturnQuantity) / (1 + (_SGSTPercent / 100) + (_CGSTPercent / 100))) - _ConcessionAmount)) * (_SGSTPercent / 100));
                            double AbatedAmt = _MRP * _ReturnQuantity / ((_SGSTPercent + _CGSTPercent + 100)) * 100;
                            return _SGSTAmount = (AbatedAmt - _ConcessionAmount) * (_SGSTPercent / 100);
                        }
                        else
                        {
                            //return _SGSTAmount = (((((MRP * _ReturnQuantity) / (1 + (_SGSTPercent / 100) + (_CGSTPercent / 100))) - _ConcessionAmount)) * (_SGSTPercent / 100));
                            double AbatedAmt = _MRP * _ReturnQuantity / ((_SGSTPercent + _CGSTPercent + 100)) * 100;
                            return _SGSTAmount = (AbatedAmt - _ConcessionAmount) * (_SGSTPercent / 100);
                        }
                    }
                }
                else
                {
                    return _SGSTAmount = 0;
                }
            }
            set
            {
                if (_SGSTAmount != value)
                {
                    if (value < 0)
                        value = 0;
                    _SGSTAmount = value;
                    if (_SGSTAmount > 0 && (_Amount - _ConcessionAmount) > 0)
                        if (_SGSTtaxtype == 2)
                        {
                            if (IsItemSaleReturn == false)
                            {
                                _SGSTPercent = (_SGSTAmount * 100) / (_Amount - _ConcessionAmount);
                            }
                        }
                        else
                        {
                        }
                    else
                        _SGSTPercent = 0;

                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _CGSTAmount;
        public double CGSTAmount
        {
            get
            {
                if (_CGSTPercent > 0)
                {
                    if (_CGSTtaxtype == 2)
                    {
                        return _CGSTAmount = ((_Amount) * _CGSTPercent / 100);
                    }
                    else
                    {
                        if (_ConcessionPercentage > 0)
                        {
                            //return _CGSTAmount = (((((MRP * _ReturnQuantity) / (1 + (_SGSTPercent / 100) + (_CGSTPercent / 100))) - _ConcessionAmount)) * (_CGSTPercent / 100));
                            double AbatedAmt = _MRP * _ReturnQuantity / (_SGSTPercent + _CGSTPercent + 100) * 100;
                            return _CGSTAmount = (AbatedAmt - _ConcessionAmount) * (_CGSTPercent / 100);
                        }
                        else
                        {
                            //return _CGSTAmount = (((((MRP * _ReturnQuantity) / (1 + (_SGSTPercent / 100) + (_CGSTPercent / 100))) - _ConcessionAmount)) * (_CGSTPercent / 100));
                            double AbatedAmt = _MRP * _ReturnQuantity / ((_SGSTPercent + _CGSTPercent + 100)) * 100;
                            return _CGSTAmount = (AbatedAmt - _ConcessionAmount) * (_CGSTPercent / 100);
                        }
                    }
                }
                else
                {
                    return _CGSTAmount = 0;
                }
            }
            set
            {
                if (_CGSTAmount != value)
                {
                    if (value < 0)
                        value = 0;
                    _CGSTAmount = value;
                    if (_CGSTAmount > 0 && (_Amount - _ConcessionAmount) > 0)
                        if (_CGSTtaxtype == 2)
                        {
                            if (IsItemSaleReturn == false)
                            {
                                _CGSTPercent = (_CGSTAmount * 100) / (_Amount - _ConcessionAmount);
                            }
                        }
                        else
                        {
                        }
                    else
                        _CGSTPercent = 0;

                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _IGSTAmount;
        public double IGSTAmount
        {
            get
            {
                if (_IGSTPercent > 0)
                {
                    if (IGSTtaxtype == 2)
                    {
                        return _IGSTAmount = ((_Amount) * _IGSTPercent / 100);
                    }
                    else
                    {
                        if (_ConcessionPercentage > 0)
                        {
                            return _IGSTAmount = ((MRP * _ReturnQuantity) - ((((MRP / (100 + (_IGSTPercent))) * 100)) * _Quantity));
                        }
                        else
                        {
                            return _IGSTAmount = ((MRP * _ReturnQuantity) - ((((MRP / (100 + (_IGSTPercent))) * 100)) * _Quantity));
                        }
                    }
                }
                else
                {
                    return _IGSTAmount = 0;
                }
            }
            set
            {
                if (_IGSTAmount != value)
                {
                    if (value < 0)
                        value = 0;
                    _IGSTAmount = value;
                    if (_IGSTAmount > 0 && (_Amount - _ConcessionAmount) > 0)
                        if (_IGSTtaxtype == 2)
                        {
                            if (IsItemSaleReturn == false)
                            {
                                _IGSTPercent = (_IGSTAmount * 100) / (_Amount - _ConcessionAmount);
                            }
                        }
                        else
                        {
                        }
                    else
                        _IGSTPercent = 0;

                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

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
      
    }

}


