using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Pathology
{
    public class clsPathoResultEntryPrintDetailsBizActionVO : IBizActionValueObject
    {
        public long ID { get; set; }
        public long UnitID { get; set; }
        public long IsDelivered { get; set; }
        public long? IsOpdIpd { get; set; }
        public string ResultIds { get; set; }
        public long OrderUnitID { get; set; }

        private clsPathoResultEntryPrintDetailsVO objDetails = null;
        public clsPathoResultEntryPrintDetailsVO ResultDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Pathology.clsPathoResultEntryPrintDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsPathoResultEntryPrintDetailsVO : IValueObject, INotifyPropertyChanged
    {
        public String PatientInfoHTML { get; set; }
        public String DoctorInfoHTML { get; set; }

        public string Pathologist1 { get; set; }
        public byte[] Signature1 { get; set; }

        public byte[] UnitLogo { get; set; }
        public byte[] DisclaimerImg { get; set; }

        public string Education1 { get; set; }
        public long PathoDoctorid1 { get; set; }

        public string Pathologist2 { get; set; }
        public byte[] Signature2 { get; set; }
        public string Education2 { get; set; }
        public long PathoDoctorid2 { get; set; }

        public string Pathologist3 { get; set; }
        public byte[] Signature3 { get; set; }
        public string Education3 { get; set; }
        public long PathoDoctorid3 { get; set; }

        public string Pathologist4 { get; set; }
        public byte[] Signature4 { get; set; }
        public string Education4 { get; set; }
        public long PathoDoctorid4 { get; set; }

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

        private string _Salutation;
        public string Salutation
        {
            get { return _Salutation; }
            set
            {
                if (_Salutation != value)
                {
                    _Salutation = value;
                    OnPropertyChanged("Salutation");
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

        private DateTime? _ResultAddedDateTime = DateTime.Now;
        public DateTime? ResultAddedDateTime
        {
            get { return _ResultAddedDateTime; }
            set
            {
                if (_ResultAddedDateTime != value)
                {
                    _ResultAddedDateTime = value;
                    OnPropertyChanged("ResultAddedDateTime");
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

        private long _OrderID;
        public long OrderID
        {
            get { return _OrderID; }
            set
            {
                if (_OrderID != value)
                {
                    _OrderID = value;
                    OnPropertyChanged("OrderID");
                }
            }
        }

        private long _OrderDetailID;
        public long OrderDetailID
        {
            get { return _OrderDetailID; }
            set
            {
                if (_OrderDetailID != value)
                {
                    _OrderDetailID = value;
                    OnPropertyChanged("OrderDetailID");
                }
            }
        }

        private long _PathPatientReportID;
        public long PathPatientReportID
        {
            get { return _PathPatientReportID; }
            set
            {
                if (_PathPatientReportID != value)
                {
                    _PathPatientReportID = value;
                    OnPropertyChanged("PathPatientReportID");
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

        private long _Pathologist;
        public long Pathologist
        {
            get { return _Pathologist; }
            set
            {
                if (_Pathologist != value)
                {
                    _Pathologist = value;
                    OnPropertyChanged("Pathologist");
                }
            }
        }

        private string _TestTemplate;
        public string TestTemplate
        {
            get { return _TestTemplate; }
            set
            {
                if (value != _TestTemplate)
                {
                    _TestTemplate = value;
                    OnPropertyChanged("TestTemplate");
                }
            }
        }

        private string _Template;
        public string Template
        {
            get { return _Template; }
            set
            {
                if (value != _Template)
                {
                    _Template = value;
                    OnPropertyChanged("Template");
                }
            }
        }

        private long _TemplateId;
        public long TemplateId
        {
            get { return _TemplateId; }
            set
            {
                if (_TemplateId != value)
                {
                    _TemplateId = value;
                    OnPropertyChanged("TemplateId");
                }
            }
        }

        private string _Test;
        public string Test
        {
            get { return _Test; }
            set
            {
                if (_Test != value)
                {
                    _Test = value;
                    OnPropertyChanged("Test");
                }
            }
        }

        private string _ReferredBy;
        public string ReferredBy
        {
            get { return _ReferredBy; }
            set
            {
                if (_ReferredBy != value)
                {
                    _ReferredBy = value;
                    OnPropertyChanged("ReferredBy");
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

        private bool _ShowinPathoReport;
        public bool ShowinPathoReport
        {
            get { return _ShowinPathoReport; }
            set
            {
                if (_ShowinPathoReport != value)
                {
                    _ShowinPathoReport = value;
                    OnPropertyChanged("ShowinPathoReport");
                }
            }
        }


        private string _SampleNo;
        public string SampleNo
        {
            get { return _SampleNo; }
            set
            {
                if (_SampleNo != value)
                {
                    _SampleNo = value;
                    OnPropertyChanged("SampleNo");
                }
            }
        }

        private DateTime? _SampleCollectionTime;
        public DateTime? SampleCollectionTime
        {
            get { return _SampleCollectionTime; }
            set
            {
                if (_SampleCollectionTime != value)
                {
                    _SampleCollectionTime = value;
                    OnPropertyChanged("SampleCollectionTime");
                }
            }
        }

        private string _Gender;
        public string Gender
        {
            get { return _Gender; }
            set
            {
                if (_Gender != value)
                {
                    _Gender = value;
                    OnPropertyChanged("Gender");
                }
            }
        }

        private long _AgeYear;
        public long AgeYear
        {
            get { return _AgeYear; }
            set
            {
                if (_AgeYear != value)
                {
                    _AgeYear = value;
                    OnPropertyChanged("AgeYear");
                }
            }
        }

        private long _AgeMonth;
        public long AgeMonth
        {
            get { return _AgeMonth; }
            set
            {
                if (_AgeMonth != value)
                {
                    _AgeMonth = value;
                    OnPropertyChanged("AgeMonth");
                }
            }
        }

        private long _AgeDate;
        public long AgeDate
        {
            get { return _AgeDate; }
            set
            {
                if (_AgeDate != value)
                {
                    _AgeDate = value;
                    OnPropertyChanged("AgeDate");
                }
            }
        }

        private string _PathoCategory;
        public string PathoCategory
        {
            get { return _PathoCategory; }
            set
            {
                if (_PathoCategory != value)
                {
                    _PathoCategory = value;
                    OnPropertyChanged("PathoCategory");
                }
            }
        }
        private string _Roles;
        public string Roles
        {
            get { return _Roles; }
            set
            {
                if (_Roles != value)
                {
                    _Roles = value;
                    OnPropertyChanged("Roles");
                }
            }
        }

        // Added BY CDS On 15/12/2016



        private string _PatientCategory;
        public string PatientCategory
        {
            get { return _PatientCategory; }
            set
            {
                if (_PatientCategory != value)
                {
                    _PatientCategory = value;
                    OnPropertyChanged("PatientCategory");
                }
            }
        }

        private string _PatientSource;
        public string PatientSource
        {
            get { return _PatientSource; }
            set
            {
                if (_PatientSource != value)
                {
                    _PatientSource = value;
                    OnPropertyChanged("PatientSource");
                }
            }
        }

        private string _Company;
        public string Company
        {
            get { return _Company; }
            set
            {
                if (_Company != value)
                {
                    _Company = value;
                    OnPropertyChanged("Company");
                }
            }
        }

        private string _ContactNo;
        public string ContactNo
        {
            get { return _ContactNo; }
            set
            {
                if (_ContactNo != value)
                {
                    _ContactNo = value;
                    OnPropertyChanged("ContactNo");
                }
            }
        }

        private string _DonarCode;
        public string DonarCode
        {
            get { return _DonarCode; }
            set
            {
                if (_DonarCode != value)
                {
                    _DonarCode = value;
                    OnPropertyChanged("DonarCode");
                }
            }
        }




        private string _ReferenceNo;
        public string ReferenceNo
        {
            get { return _ReferenceNo; }
            set
            {
                if (_ReferenceNo != value)
                {
                    _ReferenceNo = value;
                    OnPropertyChanged("ReferenceNo");
                }
            }
        }
        //				

        private DateTime? _ApprovedDateTime;
        public DateTime? ApprovedDateTime
        {
            get { return _ApprovedDateTime; }
            set
            {
                if (_ApprovedDateTime != value)
                {
                    _ApprovedDateTime = value;
                    OnPropertyChanged("ApprovedDateTime");
                }
            }
        }

        private DateTime? _GeneratedDateTime;
        public DateTime? GeneratedDateTime
        {
            get { return _GeneratedDateTime; }
            set
            {
                if (_GeneratedDateTime != value)
                {
                    _GeneratedDateTime = value;
                    OnPropertyChanged("GeneratedDateTime");
                }
            }
        }

        private bool _IsSubOptimal;
        public bool IsSubOptimal
        {
            get { return _IsSubOptimal; }
            set
            {
                if (_IsSubOptimal != value)
                {
                    _IsSubOptimal = value;
                    OnPropertyChanged("IsSubOptimal");
                }
            }
        }

        private string _SubOptimalRemark;
        public string SubOptimalRemark
        {
            get { return _SubOptimalRemark; }
            set
            {
                if (_SubOptimalRemark != value)
                {
                    _SubOptimalRemark = value;
                    OnPropertyChanged("SubOptimalRemark");
                }
            }
        }

        private string _Authorizedby;
        public string Authorizedby
        {
            get { return _Authorizedby; }
            set
            {
                if (_Authorizedby != value)
                {
                    _Authorizedby = value;
                    OnPropertyChanged("Authorizedby");
                }
            }
        }

        private string _Disclaimer;
        public string Disclaimer
        {
            get { return _Disclaimer; }
            set
            {
                if (_Disclaimer != value)
                {
                    _Disclaimer = value;
                    OnPropertyChanged("Disclaimer");
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

        private string _AdressLine1;

        public string AdressLine1
        {
            get { return _AdressLine1; }
            set { _AdressLine1 = value; }
        }

        private string _AddressLine2;

        public string AddressLine2
        {
            get { return _AddressLine2; }
            set { _AddressLine2 = value; }
        }


        private string _AddressLine3;

        public string AddressLine3
        {
            get { return _AddressLine3; }
            set { _AddressLine3 = value; }
        }

        private string _UnitContactNo;

        public string UnitContactNo
        {
            get { return _UnitContactNo; }
            set { _UnitContactNo = value; }
        }

        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private string _PinCode;

        public string PinCode
        {
            get { return _PinCode; }
            set { _PinCode = value; }
        }

        private string _TinNo;

        public string TinNo
        {
            get { return _TinNo; }
            set { _TinNo = value; }
        }

        private string _RegNo;

        public string RegNo
        {
            get { return _RegNo; }
            set { _RegNo = value; }
        }

        private string _City;
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }

        private string _MobileNo;

        public string MobileNo
        {
            get { return _MobileNo; }
            set { _MobileNo = value; }
        }

        //END 

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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


    }


}