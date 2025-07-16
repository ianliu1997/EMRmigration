using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsNewEmbryoTransferVO : IValueObject, INotifyPropertyChanged
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

        private long _CoupleID;
        public long CoupleID
        {
            get { return _CoupleID; }
            set
            {
                if (_CoupleID != value)
                {
                    _CoupleID = value;
                    OnPropertyChanged("CoupleID");
                }
            }
        }

        private long _TherapyID;
        public long TherapyID
        {
            get { return _TherapyID; }
            set
            {
                if (_TherapyID != value)
                {
                    _TherapyID = value;
                    OnPropertyChanged("TherapyID");
                }
            }
        }

        private long _TherapyUnitID;
        public long TherapyUnitID
        {
            get { return _TherapyUnitID; }
            set
            {
                if (_TherapyUnitID != value)
                {
                    _TherapyUnitID = value;
                    OnPropertyChanged("TherapyUnitID");
                }
            }
        }

        private long _CoupleUnitID;
        public long CoupleUnitID
        {
            get { return _CoupleUnitID; }
            set
            {
                if (_CoupleUnitID != value)
                {
                    _CoupleUnitID = value;
                    OnPropertyChanged("CoupleUnitID");
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

        private DateTime? _EmbryoTransferDateTime = DateTime.Now;
        public DateTime? EmbryoTransferDateTime
        {
            get { return _EmbryoTransferDateTime; }
            set
            {
                if (_EmbryoTransferDateTime != value)
                {
                    _EmbryoTransferDateTime = value;
                    OnPropertyChanged("EmbryoTransferDateTime");
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

        private long _AssisEmbryologistID;
        public long AssisEmbryologistID
        {
            get { return _AssisEmbryologistID; }
            set
            {
                if (_AssisEmbryologistID != value)
                {
                    _AssisEmbryologistID = value;
                    OnPropertyChanged("AssisEmbryologistID");
                }
            }
        }

        private decimal _EndometriumThickness;
        public decimal EndometriumThickness
        {
            get { return _EndometriumThickness; }
            set
            {
                if (_EndometriumThickness != value)
                {
                    _EndometriumThickness = value;
                    OnPropertyChanged("EndometriumThickness");
                }
            }
        }

        private long _PatternID;
        public long PatternID
        {
            get { return _PatternID; }
            set
            {
                if (_PatternID != value)
                {
                    _PatternID = value;
                    OnPropertyChanged("PatternID");
                }
            }
        }

        private bool _UterineArtery_PI;
        public bool UterineArtery_PI
        {
            get { return _UterineArtery_PI; }
            set
            {
                if (_UterineArtery_PI != value)
                {
                    _UterineArtery_PI = value;
                    OnPropertyChanged("UterineArtery_PI");
                }
            }
        }

        private bool _UterineArtery_RI;
        public bool UterineArtery_RI
        {
            get { return _UterineArtery_RI; }
            set
            {
                if (_UterineArtery_RI != value)
                {
                    _UterineArtery_RI = value;
                    OnPropertyChanged("UterineArtery_RI");
                }
            }
        }

        private bool _UterineArtery_SD;
        public bool UterineArtery_SD
        {
            get { return _UterineArtery_SD; }
            set
            {
                if (_UterineArtery_SD != value)
                {
                    _UterineArtery_SD = value;
                    OnPropertyChanged("UterineArtery_SD");
                }
            }
        }

        private bool _Endometerial_PI;
        public bool Endometerial_PI
        {
            get { return _Endometerial_PI; }
            set
            {
                if (_Endometerial_PI != value)
                {
                    _Endometerial_PI = value;
                    OnPropertyChanged("Endometerial_PI");
                }
            }
        }

        private bool _Endometerial_RI;
        public bool Endometerial_RI
        {
            get { return _Endometerial_RI; }
            set
            {
                if (_Endometerial_RI != value)
                {
                    _Endometerial_RI = value;
                    OnPropertyChanged("Endometerial_RI");
                }
            }
        }

        private bool _Endometerial_SD;
        public bool Endometerial_SD
        {
            get { return _Endometerial_SD; }
            set
            {
                if (_Endometerial_SD != value)
                {
                    _Endometerial_SD = value;
                    OnPropertyChanged("Endometerial_SD");
                }
            }
        }

        private long _CatheterTypeID;
        public long CatheterTypeID
        {
            get { return _CatheterTypeID; }
            set
            {
                if (_CatheterTypeID != value)
                {
                    _CatheterTypeID = value;
                    OnPropertyChanged("CatheterTypeID");
                }
            }
        }

        private decimal _DistanceFundus;
        public decimal DistanceFundus
        {
            get { return _DistanceFundus; }
            set
            {
                if (_DistanceFundus != value)
                {
                    _DistanceFundus = value;
                    OnPropertyChanged("DistanceFundus");
                }
            }
        }

        private bool _TeatmentUnderGA;
        public bool TeatmentUnderGA
        {
            get { return _TeatmentUnderGA; }
            set
            {
                if (_TeatmentUnderGA != value)
                {
                    _TeatmentUnderGA = value;
                    OnPropertyChanged("TeatmentUnderGA");
                }
            }
        }

        private bool _Difficulty;
        public bool Difficulty
        {
            get { return _Difficulty; }
            set
            {
                if (_Difficulty != value)
                {
                    _Difficulty = value;
                    OnPropertyChanged("Difficulty");
                }
            }
        }

        private bool _TenaculumUsed;
        public bool TenaculumUsed
        {
            get { return _TenaculumUsed; }
            set
            {
                if (_TenaculumUsed != value)
                {
                    _TenaculumUsed = value;
                    OnPropertyChanged("TenaculumUsed");
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

    public class clsNewEmbroyTransferDetailsVO : IValueObject, INotifyPropertyChanged
    {
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
        private long _ET_ID;
        public long ET_ID
        {
            get { return _ET_ID; }
            set
            {
                if (_ET_ID != value)
                {
                    _ET_ID = value;
                    OnPropertyChanged("ET_ID");
                }
            }
        }

        private long _TherapyID;
        public long TherapyID
        {
            get { return _TherapyID; }
            set
            {
                if (_TherapyID != value)
                {
                    _TherapyID = value;
                    OnPropertyChanged("TherapyID");
                }
            }
        }

        private long _TherapyUnitID;
        public long TherapyUnitID
        {
            get { return _TherapyUnitID; }
            set
            {
                if (_TherapyUnitID != value)
                {
                    _TherapyUnitID = value;
                    OnPropertyChanged("TherapyUnitID");
                }
            }
        }

        private long _ET_UnitID;
        public long ET_UnitID
        {
            get { return _ET_UnitID; }
            set
            {
                if (_ET_UnitID != value)
                {
                    _ET_UnitID = value;
                    OnPropertyChanged("ET_UnitID");
                }
            }
        }

        private long _EmbNo;
        public long EmbNo
        {
            get { return _EmbNo; }
            set
            {
                if (_EmbNo != value)
                {
                    _EmbNo = value;
                    OnPropertyChanged("EmbNo");
                }
            }
        }

        private string _TransferDay = string.Empty;
        public string TransferDay
        {
            get { return _TransferDay; }
            set
            {
                if (_TransferDay != value)
                {
                    _TransferDay = value;
                    OnPropertyChanged("TransferDay");
                }
            }
        }

        private DateTime? _TransferDate = DateTime.Now;
        public DateTime? AddedDateTime
        {
            get { return _TransferDate; }
            set
            {
                if (_TransferDate != value)
                {
                    _TransferDate = value;
                    OnPropertyChanged("AddedDateTime");
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

        private string _Score = string.Empty;
        public string Score
        {
            get { return _Score; }
            set
            {
                if (_Score != value)
                {
                    _Score = value;
                    OnPropertyChanged("Score");
                }
            }
        }

        private long _FertStageID;
        public long FertStageID
        {
            get { return _GradeID; }
            set
            {
                if (_FertStageID != value)
                {
                    _FertStageID = value;
                    OnPropertyChanged("FertStageID");
                }
            }
        }

        private bool _EmbStatus;
        public bool EmbStatus
        {
            get { return _EmbStatus; }
            set
            {
                _EmbStatus = value;
                OnPropertyChanged("EmbStatus");
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

    public class clsNewBirthDetailsVO : IValueObject, INotifyPropertyChanged
    {
        #region Property Delcaration
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

        private long _ChildID;
        public long ChildID
        {
            get { return _ChildID; }
            set
            {
                if (_ChildID != value)
                {
                    _ChildID = value;
                    OnPropertyChanged("ChildID");
                }
            }
        }

        private DateTime? _DateOfBirth = DateTime.Now;
        public DateTime? DateOfBirth
        {
            get { return _DateOfBirth; }
            set
            {
                if (_DateOfBirth != value)
                {
                    _DateOfBirth = value;
                    OnPropertyChanged("DateOfBirth");
                }
            }
        }

        private long _Week;
        public long Week
        {
            get { return _Week; }
            set
            {
                if (_Week != value)
                {
                    _Week = value;
                    OnPropertyChanged("Week");
                }
            }
        }

        private long _DeliveryMethodID;
        public long DeliveryMethodID
        {
            get { return _DeliveryMethodID; }
            set
            {
                if (_DeliveryMethodID != value)
                {
                    _DeliveryMethodID = value;
                    OnPropertyChanged("DeliveryMethodID");
                }
            }
        }

        private decimal _WeightAtBirth;
        public decimal WeightAtBirth
        {
            get { return _WeightAtBirth; }
            set
            {
                if (_WeightAtBirth != value)
                {
                    _WeightAtBirth = value;
                    OnPropertyChanged("WeightAtBirth");
                }
            }
        }

        private decimal _LengthAtBirth;
        public decimal LengthAtBirth
        {
            get { return _LengthAtBirth; }
            set
            {
                if (_LengthAtBirth != value)
                {
                    _LengthAtBirth = value;
                    OnPropertyChanged("LengthAtBirth");
                }
            }
        }

        private long _ConditionID;
        public long ConditionID
        {
            get { return _ConditionID; }
            set
            {
                if (_ConditionID != value)
                {
                    _ConditionID = value;
                    OnPropertyChanged("ConditionID");
                }
            }
        }

        private long _DiedPerinatallyID;
        public long DiedPerinatallyID
        {
            get { return _DiedPerinatallyID; }
            set
            {
                if (_DiedPerinatallyID != value)
                {
                    _DiedPerinatallyID = value;
                    OnPropertyChanged("DiedPerinatallyID");
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

        private string _Surname;
        public string Surname
        {
            get { return _Surname; }
            set
            {
                if (_Surname != value)
                {
                    _Surname = value;
                    OnPropertyChanged("Surname");
                }
            }
        }
        private string _TownOfBirth;
        public string TownOfBirth
        {
            get { return _TownOfBirth; }
            set
            {
                if (_TownOfBirth != value)
                {
                    _TownOfBirth = value;
                    OnPropertyChanged("TownOfBirth");
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

        private long _CountryOfBirthID;
        public long CountryOfBirthID
        {
            get { return _DiedPerinatallyID; }
            set
            {
                if (_CountryOfBirthID != value)
                {
                    _CountryOfBirthID = value;
                    OnPropertyChanged("CountryOfBirthID");
                }
            }
        }

        private long _DeathPostportumID;
        public long DeathPostportumID
        {
            get { return _DeathPostportumID; }
            set
            {
                if (_DeathPostportumID != value)
                {
                    _DeathPostportumID = value;
                    OnPropertyChanged("DeathPostportumID");
                }
            }
        }

        private long _TherapyID;
        public long TherapyID
        {
            get { return _TherapyID; }
            set
            {
                if (_TherapyID != value)
                {
                    _TherapyID = value;
                    OnPropertyChanged("TherapyID");
                }
            }
        }

        private long _TherapyUnitID;
        public long TherapyUnitID
        {
            get { return _TherapyUnitID; }
            set
            {
                if (_TherapyUnitID != value)
                {
                    _TherapyUnitID = value;
                    OnPropertyChanged("TherapyUnitID");
                }
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

    public class clsNewOutcomeVO : IValueObject, INotifyPropertyChanged
    {
        #region Property Delcaration
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

        private DateTime? _Beta_HCG_Assignment1_Date;
        public DateTime? Beta_HCG_Assignment1_Date
        {
            get { return _Beta_HCG_Assignment1_Date; }
            set
            {
                if (_Beta_HCG_Assignment1_Date != value)
                {
                    _Beta_HCG_Assignment1_Date = value;
                    OnPropertyChanged("Beta_HCG_Assignment1_Date");
                }
            }
        }

        private bool _Beta_HCG_Assignment1_Positive;
        public bool Beta_HCG_Assignment1_Positive
        {
            get { return _Beta_HCG_Assignment1_Positive; }
            set
            {
                if (_Beta_HCG_Assignment1_Positive != value)
                {
                    _Beta_HCG_Assignment1_Positive = value;
                    OnPropertyChanged("Beta_HCG_Assignment1_Positive");
                }
            }
        }


        private bool _Beta_HCG_Assignment1_Negative;
        public bool Beta_HCG_Assignment1_Negative
        {
            get { return _Beta_HCG_Assignment1_Negative; }
            set
            {
                if (_Beta_HCG_Assignment1_Negative != value)
                {
                    _Beta_HCG_Assignment1_Negative = value;
                    OnPropertyChanged("Beta_HCG_Assignment1_Negative");
                }
            }
        }

        private decimal _Beta_HCG_Assignment1_Value;
        public decimal Beta_HCG_Assignment1_Value
        {
            get { return _Beta_HCG_Assignment1_Value; }
            set
            {
                if (_Beta_HCG_Assignment1_Value != value)
                {
                    _Beta_HCG_Assignment1_Value = value;
                    OnPropertyChanged("Beta_HCG_Assignment1_Value");
                }
            }
        }

        private string _Beta_HCG_Assignment1_SR_Progest;
        public string Beta_HCG_Assignment1_SR_Progest
        {
            get { return _Beta_HCG_Assignment1_SR_Progest; }
            set
            {
                if (_Beta_HCG_Assignment1_SR_Progest != value)
                {
                    _Beta_HCG_Assignment1_SR_Progest = value;
                    OnPropertyChanged("Beta_HCG_Assignment1_SR_Progest");
                }
            }
        }

        private DateTime? _Beta_HCG_Assignment2_Date;
        public DateTime? Beta_HCG_Assignment2_Date
        {
            get { return _Beta_HCG_Assignment2_Date; }
            set
            {
                if (_Beta_HCG_Assignment2_Date != value)
                {
                    _Beta_HCG_Assignment2_Date = value;
                    OnPropertyChanged("Beta_HCG_Assignment2_Date");
                }
            }
        }

        private bool _Beta_HCG_Assignment2_Positive;
        public bool Beta_HCG_Assignment2_Positive
        {
            get { return _Beta_HCG_Assignment2_Positive; }
            set
            {
                if (_Beta_HCG_Assignment2_Positive != value)
                {
                    _Beta_HCG_Assignment2_Positive = value;
                    OnPropertyChanged("Beta_HCG_Assignment2_Positive");
                }
            }
        }

        private bool _Beta_HCG_Assignment2_Negative;
        public bool Beta_HCG_Assignment2_Negative
        {
            get { return _Beta_HCG_Assignment2_Negative; }
            set
            {
                if (_Beta_HCG_Assignment2_Negative != value)
                {
                    _Beta_HCG_Assignment2_Negative = value;
                    OnPropertyChanged("Beta_HCG_Assignment2_Negative");
                }
            }
        }
        private string _Beta_HCG_Assignment2_Value;
        public string Beta_HCG_Assignment2_Value
        {
            get { return _Beta_HCG_Assignment2_Value; }
            set
            {
                if (_Beta_HCG_Assignment2_Value != value)
                {
                    _Beta_HCG_Assignment2_Value = value;
                    OnPropertyChanged("Beta_HCG_Assignment2_Value");
                }
            }
        }

        private string _Beta_HCS_Assignment_USG;
        public string Beta_HCS_Assignment_USG
        {
            get { return _Beta_HCS_Assignment_USG; }
            set
            {
                if (_Beta_HCS_Assignment_USG != value)
                {
                    _Beta_HCS_Assignment_USG = value;
                    OnPropertyChanged("Beta_HCS_Assignment_USG");
                }
            }
        }

        private bool _Pregnancy_Achieved;
        public bool Pregnancy_Achieved
        {
            get { return _Pregnancy_Achieved; }
            set
            {
                if (_Pregnancy_Achieved != value)
                {
                    _Pregnancy_Achieved = value;
                    OnPropertyChanged("Pregnancy_Achieved");
                }
            }
        }

        private DateTime? _PregnancyConfDate;
        public DateTime? PregnancyConfDate
        {
            get { return _PregnancyConfDate; }
            set
            {
                if (_PregnancyConfDate != value)
                {
                    _PregnancyConfDate = value;
                    OnPropertyChanged("PregnancyConfDate");
                }
            }
        }
        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set
            {
                if (_IsClosed != value)
                {
                    _IsClosed = value;
                    OnPropertyChanged("IsClosed");
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

        private bool _BiochemicalPreg;
        public bool BiochemicalPreg
        {
            get { return _BiochemicalPreg; }
            set
            {
                if (_BiochemicalPreg != value)
                {
                    _BiochemicalPreg = value;
                    OnPropertyChanged("BiochemicalPreg");
                }
            }
        }

        private bool _FetalJeartSound;
        public bool FetalJeartSound
        {
            get { return _FetalJeartSound; }
            set
            {
                if (_FetalJeartSound != value)
                {
                    _FetalJeartSound = value;
                    OnPropertyChanged("FetalJeartSound");
                }
            }
        }

        private bool _IUD;
        public bool IUD
        {
            get { return _IUD; }
            set
            {
                if (_IUD != value)
                {
                    _IUD = value;
                    OnPropertyChanged("IUD");
                }
            }
        }

        private bool _PreTermPreg;
        public bool PreTermPreg
        {
            get { return _PreTermPreg; }
            set
            {
                if (_PreTermPreg != value)
                {
                    _PreTermPreg = value;
                    OnPropertyChanged("PreTermPreg");
                }
            }
        }

        private bool _ChemicalPreg;
        public bool ChemicalPreg
        {
            get { return _ChemicalPreg; }
            set
            {
                if (_ChemicalPreg != value)
                {
                    _ChemicalPreg = value;
                    OnPropertyChanged("ChemicalPreg");
                }
            }
        }

        private bool _FullTermPreg;
        public bool FullTermPreg
        {
            get { return _FullTermPreg; }
            set
            {
                if (_FullTermPreg != value)
                {
                    _FullTermPreg = value;
                    OnPropertyChanged("FullTermPreg");
                }
            }
        }
        private bool _Abortion;
        public bool Abortion
        {
            get { return _Abortion; }
            set
            {
                if (_Abortion != value)
                {
                    _Abortion = value;
                    OnPropertyChanged("Abortion");
                }
            }
        }

        private bool _LiveBirth;
        public bool LiveBirth
        {
            get { return _LiveBirth; }
            set
            {
                if (_LiveBirth != value)
                {
                    _LiveBirth = value;
                    OnPropertyChanged("LiveBirth");
                }
            }
        }

        private bool _Ectopic;
        public bool Ectopic
        {
            get { return _Ectopic; }
            set
            {
                if (_Ectopic != value)
                {
                    _Ectopic = value;
                    OnPropertyChanged("Ectopic");
                }
            }
        }
        private bool _CongenitalAbnorm;
        public bool CongenitalAbnorm
        {
            get { return _CongenitalAbnorm; }
            set
            {
                if (_CongenitalAbnorm != value)
                {
                    _CongenitalAbnorm = value;
                    OnPropertyChanged("CongenitalAbnorm");
                }
            }
        }


        
        private bool _Mode;
        public bool Mode
        {
            get { return _Mode; }
            set
            {
                if (_Mode != value)
                {
                    _Mode = value;
                    OnPropertyChanged("Mode");
                }
            }
        }

        private bool _Severe;
        public bool Severe
        {
            get { return _Severe; }
            set
            {
                if (_Severe != value)
                {
                    _Severe = value;
                    OnPropertyChanged("Severe");
                }
            }
        }

        private string _OHSS_Complications_Remarks;
        public string OHSS_Complications_Remarks
        {
            get { return _OHSS_Complications_Remarks; }
            set
            {
                if (_OHSS_Complications_Remarks != value)
                {
                    _OHSS_Complications_Remarks = value;
                    OnPropertyChanged("OHSS_Complications_Remarks");
                }
            }
        }

        private string _LutealSupport;
        public string LutealSupport
        {
            get { return _LutealSupport; }
            set
            {
                if (_LutealSupport != value)
                {
                    _LutealSupport = value;
                    OnPropertyChanged("LutealSupport");
                }
            }
        }

        private string _LutealSupport_Remark;
        public string LutealSupport_Remark
        {
            get { return _LutealSupport_Remark; }
            set
            {
                if (_LutealSupport_Remark != value)
                {
                    _LutealSupport_Remark = value;
                    OnPropertyChanged("_LutealSupport_Remark");
                }
            }
        }
        
        private bool _Mild;
        public bool Mild
        {
            get { return _Mild; }
            set
            {
                if (_Mild != value)
                {
                    _Mild = value;
                    OnPropertyChanged("Mild");
                }
            }
        }


        private bool _Late;
        public bool Late
        {
            get { return _Late; }
            set
            {
                if (_Late != value)
                {
                    _Late = value;
                    OnPropertyChanged("Late");
                }
            }
        }


        private bool _Early;
        public bool Early
        {
            get { return Early; }
            set
            {
                if (Early != value)
                {
                    _Early = value;
                    OnPropertyChanged("Early");
                }
            }
        }

        
        private long _Count;
        public long Count
        {
            get { return _Count; }
            set
            {
                if (_Count != value)
                {
                    _Count = value;
                    OnPropertyChanged("Count");
                }
            }
        }
        private long _MultiplePregID;
        public long MultiplePregID
        {
            get { return _MultiplePregID; }
            set
            {
                if (_MultiplePregID != value)
                {
                    _MultiplePregID = value;
                    OnPropertyChanged("MultiplePregID");
                }
            }
        }
        
        private DateTime? _OnDate;
        public DateTime? OnDate
        {
            get { return _OnDate; }
            set
            {
                if (_OnDate != value)
                {
                    _OnDate = value;
                    OnPropertyChanged("OnDate");
                }
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
