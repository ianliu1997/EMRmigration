using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Radiology
{
    public class clsGetRadTestDetailsBizActionVO : IBizActionValueObject
    {
        private List<clsRadiologyVO> objTemplateList = null;
        public List<clsRadiologyVO> TestList
        {
            get { return objTemplateList; }
            set { objTemplateList = value; }
        }

        public string Description { get; set; }
        public long Category { get; set; }
        public long ServiceID { get; set; }

        public long robID { get; set; }
        public long robUnitID { get; set; }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string SortExpression { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetRadTestDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetRadTemplateAndItemByTestIDBizActionVO : IBizActionValueObject
    {
        private List<clsRadTemplateDetailMasterVO> objTemplateList = null;
        public List<clsRadTemplateDetailMasterVO> TestList
        {
            get { return objTemplateList; }
            set { objTemplateList = value; }
        }

        private List<clsRadItemDetailMasterVO> objItemList = null;
        public List<clsRadItemDetailMasterVO> ItemList
        {
            get { return objItemList; }
            set { objItemList = value; }
        }



        public long TestID { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetRadTemplateAndItemByTestIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }


    public class clsGetRadViewTemplateBizActionVO : IBizActionValueObject
    {
        private clsRadiologyVO objTemplateList = null;
        public clsRadiologyVO Template
        {
            get { return objTemplateList; }
            set { objTemplateList = value; }
        }


        public long TemplateID { get; set; }

        public long RadiologistID { get; set; }

        public long GenderID { get; set; }

        public long ResultID { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetRadViewTemplateBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    //public class clsRadUploadReportBizActionVO : IBizActionValueObject
    //{
    //    #region IBizActionValueObject Members

    //    public string GetBizAction()
    //    {
    //        return "PalashDynamics.BusinessLayer.clsRadUploadReportBizAction";
    //    }

    //    #endregion

    //    #region IValueObject Members

    //    public string ToXml()
    //    {
    //        return this.ToString();
    //    }

    //    #endregion


    //    private int _SuccessStatus;
    //    /// <summary>
    //    /// Output Property.
    //    /// This property states the outcome of BizAction Process.
    //    /// </summary>
    //    public int SuccessStatus
    //    {
    //        get { return _SuccessStatus; }
    //        set { _SuccessStatus = value; }
    //    }

    //    public long UnitID { get; set; }

    //    private clsRadPatientReportVO objRadPatientReportDetail = new clsRadPatientReportVO();
    //    /// <summary>
    //    /// Output Property.
    //    /// This Property Contains OrderBooking List Which is Added.
    //    /// </summary>
    //    public clsRadPatientReportVO UploadReportDetails
    //    {
    //        get { return objRadPatientReportDetail; }
    //        set { objRadPatientReportDetail = value; }
    //    }

    //    public bool IsResultEntry { get; set; }
    //}

    #region For Radiology Additions

    public class clsRadResultEntryPrintDetailsVO : IValueObject, INotifyPropertyChanged
    {

        public String PatientInfoHTML { get; set; }
        public String DoctorInfoHTML { get; set; }
        public String HeaderHTML { get; set; }
        public byte[] HeaderImage { get; set; }
        public byte[] FooterImage { get; set; }
        public string RadioRegNo { get; set; }
        public string UnitName { get; set; }

        public string UnitWebsite { get; set; }
        public string UnitEmail { get; set; }

        public string UnitContact { get; set; }
        public string UnitContact1 { get; set; }
        public string UnitMobileNo { get; set; }

        public string UnitAddress { get; set; }
        public DateTime PrintDate { get; set; }
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
        public string Radiologist1 { get; set; }
        public byte[] Signature1 { get; set; }
        public string Education1 { get; set; }
        public long RadioDoctorid1 { get; set; }
        public string ReportPrepairDate { get; set; } 
        public string Radiologist2 { get; set; }
        public byte[] Signature2 { get; set; }
        public string Education2 { get; set; }
        public long RadioDoctorid2 { get; set; }
        public byte[] AfriLogo { get; set; }
        public string Radiologist3 { get; set; }
        public byte[] Signature3 { get; set; }
        public string Education3 { get; set; }
        public long RadioDoctorid3 { get; set; }

        public string Radiologist4 { get; set; }
        public byte[] Signature4 { get; set; }
        public string Education4 { get; set; }
        public long RadioDoctorid4 { get; set; }

        public long Radiologist { get; set; }

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

        private string _FirstLevelDescription;
        public string FirstLevelDescription
        {
            get { return _FirstLevelDescription; }
            set
            {
                if (_FirstLevelDescription != value)
                {
                    _FirstLevelDescription = value;
                    OnPropertyChanged("FirstLevelDescription");
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

        private string _TestNAme;
        public string TestNAme
        {
            get { return _TestNAme; }
            set
            {
                if (_TestNAme != value)
                {
                    _TestNAme = value;
                    OnPropertyChanged("TestNAme");
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

        private DateTime? _OrderDate;
        public DateTime? OrderDate
        {
            get { return _OrderDate; }
            set
            {
                if (_OrderDate != value)
                {
                    _OrderDate = value;
                    OnPropertyChanged("OrderDate");
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

        private string _AgeYearMonthDays;
        public string AgeYearMonthDays
        {
            get { return _AgeYearMonthDays; }
            set
            {
                if (_AgeYearMonthDays != value)
                {
                    _AgeYearMonthDays = value;
                    OnPropertyChanged("AgeYearMonthDays");
                }
            }
        }
        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private long _Age;
        public long Age
        {
            get { return _Age; }
            set
            {
                if (_Age != value)
                {
                    _Age = value;
                    OnPropertyChanged("Age");
                }
            }
        }

        private string _ReferredDoctor;
        public string ReferredDoctor
        {
            get { return _ReferredDoctor; }
            set
            {
                if (_ReferredDoctor != value)
                {
                    _ReferredDoctor = value;
                    OnPropertyChanged("ReferredDoctor");
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

        private DateTime? _TestDate;
        public DateTime? TestDate
        {
            get { return _TestDate; }
            set
            {
                if (_TestDate != value)
                {
                    _TestDate = value;
                    OnPropertyChanged("TestDate");
                }
            }
        }

        private string _PrintTestName;
        public string PrintTestName
        {
            get { return _PrintTestName; }
            set
            {
                if (_PrintTestName != value)
                {
                    _PrintTestName = value;
                    OnPropertyChanged("PrintTestName");
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

        private long _ResultID;
        public long ResultID
        {
            get { return _ResultID; }
            set
            {
                if (_ResultID != value)
                {
                    _ResultID = value;
                    OnPropertyChanged("ResultID");
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

        private string _RadiologistName;
        public string RadiologistName
        {
            get { return _RadiologistName; }
            set
            {
                if (_RadiologistName != value)
                {
                    _RadiologistName = value;
                    OnPropertyChanged("RadiologistName");
                }
            }
        }
        private string _RadiologistDegree;
        public string RadiologistDegree
        {
            get { return _RadiologistDegree; }
            set
            {
                if (_RadiologistDegree != value)
                {
                    _RadiologistDegree = value;
                    OnPropertyChanged("RadiologistDegree");
                }
            }
        }

        private string _Education;
        public string Education
        {
            get { return _Education; }
            set
            {
                if (_Education != value)
                {
                    _Education = value;
                    OnPropertyChanged("Education");
                }
            }
        }


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

    public class clsRadResultEntryPrintDetailsBizActionVO : IBizActionValueObject
    {
        public long ResultID { get; set; }
        public long UnitID { get; set; }
        public long OPDIPD { get; set; }

        private clsRadResultEntryPrintDetailsVO objDetails = null;
        public clsRadResultEntryPrintDetailsVO ResultDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsRadResultEntryPrintDetailsBizActionBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

    }

    #endregion

}
