using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Radiology
{
    public class clsRadMailReportVO : IValueObject, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

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

        private long _Pateint_UnitID;
        public long Pateint_UnitID
        {
            get { return _Pateint_UnitID; }
            set
            {
                if (_Pateint_UnitID != value)
                {
                    _Pateint_UnitID = value;
                    OnPropertyChanged("Pateint_UnitID");
                }
            }
        }

        private long _RBO_ID;
        public long RBO_ID
        {
            get { return _RBO_ID; }
            set
            {
                if (_RBO_ID != value)
                {
                    _RBO_ID = value;
                    OnPropertyChanged("RBO_ID");
                }
            }
        }

        private long _RBO_UnitID;
        public long RBO_UnitID
        {
            get { return _RBO_UnitID; }
            set
            {
                if (_RBO_UnitID != value)
                {
                    _RBO_UnitID = value;
                    OnPropertyChanged("RBO_UnitID");
                }
            }
        }

        private long _RBO_Details_ID;
        public long RBO_Details_ID
        {
            get { return _RBO_Details_ID; }
            set
            {
                if (_RBO_Details_ID != value)
                {
                    _RBO_Details_ID = value;
                    OnPropertyChanged("RBO_Details_ID");
                }
            }
        }

        private long _RBO_Details_UnitID;
        public long RBO_Details_UnitID
        {
            get { return _RBO_Details_UnitID; }
            set
            {
                if (_RBO_Details_UnitID != value)
                {
                    _RBO_Details_UnitID = value;
                    OnPropertyChanged("RBO_Details_UnitID");
                }
            }
        }

        private long _TestID;
        public long TestID
        {
            get { return _TestID; }
            set
            {
                if (_TestID != value)
                {
                    _TestID = value;
                    OnPropertyChanged("TestID");
                }
            }
        }

        private long? _DoctorID;
        public long? DoctorID
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

        private string _DoctorIDList;
        public string DoctorIDList
        {
            get { return _DoctorIDList; }
            set
            {
                if (_DoctorIDList != value)
                {
                    _DoctorIDList = value;
                    OnPropertyChanged("DoctorIDList");
                }
            }
        }

        private string _DoctorEmailList;
        public string DoctorEmailList
        {
            get { return _DoctorEmailList; }
            set
            {
                if (_DoctorEmailList != value)
                {
                    _DoctorEmailList = value;
                    OnPropertyChanged("DoctorEmailList");
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

        private string _OrderNo;
        public string OrderNo
        {
            get { return _OrderNo; }
            set
            {
                if (_OrderNo != value)
                {
                    _OrderNo = value;
                    OnPropertyChanged("OrderNo");
                }

            }
        }

        private string _SourceURL;
        public string SourceURL
        {
            get { return _SourceURL; }
            set
            {
                if (_SourceURL != value)
                {
                    _SourceURL = value;
                    OnPropertyChanged("SourceURL");
                }

            }
        }

        private string _PatientEmailId;
        public string PatientEmailId
        {
            get { return _PatientEmailId; }
            set
            {
                if (_PatientEmailId != value)
                {
                    _PatientEmailId = value;
                    OnPropertyChanged("PatientEmailId");
                }

            }
        }

        private string _DoctorEmailID;
        public string DoctorEmailID
        {
            get { return _DoctorEmailID; }
            set
            {
                if (_DoctorEmailID != value)
                {
                    _DoctorEmailID = value;
                    OnPropertyChanged("DoctorEmailID");
                }

            }
        }

        private byte[] _Report;
        public byte[] Report
        {
            get { return _Report; }
            set
            {
                if (_Report != value)
                {
                    _Report = value;
                    OnPropertyChanged("Report");
                }
            }
        }

        private bool _IsReportSendToPatient;
        public bool IsReportSendToPatient
        {
            get { return _IsReportSendToPatient; }
            set
            {
                if (_IsReportSendToPatient != value)
                {
                    _IsReportSendToPatient = value;
                    OnPropertyChanged("IsReportSendToPatient");
                }
            }
        }

        private bool _IsReportSendToDoctor;
        public bool IsReportSendToDoctor
        {
            get { return _IsReportSendToDoctor; }
            set
            {
                if (_IsReportSendToDoctor != value)
                {
                    _IsReportSendToDoctor = value;
                    OnPropertyChanged("IsReportSendToDoctor");
                }
            }
        }

        private bool _Is_EmailSendBy_ESA;
        public bool Is_EmailSendBy_ESA
        {
            get { return _Is_EmailSendBy_ESA; }
            set
            {
                if (_Is_EmailSendBy_ESA != value)
                {
                    _Is_EmailSendBy_ESA = value;
                    OnPropertyChanged("Is_EmailSendBy_ESA");
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

        private DateTime _AddedDateTime;
        public DateTime AddedDateTime
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

        private long _UpdatedBy_MailSend;
        public long UpdatedBy_MailSend
        {
            get { return _UpdatedBy_MailSend; }
            set
            {
                if (_UpdatedBy_MailSend != value)
                {
                    _UpdatedBy_MailSend = value;
                    OnPropertyChanged("UpdatedBy_MailSend");
                }
            }
        }

        private string _UpdatedOn_MailSend;
        public string UpdatedOn_MailSend
        {
            get { return _UpdatedOn_MailSend; }
            set
            {
                if (_UpdatedOn_MailSend != value)
                {
                    _UpdatedOn_MailSend = value;
                    OnPropertyChanged("UpdatedOn_MailSend");
                }

            }
        }

        private DateTime _UpdatedDateTime_MailSend;
        public DateTime UpdatedDateTime_MailSend
        {
            get { return _UpdatedDateTime_MailSend; }
            set
            {
                if (_UpdatedDateTime_MailSend != value)
                {
                    _UpdatedDateTime_MailSend = value;
                    OnPropertyChanged("UpdatedDateTime_MailSend");
                }
            }
        }
    }
}
