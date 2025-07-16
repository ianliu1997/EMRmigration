using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.ValueObjects.IPD
{
    public class clsIPDDischargeSummaryVO : IValueObject, INotifyPropertyChanged
    {
        public string ToXml()
        {
            return this.ToString();
        }

        public List<clsDischargeSummaryDetailsVO> DischargeSummaryDetailsList
        {
            get;
            set;
        }

        public clsDischargeSummaryDetailsVO DischargeSummaryDetails
        {
            get;
            set;
        }

        #region Property Declaration

        public String PatientInfoHTML { get; set; }
        public String PatientFooterInfoHTML { get; set; }

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

        private long _DischargeSummaryID;
        public long DischargeSummaryID
        {
            get { return _DischargeSummaryID; }
            set
            {
                if (_DischargeSummaryID != value)
                {
                    _DischargeSummaryID = value;
                    OnPropertyChanged("DischargeSummaryID");
                }
            }
        }

        private long _AdmID;
        public long AdmID
        {
            get { return _AdmID; }
            set
            {
                if (_AdmID != value)
                {
                    _AdmID = value;
                    OnPropertyChanged("AdmID");
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

        private long _AdmUnitID;
        public long AdmUnitID
        {
            get { return _AdmUnitID; }
            set
            {
                if (_AdmUnitID != value)
                {
                    _AdmUnitID = value;
                    OnPropertyChanged("AdmUnitID");
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

        # region

        private string _IPDNO;
        public string IPDNO
        {
            get { return _IPDNO; }
            set
            {
                if (_IPDNO != value)
                {
                    _IPDNO = value;
                    OnPropertyChanged("IPDNO");
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

        private string _TextDocument;
        public string TextDocument
        {
            get { return _TextDocument; }
            set
            {
                if (_TextDocument != value)
                {
                    _TextDocument = value;
                    OnPropertyChanged("TextDocument");
                }
            }
        }

        private string _BedCategory;
        public string BedCategory
        {
            get { return _BedCategory; }
            set
            {
                if (_BedCategory != value)
                {
                    _BedCategory = value;
                    OnPropertyChanged("BedCategory");
                }
            }
        }

        private string _Bed;
        public string Bed
        {
            get { return _Bed; }
            set
            {
                if (_Bed != value)
                {
                    _Bed = value;
                    OnPropertyChanged("Bed");
                }
            }
        }

        private string _Ward;
        public string Ward
        {
            get { return _Ward; }
            set
            {
                if (_Ward != value)
                {
                    _Ward = value;
                    OnPropertyChanged("Ward");
                }
            }
        }

        private string _Unit;
        public string Unit
        {
            get { return _Unit; }
            set
            {
                if (_Unit != value)
                {
                    _Unit = value;
                    OnPropertyChanged("Unit");
                }
            }
        }

        private string _DisDestination;
        public string DisDestination
        {
            get { return _DisDestination; }
            set
            {
                if (_DisDestination != value)
                {
                    _DisDestination = value;
                    OnPropertyChanged("DisDestination");
                }
            }
        }

        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set
            {
                if (_Remark != value)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }

        private bool _IsCancel;
        public bool IsCancel
        {
            get { return _IsCancel; }
            set
            {
                if (_IsCancel != value)
                {
                    _IsCancel = value;
                    OnPropertyChanged("IsCancel");
                }
            }
        }

        #endregion

        private DateTime? _AdmDate;
        public DateTime? AdmDate
        {
            get { return _AdmDate; }
            set
            {
                if (_AdmDate != value)
                {
                    _AdmDate = value;
                    OnPropertyChanged("AdmDate");
                }
            }
        }

        private DateTime? _Date;
        public DateTime? Date
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

        private DateTime _DischargeDate;
        public DateTime DischargeDate
        {
            get { return _DischargeDate; }
            set
            {
                if (_DischargeDate != value)
                {
                    _DischargeDate = value;
                    OnPropertyChanged("DischargeDate");
                }
            }
        }

        private DateTime _DischargeTime;
        public DateTime DischargeTime
        {
            get { return _DischargeTime; }
            set
            {
                if (_DischargeTime != value)
                {
                    _DischargeTime = value;
                    OnPropertyChanged("DischargeTime");
                }
            }
        }

        private DateTime _FollowUpDate;
        public DateTime FollowUpDate
        {
            get { return _FollowUpDate; }
            set
            {
                if (_FollowUpDate != value)
                {
                    _FollowUpDate = value;
                    OnPropertyChanged("FollowUpDate");
                }
            }
        }

        private long _DischargeDoctor;
        public long DischargeDoctor
        {
            get { return _DischargeDoctor; }
            set
            {
                if (_DischargeDoctor != value)
                {
                    _DischargeDoctor = value;
                    OnPropertyChanged("DischargeDoctor");
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

        private string _TemplateName;
        public string TemplateName
        {
            get { return _TemplateName; }
            set
            {
                if (_TemplateName != value)
                {
                    _TemplateName = value;
                    OnPropertyChanged("TemplateName");
                }
            }
        }

        private long _DischargeTemplateID;
        public long DischargeTemplateID
        {
            get { return _DischargeTemplateID; }
            set
            {
                if (_DischargeTemplateID != value)
                {
                    _DischargeTemplateID = value;
                    OnPropertyChanged("DischargeTemplateID");
                }
            }
        }

        private long _DischargeTemplateUnitID;
        public long DischargeTemplateUnitID
        {
            get { return _DischargeTemplateUnitID; }
            set
            {
                if (_DischargeTemplateUnitID != value)
                {
                    _DischargeTemplateUnitID = value;
                    OnPropertyChanged("DischargeTemplateUnitID");
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

        private string strPatientName = "";
        public string PatientName
        {
            get { return strPatientName = strFirstName + " " + strMiddleName + " " + strLastName; }
            set
            {
                if (value != strPatientName)
                {
                    strPatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }

        private string strFirstName = "";
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

        private string strLastName = "";
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

        private string _Gender = "";
        public string Gender
        {
            get { return _Gender; }
            set
            {
                if (value != _Gender)
                {
                    _Gender = value;
                    OnPropertyChanged("Gender");
                }
            }
        }

        private string _Address = "";
        public string Address
        {
            get { return _Address; }
            set
            {
                if (value != _Address)
                {
                    _Address = value;
                    OnPropertyChanged("Address");
                }

            }
        }


        private int _Age =0;
        public int Age
        {
            get { return _Age; }
            set
            {
                if (value != _Age)
                {
                    _Age = value;
                    OnPropertyChanged("Age");
                }

            }
        }

        private bool? _IsTextTemplate;
        public bool? IsTextTemplate
        {
            get { return _IsTextTemplate; }
            set
            {
                if (_IsTextTemplate != value)
                {
                    _IsTextTemplate = value;
                    OnPropertyChanged("IsTextTemplate");
                }
            }
        }

        //Added by AJ Date 16/11/2016
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

        //Added by Bhushan 09/01/2017

        private string _AgeWithSex;

        public string AgeWithSex
        {
            get { return _AgeWithSex; }
            set
            {
                if (_AgeWithSex != value)
                {
                    _AgeWithSex = value;
                    OnPropertyChanged("AgeWithSex");
                }
            }
        }
        //***//--------------
        #endregion

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

        private string _PinCode;

        public string PinCode
        {
            get { return _PinCode; }
            set { _PinCode = value; }
        }

        private string _TreatingDr;

        public string TreatingDr
        {
            get { return _TreatingDr; }
            set { _TreatingDr = value; }
        }

        private string TreatingEducation;

        public string TreatingEducation1
        {
            get { return TreatingEducation; }
            set { TreatingEducation = value; }
        }

        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private string _ContactNo;

        public string ContactNo
        {
            get { return _ContactNo; }
            set { _ContactNo = value; }
        }

        private string _MobileNo;

        public string MobileNo
        {
            get { return _MobileNo; }
            set { _MobileNo = value; }
        }

        #region Common Properties
        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
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

        private long? _AddedBy;
        public long? AddedBy
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

        private string _AddedOn = "";
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

        private string _AddedWindowsLoginName = "";
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

        private string _UpdatedOn = "";
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

        private DateTime? _UpdatedDateTime = DateTime.Now;
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

        private string _UpdatedWindowsLoginName = "";
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

    public class clsDischargeSummaryDetailsVO : INotifyPropertyChanged, IValueObject
    {
        private long _RowID;
        public long RowID
        {
            get { return _RowID; }
            set
            {
                if (value != _RowID)
                {
                    _RowID = value;
                    OnPropertyChanged("RowID");
                }
            }
        }

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long _ParameterID;
        public long ParameterID
        {
            get { return _ParameterID; }
            set
            {
                if (value != _ParameterID)
                {
                    _ParameterID = value;
                    OnPropertyChanged("ParameterID");
                }
            }
        }

        private long _DischargeTempDetailID;
        public long DischargeTempDetailID
        {
            get { return _DischargeTempDetailID; }
            set
            {
                if (value != _DischargeTempDetailID)
                {
                    _DischargeTempDetailID = value;
                    OnPropertyChanged("DischargeTempDetailID");
                }
            }
        }

        private long _DischargeTempDetailUnitID;
        public long DischargeTempDetailUnitID
        {
            get { return _DischargeTempDetailUnitID; }
            set
            {
                if (value != _DischargeTempDetailUnitID)
                {
                    _DischargeTempDetailUnitID = value;
                    OnPropertyChanged("DischargeTempDetailUnitID");
                }
            }
        }

        private long _DisChargeSummaryID;
        public long DisChargeSummaryID
        {
            get { return _DisChargeSummaryID; }
            set
            {
                if (value != _DisChargeSummaryID)
                {
                    _DisChargeSummaryID = value;
                    OnPropertyChanged("DisChargeSummaryID");
                }
            }
        }

        private string _ApplicableFont;
        public string ApplicableFont
        {
            get { return _ApplicableFont; }
            set
            {
                if (value != _ApplicableFont)
                {
                    _ApplicableFont = value;
                    OnPropertyChanged("ApplicableFont");
                }
            }
        }

        private string _IsTextBox;
        public string IsTextBox
        {
            get { return _IsTextBox; }
            set
            {
                if (value != _IsTextBox)
                {
                    _IsTextBox = value;
                    OnPropertyChanged("IsTextBox");
                }
            }
        }

        private string _IsRichTextBox;
        public string IsRichTextBox
        {
            get { return _IsRichTextBox; }
            set
            {
                if (value != _IsRichTextBox)
                {
                    _IsRichTextBox = value;
                    OnPropertyChanged("IsRichTextBox");
                }
            }
        }

        private string _IsTime;
        public string IsTime
        {
            get { return _IsTime; }
            set
            {
                if (value != _IsTime)
                {
                    _IsTime = value;
                    OnPropertyChanged("IsTime");
                }
            }
        }

        private string _IsCheckBox;
        public string IsCheckBox
        {
            get { return _IsCheckBox; }
            set
            {
                if (value != _IsCheckBox)
                {
                    _IsCheckBox = value;
                    OnPropertyChanged("IsCheckBox");
                }
            }
        }

        private string _IsOption;
        public string IsOption
        {
            get { return _IsOption; }
            set
            {
                if (value != _IsOption)
                {
                    _IsOption = value;
                    OnPropertyChanged("IsOption");
                }
            }
        }

        private string _IsDate;
        public string IsDate
        {
            get { return _IsDate; }
            set
            {
                if (value != _IsDate)
                {
                    _IsDate = value;
                    OnPropertyChanged("IsDate");
                }
            }
        }

        private long _DisChargeSummaryUnitId;
        public long DisChargeSummaryUnitID
        {
            get { return _DisChargeSummaryUnitId; }
            set
            {
                if (value != _DisChargeSummaryUnitId)
                {
                    _DisChargeSummaryUnitId = value;
                    OnPropertyChanged("DisChargeSummaryUnitID");
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

        private long _BindingControl;
        public long BindingControl
        {
            get { return _BindingControl; }
            set
            {
                if (value != _BindingControl)
                {
                    _BindingControl = value;
                    OnPropertyChanged("BindingControl");

                    if (_BindingControl == (int)PalashDynamics.ValueObjects.BindingControl.CheckBox)
                        BindingControlName = "CheckBox";
                    else if (_BindingControl == (int)PalashDynamics.ValueObjects.BindingControl.DatePicker)
                        BindingControlName = "DatePicker";
                    else if (_BindingControl == (int)PalashDynamics.ValueObjects.BindingControl.Option)
                        BindingControlName = "Option";
                    else if (_BindingControl == (int)PalashDynamics.ValueObjects.BindingControl.Text)
                        BindingControlName = "Text";
                    else if (_BindingControl == (int)PalashDynamics.ValueObjects.BindingControl.Time)
                        BindingControlName = "Time";
                    else
                        BindingControlName = "";
                }
            }
        }

        private string _BindingControlName;
        public string BindingControlName
        {
            get { return _BindingControlName; }
            set
            {
                if (value != _BindingControlName)
                {
                    _BindingControlName = value;
                    OnPropertyChanged("BindingControlName");
                }
            }
        }

        private string strUnitName;
        public string UnitName
        {
            get { return strUnitName; }
            set
            {
                if (value != strUnitName)
                {
                    strUnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }

        private string _FieldName;
        public string FieldName
        {
            get { return _FieldName; }
            set
            {
                if (value != _FieldName)
                {
                    _FieldName = value;
                    OnPropertyChanged("FieldName");
                }
            }
        }

        private string _FieldDesc;
        public string FieldDesc
        {
            get { return _FieldDesc; }
            set
            {
                if (value != _FieldDesc)
                {
                    _FieldDesc = value;
                    OnPropertyChanged("FieldDesc");
                }
            }
        }

        private string _FieldText;
        public string FieldText
        {
            get { return _FieldText; }
            set
            {
                if (value != _FieldText)
                {
                    _FieldText = value;
                    OnPropertyChanged("FieldText");
                }
            }
        }

        private string _ParameterName;
        public string ParameterName
        {
            get { return _ParameterName; }
            set
            {
                if (value != _ParameterName)
                {
                    _ParameterName = value;
                    OnPropertyChanged("ParameterName");
                }
            }
        }

        private bool _IsTextBoxEnable;
        public bool IsTextBoxEnable
        {
            get { return _IsTextBoxEnable; }
            set
            {
                if (value != _IsTextBoxEnable)
                {
                    _IsTextBoxEnable = value;
                    OnPropertyChanged("IsTextBoxEnable");
                }
            }
        }

        private string _TextData;
        public string TextData
        {
            get { return _TextData; }
            set
            {
                if (value != _TextData)
                {
                    _TextData = value;
                    OnPropertyChanged("TextData");
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
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion
    }
}
