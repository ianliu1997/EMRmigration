using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    public class clsProcedureChecklistDetailsVO : IValueObject, INotifyPropertyChanged
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
                OnPropertyChanged("ProcedureID");
            }
        }

        private long _CategoryId;
        public long CategoryId
        {
            get
            {
                return _CategoryId;
            }
            set
            {
                _CategoryId = value;
                OnPropertyChanged("CategoryId");
            }
        }

        private long _SubCategoryId;
        public long SubCategoryId
        {
            get
            {
                return _SubCategoryId;
            }
            set
            {
                _SubCategoryId = value;
                OnPropertyChanged("SubCategoryId");
            }
        }

        private long _ChecklistUnitID;
        public long ChecklistUnitID
        {
            get
            {
                return _ChecklistUnitID;
            }
            set
            {
                _ChecklistUnitID = value;
                OnPropertyChanged("ChecklistUnitID");
            }
        }

        private string _Category;
        public string Category
        {
            get
            {
                return _Category;
            }
            set
            {
                _Category = value;
                OnPropertyChanged("Category");
            }
        }

        private string _Remark;
        public string Remark
        {
            get
            {
                return _Remark;
            }
            set
            {
                _Remark = value;
                OnPropertyChanged("Remark");
            }
        }

        private string _SubCategory;
        public string SubCategory
        {
            get
            {
                return _SubCategory;
            }
            set
            {
                _SubCategory = value;
                OnPropertyChanged("SubCategory");
            }
        }

        private string _SubCategory1;
        public string SubCategory1
        {
            get
            {
                return _SubCategory1;
            }
            set
            {
                _SubCategory1 = value;
                OnPropertyChanged("SubCategory1");
            }
        }

        private string _SubCategory2;
        public string SubCategory2
        {
            get
            {
                return _SubCategory2;
            }
            set
            {
                _SubCategory2 = value;
                OnPropertyChanged("SubCategory2");
            }
        }

        private long _CheckListId;
        public long CheckListId
        {
            get
            {
                return _CheckListId;
            }
            set
            {
                _CheckListId = value;
                OnPropertyChanged("CheckListId");
            }
        }

        private string _Remarks;
        public string Remarks
        {
            get
            {
                return _Remarks;
            }
            set
            {
                _Remarks = value;
                OnPropertyChanged("Remarks");
            }
        }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
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
}
