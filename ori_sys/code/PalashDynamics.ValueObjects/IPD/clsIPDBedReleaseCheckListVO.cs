using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

/* ADDED BY SUDHIR PATIL ON 28/02/2014 */
namespace PalashDynamics.ValueObjects.Administration.IPD
{
    public class clsIPDBedReleaseCheckListVO : IValueObject, INotifyPropertyChanged
    {
        public string ToXml()
        {
            return this.ToXml();
        }

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


        #region Properties


        public string MasterTableName { get; set; }
        public long Id { get; set; }
        public Boolean IsMandantory { get; set; }
        public string MandantoryStatus { get; set; }
        public string Description { get; set; }

        public string Code { get; set; }
        public long UnitId { get; set; }
        public Boolean Status { get; set; }
        public long? AddUnitID { get; set; }

        public DateTime? DateTime { get; set; }
        public string On { get; set; }

        public long? By { get; set; }

        public string WindowsLoginName { get; set; }


        private bool _SelectBedReleseCheck;
        public bool SelectBedReleseCheck
        {
            get { return _SelectBedReleseCheck; }
            set
            {
                if (value != _SelectBedReleseCheck)
                {
                    _SelectBedReleseCheck = value;
                    OnPropertyChanged("SelectBedReleseCheck");
                }
            }
        }

        public Boolean PrimaryKeyViolationError { get; set; }
        public Boolean GeneralError { get; set; }

        #endregion

        #region Common Properties

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
        #endregion

    }
}
