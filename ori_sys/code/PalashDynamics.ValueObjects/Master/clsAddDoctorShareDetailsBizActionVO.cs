using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Master.DoctorPayment;

namespace PalashDynamics.ValueObjects.Master
{
     public class clsAddDoctorShareDetailsBizActionVO : IBizActionValueObject, INotifyPropertyChanged
    {
        public bool DontChangeTheExistingDoctor { get; set; }
        public bool ISFORALLDOCTOR { get; set; }
        private clsDoctorShareServicesDetailsVO _DoctorShareDetails;
        public clsDoctorShareServicesDetailsVO DoctorShareDetails
        {
            get { return _DoctorShareDetails; }
            set { _DoctorShareDetails = value; }
        }

        private List<clsDoctorShareServicesDetailsVO> _DoctorShareInfoList;
        public List<clsDoctorShareServicesDetailsVO> DoctorShareInfoList
        {
            get
            {
                if (_DoctorShareInfoList == null)
                    _DoctorShareInfoList = new List<clsDoctorShareServicesDetailsVO>();

                return _DoctorShareInfoList;
            }

            set
            {

                _DoctorShareInfoList = value;

            }
        }

        public bool IsAllDoctorShate { set; get; }
        public bool IsApplyToallDoctorWithAllTariffAndAllModality { set; get; }
        private int _SuccessStatus;
        public bool ISShareModalityWise { get; set; } 
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private long lngUnitId;
        public long UnitID
        {
            get { return lngUnitId; }
            set
            {
                if (value != lngUnitId)
                {
                    lngUnitId = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private string _ServiceId;
        public string ServiceId
        {
            get { return _ServiceId; }
            set
            {
                if (value != _ServiceId)
                {
                    _ServiceId = value;
                    OnPropertyChanged("ServiceId");
                }
            }
        }

        private long _DoctorId;
        public long DoctorId
        {
            get { return _DoctorId; }
            set
            {
                if (value != _DoctorId)
                {
                    _DoctorId = value;
                    OnPropertyChanged("DoctorId");
                }
            }
        }

        private long _DepartmentId;
        public long DepartmentId
        {
            get { return _DepartmentId; }
            set
            {
                if (value != _DepartmentId)
                {
                    _DepartmentId = value;
                    OnPropertyChanged("DepartmentId");
                }
            }
        }

        private long _TariffID;
        public long TariffID
        {
            get { return _TariffID; }
            set
            {
                if (value != _TariffID)
                {
                    _TariffID = value;
                    OnPropertyChanged("TariffID");
                }
            }
        }

        public int TotalRows { get; set; }

        private long _SpecializationID;
        public long SpecializationID
        {
            get { return _SpecializationID; }
            set
            {
                if (value != _SpecializationID)
                {
                    _SpecializationID = value;
                    OnPropertyChanged("SpecializationID");
                }
            }
        }
        public bool IsCompanyForm { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsAddDoctorShareDetailsBizAction";
        }

        #endregion

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
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion
    }

    public class clsUpdateDoctorShareDetailsBizActionVO : IBizActionValueObject, INotifyPropertyChanged
    {
        private clsDoctorShareServicesDetailsVO _DoctorShareDetails;
        public clsDoctorShareServicesDetailsVO DoctorShareDetails
        {
            get { return _DoctorShareDetails; }
            set { _DoctorShareDetails = value; }
        }


        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsUpdateDoctorShareDetailsBizAction";
        }

        #endregion

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
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion
    }

    public class clsGetDoctorShare1DetailsBizActionVO : IBizActionValueObject, INotifyPropertyChanged
    {

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

                }
            }
        }

        private long _ServiceId;
        public long ServiceId
        {
            get { return _ServiceId; }
            set
            {
                if (value != _ServiceId)
                {
                    _ServiceId = value;
                    OnPropertyChanged("ServiceId");
                }
            }
        }

