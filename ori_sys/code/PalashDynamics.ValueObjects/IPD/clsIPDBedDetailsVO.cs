using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IPD
{
    public class clsIPDBedDetailsVO : IValueObject, INotifyPropertyChanged
    {
        public string ToXml()
        {
            return this.ToString();
        }

        #region Property Declaration

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

        private long _WardID;
        public long WardID
        {
            get { return _WardID; }
            set
            {
                if (_WardID != value)
                {
                    _WardID = value;
                    OnPropertyChanged("WardID");
                }
            }
        }

        private string _WardName;
        public string WardName
        {
            get { return _WardName; }
            set
            {
                if (_WardName != value)
                {
                    _WardName = value;
                    OnPropertyChanged("WardName");
                }
            }
        }

        private string _RoomName;
        public string RoomName
        {
            get { return _RoomName; }
            set
            {
                if (_RoomName != value)
                {
                    _RoomName = value;
                    OnPropertyChanged("RoomName");
                }
            }
        }

        private string _FloorName;
        public string FloorName
        {
            get { return _FloorName; }
            set
            {
                if (_FloorName != value)
                {
                    _FloorName = value;
                    OnPropertyChanged("FloorName");
                }
            }
        }

        private string _ClassName;
        public string ClassName
        {
            get { return _ClassName; }
            set
            {
                if (_ClassName != value)
                {
                    _ClassName = value;
                    OnPropertyChanged("ClassName");
                }
            }
        }

        private decimal _Rate;
        public decimal Rate
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


        private int _MaximumRows = 20;
        public int MaximumRows
        {
            get { return _MaximumRows; }
            set
            {
                if (_MaximumRows != value)
                {
                    _MaximumRows = value;
                    OnPropertyChanged("MaximumRows");
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

    public class clsIPDBedAmmenityVO : IValueObject, INotifyPropertyChanged
    {
        public string ToXml()
        {
            return this.ToString();
        }

        #region Property Declaration

        private long _BedAmmenityID;
        public long BedAmmenityID
        {
            get { return _BedAmmenityID; }
            set
            {
                if (_BedAmmenityID != value)
                {
                    _BedAmmenityID = value;
                    OnPropertyChanged("BedAmmenityID");
                }
            }
        }

        private long _AmmenityDetailsID;
        public long AmmenityDetailsID
        {
            get { return _AmmenityDetailsID; }
            set
            {
                if (_AmmenityDetailsID != value)
                {
                    _AmmenityDetailsID = value;
                    OnPropertyChanged("AmmenityDetailsID");
                }
            }
        }

        private string _Ammenity;
        public string Ammenity
        {
            get { return _Ammenity; }
            set
            {
                if (_Ammenity != value)
                {
                    _Ammenity = value;
                    OnPropertyChanged("Ammenity");
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

    public class clsIPDAdmissionDetailVO : IValueObject, INotifyPropertyChanged
    {
        public string ToXml()
        {
            return this.ToString();
        }

        #region Property Declaration

        private long _AdmissionNo;
        public long AdmissionNo
        {
            get { return _AdmissionNo; }
            set
            {
                if (_AdmissionNo != value)
                {
                    _AdmissionNo = value;
                    OnPropertyChanged("AdmissionNo");
                }
            }
        }

        private string _IPDNo;
        public string IPDNo
        {
            get { return _IPDNo; }
            set
            {
                if (_IPDNo != value)
                {
                    _IPDNo = value;
                    OnPropertyChanged("IPDNo");
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

        private long _GenderID;
        public long GenderID
        {
            get { return _GenderID; }
            set
            {
                if (_GenderID != value)
                {
                    _GenderID = value;
                    OnPropertyChanged("GenderID");
                }
            }
        }

        private byte[] _GenderImage;
        public byte[] GenderImage
        {
            get { return _GenderImage; }
            set
            {
                if (_GenderImage != value)
                {
                    _GenderImage = value;
                    OnPropertyChanged("GenderImage");
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

        private string _Address;
        public string Address
        {
            get { return _Address; }
            set
            {
                if (_Address != value)
                {
                    _Address = value;
                    OnPropertyChanged("Address");
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

        private long _MobileNo;
        public long MobileNo
        {
            get { return _MobileNo; }
            set
            {
                if (_MobileNo != value)
                {
                    _MobileNo = value;
                    OnPropertyChanged("MobileNo");
                }
            }
        }

        private string _ReferingName;
        public string ReferingName
        {
            get { return _ReferingName; }
            set
            {
                if (_ReferingName != value)
                {
                    _ReferingName = value;
                    OnPropertyChanged("ReferingName");
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

        private string _AdmissionTime;
        public string AdmissionTime
        {
            get { return _AdmissionTime; }
            set
            {
                if (_AdmissionTime != value)
                {
                    _AdmissionTime = value;
                    OnPropertyChanged("AdmissionTime");
                }
            }
        }

        private string _AdmissionDate;
        public string AdmissionDate
        {
            get { return _AdmissionDate; }
            set
            {
                if (_AdmissionDate != value)
                {
                    _AdmissionDate = value;
                    OnPropertyChanged("AdmissionDate");
                }
            }
        }

        private DateTime? _Date;
        public DateTime? AdmDate
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    OnPropertyChanged("AdmDate");
                }
            }
        }

        private long _VisitAdmID;
        public long VisitAdmID
        {
            get { return _VisitAdmID; }
            set
            {
                if (_VisitAdmID != value)
                {
                    _VisitAdmID = value;
                    OnPropertyChanged("VisitAdmID");
                }
            }
        }

        public byte[] Photo { get; set; }


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
