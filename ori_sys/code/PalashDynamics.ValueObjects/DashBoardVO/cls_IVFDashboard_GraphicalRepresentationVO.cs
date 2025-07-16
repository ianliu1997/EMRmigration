using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
//using System.Windows.Media;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class cls_IVFDashboard_GraphicalRepresentationVO : IValueObject, INotifyPropertyChanged
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

        private long _Status;
        public long Status
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

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedOn;
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

        private string _UpdatedWindowsLoginName;
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

        private bool _Synchronized;
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

        public long DecisionID { get; set; }
        public long OocyteDonorID { get; set; }
        public long OocyteDonorUnitID { get; set; }

        private List<ClsAddObervationEmbryo> _EmbryoDayObservation = new List<ClsAddObervationEmbryo>();
        public List<ClsAddObervationEmbryo> EmbryoDayObservation
        {
            get
            {
                return _EmbryoDayObservation;
            }
            set
            {
                _EmbryoDayObservation = value;
            }
        }

        private List<MasterListItem> _Decision = new List<MasterListItem>();
        public List<MasterListItem> Decision
        {
            get
            {
                return _Decision;
            }
            set
            {
                _Decision = value;
            }
        }

        MasterListItem _SelectedDecision = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedDecision
        {
            get
            {
                return _SelectedDecision;
            }
            set
            {
                if (value != _SelectedDecision)
                {
                    _SelectedDecision = value;
                    OnPropertyChanged("SelectedDecision");
                }
            }


        }

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

        private string _MRNO;
        public string MRNO
        {
            get { return _MRNO; }
            set
            {
                if (_MRNO != value)
                {
                    _MRNO = value;
                    OnPropertyChanged("MRNO");
                }
            }
        }

        private string _Day3PGDPGS;
        public string Day3PGDPGS
        {
            get { return _Day3PGDPGS; }
            set
            {
                if (_Day3PGDPGS != value)
                {
                    _Day3PGDPGS = value;
                    OnPropertyChanged("Day3PGDPGS");
                }
            }
        }

        private string _Day5PGDPGS;
        public string Day5PGDPGS
        {
            get { return _Day5PGDPGS; }
            set
            {
                if (_Day5PGDPGS != value)
                {
                    _Day5PGDPGS = value;
                    OnPropertyChanged("Day5PGDPGS");
                }
            }
        }

        private string _Day6PGDPGS;
        public string Day6PGDPGS
        {
            get { return _Day6PGDPGS; }
            set
            {
                if (_Day6PGDPGS != value)
                {
                    _Day6PGDPGS = value;
                    OnPropertyChanged("Day6PGDPGS");
                }
            }
        }

        private string _Day7PGDPGS;
        public string Day7PGDPGS
        {
            get { return _Day7PGDPGS; }
            set
            {
                if (_Day7PGDPGS != value)
                {
                    _Day7PGDPGS = value;
                    OnPropertyChanged("Day7PGDPGS");
                }
            }
        }

        private bool _BlDay3PGDPGS;
        public bool BlDay3PGDPGS
        {
            get { return _BlDay3PGDPGS; }
            set
            {
                if (_BlDay3PGDPGS != value)
                {
                    _BlDay3PGDPGS = value;
                    OnPropertyChanged("BlDay3PGDPGS");
                }
            }
        }

        private bool _BlDay5PGDPGS;
        public bool BlDay5PGDPGS
        {
            get { return _BlDay5PGDPGS; }
            set
            {
                if (_BlDay5PGDPGS != value)
                {
                    _BlDay5PGDPGS = value;
                    OnPropertyChanged("BlDay5PGDPGS");
                }
            }
        }

        private bool _BlDay6PGDPGS;
        public bool BlDay6PGDPGS
        {
            get { return _BlDay6PGDPGS; }
            set
            {
                if (_BlDay6PGDPGS != value)
                {
                    _BlDay6PGDPGS = value;
                    OnPropertyChanged("BlDay6PGDPGS");
                }
            }
        }

        private bool _BlDay7PGDPGS;
        public bool BlDay7PGDPGS
        {
            get { return _BlDay7PGDPGS; }
            set
            {
                if (_BlDay7PGDPGS != value)
                {
                    _BlDay7PGDPGS = value;
                    OnPropertyChanged("BlDay7PGDPGS");
                }
            }
        }

        private bool _BlFrozenDay3PGDPGS;
        public bool BlFrozenDay3PGDPGS
        {
            get { return _BlFrozenDay3PGDPGS; }
            set
            {
                if (_BlFrozenDay3PGDPGS != value)
                {
                    _BlFrozenDay3PGDPGS = value;
                    OnPropertyChanged("BlFrozenDay3PGDPGS");
                }
            }
        }

        private bool _BlFrozenDay5PGDPGS;
        public bool BlFrozenDay5PGDPGS
        {
            get { return _BlFrozenDay5PGDPGS; }
            set
            {
                if (_BlFrozenDay5PGDPGS != value)
                {
                    _BlFrozenDay5PGDPGS = value;
                    OnPropertyChanged("BlFrozenDay5PGDPGS");
                }
            }
        }

        private bool _BlFrozenDay6PGDPGS;
        public bool BlFrozenDay6PGDPGS
        {
            get { return _BlFrozenDay6PGDPGS; }
            set
            {
                if (_BlFrozenDay6PGDPGS != value)
                {
                    _BlFrozenDay6PGDPGS = value;
                    OnPropertyChanged("BlFrozenDay6PGDPGS");
                }
            }
        }

        private bool _BlFrozenDay7PGDPGS;
        public bool BlFrozenDay7PGDPGS
        {
            get { return _BlFrozenDay7PGDPGS; }
            set
            {
                if (_BlFrozenDay7PGDPGS != value)
                {
                    _BlFrozenDay7PGDPGS = value;
                    OnPropertyChanged("BlFrozenDay7PGDPGS");
                }
            }
        }

        private bool _IsFrozenEmbryoPGDPGS;
        public bool IsFrozenEmbryoPGDPGS
        {
            get { return _IsFrozenEmbryoPGDPGS; }
            set
            {
                if (_IsFrozenEmbryoPGDPGS != value)
                {
                    _IsFrozenEmbryoPGDPGS = value;
                    OnPropertyChanged("IsFrozenEmbryoPGDPGS");
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
        private bool? _Day0;
        public bool? Day0
        {
            get { return _Day0; }
            set
            {
                if (_Day0 != value)
                {
                    _Day0 = value;
                    OnPropertyChanged("Day0");
                }
            }
        }

        private bool? _Day1;
        public bool? Day1
        {
            get { return _Day1; }
            set
            {
                if (_Day1 != value)
                {
                    _Day1 = value;
                    OnPropertyChanged("Day1");
                }
            }
        }
        private bool? _Day2;
        public bool? Day2
        {
            get { return _Day2; }
            set
            {
                if (_Day2 != value)
                {
                    _Day2 = value;
                    OnPropertyChanged("Day2");
                }
            }
        }
        private bool? _Day3;
        public bool? Day3
        {
            get { return _Day3; }
            set
            {
                if (_Day3 != value)
                {
                    _Day3 = value;
                    OnPropertyChanged("Day3");
                }
            }
        }
        private bool? _Day4;
        public bool? Day4
        {
            get { return _Day4; }
            set
            {
                if (_Day4 != value)
                {
                    _Day4 = value;
                    OnPropertyChanged("Day4");
                }
            }
        }
        private bool? _Day5;
        public bool? Day5
        {
            get { return _Day5; }
            set
            {
                if (_Day5 != value)
                {
                    _Day5 = value;
                    OnPropertyChanged("Day5");
                }
            }
        }
        private bool? _Day6;
        public bool? Day6
        {
            get { return _Day6; }
            set
            {
                if (_Day6 != value)
                {
                    _Day6 = value;
                    OnPropertyChanged("Day6");
                }
            }
        }
        private bool? _Day7;
        public bool? Day7
        {
            get { return _Day7; }
            set
            {
                if (_Day7 != value)
                {
                    _Day7 = value;
                    OnPropertyChanged("Day7");
                }
            }
        }
        private long _OocyteNumber;
        public long OocyteNumber
        {
            get { return _OocyteNumber; }
            set
            {
                if (_OocyteNumber != value)
                {
                    _OocyteNumber = value;
                    OnPropertyChanged("OocyteNumber");
                }
            }
        }
        private long _SerialOocyteNumber;
        public long SerialOocyteNumber
        {
            get { return _SerialOocyteNumber; }
            set
            {
                if (_SerialOocyteNumber != value)
                {
                    _SerialOocyteNumber = value;
                    OnPropertyChanged("SerialOocyteNumber");
                }
            }
        }
        private long _Day0CellStage;
        public long Day0CellStage
        {
            get { return _Day0CellStage; }
            set
            {
                if (_Day0CellStage != value)
                {
                    _Day0CellStage = value;
                    OnPropertyChanged("Day0CellStage");
                }
            }
        }
        private long _Day1CellStage;
        public long Day1CellStage
        {
            get { return _Day1CellStage; }
            set
            {
                if (_Day1CellStage != value)
                {
                    _Day1CellStage = value;
                    OnPropertyChanged("Day1CellStage");
                }
            }
        }


        private long _Day2CellStage;
        public long Day2CellStage
        {
            get { return _Day2CellStage; }
            set
            {
                if (_Day2CellStage != value)
                {
                    _Day2CellStage = value;
                    OnPropertyChanged("Day2CellStage");
                }
            }
        }
        private long _Day3CellStage;
        public long Day3CellStage
        {
            get { return _Day3CellStage; }
            set
            {
                if (_Day3CellStage != value)
                {
                    _Day3CellStage = value;
                    OnPropertyChanged("Day3CellStage");
                }
            }
        }
        private long _Day4CellStage;
        public long Day4CellStage
        {
            get { return _Day4CellStage; }
            set
            {
                if (_Day4CellStage != value)
                {
                    _Day4CellStage = value;
                    OnPropertyChanged("Day4CellStage");
                }
            }
        }
        private long _Day5CellStage;
        public long Day5CellStage
        {
            get { return _Day5CellStage; }
            set
            {
                if (_Day5CellStage != value)
                {
                    _Day5CellStage = value;
                    OnPropertyChanged("Day5CellStage");
                }
            }
        }
        private long _Day6CellStage;
        public long Day6CellStage
        {
            get { return _Day6CellStage; }
            set
            {
                if (_Day6CellStage != value)
                {
                    _Day6CellStage = value;
                    OnPropertyChanged("Day6CellStage");
                }
            }
        }
        private long _Day7CellStage;
        public long Day7CellStage
        {
            get { return _Day7CellStage; }
            set
            {
                if (_Day7CellStage != value)
                {
                    _Day7CellStage = value;
                    OnPropertyChanged("Day7CellStage");
                }
            }
        }
        private string _Day0Image;
        public string Day0Image
        {
            get { return _Day0Image; }
            set
            {
                if (_Day0Image != value)
                {
                    _Day0Image = value;
                    OnPropertyChanged("Day0Image");
                }
            }
        }
        private string _Day1Image;
        public string Day1Image
        {
            get { return _Day1Image; }
            set
            {
                if (_Day1Image != value)
                {
                    _Day1Image = value;
                    OnPropertyChanged("Day1Image");
                }
            }
        }
        private string _Day2Image;
        public string Day2Image
        {
            get { return _Day2Image; }
            set
            {
                if (_Day2Image != value)
                {
                    _Day2Image = value;
                    OnPropertyChanged("Day2Image");
                }
            }
        }
        private string _Day3Image;
        public string Day3Image
        {
            get { return _Day3Image; }
            set
            {
                if (_Day3Image != value)
                {
                    _Day3Image = value;
                    OnPropertyChanged("Day3Image");
                }
            }
        }
        private string _Day4Image;
        public string Day4Image
        {
            get { return _Day4Image; }
            set
            {
                if (_Day4Image != value)
                {
                    _Day4Image = value;
                    OnPropertyChanged("Day4Image");
                }
            }
        }
        private string _Day5Image;
        public string Day5Image
        {
            get { return _Day5Image; }
            set
            {
                if (_Day5Image != value)
                {
                    _Day5Image = value;
                    OnPropertyChanged("Day5Image");
                }
            }
        }
        private string _Day6Image;
        public string Day6Image
        {
            get { return _Day6Image; }
            set
            {
                if (_Day6Image != value)
                {
                    _Day6Image = value;
                    OnPropertyChanged("Day6Image");
                }
            }
        }

        private string _CellStage;
        public string CellStage
        {
            get { return _CellStage; }
            set
            {
                if (_CellStage != value)
                {
                    _CellStage = value;
                    OnPropertyChanged("CellStage");
                }
            }
        }

        private string _CellStageDay0;
        public string CellStageDay0
        {
            get { return _CellStageDay0; }
            set
            {
                if (_CellStageDay0 != value)
                {
                    _CellStageDay0 = value;
                    OnPropertyChanged("CellStageDay0");
                }
            }
        }
        private string _CellStageDay1;
        public string CellStageDay1
        {
            get { return _CellStageDay1; }
            set
            {
                if (_CellStageDay1 != value)
                {
                    _CellStageDay1 = value;
                    OnPropertyChanged("CellStageDay1");
                }
            }
        }
        private string _CellStageDay2;
        public string CellStageDay2
        {
            get { return _CellStageDay2; }
            set
            {
                if (_CellStageDay2 != value)
                {
                    _CellStageDay2 = value;
                    OnPropertyChanged("CellStageDay2");
                }
            }
        }
        private string _CellStageDay3;
        public string CellStageDay3
        {
            get { return _CellStageDay3; }
            set
            {
                if (_CellStageDay3 != value)
                {
                    _CellStageDay3 = value;
                    OnPropertyChanged("CellStageDay3");
                }
            }
        }
        private string _CellStageDay4;
        public string CellStageDay4
        {
            get { return _CellStageDay4; }
            set
            {
                if (_CellStageDay4 != value)
                {
                    _CellStageDay4 = value;
                    OnPropertyChanged("CellStageDay4");
                }
            }
        }
        private string _CellStageDay5;
        public string CellStageDay5
        {
            get { return _CellStageDay5; }
            set
            {
                if (_CellStageDay5 != value)
                {
                    _CellStageDay5 = value;
                    OnPropertyChanged("CellStageDay5");
                }
            }
        }
        private string _CellStageDay6;
        public string CellStageDay6
        {
            get { return _CellStageDay6; }
            set
            {
                if (_CellStageDay6 != value)
                {
                    _CellStageDay6 = value;
                    OnPropertyChanged("CellStageDay6");
                }
            }
        }
        private string _CellStageDay7;
        public string CellStageDay7
        {
            get { return _CellStageDay7; }
            set
            {
                if (_CellStageDay7 != value)
                {
                    _CellStageDay7 = value;
                    OnPropertyChanged("CellStageDay7");
                }
            }
        }

        private string _ImpressionDay0;
        public string ImpressionDay0
        {
            get { return _ImpressionDay0; }
            set
            {
                if (_ImpressionDay0 != value)
                {
                    _ImpressionDay0 = value;
                    OnPropertyChanged("ImpressionDay0");
                }
            }
        }

        private string _ImpressionDay1;
        public string ImpressionDay1
        {
            get { return _ImpressionDay1; }
            set
            {
                if (_ImpressionDay1 != value)
                {
                    _ImpressionDay1 = value;
                    OnPropertyChanged("ImpressionDay1");
                }
            }
        }

        private string _ImpressionDay2;
        public string ImpressionDay2
        {
            get { return _ImpressionDay2; }
            set
            {
                if (_ImpressionDay2 != value)
                {
                    _ImpressionDay2 = value;
                    OnPropertyChanged("ImpressionDay2");
                }
            }
        }

        private string _ImpressionDay3;
        public string ImpressionDay3
        {
            get { return _ImpressionDay3; }
            set
            {
                if (_ImpressionDay3 != value)
                {
                    _ImpressionDay3 = value;
                    OnPropertyChanged("ImpressionDay3");
                }
            }
        }

        private string _ImpressionDay4;
        public string ImpressionDay4
        {
            get { return _ImpressionDay4; }
            set
            {
                if (_ImpressionDay4 != value)
                {
                    _ImpressionDay4 = value;
                    OnPropertyChanged("ImpressionDay4");
                }
            }
        }

        private string _ImpressionDay5;
        public string ImpressionDay5
        {
            get { return _ImpressionDay5; }
            set
            {
                if (_ImpressionDay5 != value)
                {
                    _ImpressionDay5 = value;
                    OnPropertyChanged("ImpressionDay5");
                }
            }
        }

        private string _ImpressionDay6;
        public string ImpressionDay6
        {
            get { return _ImpressionDay6; }
            set
            {
                if (_ImpressionDay6 != value)
                {
                    _ImpressionDay6 = value;
                    OnPropertyChanged("ImpressionDay6");
                }
            }
        }
        private string _ImpressionDay7;
        public string ImpressionDay7
        {
            get { return _ImpressionDay7; }
            set
            {
                if (_ImpressionDay7 != value)
                {
                    _ImpressionDay7 = value;
                    OnPropertyChanged("ImpressionDay7");
                }
            }
        }
        private string _Plan0;
        public string Plan0
        {
            get { return _Plan0; }
            set
            {
                if (_Plan0 != value)
                {
                    _Plan0 = value;
                    OnPropertyChanged("Plan0");
                }
            }
        }
        private string _Plan1;
        public string Plan1
        {
            get { return _Plan1; }
            set
            {
                if (_Plan1 != value)
                {
                    _Plan1 = value;
                    OnPropertyChanged("Plan1");
                }
            }
        }
        private string _Plan2;
        public string Plan2
        {
            get { return _Plan2; }
            set
            {
                if (_Plan2 != value)
                {
                    _Plan2 = value;
                    OnPropertyChanged("Plan2");
                }
            }
        }
        private string _Plan3;
        public string Plan3
        {
            get { return _Plan3; }
            set
            {
                if (_Plan3 != value)
                {
                    _Plan3 = value;
                    OnPropertyChanged("Plan3");
                }
            }
        }
        private string _Plan4;
        public string Plan4
        {
            get { return _Plan4; }
            set
            {
                if (_Plan4 != value)
                {
                    _Plan4 = value;
                    OnPropertyChanged("Plan4");
                }
            }
        }
        private string _Plan5;
        public string Plan5
        {
            get { return _Plan5; }
            set
            {
                if (_Plan5 != value)
                {
                    _Plan5 = value;
                    OnPropertyChanged("Plan5");
                }
            }
        }
        private string _Plan6;
        public string Plan6
        {
            get { return _Plan6; }
            set
            {
                if (_Plan6 != value)
                {
                    _Plan6 = value;
                    OnPropertyChanged("Plan6");
                }
            }
        }
        private string _Plan7;
        public string Plan7
        {
            get { return _Plan7; }
            set
            {
                if (_Plan7 != value)
                {
                    _Plan7 = value;
                    OnPropertyChanged("Plan7");
                }
            }
        }
        //added by neena
        private bool _IsExtendedCulture;
        public bool IsExtendedCulture
        {
            get { return _IsExtendedCulture; }
            set
            {
                if (_IsExtendedCulture != value)
                {
                    _IsExtendedCulture = value;
                    OnPropertyChanged("IsExtendedCulture");
                }
            }
        }

        private bool _IsExtendedCultureFromOtherCycle;
        public bool IsExtendedCultureFromOtherCycle
        {
            get { return _IsExtendedCultureFromOtherCycle; }
            set
            {
                if (_IsExtendedCultureFromOtherCycle != value)
                {
                    _IsExtendedCultureFromOtherCycle = value;
                    OnPropertyChanged("IsExtendedCultureFromOtherCycle");
                }
            }
        }

        private bool _IsFertCheck;
        public bool IsFertCheck
        {
            get { return _IsFertCheck; }
            set
            {
                if (_IsFertCheck != value)
                {
                    _IsFertCheck = value;
                    OnPropertyChanged("IsFertCheck");
                }
            }
        }

        private string _FertCheckImage;
        public string FertCheckImage
        {
            get { return _FertCheckImage; }
            set
            {
                if (_FertCheckImage != value)
                {
                    _FertCheckImage = value;
                    OnPropertyChanged("FertCheckImage");
                }
            }
        }

        private long _FertCheck;
        public long FertCheck
        {
            get { return _FertCheck; }
            set
            {
                if (_FertCheck != value)
                {
                    _FertCheck = value;
                    OnPropertyChanged("FertCheck");
                }
            }
        }

        private string _Day;
        public string Day
        {
            get { return _Day; }
            set
            {
                if (_Day != value)
                {
                    _Day = value;
                    OnPropertyChanged("Day");
                }
            }
        }

        private long _DayNo;
        public long DayNo
        {
            get { return _DayNo; }
            set
            {
                if (_DayNo != value)
                {
                    _DayNo = value;
                    OnPropertyChanged("DayNo");
                }
            }
        }

        private bool _IsAddObservation;
        public bool IsAddObservation
        {
            get { return _IsAddObservation; }
            set
            {
                if (_IsAddObservation != value)
                {
                    _IsAddObservation = value;
                    OnPropertyChanged("IsAddObservation");
                }
            }
        }

        private bool _IsDecision;
        public bool IsDecision
        {
            get { return _IsDecision; }
            set
            {
                if (_IsDecision != value)
                {
                    _IsDecision = value;
                    OnPropertyChanged("IsDecision");
                }
            }
        }

        private bool _IsSelectPatient;
        public bool IsSelectPatient
        {
            get { return _IsSelectPatient; }
            set
            {
                if (_IsSelectPatient != value)
                {
                    _IsSelectPatient = value;
                    OnPropertyChanged("IsSelectPatient");
                }
            }
        }


        private bool _IsFreezedecision;
        public bool IsFreezedecision
        {
            get { return _IsFreezedecision; }
            set
            {
                if (_IsFreezedecision != value)
                {
                    _IsFreezedecision = value;
                    OnPropertyChanged("IsFreezedecision");
                }
            }
        }

        private string _CycleCode;
        public string CycleCode
        {
            get { return _CycleCode; }
            set
            {
                if (_CycleCode != value)
                {
                    _CycleCode = value;
                    OnPropertyChanged("CycleCode");
                }
            }
        }

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

        private bool _IsLabDay0Freezed;
        public bool IsLabDay0Freezed
        {
            get { return _IsLabDay0Freezed; }
            set
            {
                if (_IsLabDay0Freezed != value)
                {
                    _IsLabDay0Freezed = value;
                    OnPropertyChanged("IsLabDay0Freezed");
                }
            }
        }

        private bool _IsLabDay1Freezed;
        public bool IsLabDay1Freezed
        {
            get { return _IsLabDay1Freezed; }
            set
            {
                if (_IsLabDay1Freezed != value)
                {
                    _IsLabDay1Freezed = value;
                    OnPropertyChanged("IsLabDay1Freezed");
                }
            }
        }

        private bool _IsLabDay2Freezed;
        public bool IsLabDay2Freezed
        {
            get { return _IsLabDay2Freezed; }
            set
            {
                if (_IsLabDay2Freezed != value)
                {
                    _IsLabDay2Freezed = value;
                    OnPropertyChanged("IsLabDay2Freezed");
                }
            }
        }

        private bool _IsLabDay3Freezed;
        public bool IsLabDay3Freezed
        {
            get { return _IsLabDay3Freezed; }
            set
            {
                if (_IsLabDay3Freezed != value)
                {
                    _IsLabDay3Freezed = value;
                    OnPropertyChanged("IsLabDay3Freezed");
                }
            }
        }

        private bool _IsLabDay4Freezed;
        public bool IsLabDay4Freezed
        {
            get { return _IsLabDay4Freezed; }
            set
            {
                if (_IsLabDay4Freezed != value)
                {
                    _IsLabDay4Freezed = value;
                    OnPropertyChanged("IsLabDay4Freezed");
                }
            }
        }

        private bool _IsLabDay5Freezed;
        public bool IsLabDay5Freezed
        {
            get { return _IsLabDay5Freezed; }
            set
            {
                if (_IsLabDay5Freezed != value)
                {
                    _IsLabDay5Freezed = value;
                    OnPropertyChanged("IsLabDay5Freezed");
                }
            }
        }

        private bool _IsLabDay6Freezed;
        public bool IsLabDay6Freezed
        {
            get { return _IsLabDay6Freezed; }
            set
            {
                if (_IsLabDay6Freezed != value)
                {
                    _IsLabDay6Freezed = value;
                    OnPropertyChanged("IsLabDay6Freezed");
                }
            }
        }

        private bool _IsLabDay7Freezed;
        public bool IsLabDay7Freezed
        {
            get { return _IsLabDay7Freezed; }
            set
            {
                if (_IsLabDay7Freezed != value)
                {
                    _IsLabDay7Freezed = value;
                    OnPropertyChanged("IsLabDay7Freezed");
                }
            }
        }

        private DateTime? _ProcedureDate = DateTime.Now;
        public DateTime? ProcedureDate
        {
            get { return _ProcedureDate; }
            set
            {
                if (_ProcedureDate != value)
                {
                    _ProcedureDate = value;
                    OnPropertyChanged("ProcedureDate");
                }
            }
        }

        private DateTime? _ProcedureTime = DateTime.Now;
        public DateTime? ProcedureTime
        {
            get { return _ProcedureTime; }
            set
            {
                if (_ProcedureTime != value)
                {
                    _ProcedureTime = value;
                    OnPropertyChanged("ProcedureTime");
                }
            }
        }

        private DateTime? _PlanDate ;
        public DateTime? PlanDate
        {
            get { return _PlanDate; }
            set
            {
                if (_PlanDate != value)
                {
                    _PlanDate = value;
                    OnPropertyChanged("PlanDate");
                }
            }
        }

        private bool _Day1Visible;
        public bool Day1Visible
        {
            get { return _Day1Visible; }
            set
            {
                if (_Day1Visible != value)
                {
                    _Day1Visible = value;
                    OnPropertyChanged("Day1Visible");
                }
            }
        }

        private bool _Day2Visible;
        public bool Day2Visible
        {
            get { return _Day2Visible; }
            set
            {
                if (_Day2Visible != value)
                {
                    _Day2Visible = value;
                    OnPropertyChanged("Day2Visible");
                }
            }
        }

        private bool _Day3Visible;
        public bool Day3Visible
        {
            get { return _Day3Visible; }
            set
            {
                if (_Day3Visible != value)
                {
                    _Day3Visible = value;
                    OnPropertyChanged("Day3Visible");
                }
            }
        }

        private bool _Day4Visible;
        public bool Day4Visible
        {
            get { return _Day4Visible; }
            set
            {
                if (_Day4Visible != value)
                {
                    _Day4Visible = value;
                    OnPropertyChanged("Day4Visible");
                }
            }
        }

        private bool _Day5Visible;
        public bool Day5Visible
        {
            get { return _Day5Visible; }
            set
            {
                if (_Day5Visible != value)
                {
                    _Day5Visible = value;
                    OnPropertyChanged("Day5Visible");
                }
            }
        }

        private bool _Day6Visible;
        public bool Day6Visible
        {
            get { return _Day6Visible; }
            set
            {
                if (_Day6Visible != value)
                {
                    _Day6Visible = value;
                    OnPropertyChanged("Day6Visible");
                }
            }
        }

        private bool _Day7Visible;
        public bool Day7Visible
        {
            get { return _Day7Visible; }
            set
            {
                if (_Day7Visible != value)
                {
                    _Day7Visible = value;
                    OnPropertyChanged("Day7Visible");
                }
            }
        }

        private long _GradeID;
        public long GradeID
        {
            get { return _GradeID; }
            set
            {
                if (_GradeID != value)
                {
                    _GradeID = value;
                    OnPropertyChanged("GradeID");
                }
            }
        }

        private long _GradeIDDay1;
        public long GradeIDDay1
        {
            get { return _GradeIDDay1; }
            set
            {
                if (_GradeIDDay1 != value)
                {
                    _GradeIDDay1 = value;
                    OnPropertyChanged("GradeIDDay1");
                }
            }
        }

        private long _GradeIDDay2;
        public long GradeIDDay2
        {
            get { return _GradeIDDay2; }
            set
            {
                if (_GradeIDDay2 != value)
                {
                    _GradeIDDay2 = value;
                    OnPropertyChanged("GradeIDDay2");
                }
            }
        }

        private long _GradeIDDay3;
        public long GradeIDDay3
        {
            get { return _GradeIDDay3; }
            set
            {
                if (_GradeIDDay3 != value)
                {
                    _GradeIDDay3 = value;
                    OnPropertyChanged("GradeIDDay3");
                }
            }
        }

        private long _GradeIDDay4;
        public long GradeIDDay4
        {
            get { return _GradeIDDay4; }
            set
            {
                if (_GradeIDDay4 != value)
                {
                    _GradeIDDay4 = value;
                    OnPropertyChanged("GradeIDDay4");
                }
            }
        }

        private long _StageofDevelopmentGrade;
        public long StageofDevelopmentGrade
        {
            get { return _StageofDevelopmentGrade; }
            set
            {
                if (_StageofDevelopmentGrade != value)
                {
                    _StageofDevelopmentGrade = value;
                    OnPropertyChanged("StageofDevelopmentGrade");
                }
            }
        }

        private long _InnerCellMassGrade;
        public long InnerCellMassGrade
        {
            get { return _InnerCellMassGrade; }
            set
            {
                if (_InnerCellMassGrade != value)
                {
                    _InnerCellMassGrade = value;
                    OnPropertyChanged("InnerCellMassGrade");
                }
            }
        }

        private long _TrophoectodermGrade;
        public long TrophoectodermGrade
        {
            get { return _TrophoectodermGrade; }
            set
            {
                if (_TrophoectodermGrade != value)
                {
                    _TrophoectodermGrade = value;
                    OnPropertyChanged("TrophoectodermGrade");
                }
            }
        }

        private long _StageofDevelopmentGradeDay5;
        public long StageofDevelopmentGradeDay5
        {
            get { return _StageofDevelopmentGradeDay5; }
            set
            {
                if (_StageofDevelopmentGradeDay5 != value)
                {
                    _StageofDevelopmentGradeDay5 = value;
                    OnPropertyChanged("StageofDevelopmentGradeDay5");
                }
            }
        }

        private long _InnerCellMassGradeDay5;
        public long InnerCellMassGradeDay5
        {
            get { return _InnerCellMassGradeDay5; }
            set
            {
                if (_InnerCellMassGradeDay5 != value)
                {
                    _InnerCellMassGradeDay5 = value;
                    OnPropertyChanged("InnerCellMassGradeDay5");
                }
            }
        }

        private long _TrophoectodermGradeDay5;
        public long TrophoectodermGradeDay5
        {
            get { return _TrophoectodermGradeDay5; }
            set
            {
                if (_TrophoectodermGradeDay5 != value)
                {
                    _TrophoectodermGradeDay5 = value;
                    OnPropertyChanged("TrophoectodermGradeDay5");
                }
            }
        }

        private long _StageofDevelopmentGradeDay6;
        public long StageofDevelopmentGradeDay6
        {
            get { return _StageofDevelopmentGradeDay6; }
            set
            {
                if (_StageofDevelopmentGradeDay6 != value)
                {
                    _StageofDevelopmentGradeDay6 = value;
                    OnPropertyChanged("StageofDevelopmentGradeDay6");
                }
            }
        }

        private long _InnerCellMassGradeDay6;
        public long InnerCellMassGradeDay6
        {
            get { return _InnerCellMassGradeDay6; }
            set
            {
                if (_InnerCellMassGradeDay6 != value)
                {
                    _InnerCellMassGradeDay6 = value;
                    OnPropertyChanged("InnerCellMassGradeDay6");
                }
            }
        }

        private long _TrophoectodermGradeDay6;
        public long TrophoectodermGradeDay6
        {
            get { return _TrophoectodermGradeDay6; }
            set
            {
                if (_TrophoectodermGradeDay6 != value)
                {
                    _TrophoectodermGradeDay6 = value;
                    OnPropertyChanged("TrophoectodermGradeDay6");
                }
            }
        }

        private long _StageofDevelopmentGradeDay7;
        public long StageofDevelopmentGradeDay7
        {
            get { return _StageofDevelopmentGradeDay7; }
            set
            {
                if (_StageofDevelopmentGradeDay7 != value)
                {
                    _StageofDevelopmentGradeDay7 = value;
                    OnPropertyChanged("StageofDevelopmentGradeDay7");
                }
            }
        }

        private long _InnerCellMassGradeDay7;
        public long InnerCellMassGradeDay7
        {
            get { return _InnerCellMassGradeDay7; }
            set
            {
                if (_InnerCellMassGradeDay7 != value)
                {
                    _InnerCellMassGradeDay7 = value;
                    OnPropertyChanged("InnerCellMassGradeDay7");
                }
            }
        }

        private long _TrophoectodermGradeDay7;
        public long TrophoectodermGradeDay7
        {
            get { return _TrophoectodermGradeDay7; }
            set
            {
                if (_TrophoectodermGradeDay7 != value)
                {
                    _TrophoectodermGradeDay7 = value;
                    OnPropertyChanged("TrophoectodermGradeDay7");
                }
            }
        }

        private string _IsExtendedCultureForBoth;
        public string IsExtendedCultureForBoth
        {
            get { return _IsExtendedCultureForBoth; }
            set
            {
                if (_IsExtendedCultureForBoth != value)
                {
                    _IsExtendedCultureForBoth = value;
                    OnPropertyChanged("IsExtendedCultureForBoth");
                }
            }
        }


        private DateTime? _ObservationDate1 ;
        public DateTime? ObservationDate1
        {
            get { return _ObservationDate1; }
            set
            {
                if (_ObservationDate1 != value)
                {
                    _ObservationDate1 = value;
                    OnPropertyChanged("ObservationDate1");
                }
            }
        }

        private DateTime? _ObservationTime1;
        public DateTime? ObservationTime1
        {
            get { return _ObservationTime1; }
            set
            {
                if (_ObservationTime1 != value)
                {
                    _ObservationTime1 = value;
                    OnPropertyChanged("ObservationTime1");
                }
            }
        }

        private DateTime? _ObservationDate2;
        public DateTime? ObservationDate2
        {
            get { return _ObservationDate2; }
            set
            {
                if (_ObservationDate2 != value)
                {
                    _ObservationDate2 = value;
                    OnPropertyChanged("ObservationDate2");
                }
            }
        }
        private DateTime? _ObservationTime2;
        public DateTime? ObservationTime2
        {
            get { return _ObservationTime2; }
            set
            {
                if (_ObservationTime2 != value)
                {
                    _ObservationTime2 = value;
                    OnPropertyChanged("ObservationTime2");
                }
            }
        }

        private DateTime? _ObservationDate3;
        public DateTime? ObservationDate3
        {
            get { return _ObservationDate3; }
            set
            {
                if (_ObservationDate3 != value)
                {
                    _ObservationDate3 = value;
                    OnPropertyChanged("ObservationDate3");
                }
            }
        }

        private DateTime? _ObservationTime3;
        public DateTime? ObservationTime3
        {
            get { return _ObservationTime3; }
            set
            {
                if (_ObservationTime3 != value)
                {
                    _ObservationTime3 = value;
                    OnPropertyChanged("ObservationTime1");
                }
            }
        }

        private DateTime? _ObservationDate4;
        public DateTime? ObservationDate4
        {
            get { return _ObservationDate4; }
            set
            {
                if (_ObservationDate4 != value)
                {
                    _ObservationDate4 = value;
                    OnPropertyChanged("ObservationDate4");
                }
            }
        }

        private DateTime? _ObservationTime4;
        public DateTime? ObservationTime4
        {
            get { return _ObservationTime4; }
            set
            {
                if (_ObservationTime4 != value)
                {
                    _ObservationTime4 = value;
                    OnPropertyChanged("ObservationTime4");
                }
            }
        }

        private DateTime? _ObservationDate5;
        public DateTime? ObservationDate5
        {
            get { return _ObservationDate5; }
            set
            {
                if (_ObservationDate5 != value)
                {
                    _ObservationDate5 = value;
                    OnPropertyChanged("ObservationDate5");
                }
            }
        }

        private DateTime? _ObservationTime5;
        public DateTime? ObservationTime5
        {
            get { return _ObservationTime5; }
            set
            {
                if (_ObservationTime5 != value)
                {
                    _ObservationTime5 = value;
                    OnPropertyChanged("ObservationTime5");
                }
            }
        }

        private DateTime? _ObservationDate6;
        public DateTime? ObservationDate6
        {
            get { return _ObservationDate6; }
            set
            {
                if (_ObservationDate6 != value)
                {
                    _ObservationDate6 = value;
                    OnPropertyChanged("ObservationDate6");
                }
            }
        }

        private DateTime? _ObservationTime6;
        public DateTime? ObservationTime6
        {
            get { return _ObservationTime6; }
            set
            {
                if (_ObservationTime6 != value)
                {
                    _ObservationTime6 = value;
                    OnPropertyChanged("ObservationTime6");
                }
            }
        }

        private DateTime? _ObservationDate7;
        public DateTime? ObservationDate7
        {
            get { return _ObservationDate7; }
            set
            {
                if (_ObservationDate7 != value)
                {
                    _ObservationDate7 = value;
                    OnPropertyChanged("ObservationDate7");
                }
            }
        }

        private DateTime? _ObservationTime7;
        public DateTime? ObservationTime7
        {
            get { return _ObservationTime7; }
            set
            {
                if (_ObservationTime7 != value)
                {
                    _ObservationTime7 = value;
                    OnPropertyChanged("ObservationTime7");
                }
            }
        }   
        //
        #endregion
    }

    public class ClsAddObervationEmbryo
    {
        public bool? Day { get; set; }
        public string StrDay { get; set; }
        public bool? IsFreezed { get; set; }
        public DateTime? ServerDate { get; set; }

    }
}