        private bool _FromDoctorShareChildWindow;
        public bool FromDoctorShareChildWindow
        {
            get { return _FromDoctorShareChildWindow; }
            set
            {
                if (value != _FromDoctorShareChildWindow)
                {
                    _FromDoctorShareChildWindow = value;
                    OnPropertyChanged("FromDoctorShareChildWindow");
                }
            }
        }


        private string _ServiceCode;
        public string ServiceCode
        {
            get
            {
                return _ServiceCode;
            }
            set
            {
                _ServiceCode = value;
                OnPropertyChanged("ServiceCode");
            }


        }

        public bool ForAllDoctorShareRecord { get; set; }
        private double _DoctorShareAmount;
        public double DoctorShareAmount
        {
            get
            {
                if (_DoctorSharePercentage > 0)
                    _DoctorShareAmount = ((_ServiceRate * _DoctorSharePercentage) / 100);
                else
                    _DoctorShareAmount = 0;

                _DoctorShareAmount = Math.Round(_DoctorShareAmount);

                return _DoctorShareAmount;
            }
            set
            {
                if (_DoctorShareAmount != value)
                {
                    if (value < 0)
                        value = 0;
                    //if(_ConcessionAmount !=)
                    _DoctorShareAmount = Math.Round(value, 2);
                    if (_DoctorShareAmount > 0)
                        _DoctorSharePercentage = Math.Round(((_DoctorShareAmount * 100) / _ServiceRate), 2);
                    else
                        _DoctorSharePercentage = 0;



                    OnPropertyChanged("DoctorSharePercentage");
                    OnPropertyChanged("DoctorShareAmount");


                }
            }
        }

