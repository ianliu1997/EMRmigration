using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.CRM
{
    public class clsLoyaltyProgramVO : IValueObject, INotifyPropertyChanged
    {

        private List<clsLoyaltyProgramFamilyDetails> _LoyaltyProgramDetails;
        public List<clsLoyaltyProgramFamilyDetails> FamilyList
        {
            get
            {
                if (_LoyaltyProgramDetails == null)
                    _LoyaltyProgramDetails = new List<clsLoyaltyProgramFamilyDetails>();

                return _LoyaltyProgramDetails;
            }

            set
            {

                _LoyaltyProgramDetails = value;

            }
        }

        private List<clsLoyaltyClinicVO> _ClinicList;
        public List<clsLoyaltyClinicVO> ClinicList
        {
            get
            {
                if (_ClinicList == null)
                    _ClinicList = new List<clsLoyaltyClinicVO>();

                return _ClinicList;
            }

            set
            {

                _ClinicList = value;

            }
        }


        private List<clsLoyaltyProgramPatientCategoryVO> _CategoryList;
        public List<clsLoyaltyProgramPatientCategoryVO> CategoryList
        {
            get
            {
                if (_CategoryList == null)
                    _CategoryList = new List<clsLoyaltyProgramPatientCategoryVO>();

                return _CategoryList;
            }

            set
            {

                _CategoryList = value;

            }
        }

        private List<clsLoyaltyAttachmentVO> _AttachmentList;
        public List<clsLoyaltyAttachmentVO> AttachmentList
        {
            get
            {
                if (_AttachmentList == null)
                    _AttachmentList = new List<clsLoyaltyAttachmentVO>();

                return _AttachmentList;
            }

            set
            {

                _AttachmentList = value;

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


        private string _LoyaltyProgramName ="";
        public string LoyaltyProgramName
        {
            get { return _LoyaltyProgramName; }
            set
            {
                if (_LoyaltyProgramName != value)
                {
                    _LoyaltyProgramName = value;
                    OnPropertyChanged("LoyaltyProgramName");
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

        private long _PatientCategoryID;
        public long PatientCategoryID
        {
            get { return _PatientCategoryID; }
            set
            {
                if (_PatientCategoryID != value)
                {
                    _PatientCategoryID = value;
                    OnPropertyChanged("PatientCategoryID");
                }
            }
        }

        private string _PatientCategory ="";
        public string PatientCategory
        {
            get { return _PatientCategory; }
            set
            {
                if (_PatientCategory != value)
                {
                    _PatientCategory = value;
                    OnPropertyChanged("PatientCategory");
                }
            }
        }

        private long _TariffID;
        public long TariffID
        {
            get { return _TariffID; }
            set
            {
                if (_TariffID != value)
                {
                    _TariffID = value;
                    OnPropertyChanged("TariffID");
                }
            }
        }


        private long _MemberTariffID;
        public long MemberTariffID
        {
            get { return _MemberTariffID; }
            set
            {
                if (_MemberTariffID != value)
                {
                    _MemberTariffID = value;
                    OnPropertyChanged("MemberTariffID");
                }
            }
        }

        private long _MemberRelationID;
        public long MemberRelationID
        {
            get { return _MemberRelationID; }
            set
            {
                if (_MemberRelationID != value)
                {
                    _MemberRelationID = value;
                    OnPropertyChanged("MemberRelationID");
                }
            }
        }

         private string _Tariff ="";
         public string Tariff
         {
             get { return _Tariff; }
             set
             {
                 if (_Tariff != value)
                 {
                     _Tariff = value;
                     OnPropertyChanged("Tariff");
                 }
             }
         }
        

        private string _Remark ;
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

        private bool _IsFamily;
        public bool IsFamily
        {
            get { return _IsFamily; }
            set
            {
                if (_IsFamily != value)
                {
                    _IsFamily = value;
                    OnPropertyChanged("IsFamily");
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
                if (value != _UpdatedUnitID)
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
                if (value != _AddedBy)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }

            }

        }

        private string _AddedOn ="";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (value != _AddedOn)
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
                if (value != _AddedDateTime)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }

        }

        private string _AddedWindowsLoginName ="";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long _UpdatedBy;
        public long UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (value != _UpdatedBy)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }

            }

        }

        private string _UpdatedOn ="";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (value != _UpdatedOn)
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
                if (value != _UpdatedDateTime)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }

        }

        private string _UpdatedWindowsLoginName ="";
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (value != _UpdatedWindowsLoginName)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

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
    }



    public class clsLoyaltyClinicVO : INotifyPropertyChanged, IValueObject
    {
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

        private long _LoyaltyProgramID;
        public long LoyaltyProgramID
        {
            get { return _LoyaltyProgramID; }
            set
            {
                if (_LoyaltyProgramID != value)
                {
                    _LoyaltyProgramID = value;
                    OnPropertyChanged("LoyaltyProgramID");
                }
            }
        }

        private long _LoyaltyUnitID;
        public long LoyaltyUnitID
        {
            get { return _LoyaltyUnitID; }
            set
            {
                if (_LoyaltyUnitID != value)
                {
                    _LoyaltyUnitID = value;
                    OnPropertyChanged("LoyaltyUnitID");
                }
            }
        }

        private string _LoyaltyUnitDescription;
        public string LoyaltyUnitDescription
        {
            get { return _LoyaltyUnitDescription; }
            set
            {
                if (_LoyaltyUnitDescription != value)
                {
                    _LoyaltyUnitDescription = value;
                    OnPropertyChanged("LoyaltyUnitDescription");
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

        private bool _SelectClinic;
        public bool SelectClinic
        {
            get { return _SelectClinic; }
            set
            {
                if (_SelectClinic != value)
                {
                    _SelectClinic = value;
                    OnPropertyChanged("SelectClinic");
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

        #region CommonFileds

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (value != _CreatedUnitID)
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
                if (value != _UpdatedUnitID)
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
                if (value != _AddedBy)
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
                if (value != _AddedOn)
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
                if (value != _AddedDateTime)
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
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long _UpdatedBy;
        public long UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (value != _UpdatedBy)
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
                if (value != _UpdatedOn)
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
                if (value != _UpdatedDateTime)
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
                if (value != _UpdatedWindowsLoginName)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }

        #endregion

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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsLoyaltyProgramPatientCategoryVO : INotifyPropertyChanged, IValueObject
    {
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

        private long _LoyaltyProgramID;
        public long LoyaltyProgramID
        {
            get { return _LoyaltyProgramID; }
            set
            {
                if (_LoyaltyProgramID != value)
                {
                    _LoyaltyProgramID = value;
                    OnPropertyChanged("LoyaltyProgramID");
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

        private long _PatientCategoryID;
        public long PatientCategoryID
        {
            get { return _PatientCategoryID; }
            set
            {
                if (_PatientCategoryID != value)
                {
                    _PatientCategoryID = value;
                    OnPropertyChanged("PatientCategoryID");
                }
            }
        }

        private string _PatientCategory;
        public string PatientCategory
        {
            get { return _PatientCategory; }
            set
            {
                if (_PatientCategory != value)
                {
                    _PatientCategory = value;
                    OnPropertyChanged("PatientCategory");
                }
            }
        }

        private bool _SelectPatientCategory;
        public bool SelectPatientCategory
        {
            get { return _SelectPatientCategory; }
            set
            {
                if (_SelectPatientCategory != value)
                {
                    _SelectPatientCategory = value;
                    OnPropertyChanged("SelectPatientCategory");
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

        #region CommonFileds

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (value != _CreatedUnitID)
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
                if (value != _UpdatedUnitID)
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
                if (value != _AddedBy)
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
                if (value != _AddedOn)
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
                if (value != _AddedDateTime)
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
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long _UpdatedBy;
        public long UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (value != _UpdatedBy)
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
                if (value != _UpdatedOn)
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
                if (value != _UpdatedDateTime)
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
                if (value != _UpdatedWindowsLoginName)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }

        #endregion

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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsLoyaltyProgramFamilyDetails:INotifyPropertyChanged, IValueObject
    {
        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }

            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");

                }
            }
        }
       

        private long _LoyaltyProgramID;
        public long LoyaltyProgramID
        {
            get
            {
                return _LoyaltyProgramID;
            }

            set
            {
                if (_LoyaltyProgramID !=value )
                {
                    _LoyaltyProgramID = value;
                    OnPropertyChanged("LoyaltyProgramID");

                }
            }
            
        }


        private long _RelationID;
        public long RelationID
        {
            get
            {
                return _RelationID;
            }

            set
            {
                if (_RelationID != value)
                {
                    _RelationID = value;
                    OnPropertyChanged("RelationID");

                }
            }
        }

        private string _Relation;
        public string Relation
        {
            get
            {
                return _Relation;
            }

            set
            {
                if (_Relation != value)
                {
                    _Relation = value;
                    OnPropertyChanged("Relation");

                }
            }
        }

        private string _RelationCode ;
        public string RelationCode
        {
            get { return _RelationCode; }
            set
            {
                if (_RelationCode != value)
                {
                    _RelationCode = value;
                    OnPropertyChanged("RelationCode");

                }
            }
        }

        private string _RelationDescription;
        public string RelationDescription
        {
            get { return _RelationDescription; }
            set
            {
                if (value != _RelationDescription)
                {
                    _RelationDescription = value;
                    OnPropertyChanged("RelationDescription");
                }
            }
        }

        private bool _RelationStatus;
        public bool RelationStatus
        {
            get { return _RelationStatus; }
            set
            {
                if (value != _RelationStatus)
                {
                    _RelationStatus = value;
                    OnPropertyChanged("RelationStatus");
                }
            }
        }




        private long  _TariffID;
        public long TariffID
        {
            get
            {
                return _TariffID;
            }

            set
            {
                if (_TariffID != value)
                {
                    _TariffID = value;
                    OnPropertyChanged("TariffID");

                }
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
                if (value != _UnitID)
                {
                    _UnitID = value;

                }
            }
        }

        private string _Tariff;
        public string Tariff
        {
            get
            {
                return _Tariff;
            }

            set
            {
                if (_Tariff != value)
                {
                    _Tariff = value;
                    OnPropertyChanged("Tariff");

                }
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
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");

                }
            }
        }

        private bool _IsDefault;
        public bool IsDefault
        {
            get
            {
                return _IsDefault;
            }

            set
            {
                if (value != _IsDefault)
                {
                    _IsDefault = value;

                }
            }
        }


        #region CommonFileds

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (value != _CreatedUnitID)
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
                if (value != _UpdatedUnitID)
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
                if (value != _AddedBy)
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
                if (value != _AddedOn)
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
                if (value != _AddedDateTime)
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
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long _UpdatedBy;
        public long UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (value != _UpdatedBy)
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
                if (value != _UpdatedOn)
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
                if (value != _UpdatedDateTime)
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
                if (value != _UpdatedWindowsLoginName)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }

        #endregion

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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        
        
        
        
        
        
        
        
        
      
       
       



    }

    public class clsLoyaltyAttachmentVO : INotifyPropertyChanged, IValueObject
    {
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

        private long _LoyaltyProgramID;
        public long LoyaltyProgramID
        {
            get { return _LoyaltyProgramID; }
            set
            {
                if (_LoyaltyProgramID != value)
                {
                    _LoyaltyProgramID = value;
                    OnPropertyChanged("LoyaltyProgramID");
                }
            }
        }



        private string _DocumentName;
        public string DocumentName
        {
            get { return _DocumentName; }
            set
            {
                if (_DocumentName != value)
                {
                    _DocumentName = value;
                    OnPropertyChanged("DocumentName");
                }
            }
        }

        private string _AttachmentFileName;
        public string AttachmentFileName
        {
            get { return _AttachmentFileName; }
            set
            {
                if (_AttachmentFileName != value)
                {
                    _AttachmentFileName = value;
                    OnPropertyChanged("AttachmentFileName");
                }
            }
        }
        public byte[] Attachment { get; set; }
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

        #region CommonFileds

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (value != _CreatedUnitID)
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
                if (value != _UpdatedUnitID)
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
                if (value != _AddedBy)
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
                if (value != _AddedOn)
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
                if (value != _AddedDateTime)
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
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long _UpdatedBy;
        public long UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (value != _UpdatedBy)
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
                if (value != _UpdatedOn)
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
                if (value != _UpdatedDateTime)
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
                if (value != _UpdatedWindowsLoginName)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }

        #endregion

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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }



}
