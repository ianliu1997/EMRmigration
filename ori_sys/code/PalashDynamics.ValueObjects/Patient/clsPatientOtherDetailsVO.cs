using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Patient
{
   public  class clsPatientOtherDetailsVO:IValueObject,INotifyPropertyChanged
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
        private bool? _Question1;
        public bool? Question1
        {
            get { return _Question1; }
            set
            {
                if (_Question1 != value)
                {
                    _Question1 = value;
                    OnPropertyChanged("Question1");
                }
            }
        }

        private bool? _Question2 ;
        public bool? Question2
        {
            get { return _Question2; }
            set
            {
                if (_Question2 != value)
                {
                    _Question2 = value;
                    OnPropertyChanged("Question2");
                }
            }
        }

        private bool? _Question3 ;
        public bool? Question3
        {
            get { return _Question3; }
            set
            {
                if (_Question3 != value)
                {
                    _Question3 = value;
                    OnPropertyChanged("Question3");
                }
            }
        }

        private bool? _Question4;
        public bool? Question4
        {
            get { return _Question4; }
            set
            {
                if (_Question4 != value)
                {
                    _Question4 = value;
                    OnPropertyChanged("Question4");
                }
            }
        }

        private string _Question4Details;
        public string Question4Details
        {
            get { return _Question4Details; }
            set
            {
                if (_Question4Details != value)
                {
                    _Question4Details = value;
                    OnPropertyChanged("Question4Details");
                }
            }
        }

        private bool? _Question5A;
        public bool? Question5A
        {
            get { return _Question5A; }
            set
            {
                if (_Question5A != value)
                {
                    _Question5A = value;
                    OnPropertyChanged("Question5A");
                }
            }
        }

        private bool? _Question5B;
        public bool? Question5B
        {
            get { return _Question5B; }
            set
            {
                if (_Question5B != value)
                {
                    _Question5B = value;
                    OnPropertyChanged("Question5B");
                }
            }
        }

        private bool? _Question5C;
        public bool? Question5C
        {
            get { return _Question5C; }
            set
            {
                if (_Question5C != value)
                {
                    _Question5C = value;
                    OnPropertyChanged("Question5C");
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
