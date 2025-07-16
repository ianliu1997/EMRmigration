using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace PalashDynamics.ValueObjects.EMR
{
    public class clsEMRTemplateVO : IValueObject, INotifyPropertyChanged
    {

        #region Property Declaration Section

        private long _TemplateID;
        public long TemplateID
        {
            get { return _TemplateID; }
            set
            {
                if (_TemplateID != value)
                {
                    _TemplateID = value;
                    OnPropertyChanged("TemplateID");
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


        // by rohini dated 15.1.2016
        private string _TemplateType;
        public string TemplateType
        {
            get { return _TemplateType; }
            set
            {
                if (_TemplateType != value)
                {
                    _TemplateType = value;
                    OnPropertyChanged("TemplateType");
                }
            }
        }
        private long _TemplateTypeID;
        public long TemplateTypeID
        {
            get { return _TemplateTypeID; }
            set
            {
                if (_TemplateTypeID != value)
                {
                    _TemplateTypeID = value;
                    OnPropertyChanged("TemplateTypeID");
                }
            }
        }
        private long _TemplateSubtypeID;
        public long TemplateSubtypeID
        {
            get { return _TemplateSubtypeID; }
            set
            {
                if (_TemplateSubtypeID != value)
                {
                    _TemplateSubtypeID = value;
                    OnPropertyChanged("TemplateSubtypeID");
                }
            }
        }
        private string _TemplateSubtype;
        public string TemplateSubtype
        {
            get { return _TemplateSubtype; }
            set
            {
                if (_TemplateSubtype != value)
                {
                    _TemplateSubtype = value;
                    OnPropertyChanged("TemplateSubtype");
                }
            }
        }
        //----------

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

        private Boolean _IsPhysicalExam;
        public Boolean IsPhysicalExam
        {
            get { return _IsPhysicalExam; }
            set
            {
                if (_IsPhysicalExam != value)
                {
                    _IsPhysicalExam = value;
                    OnPropertyChanged("IsPhysicalExam");
                }
            }
        }

        private Boolean _IsForOT;
        public Boolean IsForOT
        {
            get { return _IsForOT; }
            set
            {
                if (_IsForOT != value)
                {
                    _IsForOT = value;
                    OnPropertyChanged("IsForOT");
                }
            }
        }

        private string _Template;
        public string Template
        {
            get { return _Template; }
            set
            {
                if (_Template != value)
                {
                    _Template = value;
                    OnPropertyChanged("Template");
                }
            }
        }

        private Int16 _ApplicableTo;
        public Int16 ApplicableTo
        {
            get { return _ApplicableTo; }
            set
            {
                if (_ApplicableTo != value)
                {
                    _ApplicableTo = value;
                    OnPropertyChanged("ApplicableTo");
                }
            }
        }

        private int _ApplicableCriteria;
        public int ApplicableCriteria
        {
            get { return _ApplicableCriteria; }
            set
            {
                if (_ApplicableCriteria != value)
                {
                    _ApplicableCriteria = value;
                    OnPropertyChanged("ApplicableCriteria");
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

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
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
