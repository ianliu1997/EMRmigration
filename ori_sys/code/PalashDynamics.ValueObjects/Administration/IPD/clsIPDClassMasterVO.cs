using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.IPD
{
    public class clsIPDClassMasterVO : IValueObject, INotifyPropertyChanged
    {

        public string ToXml()
        {
            return this.ToXml();
        }

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

        private long _UnitID;
        public long UnitID
        {
            get
            {
                return _UnitID;
            }
            set
            {
                _UnitID = value;
                OnPropertyChanged("UnitID");
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

        private List<MasterListItem> _AmmenityDetails;
        public List<MasterListItem> AmmenityDetails
        {
            get
            {
                if (_AmmenityDetails == null)
                    _AmmenityDetails = new List<MasterListItem>();
                return _AmmenityDetails;
            }
            set
            { _AmmenityDetails = value; }
        }


        //private long _OrderNo;
        //public long OrderNo
        //{
        //    get
        //    {
        //        return _OrderNo;
        //    }
        //    set
        //    {
        //        _OrderNo = value;
        //        OnPropertyChanged("OrderNo");
        //    }
        //}

        private decimal _DepositIPD;
        public decimal DepositIPD
        {
            get
            {
                return _DepositIPD;
            }
            set
            {
                _DepositIPD = value;
                OnPropertyChanged("DepositIPD");
            }
        }

        private decimal _DepositOT;
        public decimal DepositOT
        {
            get
            {
                return _DepositOT;
            }
            set
            {
                _DepositOT = value;
                OnPropertyChanged("DepositOT");
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


    }

    public class clsClassDataGrid : IValueObject, INotifyPropertyChanged
    {
        public string ToXml()
        {
            return this.ToXml();
        }

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

        public long TariffID { get; set; }
        public long ServiceID { get; set; }

        public clsIPDClassMasterVO clsIPDClassMasterVO { get; set; }

        public List<clsIPDClassMasterVO> clsIPDClassMasterList { get; set; }

        private Decimal _DbGross;
        public Decimal DbGross
        {
            get
            {
                return _DbGross;
            }
            set
            {
                _DbGross = value;
                OnPropertyChanged("DbGross");
            }
        }

        private Decimal _Gross;
        public Decimal Gross
        {
            get
            {
                return _Gross;
            }
            set
            {
                _Gross = value;
                OnPropertyChanged("Gross");
            }
        }

        private Decimal _Rate;
        public Decimal Rate
        {
            get
            {
                return _Rate;
            }
            set
            {
                _Rate = value;
                if (!_Rate.Equals(0))
                {
                    ADJEnabled = true;
                    if (!_ADJPerc.Equals(0))
                    {
                        ADJPerc = _ADJPerc;
                    }
                    else if (!_ADJAmt.Equals(0))
                    {
                        ADJAmt = _ADJAmt;
                    }
                    else
                    {
                        _Gross = _Rate;
                        if (!_DeductiblePerc.Equals(0))
                        {
                            if (_DeductiblePerc <= 100)
                            {
                                _DeductibleAmt = (_Gross * _DeductiblePerc) / 100;
                            }
                        }
                        OnPropertyChanged("DeductibleAmt");
                        OnPropertyChanged("DeductiblePerc");
                    }

                    OnPropertyChanged("ADJAmt");
                    //OnPropertyChanged("DeductiblePerc");
                    //OnPropertyChanged("DeductibleAmt");
                    OnPropertyChanged("ADJEnabled");
                }
                else if (_Rate.Equals(0))
                {
                    _Gross = _Rate;
                    if (_Gross.Equals(0))
                    {
                        if (!_DeductiblePerc.Equals(0))
                        {
                            if (_DeductiblePerc <= 100)
                            {
                                _DeductibleAmt = (_Gross * _DeductiblePerc) / 100;
                            }
                        }
                        OnPropertyChanged("DeductibleAmt");
                        OnPropertyChanged("DeductiblePerc");
                    }
                }

                OnPropertyChanged("Rate");
                OnPropertyChanged("Gross");
                //OnPropertyChanged("AdjType");
            }
        }

        private long _AdjType;
        public long AdjType
        {
            get
            {
                return _AdjType;
            }
            set
            {
                _AdjType = value;
                if (_AdjType.Equals(1))
                {
                    _ADJEnabled = true;
                    if (!_ADJPerc.Equals(0))
                    {
                        if (_ADJPerc <= 100)
                        {
                            _ADJAmt = (_Rate * _ADJPerc) / 100;
                            _Gross = _Rate - _ADJAmt;
                            if (!_DeductiblePerc.Equals(0))
                            {
                                _DeductibleAmt = (_Gross * _DeductiblePerc) / 100;
                            }
                        }
                        else
                        {
                            _ADJPerc = 0;
                            _ADJAmt = 0;
                            _Gross = _Rate;
                            if (!_DeductiblePerc.Equals(0))
                            {
                                _DeductibleAmt = (_Gross * _DeductiblePerc) / 100;
                            }
                        }
                    }
                    else if (!_ADJAmt.Equals(0))
                    {
                        if (_ADJAmt <= _Rate)
                        {
                            _ADJPerc = (_ADJAmt * 100) / _Rate;
                            _Gross = _Rate - _ADJAmt;
                            if (!_DeductiblePerc.Equals(0))
                            {
                                _DeductibleAmt = (_Gross * _DeductiblePerc) / 100;
                            }
                        }
                        else
                        {
                            _ADJPerc = 0;
                            _ADJAmt = 0;
                            if (!_Rate.Equals(0))
                            {
                                _Gross = _Rate;
                                if (!_DeductiblePerc.Equals(0))
                                {
                                    _DeductibleAmt = (_Gross * _DeductiblePerc) / 100;
                                }
                            }
                        }
                    }
                    else if (!Rate.Equals(0))
                    {
                        if (!ADJPerc.Equals(0))
                        {
                            _ADJAmt = (_Rate * ADJPerc) / 100;
                            Gross = _Rate + _ADJAmt;
                        }
                    }
                    else
                    {
                        _Gross = _Rate;
                    }
                    if (!_DeductiblePerc.Equals(0))
                    {
                        _DeductibleAmt = (_Gross * _DeductiblePerc) / 100;
                    }
                }
                else if (_AdjType.Equals(2))
                {
                    _ADJEnabled = true;
                    if (!_ADJPerc.Equals(0))
                    {
                        _ADJAmt = (_Rate * _ADJPerc) / 100;
                        Gross = _Rate + _ADJAmt;
                    }
                    else if (!_ADJAmt.Equals(0))
                    {
                        _ADJPerc = (_ADJAmt * 100) / _Rate;
                        Gross = _Rate + _ADJAmt;
                        if (!_DeductiblePerc.Equals(0))
                        {
                            _DeductibleAmt = (_Gross * _DeductiblePerc) / 100;
                        }
                    }
                    else if (!Rate.Equals(0))
                    {
                        if (!ADJPerc.Equals(0))
                        {
                            _ADJAmt = (_Rate * ADJPerc) / 100;
                            Gross = _Rate + _ADJAmt;
                        }
                    }
                    else
                    {
                        _Gross = _Rate;
                    }
                    if (!_DeductiblePerc.Equals(0))
                    {
                        _DeductibleAmt = (_Gross * _DeductiblePerc) / 100;
                    }
                }
                else if (_AdjType.Equals(0))
                {
                    if (!ADJPerc.Equals(0))
                    {
                        _ADJAmt = (_Rate * ADJPerc) / 100;
                    }
                    else if (!_ADJAmt.Equals(0))
                    {
                        _ADJPerc = (_ADJAmt * 100) / _Rate;
                    }
                    _Gross = _Rate;
                    if (!_DeductiblePerc.Equals(0))
                    {
                        _DeductibleAmt = (_Gross * _DeductiblePerc) / 100;
                    }
                    else if (!_DeductiblePerc.Equals(0))
                    {
                        _DeductiblePerc = (_DeductibleAmt * 100) / _Gross;
                    }
                }

                OnPropertyChanged("AdjType");
                OnPropertyChanged("Rate");
                OnPropertyChanged("Gross");
                OnPropertyChanged("ADJPerc");
                OnPropertyChanged("ADJAmt");
                OnPropertyChanged("DeductibleAmt");
                OnPropertyChanged("ADJEnabled");
            }
        }

        private bool _ADJPerc_Amt;
        public bool ADJPerc_Amt
        {
            get
            {
                return _ADJPerc_Amt;
            }
            set
            {
                _ADJPerc_Amt = value;
                if (_ADJPerc_Amt.Equals(false))
                {
                    if (!_Rate.Equals(0))
                        _Gross = _Rate;
                    _ADJPerc = 0;
                    _ADJAmt = 0;
                }
                if (_AdjType.Equals(1))
                {
                    AdjType = 1;
                }
                else if (_AdjType.Equals(2))
                {
                    AdjType = 2;
                }
                //else if(_AdjType.Equals(0))
                //{
                //    AdjType = 0;
                //}
                OnPropertyChanged("AdjType");
                OnPropertyChanged("ADJPerc_Amt");
                OnPropertyChanged("ADJPerc");
                OnPropertyChanged("ADJAmt");
                OnPropertyChanged("Gross");
            }
        }

        private Decimal _ADJPerc;
        public Decimal ADJPerc
        {
            get
            {
                return _ADJPerc;
            }
            set
            {
                _ADJPerc = value;

                if (!_Rate.Equals(0) && !_ADJPerc.Equals(0))
                {
                    if (_AdjType.Equals(1))
                    {
                        if (_ADJPerc <= 100)
                        {
                            _ADJAmt = (_Rate * _ADJPerc) / 100;
                            _Gross = _Rate - _ADJAmt;
                            if (!_DeductiblePerc.Equals(0))
                            {
                                _DeductibleAmt = (_Gross * _DeductiblePerc) / 100;
                            }
                        }
                        else
                        {
                            _ADJPerc = 0;
                            _ADJAmt = 0;
                            _Gross = _Rate;
                            if (!_DeductiblePerc.Equals(0))
                            {
                                _DeductibleAmt = (_Gross * _DeductiblePerc) / 100;
                            }
                        }
                    }
                    else if (_AdjType.Equals(2))
                    {
                        //if(_ADJPerc <= 100)
                        //{
                        _ADJAmt = (_Rate * _ADJPerc) / 100;
                        _Gross = _Rate + _ADJAmt;
                        if (!_DeductiblePerc.Equals(0))
                        {
                            _DeductibleAmt = (_Gross * _DeductiblePerc) / 100;
                        }
                        //}
                        //else
                        //{
                        //    _ADJPerc = 0;
                        //    _ADJAmt = 0;
                        //    _Gross = _Rate;
                        //    if(!_DeductiblePerc.Equals(0))
                        //    {
                        //        _DeductibleAmt = (_Gross * _DeductiblePerc) / 100;
                        //    }
                        //}
                    }
                    else if (_AdjType.Equals(0))
                    {
                        _ADJAmt = (_Rate * _ADJPerc) / 100;
                    }

                }
                else if (_Rate.Equals(0) && _ADJPerc.Equals(0))
                {
                    _ADJAmt = 0;
                    _ADJPerc = 0;
                    _Gross = 0;
                }
                else if (_ADJPerc.Equals(0))
                {
                    _ADJAmt = 0;
                    _Gross = _Rate;
                }

                OnPropertyChanged("ADJPerc");
                OnPropertyChanged("ADJAmt");
                OnPropertyChanged("DeductiblePerc");
                OnPropertyChanged("DeductibleAmt");
                OnPropertyChanged("Gross");
            }
        }

        private Decimal _ADJAmt;
        public Decimal ADJAmt
        {
            get
            {
                return _ADJAmt;
            }
            set
            {
                _ADJAmt = value;
                if (!_Rate.Equals(0) && !_ADJAmt.Equals(0))
                {
                    if (_AdjType.Equals(1))
                    {
                        if (_ADJAmt <= _Rate)
                        {
                            _ADJPerc = (_ADJAmt * 100) / _Rate;
                            _Gross = _Rate - _ADJAmt;
                            if (!_DeductiblePerc.Equals(0))
                            {
                                _DeductibleAmt = (_Gross * _DeductiblePerc) / 100;
                            }
                        }
                        else
                        {
                            _ADJPerc = 0;
                            _ADJAmt = 0;
                            if (!_Rate.Equals(0))
                            {
                                _Gross = _Rate;
                                if (!_DeductiblePerc.Equals(0))
                                {
                                    _DeductibleAmt = (_Gross * _DeductiblePerc) / 100;
                                }
                            }
                        }
                    }
                    else if (_AdjType.Equals(2))
                    {
                        _ADJPerc = (_ADJAmt * 100) / _Rate;
                        _Gross = _Rate + _ADJAmt;

                    }
                    else if (_AdjType.Equals(0))
                    {
                        _ADJPerc = (_ADJAmt * 100) / _Rate;
                    }
                }
                else if (_Rate.Equals(0) && _ADJAmt.Equals(0))
                {
                    _ADJAmt = 0;
                    _ADJPerc = 0;
                    _Gross = 0;
                }
                else if (_ADJAmt.Equals(0))
                {
                    _ADJPerc = 0;
                    _Gross = _Rate;
                }
                OnPropertyChanged("AdjType");
                OnPropertyChanged("ADJAmt");
                OnPropertyChanged("ADJPerc");
                OnPropertyChanged("DeductiblePerc");
                OnPropertyChanged("DeductibleAmt");
                OnPropertyChanged("Gross");
            }
        }

        private bool _ADJEnabled;
        public bool ADJEnabled
        {
            get
            {
                return _ADJEnabled;
            }
            set
            {
                _ADJEnabled = value;
                OnPropertyChanged("ADJEnabled");
            }
        }

        private bool _DeductiblePerc_Amt;
        public bool DeductiblePerc_Amt
        {
            get
            {
                return _DeductiblePerc_Amt;
            }
            set
            {
                _DeductiblePerc_Amt = value;
                if (_DeductiblePerc_Amt.Equals(false))
                {
                    _DeductibleAmt = 0;
                    _DeductiblePerc = 0;
                }
                OnPropertyChanged("DeductibleAmt");
                OnPropertyChanged("DeductiblePerc");
                OnPropertyChanged("DeductiblePerc_Amt");
            }
        }

        private Decimal _DeductiblePerc;
        public Decimal DeductiblePerc
        {
            get
            {
                return _DeductiblePerc;
            }
            set
            {
                _DeductiblePerc = value;
                if (!_Gross.Equals(0) && !_DeductiblePerc.Equals(0))
                {
                    if (_DeductiblePerc <= 100)
                    {
                        _DeductibleAmt = (_Gross * _DeductiblePerc) / 100;
                    }
                    else
                    {
                        _DeductiblePerc = 0;
                        _DeductibleAmt = 0;
                    }
                }
                else if (_Gross.Equals(0) && _DeductiblePerc.Equals(0))
                {
                    _DeductiblePerc = 0;
                    _DeductibleAmt = 0;
                }
                else if (_DeductiblePerc.Equals(0))
                {
                    _DeductibleAmt = 0;
                }
                OnPropertyChanged("DeductibleAmt");
                OnPropertyChanged("DeductiblePerc");
            }
        }

        private Decimal _DeductibleAmt;
        public Decimal DeductibleAmt
        {
            get
            {
                return _DeductibleAmt;
            }
            set
            {
                _DeductibleAmt = value;
                if (!_Gross.Equals(0) && !_DeductibleAmt.Equals(0))
                {
                    if (_DeductibleAmt <= _Gross)
                    {
                        _DeductiblePerc = (_DeductibleAmt * 100) / _Gross;
                    }
                    else
                    {
                        _DeductibleAmt = 0;
                        _DeductiblePerc = 0;
                    }
                }
                else if (_Gross.Equals(0) && _DeductibleAmt.Equals(0))
                {
                    _DeductiblePerc = 0;
                    _DeductibleAmt = 0;
                }
                else if (_DeductibleAmt.Equals(0))
                {
                    _DeductiblePerc = 0;
                }
                OnPropertyChanged("DeductibleAmt");
                OnPropertyChanged("DeductiblePerc");
            }
        }

        private bool _DeductEnabled;
        public bool DeductEnabled
        {
            get
            {
                return _DeductEnabled;
            }
            set
            {
                _DeductEnabled = value;
                OnPropertyChanged("DeductEnabled");
            }
        }

        private bool _chkADJ;
        public bool chkADJ
        {
            get
            {
                return _chkADJ;
            }
            set
            {
                _chkADJ = value;
                OnPropertyChanged("chkADJ");
            }
        }

        private long _RowID;
        public long RowID
        {
            get
            {
                return _RowID;
            }
            set
            {
                _RowID = value;
                //OnPropertyChanged("RowID");
            }
        }

    }
}
