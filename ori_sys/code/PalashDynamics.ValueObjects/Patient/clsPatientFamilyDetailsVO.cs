using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Patient
{
    public class clsPatientFamilyDetailsVO : IValueObject, INotifyPropertyChanged
    {
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }

            }
        }

        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (value != _PatientID)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }

            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (value != _PatientUnitID)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }

            }
        }


        private string _FirstName = "";
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        private string _MiddleName = "";
        public string MiddleName
        {
            get
            {
                return _MiddleName;
            }
            set
            {
                if (_MiddleName != value)
                {
                    _MiddleName = value;
                    OnPropertyChanged("MiddleName");
                }
            }
        }

        private string _LastName = "";
        public string LastName
        {
            get { return _LastName; }
            set
            {
                if (_LastName != value)
                {
                    _LastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        


        private string strPatientName = "";
        public string PatientName
        {
            get { return strPatientName = _FirstName + " " + _MiddleName + " " + _LastName; }
            set
            {
                if (value != strPatientName)
                {
                    strPatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }

        private DateTime? _DateOfBirth = null;
        public DateTime? DateOfBirth
        {
            get
            {
                return _DateOfBirth;
            }
            set
            {
                if (value != _DateOfBirth)
                {

                    _DateOfBirth = value;
                    OnPropertyChanged("DateOfBirth");
                }
            }
        }


        private long _RelationID;
        public long RelationID
        {
            get { return _RelationID; }
            set
            {
                if (value != _RelationID)
                {
                    _RelationID = value;
                    OnPropertyChanged("RelationID");
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


        private string _Relation;
        public string Relation
        {
            get { return _Relation; }
            set
            {
                if (value != _Relation)
                {
                    _Relation = value;
                    OnPropertyChanged("Relation");
                }

            }
        }

        private string _Tariff;
        public string Tariff
        {
            get { return _Tariff; }
            set
            {
                if (value != _Tariff)
                {
                    _Tariff = value;
                    OnPropertyChanged("Tariff");
                }

            }
        }

        private bool _MemberRegistered;
        public bool MemberRegistered
        {
            get { return _MemberRegistered; }
            set
            {
                if (_MemberRegistered != value)
                {
                    _MemberRegistered = value;
                    OnPropertyChanged("MemberRegistered");
                }
            }
        }

        #region Common Properties



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

        private string _Unit;
        public string Unit
        {
            get { return _Unit; }
            set
            {
                if (_Unit != value)
                {
                    _Unit = value;
                    OnPropertyChanged("Unit");
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
    }

    public class clsPatientServiceDetails : IValueObject, INotifyPropertyChanged
    {
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }

            }
        }

        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (value != _PatientID)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }

            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (value != _PatientUnitID)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }

            }
        }


        private long _RelationID;
        public long RelationID
        {
            get { return _RelationID; }
            set
            {
                if (value != _RelationID)
                {
                    _RelationID = value;
                    OnPropertyChanged("RelationID");
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


        private long _LoyaltyID;
        public long LoyaltyID
        {
            get { return _LoyaltyID; }
            set
            {
                if (value != _LoyaltyID)
                {
                    _LoyaltyID = value;
                    OnPropertyChanged("LoyaltyID");
                }

            }
        }

        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set
            {
                if (value != _ServiceID)
                {
                    _ServiceID = value;
                    OnPropertyChanged("ServiceID");
                }

            }
        }
       
        private string _ServiceName = "";
        public string ServiceName
        {
            get { return _ServiceName; }
            set
            {
                if (value != _ServiceName)
                {
                    _ServiceName = value;
                    OnPropertyChanged("ServiceName");
                }
            }
        }

        private string _ServiceCode = "";
        public string ServiceCode
        {
            get { return _ServiceCode; }
            set
            {
                if (value != _ServiceCode)
                {
                    _ServiceCode = value;
                    OnPropertyChanged("ServiceCode");
                }
            }
        }

        private double _Rate;
        public double Rate
        {
            get { return _Rate; }
            set
            {
                if (value != _Rate)
                {
                    _Rate = value;
                    OnPropertyChanged("Rate");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("NetAmount");
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
                    _ConcessionPercentage = value;

                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _ConcessionAmount;
        public double ConcessionAmount
        {
            get
            {
                if (_ConcessionPercentage != 0)
                {
                    return _ConcessionAmount = ((Rate * _ConcessionPercentage) / 100);
                }
                else
                    return _ConcessionAmount;
            }
            set
            {
                if (_ConcessionAmount != value)
                {
                    if (value < 0)
                        value = 0;

                    _ConcessionAmount = value;
                    //_ConcessionPercentage = 0;
                    //OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("NetAmount");

                }
            }
        }

        private double _NetAmount;
        public double NetAmount
        {

            get { return _NetAmount = _Rate - _ConcessionAmount; }
            set
            {
                if (_NetAmount != value)
                {
                    _NetAmount = value;
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private bool _SelectService;
        public bool SelectService
        {
            get { return _SelectService; }
            set
            {
                if (_SelectService != value)
                {
                    _SelectService = value;
                    OnPropertyChanged("SelectService");
                }
            }
        }




        

        #region Common Properties



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

        private string _Unit;
        public string Unit
        {
            get { return _Unit; }
            set
            {
                if (_Unit != value)
                {
                    _Unit = value;
                    OnPropertyChanged("Unit");
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
    
    
    }
}
