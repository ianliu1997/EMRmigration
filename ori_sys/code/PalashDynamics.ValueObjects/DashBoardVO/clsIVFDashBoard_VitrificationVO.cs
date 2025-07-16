using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
//using System.Windows.Media;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashBoard_VitrificationVO : IValueObject, INotifyPropertyChanged
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

        private Boolean _Status;
        public Boolean Status
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

        private DateTime? _AddedDateTime;
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

        private DateTime? _UpdatedDateTime;
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

        //added by neena
        private long _DonateCryoID;
        public long DonateCryoID
        {
            get { return _DonateCryoID; }
            set
            {
                if (_DonateCryoID != value)
                {
                    _DonateCryoID = value;
                    OnPropertyChanged("DonateCryoID");
                }
            }
        }
        //

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
        private long _DonorPatientID;
        public long DonorPatientID
        {
            get { return _DonorPatientID; }
            set
            {
                if (_DonorPatientID != value)
                {
                    _DonorPatientID = value;
                    OnPropertyChanged("DonorPatientID");
                }
            }
        }

        private long _DonorPatientUnitID;
        public long DonorPatientUnitID
        {
            get { return _DonorPatientUnitID; }
            set
            {
                if (_DonorPatientUnitID != value)
                {
                    _DonorPatientUnitID = value;
                    OnPropertyChanged("DonorPatientUnitID");
                }
            }
        }
        private DateTime? _ReceivingDate;
        public DateTime? ReceivingDate
        {
            get { return _ReceivingDate; }
            set
            {
                if (_ReceivingDate != value)
                {
                    _ReceivingDate = value;
                    OnPropertyChanged("ReceivingDate");
                }
            }
        }
        private string _VitrificationNo;
        public string VitrificationNo
        {
            get { return _VitrificationNo; }
            set
            {
                if (_VitrificationNo != value)
                {
                    _VitrificationNo = value;
                    OnPropertyChanged("VitrificationNo");
                }
            }
        }
        private string _SerialOocyteNumberString;
        public string SerialOocyteNumberString
        {
            get { return _SerialOocyteNumberString; }
            set
            {
                if (_SerialOocyteNumberString != value)
                {
                    _SerialOocyteNumberString = value;
                    OnPropertyChanged("SerialOocyteNumberString");
                }
            }
        }
        private DateTime? _DateTime;
        public DateTime? DateTime
        {
            get { return _DateTime; }
            set
            {
                if (_DateTime != value)
                {
                    _DateTime = value;
                    OnPropertyChanged("DateTime");
                }
            }
        }

        private DateTime? _ExpiryDate;
        public DateTime? ExpiryDate
        {
            get { return _ExpiryDate; }
            set
            {
                if (_ExpiryDate != value)
                {
                    _ExpiryDate = value;
                    OnPropertyChanged("ExpiryDate");
                }
            }
        }


        private bool _UsedByOtherCycle;
        public bool UsedByOtherCycle
        {
            get { return _UsedByOtherCycle; }
            set
            {
                if (_UsedByOtherCycle != value)
                {
                    _UsedByOtherCycle = value;
                    OnPropertyChanged("UsedByOtherCycle");
                }
            }
        }
        private long _UsedTherapyID;
        public long UsedTherapyID
        {
            get { return _UsedTherapyID; }
            set
            {
                if (_UsedTherapyID != value)
                {
                    _UsedTherapyID = value;
                    OnPropertyChanged("UsedTherapyID");
                }
            }
        }

        private long _UsedTherapyUnitID;
        public long UsedTherapyUnitID
        {
            get { return _UsedTherapyUnitID; }
            set
            {
                if (_UsedTherapyUnitID != value)
                {
                    _UsedTherapyUnitID = value;
                    OnPropertyChanged("UsedTherapyUnitID");
                }
            }
        }
        private DateTime? _PickUpDate;
        public DateTime? PickUpDate
        {
            get { return _PickUpDate; }
            set
            {
                if (_PickUpDate != value)
                {
                    _PickUpDate = value;
                    OnPropertyChanged("PickUpDate");
                }
            }
        }

        private bool _ConsentForm;
        public bool ConsentForm
        {
            get { return _ConsentForm; }
            set
            {
                if (_ConsentForm != value)
                {
                    _ConsentForm = value;
                    OnPropertyChanged("ConsentForm");
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

        private bool _IsThawingDone;
        public bool IsThawingDone
        {
            get { return _IsThawingDone; }
            set
            {
                if (_IsThawingDone != value)
                {
                    _IsThawingDone = value;
                    OnPropertyChanged("IsThawingDone");
                }
            }
        }
        private bool _IsOnlyVitrification;
        public bool IsOnlyVitrification
        {
            get { return _IsOnlyVitrification; }
            set
            {
                if (_IsOnlyVitrification != value)
                {
                    _IsOnlyVitrification = value;
                    OnPropertyChanged("IsThawingDone");
                }
            }
        }

        // Flag use to save only Vitrification\Cryo details from IVF/ICSI/IVF-ICSI/FE ICSI cycles which will be thaw under Freeze All Oocyte/Freeze-Thaw Transfer cycles
        private bool _IsCryoWOThaw;
        public bool IsCryoWOThaw
        {
            get { return _IsCryoWOThaw; }
            set
            {
                if (_IsCryoWOThaw != value)
                {
                    _IsCryoWOThaw = value;
                    OnPropertyChanged("IsCryoWOThaw");
                }
            }
        }

        // Added By CDS 

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

        private long _AssitantEmbryologistID;
        public long AssitantEmbryologistID
        {
            get { return _AssitantEmbryologistID; }
            set
            {
                if (_AssitantEmbryologistID != value)
                {
                    _AssitantEmbryologistID = value;
                    OnPropertyChanged("AssitantEmbryologistID");
                }
            }
        }
        private bool _IsOnlyForEmbryoVitrification;
        public bool IsOnlyForEmbryoVitrification
        {
            get { return _IsOnlyForEmbryoVitrification; }
            set
            {
                if (_IsOnlyForEmbryoVitrification != value)
                {
                    _IsOnlyForEmbryoVitrification = value;
                    OnPropertyChanged("IsOnlyForEmbryoVitrification");
                }
            }
        }

        private bool _SaveForSingleEntry;
        public bool SaveForSingleEntry
        {
            get { return _SaveForSingleEntry; }
            set
            {
                if (_SaveForSingleEntry != value)
                {
                    _SaveForSingleEntry = value;
                    OnPropertyChanged("SaveForSingleEntry");
                }
            }
        }
        //END 

        private long _SrcOoctyID;
        public long SrcOoctyID
        {
            get { return _SrcOoctyID; }
            set
            {
                if (_SrcOoctyID != value)
                {
                    _SrcOoctyID = value;
                    OnPropertyChanged("SrcOoctyID");
                }
            }
        }
        private long _SrcSemenID;
        public long SrcSemenID
        {
            get { return _SrcSemenID; }
            set
            {
                if (_SrcSemenID != value)
                {
                    _SrcSemenID = value;
                    OnPropertyChanged("SrcSemenID");
                }
            }
        }
        private string _SrcOoctyCode;
        public string SrcOoctyCode
        {
            get { return _SrcOoctyCode; }
            set
            {
                if (_SrcOoctyCode != value)
                {
                    _SrcOoctyCode = value;
                    OnPropertyChanged("SrcOoctyCode");
                }
            }
        }
        private string _SrcSemenCode;
        public string SrcSemenCode
        {
            get { return _SrcSemenCode; }
            set
            {
                if (_SrcSemenCode != value)
                {
                    _SrcSemenCode = value;
                    OnPropertyChanged("SrcSemenCode");
                }
            }
        }
        private bool _UsedOwnOocyte;
        public bool UsedOwnOocyte
        {
            get { return _UsedOwnOocyte; }
            set
            {
                if (_UsedOwnOocyte != value)
                {
                    _UsedOwnOocyte = value;
                    OnPropertyChanged("UsedOwnOocyte");
                }
            }
        }

        public bool _IsRefeeze;
        public bool IsRefeeze
        {
            get { return _IsRefeeze; }
            set
            {
                if (_IsRefeeze != value)
                {
                    _IsRefeeze = value;
                    OnPropertyChanged("IsRefeeze");
                }
            }
        }


        #endregion
    }

    public class clsIVFDashBoard_VitrificationDetailsVO : IValueObject, INotifyPropertyChanged
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
        //private DateTime? _VitrificationDate;
        //public DateTime? VitrificationDate
        //{
        //    get { return _VitrificationDate; }
        //    set
        //    {
        //        if (_VitrificationDate != value)
        //        {
        //            _VitrificationDate = value;
        //            OnPropertyChanged("VitrificationDate");
        //        }
        //    }
        //} 

        private DateTime? _VitrificationTime;
        public DateTime? VitrificationTime
        {
            get { return _VitrificationTime; }
            set
            {
                if (_VitrificationTime != value)
                {
                    _VitrificationTime = value;
                    OnPropertyChanged("VitrificationTime");
                }
            }
        }

        private DateTime? _ExpiryDate;
        public DateTime? ExpiryDate
        {
            get { return _ExpiryDate; }
            set
            {
                if (_ExpiryDate != value)
                {
                    _ExpiryDate = value;
                    OnPropertyChanged("ExpiryDate");
                }
            }
        }

        private DateTime? _ExpiryTime;
        public DateTime? ExpiryTime
        {
            get { return _ExpiryTime; }
            set
            {
                if (_ExpiryTime != value)
                {
                    _ExpiryTime = value;
                    OnPropertyChanged("ExpiryTime");
                }
            }
        }

        private string _PGDPGS;
        public string PGDPGS
        {
            get { return _PGDPGS; }
            set
            {
                if (_PGDPGS != value)
                {
                    _PGDPGS = value;
                    OnPropertyChanged("PGDPGS");
                }
            }
        }

        private bool _IsFreshEmbryoPGDPGS;
        public bool IsFreshEmbryoPGDPGS
        {
            get { return _IsFreshEmbryoPGDPGS; }
            set
            {
                if (_IsFreshEmbryoPGDPGS != value)
                {
                    _IsFreshEmbryoPGDPGS = value;
                    OnPropertyChanged("IsFreshEmbryoPGDPGS");
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

        public bool LongTerm { get; set; }
        public bool ShortTerm { get; set; }
        public string Type { get; set; }

        private DateTime? _ReceivingDate;
        public DateTime? ReceivingDate
        {
            get { return _ReceivingDate; }
            set
            {
                if (_ReceivingDate != value)
                {
                    _ReceivingDate = value;
                    OnPropertyChanged("ReceivingDate");
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
        private long _RecepientPatientID;
        public long RecepientPatientID
        {
            get { return _RecepientPatientID; }
            set
            {
                if (_RecepientPatientID != value)
                {
                    _RecepientPatientID = value;
                    OnPropertyChanged("RecepientPatientID");
                }
            }
        }
        private long _RecepientPatientUnitID;
        public long RecepientPatientUnitID
        {
            get { return _RecepientPatientUnitID; }
            set
            {
                if (_RecepientPatientUnitID != value)
                {
                    _RecepientPatientUnitID = value;
                    OnPropertyChanged("RecepientPatientUnitID");
                }
            }
        }

        private long _DonorPatientID;
        public long DonorPatientID
        {
            get { return _DonorPatientID; }
            set
            {
                if (_DonorPatientID != value)
                {
                    _DonorPatientID = value;
                    OnPropertyChanged("DonorPatientID");
                }
            }
        }
        private long _DonorPatientUnitID;
        public long DonorPatientUnitID
        {
            get { return _DonorPatientUnitID; }
            set
            {
                if (_DonorPatientUnitID != value)
                {
                    _DonorPatientUnitID = value;
                    OnPropertyChanged("DonorPatientUnitID");
                }
            }
        }

        private bool _IsDonorCycleDonateCryo;
        public bool IsDonorCycleDonateCryo
        {
            get { return _IsDonorCycleDonateCryo; }
            set
            {
                if (_IsDonorCycleDonateCryo != value)
                {
                    _IsDonorCycleDonateCryo = value;
                    OnPropertyChanged("IsDonorCycleDonateCryo");
                }
            }
        }

        private long _VitrivicationID;
        public long VitrivicationID
        {
            get { return _VitrivicationID; }
            set
            {
                if (_VitrivicationID != value)
                {
                    _VitrivicationID = value;
                    OnPropertyChanged("VitrivicationID");
                }
            }
        }

        private long _VitrificationUnitID;
        public long VitrificationUnitID
        {
            get { return _VitrificationUnitID; }
            set
            {
                if (_VitrificationUnitID != value)
                {
                    _VitrificationUnitID = value;
                    OnPropertyChanged("VitrificationUnitID");
                }
            }
        }

        private long _LabDayID;
        public long LabDayID
        {
            get { return _LabDayID; }
            set
            {
                if (_LabDayID != value)
                {
                    _LabDayID = value;
                    OnPropertyChanged("LabDayID");
                }
            }
        }

        private long _EmbNumber;
        public long EmbNumber
        {
            get { return _EmbNumber; }
            set
            {
                if (_EmbNumber != value)
                {
                    _EmbNumber = value;
                    OnPropertyChanged("EmbNumber");
                }
            }
        }
        private string _OocNo;
        public string OocNo
        {
            get { return _OocNo; }
            set
            {
                if (_OocNo != value)
                {
                    _OocNo = value;
                    OnPropertyChanged("OocNo");
                }
            }
        }
        private long _EmbSerialNumber;
        public long EmbSerialNumber
        {
            get { return _EmbSerialNumber; }
            set
            {
                if (_EmbSerialNumber != value)
                {
                    _EmbSerialNumber = value;
                    OnPropertyChanged("EmbSerialNumber");
                }
            }
        }

        private string _LeafNo;
        public string LeafNo
        {
            get { return _LeafNo; }
            set
            {
                if (_LeafNo != value)
                {
                    _LeafNo = value;
                    OnPropertyChanged("LeafNo");
                }
            }
        }

        private string _EmbDays;
        public string EmbDays
        {
            get { return _EmbDays; }
            set
            {
                if (_EmbDays != value)
                {
                    _EmbDays = value;
                    OnPropertyChanged("EmbDays");
                }
            }
        }

        private long _ColorCodeID;
        public long ColorCodeID
        {
            get { return _ColorCodeID; }
            set
            {
                if (_ColorCodeID != value)
                {
                    _ColorCodeID = value;
                    OnPropertyChanged("ColorCodeID");
                }
            }
        }

        private string _ColorCode;
        public string ColorCode
        {
            get { return _ColorCode; }
            set
            {
                if (_ColorCode != value)
                {
                    _ColorCode = value;
                    OnPropertyChanged("ColorCode");
                }
            }
        }

        public string PatientName { get; set; }
        public string MRNo { get; set; }

        public string DonorPatientName { get; set; }
        public string DonorMRNo { get; set; }

        private long _CanId;
        public long CanId
        {
            get { return _CanId; }
            set
            {
                if (_CanId != value)
                {
                    _CanId = value;
                    OnPropertyChanged("CanId");
                }
            }
        }

        private string _Can;
        public string Can
        {
            get { return _Can; }
            set
            {
                if (_Can != value)
                {
                    _Can = value;
                    OnPropertyChanged("Can");
                }
            }
        }

        private long _StrawId;
        public long StrawId
        {
            get { return _StrawId; }
            set
            {
                if (_StrawId != value)
                {
                    _StrawId = value;
                    OnPropertyChanged("StrawId");
                }
            }
        }

        private string _Straw;
        public string Straw
        {
            get { return _Straw; }
            set
            {
                if (_Straw != value)
                {
                    _Straw = value;
                    OnPropertyChanged("Straw");
                }
            }
        }

        private long _GobletShapeId;
        public long GobletShapeId
        {
            get { return _GobletShapeId; }
            set
            {
                if (_GobletShapeId != value)
                {
                    _GobletShapeId = value;
                    OnPropertyChanged("GobletShapeId");
                }
            }
        }

        private string _GobletShape;
        public string GobletShape
        {
            get { return _GobletShape; }
            set
            {
                if (_GobletShape != value)
                {
                    _GobletShape = value;
                    OnPropertyChanged("GobletShape");
                }
            }
        }

        private long _GobletSizeId;
        public long GobletSizeId
        {
            get { return _GobletSizeId; }
            set
            {
                if (_GobletSizeId != value)
                {
                    _GobletSizeId = value;
                    OnPropertyChanged("GobletSizeId");
                }
            }
        }

        private string _GobletSize;
        public string GobletSize
        {
            get { return _GobletSize; }
            set
            {
                if (_GobletSize != value)
                {
                    _GobletSize = value;
                    OnPropertyChanged("GobletSize");
                }
            }
        }

        private long _TankId;
        public long TankId
        {
            get { return _TankId; }
            set
            {
                if (_TankId != value)
                {
                    _TankId = value;
                    OnPropertyChanged("TankId");
                }
            }
        }

        private string _Tank;
        public string Tank
        {
            get { return _Tank; }
            set
            {
                if (_Tank != value)
                {
                    _Tank = value;
                    OnPropertyChanged("Tank");
                }
            }
        }

        private long _ConistorNo;
        public long ConistorNo
        {
            get { return _ConistorNo; }
            set
            {
                if (_ConistorNo != value)
                {
                    _ConistorNo = value;
                    OnPropertyChanged("ConistorNo");
                }
            }
        }

        private string _Conistor;
        public string Conistor
        {
            get { return _Conistor; }
            set
            {
                if (_Conistor != value)
                {
                    _Conistor = value;
                    OnPropertyChanged("Conistor");
                }
            }
        }

        //ProtocolTypeID	bigint chnge in DB
        private long _ProtocolTypeID;
        public long ProtocolTypeID
        {
            get { return _ProtocolTypeID; }
            set
            {
                if (_ProtocolTypeID != value)
                {
                    _ProtocolTypeID = value;
                    OnPropertyChanged("ProtocolTypeID");
                }
            }
        }
        //   CellStageID	

        private long _CellStageID;
        public long CellStageID
        {
            get { return _CellStageID; }
            set
            {
                if (_CellStageID != value)
                {
                    _CellStageID = value;
                    OnPropertyChanged("CellStageID");
                }
            }
        }
        //   GradeID	
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

        private long _OocyteGradeID;
        public long OocyteGradeID
        {
            get { return _OocyteGradeID; }
            set
            {
                if (_OocyteGradeID != value)
                {
                    _OocyteGradeID = value;
                    OnPropertyChanged("OocyteGradeID");
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

        private string _StageofDevelopmentGradeStr;
        public string StageofDevelopmentGradeStr
        {
            get { return _StageofDevelopmentGradeStr; }
            set
            {
                if (_StageofDevelopmentGradeStr != value)
                {
                    _StageofDevelopmentGradeStr = value;
                    OnPropertyChanged("StageofDevelopmentGradeStr");
                }
            }
        }

        private string _InnerCellMassGradeStr;
        public string InnerCellMassGradeStr
        {
            get { return _InnerCellMassGradeStr; }
            set
            {
                if (_InnerCellMassGradeStr != value)
                {
                    _InnerCellMassGradeStr = value;
                    OnPropertyChanged("InnerCellMassGradeStr");
                }
            }
        }

        private string _TrophoectodermGradeStr;
        public string TrophoectodermGradeStr
        {
            get { return _TrophoectodermGradeStr; }
            set
            {
                if (_TrophoectodermGradeStr != value)
                {
                    _TrophoectodermGradeStr = value;
                    OnPropertyChanged("TrophoectodermGradeStr");
                }
            }
        }

        private bool _IsSaved;
        public bool IsSaved
        {
            get { return _IsSaved; }
            set
            {
                if (_IsSaved != value)
                {
                    _IsSaved = value;
                    OnPropertyChanged("IsSaved");
                }
            }
        }

        private bool _IsRefreezeFromOtherCycle;
        public bool IsRefreezeFromOtherCycle
        {
            get { return _IsRefreezeFromOtherCycle; }
            set
            {
                if (_IsRefreezeFromOtherCycle != value)
                {
                    _IsRefreezeFromOtherCycle = value;
                    OnPropertyChanged("IsRefreezeFromOtherCycle");
                }
            }
        }

        private bool _IsRefreeze;
        public bool IsRefreeze
        {
            get { return _IsRefreeze; }
            set
            {
                if (_IsRefreeze != value)
                {
                    _IsRefreeze = value;
                    OnPropertyChanged("IsRefreeze");
                }
            }
        }

        private bool _IsCheckRefreeze;
        public bool IsCheckRefreeze
        {
            get { return _IsCheckRefreeze; }
            set
            {
                if (_IsCheckRefreeze != value)
                {
                    _IsCheckRefreeze = value;
                    OnPropertyChanged("IsCheckRefreeze");
                }
            }
        }


        private string _IsRefreezeFromBothCycle;
        public string IsRefreezeFromBothCycle
        {
            get { return _IsRefreezeFromBothCycle; }
            set
            {
                if (_IsRefreezeFromBothCycle != value)
                {
                    _IsRefreezeFromBothCycle = value;
                    OnPropertyChanged("IsRefreezeFromBothCycle");
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

        private bool _IsDonate;
        public bool IsDonate
        {
            get { return _IsDonate; }
            set
            {
                if (_IsDonate != value)
                {
                    _IsDonate = value;
                    OnPropertyChanged("IsDonate");
                }
            }
        }

        private bool _IsDonateCryo;
        public bool IsDonateCryo
        {
            get { return _IsDonateCryo; }
            set
            {
                if (_IsDonateCryo != value)
                {
                    _IsDonateCryo = value;
                    OnPropertyChanged("IsDonateCryo");
                }
            }
        }

        private bool _IsDonatedCryoReceived;
        public bool IsDonatedCryoReceived
        {
            get { return _IsDonatedCryoReceived; }
            set
            {
                if (_IsDonatedCryoReceived != value)
                {
                    _IsDonatedCryoReceived = value;
                    OnPropertyChanged("IsDonatedCryoReceived");
                }
            }
        }    

        private string _SOOCytes;
        public string SOOCytes
        {
            get { return _SOOCytes; }
            set
            {
                if (_SOOCytes != value)
                {
                    _SOOCytes = value;
                    OnPropertyChanged("SOOCytes");
                }
            }
        }

        private string _OSCode;
        public string OSCode
        {
            get { return _OSCode; }
            set
            {
                if (_OSCode != value)
                {
                    _OSCode = value;
                    OnPropertyChanged("OSCode");
                }
            }
        }

        private string _OSSemen;
        public string OSSemen
        {
            get { return _OSSemen; }
            set
            {
                if (_OSSemen != value)
                {
                    _OSSemen = value;
                    OnPropertyChanged("OSSemen");
                }
            }
        }

        private string _SSCode;
        public string SSCode
        {
            get { return _SSCode; }
            set
            {
                if (_SSCode != value)
                {
                    _SSCode = value;
                    OnPropertyChanged("SSCode");
                }
            }
        }

        private string _TransferDay;
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

        private string _VitrificationNo;
        public string VitrificationNo
        {
            get { return _VitrificationNo; }
            set
            {
                if (_VitrificationNo != value)
                {
                    _VitrificationNo = value;
                    OnPropertyChanged("VitrificationNo");
                }
            }
        }

        #region // For IVF ADM Changes

        private bool _IsFreezeOocytes;      //Flag for fetching Freeze Oocytes under Freeze All Oocytes Cycle 
        public bool IsFreezeOocytes
        {
            get { return _IsFreezeOocytes; }
            set
            {
                if (_IsFreezeOocytes != value)
                {
                    _IsFreezeOocytes = value;
                    OnPropertyChanged("IsFreezeOocytes");
                }
            }
        }

        private string _CryoCode;        //Grade
        public string CryoCode
        {
            get { return _CryoCode; }
            set
            {
                if (_CryoCode != value)
                {
                    _CryoCode = value;
                    OnPropertyChanged("CryoCode");
                }
            }
        }

        private DateTime? _TransferDate;
        public DateTime? TransferDate
        {
            get { return _TransferDate; }
            set
            {
                if (_TransferDate != value)
                {
                    _TransferDate = value;
                    OnPropertyChanged("TransferDate");
                }
            }
        }

        private DateTime? _DateTime;
        public DateTime? DateTime
        {
            get { return _DateTime; }
            set
            {
                if (_DateTime != value)
                {
                    _DateTime = value;
                    OnPropertyChanged("DateTime");
                }
            }
        }

        private DateTime? _VitrificationDate;
        public DateTime? VitrificationDate
        {
            get { return _VitrificationDate; }
            set
            {
                if (_VitrificationDate != value)
                {
                    _VitrificationDate = value;
                    OnPropertyChanged("VitrificationDate");
                }
            }
        }

        private string _EmbStatus;
        public string EmbStatus
        {
            get { return _EmbStatus; }
            set
            {
                if (_EmbStatus != value)
                {
                    _EmbStatus = value;
                    OnPropertyChanged("EmbStatus");
                }
            }
        }

        private bool _IsThawingDone;
        public bool IsThawingDone
        {
            get { return _IsThawingDone; }
            set
            {
                if (_IsThawingDone != value)
                {
                    _IsThawingDone = value;
                    OnPropertyChanged("IsThawingDone");
                }
            }
        }

        private bool _IsOnlyVitrification;
        public bool IsOnlyVitrification
        {
            get { return _IsOnlyVitrification; }
            set
            {
                if (_IsOnlyVitrification != value)
                {
                    _IsOnlyVitrification = value;
                    OnPropertyChanged("IsOnlyVitrification");
                }
            }
        }

        //added by neena
        private bool _IsEnabled;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set
            {
                if (_IsEnabled != value)
                {
                    _IsEnabled = value;
                    OnPropertyChanged("IsEnabled");
                }
            }
        }

        private string _IsImgVisible;
        public string IsImgVisible
        {
            get { return _IsImgVisible; }
            set
            {
                if (_IsImgVisible != value)
                {
                    _IsImgVisible = value;
                    OnPropertyChanged("IsImgVisible");
                }
            }
        }

        private string _IsImg1Visible;
        public string IsImg1Visible
        {
            get { return _IsImg1Visible; }
            set
            {
                if (_IsImg1Visible != value)
                {
                    _IsImg1Visible = value;
                    OnPropertyChanged("IsImg1Visible");
                }
            }
        }
        //

        private string _Comments;
        public string Comments
        {
            get { return _Comments; }
            set
            {
                if (_Comments != value)
                {
                    _Comments = value;
                    OnPropertyChanged("Comments");
                }
            }
        }

        private long _SerialOccyteNo;
        public long SerialOccyteNo
        {
            get { return _SerialOccyteNo; }
            set
            {
                if (_SerialOccyteNo != value)
                {
                    _SerialOccyteNo = value;
                    OnPropertyChanged("SerialOccyteNo");
                }
            }
        }
        private List<MasterListItem> _CanIdList = new List<MasterListItem>();
        public List<MasterListItem> CanIdList
        {
            get
            {
                return _CanIdList;
            }
            set
            {
                _CanIdList = value;
            }
        }

        private MasterListItem _SelectedCanId = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedCanId
        {
            get
            {
                return _SelectedCanId;
            }
            set
            {
                _SelectedCanId = value;
            }
        }

        private List<MasterListItem> _CellStageList = new List<MasterListItem>();
        public List<MasterListItem> CellStageList
        {
            get
            {
                return _CellStageList;
            }
            set
            {
                _CellStageList = value;
            }
        }

        private MasterListItem _SelectedCellStage = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedCellStage
        {
            get
            {
                return _SelectedCellStage;
            }
            set
            {
                _SelectedCellStage = value;
            }
        }

        private List<MasterListItem> _GradeList = new List<MasterListItem>();
        public List<MasterListItem> GradeList
        {
            get
            {
                return _GradeList;
            }
            set
            {
                _GradeList = value;
            }
        }

        private MasterListItem _SelectedGrade = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedGrade
        {
            get
            {
                return _SelectedGrade;
            }
            set
            {
                _SelectedGrade = value;
            }
        }

        private List<MasterListItem> _OocyteGradeList = new List<MasterListItem>();
        public List<MasterListItem> OocyteGradeList
        {
            get
            {
                return _OocyteGradeList;
            }
            set
            {
                _OocyteGradeList = value;
            }
        }

        private MasterListItem _SelectedOocyteGrade = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedOocyteGrade
        {
            get
            {
                return _SelectedOocyteGrade;
            }
            set
            {
                _SelectedOocyteGrade = value;
            }
        }

        //Added by Saily P

        private List<MasterListItem> _StrawIdList = new List<MasterListItem>();
        public List<MasterListItem> StrawIdList
        {
            get
            {
                return _StrawIdList;
            }
            set
            {
                _StrawIdList = value;
            }
        }

        private MasterListItem _SelectedStrawId = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedStrawId
        {
            get
            {
                return _SelectedStrawId;
            }
            set
            {
                _SelectedStrawId = value;
            }
        }

        private List<MasterListItem> _GobletShapeList = new List<MasterListItem>();
        public List<MasterListItem> GobletShapeList
        {
            get
            {
                return _GobletShapeList;
            }
            set
            {
                _GobletShapeList = value;
            }
        }

        private MasterListItem _SelectedGobletShape = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedGobletShape
        {
            get
            {
                return _SelectedGobletShape;
            }
            set
            {
                _SelectedGobletShape = value;
            }
        }

        private List<MasterListItem> _GobletSizeList = new List<MasterListItem>();
        public List<MasterListItem> GobletSizeList
        {
            get
            {
                return _GobletSizeList;
            }
            set
            {
                _GobletSizeList = value;
            }
        }

        private MasterListItem _SelectedGobletSize = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedGobletSize
        {
            get
            {
                return _SelectedGobletSize;
            }
            set
            {
                _SelectedGobletSize = value;
            }
        }

        private List<MasterListItem> _CanisterIdList = new List<MasterListItem>();
        public List<MasterListItem> CanisterIdList
        {
            get
            {
                return _CanisterIdList;
            }
            set
            {
                _CanisterIdList = value;
            }
        }

        private MasterListItem _SelectedCanisterId = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedCanisterId
        {
            get
            {
                return _SelectedCanisterId;
            }
            set
            {
                _SelectedCanisterId = value;
            }
        }

        private List<MasterListItem> _TankList = new List<MasterListItem>();
        public List<MasterListItem> TankList
        {
            get
            {
                return _TankList;
            }
            set
            {
                _TankList = value;
            }
        }

        private MasterListItem _SelectedTank = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedTank
        {
            get
            {
                return _SelectedTank;
            }
            set
            {
                _SelectedTank = value;
            }
        }
        private List<MasterListItem> _ColorSelectorList = new List<MasterListItem>();
        public List<MasterListItem> ColorSelectorList
        {
            get
            {
                return _ColorSelectorList;
            }
            set
            {
                _ColorSelectorList = value;
            }
        }

        private MasterListItem _SelectedColorSelector = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedColorSelector
        {
            get
            {
                return _SelectedColorSelector;
            }
            set
            {
                _SelectedColorSelector = value;
            }
        }

        //Added By CDS 
        private List<MasterListItem> _PostThawingPlanList = new List<MasterListItem>();
        public List<MasterListItem> PostThawingPlanList
        {
            get
            {
                return _PostThawingPlanList;
            }
            set
            {
                _PostThawingPlanList = value;
            }
        }

        private MasterListItem _SelectedPostThawingPlan = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedPostThawingPlan
        {
            get
            {
                return _SelectedPostThawingPlan;
            }
            set
            {
                _SelectedPostThawingPlan = value;
            }
        }

        private string _PostThawingPlan;
        public string PostThawingPlan
        {
            get { return _PostThawingPlan; }
            set
            {
                if (_PostThawingPlan != value)
                {
                    _PostThawingPlan = value;
                    OnPropertyChanged("PostThawingPlan");
                }
            }
        }

        private long _PostThawingPlanID;
        public long PostThawingPlanID
        {
            get { return _PostThawingPlanID; }
            set
            {
                if (_PostThawingPlanID != value)
                {
                    _PostThawingPlanID = value;
                    OnPropertyChanged("PostThawingPlanID");
                }
            }
        }

        //END 
        private long _PlanId;
        public long PlanId
        {
            get { return _PlanId; }
            set
            {
                if (_PlanId != value)
                {
                    _PlanId = value;
                    OnPropertyChanged("PlanId");
                }
            }
        }

        private bool _IsDiscard;
        public bool IsDiscard
        {
            get { return _IsDiscard; }
            set
            {
                if (_IsDiscard != value)
                {
                    _IsDiscard = value;
                    OnPropertyChanged("IsDiscard");
                }
            }
        }

        private List<MasterListItem> _PlanList = new List<MasterListItem>();
        public List<MasterListItem> PlanList
        {
            get
            {
                return _PlanList;
            }
            set
            {
                _PlanList = value;
            }
        }

        private MasterListItem _SelectedPlanId = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedPlanId
        {
            get
            {
                return _SelectedPlanId;
            }
            set
            {
                _SelectedPlanId = value;
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
        private string _Grade;
        public string Grade
        {
            get { return _Grade; }
            set
            {
                if (_Grade != value)
                {
                    _Grade = value;
                    OnPropertyChanged("Grade");
                }
            }
        }
        private long _OocyteDonorID;
        public long OocyteDonorID
        {
            get { return _OocyteDonorID; }
            set
            {
                if (_OocyteDonorID != value)
                {
                    _OocyteDonorID = value;
                    OnPropertyChanged("OocyteDonorID");
                }
            }
        }

        private long _TransferDayNo;
        public long TransferDayNo
        {
            get { return _TransferDayNo; }
            set
            {
                if (_TransferDayNo != value)
                {
                    _TransferDayNo = value;
                    OnPropertyChanged("TransferDayNo");
                }
            }
        }

        private long _CleavageGrade;
        public long CleavageGrade
        {
            get { return _CleavageGrade; }
            set
            {
                if (_CleavageGrade != value)
                {
                    _CleavageGrade = value;
                    OnPropertyChanged("CleavageGrade");
                }
            }
        }

        private long _OocyteDonorUnitID;
        public long OocyteDonorUnitID
        {
            get { return _OocyteDonorUnitID; }
            set
            {
                if (_OocyteDonorUnitID != value)
                {
                    _OocyteDonorUnitID = value;
                    OnPropertyChanged("OocyteDonorUnitID");
                }
            }
        }

        private bool _SelectOocyte;
        public bool SelectOocyte
        {
            get { return _SelectOocyte; }
            set
            {
                if (value != _SelectOocyte)
                {
                    _SelectOocyte = value;
                    OnPropertyChanged("SelectOocyte");
                }
            }
        }
        #endregion

    }

}
