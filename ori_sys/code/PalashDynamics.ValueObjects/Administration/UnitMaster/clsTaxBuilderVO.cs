using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.UnitMaster
{
    public class clsTaxBuilderVO : IValueObject, INotifyPropertyChanged
    {
        public string ToXml()
        {
            return this.ToXml();
        }

        #region INotifyPropertyChanged Members


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

        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    OnPropertyChanged("IsSelected");
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
                _ID = value;
                OnPropertyChanged("ID");
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
        //private long _UnitID;
        //public long UnitID
        //{
        //    get
        //    {
        //        return _UnitID;
        //    }
        //    set
        //    {
        //        _UnitID = value;
        //        OnPropertyChanged("UnitID");
        //    }
        //}

        //private string _UnitName;
        //public string UnitName
        //{
        //    get { return _UnitName; }
        //    set 
        //    {
        //        _UnitName = value;
        //        OnPropertyChanged("UnitName");
        //    }
        //}

        private string _TaxTypeName;
        public string TaxTypeName
        {
            get
            {
                return _TaxTypeName;
            }
            set
            {
                _TaxTypeName = value;
                OnPropertyChanged("TaxType");
            }

        }

        private string _Code;
        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                _Code = value;
                OnPropertyChanged("Code");
            }
        }

        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                OnPropertyChanged("Description");
            }
        }

        private long _TaxType;
        public long TaxType
        {
            get
            {
                return _TaxType;
            }
            set
            {
                _TaxType = value;
                OnPropertyChanged("TaxType");
            }
        }

        private long _AddServiceID;
        public long AddServiceID
        {
            get
            {
                return _AddServiceID;
            }
            set
            {
                _AddServiceID = value;
                OnPropertyChanged("AddServiceID");
            }
        }

        private double _TaxPercentage;
        public double TaxPercentage
        {
            get
            {
                return _TaxPercentage;
            }
            set
            {
                _TaxPercentage = value;
                OnPropertyChanged("TaxPercentage");
            }
        }

        private long _TaxApplicableOn;
        public long TaxApplicableOn
        {
            get
            {
                return _TaxApplicableOn;
            }
            set
            {
                _TaxApplicableOn = value;
                OnPropertyChanged("TaxApplicableOn");
            }
        }

        private long _TaxOnTaxID;
        public long TaxOnTaxID
        {
            get
            {
                return _TaxOnTaxID;
            }
            set
            {
                _TaxOnTaxID = value;
                OnPropertyChanged("TaxOnTaxID");
            }
        }
        private bool _IsStatutory;
        public bool IsStatutory
        {
            get
            {
                return _IsStatutory;
            }
            set
            {
                _IsStatutory = value;
                OnPropertyChanged("IsStatutory");
            }
        }

        private bool _IsAddAsService;
        public bool IsAddAsService
        {
            get
            {
                return _IsAddAsService;
            }
            set
            {
                _IsAddAsService = value;
                OnPropertyChanged("IsAddAsService");
            }
        }

        private bool _IsItemServiceWiseApp;
        public bool IsItemServiceWiseApp
        {
            get
            {
                return _IsItemServiceWiseApp;
            }
            set
            {
                _IsItemServiceWiseApp = value;
                OnPropertyChanged("IsItemServiceWiseApp");
            }
        }

        private bool _Status;
        public bool Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                OnPropertyChanged("Status");
            }
        }

        private long? _CreatedUnitID;
        public long? CreatedUnitID
        {
            get
            {
                return _CreatedUnitID;
            }
            set
            {
                _CreatedUnitID = value;
                OnPropertyChanged("CreatedUnitID");
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get
            {
                return _UpdatedUnitID;
            }
            set
            {
                _UpdatedUnitID = value;
                OnPropertyChanged("UpdatedUnitID");
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get
            {
                return _AddedBy;
            }
            set
            {
                _AddedBy = value;
                OnPropertyChanged("AddedBy");
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
                _AddedOn = value;
                OnPropertyChanged("AddedOn");
            }
        }

        private DateTime? _AddedDateTime;
        public DateTime? AddedDateTime
        {
            get
            {
                return _AddedDateTime;
            }
            set
            {
                _AddedDateTime = value;
                OnPropertyChanged("AddedDateTime");
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get
            {
                return _UpdatedBy;
            }
            set
            {
                _UpdatedBy = value;
                OnPropertyChanged("UpdatedBy");
            }
        }


        private string _UpdatedOn;
        public string UpdatedOn
        {
            get
            {
                return _UpdatedOn;
            }
            set
            {

                _UpdatedOn = value;
                OnPropertyChanged("UpdatedOn");
            }
        }

        private DateTime? _UpdatedDateTime;
        public DateTime? UpdatedDateTime
        {
            get
            {
                return _UpdatedDateTime;
            }
            set
            {
                _UpdatedDateTime = value;
                OnPropertyChanged("UpdatedDateTime");
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
                _AddedWindowsLoginName = value;
                OnPropertyChanged("AddedWindowsLoginName");
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
                _UpdateWindowsLoginName = value;
                OnPropertyChanged("UpdateWindowsLoginName");
            }
        }

        public Boolean PrimaryKeyViolationError { get; set; }
        public Boolean GeneralError { get; set; }
    }
}
