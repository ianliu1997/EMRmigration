using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    public class clsOTDetailsDocDetailsVO : IValueObject, INotifyPropertyChanged
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

        private long _OTDetailsID;
        public long OTDetailsID
        {
            get
            {
                return _OTDetailsID;
            }
            set
            {
                _OTDetailsID = value;
                OnPropertyChanged("OTDetailsID");
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

        private long? _DesignationID;
        public long? DesignationID
        {
            get
            {
                return _DesignationID;
            }
            set
            {
                _DesignationID = value;
                OnPropertyChanged("DesignationID");
            }
        }

        private string _DoctorCode;
        public string DoctorCode
        {
            get
            {
                return _DoctorCode;
            }
            set
            {
                _DoctorCode = value;
                OnPropertyChanged("DoctorCode");
            }
        }

        public string designationDesc { get; set; }
        private long? _DoctorID;
        public long? DoctorID
        {
            get
            {
                return _DoctorID;
            }
            set
            {
                _DoctorID = value;
                OnPropertyChanged("DoctorID");
            }
        }

        private string _ProcedureName;
        public string ProcedureName
        {
            get
            {
                return _ProcedureName;
            }
            set
            {
                _ProcedureName = value;
                OnPropertyChanged("ProcedureName");
            }
        }

        public string docDesc { get; set; }
        private double? _DocFees;
        public double? DocFees
        {
            get
            {
                return _DocFees;
            }
            set
            {
                _DocFees = value;
                OnPropertyChanged("DocFees");
            }
        }

        private long? _ProcedureID;
        public long? ProcedureID
        {
            get
            {
                return _ProcedureID;
            }
            set
            {
                _ProcedureID = value;
                OnPropertyChanged("ProcedureID");
            }
        }

        List<MasterListItem> _ProcedureList = new List<MasterListItem>();

        public List<MasterListItem> ProcedureList
        {
            get
            {
                return _ProcedureList;
            }
            set
            {
                if (value != _ProcedureList)
                {
                    _ProcedureList = value;
                }
            }

        }

        public string _StrStartTime;
        public string StrStartTime
        {
            get
            {
                return _StrStartTime;
            }
            set
            {
                _StrStartTime = value;
            }
        }

        public string _StrEndTime;
        public string StrEndTime
        {
            get
            {
                return _StrEndTime;
            }
            set
            {
                _StrEndTime = value;
            }
        }

        MasterListItem _SelectedProcedure = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedProcedure
        {
            get
            {
                return _SelectedProcedure;
            }
            set
            {
                if (value != _SelectedProcedure)
                {
                    _SelectedProcedure = value;
                    OnPropertyChanged("SelectedProcedure");
                }
            }


        }
    }

     
}
