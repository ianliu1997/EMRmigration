using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration.IPD;


namespace PalashDynamics.ValueObjects.IPD
{
    public class clsIPDBedUnderMaintenanceVO : IValueObject, INotifyPropertyChanged
    {

        public string ToXml()
        {
            return this.ToString();
        }

        private List<clsIPDBedUnderMaintenanceVO> _BedUnderMList;
        public List<clsIPDBedUnderMaintenanceVO> BedUnderMList
        {
            get
            {
                return _BedUnderMList;
            }
            set
            {
                if (value != null)
                {
                    _BedUnderMList = value;
                }
            }
        }


        #region Property Declaration

        public DateTime? ExpectedReleasedDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ReleasedDate { get; set; }
        //public DateTime? Released { get; set; }
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


        private long _AdmID;
        public long AdmID
        {
            get { return _AdmID; }
            set
            {
                if (_AdmID != value)
                {
                    _AdmID = value;
                    OnPropertyChanged("AdmID");
                }
            }
        }

        private long _BedUnitID;
        public long BedUnitID
        {
            get { return _BedUnitID; }
            set
            {
                if (_BedUnitID != value)
                {
                    _BedUnitID = value;
                    OnPropertyChanged("BedUnitID");
                }
            }
        }


        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private bool _IsUnderMaintanence;
        public bool IsUnderMaintanence
        {
            get
            {
                return _IsUnderMaintanence;
            }
            set
            {
                _IsUnderMaintanence = value;
                OnPropertyChanged("IsUnderMaintanence");
            }
        }

        private long _AdmUnitID;
        public long AdmUnitID
        {
            get { return _AdmUnitID; }
            set
            {
                if (_AdmUnitID != value)
                {
                    _AdmUnitID = value;
                    OnPropertyChanged("AdmUnitID");
                }
            }
        }

        private string _BedNo;
        public string BedNo
        {
            get { return _BedNo; }
            set
            {
                if (_BedNo != value)
                {
                    _BedNo = value;
                    OnPropertyChanged("BedNo");
                }
            }
        }

        private long _BedID;
        public long BedID
        {
            get { return _BedID; }
            set
            {
                if (_BedID != value)
                {
                    _BedID = value;
                    OnPropertyChanged("BedID");
                }
            }
        }

        private long _WardId;
        public long WardId
        {
            get { return _WardId; }
            set
            {
                if (_WardId != value)
                {
                    _WardId = value;
                    OnPropertyChanged("WardId");
                }
            }
        }
        private string _BedName;
        public string BedName
        {
            get { return _BedName; }
            set
            {
                if (_BedName != value)
                {
                    _BedName = value;
                    OnPropertyChanged("BedName");
                }
            }
        }
        private string _Bed;
        public string Bed
        {
            get { return _Bed; }
            set
            {
                if (_Bed != value)
                {
                    _Bed = value;
                    OnPropertyChanged("Bed");
                }
            }
        }

        private string _Ward;
        public string Ward
        {
            get { return _Ward; }
            set
            {
                if (_Ward != value)
                {
                    _Ward = value;
                    OnPropertyChanged("Ward");
                }
            }
        }

        private string _BedClass;
        public string BedClass
        {
            get { return _BedClass; }
            set
            {
                if (_BedClass != value)
                {
                    _BedClass = value;
                    OnPropertyChanged("BedClass");
                }
            }
        }

        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set
            {
                if (_Remark != value)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }


        private string _ReleaseRemark;
        public string ReleaseRemark
        {
            get { return _ReleaseRemark; }
            set
            {
                if (_ReleaseRemark != value)
                {
                    _ReleaseRemark = value;
                    OnPropertyChanged("ReleaseRemark");
                }
            }
        }

        private bool _IsReleased;
        public bool IsReleased
        {
            get { return _IsReleased; }
            set
            {
                if (_IsReleased != value)
                {
                    _IsReleased = value;
                    OnPropertyChanged("IsReleased");
                }
            }
        }


        private string _MRNo;
        public string MRNo
        {
            get { return _MRNo; }
            set
            {
                if (_MRNo != value)
                {
                    _MRNo = value;
                    OnPropertyChanged("MRNo");
                }
            }
        }

        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public int StartIndex { get; set; }



        #endregion

        #region Common Properties

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





    }
}
