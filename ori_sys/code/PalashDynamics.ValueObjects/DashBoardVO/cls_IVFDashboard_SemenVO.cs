using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class cls_IVFDashboard_SemenVO : IValueObject, INotifyPropertyChanged
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

        private float _SpermCount;
        public float SpermCount
        {
            get { return _SpermCount; }
            set
            {
                if (_SpermCount != value)
                {
                    _SpermCount = value;
                    OnPropertyChanged("SpermCount");
                }
            }
        }

        private float _TotalSpermCount;
        public float TotalSpermCount
        {
            get { return _TotalSpermCount; }
            set
            {
                if (_TotalSpermCount != value)
                {
                    _TotalSpermCount = value;
                    OnPropertyChanged("TotalSpermCount");
                }
            }
        }

        private float _Motility;
        public float Motility
        {
            get { return _Motility; }
            set
            {
                if (_Motility != value)
                {
                    _Motility = value;
                    OnPropertyChanged("Motility");
                }
            }
        }

        private float _NonMotility;
        public float NonMotility
        {
            get { return _NonMotility; }
            set
            {
                if (_NonMotility != value)
                {
                    _NonMotility = value;
                    OnPropertyChanged("NonMotility");
                }
            }
        }

        private float _TotalMotility;
        public float TotalMotility
        {
            get { return _TotalMotility; }
            set
            {
                if (_TotalMotility != value)
                {
                    _TotalMotility = value;
                    OnPropertyChanged("TotalMotility");
                }
            }
        }

        private float _MotilityGradeI;
        public float MotilityGradeI
        {
            get { return _MotilityGradeI; }
            set
            {
                if (_MotilityGradeI != value)
                {
                    _MotilityGradeI = value;
                    OnPropertyChanged("MotilityGradeI");
                }
            }
        }

        private float _MotilityGradeII;
        public float MotilityGradeII
        {
            get { return _MotilityGradeII; }
            set
            {
                if (_MotilityGradeII != value)
                {
                    _MotilityGradeII = value;
                    OnPropertyChanged("MotilityGradeII");
                }
            }
        }

        private float _MotilityGradeIII;
        public float MotilityGradeIII
        {
            get { return _MotilityGradeIII; }
            set
            {
                if (_MotilityGradeIII != value)
                {
                    _MotilityGradeIII = value;
                    OnPropertyChanged("MotilityGradeIII");
                }
            }
        }

        private float _MotilityGradeIV;
        public float MotilityGradeIV
        {
            get { return _MotilityGradeIV; }
            set
            {
                if (_MotilityGradeIV != value)
                {
                    _MotilityGradeIV = value;
                    OnPropertyChanged("MotilityGradeIV");
                }
            }
        }

        private float _Amorphus;
        public float Amorphus
        {
            get { return _Amorphus; }
            set
            {
                if (_Amorphus != value)
                {
                    _Amorphus = value;
                    OnPropertyChanged("Amorphus");
                }
            }
        }

        private float _NeckAppendages;
        public float NeckAppendages
        {
            get { return _NeckAppendages; }
            set
            {
                if (_NeckAppendages != value)
                {
                    _NeckAppendages = value;
                    OnPropertyChanged("NeckAppendages");
                }
            }
        }

        private float _Pyriform;
        public float Pyriform
        {
            get { return _Pyriform; }
            set
            {
                if (_Pyriform != value)
                {
                    _Pyriform = value;
                    OnPropertyChanged("Pyriform");
                }
            }
        }


        private float _Macrocefalic;
        public float Macrocefalic
        {
            get { return _Macrocefalic; }
            set
            {
                if (_Macrocefalic != value)
                {
                    _Macrocefalic = value;
                    OnPropertyChanged("Macrocefalic");
                }
            }
        }

        private float _Microcefalic;
        public float Microcefalic
        {
            get { return _Microcefalic; }
            set
            {
                if (_Microcefalic != value)
                {
                    _Microcefalic = value;
                    OnPropertyChanged("Microcefalic");
                }
            }
        }

        private float _BrockenNeck;
        public float BrockenNeck
        {
            get { return _BrockenNeck; }
            set
            {
                if (_BrockenNeck != value)
                {
                    _BrockenNeck = value;
                    OnPropertyChanged("BrockenNeck");
                }
            }
        }

        private float _RoundHead;
        public float RoundHead
        {
            get { return _RoundHead; }
            set
            {
                if (_RoundHead != value)
                {
                    _RoundHead = value;
                    OnPropertyChanged("RoundHead");
                }
            }
        }

        private float _DoubleHead;
        public float DoubleHead
        {
            get { return _DoubleHead; }
            set
            {
                if (_DoubleHead != value)
                {
                    _DoubleHead = value;
                    OnPropertyChanged("DoubleHead");
                }
            }
        }

        private float _Total;
        public float Total
        {
            get { return _Total; }
            set
            {
                if (_Total != value)
                {
                    _Total = value;
                    OnPropertyChanged("Total");
                }
            }
        }

        private float _MorphologicalAbnormilities;
        public float MorphologicalAbnormilities
        {
            get { return _MorphologicalAbnormilities; }
            set
            {
                if (_MorphologicalAbnormilities != value)
                {
                    _MorphologicalAbnormilities = value;
                    OnPropertyChanged("MorphologicalAbnormilities");
                }
            }
        }

        private float _NormalMorphology;
        public float NormalMorphology
        {
            get { return _NormalMorphology; }
            set
            {
                if (_NormalMorphology != value)
                {
                    _NormalMorphology = value;
                    OnPropertyChanged("NormalMorphology");
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

        private float _SpermMorphology5thPercentile;
        public float SpermMorphology5thPercentile
        {
            get { return _SpermMorphology5thPercentile; }
            set
            {
                if (_SpermMorphology5thPercentile != value)
                {
                    _SpermMorphology5thPercentile = value;
                    OnPropertyChanged("SpermMorphology5thPercentile");
                }
            }
        }

        private float _SpermMorphology75thPercentile;
        public float SpermMorphology75thPercentile
        {
            get { return _SpermMorphology75thPercentile; }
            set
            {
                if (_SpermMorphology75thPercentile != value)
                {
                    _SpermMorphology75thPercentile = value;
                    OnPropertyChanged("SpermMorphology75thPercentile");
                }
            }
        }

        private float _RapidProgressive;
        public float RapidProgressive
        {
            get { return _RapidProgressive; }
            set
            {
                if (_RapidProgressive != value)
                {
                    _RapidProgressive = value;
                    OnPropertyChanged("RapidProgressive");
                }
            }
        }

        private float _SlowProgressive;
        public float SlowProgressive
        {
            get { return _SlowProgressive; }
            set
            {
                if (_SlowProgressive != value)
                {
                    _SlowProgressive = value;
                    OnPropertyChanged("SlowProgressive");
                }
            }
        }

        private string _SpermMorphologySubNormal;
        public string SpermMorphologySubNormal
        {
            get { return _SpermMorphologySubNormal; }
            set
            {
                if (_SpermMorphologySubNormal != value)
                {
                    _SpermMorphologySubNormal = value;
                    OnPropertyChanged("SpermMorphologySubNormal");
                }
            }
        }

        private string _NormalFormsComments;
        public string NormalFormsComments
        {
            get { return _NormalFormsComments; }
            set
            {
                if (_NormalFormsComments != value)
                {
                    _NormalFormsComments = value;
                    OnPropertyChanged("NormalFormsComments");
                }
            }
        }

        private string _OverAllDefectsComments;
        public string OverAllDefectsComments
        {
            get { return _OverAllDefectsComments; }
            set
            {
                if (_OverAllDefectsComments != value)
                {
                    _OverAllDefectsComments = value;
                    OnPropertyChanged("OverAllDefectsComments");
                }
            }
        }

        private string _HeadDefectsComments;
        public string HeadDefectsComments
        {
            get { return _HeadDefectsComments; }
            set
            {
                if (_HeadDefectsComments != value)
                {
                    _HeadDefectsComments = value;
                    OnPropertyChanged("HeadDefectsComments");
                }
            }
        }

        private string _MidPieceNeckDefectsComments;
        public string MidPieceNeckDefectsComments
        {
            get { return _MidPieceNeckDefectsComments; }
            set
            {
                if (_MidPieceNeckDefectsComments != value)
                {
                    _MidPieceNeckDefectsComments = value;
                    OnPropertyChanged("MidPieceNeckDefectsComments");
                }
            }
        }

        private string _TailDefectsComments;
        public string TailDefectsComments
        {
            get { return _TailDefectsComments; }
            set
            {
                if (_TailDefectsComments != value)
                {
                    _TailDefectsComments = value;
                    OnPropertyChanged("TailDefectsComments");
                }
            }
        }

        private string _ExcessiveResidualComments;
        public string ExcessiveResidualComments
        {
            get { return _ExcessiveResidualComments; }
            set
            {
                if (_ExcessiveResidualComments != value)
                {
                    _ExcessiveResidualComments = value;
                    OnPropertyChanged("ExcessiveResidualComments");
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

        private string _Fructose;
        public string Fructose
        {
            get { return _Fructose; }
            set
            {
                if (_Fructose != value)
                {
                    _Fructose = value;
                    OnPropertyChanged("Fructose");
                }
            }
        }

        private float _Live;
        public float Live
        {
            get { return _Live; }
            set
            {
                if (_Live != value)
                {
                    _Live = value;
                    OnPropertyChanged("Live");
                }
            }
        }

        private float _Dead;
        public float Dead
        {
            get { return _Dead; }
            set
            {
                if (_Dead != value)
                {
                    _Dead = value;
                    OnPropertyChanged("Dead");
                }
            }
        }

        private float _TotalAdvanceMotility;
        public float TotalAdvanceMotility
        {
            get { return _TotalAdvanceMotility; }
            set
            {
                if (_TotalAdvanceMotility != value)
                {
                    _TotalAdvanceMotility = value;
                    OnPropertyChanged("TotalAdvanceMotility");
                }
            }
        }

        private float _TotalAdvance5thPercentile;
        public float TotalAdvance5thPercentile
        {
            get { return _TotalAdvance5thPercentile; }
            set
            {
                if (_TotalAdvance5thPercentile != value)
                {
                    _TotalAdvance5thPercentile = value;
                    OnPropertyChanged("TotalAdvance5thPercentile");
                }
            }
        }

        private float _TotalAdvance75thPercentile;
        public float TotalAdvance75thPercentile
        {
            get { return _TotalAdvance75thPercentile; }
            set
            {
                if (_TotalAdvance75thPercentile != value)
                {
                    _TotalAdvance75thPercentile = value;
                    OnPropertyChanged("TotalAdvance75thPercentile");
                }
            }
        }
        //

        private float _CytoplasmicDroplet;
        public float CytoplasmicDroplet
        {
            get { return _CytoplasmicDroplet; }
            set
            {
                if (_CytoplasmicDroplet != value)
                {
                    _CytoplasmicDroplet = value;
                    OnPropertyChanged("CytoplasmicDroplet");
                }
            }
        }


        private float _Others;
        public float Others
        {
            get { return _Others; }
            set
            {
                if (_Others != value)
                {
                    _Others = value;
                    OnPropertyChanged("Others");
                }
            }
        }

        private float _MidPieceTotal;
        public float MidPieceTotal
        {
            get { return _MidPieceTotal; }
            set
            {
                if (_MidPieceTotal != value)
                {
                    _MidPieceTotal = value;
                    OnPropertyChanged("MidPieceTotal");
                }
            }
        }

        private float _CoiledTail;
        public float CoiledTail
        {
            get { return _CoiledTail; }
            set
            {
                if (_CoiledTail != value)
                {
                    _CoiledTail = value;
                    OnPropertyChanged("CoiledTail");
                }
            }
        }


        private float _ShortTail;
        public float ShortTail
        {
            get { return _ShortTail; }
            set
            {
                if (_ShortTail != value)
                {
                    _ShortTail = value;
                    OnPropertyChanged("ShortTail");
                }
            }
        }


        private float _HairpinTail;
        public float HairpinTail
        {
            get { return _HairpinTail; }
            set
            {
                if (_HairpinTail != value)
                {
                    _HairpinTail = value;
                    OnPropertyChanged("HairpinTail");
                }
            }
        }


        private float _DoubleTail;
        public float DoubleTail
        {
            get { return _DoubleTail; }
            set
            {
                if (_DoubleTail != value)
                {
                    _DoubleTail = value;
                    OnPropertyChanged("DoubleTail");
                }
            }
        }

        private float _TailOthers;
        public float TailOthers
        {
            get { return _TailOthers; }
            set
            {
                if (_TailOthers != value)
                {
                    _TailOthers = value;
                    OnPropertyChanged("TailOthers");
                }
            }
        }

        private float _TailTotal;
        public float TailTotal
        {
            get { return _TailTotal; }
            set
            {
                if (_TailTotal != value)
                {
                    _TailTotal = value;
                    OnPropertyChanged("TailTotal");
                }
            }
        }

        private string _HeadToHead;
        public string HeadToHead
        {
            get { return _HeadToHead; }
            set
            {
                if (_HeadToHead != value)
                {
                    _HeadToHead = value;
                    OnPropertyChanged("HeadToHead");
                }
            }
        }

        private string _TailToTail;
        public string TailToTail
        {
            get { return _TailToTail; }
            set
            {
                if (_TailToTail != value)
                {
                    _TailToTail = value;
                    OnPropertyChanged("TailToTail");
                }
            }
        }



        private string _HeadToTail;
        public string HeadToTail
        {
            get { return _HeadToTail; }
            set
            {
                if (_HeadToTail != value)
                {
                    _HeadToTail = value;
                    OnPropertyChanged("HeadToTail");
                }
            }
        }

        private string _SpermToOther;
        public string SpermToOther
        {
            get { return _SpermToOther; }
            set
            {
                if (_SpermToOther != value)
                {
                    _SpermToOther = value;
                    OnPropertyChanged("SpermToOther");
                }
            }
        }

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
        
        private string _Infections;
        public string Infections
        {
            get { return _Infections; }
            set
            {
                if (_Infections != value)
                {
                    _Infections = value;
                    OnPropertyChanged("Infections");
                }
            }
        }


        private string _OtherFindings;
        public string OtherFindings
        {
            get { return _OtherFindings; }
            set
            {
                if (_OtherFindings != value)
                {
                    _OtherFindings = value;
                    OnPropertyChanged("OtherFindings");
                }
            }
        }

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


        private long _InterpretationsID;
        public long InterpretationsID
        {
            get { return _InterpretationsID; }
            set
            {
                if (_InterpretationsID != value)
                {
                    _InterpretationsID = value;
                    OnPropertyChanged("InterpretationsID");
                }
            }
        }

        private long _EmbryologistID;
        public long EmbryologistID
        {
            get { return _EmbryologistID; }
            set
            {
                if (_EmbryologistID != value)
                {
                    _EmbryologistID = value;
                    OnPropertyChanged("EmbryologistID");
                }
            }
        }

        private string _VolumeRange;
        public string VolumeRange
        {
            get { return _VolumeRange; }
            set
            {
                if (_VolumeRange != value)
                {
                    _VolumeRange = value;
                    OnPropertyChanged("VolumeRange");
                }
            }
        }

        private string _PHRange;
        public string PHRange
        {
            get { return _PHRange; }
            set
            {
                if (_PHRange != value)
                {
                    _PHRange = value;
                    OnPropertyChanged("PHRange");
                }
            }
        }

        private string _PusCellsRange;
        public string PusCellsRange
        {
            get { return _PusCellsRange; }
            set
            {
                if (_PusCellsRange != value)
                {
                    _PusCellsRange = value;
                    OnPropertyChanged("PusCellsRange");
                }
            }
        }

        private string _MorphologyAbnormilityRange;
        public string MorphologyAbnormilityRange
        {
            get { return _MorphologyAbnormilityRange; }
            set
            {
                if (_MorphologyAbnormilityRange != value)
                {
                    _MorphologyAbnormilityRange = value;
                    OnPropertyChanged("MorphologyAbnormilityRange");
                }
            }
        }

        private string _NormalMorphologyRange;
        public string NormalMorphologyRange
        {
            get { return _NormalMorphologyRange; }
            set
            {
                if (_NormalMorphologyRange != value)
                {
                    _NormalMorphologyRange = value;
                    OnPropertyChanged("NormalMorphologyRange");
                }
            }
        }

        private string _SpermConcentrationRange;
        public string SpermConcentrationRange
        {
            get { return _SpermConcentrationRange; }
            set
            {
                if (_SpermConcentrationRange != value)
                {
                    _SpermConcentrationRange = value;
                    OnPropertyChanged("SpermConcentrationRange");
                }
            }
        }

        private string _EjaculateRange;
        public string EjaculateRange
        {
            get { return _EjaculateRange; }
            set
            {
                if (_EjaculateRange != value)
                {
                    _EjaculateRange = value;
                    OnPropertyChanged("EjaculateRange");
                }
            }
        }

        private string _TotalMotilityRange;
        public string TotalMotilityRange
        {
            get { return _TotalMotilityRange; }
            set
            {
                if (_TotalMotilityRange != value)
                {
                    _TotalMotilityRange = value;
                    OnPropertyChanged("TotalMotilityRange");
                }
            }
        } 
        
        
        
        #endregion

    }
}
