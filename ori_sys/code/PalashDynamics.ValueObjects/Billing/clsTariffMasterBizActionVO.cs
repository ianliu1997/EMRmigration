using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.ValueObjects.Billing
{
    public class clsTariffMasterBizActionVO : IValueObject, INotifyPropertyChanged
    {

        private List<clsServiceMasterVO> _objServiceMasterDetails = null;
        public List<clsServiceMasterVO> ServiceMasterList
        {
            get { return _objServiceMasterDetails; }
            set { _objServiceMasterDetails = value; }

        }

        private List<clsTariffMasterBizActionVO> _objServiceSpecializationMasterDetails = null;
        public List<clsTariffMasterBizActionVO> ServiceSpecializationMasterList
        {
            get { return _objServiceSpecializationMasterDetails; }
            set { _objServiceSpecializationMasterDetails = value; }

        }

        private bool _IsApplicable;
        public bool IsApplicable
        {
            get { return _IsApplicable; }
            set
            {
                if (value != _IsApplicable)
                {
                    _IsApplicable = value;
                    OnPropertyChanged("IsApplicable");
                }
            }
        }

        public DateTime? FromEffectiveDate;
        public DateTime? ToEffectiveDate;

        private bool _IsSetRateForAll;
        public bool IsSetRateForAll
        {
            get { return _IsSetRateForAll; }
            set
            {
                if (value != _IsSetRateForAll)
                {
                    _IsSetRateForAll = value;
                    OnPropertyChanged("IsSetRateForAll");
                }
            }
        }

        private bool _IsFreeze;
        public bool IsFreeze
        {
            get { return _IsFreeze; }
            set
            {
                if (value != _IsFreeze)
                {
                    _IsFreeze = value;
                    OnPropertyChanged("IsFreeze");
                }
            }
        }


        private bool _SelectTariff;
        public bool SelectTariff
        {
            get { return _SelectTariff; }
            set
            {
                if (value != _SelectTariff)
                {
                    _SelectTariff = value;
                    OnPropertyChanged("SelectTariff");
                }
            }
        }

        private long _RowID;
        public long RowID
        {
            get { return _RowID; }
            set
            {
                if (value != _RowID)
                {
                    _RowID = value;
                    OnPropertyChanged("RowID");
                }
            }
        }

        private long _GroupID;
        public long GroupID
        {
            get { return _GroupID; }
            set
            {
                if (_GroupID != value)
                {
                    _GroupID = value;
                    OnPropertyChanged("GroupID");
                }
            }
        }

        private string _StrGroup;
        public string StrGroup
        {
            get { return _StrGroup; }
            set
            {
                if (_StrGroup != value)
                {
                    _StrGroup = value;
                    OnPropertyChanged("StrGroup");
                }
            }
        }

        private string _Specialization;
        public string Specialization
        {
            get { return _Specialization; }
            set
            {
                if (_Specialization != value)
                {
                    _Specialization = value;
                    OnPropertyChanged("Specialization");
                }
            }
        }

        private string _StrSubGroup;
        public string StrSubGroup
        {
            get { return _StrSubGroup; }
            set
            {
                if (_StrSubGroup != value)
                {
                    _StrSubGroup = value;
                    OnPropertyChanged("StrSubGroup");
                }
            }
        }

        private long _SubGroupID;
        public long SubGroupID
        {
            get { return _SubGroupID; }
            set
            {
                if (_SubGroupID != value)
                {
                    _SubGroupID = value;
                    OnPropertyChanged("SubGroupID");
                }
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

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set
            {
                if (_Code != value)
                {
                    _Code = value;
                    OnPropertyChanged("Code");
                }
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private int _NoOfVisit;
        public int NoOfVisit
        {
            get { return _NoOfVisit; }
            set
            {
                if (_NoOfVisit != value)
                {
                    _NoOfVisit = value;
                    OnPropertyChanged("NoOfVisit");
                }
            }
        }

        private bool _AllVisit;
        public bool AllVisit
        {
            get { return _AllVisit; }
            set
            {
                if (_AllVisit != value)
                {
                    _AllVisit = value;
                    OnPropertyChanged("AllVisit");
                }
            }
        }

        private bool _Specify;
        public bool Specify
        {
            get { return _Specify; }
            set
            {
                if (_Specify != value)
                {
                    _Specify = value;
                    OnPropertyChanged("Specify");
                }
            }
        }

        private bool _CheckDate;
        public bool CheckDate
        {
            get { return _CheckDate; }
            set
            {
                if (_CheckDate != value)
                {
                    _CheckDate = value;
                    OnPropertyChanged("CheckDate");
                }
            }
        }

        private DateTime? _EffectiveDate;
        public DateTime? EffectiveDate
        {
            get { return _EffectiveDate; }
            set
            {
                if (_EffectiveDate != value)
                {
                    _EffectiveDate = value;
                    OnPropertyChanged("EffectiveDate");
                }
            }
        }

        private DateTime? _ExpiryDate;
        public DateTime? ExpiryDate
        {
            get { return _ExpiryDate; }
            set
            {
                if (_ExpiryDate != value)
                {
                    _ExpiryDate = value;
                    OnPropertyChanged("ExpiryDate");
                }
            }
        }

        private long _BulkRateChangeID;
        public long BulkRateChangeID
        {
            get { return _BulkRateChangeID; }
            set
            {
                if (value != _BulkRateChangeID)
                {
                    _BulkRateChangeID = value;
                    OnPropertyChanged("BulkRateChangeID");
                }
            }
        }

        private long _BulkRateChangeUnitID;
        public long BulkRateChangeUnitID
        {
            get { return _BulkRateChangeUnitID; }
            set
            {
                if (value != _BulkRateChangeUnitID)
                {
                    _BulkRateChangeUnitID = value;
                    OnPropertyChanged("BulkRateChangeUnitID");
                }
            }
        }

        private long _TariffID;
        public long TariffID
        {
            get { return _TariffID; }
            set
            {
                if (value != _TariffID)
                {
                    _TariffID = value;
                    OnPropertyChanged("TariffID");
                }
            }
        }

        private string _TariffName;
        public string TariffName
        {
            get { return _TariffName; }
            set
            {
                if (value != _TariffName)
                {
                    _TariffName = value;
                    OnPropertyChanged("TariffName");
                }
            }
        }

        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set
            {
                if (value != _Remark)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }

        



        #region Common Properties


        private bool _Status;
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


        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }


        private long _UpdatedUnitID;
        public long UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
                }
            }
        }


        private long _AddedBy;
        public long AddedBy
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

        private string _strAddedBy;
        public string strAddedBy
        {
            get { return _strAddedBy; }
            set
            {
                if (_strAddedBy != value)
                {
                    _strAddedBy = value;
                    OnPropertyChanged("strAddedBy");
                }
            }
        }


        private string _AddedOn;
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

        private DateTime? _AddedDateTime;
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

        private string _AddedWindowsLoginName;
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


        private string _UpdatedOn;
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

        private DateTime? _UpdatedDateTime;
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

        private string _UpdatedWindowsLoginName;
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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion
    }
}
