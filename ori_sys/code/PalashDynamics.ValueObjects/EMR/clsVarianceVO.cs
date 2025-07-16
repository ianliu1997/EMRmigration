using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsVarianceVO : IValueObject, INotifyPropertyChanged
    {
        #region Property Declaration Section

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

        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set
            {
                if (_DoctorID != value)
                {
                    _DoctorID = value;
                    OnPropertyChanged("DoctorID");
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

        private DateTime? _Date = DateTime.Now;
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

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get
            {
                return _CurrentDate;
            }
            set
            {
                if (value != _CurrentDate)
                {
                    _CurrentDate = value;
                    OnPropertyChanged("CurrentDate");
                }
            }
        }


        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {
                if (_VisitID != value)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }

        private string _Variance1;
        public string Variance1
        {
            get { return _Variance1; }
            set
            {
                if (_Variance1 != value)
                {
                    _Variance1 = value;
                    OnPropertyChanged("Variance1");
                }
            }
        }

        private string _Variance2;
        public string Variance2
        {
            get { return _Variance2; }
            set
            {
                if (_Variance2 != value)
                {
                    _Variance2 = value;
                    OnPropertyChanged("Variance2");
                }
            }
        }

        private string _Variance3;
        public string Variance3
        {
            get { return _Variance3; }
            set
            {
                if (_Variance3 != value)
                {
                    _Variance3 = value;
                    OnPropertyChanged("Variance3");
                }
            }
        }

        private string _Variance4;
        public string Variance4
        {
            get { return _Variance4; }
            set
            {
                if (_Variance4 != value)
                {
                    _Variance4 = value;
                    OnPropertyChanged("Variance4");
                }
            }
        }

        private string _Variance5;
        public string Variance5
        {
            get { return _Variance5; }
            set
            {
                if (_Variance5 != value)
                {
                    _Variance5 = value;
                    OnPropertyChanged("Variance5");
                }
            }
        }

        private string _Variance6;
        public string Variance6
        {
            get { return _Variance6; }
            set
            {
                if (_Variance6 != value)
                {
                    _Variance6 = value;
                    OnPropertyChanged("Variance6");
                }
            }
        }

        private string _Variance7;
        public string Variance7
        {
            get { return _Variance7; }
            set
            {
                if (_Variance7 != value)
                {
                    _Variance7 = value;
                    OnPropertyChanged("Variance7");
                }
            }
        }

        private string _ListVariance1;
        public string ListVariance1
        {
            get { return _ListVariance1; }
            set
            {
                if (_ListVariance1 != value)
                {
                    _ListVariance1 = value;
                    OnPropertyChanged("ListVariance1");
                }
            }
        }

        private string _ListVariance2;
        public string ListVariance2
        {
            get { return _ListVariance2; }
            set
            {
                if (_ListVariance2 != value)
                {
                    _ListVariance2 = value;
                    OnPropertyChanged("ListVariance2");
                }
            }
        }
        private string _ListVariance3;
        public string ListVariance3
        {
            get { return _ListVariance3; }
            set
            {
                if (_ListVariance3 != value)
                {
                    _ListVariance3 = value;
                    OnPropertyChanged("ListVariance3");
                }
            }
        }

        private string _ListVariance4;
        public string ListVariance4
        {
            get { return _ListVariance4; }
            set
            {
                if (_ListVariance4 != value)
                {
                    _ListVariance4 = value;
                    OnPropertyChanged("ListVariance4");
                }
            }
        }

        private string _ListVariance5;
        public string ListVariance5
        {
            get { return _ListVariance5; }
            set
            {
                if (_ListVariance5 != value)
                {
                    _ListVariance5 = value;
                    OnPropertyChanged("ListVariance5");
                }
            }
        }

        private string _ListVariance6;
        public string ListVariance6
        {
            get { return _ListVariance6; }
            set
            {
                if (_ListVariance6 != value)
                {
                    _ListVariance6 = value;
                    OnPropertyChanged("ListVariance6");
                }
            }
        }

        private string _ListVariance7;
        public string ListVariance7
        {
            get { return _ListVariance7; }
            set
            {
                if (_ListVariance7 != value)
                {
                    _ListVariance7 = value;
                    OnPropertyChanged("ListVariance7");
                }
            }
        }

        //private long _TemplateDataId;
        //public long TemplateDataId
        //{
        //    get { return _TemplateDataId; }
        //    set
        //    {
        //        if (_TemplateDataId != value)
        //        {
        //            _TemplateDataId = value;
        //            OnPropertyChanged("TemplateDataId");
        //        }
        //    }
        //}

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
