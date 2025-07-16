//Created Date:15/Sep/2015
//Created By: Changdeo Sase
//Specification: BizActionVO Package Services Details

using System;
using System.Collections.Generic;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.ValueObjects.Billing
{
    public class clsGetPackageTariffBizActionVO : IBizActionValueObject, INotifyPropertyChanged
    {

        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Billing.clsGetPackageTariffBizAction";
        }
        #endregion

        private long _TariffID;
        public long TariffID
        {
            get { return _TariffID; }
            set
            {
                if (_TariffID != value)
                {
                    _TariffID = value;
                    OnPropertyChanged("TariffID");
                }
            }
        }

        private bool  _isPackageTariff;
        public bool isPackageTariff
        {
            get { return _isPackageTariff; }
            set
            {
                if (_isPackageTariff != value)
                {
                    _isPackageTariff = value;
                    OnPropertyChanged("isPackageTariff");
                }
            }
        }


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion
    }


    public class clsGetPackageDetailsBizActionVO : IBizActionValueObject, INotifyPropertyChanged
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Billing.clsGetPackageDetailsBizAction";
        }

        #endregion

        private List<clsServiceMasterVO> _objAvailedServiceList = null;
        public List<clsServiceMasterVO> AvailedServiceList
        {
            get { return _objAvailedServiceList; }
            set { _objAvailedServiceList = value; }

        }

        private List<clsServiceMasterVO> _objPendingServiceList = null;
        public List<clsServiceMasterVO> PendingServiceList
        {
            get { return _objPendingServiceList; }
            set { _objPendingServiceList = value; }

        }


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

        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }
            }
        }


        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }


        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {
                if (_VisitID != value)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }


        private string _ServiceName;
        public string ServiceName
        {
            get { return _ServiceName; }
            set { _ServiceName = value; }

        }

        private long _Specialization;
        public long Specialization
        {
            get { return _Specialization; }
            set { _Specialization = value; }
        }

        private long _SubSpecialization;
        public long SubSpecialization
        {
            get { return _SubSpecialization; }
            set { _SubSpecialization = value; }
        }
        public long TariffID { get; set; }
        public long PackageID { get; set; }
        public int Age { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string SearchExpression { get; set; }
        public string PackageIDList { get; set; }
        public long SponsorID { get; set; }
        public long SponsorUnitID { get; set; }

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion
    }

    
    public class clsPackageServiceMasterVO :clsServiceMasterVO, IValueObject, INotifyPropertyChanged
    {
        private DateTime? _UsedDate;
        public DateTime? UsedDate
        {
            get { return _UsedDate; }
            set
            {
                if (value != _UsedDate)
                {
                    _UsedDate = value;
                    OnPropertyChanged("UsedDate");
                }
            }
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
    }
}
