using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PalashDynamics.ValueObjects.Administration.IPD
{   
    public class clsIPDGetAdmissionTypeServiceListBizActionVO : IBizActionValueObject
    {
        public bool IsPagingEnabled { get; set; }
        public int StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public string SearchExpression { get; set; }
        public int TotalRows { get; set; }

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    //  OnPropertyChanged("ID");
                }
            }
        }

        public List<clsServiceMasterVO> ServiceList { get; set; }
        public List<clsServiceMasterVO> SelectedServiceList { get; set; }
        
        private long _AdmissionTypeID;
        public long AdmissionTypeID
        {
            get { return _AdmissionTypeID; }
            set
            {
                if (value != _AdmissionTypeID)
                {
                    _AdmissionTypeID = value;
                    //  OnPropertyChanged("ID");
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
                    //OnPropertyChanged("UnitID");
                }
            }
        }

        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set
            {
                if (value != _ServiceID)
                {
                    _ServiceID = value;
                    //OnPropertyChanged("ServiceID");
                }
            }
        }

        private string _ServiceName;
        public string ServiceName
        {
            get { return _ServiceName; }
            set
            {
                if (value != _ServiceName)
                {
                    _ServiceName = value;
                    //OnPropertyChanged("ServiceID");
                }
            }
        }

        private long _SpecializationID;
        public long SpecializationID
        {
            get { return _SpecializationID; }
            set
            {
                if (value != _SpecializationID)
                {
                    _SpecializationID = value;
                    //OnPropertyChanged("ServiceID");
                }
            }
        }

        private long _SubSpecializationID;
        public long SubSpecializationID
        {
            get { return _SubSpecializationID; }
            set
            {
                if (value != _SubSpecializationID)
                {
                    _SubSpecializationID = value;
                    //OnPropertyChanged("ServiceID");
                }
            }
        }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDGetAdmissionTypeServiceListBizAction";
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
