using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    public class clsPatientProcedureVO : IValueObject, INotifyPropertyChanged
    {
        #region IValueObject
        public string ToXml()
        {
            return this.ToString();
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

        public string ProcDesc { get; set; }
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



        private long _ServiceID;
        public long ServiceID
        {
            get
            {
                return _ServiceID;
            }
            set
            {
                _ServiceID = value;
                OnPropertyChanged("ServiceID");
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
        //private long _PatientID;
        //public long PatientID
        //{
        //    get
        //    {
        //        return _PatientID;
        //    }
        //    set
        //    {
        //        _PatientID = value;
        //        OnPropertyChanged("PatientID");
        //    }
        //}

        private long _ProcedureTypeID;
        public long ProcedureTypeID
        {
            get
            {
                return _ProcedureTypeID;
            }
            set
            {
                _ProcedureTypeID = value;
                OnPropertyChanged("ProcedureTypeID");
            }
        }

        private long _AnesthesiaTypeID;
        public long AnesthesiaTypeID
        {
            get
            {
                return _AnesthesiaTypeID;
            }
            set
            {
                _AnesthesiaTypeID = value;
                OnPropertyChanged("AnesthesiaTypeID");
            }
        }


        private long _PatientProcScheduleUnitID;
        public long PatientProcScheduleUnitID
        {
            get
            {
                return _PatientProcScheduleUnitID;
            }
            set
            {
                _PatientProcScheduleUnitID = value;
                OnPropertyChanged("PatientProcScheduleUnitID");
            }
        }

        private long _PatientProcScheduleID;
        public long PatientProcScheduleID
        {
            get
            {
                return _PatientProcScheduleID;
            }
            set
            {
                _PatientProcScheduleID = value;
                OnPropertyChanged("PatientProcScheduleID");
            }
        }
        private long _ProcedureID;
        public long ProcedureID
        {
            get
            {
                return _ProcedureID;
            }
            set
            {
                _ProcedureID = value;
                OnPropertyChanged("PatientProcedureScheduleID");
            }
        }


        private long _ProcedureUnitID;
        public long ProcedureUnitID
        {
            get
            {
                return _ProcedureUnitID;
            }
            set
            {
                _ProcedureUnitID = value;
                OnPropertyChanged("PatientProcedureScheduleUnitID");
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

        private long _Quantity;
        public long Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                _Quantity = value;
                //OnPropertyChanged("Quantity");
                //OnPropertyChanged("TotalAmount");
                //OnPropertyChanged("ConcessionPercent");
                //OnPropertyChanged("ConcessionAmount");
                //OnPropertyChanged("ServiceTaxPercent");
                //OnPropertyChanged("ServiceTaxAmount");
                //OnPropertyChanged("NetAmount");
            }
        }

        private bool _IsEmergency;
        public bool IsEmergency
        {
            get
            {
                return _IsEmergency;
            }
            set
            {
                _IsEmergency = value;
                OnPropertyChanged("IsEmergency");
            }
        }

        private bool _IsHighRisk;
        public bool IsHighRisk
        {
            get
            {
                return _IsHighRisk;
            }
            set
            {
                _IsHighRisk = value;
                OnPropertyChanged("IsHighRisk");
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

    public class clsProcedureCheckListVO : IValueObject, INotifyPropertyChanged
    {
        public clsProcedureCheckListVO()
        { }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        #region Properties
        ///set/get the propertites for ProcedureCheckList Master 

        private long _CategoryId;
        public long CategoryId
        {
            get { return _CategoryId; }
            set
            {
                if (_CategoryId != value)
                {
                    _CategoryId = value;
                    OnPropertyChanged("CategoryId");
                }
            }
        }

        private long _SubCategoryId;
        public long SubCategoryId
        {
            get { return _SubCategoryId; }
            set
            {
                if (_SubCategoryId != value)
                {
                    _SubCategoryId = value;
                    OnPropertyChanged("SubCategoryId");
                }
            }
        }

        private String _Category;
        public String Category
        {
            get { return _Category; }
            set
            {
                if (_Category != value)
                {
                    _Category = value;
                    OnPropertyChanged("Category");
                }
            }

        }

        private String _SubCategory;
        public String SubCategory
        {
            get { return _SubCategory; }
            set
            {
                if (_SubCategory != value)
                {
                    _SubCategory = value;
                    OnPropertyChanged("SubCategory");
                }
            }

        }
        private String _Code;
        public String Code
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

        private String _Description;
        public String Description
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

        private long _UnitId;
        public long UnitId
        {
            get { return _UnitId; }
            set
            {
                if (value != _UnitId)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
                }
            }
        }

        #endregion

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

        #endregion
    }

    public class clsProcedureSubCategoryVO : IValueObject, INotifyPropertyChanged
    {
        public clsProcedureSubCategoryVO()
        { }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        #region Properties
        ///set/get the propertites for ProcedureSubCategory Master 
        ///Author - Kiran Devkar
        ///Added On- 23/10/2012   

        private long _CategoryId;
        public long CategoryId
        {
            get { return _CategoryId; }
            set
            {
                if (_CategoryId != value)
                {
                    _CategoryId = value;
                    OnPropertyChanged("CategoryId");
                }
            }
        }

        private String _Category;
        public String Category
        {
            get { return _Category; }
            set
            {
                if (_Category != value)
                {
                    _Category = value;
                    OnPropertyChanged("Category");
                }
            }

        }

        private String _Code;
        public String Code
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

        private String _Description;
        public String Description
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

        private long _UnitId;
        public long UnitId
        {
            get { return _UnitId; }
            set
            {
                if (value != _UnitId)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
                }
            }
        }

        #endregion

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
                    // OnPropertyChanged("Status");
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

        #endregion

    }

}
