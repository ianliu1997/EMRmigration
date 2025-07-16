using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsItemLocationVO : INotifyPropertyChanged, IValueObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
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
                    OnPropertyChanged("UnitID");
                }
            }
        }
        private long _ItemID;
        public long ItemID
        {
            get
            {
                return _ItemID;
            }
            set
            {
                if (value != _ItemID)
                {
                    _ItemID = value;
                    OnPropertyChanged("ItemID");
                }
            }
        }
        private long _StoreID;
        public long StoreID
        {
            get
            {
                return _StoreID;
            }
            set
            {
                if (value != _StoreID)
                {
                    _StoreID = value;
                    OnPropertyChanged("StoreID");
                }
            }
        }
        private string _StoreName;
        public string StoreName
        {
            get
            {
                return _StoreName;
            }
            set
            {
                if (value != _StoreName)
                {
                    _StoreName = value;
                    OnPropertyChanged("_StoreName");
                }
            }
        }
        private long _RackID;
        public long RackID
        {
            get
            {
                return _RackID;
            }
            set
            {
                if (value != _RackID)
                {
                    _RackID = value;
                    OnPropertyChanged("RackID");
                }
            }
        }
        private long _ShelfID;
        public long ShelfID
        {
            get
            {
                return _ShelfID;
            }
            set
            {
                if (value != _ShelfID)
                {
                    _ShelfID = value;
                    OnPropertyChanged("ShelfID");
                }
            }
        }
        private long _ContainerID;
        public long ContainerID
        {
            get
            {
                return _ContainerID;
            }
            set
            {
                if (value != _ContainerID)
                {
                    _ContainerID = value;
                    OnPropertyChanged("ContainerID");
                }
            }
        }
        private string _Rackname;
        public string Rackname
        {
            get
            {
                return _Rackname;
            }
            set
            {
                if (value != _Rackname)
                {
                    _Rackname = value;
                    OnPropertyChanged("Rackname");
                }
            }
        }
        private string _Shelfname;
        public string Shelfname
        {
            get
            {
                return _Shelfname;
            }
            set
            {
                if (value != _Shelfname)
                {
                    _Shelfname = value;
                    OnPropertyChanged("Shelfname");
                }
            }
        }
        private string _Containername;
        public string Containername
        {
            get
            {
                return _Containername;
            }
            set
            {
                if (value != _Containername)
                {
                    _Containername = value;
                    OnPropertyChanged("Containername");
                }
            }
        }

        #region Comman Properties
        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get
            {
                return _CreatedUnitID;
            }
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
            get
            {
                return _UpdatedUnitID;
            }
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
            get
            {
                return _AddedBy;
            }
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
            get
            {
                return _AddedOn;
            }
            set
            {
                if (value != _AddedOn)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }
        private DateTime _AddedDateTime;
        public DateTime AddedDateTime
        {
            get
            {
                return _AddedDateTime;
            }
            set
            {
                if (value != _AddedDateTime)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }
        private long _UpdatedBy;
        public long UpdatedBy
        {
            get
            {
                return _UpdatedBy;
            }
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
        public string UpdateddOn
        {
            get
            {
                return _UpdatedOn;
            }
            set
            {
                if (value != _UpdatedOn)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
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
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("_Status");
                }
            }
        }

        private DateTime _UpdatedDateTime;
        public DateTime UpdatedDateTime
        {
            get
            {
                return _UpdatedDateTime;
            }
            set
            {
                if (value != _UpdatedDateTime)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
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
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
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
                if (value != _UpdateWindowsLoginName)
                {
                    _UpdateWindowsLoginName = value;
                    OnPropertyChanged("UpdateWindowsLoginName");
                }
            }
        }
        
        #endregion
    }
}