        private double _DoctorSharePercentage;
        public double DoctorSharePercentage
        {
            get { return _DoctorSharePercentage; }
            set
            {
                if (_DoctorSharePercentage != value)
                {
                    if (value < 0)
                        value = 0;

                    if (value > 100)
                        value = 100;
                    _DoctorSharePercentage = value;


                    OnPropertyChanged("DoctorShareAmount");
                    OnPropertyChanged("DoctorSharePercentage");

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
                    OnPropertyChanged("ServiceName");
                }
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (value != _Description)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        private long _DoctorId;
        public long DoctorId
        {
            get { return _DoctorId; }
            set
            {
                if (value != _DoctorId)
                {
                    _DoctorId = value;
                    OnPropertyChanged("DoctorId");
                }
            }
        }

        private long _ModalityID;
        public long ModalityID
        {
            get { return _ModalityID; }
            set
            {
                if (value != _ModalityID)
                {
                    _ModalityID = value;
                    OnPropertyChanged("ModalityID");
                }
            }
        }


        private string strDoctorName = "";
        public string DoctorName
        {
            get { return strDoctorName = strFirstName + " " + strMiddleName + " " + strLastName; }
            set
            {
                if (value != strDoctorName)
                {
                    strDoctorName = value;
                    OnPropertyChanged("DoctorName");
                }
            }
        }
        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }

        private string strFirstName;
        public string FirstName
        {
            get { return strFirstName; }
            set
            {
                if (value != strFirstName)
                {
                    strFirstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        private string strMiddleName = "";
        public string MiddleName
        {
            get { return strMiddleName; }
            set
            {
                if (value != strMiddleName)
                {
                    strMiddleName = value;
                    OnPropertyChanged("MiddleName");
                }
            }
        }

        private string strLastName;
        public string LastName
        {
            get { return strLastName; }
            set
            {
                if (value != strLastName)
                {
                    strLastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        private double _ServiceRate;
        public double ServiceRate
        {
            get { return _ServiceRate; }
            set
            {
                if (value != _ServiceRate)
                {
                    _ServiceRate = value;
                    OnPropertyChanged("ServiceRate");
                }
            }
        }

        private long _SpecID;
        public long SpecID
        {
            get { return _SpecID; }
            set 
            {
                if (value != _SpecID)
                {
                    _SpecID = value;
                    OnPropertyChanged("SpecID");
                }
           }
        
        }

        private long _SubSpecID;
        public long SubSpecID
        {
            get { return _SubSpecID; }
            set
            {
                if (value != _SubSpecID)
                {
                    _SubSpecID = value;
                    OnPropertyChanged("SubSpecID");
                }
            }

        }
        private string _SpecializationID;
        public string SpecializationID
        {
            get { return _SpecializationID; }
            set { _SpecializationID = value; }
        
        }
        private string _SubSpecializationID;
        public string SubSpecializationID
        {
            get { return _SubSpecializationID; }
            set { _SubSpecializationID = value; }


        }
        private long _TariffID;
        public long TariffID
        {
            get { return _TariffID; }
            set
            {
                if (value != _TariffID)
                {
                    _TariffID = value;
                    OnPropertyChanged("TariffID");
                }
            }
        }

        private string _TariffName;
        public string TariffName
        {
            get { return _TariffName; }
            set
            {
                if (value != _TariffName)
                {
                    _TariffName = value;
                    OnPropertyChanged("TariffName");
                }
            }
        }

        private long _TariffServiceID;
        public long TariffServiceID
        {
            get { return _TariffServiceID; }
            set
            {
                if (value != _TariffServiceID)
                {
                    _TariffServiceID = value;
                    OnPropertyChanged("TariffServiceID");
                }
            }
        }

        private string _TariffServiceName;
        public string TariffServiceName
        {
            get { return _TariffServiceName; }
            set
            {
                if (value != _TariffServiceName)
                {
                    _TariffServiceName = value;
                    OnPropertyChanged("TariffServiceName");
                }
            }
        }

        private List<clsDoctorShareServicesDetailsVO> _DoctorShareInfoGetList;
        public List<clsDoctorShareServicesDetailsVO> DoctorShareInfoGetList
        {
            get
            {
                if (_DoctorShareInfoGetList == null)
                    _DoctorShareInfoGetList = new List<clsDoctorShareServicesDetailsVO>();

                return _DoctorShareInfoGetList;
            }

            set
            {

                _DoctorShareInfoGetList = value;

            }
        }

        public string DocIds { get; set; }
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetDoctorShare1DetailsBizAction";
        }

        #endregion

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
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion
    }

    public class clsDeleteDoctorShareForOverRideExistingShareVO : IBizActionValueObject, INotifyPropertyChanged
    {
        private clsDoctorShareServicesDetailsVO _ExistingDoctorShareDetails;
        public clsDoctorShareServicesDetailsVO ExistingDoctorShareDetails
        {
            get { return _ExistingDoctorShareDetails; }
            set { _ExistingDoctorShareDetails = value; }
        }

        private List<clsDoctorShareServicesDetailsVO> _ExistingDoctorShareInfoList;
        public List<clsDoctorShareServicesDetailsVO> ExistingDoctorShareInfoList
        {
            get
            {
                if (_ExistingDoctorShareInfoList == null)
                    _ExistingDoctorShareInfoList = new List<clsDoctorShareServicesDetailsVO>();

                return _ExistingDoctorShareInfoList;
            }

            set
            {

                _ExistingDoctorShareInfoList = value;

            }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsDeleteDoctorShareForOverRideExistingShare";
        }

        #endregion

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
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion

    }

    public class clsUpdateDoctorShareBizActionVO : IBizActionValueObject
    {
        private List<clsDoctorPaymentVO> objDoctorShare = null;
        public List<clsDoctorPaymentVO> objDoctorShareList
        {
            get { return objDoctorShare; }
            set { objDoctorShare = value; }
        }

        private clsDoctorPaymentVO objDocShare = null;
        public clsDoctorPaymentVO objDocShareList
        {
            get { return objDocShare; }
            set { objDocShare = value; }
        }



        #region IBizActionValueObject Members

        public string GetBizAction()
        {

            return "PalashDynamics.BusinessLayer.Master.clsUpdateDoctorShareBizAction";
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

