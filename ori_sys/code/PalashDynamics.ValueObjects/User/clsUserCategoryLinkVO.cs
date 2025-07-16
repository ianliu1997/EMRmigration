using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.User
{
    public class clsUserCategoryLinkVO : IValueObject, INotifyPropertyChanged
    {
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

        #region Properties
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _CategoryType;
        private string _CategoryName;
        private bool _IsSelected;
        private long _CategoryTypeID;
        private long _UserID;
        private long _UserCategoryLinkID;
        private long _CategoryID;
        public string CategoryType
        {
            get { return _CategoryType; }
            set
            {
                if (value != _CategoryType)
                {
                    _CategoryType = value;
                    OnPropertyChanged("CategoryType");
                }
            }
        }

        public string CategoryName
        {
            get { return _CategoryName; }
            set
            {
                if (value != _CategoryName)
                {
                    _CategoryName = value;
                    OnPropertyChanged("CategoryName");
                }
            }
        }

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (value != _IsSelected)
                {
                    _IsSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        public long CategoryTypeID
        {
            get { return _CategoryTypeID; }
            set
            {
                if (value != _CategoryTypeID)
                {
                    _CategoryTypeID = value;
                    OnPropertyChanged("CategoryTypeID");
                }
            }
        }

        public long CategoryID
        {
            get { return _CategoryID; }
            set
            {
                if (_CategoryID != value)
                {
                    _CategoryID = value;
                    OnPropertyChanged("CategoryID");
                }
            }
        }

        public long UserID
        {
            get { return _UserID; }
            set
            {
                if (value != _UserID)
                {
                    _UserID = value;
                    OnPropertyChanged("UserID");
                }
            }
        }

        public long UserCategoryLinkID
        {
            get { return _UserCategoryLinkID; }
            set
            {
                if (value != _UserCategoryLinkID)
                {
                    _UserCategoryLinkID = value;
                    OnPropertyChanged("UserCategoryLinkID");
                }
            }
        }
        #endregion

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



        #endregion
    }
}
