using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Pathology.PathologyMasters
{
    public class clsPathoParameterMasterVO : IValueObject, INotifyPropertyChanged
    {
        #region Property Declaration Section

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
        public string ParameterDesc
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    //OnPropertyChanged("Description");
                }
            }
        }

        private string _PrintName;
        public string PrintName
        {
            get { return _PrintName; }
            set
            {
                if (_PrintName != value)
                {
                    _PrintName = value;
                    OnPropertyChanged("PrintName");
                }
            }
        }

        private long _ParamUnit;
        public long ParamUnit
        {
            get { return _ParamUnit; }
            set
            {
                if (_ParamUnit != value)
                {
                    _ParamUnit = value;
                    OnPropertyChanged("ParameterUnitId");
                }
            }
        }

        //added by neena
        public long _ExcutionCalenderParameterID;
        public long ExcutionCalenderParameterID { get { return _ExcutionCalenderParameterID; } set { _ExcutionCalenderParameterID = value; } }
        //

        private string _strParameterUnitName;
        public string stringstrParameterUnitName
        {
            get { return _strParameterUnitName; }
            set
            {
                if (_strParameterUnitName != value)
                {
                    _strParameterUnitName = value;
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

        //by rohini dated 15/1/2015
        private double _DeltaCheckPer;
        public double DeltaCheckPer
        {
            get { return _DeltaCheckPer; }
            set
            {
                if (_DeltaCheckPer != value)
                {
                    _DeltaCheckPer = value;
                    OnPropertyChanged("DeltaCheckPer"); 
                }
            }
        }

        private bool _IsFlagReflex;
        public bool IsFlagReflex
        {
            get { return _IsFlagReflex; }
            set
            {
                if (_IsFlagReflex != value)
                {
                    _IsFlagReflex = value;
                    OnPropertyChanged("IsFlagReflex");
                }
            }
        }

        private List<MasterListItem> _ServiceList;
        public List<MasterListItem> ServiceList
            {
                get { return _ServiceList; }
                set
                {
                    if (_ServiceList != value)
                    {
                        _ServiceList = value;
                        OnPropertyChanged("ServiceList");
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
        //-------

        //private string _DefaultMale;
        //public string DefaultMale
        //{
        //    get { return _DefaultMale; }
        //    set
        //    {
        //        if (_DefaultMale != value)
        //        {
        //            _DefaultMale = value;
        //            OnPropertyChanged("DefaultMaleGen");
        //        }
        //    }
        //}

        //private string _DefaultFemale;
        //public string DefaultFemale
        //{
        //    get { return _DefaultFemale; }
        //    set
        //    {
        //        if (_DefaultFemale != value)
        //        {
        //            _DefaultFemale = value;
        //            OnPropertyChanged("DefaultFemaleGen");
        //        }
        //    }
        //}

        //private string _DefaultChild;
        //public string DefaultChild
        //{
        //    get { return _DefaultChild; }
        //    set
        //    {
        //        if (_DefaultChild != value)
        //        {
        //            _DefaultChild = value;
        //            OnPropertyChanged("DefaultChildGen");
        //        }
        //    }
        //}

        //private string _DefaultInfant;
        //public string DefaultInfant
        //{
        //    get { return _DefaultInfant; }
        //    set
        //    {
        //        if (_DefaultInfant != value)
        //        {
        //            _DefaultInfant = value;
        //            OnPropertyChanged("DefaultInfantGen");
        //        }
        //    }
        //}

        //private string _DefaultGeneral;
        //public string DefaultGeneral
        //{
        //    get { return _DefaultGeneral; }
        //    set
        //    {
        //        if (_DefaultGeneral != value)
        //        {
        //            _DefaultGeneral = value;
        //            OnPropertyChanged("DefaultGeneralGen");
        //        }
        //    }
        //}

        //private string _RangeLMale;
        //public string RangeLMale
        //{
        //    get { return _RangeLMale; }
        //    set
        //    {
        //        if (_RangeLMale != value)
        //        {
        //            _RangeLMale = value;
        //            OnPropertyChanged("RangeMaleLower");
        //        }
        //    }
        //}

        //private string _RangeUMale;
        //public string RangeUMale
        //{
        //    get { return _RangeUMale; }
        //    set
        //    {
        //        if (_RangeUMale != value)
        //        {
        //            _RangeUMale = value;
        //            OnPropertyChanged("RangeMaleUpper");
        //        }
        //    }
        //}

        //private string _RangeLFemale;
        //public string RangeLFemale
        //{
        //    get { return _RangeLFemale; }
        //    set
        //    {
        //        if (_RangeLFemale != value)
        //        {
        //            _RangeLFemale = value;
        //            OnPropertyChanged("RangeFemaleLower");
        //        }
        //    }
        //}

        //private string _RangeUFemale;
        //public string RangeUFemale
        //{
        //    get { return _RangeUFemale; }
        //    set
        //    {
        //        if (_RangeUFemale != value)
        //        {
        //            _RangeUFemale = value;
        //            OnPropertyChanged("RangeFemaleUpper");
        //        }
        //    }
        //}

        //private string _RangeLChild;
        //public string RangeLChild
        //{
        //    get { return _RangeLChild; }
        //    set
        //    {
        //        if (_RangeLChild != value)
        //        {
        //            _RangeLChild = value;
        //            OnPropertyChanged("RangeChildLower");
        //        }
        //    }
        //}

        //private string _RangeUChild;
        //public string RangeUChild
        //{
        //    get { return _RangeUChild; }
        //    set
        //    {
        //        if (_RangeUChild != value)
        //        {
        //            _RangeUChild = value;
        //            OnPropertyChanged("RangeChildUpper");
        //        }
        //    }
        //}

        //private string _RangeLInfant;
        //public string RangeLInfant
        //{
        //    get { return _RangeLInfant; }
        //    set
        //    {
        //        if (_RangeLInfant != value)
        //        {
        //            _RangeLInfant = value;
        //            OnPropertyChanged("RangeInfantLower");
        //        }
        //    }
        //}

        //private string _RangeUInfant;
        //public string RangeUInfant
        //{
        //    get { return _RangeUInfant; }
        //    set
        //    {
        //        if (_RangeUInfant != value)
        //        {
        //            _RangeUInfant = value;
        //            OnPropertyChanged("RangeInfantUpper");
        //        }
        //    }
        //}

        //private string _RangeLGen;
        //public string RangeLGen
        //{
        //    get { return _RangeLGen; }
        //    set
        //    {
        //        if (_RangeLGen != value)
        //        {
        //            _RangeLGen = value;
        //            OnPropertyChanged("RangeGenLower");
        //        }
        //    }
        //}

        //private string _RangeUGen;
        //public string RangeUGen
        //{
        //    get { return _RangeUGen; }
        //    set
        //    {
        //        if (_RangeUGen != value)
        //        {
        //            _RangeUGen = value;
        //            OnPropertyChanged("RangeGenUpper");
        //        }
        //    }
        //}

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
        private string _TextRange;
        public string TextRange
        {
            get { return _TextRange; }
            set
            {
                if (_TextRange != value)
                {
                    _TextRange = value;
                    OnPropertyChanged("TextRange");
                }
            }
        }

        //private bool _HasSpace;
        //public bool HasSpace
        //{
        //    get { return _HasSpace; }
        //    set
        //    {
        //        if (_HasSpace != value)
        //        {
        //            _HasSpace = value;
        //            OnPropertyChanged("HasSpace");
        //        }
        //    }
        //}

        private List<clsPathoParameterHelpValueMasterVO> _Items = new List<clsPathoParameterHelpValueMasterVO>();
        public List<clsPathoParameterHelpValueMasterVO> Items
        {
            get { return _Items; }
            set
            {
                _Items = value;
                OnPropertyChanged("Items");

            }
        }

        private List<clsPathoParameterDefaultValueMasterVO> _DefaultValues = new List<clsPathoParameterDefaultValueMasterVO>();
        public List<clsPathoParameterDefaultValueMasterVO> DefaultValues
        {
            get { return _DefaultValues; }
            set
            {
                _DefaultValues = value;
                OnPropertyChanged("DefaultValues");
            }
        }

        #endregion

        #region Common Properties

        private string _LinkServer;
        public string LinkServer
        {
            get { return _LinkServer; }
            set
            {
                if (_LinkServer != value)
                {
                    _LinkServer = value;
                    OnPropertyChanged("LinkServer");
                }
            }
        }

        private long _UnitId;
        public long UnitId
        {
            get { return _UnitId; }
            set
            {
                if (_UnitId != value)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
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

        #endregion

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

    }

    public class clsPathoParameterHelpValueMasterVO : IValueObject, INotifyPropertyChanged
    {

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

        private string _strHelp;
        public string strHelp
        {
            get { return _strHelp; }
            set
            {
                if (_strHelp != value)
                {
                    _strHelp = value;
                    OnPropertyChanged("strHelp");
                }
            }
        }

        private bool _IsDefault;
        public bool IsDefault
        {
            get { return _IsDefault; }
            set
            {
                if (_IsDefault != value)
                {
                    _IsDefault = value;
                    OnPropertyChanged("IsDefault");
                }
            }
        }
        private bool _IsDuplicate;
        public bool IsDuplicate
        {
            get { return _IsDuplicate; }
            set
            {
                if (_IsDuplicate != value)
                {
                    _IsDuplicate = value;
                    OnPropertyChanged("IsDuplicate");
                }
            }
        }


        //added by rohini dated 3.1.16
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
    }

    public class clsPathoParameterDefaultValueMasterVO : IValueObject, INotifyPropertyChanged
    {

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
     //---ADDED BY ROHINI DATYED 15/1/2016--------//
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
        private string _Machine;
        public string Machine
        {
            get { return _Machine; }
            set
            {
                if (_Machine != value)
                {
                    _Machine = value;
                    OnPropertyChanged("Machine");
                }
            }
        }
        private string _AgeValue;
        public string AgeValue
        {
            get { return _AgeValue; }
            set
            {
                if (_AgeValue != value)
                {
                    _AgeValue = value;
                    OnPropertyChanged("AgeValue");
                }
            }
        }
        //added by rohini dated 19.2.16 as per disscussion with nilesh sir for parameter master to difine services

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
        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set
            {
                if (_ServiceID != value)
                {
                    _ServiceID = value;
                    OnPropertyChanged("ServiceIDS");
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
        private long _ParameterDefaultID;
        public long ParameterDefaultID
        {
            get { return _ParameterDefaultID; }
            set
            {
                if (_ParameterDefaultID != value)
                {
                    _ParameterDefaultID = value;
                    OnPropertyChanged("ParameterDefaultID");
                }
            }
        }

        private Double _MinImprobable;
        public Double MinImprobable
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
        private Double _MaxImprobable;
        public Double MaxImprobable
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


        private Double _LowReflexValue;
        public Double LowReflexValue
        {
            get { return _LowReflexValue; }
            set
            {
                if (_LowReflexValue != value)
                {
                    _LowReflexValue = value;
                    OnPropertyChanged("LowReflexValue");
                }
            }
        }
        private Double _HighReflexValue;
        public Double HighReflexValue
        {
            get { return _HighReflexValue; }
            set
            {
                if (_HighReflexValue != value)
                {
                    _HighReflexValue = value;
                    OnPropertyChanged("HighReflexValue");
                }
            }
        }

        // Varying Reference Range 
        // Added by Anumani

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

        private string _VaryingReference;
        public string VaryingReference
        {
            get { return _VaryingReference; }
            set
            {
                if (_VaryingReference != value)
                {
                    _VaryingReference = value;
                    OnPropertyChanged("VaryingReference");
                }
            }
        }
        //
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
        private bool _IsReflexTesting;
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
        private MasterListItem _MasterListItem;
        public MasterListItem MasterListItem
        {
            get { return _MasterListItem; }
            set
            {
                if (_MasterListItem != value)
                {
                    _MasterListItem = value;
                    OnPropertyChanged("MasterListItem");
                }
            }
        }

        private List<MasterListItem> _List;
        public List<MasterListItem> List
        {
            get { return _List; }
            set
            {
                if (_List != value)
                {
                    _List = value;
                    OnPropertyChanged("List");
                }
            }
        }
       //---------------//
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


        private Double _DefaultValue;
        public Double DefaultValue
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

        private Double _MinValue;
        public Double MinValue
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

        private Double _HighReffValue;
        public Double HighReffValue
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

        private Double _LowReffValue;
        public Double LowReffValue
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

        private Double _UpperPanicValue;
        public Double UpperPanicValue
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
        private Double _LowerPanicValue;
        public Double LowerPanicValue
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

        private Double _MaxValue;
        public Double MaxValue
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

        private bool _IsAge;
        public bool IsAge
        {
            get { return _IsAge; }
            set
            {
                if (_IsAge != value)
                {
                    _IsAge = value;
                    OnPropertyChanged("IsAgeApplicable");
                }
            }
        }

        private Double _AgeFrom;
        public Double AgeFrom
        {
            get { return _AgeFrom; }
            set
            {
                if (_AgeFrom != value)
                {
                    _AgeFrom = value;
                    OnPropertyChanged("AgeFrom");
                }
            }
        }

        private Double _AgeTo;
        public Double AgeTo
        {
            get { return _AgeTo; }
            set
            {
                if (_AgeTo != value)
                {
                    _AgeTo = value;
                    OnPropertyChanged("AgeTo");
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
        public string ToXml()
        {
            return this.ToString();
        }
    }
}
