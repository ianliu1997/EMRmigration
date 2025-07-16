using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Pathology
{
    public class clsPathoTestOutSourceDetailsVO : IValueObject, INotifyPropertyChanged
    {
        #region Properties
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

        private long _OrderDetailID;
        public long OrderDetailID
        {
            get { return _OrderDetailID; }
            set
            {
                if (_OrderDetailID != value)
                {
                    _OrderDetailID = value;
                    OnPropertyChanged("OrderDetailID");
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

        private long _OrderID;
        public long OrderID
        {
            get { return _OrderID; }
            set
            {
                if (_OrderID != value)
                {
                    _OrderID = value;
                    OnPropertyChanged("OrderID");
                }
            }
        }

        private string _OrderNo;
        public string OrderNo
        {
            get { return _OrderNo; }
            set
            {
                if (_OrderNo != value)
                {
                    _OrderNo = value;
                    OnPropertyChanged("OrderNo");
                }
            }
        }

        private bool _IsChangedAgency;
        [DefaultValue(false)]
        public bool IsChangedAgency
        {
            get { return _IsChangedAgency; }
            set
            {
                if (_IsChangedAgency != value)
                {
                    _IsChangedAgency = value;
                    OnPropertyChanged("IsChangedAgency");
                }

            }
        }

        private bool _IsForOutSource;
        [DefaultValue(false)]
        public bool IsForOutSource
        {
            get { return _IsForOutSource; }
            set
            {
                if (_IsForOutSource != value)
                {
                    _IsForOutSource = value;
                    OnPropertyChanged("IsForOutSource");
                }
            }
        }

        private bool _IsForOutSourceChecked = true;
        public bool IsForOutSourceChecked
        {
            get { return _IsForOutSourceChecked; }
            set
            {
                if (_IsForOutSourceChecked != value)
                {
                    _IsForOutSourceChecked = value;
                    OnPropertyChanged("IsForOutSourceChecked");
                }
            }
        }

        private string _AgencyChangedImage;
        public string AgencyChangedImage
        {
            get { return _AgencyChangedImage; }
            set
            {
                if (_AgencyChangedImage != value)
                {
                    _AgencyChangedImage = value;
                    OnPropertyChanged("AgencyChangedImage");
                }
            }
        }

        private string _AgencyAssignedImage;
        public string AgencyAssignedImage
        {
            get { return _AgencyAssignedImage; }
            set
            {
                if (_AgencyAssignedImage != value)
                {
                    _AgencyAssignedImage = value;
                    OnPropertyChanged("AgencyAssignedImage");
                }
            }
        }

        private DateTime? _OrderDate;
        public DateTime? OrderDate
        {
            get { return _OrderDate; }
            set
            {
                if (_OrderDate != value)
                {
                    _OrderDate = value;
                    OnPropertyChanged("OrderDate");
                }
            }
        }

        private DateTime? _FromDate;
        public DateTime? FromDate
        {
            get { return _FromDate; }
            set
            {
                if (_FromDate != value)
                {
                    _FromDate = value;
                    OnPropertyChanged("FromDate");
                }
            }
        }

        private DateTime? _ToDate;
        public DateTime? ToDate
        {
            get { return _ToDate; }
            set
            {
                if (_ToDate != value)
                {
                    _ToDate = value;
                    OnPropertyChanged("ToDate");
                }
            }
        }

        private long _BillID;
        public long BillID
        {
            get { return _BillID; }
            set
            {
                if (_BillID != value)
                {
                    _BillID = value;
                    OnPropertyChanged("BillID");
                }
            }
        }

        private string _BillNo;
        public string BillNo
        {
            get { return _BillNo; }
            set
            {
                if (_BillNo != value)
                {
                    _BillNo = value;
                    OnPropertyChanged("BillNo");
                }
            }
        }

        private string _MRNo;
        public string MRNo
        {
            get { return _MRNo; }
            set
            {
                if (_MRNo != value)
                {
                    _MRNo = value;
                    OnPropertyChanged("MRNo");
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

        private string _GenderName = "";
        public string GenderName
        {
            get { return _GenderName; }
            set
            {
                if (_GenderName != value)
                {
                    _GenderName = value;
                    OnPropertyChanged("GenderName");
                }
            }
        }

        private string strPatientName = "";
        public string PatientName
        {
            get { return strPatientName = _Prefix+" "+_FirstName + " " + _MiddleName + " " + _LastName; }
            set
            {
                if (value != strPatientName)
                {
                    strPatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }

        private string _TestName = String.Empty;
        public string TestName
        {
            get { return _TestName; }
            set
            {
                if (_TestName != value)
                {
                    _TestName = value;
                    OnPropertyChanged("TestName");
                }
            }
        }

        private long _TestID;
        public long TestID
        {
            get { return _TestID; }
            set
            {
                if (_TestID != value)
                {
                    _TestID = value;
                    OnPropertyChanged("TestID");
                }
            }
        }

        private long _AgencyID;
        public long AgencyID
        {
            get { return _AgencyID; }
            set
            {
                if (_AgencyID != value)
                {
                    _AgencyID = value;
                    OnPropertyChanged("AgencyID");
                }
            }
        }

        private long _PreviousAgencyID;
        public long PreviousAgencyID
        {
            get { return _PreviousAgencyID; }
            set
            {
                if (_PreviousAgencyID != value)
                {
                    _PreviousAgencyID = value;
                    OnPropertyChanged("PreviousAgencyID");
                }
            }
        }

        private long _ChangedAgencyID;
        public long ChangedAgencyID
        {
            get { return _ChangedAgencyID; }
            set
            {
                if (_ChangedAgencyID != value)
                {
                    _ChangedAgencyID = value;
                    OnPropertyChanged("ChangedAgencyID");
                }
            }
        }

        private string _ReasonToChangeAgency;
        public string ReasonToChangeAgency
        {
            get { return _ReasonToChangeAgency; }
            set
            {
                if (_ReasonToChangeAgency != value)
                {
                    _ReasonToChangeAgency = value;
                    OnPropertyChanged("ReasonToChangeAgency");
                }
            }
        }

        private string _AgencyAssignReason;
        public string AgencyAssignReason
        {
            get { return _AgencyAssignReason; }
            set
            {
                if (_AgencyAssignReason != value)
                {
                    _AgencyAssignReason = value;
                    OnPropertyChanged("AgencyAssignReason");
                }
            }
        }

        private string _AgencyName;
        public string AgencyName
        {
            get { return _AgencyName; }
            set
            {
                if (_AgencyName != value)
                {
                    _AgencyName = value;
                    OnPropertyChanged("AgencyName");
                }
            }
        }

        private DateTime _Date;
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

        private bool _IsOutSourced;
        public bool IsOutSourced
        {
            get { return _IsOutSourced; }
            set
            {
                if (_IsOutSourced != value)
                {
                    _IsOutSourced = value;
                    OnPropertyChanged("IsOutSourced");
                }
            }
        }


        private bool _IsOutSourced1;
        public bool IsOutSourced1
        {
            get { return _IsOutSourced1; }
            set
            {
                if (_IsOutSourced1 != value)
                {
                    _IsOutSourced1 = value;
                    OnPropertyChanged("IsOutSourced1");
                }
            }
        }
        private bool _IsSampleCollected;
        public bool IsSampleCollected
        {
            get { return _IsSampleCollected; }
            set
            {
                if (_IsSampleCollected != value)
                {
                    _IsSampleCollected = value;
                    OnPropertyChanged("IsSampleCollected");
                }
            }
        }

        private DateTime? _SampleDispatchDateTime;
        public DateTime? SampleDispatchDateTime
        {
            get { return _SampleDispatchDateTime; }
            set
            {
                if (_SampleDispatchDateTime != value)
                {
                    _SampleDispatchDateTime = value;
                    OnPropertyChanged("SampleDispatchDateTime");
                }
            }
        }

        private string _SampleCollectedImage;
        public string SampleCollectedImage
        {
            get { return _SampleCollectedImage; }
            set
            {
                if (_SampleCollectedImage != value)
                {
                    _SampleCollectedImage = value;
                    OnPropertyChanged("SampleCollectedImage");
                }
            }
        }
        private string _Prefix;
        public string Prefix
        {
            get { return _Prefix; }
            set
            {
                if (_Prefix != value)
                {
                    _Prefix = value;
                    OnPropertyChanged("Prefix");
                }
            }
        }
        private bool _IsForUnassignedAgencyTest;
        [DefaultValue(false)]
        public bool IsForUnassignedAgencyTest
        {
            get { return _IsForUnassignedAgencyTest; }
            set
            {
                if (_IsForUnassignedAgencyTest != value)
                {
                    _IsForUnassignedAgencyTest = value;
                    OnPropertyChanged("IsOutSourced");
                }
            }
        }

        private bool _OutSourceType;
        [DefaultValue(false)]
        public bool OutSourceType
        {
            get { return _OutSourceType; }
            set
            {
                if (_OutSourceType != value)
                {
                    _OutSourceType = value;
                    OnPropertyChanged("OutSourceType");
                }
            }
        }

        // Added By Anumani on 14/03/2016
        // In Order to get map only assigned Agencies to the respective services 
        // In the ComboBox of Change Test Agency

        private long _ServiceId;
        public long ServiceID
        {
            get { return _ServiceId; }
            set
            {
                if (_ServiceId != value)
                {
                    _ServiceId = value;
                    OnPropertyChanged("ServiceID");
                }
            }
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
        private string _UnitName;
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                if (_UnitName != value)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
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
