using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace PalashDynamics.ValueObjects.Pathology.PathologyMasters
{
    public class clsPathoTestParameterVO : IValueObject, INotifyPropertyChanged
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

        List<MasterListItem> _HelpValueList = new List<MasterListItem>();
        public List<MasterListItem> HelpValueList
        {
            get
            {
                return _HelpValueList;
            }
            set
            {
                if (value != _HelpValueList)
                {
                    _HelpValueList = value;
                }
            }

        }

        MasterListItem _SelectedHelpValue = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedHelpValue
        {
            get
            {
                return _SelectedHelpValue;
            }
            set
            {
                if (value != _SelectedHelpValue)
                {
                    _SelectedHelpValue = value;
                    OnPropertyChanged("SelectedHelpValue");
                }
            }


        }

        private object _ApColor;
        public object ApColor
        {
            get { return _ApColor; }
            set
            {
                if (_ApColor != value)
                {
                    _ApColor = value;
                    OnPropertyChanged("ApColor");
                }
            }
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

        private long _PathoTestID;
        public long PathoTestID
        {
            get { return _PathoTestID; }
            set
            {
                if (_PathoTestID != value)
                {
                    _PathoTestID = value;
                    OnPropertyChanged("PathoTestID");
                }
            }
        }

        private long _PathoSubTestID;
        public long PathoSubTestID
        {
            get { return _PathoSubTestID; }
            set
            {
                if (_PathoSubTestID != value)
                {
                    _PathoSubTestID = value;
                    OnPropertyChanged("PathoSubTestID");
                }
            }
        }

        private DateTime? _FromDate;
        public DateTime? FromDate
        {
            get
            { return _FromDate; }
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
            get
            { return _ToDate; }
            set
            {
                if (_ToDate != value)
                {
                    _ToDate = value;
                    OnPropertyChanged("ToDate");
                }
            }
        }

        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set
            {
                if (_LastName != value)
                {
                    _LastName = value;
                    OnPropertyChanged("LastName");
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

        private string _MrNo;
        public string MrNo
        {
            get { return _MrNo; }
            set
            {
                if (_MrNo != value)
                {
                    _MrNo = value;
                    OnPropertyChanged("MrNo");
                }
            }
        }

        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }


        private string _PathoTestName;
        public string PathoTestName
        {
            get { return _PathoTestName; }
            set
            {
                if (_PathoTestName != value)
                {
                    _PathoTestName = value;
                    OnPropertyChanged("PathoTestName");
                }
            }
        }

        private string _PathoSubTestName;
        public string PathoSubTestName
        {
            get { return _PathoSubTestName; }
            set
            {
                if (_PathoSubTestName != value)
                {
                    _PathoSubTestName = value;
                    OnPropertyChanged("PathoSubTestName");
                }
            }
        }

        public string Description { get; set; }

        private long _ParamSTID;
        public long ParamSTID
        {
            get { return _ParamSTID; }
            set
            {
                if (_ParamSTID != value)
                {
                    _ParamSTID = value;
                    OnPropertyChanged("ParamSTID");
                }
            }
        }

        private string _ParameterName;
        public string ParameterName
        {
            get { return _ParameterName; }
            set
            {
                if (_ParameterName != value)
                {
                    _ParameterName = value;
                    OnPropertyChanged("ParameterName");
                }
            }
        }

        private string _ParameterCode;
        public string ParameterCode
        {
            get { return _ParameterCode; }
            set
            {
                if (_ParameterCode != value)
                {
                    _ParameterCode = value;
                    OnPropertyChanged("ParameterCode");
                }
            }
        }


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
        private string _Formula;
        public string Formula
        {
            get { return _Formula; }
            set
            {
                if (_Formula != value)
                {
                    _Formula = value;
                    OnPropertyChanged("Formula");
                }
            }
        }

      
        private long _ParameterUnitID;
        public long ParameterUnitID
        {
            get { return _ParameterUnitID; }
            set
            {
                if (_ParameterUnitID != value)
                {
                    _ParameterUnitID = value;
                    OnPropertyChanged("ParameterUnitID");
                }
            }
        }

        private long _ParameterID;
        public long ParameterID
        {
            get { return _ParameterID; }
            set
            {
                if (_ParameterID != value)
                {
                    _ParameterID = value;
                    OnPropertyChanged("ParameterID");
                }
            }
        }

        private string _ParameterUnit;
        public string ParameterUnit
        {
            get { return _ParameterUnit; }
            set
            {
                if (_ParameterUnit != value)
                {
                    _ParameterUnit = value;
                    OnPropertyChanged("ParameterUnit");
                }
            }
        }

        private float _MinValue;
        public float MinValue
        {
            get { return _MinValue; }
            set
            {
                if (_MinValue != value)
                {
                    _MinValue = value;
                    OnPropertyChanged("MinValue");
                }
            }
        }

        private string _Category;
        public string Category
        {
            get { return _Category; }
            set
            {
                if (_Category != value)
                {
                    _Category = value;
                    OnPropertyChanged("Category");
                }
            }
        }
        private long? _CategoryID;
        public long? CategoryID
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



        private bool _HelpValueDefault;
        public bool HelpValueDefault
        {
            get { return _HelpValueDefault; }
            set
            {
                if (_HelpValueDefault != value)
                {
                    _HelpValueDefault = value;
                    OnPropertyChanged("HelpValueDefault");
                }
            }
        }



        private string _HelpValue;
        public string HelpValue
        {
            get { return _HelpValue; }
            set
            {
                if (_HelpValue != value)
                {
                    _HelpValue = value;
                    OnPropertyChanged("HelpValue");
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

        private string _TestAndSampleNO;
          public string TestAndSampleNO
        {
            get { return _TestAndSampleNO; }
            set
            {
                if (_TestAndSampleNO != value)
                {
                    _TestAndSampleNO = value;
                    OnPropertyChanged("TestAndSampleNO");
                }
            }
        }
        

        //by rohini to show default help value at the time of loadding row
        private string _HelpValue1;
        public string HelpValue1
        {
            get { return _HelpValue1; }
            set
            {
                if (_HelpValue1 != value)
                {
                    _HelpValue1 = value;
                    OnPropertyChanged("HelpValue1");
                }
            }
        }
        private bool _IsAbnoramal;
        public bool IsAbnoramal
        {
            get { return _IsAbnoramal; }
            set
            {
                if (_IsAbnoramal != value)
                {
                    _IsAbnoramal = value;
                    OnPropertyChanged("IsAbnoramal");
                }
            }
        }
        //
        private long _HelpValueID;
        public long HelpValueID
        {
            get { return _HelpValueID; }
            set
            {
                if (_HelpValueID != value)
                {
                    _HelpValueID = value;
                    OnPropertyChanged("HelpValueID");
                }
            }
        }

        private float _MaxValue;
        public float MaxValue
        {
            get { return _MaxValue; }
            set
            {
                if (_MaxValue != value)
                {
                    _MaxValue = value;
                    OnPropertyChanged("MaxValue");
                }
            }
        }

        private string _DefaultValue;
        public string DefaultValue
        {
            get { return _DefaultValue; }
            set
            {
                if (_DefaultValue != value)
                {
                    _DefaultValue = value;
                    OnPropertyChanged("DefaultValue");
                }
            }
        }

        private string _ResultValue;
        public string ResultValue
        {
            get { return _ResultValue; }
            set
            {
                if (_ResultValue != value)
                {
                    _ResultValue = value;
                    OnPropertyChanged("ResultValue");
                }
            }
        }

        private string _NormalRange;
        public string NormalRange
        {
            get { return _NormalRange; }
            set
            {
                if (value != _NormalRange)
                {
                    _NormalRange = value;
                    OnPropertyChanged("NormalRange");
                }
            }
        }

        private long _PrintNameTypeID;
        public long PrintNameTypeID
        {
            get { return _PrintNameTypeID; }
            set
            {
                if (_PrintNameTypeID != value)
                {
                    _PrintNameTypeID = value;
                    OnPropertyChanged("PrintNameTypeID");
                }
            }
        }
        // Added by Anumani
        private double _DeltaCheckDefaultValue;
        public double DeltaCheckDefaultValue
        {
            get { return _DeltaCheckDefaultValue; }
            set
            {
                if (_DeltaCheckDefaultValue != value)
                {
                    _DeltaCheckDefaultValue = value;
                    OnPropertyChanged("DeltaCheckPass");
                }
            }
        }

        private string _DeltacheckString;
        public string DeltacheckString
        {
            get { return _DeltacheckString; }
            set
            {
                if (value != _DeltacheckString)
                {
                    _DeltacheckString = value;
                    OnPropertyChanged("DeltacheckString");
                }
            }
        }

        private bool _DeltaCheckFail = false;
        public bool DeltaCheckFail
        {
            get { return _DeltaCheckFail; }
            set
            {
                if (_DeltaCheckFail != value)
                {
                    _DeltaCheckFail = value;
                    OnPropertyChanged("DeltaCheckFail");
                }
            }
        }

        private bool _DeltaCheck = false;
        public bool DeltaCheck
        {
            get { return _DeltaCheck; }
            set
            {
                if (_DeltaCheck != value)
                {
                    _DeltaCheck = value;
                    OnPropertyChanged("DeltaCheck");
                }
            }
        }


        private bool _IsReflexTesting = false;
        public bool IsReflexTesting
        {
            get { return _IsReflexTesting; }
            set
            {
                if (_IsReflexTesting != value)
                {
                    _IsReflexTesting = value;
                    OnPropertyChanged("IsReflexTesting");
                }
            }
        }

        private double _DeltaCheckValue;
        public double DeltaCheckValue
        {
            get { return _DeltaCheckValue; }
            set
            {
                if (_DeltaCheckValue != value)
                {
                    _DeltaCheckValue = value;
                    OnPropertyChanged("DeltaCheckValue");
                }
            }
        }

        private bool _ReflexTestingFlag;
        public bool ReflexTestingFlag
        {
            get { return _ReflexTestingFlag; }
            set
            {
                if (_ReflexTestingFlag != value)
                {
                    _ReflexTestingFlag = value;
                    OnPropertyChanged("ReflexTestingFlag");
                }
            }
        }

        private bool _IsMachine;
        public bool IsMachine
        {
            get { return _IsMachine; }
            set
            {
                if (_IsMachine != value)
                {
                    _IsMachine = value;
                    OnPropertyChanged("IsMachine");
                }
            }
        }




        private string _ReflexTesting;
        public string ReflexTesting
        {
            get { return _ReflexTesting; }
            set
            {
                if (_ReflexTesting != value)
                {
                    _ReflexTesting = value;
                    OnPropertyChanged("ReflexTesting");
                }
            }
        }

        private string _MachineMannual;
        public string MachineMannual
        {
            get { return _MachineMannual; }
            set
            {
                if (_MachineMannual != value)
                {
                    _MachineMannual = value;
                    OnPropertyChanged("MachineMannual");
                }
            }
        }

        private string _PreviousResultValue;
        public string PreviousResultValue
        {
            get { return _PreviousResultValue; }
            set
            {
                if (_PreviousResultValue != value)
                {
                    _PreviousResultValue = value;
                    OnPropertyChanged("PreviousResultValue");
                }
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


        private double _MinImprobable;
        public double MinImprobable
        {
            get { return _MinImprobable; }
            set
            {
                if (_MinImprobable != value)
                {
                    _MinImprobable = value;
                    OnPropertyChanged("MinImprobable");
                }
            }

        }

        private double _MaxImprobable;
        public double MaxImprobable
        {
            get { return _MaxImprobable; }
            set
            {
                if (_MaxImprobable != value)
                {
                    _MaxImprobable = value;
                    OnPropertyChanged("MaxImprobable");
                }
            }

        }

        // Properties Added on 21.04.2016 
        // To reflect Auto Verification of Result Values which are in Normal Ranges.

        private bool _IsFirstLevel;
        public bool IsFirstLevel
        {
            get { return _IsFirstLevel; }
            set
            {
                if (_IsFirstLevel != value)
                {
                    _IsFirstLevel = value;
                    OnPropertyChanged("IsFirstLevel");
                }
            }
        }


        private bool _IsSecondLevel;
        public bool IsSecondLevel
        {
            get { return _IsSecondLevel; }
            set
            {
                if (_IsSecondLevel != value)
                {
                    _IsSecondLevel = value;
                    OnPropertyChanged("IsSecondLevel");
                }
            }
        }



      
        private double _HighReffValue;
        public double HighReffValue
        {
            get { return _HighReffValue; }
            set
            {
                if (_HighReffValue != value)
                {
                    _HighReffValue = value;
                    OnPropertyChanged("HighReffValue");

                }
            }

        }

        private double _LowReffValue;
        public double LowReffValue
        {
            get { return _LowReffValue; }
            set
            {
                if (_LowReffValue != value)
                {
                    _LowReffValue = value;
                    OnPropertyChanged("LowReffValue");

                }
            }

        }

        private double _UpperPanicValue;
        public double UpperPanicValue
        {
            get { return _UpperPanicValue; }
            set
            {
                if (_UpperPanicValue != value)
                {
                    _UpperPanicValue = value;
                    OnPropertyChanged("UpperPanicValue");

                }
            }

        }

        private double _LowerPanicValue;
        public double LowerPanicValue
        {
            get { return _LowerPanicValue; }
            set
            {
                if (_LowerPanicValue != value)
                {
                    _LowerPanicValue = value;
                    OnPropertyChanged("LowerPanicValue");

                }
            }

        }

        private double _LowReflex;
        public double LowReflex
        {
            get { return _LowReflex; }
            set
            {
                if (_LowReflex != value)
                {
                    _LowReflex = value;
                    OnPropertyChanged("LowReflex");

                }
            }

        }

        private double _HighReflex;
        public double HighReflex
        {
            get { return _HighReflex; }
            set
            {
                if (_HighReflex != value)
                {
                    _HighReflex = value;
                    OnPropertyChanged("HighReflex");

                }
            }

        }


     

        private string _ReferenceRange;
        public string ReferenceRange
        {
            get { return _ReferenceRange; }
            set
            {
                if (value != _ReferenceRange)
                {
                    _ReferenceRange = value;
                    OnPropertyChanged("ReferenceRange");
                }
            }
        }


        // Added On 5.05.2016 
        // To Reflect Panic and Improbable flags 
        // Panic Range
        private object _ApColorPanic;
        public object ApColorPanic
        {
            get { return _ApColorPanic; }
            set
            {
                if (_ApColorPanic != value)
                {
                    _ApColorPanic = value;
                    OnPropertyChanged("ApColorPanic");
                }
            }
        }

        // Improbable Range
        private object _ApColorImp;
        public object ApColorImp
        {
            get { return _ApColorImp; }
            set
            {
                if (_ApColorImp != value)
                {
                    _ApColorImp = value;
                    OnPropertyChanged("ApColorImp");
                }
            }
        }
       
        // Varying references Ranges 
        // Added on 26/07/204

        private string _VaryingReferences;
        public string VaryingReferences
        {
            get { return _VaryingReferences; }
            set
            {
                if (_VaryingReferences != value)
                {
                    _VaryingReferences = value;
                    OnPropertyChanged("VaryingReferences");
                }
            }
        }




        // ENDS

        private long _PrintPosition;
        public long PrintPosition
        {
            get { return _PrintPosition; }
            set
            {
                if (_PrintPosition != value)
                {
                    _PrintPosition = value;
                    OnPropertyChanged("PrintPosition");
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

            private bool _IsTemplate;
            public bool IsTemplate
            {
                get { return _IsTemplate; }
                set
                {
                    if (_IsTemplate != value)
                    {
                        _IsTemplate = value;
                        OnPropertyChanged("IsTemplate");
                    }
                }
            }

        private DateTime? _OrderDate;
        public DateTime? OrderDate
        {
            get
            {
                return _OrderDate;
            }
            set
            {
                if (_OrderDate != value)
                {
                    _OrderDate = value;
                    OnPropertyChanged("OrderDate");
                }
            }
        }

        private bool _IsNumeric;
        public bool IsNumeric
        {
            get { return _IsNumeric; }
            set
            {
                if (_IsNumeric != value)
                {
                    _IsNumeric = value;
                    OnPropertyChanged("IsNumeric");
                }
            }
        }

        private bool _IsReadOnly;
        public bool IsReadOnly
        {
            get { return _IsReadOnly; }
            set
            {
                if (_IsReadOnly != value)
                {
                    _IsReadOnly = value;
                    OnPropertyChanged("IsReadOnly");
                }
            }
        }

        private bool _SelStatus;
        public bool SelStatus
        {
            get { return _SelStatus; }
            set
            {
                if (_SelStatus != value)
                {
                    _SelStatus = value;
                    OnPropertyChanged("SelStatus");
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


        private string _LoginName;
        public string LoginName
        {
            get { return _LoginName; }
            set
            {
                if (_LoginName != value)
                {
                    _LoginName = value;
                    OnPropertyChanged("LoginName");
                }
            }
        }

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set
            {
                if (_UserName != value)
                {
                    _UserName = value;
                    OnPropertyChanged("UserName");
                }
            }
        }

        List<MasterListItem> _PrintName = new List<MasterListItem> 
        { 
            new MasterListItem{ ID=0,Description="--Select--"} ,
            new MasterListItem{ ID=1,Description="Text"} ,
            new MasterListItem{ ID=2,Description="Numeric"} ,
           
        };

        public List<MasterListItem> PrintName
        {
            get
            {
                return _PrintName;
            }
            set
            {
                if (value != _PrintName)
                {
                    _PrintName = value;
                }
            }

        }

        MasterListItem _SelectedPrintName = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedPrintName
        {
            get
            {
                return _SelectedPrintName;
            }
            set
            {
                if (value != _SelectedPrintName)
                {
                    _SelectedPrintName = value;
                    OnPropertyChanged("SelectedPrintName");
                }
            }


        }

        public string Print { get; set; }
        public string FootNote { get; set; }
      
        public string Note { get; set; }

        //Added by Anumani on 22.02.2016

        private long _PatientId;
        public long PatientId
        {
            get
            {
                return _PatientId;
             }
            set
            {
                if (value != _PatientId)
                {
                    _PatientId = value;
                    OnPropertyChanged("PatinetId");

                }
            }
        }

        private long _ParameterDefaultValueId;
        public long ParameterDefaultValueId
        {
            get
            {
                return _ParameterDefaultValueId;
            }
            set
            {
                if (value != _ParameterDefaultValueId)
                {
                    _ParameterDefaultValueId = value;
                    OnPropertyChanged("ParameterDefaultValueId");

                }
            }
        }


        




        #endregion

        public override string ToString()
        {
            return Description;
        }

        #region Newly added 

        private string _TestCategory;
        public string TestCategory
        {
            get { return _TestCategory; }
            set
            {
                if (_TestCategory != value)
                {
                    _TestCategory = value;
                    OnPropertyChanged("TestCategory");
                }
            }
        }

        private long? _TestCategoryID;
        public long? TestCategoryID
        {
            get { return _TestCategoryID; }
            set
            {
                if (_TestCategoryID != value)
                {
                    _TestCategoryID = value;
                    OnPropertyChanged("TestCategoryID");
                }
            }
        }

        #endregion 

    }

    public class clsPathoSubTestVO : IValueObject, INotifyPropertyChanged
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
        
        #region Property Declaration

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

        private bool _PrintSubTestName;
        public bool PrintSubTestName
        {
            get { return _PrintSubTestName; }
            set
            {
                if (_PrintSubTestName != value)
                {
                    _PrintSubTestName = value;
                    OnPropertyChanged("_PrintSubTestName");
                }
            }
        }
        private long _PrintPosition;
        public long PrintPosition
        {
            get { return _PrintPosition; }
            set
            {
                if (_PrintPosition != value)
                {
                    _PrintPosition = value;
                    OnPropertyChanged("PrintPosition");
                }
            }
        }

        private long _PathoTestID;
        public long PathoTestID
        {
            get { return _PathoTestID; }
            set
            {
                if (_PathoTestID != value)
                {
                    _PathoTestID = value;
                    OnPropertyChanged("PathoTestID");
                }
            }
        }

        private long _ParamSTID;
        public long ParamSTID
        {
            get { return _ParamSTID; }
            set
            {
                if (_ParamSTID != value)
                {
                    _ParamSTID = value;
                    OnPropertyChanged("ParamSTID");
                }
            }
        }

        private string _PathoTestName;
        public string PathoTestName
        {
            get { return _PathoTestName; }
            set
            {
                if (_PathoTestName != value)
                {
                    _PathoTestName = value;
                    OnPropertyChanged("PathoTestName");
                }
            }
        }

        private string _PathoSubTestName;
        public string PathoSubTestName
        {
            get { return _PathoSubTestName; }
            set
            {
                if (_PathoSubTestName != value)
                {
                    _PathoSubTestName = value;
                    OnPropertyChanged("PathoSubTestName");
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

        public string Description { get; set; }
        
        List<MasterListItem> _PrintName = new List<MasterListItem> 
        { 
          //  new MasterListItem{ ID=0,Description="--Select--"} ,
            new MasterListItem{ ID=1,Description="Text"} ,
            //new MasterListItem{ ID=2,Description="Numeric"} ,
           
        };

        public List<MasterListItem> PrintName
        {
            get
            {
                return _PrintName;
            }
            set
            {
                if (value != _PrintName)
                {
                    _PrintName = value;
                }
            }

        }

        //MasterListItem _SelectedPrintName = new MasterListItem { ID = 0, Description = "--Select--" };
        MasterListItem _SelectedPrintName = new MasterListItem { ID = 1, Description = "Text" };
        public MasterListItem SelectedPrintName
        {
            get
            {
                return _SelectedPrintName;
            }
            set
            {
                if (value != _SelectedPrintName)
                {
                    _SelectedPrintName = value;
                    OnPropertyChanged("SelectedPrintName");
                }
            }
        }

        public string Print { get; set; }

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

        private bool _SelStatus;
        public bool SelStatus
        {
            get { return _SelStatus; }
            set
            {
                if (_SelStatus != value)
                {
                    _SelStatus = value;
                    OnPropertyChanged("SelStatus");
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
        private long _SubTestID;
        public long SubTestID
        {
            get
            {
                return _SubTestID;
            }
            set
            {
                if (value != _SubTestID)
                {
                    _SubTestID = value;
                    OnPropertyChanged("SubTestID");
                }
            }
        }
        private long _MachineID;
        public long MachineID
        {
            get
            {
                return _MachineID;
            }
            set
            {
                if (value != _MachineID)
                {
                    _MachineID = value;
                    OnPropertyChanged("MachineID");
                }
            }
        }
        #endregion

    }

}
