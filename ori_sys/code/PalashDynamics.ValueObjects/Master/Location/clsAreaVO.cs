using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Master.Location
{
    public class clsAreaVO : INotifyPropertyChanged, IValueObject
    {
        public clsAreaVO()
        {

        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        private long _ID;
        public long AreaID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("AreaID");
                }
            }
        }

        private string _Area;
        public string AreaName
        {
            get { return _Area; }
            set
            {
                if (value != _Area)
                {
                    _Area = value;
                    OnPropertyChanged("AreaName");
                }
            }
        }

        private long _CityID;
        public long CityID
        {
            get { return _CityID; }
            set
            {
                if (value != _CityID)
                {
                    _CityID = value;
                    OnPropertyChanged("CityID");
                }
            }
        }

        private string _PinCode;
        public string PinCode
        {
            get { return _PinCode; }
            set
            {
                if (value != _PinCode)
                {
                    _PinCode = value;
                    OnPropertyChanged("PinCode");
                }
            }
        }

        private bool _SelectedService;
        public bool SelectedService
        {
            get { return _SelectedService; }
            set
            {
                if (value != _SelectedService)
                {
                    _SelectedService = value;
                    OnPropertyChanged("SelectedService");
                }
            }
        }

        private bool _IsSelectedArea = true;
        public bool IsSelectedArea
        {
            get { return _IsSelectedArea; }
            set
            {
                if (value != _IsSelectedArea)
                {
                    _IsSelectedArea = value;
                    OnPropertyChanged("IsSelectedArea");
                }
            }
        }

        private bool _SelectedArea = false;
        public bool SelectedArea
        {
            get { return _SelectedArea; }
            set
            {
                if (value != _SelectedArea)
                {
                    _SelectedArea = value;
                    OnPropertyChanged("SelectedArea");
                }
            }
        }

        #region CommonField

        private bool blnStatus;
        public bool Status
        {
            get { return blnStatus; }
            set
            {
                if (value != blnStatus)
                {
                    blnStatus = value;
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


        private string _AddedOn;
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

        private DateTime? _AddedDateTime = DateTime.Now;
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

        private string _AddedWindowsLoginName;
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


        private string _UpdatedBy;
        public string UpdatedBy
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

        private string _UpdatedOn;
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

        private DateTime? _UpdatedDateTime = DateTime.Now;
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

        private string _UpdatedWindowsLoginName;
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

        public override string ToString()
        {
            String rtnValue = "";

            if (PinCode != null && PinCode != String.Empty)
                rtnValue = AreaName + " [ " + PinCode + " ]";
            else
                rtnValue = AreaName;

            return rtnValue;

           
        }
    }
}
