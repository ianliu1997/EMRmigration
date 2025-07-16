using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;

namespace PalashDynamics.ValueObjects.Master.CompanyPayment
{
    public class clsCompanyPaymentDetailsVO : IValueObject, INotifyPropertyChanged
    {

        public clsCompanyPaymentDetailsVO()
        {

        }
        public clsCompanyPaymentDetailsVO(long Id, string Description)
        {
            this.ServiceId = Id;
            this.ServiceName = Description;

        }

        private List<clsCompanyPaymentDetailsVO> _CompanyPaymentInfoList;
        public List<clsCompanyPaymentDetailsVO> CompanyPaymentInfoList
        {
            get
            {
                if (_CompanyPaymentInfoList == null)
                    _CompanyPaymentInfoList = new List<clsCompanyPaymentDetailsVO>();

                return _CompanyPaymentInfoList;
            }

            set
            {

                _CompanyPaymentInfoList = value;

            }
        }

        private clsPaymentVO _PaymentDetails = new clsPaymentVO();
        public clsPaymentVO PaymentDetails
        {
            get { return _PaymentDetails; }
            set
            {

                _PaymentDetails = value;
                OnPropertyChanged("PaymentDetails");

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


        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set
            {
                if (value != _UserName)
                {
                    _UserName = value;
                    OnPropertyChanged("UserName");
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


        private double _TotalAmountClinical;
        public double TotalAmountClinical
        {
            get { return _TotalAmountClinical; }
            set
            {
                if (_TotalAmountClinical != value)
                {
                    _TotalAmountClinical = value;
                    OnPropertyChanged("TotalAmountClinical");
                }
            }
        }



        

        private DateTime _InvoiceDate;
        public DateTime InvoiceDate
        {
            get { return _InvoiceDate; }
            set
            {
                if (_InvoiceDate != value)
                {
                    _InvoiceDate = value;
                    OnPropertyChanged("InvoiceDate");
                }
            }
        }

        private DateTime _BillDate;
        public DateTime BillDate
        {
            get { return _BillDate; }
            set
            {
                if (_BillDate != value)
                {
                    _BillDate = value;
                    OnPropertyChanged("BillDate");
                }
            }
        }
        private string _Date;
        public string Date
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
        private string _BillNo;
        public string BillNo
        {
            get { return _BillNo; }
            set
            {
                if (_BillNo != value)
                {
                    _BillNo = value;
                    OnPropertyChanged("BillNo");
                }
            }
        }

        private long _InvoiceID;
        public long InvoiceID
        {
            get { return _InvoiceID; }
            set
            {
                if (_InvoiceID != value)
                {
                    _InvoiceID = value;
                    OnPropertyChanged("InvoiceID");
                }
            }
        }

        private bool _IsFreezed;
        public bool IsFreezed
        {
            get { return _IsFreezed; }
            set
            {
                if (_IsFreezed != value)
                {
                    _IsFreezed = value;
                    OnPropertyChanged("IsFreezed");
                }
            }
        }

        private string _InvoiceNo;
        public string InvoiceNo
        {
            get { return _InvoiceNo; }
            set
            {
                if (_InvoiceNo != value)
                {
                    _InvoiceNo = value;
                    OnPropertyChanged("InvoiceNo");
                }
            }
        }
        private double _TotalConcessionAmount;
        public double TotalConcessionAmount
        {
            get { return _TotalConcessionAmount; }
            set
            {
                if (_TotalConcessionAmount != value)
                {
                    _TotalConcessionAmount = value;
                    OnPropertyChanged("TotalConcessionAmount");
                }
            }
        }
        private double _PaidAmount;
        public double PaidAmount
        {
            get { return _PaidAmount; }
            set
            {
                if (_PaidAmount != value)
                {
                    _PaidAmount = value;
                    OnPropertyChanged("PaidAmount");
                }
            }
        }

        private double _TotalAmount;
        public double TotalAmount
        {
            get { return _TotalAmount; }
            set
            {
                if (_TotalAmount != value)
                {
                    _TotalAmount = value;
                    OnPropertyChanged("TotalAmount");
                }
            }
        }

        public double TDSAmount { get; set; }

        private double _BalAmt;
        public double BalAmt
        {
            get { return _BalAmt; }
            set
            {
                if (_BalAmt != value)
                {
                    _BalAmt = value;
                    OnPropertyChanged("BalAmt");
                }
            }
        }
        private double _FinalShareAmount;
        public double FinalShareAmount
        {
            get { return _FinalShareAmount; }
            set
            {
                if (_FinalShareAmount != value)
                {
                    _FinalShareAmount = value;
                    OnPropertyChanged("FinalShareAmount");
                }
            }
        }

        private double _PayShareAmount;
        public double PayShareAmount
        {
            get { return _PayShareAmount; }
            set
            {
                if (_PayShareAmount != value)
                {
                    _PayShareAmount = value;
                    OnPropertyChanged("PayShareAmount");
                }
            }
        }

        private double _PaySharePercentage;
        public double PaySharePercentage
        {
            get { return _PaySharePercentage; }
            set
            {
                if (_PaySharePercentage != value)
                {
                    if (value < 0)
                        value = 0;

                    if (value > 100)
                        value = 100;
                    _PaySharePercentage = value;


                    OnPropertyChanged("PayShareAmount");
                    OnPropertyChanged("PaySharePercentage");
                }
            }
        }
        private double _NetBillAmount;
        public double NetBillAmount
        {
            get { return _NetBillAmount; }
            set
            {
                if (_NetBillAmount != value)
                {
                    _NetBillAmount = value;
                    OnPropertyChanged("NetBillAmount");
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

        //Added BY PMG

        private double _BalanceAmountSelf;
        public double BalanceAmountSelf
        {
            get { return _BalanceAmountSelf; }
            set
            {
                if (_BalanceAmountSelf != value)
                {
                    _BalanceAmountSelf = value;
                    OnPropertyChanged("BalanceAmountSelf");
                }
            }
        }

        public bool IsConcessionAthorization { get; set; }


        private double _TempBalanceAmount;
        public double TempBalanceAmount
        {
            get { return _TempBalanceAmount; }
            set
            {
                if (_TempBalanceAmount != value)
                {
                    _TempBalanceAmount = value;
                    OnPropertyChanged("TempBalanceAmount");
                }
            }
        }

        private double _TempPaidAmount;
        public double TempPaidAmount
        {
            get { return _TempPaidAmount; }
            set
            {
                if (_TempPaidAmount != value)
                {
                    _TempPaidAmount = value;
                    OnPropertyChanged("TempPaidAmount");
                }
            }
        }


        private long _Opd_Ipd_External_Id;
        public long Opd_Ipd_External_Id
        {
            get { return _Opd_Ipd_External_Id; }
            set
            {
                if (_Opd_Ipd_External_Id != value)
                {
                    _Opd_Ipd_External_Id = value;
                    OnPropertyChanged("Opd_Ipd_External_Id");
                }
            }
        }

        private long _Opd_Ipd_External_UnitId;
        public long Opd_Ipd_External_UnitId
        {
            get { return _Opd_Ipd_External_UnitId; }
            set
            {
                if (_Opd_Ipd_External_UnitId != value)
                {
                    _Opd_Ipd_External_UnitId = value;
                    OnPropertyChanged("Opd_Ipd_External_UnitId");
                }
            }
        }

        private short _Opd_Ipd_External;
        public short Opd_Ipd_External
        {
            get { return _Opd_Ipd_External; }
            set
            {
                if (_Opd_Ipd_External != value)
                {
                    _Opd_Ipd_External = value;
                    OnPropertyChanged("Opd_Ipd_External");
                }
            }
        }

        public bool IsEnable { get; set; }

        private bool _Selected;
        public bool Selected
        {
            get { return _Selected; }
            set
            {
                if (_Selected != value)
                {
                    _Selected = value;
                    OnPropertyChanged("Selected");
                }
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

        private long _PatientSourceTypeID;
        public long PatientSourceTypeID
        {
            get { return _PatientSourceTypeID; }
            set
            {
                if (value != _PatientSourceTypeID)
                {
                    _PatientSourceTypeID = value;
                    OnPropertyChanged("PatientSourceTypeID");
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

        private string _PatientName;
        public string PatientName
        {
            get { return _PatientName; }
            set
            {
                if (value != _PatientName)
                {
                    _PatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }

        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                if (value != _FirstName)
                {
                    _FirstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }
        private string _MiddleName;
        public string MiddleName
        {
            get { return _MiddleName; }
            set
            {
                if (value != _MiddleName)
                {
                    _MiddleName = value;
                    OnPropertyChanged("MiddleName");
                }
            }
        }
        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set
            {
                if (value != _LastName)
                {
                    _LastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        private string _Prefix;
        public string Prefix
        {
            get { return _Prefix; }
            set
            {
                if (value != _Prefix)
                {
                    _Prefix = value;
                    OnPropertyChanged("Prefix");
                }
            }
        }



        private string strPatientName = string.Empty;
        public string PatientName1
        {
            get { return strPatientName = _Prefix + " " +_FirstName + " " + _MiddleName + " " + _LastName; }
            set
            {
                if (value != strPatientName)
                {
                    strPatientName = value;
                    OnPropertyChanged("PatientName1");
                }
            }
        }

        private string _MRNo;
        public string MRNo
        {
            get { return _MRNo; }
            set
            {
                if (value != _MRNo)
                {
                    _MRNo = value;
                    OnPropertyChanged("MRNo");
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

        //private double _DoctorShareAmount;
        //public double DoctorShareAmount
        //{
        //    get { return _DoctorShareAmount; }
        //    set
        //    {
        //        if (value != _DoctorShareAmount)
        //        {
        //            _DoctorShareAmount = value;
        //            OnPropertyChanged("DoctorShareAmount");

        //        }
        //    }
        //}

        private double _DoctorShareAmount;
        public double DoctorShareAmount
        {
            get
            {
                if (_DoctorSharePercentage > 0)
                    _DoctorShareAmount = ((_ServiceRate * _DoctorSharePercentage) / 100);
                else
                    _DoctorShareAmount = 0;

                _DoctorShareAmount = Math.Round(_DoctorShareAmount, 2);

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

        //private double _DoctorSharePercentage;
        //public double DoctorSharePercentage
        //{
        //    get { return _DoctorSharePercentage; }
        //    set
        //    {
        //        if (value != _DoctorSharePercentage)
        //        {
        //            _DoctorSharePercentage = value;
        //            OnPropertyChanged("DoctorSharePercentage");

        //        }
        //    }
        //}


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
