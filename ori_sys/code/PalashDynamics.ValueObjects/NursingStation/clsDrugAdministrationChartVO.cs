using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.NursingStation.EMR;

namespace PalashDynamics.ValueObjects.NursingStation
{
    public class clsDrugAdministrationChartVO : IValueObject, INotifyPropertyChanged
    {
        public string ToXml()
        {
            return this.ToString();
        }

        //private List<clsPrescriptionMasterVO> _PrescriptionMasterList = null;
        //public List<clsPrescriptionMasterVO> PrescriptionMasterList
        //{
        //    get { return _PrescriptionMasterList; }
        //    set { _PrescriptionMasterList = value; }
        //}

        //private List<clsPrescriptionDetailsVO> _DrugList = null;
        //public List<clsPrescriptionDetailsVO> DrugList
        //{
        //    get { return _DrugList; }
        //    set { _DrugList = value; }
        //}

        //private List<clsFeedingDetailsVO> _FeedingDetails = null;
        //public List<clsFeedingDetailsVO> FeedingDetailsList
        //{
        //    get { return _FeedingDetails; }
        //    set { _FeedingDetails = value; }
        //}

        private long _AdmissionID;
        public long AdmissionID
        {
            get { return _AdmissionID; }
            set
            {
                if (value != _AdmissionID)
                {
                    _AdmissionID = value;
                    OnPropertyChanged("AdmissionID");
                }
            }
        }

        private long _AdmissionUnitID;
        public long AdmissionUnitID
        {
            get { return _AdmissionUnitID; }
            set
            {
                if(value != _AdmissionUnitID)
                {
                    _AdmissionUnitID = value;
                    OnPropertyChanged("AdmissionUnitID");
                }
            }
        }

        private long _TotalRows;
        public long TotalRows
        {
            get { return _TotalRows; }
            set
            {
                if (value != _TotalRows)
                {
                    _TotalRows = value;
                    OnPropertyChanged("TotalRows");
                }
            }
        }
        
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

    public class clsFeedingDetailsVO : IValueObject, INotifyPropertyChanged
    {
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

        public string ToXml()
        {
            return this.ToString();
        }

        
        private string _DrugName;
        public string DrugName
        {
            get { return _DrugName; }
            set
            {
                if (value != _DrugName)
                {
                    _DrugName = value;
                    OnPropertyChanged("DrugName");
                }
            }
        }

        private DateTime _Date = DateTime.Now;
        public DateTime Date
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

        private string _Time;
        public string Time
        {
            get { return _Time; }
            set
            {
                if (_Time != value)
                {
                    _Time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        private long _Quantity;
        public long Quantity
        {
            get { return _Quantity; }
            set
            {
                if (value != _Quantity)
                {
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }

        private long _DepartmentID;
        public long DepartmentID
        {
            get { return _DepartmentID; }
            set
            {
                if (_DepartmentID != value)
                {
                    _DepartmentID = value;
                    OnPropertyChanged("DepartmentID");
                }
            }
        }

        private long _StaffID;
        public long StaffID
        {
            get { return _StaffID; }
            set
            {
                if (value != _StaffID)
                {
                    _StaffID = value;
                    OnPropertyChanged("StaffID");
                }
            }
        }

        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set
            {
                if (value != _Remark)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }

    }


}
