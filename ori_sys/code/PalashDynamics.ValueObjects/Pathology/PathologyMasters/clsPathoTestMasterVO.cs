using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Radiology;

namespace PalashDynamics.ValueObjects.Pathology.PathologyMasters
{
    public class clsPathoTestMasterVO : IValueObject, INotifyPropertyChanged
    {
        #region IValueObject
        public string ToXml()
        {
            return this.ToString();
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

        #region Property Declartion


        private List<clsPathoTestParameterVO> _TestParameterList;
        public List<clsPathoTestParameterVO> TestParameterList
        {
            get
            {
                if (_TestParameterList == null)
                    _TestParameterList = new List<clsPathoTestParameterVO>();
                return _TestParameterList;
            }
            set
            {
                _TestParameterList = value;
            }
        }

        private List<clsPathoTestItemDetailsVO> _TestItemList;
        public List<clsPathoTestItemDetailsVO> TestItemList
        {
            get
            {
                if (_TestItemList == null)
                    _TestItemList = new List<clsPathoTestItemDetailsVO>();

                return _TestItemList;
            }

            set
            {

                _TestItemList = value;

            }
        }

        private List<clsPathoTestSampleVO> _TestSampleList;
        public List<clsPathoTestSampleVO> TestSampleList
        {
            get
            {
                if (_TestSampleList == null)
                    _TestSampleList = new List<clsPathoTestSampleVO>();

                return _TestSampleList;
            }

            set
            {

                _TestSampleList = value;

            }
        }

        //added by rohini dated 21.1.16

        private List<clsPathoTemplateVO> _TestTemplateList;
        public List<clsPathoTemplateVO> TestTemplateList
        {
            get
            {
                if (_TestTemplateList == null)
                    _TestTemplateList = new List<clsPathoTemplateVO>();

                return _TestTemplateList;
            }

            set
            {

                _TestTemplateList = value;

            }
        }
        private bool _IsAbnormal;
        public bool IsAbnormal
        {
            get { return _IsAbnormal; }
            set
            {
                if (_IsAbnormal != value)
                {
                    _IsAbnormal = value;
                    OnPropertyChanged("IsAbnormal");
                }
            }
        }
     
        private bool _ReportTemplate;
        public bool ReportTemplate
        {
            get { return _ReportTemplate; }
            set
            {
                if (_ReportTemplate != value)
                {
                    _ReportTemplate = value;
                    OnPropertyChanged("ReportTemplate");
                }
            }
        }

        private bool _IsFromParameter;
        public bool IsFromParameter
        {
            get { return _IsFromParameter; }
            set
            {
                if (_IsFromParameter != value)
                {
                    _IsFromParameter = value;
                    OnPropertyChanged("IsFromParameter");
                }
            }
        }
        //

        private List<clsPathoSubTestVO> _SubTestList;
        public List<clsPathoSubTestVO> SubTestList
        {
            get
            {
                if (_SubTestList == null)
                    _SubTestList = new List<clsPathoSubTestVO>();
                return _SubTestList;
            }
            set { _SubTestList = value; }
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

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set
            {
                if (_Code != value)
                {
                    _Code = value;
                    OnPropertyChanged("Code");
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


        //by rohinee 
        private string _FormulaID;
        public string FormulaID
        {
            get { return _FormulaID; }
            set
            {
                if (_FormulaID != value)
                {
                    _FormulaID = value;
                    OnPropertyChanged("FormulaID");
                }
            }
        }
        //
        private string _TestPrintName;
        public string TestPrintName
        {
            get { return _TestPrintName; }
            set
            {
                if (_TestPrintName != value)
                {
                    _TestPrintName = value;
                    OnPropertyChanged("TestPrintName");
                }
            }
        }

        private short _Applicableto;
        public short Applicableto
        {
            get { return _Applicableto; }
            set
            {
                if (_Applicableto != value)
                {
                    _Applicableto = value;
                    OnPropertyChanged("Applicableto");
                }
            }
        }

        private bool _IsSubTest;
        public bool IsSubTest
        {
            get { return _IsSubTest; }
            set
            {
                if (_IsSubTest != value)
                {
                    _IsSubTest = value;
                    OnPropertyChanged("IsSubTest");
                }
            }
        }

        private long _CategoryID;
        public long CategoryID
        {
            get { return _CategoryID; }
            set
            {
                if (_CategoryID != value)
                {
                    _CategoryID = value;
                    OnPropertyChanged("CategoryID");
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

        private bool _IsParameter;
        public bool IsParameter
        {
            get { return _IsParameter; }
            set
            {
                if (_IsParameter != value)
                {
                    _IsParameter = value;
                    OnPropertyChanged("IsParameter");
                }
            }
        }
        
        //by rohini dated 20.1.16
        private double _TurnAroundTime;
        public double TurnAroundTime
        {
            get { return _TurnAroundTime; }
            set
            {
                if (_TurnAroundTime != value)
                {
                    _TurnAroundTime = value;
                    OnPropertyChanged("TurnAroundTime");
                }
            }
        }

        private long _TubeID;
        public long TubeID
        {
            get { return _TubeID; }
            set
            {
                if (_TubeID != value)
                {
                    _TubeID = value;
                    OnPropertyChanged("TubeID");
                }
            }
        }
        private int _IsFormTemplate;
        public int IsFormTemplate
        {
            get { return _IsFormTemplate; }
            set
            {
                if (_IsFormTemplate != value)
                {
                    _IsFormTemplate = value;
                    OnPropertyChanged("IsFormTemplate");
                }
            }
        }
        private int _IsWordTemplate;
        public int IsWordTemplate
        {
            get { return _IsWordTemplate; }
            set
            {
                if (_IsWordTemplate != value)
                {
                    _IsWordTemplate = value;
                    OnPropertyChanged("IsWordTemplate");
                }
            }
        }
        //-------------------------/rohini/----------------
        private string _Note;
        public string Note
        {
            get { return _Note; }
            set
            {
                if (_Note != value)
                {
                    _Note = value;
                    OnPropertyChanged("Note");
                }
            }
        }

        private bool _HasNormalRange;
        public bool HasNormalRange
        {
            get { return _HasNormalRange; }
            set
            {
                if (_HasNormalRange != value)
                {
                    _HasNormalRange = value;
                    OnPropertyChanged("HasNormalRange");
                }
            }
        }

        private bool _HasObserved;
        public bool HasObserved
        {
            get { return _HasObserved; }
            set
            {
                if (_HasObserved != value)
                {
                    _HasObserved = value;
                    OnPropertyChanged("HasObserved");
                }
            }
        }

        private bool _PrintTestName;
        public bool PrintTestName
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

        private bool _Status;
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

        private DateTime _ApplicableFrom;
        public DateTime ApplicableFrom
        {
            get { return _ApplicableFrom; }
            set
            {
                if (_ApplicableFrom != value)
                {
                    _ApplicableFrom = value;
                    OnPropertyChanged("ApplicableFrom");
                }
            }
        }

        private long _ApprovedBy;
        public long ApprovedBy
        {
            get { return _ApprovedBy; }
            set
            {
                if (_ApprovedBy != value)
                {
                    _ApprovedBy = value;
                    OnPropertyChanged("ApprovedBy");
                }
            }
        }

      

       

        private DateTime _Date;
        public DateTime Date
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

        private DateTime _Time;
        public DateTime Time
        {
            get { return _Time; }
            set
            {
                if (_Time != value)
                {
                    _Time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

     

        private bool _IsCultureSenTest;
        public bool IsCultureSenTest
        {
            get { return _IsCultureSenTest; }
            set
            {
                if (_IsCultureSenTest != value)
                {
                    _IsCultureSenTest = value;
                    OnPropertyChanged("IsCultureSenTest");
                }
            }
        }

        private bool _NeedAuthorization;
        public bool NeedAuthorization
        {
            get { return _NeedAuthorization; }
            set
            {
                if (_NeedAuthorization != value)
                {
                    _NeedAuthorization = value;
                    OnPropertyChanged("NeedAuthorization");
                }
            }
        }

        private bool _FreeFormat;
        public bool FreeFormat
        {
            get { return _FreeFormat; }
            set
            {
                if (_FreeFormat != value)
                {
                    _FreeFormat = value;
                    OnPropertyChanged("FreeFormat");
                }
            }
        }

        private long _MachineID;
        public long MachineID
        {
            get { return _MachineID; }
            set
            {
                if (_MachineID != value)
                {
                    _MachineID = value;
                    OnPropertyChanged("MachineID");
                }
            }
        }

        private string _Technique;
        public string Technique
        {
            get { return _Technique; }
            set
            {
                if (_Technique != value)
                {
                    _Technique = value;
                    OnPropertyChanged("Technique");
                }
            }
        }

        private string _FootNote;
        public string FootNote
        {
            get { return _FootNote; }
            set
            {
                if (_FootNote != value)
                {
                    _FootNote = value;
                    OnPropertyChanged("FootNote");
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
        private long _SubTestID;
        public long SubTestID
        {
            get { return _SubTestID; }
            set
            {
                if (_SubTestID != value)
                {
                    _SubTestID = value;
                    OnPropertyChanged("SubTestID");
                }
            }
        }
        private long? _CreatedUnitID;
        public long? CreatedUnitID
        {
            get
            {
                return _CreatedUnitID;
            }
            set
            {
                _CreatedUnitID = value;
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get
            {
                return _UpdatedUnitID;
            }
            set
            {
                _UpdatedUnitID = value;
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get
            {
                return _AddedBy;
            }
            set
            {
                _AddedBy = value;
            }
        }

        private string _AddedOn;
        public string AddedOn
        {
            get
            {
                return _AddedOn;
            }
            set
            {
                _AddedOn = value;
            }
        }

        private DateTime? _AddedDateTime;
        public DateTime? AddedDateTime
        {
            get
            {
                return _AddedDateTime;
            }
            set
            {
                _AddedDateTime = value;
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get
            {
                return _UpdatedBy;
            }
            set
            {
                _UpdatedBy = value;
            }
        }


        private string _UpdatedOn;
        public string UpdatedOn
        {
            get
            {
                return _UpdatedOn;
            }
            set
            {
                _UpdatedOn = value;
            }
        }

        private DateTime? _UpdatedDateTime;
        public DateTime? UpdatedDateTime
        {
            get
            {
                return _UpdatedDateTime;
            }
            set
            {
                _UpdatedDateTime = value;
            }
        }

        private string _AddedWindowsLoginName;
        public string AddedWindowsLoginName
        {
            get
            {
                return _AddedWindowsLoginName;
            }
            set
            {
                _AddedWindowsLoginName = value;
            }
        }

        private string _UpdateWindowsLoginName;
        public string UpdateWindowsLoginName
        {
            get
            {
                return _UpdateWindowsLoginName;
            }
            set
            {
                _UpdateWindowsLoginName = value;
            }
        }

       //added by rohini datyed 19.1.16
        public List<clsPathoTestMasterVO> SupplierList { get; set; }
        List<MasterListItem> _HPLevelList = new List<MasterListItem> 
        { 
            new MasterListItem{ ID=0,Description="--Select--"} ,
            new MasterListItem{ ID=1,Description="I"} ,
            new MasterListItem{ ID=2,Description="II"} ,
            new MasterListItem{ ID=3,Description="III"} ,
        };
        public List<MasterListItem> HPLevelList
        {
            get
            {
                return _HPLevelList;
            }
            set
            {
                if (value != _HPLevelList)
                {
                    _HPLevelList = value;
                }
            }

        }
        MasterListItem _SelectedHPLevel = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedHPLevel
        {
            get
            {
                return _SelectedHPLevel;
            }
            set
            {
                if (value != _SelectedHPLevel)
                {
                    _SelectedHPLevel = value;
                    OnPropertyChanged("SelectedHPLevel");
                }
            }
        }
        private string _SupplierName;
        public string SupplierName
        {
            get
            {
                return _SupplierName;
            }
            set
            {
                if (value != _SupplierName)
                {
                    _SupplierName = value;
                    OnPropertyChanged("SupplierName");
                }
            }
        }
        
        private Boolean _status;
        public Boolean status
        {
            get
            {
                return _status;
            }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged("status");
                }
            }
        }
        private long _TestID;
        public long TestID
        {
            get
            {
                return _TestID;
            }
            set
            {
                if (value != _TestID)
                {
                    _TestID = value;
                    OnPropertyChanged("TestID");
                }
            }
        }
        //
     

        #endregion
    }
}
