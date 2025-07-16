using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
   public class cls_IVFDashboard_SemenWashVO : IValueObject, INotifyPropertyChanged
    {

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

        #region Common Properties

        private long? _semenused;//Use this property to check enable/disable row
        public long? semenused
        {
            get { return _semenused; }
            set
            {
                if (_semenused != value)
                {
                    _semenused = value;
                    OnPropertyChanged("semenused");
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


        private bool _Synchronized = true;
        public bool Synchronized
        {
            get { return _Synchronized; }
            set
            {
                if (_Synchronized != value)
                {
                    _Synchronized = value;
                    OnPropertyChanged("Synchronized");
                }
            }
        }

        #endregion

        #region Properties

        private long _SemenUsedDetailsID;
        public long SemenUsedDetailsID
        {
            get { return _SemenUsedDetailsID; }
            set
            {
                if (_SemenUsedDetailsID != value)
                {
                    _SemenUsedDetailsID = value;
                    OnPropertyChanged("SemenUsedDetailsID");
                }
            }

        }

        private bool _IsUsed;
        public bool IsUsed
        {
            get { return _IsUsed; }
            set
            {
                if (_IsUsed != value)
                {
                    _IsUsed = value;
                    OnPropertyChanged("IsUsed");
                }
            }
        }

        private string _Eye;
        public string Eye
        {
            get { return _Eye; }
            set
            {
                if (_Eye != value)
                {
                    _Eye = value;
                    OnPropertyChanged("Eye");
                }
            }
        }
        private string _Hair;
        public string Hair
        {
            get { return _Hair; }
            set
            {
                if (_Hair != value)
                {
                    _Hair = value;
                    OnPropertyChanged("Hair");
                }
            }
        }
        private string _Skin;
        public string Skin
        {
            get { return _Skin; }
            set
            {
                if (_Skin != value)
                {
                    _Skin = value;
                    OnPropertyChanged("Skin");
                }
            }
        }
        private string _BloodGroup;
        public string BloodGroup
        {
            get { return _BloodGroup; }
            set
            {
                if (_BloodGroup != value)
                {
                    _BloodGroup = value;
                    OnPropertyChanged("BloodGroup");
                }
            }
            
        }
        private float _Height;
        public float Height
        {
            get { return _Height; }
            set
            {
                if (_Height != value)
                {
                    _Height = value;
                    OnPropertyChanged("Height");
                }
            }
        }
        private string _BoneStructure;
        public string BoneStructure
        {
            get { return _BoneStructure; }
            set
            {
                if (_BoneStructure != value)
                {
                    _BoneStructure = value;
                    OnPropertyChanged("BoneStructure");
                }
            }
        }
       
        private string _DonorCode;
        public string DonorCode
        {
            get { return _DonorCode; }
            set
            {
                if (_DonorCode != value)
                {
                    _DonorCode = value;
                    OnPropertyChanged("DonorCode");
                }
            }
        }
        private string _Lab;
        public string Lab
        {
            get { return _Lab; }
            set
            {
                if (_Lab != value)
                {
                    _Lab = value;
                    OnPropertyChanged("Lab");
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

        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {
                if (_VisitID != value)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }


        private DateTime? _CollectionDate;
        public DateTime? CollectionDate
        {
            get { return _CollectionDate; }
            set
            {
                if (_CollectionDate != value)
                {
                    _CollectionDate = value;
                    OnPropertyChanged("CollectionDate");
                }
            }
        }

        private bool _ISChecked;
        public bool ISChecked
        {
            get { return _ISChecked; }
            set
            {
                if (_ISChecked != value)
                {
                    _ISChecked = value;
                    OnPropertyChanged("ISChecked");
                }
            }
        }

        public long UsedPlanTherapyID { get; set; }
        public long UsedPlanTherapyUnitID { get; set; }

        private bool _ISReadonly;
        public bool ISReadonly
        {
            get { return _ISReadonly; }
            set
            {
                if (_ISReadonly != value)
                {
                    _ISReadonly = value;
                    OnPropertyChanged("ISReadonly");
                }
            }
        }

        private long _MethodOfCollectionID;
        public long MethodOfCollectionID
        {
            get { return _MethodOfCollectionID; }
            set
            {
                if (_MethodOfCollectionID != value)
                {
                    _MethodOfCollectionID = value;
                    OnPropertyChanged("MethodOfCollectionID");
                }
            }
        }

        private string _TypeOfSperm;
        public string TypeOfSperm
        {
            get { return _TypeOfSperm; }
            set
            {
                if (_TypeOfSperm != value)
                {
                    _TypeOfSperm = value;
                    OnPropertyChanged("TypeOfSperm");
                }
            }
        }

        private long _SpermTypeID;
        public long SpermTypeID
        {
            get { return _SpermTypeID; }
            set
            {
                if (_SpermTypeID != value)
                {
                    _SpermTypeID = value;
                    OnPropertyChanged("SpermTypeID");
                }
            }
        }


        private string _SampleCode;
        public string SampleCode
        {
            get { return _SampleCode; }
            set
            {
                if (_SampleCode != value)
                {
                    _SampleCode = value;
                    OnPropertyChanged("SampleCode");
                }
            }
        }

        private string _SampleLinkID;
        public string SampleLinkID
        {
            get { return _SampleLinkID; }
            set
            {
                if (_SampleLinkID != value)
                {
                    _SampleLinkID = value;
                    OnPropertyChanged("SampleLinkID");
                }
            }
        }


        private string _MethodOfCollection;
        public string MethodOfCollection
        {
            get { return _MethodOfCollection; }
            set
            {
                if (_MethodOfCollection != value)
                {
                    _MethodOfCollection = value;
                    OnPropertyChanged("MethodOfCollection");
                }
            }
        }

        private long _AbstinenceID;
        public long AbstinenceID
        {
            get { return _AbstinenceID; }
            set
            {
                if (_AbstinenceID != value)
                {
                    _AbstinenceID = value;
                    OnPropertyChanged("AbstinenceID");
                }
            }
        }

        private DateTime? _CollectionTime;
        public DateTime? CollectionTime
        {
            get { return _CollectionTime; }
            set
            {
                if (_CollectionTime != value)
                {
                    _CollectionTime = value;
                    OnPropertyChanged("CollectionTime");
                }
            }
        }


        private DateTime? _TimeRecSampLab;
        public DateTime? TimeRecSampLab
        {
            get { return _TimeRecSampLab; }
            set
            {
                if (_TimeRecSampLab != value)
                {
                    _TimeRecSampLab = value;
                    OnPropertyChanged("TimeRecSampLab");
                }
            }
        }

        public bool Complete { get; set; }

        public bool CollecteAtCentre { get; set; }

        public bool IsFrozenSample { get; set; }       

        private long _ColorID;
        public long ColorID
        {
            get { return _ColorID; }
            set
            {
                if (_ColorID != value)
                {
                    _ColorID = value;
                    OnPropertyChanged("ColorID");
                }
            }
        }


        private float _Quantity;
        public float Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }

        private float _PH;
        public float PH
        {
            get { return _PH; }
            set
            {
                if (_PH != value)
                {
                    _PH = value;
                    OnPropertyChanged("PH");
                }
            }
        }


        private string _LiquificationTime;
        public string LiquificationTime
        {
            get { return _LiquificationTime; }
            set
            {
                if (_LiquificationTime != value)
                {
                    _LiquificationTime = value;
                    OnPropertyChanged("LiquificationTime");
                }
            }
        }

        public bool Viscosity { get; set; }

        private long _RangeViscosityID;
        public long RangeViscosityID
        {
            get { return _RangeViscosityID; }
            set
            {
                if (_RangeViscosityID != value)
                {
                    _RangeViscosityID = value;
                    OnPropertyChanged("RangeViscosityID");
                }
            }
        }

        public bool Odour { get; set; }


        private float _PreSpermCount;
        public float PreSpermCount
        {
            get { return _PreSpermCount; }
            set
            {
                if (_PreSpermCount != value)
                {
                    _PreSpermCount = value;
                    OnPropertyChanged("PreSpermCount");
                }
            }
        }

        private float _PreTotalSpermCount;
        public float PreTotalSpermCount
        {
            get { return _PreTotalSpermCount; }
            set
            {
                if (_PreTotalSpermCount != value)
                {
                    _PreTotalSpermCount = value;
                    OnPropertyChanged("PreTotalSpermCount");
                }
            }
        }

       //added by neena
        private string _AllComments;
        public string AllComments
        {
            get { return _AllComments; }
            set
            {
                if (_AllComments != value)
                {
                    _AllComments = value;
                    OnPropertyChanged("AllComments");
                }
            }
        }

        private float _Sperm5thPercentile;
        public float Sperm5thPercentile
        {
            get { return _Sperm5thPercentile; }
            set
            {
                if (_Sperm5thPercentile != value)
                {
                    _Sperm5thPercentile = value;
                    OnPropertyChanged("Sperm5thPercentile");
                }
            }
        }

        private float _Sperm75thPercentile;
        public float Sperm75thPercentile
        {
            get { return _Sperm75thPercentile; }
            set
            {
                if (_Sperm75thPercentile != value)
                {
                    _Sperm75thPercentile = value;
                    OnPropertyChanged("Sperm75thPercentile");
                }
            }
        }

        private float _Ejaculate5thPercentile;
        public float Ejaculate5thPercentile
        {
            get { return _Ejaculate5thPercentile; }
            set
            {
                if (_Ejaculate5thPercentile != value)
                {
                    _Ejaculate5thPercentile = value;
                    OnPropertyChanged("Ejaculate5thPercentile");
                }
            }
        }

        private float _Ejaculate75thPercentile;
        public float Ejaculate75thPercentile
        {
            get { return _Ejaculate75thPercentile; }
            set
            {
                if (_Ejaculate75thPercentile != value)
                {
                    _Ejaculate75thPercentile = value;
                    OnPropertyChanged("Ejaculate75thPercentile");
                }
            }
        }

        private float _TotalMotility5thPercentile;
        public float TotalMotility5thPercentile
        {
            get { return _TotalMotility5thPercentile; }
            set
            {
                if (_TotalMotility5thPercentile != value)
                {
                    _TotalMotility5thPercentile = value;
                    OnPropertyChanged("TotalMotility5thPercentile");
                }
            }
        }

        private float _TotalMotility75thPercentile;
        public float TotalMotility75thPercentile
        {
            get { return _TotalMotility75thPercentile; }
            set
            {
                if (_TotalMotility75thPercentile != value)
                {
                    _TotalMotility75thPercentile = value;
                    OnPropertyChanged("TotalMotility75thPercentile");
                }
            }
        }

       //

        private float _PreMotility;
        public float PreMotility
        {
            get { return _PreMotility; }
            set
            {
                if (_PreMotility != value)
                {
                    _PreMotility = value;
                    OnPropertyChanged("PreMotility");
                }
            }
        }

        private float _PreNonMotility;
        public float PreNonMotility
        {
            get { return _PreNonMotility; }
            set
            {
                if (_PreNonMotility != value)
                {
                    _PreNonMotility = value;
                    OnPropertyChanged("PreNonMotility");
                }
            }
        }

        private float _PreTotalMotility;
        public float PreTotalMotility
        {
            get { return _PreTotalMotility; }
            set
            {
                if (_PreTotalMotility != value)
                {
                    _PreTotalMotility = value;
                    OnPropertyChanged("PreTotalMotility");
                }
            }
        }

        private float _PreMotilityGradeI;
        public float PreMotilityGradeI
        {
            get { return _PreMotilityGradeI; }
            set
            {
                if (_PreMotilityGradeI != value)
                {
                    _PreMotilityGradeI = value;
                    OnPropertyChanged("PreMotilityGradeI");
                }
            }
        }

        private float _PreMotilityGradeII;
        public float PreMotilityGradeII
        {
            get { return _PreMotilityGradeII; }
            set
            {
                if (_PreMotilityGradeII != value)
                {
                    _PreMotilityGradeII = value;
                    OnPropertyChanged("PreMotilityGradeII");
                }
            }
        }

        private float _PreMotilityGradeIII;
        public float PreMotilityGradeIII
        {
            get { return _PreMotilityGradeIII; }
            set
            {
                if (_PreMotilityGradeIII != value)
                {
                    _PreMotilityGradeIII = value;
                    OnPropertyChanged("PreMotilityGradeIII");
                }
            }
        }

        private float _PreMotilityGradeIV;
        public float PreMotilityGradeIV
        {
            get { return _PreMotilityGradeIV; }
            set
            {
                if (_PreMotilityGradeIV != value)
                {
                    _PreMotilityGradeIV = value;
                    OnPropertyChanged("PreMotilityGradeIV");
                }
            }
        }

        private float _PreNormalMorphology;
        public float PreNormalMorphology
        {
            get { return _PreNormalMorphology; }
            set
            {
                if (_PreNormalMorphology != value)
                {
                    _PreNormalMorphology = value;
                    OnPropertyChanged("PreNormalMorphology");
                }
            }
        }


       // Added by as per the changes in Milann 

        private float _PreAmount;
        public float PreAmount
        {
            get { return _PreAmount; }
            set
            {
                if (_PreAmount != value)
                {
                    _PreAmount = value;
                    OnPropertyChanged("PreAmount");       
                }
            }
        }


        private float _PreNormalForms;
        public float PreNormalForms
        {
            get { return _PreNormalForms; }
            set
            {
                if (_PreNormalForms != value)
                {
                    _PreNormalForms = value;
                    OnPropertyChanged("PreNormalForms");
                }
            }
        }

        private float _PreProgMotility;
        public float PreProgMotility
        {
            get { return _PreProgMotility; }
            set
            {
                if (_PreProgMotility != value)
                {
                    _PreProgMotility = value;
                    OnPropertyChanged("PreProgMotility");
                }
            }
        }


        private float _PostNormalForms;
        public float PostNormalForms
        {
            get { return _PostNormalForms; }
            set
            {
                if (_PostNormalForms != value)
                {
                    _PostNormalForms = value;
                    OnPropertyChanged("PostNormalForms");
                }
            }
        }

        private float _PostAmount;
        public float PostAmount
        {
            get { return _PostAmount; }
            set
            {
                if (_PostAmount != value)
                {
                    _PostAmount = value;
                    OnPropertyChanged("PostAmount");
                }
            }
        }
        private float _PostProgMotility;
        public float PostProgMotility
        {
            get { return _PostProgMotility; }
            set
            {
                if (_PostProgMotility != value)
                {
                    _PostProgMotility = value;
                    OnPropertyChanged("PostProgMotility");
                }
            }
        }

        public string Cyclecode { get; set; }
        public string CycleDuration { get; set; }

        private DateTime? _TherapyStartDate = null;
        public DateTime? TherapyStartDate
        {
            get
            {
                return _TherapyStartDate;
            }
            set
            {
                _TherapyStartDate = value;
            }
        }

        public string PlannedTreatment { get; set; }

        private DateTime? _PreperationDate = null;
        public DateTime? PreperationDate
        {
            get
            {
                return _PreperationDate;
            }
            set
            {
                _PreperationDate = value;
            }
        }
       

        public string Inseminated{get; set; }
        public string MotileSperm { get; set; }
        public long MainInductionID { get; set; }
        public long ExternalSimulationID { get; set; }
        public long PlannedTreatmentID { get; set; }
        public long FinalPlannedTreatmentID { get; set; }

       //added by neena
        private float _PreNonProgressive;
        public float PreNonProgressive
        {
            get { return _PreNonProgressive; }
            set
            {
                if (_PreNonProgressive != value)
                {
                    _PreNonProgressive = value;
                    OnPropertyChanged("PreNonProgressive");
                }
            }
        }

        private float _PostNonProgressive;
        public float PostNonProgressive
        {
            get { return _PostNonProgressive; }
            set
            {
                if (_PostNonProgressive != value)
                {
                    _PostNonProgressive = value;
                    OnPropertyChanged("PostNonProgressive");
                }
            }
        }

        private float _PreNonMotile;
        public float PreNonMotile
        {
            get { return _PreNonMotile; }
            set
            {
                if (_PreNonMotile != value)
                {
                    _PreNonMotile = value;
                    OnPropertyChanged("PreNonMotile");
                }
            }
        }

        private float _PostNonMotile;
        public float PostNonMotile
        {
            get { return _PostNonMotile; }
            set
            {
                if (_PostNonMotile != value)
                {
                    _PostNonMotile = value;
                    OnPropertyChanged("PostNonMotile");
                }
            }
        }

        private float _PreMotileSpermCount;
        public float PreMotileSpermCount
        {
            get { return _PreMotileSpermCount; }
            set
            {
                if (_PreMotileSpermCount != value)
                {
                    _PreMotileSpermCount = value;
                    OnPropertyChanged("PreMotileSpermCount");
                }
            }
        }

        private float _PostMotileSpermCount;
        public float PostMotileSpermCount
        {
            get { return _PostMotileSpermCount; }
            set
            {
                if (_PostMotileSpermCount != value)
                {
                    _PostMotileSpermCount = value;
                    OnPropertyChanged("PostMotileSpermCount");
                }
            }
        }
       //

       //End 


        //private float _PrePusCells;
        //public float PrePusCells
        //{
        //    get { return _PrePusCells; }
        //    set
        //    {
        //        if (_PrePusCells != value)
        //        {
        //            _PrePusCells = value;
        //            OnPropertyChanged("PrePusCells");
        //        }
        //    }
        //}

        //private float _PreRoundCells;
        //public float PreRoundCells
        //{
        //    get { return _PreRoundCells; }
        //    set
        //    {
        //        if (_PreRoundCells != value)
        //        {
        //            _PreRoundCells = value;
        //            OnPropertyChanged("PreRoundCells");
        //        }
        //    }
        //}

        //private float _PreEpithelialCells;
        //public float PreEpithelialCells
        //{
        //    get { return _PreEpithelialCells; }
        //    set
        //    {
        //        if (_PreEpithelialCells != value)
        //        {
        //            _PreEpithelialCells = value;
        //            OnPropertyChanged("PreEpithelialCells");
        //        }
        //    }
        //}

        //private long _PreCheckedByID;
        //public long PreCheckedByID
        //{
        //    get { return _PreCheckedByID; }
        //    set
        //    {
        //        if (_PreCheckedByID != value)
        //        {
        //            _PreCheckedByID = value;
        //            OnPropertyChanged("PreCheckedByID");
        //        }
        //    }
        //}

       // POST Details 

        private float _PostSpermCount;
        public float PostSpermCount
        {
            get { return _PostSpermCount; }
            set
            {
                if (_PostSpermCount != value)
                {
                    _PostSpermCount = value;
                    OnPropertyChanged("PostSpermCount");
                }
            }
        }

        private float _PostTotalSpermCount;
        public float PostTotalSpermCount
        {
            get { return _PostTotalSpermCount; }
            set
            {
                if (_PostTotalSpermCount != value)
                {
                    _PostTotalSpermCount = value;
                    OnPropertyChanged("PostTotalSpermCount");
                }
            }
        }

        private float _PostMotility;
        public float PostMotility
        {
            get { return _PostMotility; }
            set
            {
                if (_PostMotility != value)
                {
                    _PostMotility = value;
                    OnPropertyChanged("PostMotility");
                }
            }
        }

        private float _PostNonMotility;
        public float PostNonMotility
        {
            get { return _PostNonMotility; }
            set
            {
                if (_PostNonMotility != value)
                {
                    _PostNonMotility = value;
                    OnPropertyChanged("PostNonMotility");
                }
            }
        }

        private float _PostTotalMotility;
        public float PostTotalMotility
        {
            get { return _PostTotalMotility; }
            set
            {
                if (_PostTotalMotility != value)
                {
                    _PostTotalMotility = value;
                    OnPropertyChanged("PostTotalMotility");
                }
            }
        }

        private float _PostMotilityGradeI;
        public float PostMotilityGradeI
        {
            get { return _PostMotilityGradeI; }
            set
            {
                if (_PostMotilityGradeI != value)
                {
                    _PostMotilityGradeI = value;
                    OnPropertyChanged("PostMotilityGradeI");
                }
            }
        }

        private float _PostMotilityGradeII;
        public float PostMotilityGradeII
        {
            get { return _PostMotilityGradeII; }
            set
            {
                if (_PostMotilityGradeII != value)
                {
                    _PostMotilityGradeII = value;
                    OnPropertyChanged("PostMotilityGradeII");
                }
            }
        }

        private float _PostMotilityGradeIII;
        public float PostMotilityGradeIII
        {
            get { return _PostMotilityGradeIII; }
            set
            {
                if (_PostMotilityGradeIII != value)
                {
                    _PostMotilityGradeIII = value;
                    OnPropertyChanged("PostMotilityGradeIII");
                }
            }
        }

        private float _PostMotilityGradeIV;
        public float PostMotilityGradeIV
        {
            get { return _PostMotilityGradeIV; }
            set
            {
                if (_PostMotilityGradeIV != value)
                {
                    _PostMotilityGradeIV = value;
                    OnPropertyChanged("PostMotilityGradeIV");
                }
            }
        }

        private float _PostNormalMorphology;
        public float PostNormalMorphology
        {
            get { return _PostNormalMorphology; }
            set
            {
                if (_PostNormalMorphology != value)
                {
                    _PostNormalMorphology = value;
                    OnPropertyChanged("PostNormalMorphology");
                }
            }
        }

       // Other Properties

        private string _PusCells;
        public string PusCells
        {
            get { return _PusCells; }
            set
            {
                if (_PusCells != value)
                {
                    _PusCells = value;
                    OnPropertyChanged("PusCells");
                }
            }
        }

        private string _RoundCells;
        public string RoundCells
        {
            get { return _RoundCells; }
            set
            {
                if (_RoundCells != value)
                {
                    _RoundCells = value;
                    OnPropertyChanged("RoundCells");
                }
            }
        }

        private string _EpithelialCells;
        public string EpithelialCells
        {
            get { return _EpithelialCells; }
            set
            {
                if (_EpithelialCells != value)
                {
                    _EpithelialCells = value;
                    OnPropertyChanged("EpithelialCells");
                }
            }
        }

        private string _AnyOtherCells;
        public string AnyOtherCells
        {
            get { return _AnyOtherCells; }
            set
            {
                if (_AnyOtherCells != value)
                {
                    _AnyOtherCells = value;
                    OnPropertyChanged("AnyOtherCells");
                }
            }
        }
        private long _CheckedByDoctorID;
        public long CheckedByDoctorID
        {
            get { return _CheckedByDoctorID; }
            set
            {
                if (_CheckedByDoctorID != value)
                {
                    _CheckedByDoctorID = value;
                    OnPropertyChanged("CheckedByDoctorID");
                }
            }
        }



        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set
            {
                if (_Comment != value)
                {
                    _Comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }


        private long _CommentID;
        public long CommentID
        {
            get { return _CommentID; }
            set
            {
                if (_CommentID != value)
                {
                    _CommentID = value;
                    OnPropertyChanged("CommentID");
                }
            }
        }

      //IUI Properties        

        private DateTime? _IUIDate;
        public DateTime? IUIDate
        {
            get { return _IUIDate; }
            set
            {
                if (_IUIDate != value)
                {
                    _IUIDate = value;
                    OnPropertyChanged("IUIDate");
                }
            }
        }

		private long _InSeminatedByID;
        public long InSeminatedByID
        {
            get { return _InSeminatedByID; }
            set
            {
                if (_InSeminatedByID != value)
                {
                    _InSeminatedByID = value;
                    OnPropertyChanged("InSeminatedByID");
                }
            }
        }


	private long _InSeminationLocationID;
        public long InSeminationLocationID
        {
            get { return _InSeminationLocationID; }
            set
            {
                if (_InSeminationLocationID != value)
                {
                    _InSeminationLocationID = value;
                    OnPropertyChanged("InSeminationLocationID");
                }
            }
        }

	private long _WitnessByID;
        public long WitnessByID
        {
            get { return _WitnessByID; }
            set
            {
                if (_WitnessByID != value)
                {
                    _WitnessByID = value;
                    OnPropertyChanged("WitnessByID");
                }
            }
        }

	
       private long _InSeminationMethodID;
        public long InSeminationMethodID
        {
            get { return _InSeminationMethodID; }
            set
            {
                if (_InSeminationMethodID != value)
                {
                    _InSeminationMethodID = value;
                    OnPropertyChanged("InSeminationMethodID");
                }
            }
        }

	 private DateTime? _ThawDate;
        public DateTime? ThawDate
        {
            get { return _ThawDate; }
            set
            {
                if (_ThawDate != value)
                {
                    _ThawDate = value;
                    OnPropertyChanged("ThawDate");
                }
            }
        }




        private string _SampleID;
        public string SampleID
        {
            get { return _SampleID; }
            set
            {
                if (_SampleID != value)
                {
                    _SampleID = value;
                    OnPropertyChanged("SampleID");
                }
            }
        }


     private long _DonorID;
        public long DonorID
        {
            get { return _DonorID; }
            set
            {
                if (_DonorID != value)
                {
                    _DonorID = value;
                    OnPropertyChanged("DonorID");
                }
            }
        }

       private long _DonorUnitID;
        public long DonorUnitID
        {
            get { return _DonorUnitID; }
            set
            {
                if (_DonorUnitID != value)
                {
                    _DonorUnitID = value;
                    OnPropertyChanged("DonorUnitID");
                }
            }
        }

        private long _PlanTherapyUnitID;
        public long PlanTherapyUnitID
        {
            get { return _PlanTherapyUnitID; }
            set
            {
                if (_PlanTherapyUnitID != value)
                {
                    _PlanTherapyUnitID = value;
                    OnPropertyChanged("PlanTherapyUnitID");
                }
            }
        }
        private long _PlanTherapyID;
        public long PlanTherapyID
        {
            get { return _PlanTherapyID; }
            set
            {
                if (_PlanTherapyID != value)
                {
                    _PlanTherapyID = value;
                    OnPropertyChanged("PlanTherapyID");
                }
            }
        }
        private long _BatchID;
        public long BatchID
        {
            get { return _BatchID; }
            set
            {
                if (_BatchID != value)
                {
                    _BatchID = value;
                    OnPropertyChanged("BatchID");
                }
            }
        }
        private long _BatchUnitID;
        public long BatchUnitID
        {
            get { return _BatchUnitID; }
            set
            {
                if (_BatchUnitID != value)
                {
                    _BatchUnitID = value;
                    OnPropertyChanged("BatchUnitID");
                }
            }
        }
        private string _BatchCode;
        public string BatchCode
        {
            get { return _BatchCode; }
            set
            {
                if (_BatchCode != value)
                {
                    _BatchCode = value;
                    OnPropertyChanged("BatchCode");
                }
            }
        }
    #region andrologist 
        // add by Devidas 9/Mar/2017 

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

        private string _Spillage;
        public string Spillage
        {
            get { return _Spillage; }
            set
            {
                if (_Spillage != value)
                {
                    _Spillage = value;
                    OnPropertyChanged("Spillage");
                }
            }
        }
        private long _SemenProcessingMethodID;
        public long SemenProcessingMethodID
        {
            get { return _SemenProcessingMethodID; }
            set
            {
                if (_SemenProcessingMethodID != value)
                {
                    _SemenProcessingMethodID = value;
                    OnPropertyChanged("SemenProcessingId");
                }
            }
        }

        private string _MediaUsed;
        public string MediaUsed
        {
            get { return _MediaUsed; }
            set
            {
                if (_MediaUsed != value)
                {
                    _MediaUsed = value;
                    OnPropertyChanged("MediaUsed");
                }
            }
        }
        private string _SpermRecoveredFrom;
        public string SpermRecoveredFrom
        {
            get { return _SpermRecoveredFrom; }
            set
            {
                if (_SpermRecoveredFrom != value)
                {
                    _SpermRecoveredFrom = value;
                    OnPropertyChanged("SpermRecoveredFrom");
                }
            }
        }
        private long _BloodGroupID;
        public long BloodGroupID
        {
            get { return _BloodGroupID; }
            set
            {
                if (_BloodGroupID != value)
                {
                    _BloodGroupID = value;
                    OnPropertyChanged("BloodGroupID");
                }
            }
        }
        private long _TransacationTypeID;
        public long TransacationTypeID
        {
            get { return _TransacationTypeID; }
            set
            {
                if (_TransacationTypeID != value)
                {
                    _TransacationTypeID = value;
                    OnPropertyChanged("TransacationTypeID");
                }
            }
        }
        private long _HIV;
        public long HIV
        {
            get { return _HIV; }
            set
            {
                if (_HIV != value)
                {
                    _HIV = value;
                    OnPropertyChanged("HIV");
                }
            }
        }
        private long _HBSAG;
        public long HBSAG
        {
            get { return _HBSAG; }
            set
            {
                if (_HBSAG != value)
                {
                    _HBSAG = value;
                    OnPropertyChanged("HBSAG");
                }
            }
        }

        private long _HCV;
        public long HCV
        {
            get { return _HCV; }
            set
            {
                if (_HCV != value)
                {
                    _HCV = value;
                    OnPropertyChanged("HCV");
                }
            }
        }

        private long _VDRL;
        public long VDRL
        {
            get { return _VDRL; }
            set
            {
                if (_VDRL != value)
                {
                    _VDRL = value;
                    OnPropertyChanged("VDRL");
                }
            }
        }
        private string _Andrologist;
        public string Andrologist
        {
            get { return _Andrologist; }
            set
            {
                if (_Andrologist != value)
                {
                    _Andrologist = value;
                    OnPropertyChanged("Andrologist");
                }
            }
        }

        private string _Interpretations;
        public string Interpretations
        {
            get { return _Interpretations; }
            set
            {
                if (_Interpretations != value)
                {
                    _Interpretations = value;
                    OnPropertyChanged("Interpretations");
                }
            }
        }

       #endregion
      public bool ISFromIUI { get; set; }
        //private string _Infections;
        //public string Infections
        //{
        //    get { return _Infections; }
        //    set
        //    {
        //        if (_Infections != value)
        //        {
        //            _Infections = value;
        //            OnPropertyChanged("Infections");
        //        }
        //    }
        //}


        //private string _OtherFindings;
        //public string OtherFindings
        //{
        //    get { return _OtherFindings; }
        //    set
        //    {
        //        if (_OtherFindings != value)
        //        {
        //            _OtherFindings = value;
        //            OnPropertyChanged("OtherFindings");
        //        }
        //    }
        //}

        //private string _Interpretations;
        //public string Interpretations
        //{
        //    get { return _Interpretations; }
        //    set
        //    {
        //        if (_Interpretations != value)
        //        {
        //            _Interpretations = value;
        //            OnPropertyChanged("Interpretations");
        //        }
        //    }
        //}
        //private float _Amorphus;
        //public float Amorphus
        //{
        //    get { return _Amorphus; }
        //    set
        //    {
        //        if (_Amorphus != value)
        //        {
        //            _Amorphus = value;
        //            OnPropertyChanged("Amorphus");
        //        }
        //    }
        //}

        //private float _NeckAppendages;
        //public float NeckAppendages
        //{
        //    get { return _NeckAppendages; }
        //    set
        //    {
        //        if (_NeckAppendages != value)
        //        {
        //            _NeckAppendages = value;
        //            OnPropertyChanged("NeckAppendages");
        //        }
        //    }
        //}

        //private float _Pyriform;
        //public float Pyriform
        //{
        //    get { return _Pyriform; }
        //    set
        //    {
        //        if (_Pyriform != value)
        //        {
        //            _Pyriform = value;
        //            OnPropertyChanged("Pyriform");
        //        }
        //    }
        //}


        //private float _Macrocefalic;
        //public float Macrocefalic
        //{
        //    get { return _Macrocefalic; }
        //    set
        //    {
        //        if (_Macrocefalic != value)
        //        {
        //            _Macrocefalic = value;
        //            OnPropertyChanged("Macrocefalic");
        //        }
        //    }
        //}
        //private float _Microcefalic;
        //public float Microcefalic
        //{
        //    get { return _Microcefalic; }
        //    set
        //    {
        //        if (_Microcefalic != value)
        //        {
        //            _Microcefalic = value;
        //            OnPropertyChanged("Microcefalic");
        //        }
        //    }
        //}

        //private float _BrockenNeck;
        //public float BrockenNeck
        //{
        //    get { return _BrockenNeck; }
        //    set
        //    {
        //        if (_BrockenNeck != value)
        //        {
        //            _BrockenNeck = value;
        //            OnPropertyChanged("BrockenNeck");
        //        }
        //    }
        //}

        //private float _RoundHead;
        //public float RoundHead
        //{
        //    get { return _RoundHead; }
        //    set
        //    {
        //        if (_RoundHead != value)
        //        {
        //            _RoundHead = value;
        //            OnPropertyChanged("RoundHead");
        //        }
        //    }
        //}

        //private float _DoubleHead;
        //public float DoubleHead
        //{
        //    get { return _DoubleHead; }
        //    set
        //    {
        //        if (_DoubleHead != value)
        //        {
        //            _DoubleHead = value;
        //            OnPropertyChanged("DoubleHead");
        //        }
        //    }
        //}

        //private float _Total;
        //public float Total
        //{
        //    get { return _Total; }
        //    set
        //    {
        //        if (_Total != value)
        //        {
        //            _Total = value;
        //            OnPropertyChanged("Total");
        //        }
        //    }
        //}

        //private float _MorphologicalAbnormilities;
        //public float MorphologicalAbnormilities
        //{
        //    get { return _MorphologicalAbnormilities; }
        //    set
        //    {
        //        if (_MorphologicalAbnormilities != value)
        //        {
        //            _MorphologicalAbnormilities = value;
        //            OnPropertyChanged("MorphologicalAbnormilities");
        //        }
        //    }
        //}



        //private float _CytoplasmicDroplet;
        //public float CytoplasmicDroplet
        //{
        //    get { return _CytoplasmicDroplet; }
        //    set
        //    {
        //        if (_CytoplasmicDroplet != value)
        //        {
        //            _CytoplasmicDroplet = value;
        //            OnPropertyChanged("CytoplasmicDroplet");
        //        }
        //    }
        //}


        //private float _Others;
        //public float Others
        //{
        //    get { return _Others; }
        //    set
        //    {
        //        if (_Others != value)
        //        {
        //            _Others = value;
        //            OnPropertyChanged("Others");
        //        }
        //    }
        //}

        //private float _MidPieceTotal;
        //public float MidPieceTotal
        //{
        //    get { return _MidPieceTotal; }
        //    set
        //    {
        //        if (_MidPieceTotal != value)
        //        {
        //            _MidPieceTotal = value;
        //            OnPropertyChanged("MidPieceTotal");
        //        }
        //    }
        //}

        //private float _CoiledTail;
        //public float CoiledTail
        //{
        //    get { return _CoiledTail; }
        //    set
        //    {
        //        if (_CoiledTail != value)
        //        {
        //            _CoiledTail = value;
        //            OnPropertyChanged("CoiledTail");
        //        }
        //    }
        //}


        //private float _ShortTail;
        //public float ShortTail
        //{
        //    get { return _ShortTail; }
        //    set
        //    {
        //        if (_ShortTail != value)
        //        {
        //            _ShortTail = value;
        //            OnPropertyChanged("ShortTail");
        //        }
        //    }
        //}


        //private float _HairpinTail;
        //public float HairpinTail
        //{
        //    get { return _HairpinTail; }
        //    set
        //    {
        //        if (_HairpinTail != value)
        //        {
        //            _HairpinTail = value;
        //            OnPropertyChanged("HairpinTail");
        //        }
        //    }
        //}


        //private float _DoubleTail;
        //public float DoubleTail
        //{
        //    get { return _DoubleTail; }
        //    set
        //    {
        //        if (_DoubleTail != value)
        //        {
        //            _DoubleTail = value;
        //            OnPropertyChanged("DoubleTail");
        //        }
        //    }
        //}

        //private float _TailOthers;
        //public float TailOthers
        //{
        //    get { return _TailOthers; }
        //    set
        //    {
        //        if (_TailOthers != value)
        //        {
        //            _TailOthers = value;
        //            OnPropertyChanged("TailOthers");
        //        }
        //    }
        //}

        //private float _TailTotal;
        //public float TailTotal
        //{
        //    get { return _TailTotal; }
        //    set
        //    {
        //        if (_TailTotal != value)
        //        {
        //            _TailTotal = value;
        //            OnPropertyChanged("TailTotal");
        //        }
        //    }
        //}

        //private string _HeadToHead;
        //public string HeadToHead
        //{
        //    get { return _HeadToHead; }
        //    set
        //    {
        //        if (_HeadToHead != value)
        //        {
        //            _HeadToHead = value;
        //            OnPropertyChanged("HeadToHead");
        //        }
        //    }
        //}

        //private string _TailToTail;
        //public string TailToTail
        //{
        //    get { return _TailToTail; }
        //    set
        //    {
        //        if (_TailToTail != value)
        //        {
        //            _TailToTail = value;
        //            OnPropertyChanged("TailToTail");
        //        }
        //    }
        //}
       

        //private string _HeadToTail;
        //public string HeadToTail
        //{
        //    get { return _HeadToTail; }
        //    set
        //    {
        //        if (_HeadToTail != value)
        //        {
        //            _HeadToTail = value;
        //            OnPropertyChanged("HeadToTail");
        //        }
        //    }
        //}

        //private string _SpermToOther;
        //public string SpermToOther
        //{
        //    get { return _SpermToOther; }
        //    set
        //    {
        //        if (_SpermToOther != value)
        //        {
        //            _SpermToOther = value;
        //            OnPropertyChanged("SpermToOther");
        //        }
        //    }
        //}
        #endregion

    }
}
