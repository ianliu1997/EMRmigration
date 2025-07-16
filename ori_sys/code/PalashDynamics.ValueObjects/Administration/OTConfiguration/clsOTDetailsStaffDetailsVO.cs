using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    public class clsOTDetailsStaffDetailsVO : IValueObject, INotifyPropertyChanged
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

        private long? _StaffID;
        public long? StaffID
        {
            get
            {
                return _StaffID;
            }
            set
            {
                _StaffID = value;
                OnPropertyChanged("StaffID");
            }
        }
        public string StaffDesc { get; set; }

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
