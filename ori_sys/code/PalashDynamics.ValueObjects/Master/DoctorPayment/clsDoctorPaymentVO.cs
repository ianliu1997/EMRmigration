using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Master.DoctorPayment
{
    public class clsDoctorPaymentVO : IValueObject, INotifyPropertyChanged
    {
        public clsDoctorPaymentVO()
        {

        }
        public clsDoctorPaymentVO(long Id, string Description)
        {
            this.DoctorID = Id;
            this.DoctorName = Description;
        }

        public string SelectedColor { get; set; }

        private System.Windows.Media.Color _Color;
        public System.Windows.Media.Color Color
        {
            get
            {
                return _Color;
            }
            set
            {
                _Color = value;
                SelectedColor = Color.ToString();

            }
        }
        #region IValueObject Members
        public bool IsChecked { get; set; }

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

        #region Propery Declaration

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
        public bool IsPaymentChanged { get; set; }
        public bool IsPaymentDone { get; set; }

        public string BillIDs { get; set; }

        public string BillUnitIDs { get; set; }
        public double TotalBillShareAmount { get; set; }

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

        private bool _IsEnable;
        public bool IsEnable
        {
            get { return _IsEnable; }
            set
            {
                if (_IsEnable != value)
                {
                    _IsEnable = value;
                    OnPropertyChanged("IsEnable");
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

        private string _DepartmentName;
        public string DepartmentName
        {
            get { return _DepartmentName; }
            set
            {
                if (_DepartmentName != value)
                {
                    _DepartmentName = value;
                    OnPropertyChanged("DepartmentName");
                }
            }
        }

        private long _ClassificationID;
        public long ClassificationID
        {
            get { return _ClassificationID; }
            set
            {
                if (_ClassificationID != value)
                {
                    _ClassificationID = value;
                    OnPropertyChanged("ClassificationID");
                }
            }
        }

        private string _ClassificationName;
        public string ClassificationName
        {
            get { return _ClassificationName; }
            set
            {
                if (_ClassificationName != value)
                {
                    _ClassificationName = value;
                    OnPropertyChanged("ClassificationName");
                }
            }
        }

        private long _DoctorTypeID;
        public long DoctorTypeID
        {
            get { return _DoctorTypeID; }
            set
            {
                if (_DoctorTypeID != value)
                {
                    _DoctorTypeID = value;
                    OnPropertyChanged("DoctorTypeID");
                }
            }
        }

        private string _DoctorTypeName;
        public string DoctorTypeName
        {
            get { return _DoctorTypeName; }
            set
            {
                if (_DoctorTypeName != value)
                {
                    _DoctorTypeName = value;
                    OnPropertyChanged("DoctorTypeName");
                }
            }
        }


        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set
            {
                if (_DoctorID != value)
                {
                    _DoctorID = value;
                    OnPropertyChanged("DoctorID");
                }
            }
        }

        private long _DoctorUnitID;
        public long DoctorUnitID
        {
            get { return _DoctorUnitID; }
            set
            {
                if (_DoctorUnitID != value)
                {
                    _DoctorUnitID = value;
                    OnPropertyChanged("DoctorUnitID");
                }
            }
        }

        private string _DoctorName;
        public string DoctorName
        {
            get { return _DoctorName; }
            set
            {
                if (_DoctorName != value)
                {
                    _DoctorName = value;
                    OnPropertyChanged("DoctorName");
                }
            }
        }

        private string _ServiceName;
        public string ServiceName
        {
            get { return _ServiceName; }
            set
            {
                if (_ServiceName != value)
                {
                    _ServiceName = value;
                    OnPropertyChanged("ServiceName");
                }
            }
        }


        private double _TotalBillAmount;
        public double TotalBillAmount
        {
            get { return _TotalBillAmount; }
            set
            {
                if (_TotalBillAmount != value)
                {
                    _TotalBillAmount = value;
                    OnPropertyChanged("TotalBillAmount");
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


        private double _TotalShareAmount;
        public double TotalShareAmount
        {
            get { return _TotalShareAmount; }
            set
            {
                if (_TotalShareAmount != value)
                {
                    _TotalShareAmount = value;
                    OnPropertyChanged("TotalShareAmount");
                }
            }
        }

        private double _PaymentPercentage;
        public double PaymentPercentage
        {
            get { return _PaymentPercentage; }
            set
            {
                if (_PaymentPercentage != value)
                {
                    _PaymentPercentage = value;
                    OnPropertyChanged("PaymentPercentage");
                }
            }
        }


        private double _PaymentAmount;
        public double PaymentAmount
        {
            get { return _PaymentAmount; }
            set
            {
                if (_PaymentAmount != value)
                {
                    _PaymentAmount = value;
                    OnPropertyChanged("PaymentAmount");
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

        private double _SelfAmount;
        public double SelfAmount
        {
            get { return _SelfAmount; }
            set
            {
                if (_SelfAmount != value)
                {
                    _SelfAmount = value;
                    OnPropertyChanged("SelfAmount");
                }
            }
        }



        private double _TariffName;
        public double TariffName
        {
            get { return _TariffName; }
            set
            {
                if (_TariffName != value)
                {
                    _TariffName = value;
                    OnPropertyChanged("TariffName");
                }
            }
        }

        private double _BalanceAmount;
        public double BalanceAmount
        {
            get { return _BalanceAmount; }
            set
            {
                if (_BalanceAmount != value)
                {
                    _BalanceAmount = value;
                    OnPropertyChanged("BalanceAmount");
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

        private string _PatientName;
        public string PatientName
        {
            get { return _PatientName; }
            set
            {
                if (_PatientName != value)
                {
                    _PatientName = value;
                    OnPropertyChanged("PatientName");
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

        private string _UnitName;
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                if (_UnitName != value)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }

        private string _ScanNo;
        public string ScanNo
        {
            get { return _ScanNo; }
            set
            {
                if (_ScanNo != value)
                {
                    _ScanNo = value;
                    OnPropertyChanged("ScanNo");
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

        private double _DoctorShareAmount;
        public double DoctorShareAmount
        {
            get { return _DoctorShareAmount; }
            set
            {
                if (_DoctorShareAmount != value)
                {
                    _DoctorShareAmount = value;
                    OnPropertyChanged("DoctorShareAmount");
                }
            }
        }

        private string _PaymentNo;
        public string PaymentNo
        {
            get { return _PaymentNo; }
            set
            {
                if (_PaymentNo != value)
                {
                    _PaymentNo = value;
                    OnPropertyChanged("PaymentNo");
                }
            }
        }

        private DateTime? _PaymentDate;
        public DateTime? PaymentDate
        {
            get { return _PaymentDate; }
            set
            {
                if (_PaymentDate != value)
                {
                    _PaymentDate = value;
                    OnPropertyChanged("PaymentDate");
                }
            }
        }


        private DateTime? _FromDate;
        public DateTime? FromDate
        {
            get { return _FromDate; }
            set
            {
                if (_FromDate != value)
                {
                    _FromDate = value;
                    OnPropertyChanged("FromDate");
                }
            }
        }


        private DateTime? _ToDate;
        public DateTime? ToDate
        {
            get { return _ToDate; }
            set
            {
                if (_ToDate != value)
                {
                    _ToDate = value;
                    OnPropertyChanged("ToDate");
                }
            }
        }




        private DateTime _VisitDate;
        public DateTime VisitDate
        {
            get { return _VisitDate; }
            set
            {
                if (_VisitDate != value)
                {
                    _VisitDate = value;
                    OnPropertyChanged("VisitDate");
                }
            }
        }

        private long _BillID;
        public long BillID
        {
            get { return _BillID; }
            set
            {
                if (_BillID != value)
                {
                    _BillID = value;
                    OnPropertyChanged("BillID");
                }
            }
        }

        private long _BillUnitID;
        public long BillUnitID
        {
            get { return _BillUnitID; }
            set
            {
                if (_BillUnitID != value)
                {
                    _BillUnitID = value;
                    OnPropertyChanged("BillUnitID");
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
                    _DoctorSharePercentage = value;
                    OnPropertyChanged("DoctorSharePercentage");
                }
            }
        }

        private long _DoctorPaymentID;
        public long DoctorPaymentID
        {
            get { return _DoctorPaymentID; }
            set
            {
                if (_DoctorPaymentID != value)
                {
                    _DoctorPaymentID = value;
                    OnPropertyChanged("DoctorPaymentID");
                }
            }
        }

        private double _DoctorPaymentUnitID;
        public double DoctorPaymentUnitID
        {
            get { return _DoctorPaymentUnitID; }
            set
            {
                if (_DoctorPaymentUnitID != value)
                {
                    _DoctorPaymentUnitID = value;
                    OnPropertyChanged("DoctorPaymentUnitID");
                }
            }
        }

        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set
            {
                if (_ServiceID != value)
                {
                    _ServiceID = value;
                    OnPropertyChanged("ServiceID");
                }
            }
        }

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


        private long _TariffServiceID;
        public long TariffServiceID
        {
            get { return _TariffServiceID; }
            set
            {
                if (_TariffServiceID != value)
                {
                    _TariffServiceID = value;
                    OnPropertyChanged("TariffServiceID");
                }
            }
        }



        private bool _IsOnBill = true;
        public bool IsOnBill
        {
            get { return _IsOnBill; }
            set
            {
                if (_IsOnBill != value)
                {
                    _IsOnBill = value;
                    OnPropertyChanged("IsOnBill");
                }
            }
        }

        private bool _IsFix = true;
        public bool IsFix
        {
            get { return _IsFix; }
            set
            {
                if (_IsFix != value)
                {
                    _IsFix = value;
                    OnPropertyChanged("IsFix");
                }
            }
        }

        private double _Rate;
        public double Rate
        {
            get { return _Rate; }
            set
            {
                if (_Rate != value)
                {
                    _Rate = value;
                    OnPropertyChanged("Rate");
                }
            }
        }


        private double _DoctorPayAmount;
        public double DoctorPayAmount
        {
            get { return _DoctorPayAmount; }
            set
            {
                if (_DoctorPayAmount != value)
                {
                    _DoctorPayAmount = value;
                    OnPropertyChanged("DoctorPayAmount");
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


        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }


        private long _UpdatedUnitID;
        public long UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
                }
            }
        }


        private long _AddedBy;
        public long AddedBy
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

        private DateTime? _AddedDateTime;
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

        private string _AddedWindowsLoginName;
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

        private DateTime? _UpdatedDateTime;
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

        private string _UpdatedWindowsLoginName;
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

        private long _TotalRows;
        public long TotalRows
        {
            get { return _TotalRows; }
            set
            {
                if (_TotalRows != value)
                {
                    _TotalRows = value;
                    OnPropertyChanged("TotalRows");
                }
            }
        }

        #endregion

        public override string ToString()
        {
            return DoctorName;
        }
    }
}
