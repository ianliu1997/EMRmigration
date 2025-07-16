using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Master
{
    public class clsDoctorShareServicesDetailsVO : IValueObject, INotifyPropertyChanged
    {
        public clsDoctorShareServicesDetailsVO()
        {

        }
        public clsDoctorShareServicesDetailsVO(long Id, string Description)
        {
            this.ServiceId = Id;
            this.ServiceName = Description;

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

        #region Added By Yogita

        private bool _SelectService = false;
        public bool SelectService
        {
            get { return _SelectService; }
            set
            {
                if (value != _SelectService)
                {
                    _SelectService = value;
                    OnPropertyChanged("SelectService");
                }
            }
        }

        private bool _SelectedService = false;
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


        private bool _IsSelectedService = true;
        public bool IsSelectedService
        {
            get { return _IsSelectedService; }
            set
            {
                if (value != _IsSelectedService)
                {
                    _IsSelectedService = value;
                    OnPropertyChanged("IsSelectedService");
                }
            }
        }

        #endregion

        private string _PageName;
        public string PageName
        {
            get { return _PageName; }
            set
            {
                if (value != _PageName)
                {
                    _PageName = value;
                    OnPropertyChanged("PageName");
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

        private Boolean _IsSelected;
        public Boolean IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        private Boolean _IsReadOnly;
        public Boolean IsReadOnly
        {
            get
            {
                return _IsReadOnly;
            }
            set
            {
                _IsReadOnly = value;
                OnPropertyChanged("IsReadOnly");
            }
        }


        private Boolean _IsEnabled;
        public Boolean IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            set
            {
                _IsEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
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

        private string _UnitName;
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                if (value != _UnitName)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }

        private bool _Status = false;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
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

        private string _ShortDescription;
        public string ShortDescription
        {
            get { return _ShortDescription; }
            set
            {
                if (value != _ShortDescription)
                {
                    _ShortDescription = value;
                    OnPropertyChanged("ShortDescription");
                }
            }
        }

        private string _LongDescription;
        public string LongDescription
        {
            get { return _LongDescription; }
            set
            {
                if (value != _LongDescription)
                {
                    _LongDescription = value;
                    OnPropertyChanged("LongDescription");
                }
            }
        }

        private decimal _StaffDiscountAmount;
        public decimal StaffDiscountAmount
        {
            get { return _StaffDiscountAmount; }
            set
            {
                if (value != _StaffDiscountAmount)
                {
                    _StaffDiscountAmount = value;
                    OnPropertyChanged("StaffDiscountAmount");
                }
            }
        }

        private decimal _ServiceTaxAmount;
        public decimal ServiceTaxAmount
        {
            get { return _ServiceTaxAmount; }
            set
            {
                if (value != _ServiceTaxAmount)
                {
                    _ServiceTaxAmount = value;
                    OnPropertyChanged("ServiceTaxAmount");
                }
            }
        }


        private decimal _ServiceTaxPercent;
        public decimal ServiceTaxPercent
        {
            get { return _ServiceTaxPercent; }
            set
            {
                if (value != _ServiceTaxPercent)
                {
                    _ServiceTaxPercent = value;
                    OnPropertyChanged("ServiceTaxPercent");
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

        private string _DoctorName;
        public string DoctorName
        {
            get { return _DoctorName; }
            set
            {
                if (value != _DoctorName)
                {
                    _DoctorName = value;
                    OnPropertyChanged("DoctorName");
                }
            }
        }

        private long _CompanyID;
        public long CompanyID
        {
            get { return _CompanyID; }
            set
            {
                if (value != _CompanyID)
                {
                    _CompanyID = value;
                    OnPropertyChanged("CompanyID");
                }
            }
        }
        private long _AssCompanyID;
        public long AssCompanyID
        {
            get { return _AssCompanyID; }
            set
            {
                if (value != _AssCompanyID)
                {
                    _AssCompanyID = value;
                    OnPropertyChanged("AssCompanyID");
                }
            }
        }

        private string _CompanyName;
        public string CompanyName
        {
            get { return _CompanyName; }
            set
            {
                if (value != _CompanyName)
                {
                    _CompanyName = value;
                    OnPropertyChanged("CompanyName");
                }
            }
        }
        private string _AssCompanyName;
        public string AssCompanyName
        {
            get { return _AssCompanyName; }
            set
            {
                if (value != _AssCompanyName)
                {
                    _AssCompanyName = value;
                    OnPropertyChanged("AssCompanyName");
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

        private string _DepartmentName;
        public string DepartmentName
        {
            get { return _DepartmentName; }
            set
            {
                if (value != _DepartmentName)
                {
                    _DepartmentName = value;
                    OnPropertyChanged("DepartmentName");
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

        private string _Command;
        public string Command
        {
            get { return _Command; }
            set
            {
                if (_Command != value)
                {
                    _Command = value;
                    OnPropertyChanged("Command");
                }
            }
        }

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

        private string _Modality;
        public string Modality
        {
            get { return _Modality; }
            set
            {
                if (value != _Modality)
                {
                    _Modality = value;
                    OnPropertyChanged("Modality");
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

        private long _SubSpecializationId;
        public long SubSpecializationId
        {
            get { return _SubSpecializationId; }
            set { _SubSpecializationId = value; }

        }

        private string _SpecializationName;
        public string SpecializationName
        {
            get { return _SpecializationName; }
            set
            {
                if (value != _SpecializationName)
                {
                    _SpecializationName = value;
                    OnPropertyChanged("SpecializationName");
                }
            }
        }
        private string _SubSpecializationName;
        public string SubSpecializationName
        {
            get { return _SubSpecializationName; }
            set
            {
                if (value != _SubSpecializationName)
                {
                    _SubSpecializationName = value;
                    OnPropertyChanged("SubSpecializationName");
                }
            }
        }


        private long _ClassID;
        public long ClassID
        {
            get { return _ClassID; }
            set
            {
                if (value != _ClassID)
                {
                    _ClassID = value;
                    OnPropertyChanged("ClassID");
                }
            }
        }

        private string _ClassName;
        public string ClassName
        {
            get { return _ClassName; }
            set
            {
                if (value != _ClassName)
                {
                    _ClassName = value;
                    OnPropertyChanged("ClassName");
                }
            }
        }

        private string _ClassifficationID;
        public string ClassifficationID
        {
            get { return _ClassifficationID; }
            set
            {
                if (value != _ClassifficationID)
                {
                    _ClassifficationID = value;
                    OnPropertyChanged("ClassifficationID");
                }
            }
        }

        private string _ClassifficationName;
        public string ClassifficationName
        {
            get { return _ClassifficationName; }
            set
            {
                if (value != _ClassifficationName)
                {
                    _ClassifficationName = value;
                    OnPropertyChanged("ClassifficationName");
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
