using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.ANC
{
    public class clsANCVO : IValueObject, INotifyPropertyChanged
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

        #region Properties

        private long _CoupleID;
        public long CoupleID
        {
            get { return _CoupleID; }
            set
            {
                if (_CoupleID != value)
                {
                    _CoupleID = value;
                    OnPropertyChanged("CoupleID");
                }
            }
        }

        private long _CoupleUnitID;
        public long CoupleUnitID
        {
            get { return _CoupleUnitID; }
            set
            {
                if (_CoupleUnitID != value)
                {
                    _CoupleUnitID = value;
                    OnPropertyChanged("CoupleUnitID");
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
        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }

        private string _ANC_Code;
        public string ANC_Code
        {
            get { return _ANC_Code; }
            set
            {
                if (_ANC_Code != value)
                {
                    _ANC_Code = value;
                    OnPropertyChanged("ANC_Code");
                }
            }
        }
        private DateTime? _Date = null;
        public DateTime? Date
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
        private DateTime? _DateofMarriage = null;
        public DateTime? DateofMarriage
        {
            get { return _DateofMarriage; }
            set
            {
                if (_DateofMarriage != value)
                {
                    _DateofMarriage = value;
                    OnPropertyChanged("DateofMarriage");
                }
            }
        }
        private string _AgeOfMenarche;
        public string AgeOfMenarche
        {
            get { return _AgeOfMenarche; }
            set
            {
                if (_AgeOfMenarche != value)
                {
                    _AgeOfMenarche = value;
                    OnPropertyChanged("AgeOfMenarche");
                }
            }
        }
        private string _M;
        public string M
        {
            get { return _M; }
            set
            {
                if (_M != value)
                {
                    _M = value;
                    OnPropertyChanged("M");
                }
            }
        }
        private string _G;
        public string G
        {
            get { return _G; }
            set
            {
                if (_G != value)
                {
                    _G = value;
                    OnPropertyChanged("G");
                }
            }
        }
        private string _P;
        public string P
        {
            get { return _P; }
            set
            {
                if (_P != value)
                {
                    _P = value;
                    OnPropertyChanged("P");
                }
            }
        }
        private string _A;
        public string A
        {
            get { return _A; }
            set
            {
                if (_A != value)
                {
                    _A = value;
                    OnPropertyChanged("A");
                }
            }
        }
        private string _L;
        public string L
        {
            get { return _L; }
            set
            {
                if (_L != value)
                {
                    _L = value;
                    OnPropertyChanged("L");
                }
            }
        }

        private DateTime? _LMPDate = null;
        public DateTime? LMPDate
        {
            get { return _LMPDate; }
            set
            {
                if (_LMPDate != value)
                {
                    _LMPDate = value;
                    OnPropertyChanged("LMPDate");
                }
            }
        }

        private DateTime? _EDDDate = null;
        public DateTime? EDDDate
        {
            get { return _EDDDate; }
            set
            {
                if (_EDDDate != value)
                {
                    _EDDDate = value;
                    OnPropertyChanged("EDDDate");
                }
            }
        }

        private long _Gender;
        public long Gender
        {
            get { return _Gender; }
            set
            {
                if (_Gender != value)
                {
                    _Gender = value;
                    OnPropertyChanged("Gender");
                }
            }
        }
        private string _SpecialRemark;
        public string SpecialRemark
        {
            get { return _SpecialRemark; }
            set
            {
                if (_SpecialRemark != value)
                {
                    _SpecialRemark = value;
                    OnPropertyChanged("SpecialRemark");
                }
            }
        }
        private bool _Isfreezed;
        public bool Isfreezed
        {
            get { return _Isfreezed; }
            set
            {
                if (_Isfreezed != value)
                {
                    _Isfreezed = value;
                    OnPropertyChanged("Isfreezed");
                }
            }
        }
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
        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set
            {
                if (_IsClosed != value)
                {
                    _IsClosed = value;
                    OnPropertyChanged("IsClosed");
                }
            }
        }
        #endregion
    }

    public class clsANCDocumentsVO : IValueObject, INotifyPropertyChanged
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
        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }

        private long _CoupleID;
        public long CoupleID
        {
            get { return _CoupleID; }
            set
            {
                if (_CoupleID != value)
                {
                    _CoupleID = value;
                    OnPropertyChanged("CoupleID");
                }
            }
        }

        private long _CoupleUnitID;
        public long CoupleUnitID
        {
            get { return _CoupleUnitID; }
            set
            {
                if (_CoupleUnitID != value)
                {
                    _CoupleUnitID = value;
                    OnPropertyChanged("CoupleUnitID");
                }
            }
        }

        private DateTime? _Date = null;
        public DateTime? Date
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

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                if (_Title != value)
                {
                    _Title = value;
                    OnPropertyChanged("Title");
                }
            }
        }
        private long _ANCID;
        public long ANCID
        {
            get { return _ANCID; }
            set
            {
                if (_ANCID != value)
                {
                    _ANCID = value;
                    OnPropertyChanged("ANCID");
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
        private string _AttachedFileName;
        public string AttachedFileName
        {
            get { return _AttachedFileName; }
            set
            {
                if (_AttachedFileName != value)
                {
                    _AttachedFileName = value;
                    OnPropertyChanged("AttachedFileName");
                }
            }
        }
        private byte[] _AttachedFileContent;
        public byte[] AttachedFileContent
        {
            get { return _AttachedFileContent; }
            set
            {
                if (_AttachedFileContent != value)
                {
                    _AttachedFileContent = value;
                    OnPropertyChanged("AttachedFileContent");
                }
            }
        }
        private bool _IsDeleted;
        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set
            {
                if (_IsDeleted != value)
                {
                    _IsDeleted = value;
                    OnPropertyChanged("IsDeleted");
                }
            }
        }

        private bool _Isfreezed;
        public bool Isfreezed
        {
            get { return _Isfreezed; }
            set
            {
                if (_Isfreezed != value)
                {
                    _Isfreezed = value;
                    OnPropertyChanged("Isfreezed");
                }
            }
        }
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




        #endregion
    }

    public class clsANCExaminationVO : IValueObject, INotifyPropertyChanged
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

        private long _ANCID;
        public long ANCID
        {
            get { return _ANCID; }
            set
            {
                if (_ANCID != value)
                {
                    _ANCID = value;
                    OnPropertyChanged("ANCID");
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
        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }

        private DateTime? _ExaDate = null;
        public DateTime? ExaDate
        {
            get { return _ExaDate; }
            set
            {
                if (_ExaDate != value)
                {
                    _ExaDate = value;
                    OnPropertyChanged("ExaDate");
                }
            }
        }

        private bool _Isfreezed;
        public bool Isfreezed
        {
            get { return _Isfreezed; }
            set
            {
                if (_Isfreezed != value)
                {
                    _Isfreezed = value;
                    OnPropertyChanged("Isfreezed");
                }
            }
        }

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

        private List<clsANCExaminationDetailsVO> _ANCExaminationDetailsList;
        public List<clsANCExaminationDetailsVO> ANCExaminationDetailsList
        {
            get
            {
                return _ANCExaminationDetailsList;
            }
            set
            {
                _ANCExaminationDetailsList = value;
            }
        }

        #endregion
    }

    public class clsANCExaminationDetailsVO : IValueObject, INotifyPropertyChanged
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

        private long _ExaminationID;
        public long ExaminationID
        {
            get { return _ExaminationID; }
            set
            {
                if (_ExaminationID != value)
                {
                    _ExaminationID = value;
                    OnPropertyChanged("ExaminationID");
                }
            }
        }
        private long _CoupleID;
        public long CoupleID
        {
            get { return _CoupleID; }
            set
            {
                if (_CoupleID != value)
                {
                    _CoupleID = value;
                    OnPropertyChanged("CoupleID");
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
        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }
        private long _CoupleUnitID;
        public long CoupleUnitID
        {
            get { return _CoupleUnitID; }
            set
            {
                if (_CoupleUnitID != value)
                {
                    _CoupleUnitID = value;
                    OnPropertyChanged("CoupleUnitID");
                }
            }
        }
        private long _ANCID;
        public long ANCID
        {
            get { return _ANCID; }
            set
            {
                if (_ANCID != value)
                {
                    _ANCID = value;
                    OnPropertyChanged("ANCID");
                }
            }
        }
        private DateTime? _ExaDate = null;
        public DateTime? ExaDate
        {
            get { return _ExaDate; }
            set
            {
                if (_ExaDate != value)
                {
                    _ExaDate = value;
                    OnPropertyChanged("ExaDate");
                }
            }
        }

        private DateTime? _ExaTime = null;
        public DateTime? ExaTime
        {
            get { return _ExaTime; }
            set
            {
                if (_ExaTime != value)
                {
                    _ExaTime = value;
                    OnPropertyChanged("ExaTime");
                }
            }
        }

        private long _Doctor;
        public long Doctor
        {
            get { return _Doctor; }
            set
            {
                if (_Doctor != value)
                {
                    _Doctor = value;
                    OnPropertyChanged("Doctor");
                }
            }
        }
        private string _PeriodOfPreg;
        public string PeriodOfPreg
        {
            get { return _PeriodOfPreg; }
            set
            {
                if (_PeriodOfPreg != value)
                {
                    _PeriodOfPreg = value;
                    OnPropertyChanged("PeriodOfPreg");
                }
            }
        }
        private float _Weight;
        public float Weight
        {
            get { return _Weight; }
            set
            {
                if (_Weight != value)
                {
                    _Weight = value;
                    OnPropertyChanged("Height");
                }
            }
        }
        private float _BP;
        public float BP
        {
            get { return _BP; }
            set
            {
                if (_BP != value)
                {
                    _BP = value;
                    OnPropertyChanged("BP");
                }
            }
        }
        private long _PresenAndPos;
        public long PresenAndPos
        {
            get { return _PresenAndPos; }
            set
            {
                if (_PresenAndPos != value)
                {
                    _PresenAndPos = value;
                    OnPropertyChanged("PresenAndPos");
                }
            }
        }
        private long _FHS;
        public long FHS
        {
            get { return _FHS; }
            set
            {
                if (_FHS != value)
                {
                    _FHS = value;
                    OnPropertyChanged("FHS");
                }
            }
        }
        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set
            {
                if (_Remark != value)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }

        private string _Oadema;
        public string Oadema
        {
            get { return _Oadema; }
            set
            {
                if (_Oadema != value)
                {
                    _Oadema = value;
                    OnPropertyChanged("Oadema");
                }
            }
        }
        private long _OademaID;
        public long OademaID
        {
            get { return _OademaID; }
            set
            {
                if (_OademaID != value)
                {
                    _OademaID = value;
                    OnPropertyChanged("OademaID");
                }
            }
        }
        private float _FundalHeight;
        public float FundalHeight
        {
            get { return _FundalHeight; }
            set
            {
                if (_FundalHeight != value)
                {
                    _FundalHeight = value;
                    OnPropertyChanged("FundalHeight");
                }
            }
        }

        private string _PresenAndPosDescription;
        public string PresenAndPosDescription
        {
            get { return _PresenAndPosDescription; }
            set
            {
                if (_PresenAndPosDescription != value)
                {
                    _PresenAndPosDescription = value;
                    OnPropertyChanged("PresenAndPosDescription");
                }
            }
        }
        private string _FHSDescription;
        public string FHSDescription
        {
            get { return _FHSDescription; }
            set
            {
                if (_FHSDescription != value)
                {
                    _FHSDescription = value;
                    OnPropertyChanged("FHSDescription");
                }
            }
        }
        private string _DoctorName;
        public string DoctorName
        {
            get { return _DoctorName; }
            set
            {
                if (_DoctorName != value)
                {
                    _DoctorName = value;
                    OnPropertyChanged("DoctorName");
                }
            }
        }
        private string _Oadema2;
        public string Oadema2
        {
            get { return _Oadema2; }
            set
            {
                if (_Oadema2 != value)
                {
                    _Oadema2 = value;
                    OnPropertyChanged("Oadema2");
                }
            }
        }

        private string _Oadema3;
        public string Oadema3
        {
            get { return _Oadema3; }
            set
            {
                if (_Oadema3 != value)
                {
                    _Oadema3 = value;
                    OnPropertyChanged("Oadema3");
                }
            }
        }
        private string _RelationtoBrim;
        public string RelationtoBrim
        {
            get { return _RelationtoBrim; }
            set
            {
                if (_RelationtoBrim != value)
                {
                    _RelationtoBrim = value;
                    OnPropertyChanged("RelationtoBrim");
                }
            }
        }

        private string _Treatment;
        public string Treatment
        {
            get { return _Treatment; }
            set
            {
                if (_Treatment != value)
                {
                    _Treatment = value;
                    OnPropertyChanged("Treatment");
                }
            }
        }
        private string _HTofUterus;
        public string HTofUterus
        {
            get { return _HTofUterus; }
            set
            {
                if (_HTofUterus != value)
                {
                    _HTofUterus = value;
                    OnPropertyChanged("HTofUterus");
                }
            }
        }
        private string _AbdGirth;
        public string AbdGirth
        {
            get { return _AbdGirth; }
            set
            {
                if (_AbdGirth != value)
                {
                    _AbdGirth = value;
                    OnPropertyChanged("AbdGirth");
                }
            }
        }
        private bool _Isfreezed;
        public bool Isfreezed
        {
            get { return _Isfreezed; }
            set
            {
                if (_Isfreezed != value)
                {
                    _Isfreezed = value;
                    OnPropertyChanged("Isfreezed");
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

        private bool _IsView = true;
        public bool IsView
        {
            get { return _IsView; }
            set
            {
                if (_IsView != value)
                {
                    _IsView = value;
                    OnPropertyChanged("IsView");
                }
            }
        }

        #endregion
    }

    public class clsANCHistoryVO : IValueObject, INotifyPropertyChanged
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
        private long _CoupleID;
        public long CoupleID
        {
            get { return _CoupleID; }
            set
            {
                if (_CoupleID != value)
                {
                    _CoupleID = value;
                    OnPropertyChanged("CoupleID");
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
        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }
        private long _CoupleUnitID;
        public long CoupleUnitID
        {
            get { return _CoupleUnitID; }
            set
            {
                if (_CoupleUnitID != value)
                {
                    _CoupleUnitID = value;
                    OnPropertyChanged("CoupleUnitID");
                }
            }
        }
        private long _ANCID;
        public long ANCID
        {
            get { return _ANCID; }
            set
            {
                if (_ANCID != value)
                {
                    _ANCID = value;
                    OnPropertyChanged("ANCID");
                }
            }
        }
        private bool _Hypertension;
        public bool Hypertension
        {
            get { return _Hypertension; }
            set
            {
                if (_Hypertension != value)
                {
                    _Hypertension = value;
                    OnPropertyChanged("Hypertension");
                }
            }
        }
        private bool _TB;
        public bool TB
        {
            get { return _TB; }
            set
            {
                if (_TB != value)
                {
                    _TB = value;
                    OnPropertyChanged("TB");
                }
            }
        }
        private bool _Diabeties;
        public bool Diabeties
        {
            get { return _Diabeties; }
            set
            {
                if (_Diabeties != value)
                {
                    _Diabeties = value;
                    OnPropertyChanged("Diabeties");
                }
            }
        }
        private bool _CongenitalAnomolies;
        public bool CongenitalAnomolies
        {
            get { return _CongenitalAnomolies; }
            set
            {
                if (_CongenitalAnomolies != value)
                {
                    _CongenitalAnomolies = value;
                    OnPropertyChanged("CongenitalAnomolies");
                }
            }
        }
        private DateTime? _LMPDate;
        public DateTime? LMPDate
        {
            get { return _LMPDate; }
            set
            {
                if (_LMPDate != value)
                {
                    _LMPDate = value;
                    OnPropertyChanged("LMPDate");
                }
            }
        }

        private DateTime? _EDDDate;
        public DateTime? EDDDate
        {
            get { return _EDDDate; }
            set
            {
                if (_EDDDate != value)
                {
                    _EDDDate = value;
                    OnPropertyChanged("EDDDate");
                }
            }
        }
        private string _PastRemark;
        public string PastRemark
        {
            get { return _PastRemark; }
            set
            {
                if (_PastRemark != value)
                {
                    _PastRemark = value;
                    OnPropertyChanged("PastRemark");
                }
            }
        }
        private bool _TTIstDose;
        public bool TTIstDose
        {
            get { return _TTIstDose; }
            set
            {
                if (_TTIstDose != value)
                {
                    _TTIstDose = value;
                    OnPropertyChanged("TTIstDose");
                }
            }
        }
        private bool _TTIIstDose;
        public bool TTIIstDose
        {
            get { return _TTIIstDose; }
            set
            {
                if (_TTIIstDose != value)
                {
                    _TTIIstDose = value;
                    OnPropertyChanged("TTIIstDose");
                }
            }
        }

        private DateTime? _DateIstDose = null;
        public DateTime? DateIstDose
        {
            get { return _DateIstDose; }
            set
            {
                if (_DateIstDose != value)
                {
                    _DateIstDose = value;
                    OnPropertyChanged("DateIstDose");
                }
            }
        }
        private DateTime? _DateIIstDose = null;
        public DateTime? DateIIstDose
        {
            get { return _DateIIstDose; }
            set
            {
                if (_DateIIstDose != value)
                {
                    _DateIIstDose = value;
                    OnPropertyChanged("DateIIstDose ");
                }
            }
        }
        private bool _Isfreezed;
        public bool Isfreezed
        {
            get { return _Isfreezed; }
            set
            {
                if (_Isfreezed != value)
                {
                    _Isfreezed = value;
                    OnPropertyChanged("Isfreezed");
                }
            }
        }
        private DateTime? _LLMPDate = null;
        public DateTime? LLMPDate
        {
            get { return _LLMPDate; }
            set
            {
                if (_LLMPDate != value)
                {
                    _LLMPDate = value;
                    OnPropertyChanged("LLMPDate");
                }
            }
        }
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

        private string _Menstrualcycle;
        public string Menstrualcycle
        {
            get { return _Menstrualcycle; }
            set
            {
                if (_Menstrualcycle != value)
                {
                    _Menstrualcycle = value;
                    OnPropertyChanged("Menstrualcycle");
                }
            }
        }
        private string _Duration;
        public string Duration
        {
            get { return _Duration; }
            set
            {
                if (_Duration != value)
                {
                    _Duration = value;
                    OnPropertyChanged("Duration");
                }
            }
        }
        private string _Disorder;
        public string Disorder
        {
            get { return _Disorder; }
            set
            {
                if (_Disorder != value)
                {
                    _Disorder = value;
                    OnPropertyChanged("Disorder");
                }
            }
        }
        private bool _Twins;
        public bool Twins
        {
            get { return _Twins; }
            set
            {
                if (_Twins != value)
                {
                    _Twins = value;
                    OnPropertyChanged("Twins");
                }
            }
        }
        private bool _PersonalHistory;
        public bool PersonalHistory
        {
            get { return _PersonalHistory; }
            set
            {
                if (_PersonalHistory != value)
                {
                    _PersonalHistory = value;
                    OnPropertyChanged("PersonalHistory");
                }
            }
        }
        private string _txtTwins;
        public string txtTwins
        {
            get { return _txtTwins; }
            set
            {
                if (_txtTwins != value)
                {
                    _txtTwins = value;
                    OnPropertyChanged("txtTwins");
                }
            }
        }
        private string _txtPersonalHistory;
        public string txtPersonalHistory
        {
            get { return _txtPersonalHistory; }
            set
            {
                if (_txtPersonalHistory != value)
                {
                    _txtPersonalHistory = value;
                    OnPropertyChanged("txtPersonalHistory");
                }
            }
        }
        private string _txtHypertension;
        public string txtHypertension
        {
            get { return _txtHypertension; }
            set
            {
                if (_txtHypertension != value)
                {
                    _txtHypertension = value;
                    OnPropertyChanged("txtHypertension");
                }
            }
        }
        private string _txtTB;
        public string txtTB
        {
            get { return _txtTB; }
            set
            {
                if (_txtTB != value)
                {
                    _txtTB = value;
                    OnPropertyChanged("txtTB");
                }
            }
        }
        private string _txtDiabeties;
        public string txtDiabeties
        {
            get { return _txtDiabeties; }
            set
            {
                if (_txtDiabeties != value)
                {
                    _txtDiabeties = value;
                    OnPropertyChanged("txtDiabeties");
                }
            }
        }
        private string _DrugsPresent;
        public string DrugsPresent
        {
            get { return _DrugsPresent; }
            set
            {
                if (_DrugsPresent != value)
                {
                    _DrugsPresent = value;
                    OnPropertyChanged("DrugsPresent");
                }
            }
        }
        private string _DrugsPast;
        public string DrugsPast
        {
            get { return _DrugsPast; }
            set
            {
                if (_DrugsPast != value)
                {
                    _DrugsPast = value;
                    OnPropertyChanged("DrugsPast");
                }
            }
        }
        private string _Surgery;
        public string Surgery
        {
            get { return _Surgery; }
            set
            {
                if (_Surgery != value)
                {
                    _Surgery = value;
                    OnPropertyChanged("Surgery");
                }
            }
        }
        private string _Drugs;
        public string Drugs
        {
            get { return _Drugs; }
            set
            {
                if (_Drugs != value)
                {
                    _Drugs = value;
                    OnPropertyChanged("Drugs");
                }
            }
        }



        private float _Weight;
        public float Weight
        {
            get { return _Weight; }
            set
            {
                if (_Weight != value)
                {
                    _Weight = value;
                    OnPropertyChanged("Weight");
                }
            }
        }

        private float _Height;
        public float Height
        {
            get { return _Height; }
            set
            {
                if (_Height != value)
                {
                    _Height = value;
                    OnPropertyChanged("Height");
                }
            }
        }

        private float _BpInMm;
        public float BpInMm
        {
            get { return _BpInMm; }
            set
            {
                if (_BpInMm != value)
                {
                    _BpInMm = value;
                    OnPropertyChanged("BpInMm");
                }
            }
        }

        private float _BpInHg;
        public float BpInHg
        {
            get { return _BpInHg; }
            set
            {
                if (_BpInHg != value)
                {
                    _BpInHg = value;
                    OnPropertyChanged("BpInHg");
                }
            }
        }

        private float _Pulse;
        public float Pulse
        {
            get { return _Pulse; }
            set
            {
                if (_Pulse != value)
                {
                    _Pulse = value;
                    OnPropertyChanged("Pulse");
                }
            }
        }


        private string _RS;
        public string RS
        {
            get { return _RS; }
            set
            {
                if (_RS != value)
                {
                    _RS = value;
                    OnPropertyChanged("RS");
                }
            }
        }

        private string _CVS;
        public string CVS
        {
            get { return _CVS; }
            set
            {
                if (_CVS != value)
                {
                    _CVS = value;
                    OnPropertyChanged("CVS");
                }
            }
        }


        private string _CNS;
        public string CNS
        {
            get { return _CNS; }
            set
            {
                if (_CNS != value)
                {
                    _CNS = value;
                    OnPropertyChanged("CNS");
                }
            }
        }


        private string _Anaemia;
        public string Anaemia
        {
            get { return _Anaemia; }
            set
            {
                if (_Anaemia != value)
                {
                    _Anaemia = value;
                    OnPropertyChanged("Anaemia");
                }
            }
        }
        private string _Lymphadenopathy;
        public string Lymphadenopathy
        {
            get { return _Lymphadenopathy; }
            set
            {
                if (_Lymphadenopathy != value)
                {
                    _Lymphadenopathy = value;
                    OnPropertyChanged("Lymphadenopathy");
                }
            }
        }
        private string _Edema;
        public string Edema
        {
            get { return _Edema; }
            set
            {
                if (_Edema != value)
                {
                    _Edema = value;
                    OnPropertyChanged("Edema");
                }
            }
        }
        private string _Breasts;
        public string Breasts
        {
            get { return _Breasts; }
            set
            {
                if (_Breasts != value)
                {
                    _Breasts = value;
                    OnPropertyChanged("Breasts");
                }
            }
        }
        private bool _Hirsutism;
        public bool Hirsutism
        {
            get { return _Hirsutism; }
            set
            {
                if (_Hirsutism != value)
                {
                    _Hirsutism = value;
                    OnPropertyChanged("Hirsutism");
                }
            }
        }
        private long _HirsutismID;
        public long HirsutismID
        {
            get { return _HirsutismID; }
            set
            {
                if (_HirsutismID != value)
                {
                    _HirsutismID = value;
                    OnPropertyChanged("HirsutismID");
                }
            }
        }
        private string _Goitre;
        public string Goitre
        {
            get { return _Goitre; }
            set
            {
                if (_Goitre != value)
                {
                    _Goitre = value;
                    OnPropertyChanged("Goitre");
                }
            }
        }
        private string _frequencyandtimingofintercourse;
        public string frequencyandtimingofintercourse
        {
            get { return _frequencyandtimingofintercourse; }
            set
            {
                if (_frequencyandtimingofintercourse != value)
                {
                    _frequencyandtimingofintercourse = value;
                    OnPropertyChanged("frequencyandtimingofintercourse");
                }
            }
        }
        private string _Flurseminis;
        public string Flurseminis
        {
            get { return _Flurseminis; }
            set
            {
                if (_Flurseminis != value)
                {
                    _Flurseminis = value;
                    OnPropertyChanged("Flurseminis");
                }
            }
        }
        private string _AnyOtherfactor;
        public string AnyOtherfactor
        {
            get { return _AnyOtherfactor; }
            set
            {
                if (_AnyOtherfactor != value)
                {
                    _AnyOtherfactor = value;
                    OnPropertyChanged("AnyOtherfactor");
                }
            }
        }
        private string _Otherimportantreleventfactor;
        public string Otherimportantreleventfactor
        {
            get { return _Otherimportantreleventfactor; }
            set
            {
                if (_Otherimportantreleventfactor != value)
                {
                    _Otherimportantreleventfactor = value;
                    OnPropertyChanged("Otherimportantreleventfactor");
                }
            }
        }
        #endregion
    }

    public class clsANCInvestigationVO : IValueObject, INotifyPropertyChanged
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
        private long _ANCID;
        public long ANCID
        {
            get { return _ANCID; }
            set
            {
                if (_ANCID != value)
                {
                    _ANCID = value;
                    OnPropertyChanged("ANCID");
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
        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }
        private DateTime _InvDate;
        public DateTime InvDate
        {
            get { return _InvDate; }
            set
            {
                if (_InvDate != value)
                {
                    _InvDate = value;
                    OnPropertyChanged("InvDate");
                }
            }
        }
        private long _Investigation;
        public long Investigation
        {
            get { return _Investigation; }
            set
            {
                if (_Investigation != value)
                {
                    _Investigation = value;
                    OnPropertyChanged("Investigation");
                }
            }
        }
        private string _Report;
        public string Report
        {
            get { return _Report; }
            set
            {
                if (_Report != value)
                {
                    _Report = value;
                    OnPropertyChanged("Report");
                }
            }
        }
        private bool _Isfreezed;
        public bool Isfreezed
        {
            get { return _Isfreezed; }
            set
            {
                if (_Isfreezed != value)
                {
                    _Isfreezed = value;
                    OnPropertyChanged("Isfreezed");
                }
            }
        }
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

        private List<clsANCInvestigationDetailsVO> _ANCInvestigationDetailsList;
        public List<clsANCInvestigationDetailsVO> ANCInvestigationDetailsList
        {
            get
            {
                return _ANCInvestigationDetailsList;
            }
            set
            {
                _ANCInvestigationDetailsList = value;
            }
        }


        #endregion
    }

    public class clsANCInvestigationDetailsVO : IValueObject, INotifyPropertyChanged
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
        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
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
        private long _ANCID;
        public long ANCID
        {
            get { return _ANCID; }
            set
            {
                if (_ANCID != value)
                {
                    _ANCID = value;
                    OnPropertyChanged("ANCID");
                }
            }
        }
        private long _InvestigationID;
        public long InvestigationID
        {
            get { return _InvestigationID; }
            set
            {
                if (_InvestigationID != value)
                {
                    _InvestigationID = value;
                    OnPropertyChanged("InvestigationID");
                }
            }
        }
        private long _CoupleID;
        public long CoupleID
        {
            get { return _CoupleID; }
            set
            {
                if (_CoupleID != value)
                {
                    _CoupleID = value;
                    OnPropertyChanged("CoupleID");
                }
            }
        }

        private long _CoupleUnitID;
        public long CoupleUnitID
        {
            get { return _CoupleUnitID; }
            set
            {
                if (_CoupleUnitID != value)
                {
                    _CoupleUnitID = value;
                    OnPropertyChanged("CoupleUnitID");
                }
            }
        }
        private DateTime _InvDate;
        public DateTime InvDate
        {
            get { return _InvDate; }
            set
            {
                if (_InvDate != value)
                {
                    _InvDate = value;
                    OnPropertyChanged("InvDate");
                }
            }
        }
        private string _Investigation;
        public string Investigation
        {
            get { return _Investigation; }
            set
            {
                if (_Investigation != value)
                {
                    _Investigation = value;
                    OnPropertyChanged("Investigation");
                }
            }
        }
        private string _Report;
        public string Report
        {
            get { return _Report; }
            set
            {
                if (_Report != value)
                {
                    _Report = value;
                    OnPropertyChanged("Report");
                }
            }
        }
        private decimal _Height;
        public decimal Height
        {
            get { return _Height; }
            set
            {
                if (_Height != value)
                {
                    _Height = value;
                    OnPropertyChanged("Height");
                }
            }
        }
        private decimal _Breast;
        public decimal Breast
        {
            get { return _Breast; }
            set
            {
                if (_Breast != value)
                {
                    _Breast = value;
                    OnPropertyChanged("Breast");
                }
            }
        }
        private decimal _Chest;
        public decimal Chest
        {
            get { return _Chest; }
            set
            {
                if (_Chest != value)
                {
                    _Chest = value;
                    OnPropertyChanged("Chest");
                }
            }
        }
        private bool _Isfreezed;
        public bool Isfreezed
        {
            get { return _Isfreezed; }
            set
            {
                if (_Isfreezed != value)
                {
                    _Isfreezed = value;
                    OnPropertyChanged("Isfreezed");
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

        private bool _IsView = true;
        public bool IsView
        {
            get { return _IsView; }
            set
            {
                if (_IsView != value)
                {
                    _IsView = value;
                    OnPropertyChanged("IsView");
                }
            }
        }

        #endregion
    }

    public class clsANCObstetricHistoryVO : IValueObject, INotifyPropertyChanged
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
        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
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
        private long _HistoryID;
        public long HistoryID
        {
            get { return _HistoryID; }
            set
            {
                if (_HistoryID != value)
                {
                    _HistoryID = value;
                    OnPropertyChanged("HistoryID");
                }
            }
        }
        private string _MaturityComplication;
        public string MaturityComplication
        {
            get { return _MaturityComplication; }
            set
            {
                if (_MaturityComplication != value)
                {
                    _MaturityComplication = value;
                    OnPropertyChanged("MaturityComplication");
                }
            }
        }
        private string _ObstetricRemark;
        public string ObstetricRemark
        {
            get { return _ObstetricRemark; }
            set
            {
                if (_ObstetricRemark != value)
                {
                    _ObstetricRemark = value;
                    OnPropertyChanged("ObstetricRemark");
                }
            }
        }
        private string _ModeOfDelivary;
        public string ModeOfDelivary
        {
            get { return _ModeOfDelivary; }
            set
            {
                if (_ModeOfDelivary != value)
                {
                    _ModeOfDelivary = value;
                    OnPropertyChanged("ModeOfDelivary");
                }
            }
        }
        private bool _Isfreezed;
        public bool Isfreezed
        {
            get { return _Isfreezed; }
            set
            {
                if (_Isfreezed != value)
                {
                    _Isfreezed = value;
                    OnPropertyChanged("Isfreezed");
                }
            }
        }
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
        private DateTime? _DateYear;
        public DateTime? DateYear
        {
            get { return _DateYear; }
            set
            {
                if (_DateYear != value)
                {
                    _DateYear = value;
                    OnPropertyChanged("DateYear");
                }
            }
        }

        private string _Gestation;
        public string Gestation
        {
            get { return _Gestation; }
            set
            {
                if (_Gestation != value)
                {
                    _Gestation = value;
                    OnPropertyChanged("Gestation");
                }
            }
        }
        private string _TypeofDelivery;
        public string TypeofDelivery
        {
            get { return _TypeofDelivery; }
            set
            {
                if (_TypeofDelivery != value)
                {
                    _TypeofDelivery = value;
                    OnPropertyChanged("TypeofDelivery");
                }
            }
        }
        private long _Baby;
        public long Baby
        {
            get { return _Baby; }
            set
            {
                if (_Baby != value)
                {
                    _Baby = value;
                    OnPropertyChanged("Baby");
                }
            }
        }

        private bool _IsView = true;
        public bool IsView
        {
            get { return _IsView; }
            set
            {
                if (_IsView != value)
                {
                    _IsView = value;
                    OnPropertyChanged("IsView");
                }
            }
        }

        private List<clsANCObstetricHistoryDetailsVO> _ANCObstetricHistoryDetailsList;
        public List<clsANCObstetricHistoryDetailsVO> ANCObstetricHistoryDetailsList
        {
            get
            {
                return _ANCObstetricHistoryDetailsList;
            }
            set
            {
                _ANCObstetricHistoryDetailsList = value;
            }
        }

        #endregion
    }

    public class clsANCObstetricHistoryDetailsVO : IValueObject, INotifyPropertyChanged
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
        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
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
        private long _ObstetricHistoryID;
        public long ObstetricHistoryID
        {
            get { return _ObstetricHistoryID; }
            set
            {
                if (_ObstetricHistoryID != value)
                {
                    _ObstetricHistoryID = value;
                    OnPropertyChanged("ObstetricHistoryID");
                }
            }
        }
        private string _MaturityComplication;
        public string MaturityComplication
        {
            get { return _MaturityComplication; }
            set
            {
                if (_MaturityComplication != value)
                {
                    _MaturityComplication = value;
                    OnPropertyChanged("MaturityComplication");
                }
            }
        }
        private string _ObstetricRemark;
        public string ObstetricRemark
        {
            get { return _ObstetricRemark; }
            set
            {
                if (_ObstetricRemark != value)
                {
                    _ObstetricRemark = value;
                    OnPropertyChanged("ObstetricRemark");
                }
            }
        }
        private bool _Isfreezed;
        public bool Isfreezed
        {
            get { return _Isfreezed; }
            set
            {
                if (_Isfreezed != value)
                {
                    _Isfreezed = value;
                    OnPropertyChanged("Isfreezed");
                }
            }
        }
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
        #endregion
    }

    public class clsANCSuggestionVO : IValueObject, INotifyPropertyChanged
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
        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
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
        private long _ANCID;
        public long ANCID
        {
            get { return _ANCID; }
            set
            {
                if (_ANCID != value)
                {
                    _ANCID = value;
                    OnPropertyChanged("ANCID");
                }
            }
        }
        private long _CoupleID;
        public long CoupleID
        {
            get { return _CoupleID; }
            set
            {
                if (_CoupleID != value)
                {
                    _CoupleID = value;
                    OnPropertyChanged("CoupleID");
                }
            }
        }

        private long _CoupleUnitID;
        public long CoupleUnitID
        {
            get { return _CoupleUnitID; }
            set
            {
                if (_CoupleUnitID != value)
                {
                    _CoupleUnitID = value;
                    OnPropertyChanged("CoupleUnitID");
                }
            }
        }


        private bool _Smoking;
        public bool Smoking
        {
            get { return _Smoking; }
            set
            {
                if (_Smoking != value)
                {
                    _Smoking = value;
                    OnPropertyChanged("Smoking");
                }
            }
        }
        private bool _Alcoholic;
        public bool Alcoholic
        {
            get { return _Alcoholic; }
            set
            {
                if (_Alcoholic != value)
                {
                    _Alcoholic = value;
                    OnPropertyChanged("Alcoholic");
                }
            }
        }
        private bool _Medication;
        public bool Medication
        {
            get { return _Medication; }
            set
            {
                if (_Medication != value)
                {
                    _Medication = value;
                    OnPropertyChanged("Medication");
                }
            }
        }
        private bool _Exercise;
        public bool Exercise
        {
            get { return _Exercise; }
            set
            {
                if (_Exercise != value)
                {
                    _Exercise = value;
                    OnPropertyChanged("Exercise");
                }
            }
        }
        private bool _Xray;
        public bool Xray
        {
            get { return _Xray; }
            set
            {
                if (_Xray != value)
                {
                    _Xray = value;
                    OnPropertyChanged("Xray");
                }
            }
        }
        private bool _IrregularDiet;
        public bool IrregularDiet
        {
            get { return _IrregularDiet; }
            set
            {
                if (_IrregularDiet != value)
                {
                    _IrregularDiet = value;
                    OnPropertyChanged("IrregularDiet");
                }
            }
        }
        private bool _Exertion;
        public bool Exertion
        {
            get { return _Exertion; }
            set
            {
                if (_Exertion != value)
                {
                    _Exertion = value;
                    OnPropertyChanged("Exertion");
                }
            }
        }
        private bool _Cplace;
        public bool Cplace
        {
            get { return _Cplace; }
            set
            {
                if (_Cplace != value)
                {
                    _Cplace = value;
                    OnPropertyChanged("Cplace");
                }
            }
        }
        private bool _Tea;
        public bool Tea
        {
            get { return _Tea; }
            set
            {
                if (_Tea != value)
                {
                    _Tea = value;
                    OnPropertyChanged("Tea");
                }
            }
        }
        private bool _HeavyObject;
        public bool HeavyObject
        {
            get { return _HeavyObject; }
            set
            {
                if (_HeavyObject != value)
                {
                    _HeavyObject = value;
                    OnPropertyChanged("HeavyObject");
                }
            }
        }
        private long _Consult;
        public long Consult
        {
            get { return _Consult; }
            set
            {
                if (_Consult != value)
                {
                    _Consult = value;
                    OnPropertyChanged("Consult");
                }
            }
        }
        private bool _Bag;
        public bool Bag
        {
            get { return _Bag; }
            set
            {
                if (_Bag != value)
                {
                    _Bag = value;
                    OnPropertyChanged("Bag");
                }
            }
        }
        private bool _Sheets;
        public bool Sheets
        {
            get { return _Sheets; }
            set
            {
                if (_Sheets != value)
                {
                    _Sheets = value;
                    OnPropertyChanged("Sheets");
                }
            }
        }
        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set
            {
                if (_Remark != value)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }
        private string _ConsultIDCollection;
        public string ConsultIDCollection
        {
            get { return _ConsultIDCollection; }
            set
            {
                if (_ConsultIDCollection != value)
                {
                    _ConsultIDCollection = value;
                    OnPropertyChanged("ConsultIDCollection");
                }
            }
        }
        private bool _Isfreezed;
        public bool Isfreezed
        {
            get { return _Isfreezed; }
            set
            {
                if (_Isfreezed != value)
                {
                    _Isfreezed = value;
                    OnPropertyChanged("Isfreezed");
                }
            }
        }
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


        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set
            {
                if (_IsClosed != value)
                {
                    _IsClosed = value;
                    OnPropertyChanged("IsClosed");
                }
            }
        }
        #endregion
    }

    public class clsANCUSGVO : IValueObject, INotifyPropertyChanged
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
        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
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
        private long _ANCID;
        public long ANCID
        {
            get { return _ANCID; }
            set
            {
                if (_ANCID != value)
                {
                    _ANCID = value;
                    OnPropertyChanged("ANCID");
                }
            }
        }
        private DateTime? _USGDate = null;
        public DateTime? USGDate
        {
            get { return _USGDate; }
            set
            {
                if (_USGDate != value)
                {
                    _USGDate = value;
                    OnPropertyChanged("USGDate");
                }
            }
        }
        private double _SLNo;
        public double SLNo
        {
            get { return _SLNo; }
            set
            {
                if (_SLNo != value)
                {
                    _SLNo = value;
                    OnPropertyChanged("SLNo");
                }
            }
        }
        private string _Report;
        public string Report
        {
            get { return _Report; }
            set
            {
                if (_Report != value)
                {
                    _Report = value;
                    OnPropertyChanged("Report");
                }
            }
        }
        private bool _Isfreezed;
        public bool Isfreezed
        {
            get { return _Isfreezed; }
            set
            {
                if (_Isfreezed != value)
                {
                    _Isfreezed = value;
                    OnPropertyChanged("Isfreezed");
                }
            }
        }
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

        private List<clsANCUSGDetailsVO> _ANCUSGDetailsList;
        public List<clsANCUSGDetailsVO> ANCUSGDetailsList
        {
            get
            {
                return _ANCUSGDetailsList;
            }
            set
            {
                _ANCUSGDetailsList = value;
            }
        }
        #endregion
    }

    public class clsANCUSGDetailsVO : IValueObject, INotifyPropertyChanged
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
        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
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
        private long _CoupleID;
        public long CoupleID
        {
            get { return _CoupleID; }
            set
            {
                if (_CoupleID != value)
                {
                    _CoupleID = value;
                    OnPropertyChanged("CoupleID");
                }
            }
        }

        private long _CoupleUnitID;
        public long CoupleUnitID
        {
            get { return _CoupleUnitID; }
            set
            {
                if (_CoupleUnitID != value)
                {
                    _CoupleUnitID = value;
                    OnPropertyChanged("CoupleUnitID");
                }
            }
        }
        private long _ANCID;
        public long ANCID
        {
            get { return _ANCID; }
            set
            {
                if (_ANCID != value)
                {
                    _ANCID = value;
                    OnPropertyChanged("ANCID");
                }
            }
        }
        private long _USGID;
        public long USGID
        {
            get { return _USGID; }
            set
            {
                if (_USGID != value)
                {
                    _USGID = value;
                    OnPropertyChanged("USGID");
                }
            }
        }
        private DateTime? _USGDate = null;
        public DateTime? USGDate
        {
            get { return _USGDate; }
            set
            {
                if (_USGDate != value)
                {
                    _USGDate = value;
                    OnPropertyChanged("USGDate");
                }
            }
        }
        private decimal _SLNo;
        public decimal SLNo
        {
            get { return _SLNo; }
            set
            {
                if (_SLNo != value)
                {
                    _SLNo = value;
                    OnPropertyChanged("SLNo");
                }
            }
        }
        private string _Report;
        public string Report
        {
            get { return _Report; }
            set
            {
                if (_Report != value)
                {
                    _Report = value;
                    OnPropertyChanged("Report");
                }
            }
        }
        private string _PV;
        public string PV
        {
            get { return _PV; }
            set
            {
                if (_PV != value)
                {
                    _PV = value;
                    OnPropertyChanged("PV");
                }
            }
        }

        private bool _Isfreezed;
        public bool Isfreezed
        {
            get { return _Isfreezed; }
            set
            {
                if (_Isfreezed != value)
                {
                    _Isfreezed = value;
                    OnPropertyChanged("Isfreezed");
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


        private string _Sonography;
        public string Sonography
        {
            get { return _Sonography; }
            set
            {
                if (_Sonography != value)
                {
                    _Sonography = value;
                    OnPropertyChanged("Sonography");
                }
            }
        }
        private string _OvulationMonitors;
        public string OvulationMonitors
        {
            get { return _OvulationMonitors; }
            set
            {
                if (_OvulationMonitors != value)
                {
                    _OvulationMonitors = value;
                    OnPropertyChanged("OvulationMonitors");
                }
            }
        }
        private string _Laparoscopy;
        public string Laparoscopy
        {
            get { return _Laparoscopy; }
            set
            {
                if (_Laparoscopy != value)
                {
                    _Laparoscopy = value;
                    OnPropertyChanged("Laparoscopy");
                }
            }
        }
        private string _Mysteroscopy;
        public string Mysteroscopy
        {
            get { return _Mysteroscopy; }
            set
            {
                if (_Mysteroscopy != value)
                {
                    _Mysteroscopy = value;
                    OnPropertyChanged("Mysteroscopy");
                }
            }
        }
        private string _INVTreatment;
        public string INVTreatment
        {
            get { return _INVTreatment; }
            set
            {
                if (_INVTreatment != value)
                {
                    _INVTreatment = value;
                    OnPropertyChanged("INVTreatment");
                }
            }
        }
        private string _txtSIFT;
        public string txtSIFT
        {
            get { return _txtSIFT; }
            set
            {
                if (_txtSIFT != value)
                {
                    _txtSIFT = value;
                    OnPropertyChanged("txtSIFT");
                }
            }
        }
        private string _txtGIFT;
        public string txtGIFT
        {
            get { return _txtGIFT; }
            set
            {
                if (_txtGIFT != value)
                {
                    _txtGIFT = value;
                    OnPropertyChanged("txtGIFT");
                }
            }
        }
        private string _txtIVF;
        public string txtIVF
        {
            get { return _txtIVF; }
            set
            {
                if (_txtIVF != value)
                {
                    _txtIVF = value;
                    OnPropertyChanged("txtIVF");
                }
            }
        }
        private DateTime? _IVF;
        public DateTime? IVF
        {
            get { return _IVF; }
            set
            {
                if (_IVF != value)
                {
                    _IVF = value;
                    OnPropertyChanged("IVF");
                }
            }
        }
        private DateTime? _GIFT;
        public DateTime? GIFT
        {
            get { return _GIFT; }
            set
            {
                if (_GIFT != value)
                {
                    _GIFT = value;
                    OnPropertyChanged("GIFT");
                }
            }
        }
        private DateTime? _SIFT;
        public DateTime? SIFT
        {
            get { return _SIFT; }
            set
            {
                if (_SIFT != value)
                {
                    _SIFT = value;
                    OnPropertyChanged("SIFT");
                }
            }
        }
        #endregion
    }

    public class clsAddANCBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.ANC.clsAddANCBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        private clsANCVO _ANCGeneralDetails = new clsANCVO();
        public clsANCVO ANCGeneralDetails
        {
            get
            {
                return _ANCGeneralDetails;
            }
            set
            {
                _ANCGeneralDetails = value;
            }
        }

        private clsANCDocumentsVO _ANCDocument = new clsANCDocumentsVO();
        public clsANCDocumentsVO ANCDocument
        {
            get
            {
                return _ANCDocument;
            }
            set
            {
                _ANCDocument = value;
            }
        }

        private clsANCExaminationVO _ANCExamination = new clsANCExaminationVO();
        public clsANCExaminationVO ANCExamination
        {
            get
            {
                return _ANCExamination;
            }
            set
            {
                _ANCExamination = value;
            }
        }

        private clsANCHistoryVO _ANCHistory = new clsANCHistoryVO();
        public clsANCHistoryVO ANCHistory
        {
            get
            {
                return _ANCHistory;
            }
            set
            {
                _ANCHistory = value;
            }
        }

        private clsANCInvestigationVO _ANCInvestigation = new clsANCInvestigationVO();
        public clsANCInvestigationVO ANCInvestigation
        {
            get
            {
                return _ANCInvestigation;
            }
            set
            {
                _ANCInvestigation = value;
            }
        }

        private clsANCObstetricHistoryVO _ANCObstetricHistory = new clsANCObstetricHistoryVO();
        public clsANCObstetricHistoryVO ANCObstetricHistory
        {
            get
            {
                return _ANCObstetricHistory;
            }
            set
            {
                _ANCObstetricHistory = value;
            }
        }

        private clsANCSuggestionVO _ANCSuggestion = new clsANCSuggestionVO();
        public clsANCSuggestionVO ANCSuggestion
        {
            get
            {
                return _ANCSuggestion;
            }
            set
            {
                _ANCSuggestion = value;
            }
        }

        private clsANCUSGVO _ANCUSG = new clsANCUSGVO();
        public clsANCUSGVO ANCUSG
        {
            get
            {
                return _ANCUSG;
            }
            set
            {
                _ANCUSG = value;
            }
        }

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long TabID { get; set; }

        public bool IsUpdate { get; set; }

    }
}
