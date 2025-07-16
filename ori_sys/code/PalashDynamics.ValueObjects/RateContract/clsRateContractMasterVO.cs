using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.RateContract
{
    public class clsRateContractMasterVO : IValueObject, INotifyPropertyChanged
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

        #region Property Declaration
        private long _RateContractID;
        public long RateContractID
        {
            get { return _RateContractID; }
            set { if (_RateContractID != value) { _RateContractID = value; OnPropertyChanged("RateContractID"); } }
        }
        private long _RateContractUnitID;
        public long RateContractUnitID
        {
            get { return _RateContractUnitID; }
            set { if (_RateContractUnitID != value) { _RateContractUnitID = value; OnPropertyChanged("RateContractUnitID"); } }
        }
        private string _Code;
        public string Code
        {
            get { return _Code; }
            set { if (_Code != value) { _Code = value; OnPropertyChanged("Code"); } }
        }
        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { if (_Description != value) { _Description = value; OnPropertyChanged("Description"); } }
        }
        private DateTime _ContractDate;
        public DateTime ContractDate
        {
            get { return _ContractDate; }
            set { if (_ContractDate != value) { _ContractDate = value; OnPropertyChanged("ContractDate"); } }
        }
        private Double _ContractValue;
        public Double ContractValue
        {
            get { return _ContractValue; }
            set { if (_ContractValue != value) { _ContractValue = value; OnPropertyChanged("ContractValue"); } }
        }
        private long _SupplierId;
        public long SupplierId
        {
            get { return _SupplierId; }
            set { if (_SupplierId != value) { _SupplierId = value; OnPropertyChanged("SupplierId"); } }
        }
        private DateTime _FromDate;
        public DateTime FromDate
        {
            get { return _FromDate; }
            set { if (_FromDate != value) { _FromDate = value; OnPropertyChanged("FromDate"); } }
        }
        private DateTime _ToDate;
        public DateTime ToDate
        {
            get { return _ToDate; }
            set { if (_ToDate != value) { _ToDate = value; OnPropertyChanged("ToDate"); } }
        }
        private Boolean _IsFreeze;
        public Boolean IsFreeze
        {
            get { return _IsFreeze; }
            set { if (_IsFreeze != value) { _IsFreeze = value; OnPropertyChanged("ISFreeze"); } }
        }
        #endregion Property Declaration
    }
    public class clsRateContractItemDetailsVO : IValueObject, INotifyPropertyChanged
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

        #region Property Declaration
        private long _RateContractID;
        public long RateContractID
        {
            get { return _RateContractID; }
            set { if (_RateContractID != value) { _RateContractID = value; OnPropertyChanged("RateContractID"); } }
        }
        private long _RateContractUnitID;
        public long RateContractUnitID
        {
            get { return _RateContractUnitID; }
            set { if (_RateContractUnitID != value) { _RateContractUnitID = value; OnPropertyChanged("RateContractUnitID"); } }
        }
        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set { if (_Remarks != value) { _Remarks = value; OnPropertyChanged("Remarks"); } }
        }
        private long _ItemID;
        public long ItemID
        {
            get { return _ItemID; }
            set { if (_ItemID != value) { _ItemID = value; OnPropertyChanged("ItemID"); } }
        }
        private Boolean _UnlimitedQuantity;
        public Boolean UnlimitedQuantity
        {
            get { return _UnlimitedQuantity; }
            set { if (_UnlimitedQuantity != value) { _UnlimitedQuantity = value; OnPropertyChanged("UnlimitedQuantity"); } }
        }
        private String _Condition;
        public string Condition
        {
            get { return _Condition; }
            set { if (_Condition != value) { _Condition = value; OnPropertyChanged("Condition"); } }
        }
        private double _Quantity;
        public double Quantity
        {
            get { return _Quantity; }
            set { if (_Quantity != value) { _Quantity = value; OnPropertyChanged("Quantity"); } }
        }
        private double _MinQuantity;
        public double MinQuantity
        {
            get { return _MinQuantity; }
            set { if (_MinQuantity != value) { _MinQuantity = value; OnPropertyChanged("MinQuantity"); } }
        }
        private double _MaxQuantity;
        public double MaxQuantity
        {
            get { return _MaxQuantity; }
            set { if (_MaxQuantity != value) { _MaxQuantity = value; OnPropertyChanged("MaxQuantity"); } }
        }
        private Boolean _IsAllowMultiple;
        public Boolean IsAllowMultiple
        {
            get { return _IsAllowMultiple; }
            set { if (_IsAllowMultiple != value) { _IsAllowMultiple = value; OnPropertyChanged("IsAllowMultiple"); } }
        }
        private double _Rate;
        public double Rate
        {
            get { return _Rate; }
            set { if (_Rate != value) { _Rate = value; OnPropertyChanged("Rate"); } }
        }
        private double _Amount;
        public double Amount
        {
            get { return _Amount; }
            set { if (_Amount != value) { _Amount = value; OnPropertyChanged("Amount"); } }
        }
        private double _DiscountPercent;
        public double DiscountPercent
        {
            get { return _DiscountPercent; }
            set { if (_DiscountPercent != value) { _DiscountPercent = value; OnPropertyChanged("DiscountPercent"); } }
        }
        private double _DiscountAmount;
        public double DiscountAmount
        {
            get { return _DiscountAmount; }
            set { if (_DiscountAmount != value) { _DiscountAmount = value; OnPropertyChanged("DiscountAmount"); } }
        }
        private double _NetAmount;
        public double NetAmount
        {
            get { return _NetAmount; }
            set { if (_NetAmount != value) { _NetAmount = value; OnPropertyChanged("NetAmount"); } }
        }
        private double _PendingQuantity;
        public double PendingQuantity
        {
            get { return _PendingQuantity; }
            set { if (_PendingQuantity != value) { _PendingQuantity = value; OnPropertyChanged("PendingQuantity"); } }
        }
        private double _MRP;
        public double MRP
        {
            get { return _MRP; }
            set { if (_MRP != value) { _MRP = value; OnPropertyChanged("MRP"); } }
        }
                #endregion Property Declaration
    }
}
