using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsPGDHistoryVO : IValueObject, INotifyPropertyChanged
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
        private string _ChromosomalDisease;
        public string ChromosomalDisease
        {
            get { return _ChromosomalDisease; }
            set
            {
                if (_ChromosomalDisease != value)
                {
                    _ChromosomalDisease = value;
                    OnPropertyChanged("ChromosomalDisease");
                }
            }
        }
        private string _XLinkedRecessive;
        public string XLinkedRecessive
        {
            get { return _XLinkedRecessive; }
            set
            {
                if (_XLinkedRecessive != value)
                {
                    _XLinkedRecessive = value;
                    OnPropertyChanged("XLinkedRecessive");
                }
            }
        }
        private string _XLinkedDominant;
        public string XLinkedDominant
        {
            get { return _XLinkedDominant; }
            set
            {
                if (_XLinkedDominant != value)
                {
                    _XLinkedDominant = value;
                    OnPropertyChanged("XLinkedDominant");
                }
            }
        }
        private string _AutosomalDominant;
        public string AutosomalDominant
        {
            get { return _AutosomalDominant; }
            set
            {
                if (_AutosomalDominant != value)
                {
                    _AutosomalDominant = value;
                    OnPropertyChanged("AutosomalDominant");
                }
            }
        }
        private string _AutosomalRecessive;
        public string AutosomalRecessive
        {
            get { return _AutosomalRecessive; }
            set
            {
                if (_AutosomalRecessive != value)
                {
                    _AutosomalRecessive = value;
                    OnPropertyChanged("AutosomalRecessive");
                }
            }
        }
        private string _Ylinked;
        public string Ylinked
        {
            get { return _Ylinked; }
            set
            {
                if (_Ylinked != value)
                {
                    _Ylinked = value;
                    OnPropertyChanged("Ylinked");
                }
            }
        }
        private long _FamilyHistory;
        public long FamilyHistory
        {
            get { return _FamilyHistory; }
            set
            {
                if (_FamilyHistory != value)
                {
                    _FamilyHistory = value;
                    OnPropertyChanged("FamilyHistory");
                }
            }
        }

        private long _AffectedPartner;
        public long AffectedPartner
        {
            get { return _AffectedPartner; }
            set
            {
                if (_AffectedPartner != value)
                {
                    _AffectedPartner = value;
                    OnPropertyChanged("AffectedPartner");
                }
            }
        }

        #endregion


    }

    public class clsPGDGeneralDetailsVO : IValueObject, INotifyPropertyChanged 
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

        private long _LabDayUnitID;
        public long LabDayUnitID
        {
            get { return _LabDayUnitID; }
            set
            {
                if (_LabDayUnitID != value)
                {
                    _LabDayUnitID = value;
                    OnPropertyChanged("LabDayUnitID");
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

        private bool _FrozenPGDPGS;
        public bool FrozenPGDPGS
        {
            get { return _FrozenPGDPGS; }
            set
            {
                if (_FrozenPGDPGS != value)
                {
                    _FrozenPGDPGS = value;
                    OnPropertyChanged("FrozenPGDPGS");
                }
            }
        }

        private long _LabDayNo;
        public long LabDayNo
        {
            get { return _LabDayNo; }
            set
            {
                if (_LabDayNo != value)
                {
                    _LabDayNo = value;
                    OnPropertyChanged("LabDayNo");
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

        private long _SerialEmbNumber;
        public long SerialEmbNumber
        {
            get { return _SerialEmbNumber; }
            set
            {
                if (_SerialEmbNumber != value)
                {
                    _SerialEmbNumber = value;
                    OnPropertyChanged("SerialEmbNumber");
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
        private long _Physician;
        public long Physician
        {
            get { return _Physician; }
            set
            {
                if (_Physician != value)
                {
                    _Physician = value;
                    OnPropertyChanged("Physician");
                }
            }
        }        

        private long _BiospyID;
        public long BiospyID
        {
            get { return _BiospyID; }
            set
            {
                if (_BiospyID != value)
                {
                    _BiospyID = value;
                    OnPropertyChanged("BiospyID");
                }
            }
        }
        private string _ReferringFacility;
        public string ReferringFacility
        {
            get { return _ReferringFacility; }
            set
            {
                if (_ReferringFacility != value)
                {
                    _ReferringFacility = value;
                    OnPropertyChanged("ReferringFacility");
                }
            }
        }
        private string _ResonOfReferal;
        public string ResonOfReferal
        {
            get { return _ResonOfReferal; }
            set
            {
                if (_ResonOfReferal != value)
                {
                    _ResonOfReferal = value;
                    OnPropertyChanged("ResonOfReferal");
                }
            }
        }
        private long _SpecimanUsedID;
        public long SpecimanUsedID
        {
            get { return _SpecimanUsedID; }
            set
            {
                if (_SpecimanUsedID != value)
                {
                    _SpecimanUsedID = value;
                    OnPropertyChanged("SpecimanUsedID");
                }
            }
        }
        private string _MainFISHRemark;
        public string MainFISHRemark
        {
            get { return _MainFISHRemark; }
            set
            {
                if (_MainFISHRemark != value)
                {
                    _MainFISHRemark = value;
                    OnPropertyChanged("MainFISHRemark");
                }
            }
        }
        private string _MainKaryotypingRemark;
        public string MainKaryotypingRemark
        {
            get { return _MainKaryotypingRemark; }
            set
            {
                if (_MainKaryotypingRemark != value)
                {
                    _MainKaryotypingRemark = value;
                    OnPropertyChanged("MainKaryotypingRemark");
                }
            }
        }
        private long _TechniqueID;
        public long TechniqueID
        {
            get { return _TechniqueID; }
            set
            {
                if (_TechniqueID != value)
                {
                    _TechniqueID = value;
                    OnPropertyChanged("TechniqueID");
                }
            }
        }
        private string _SourceURL;
        public string SourceURL
        {
            get { return _SourceURL; }
            set
            {
                if (_SourceURL != value)
                {
                    _SourceURL = value;
                    OnPropertyChanged("SourceURL");
                }
            }
        }
        private string _FileName;
        public string FileName
        {
            get { return _FileName; }
            set
            {
                if (_FileName != value)
                {
                    _FileName = value;
                    OnPropertyChanged("FileName");
                }
            }
        }

        private byte[] _Report;
        public byte[] Report
        {
            get { return _Report; }
            set
            {
                if (_Report != value)
                {
                    _Report = value;
                    OnPropertyChanged("Report");

                }

            }

        }

        private List<clsAddImageVO> _ImgList = new List<clsAddImageVO>();
        public List<clsAddImageVO> ImgList
        {
            get
            {
                return _ImgList;
            }
            set
            {
                _ImgList = value;
            }
        }

        #region For ART Flow - PGD

        private long _TestOrderedID;
        public long TestOrderedID
        {
            get { return _TestOrderedID; }
            set
            {
                if (_TestOrderedID != value)
                {
                    _TestOrderedID = value;
                    OnPropertyChanged("TestOrderedID");
                }
            }
        }


        private long _ReferringID;
        public long ReferringID
        {
            get { return _ReferringID; }
            set
            {
                if (_ReferringID != value)
                {
                    _ReferringID = value;
                    OnPropertyChanged("ReferringID");
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

        private DateTime? _SampleReceiveDate;
        public DateTime? SampleReceiveDate
        {
            get { return _SampleReceiveDate; }
            set
            {
                if (_SampleReceiveDate != value)
                {
                    _SampleReceiveDate = value;
                    OnPropertyChanged("SampleReceiveDate");
                }
            }
        }


        private DateTime? _ResultDate;
        public DateTime? ResultDate
        {
            get { return _ResultDate; }
            set
            {
                if (_ResultDate != value)
                {
                    _ResultDate = value;
                    OnPropertyChanged("ResultDate");
                }
            }
        }

        private string _MainFISHInterpretation;
        public string MainFISHInterpretation
        {
            get { return _MainFISHInterpretation; }
            set
            {
                if (_MainFISHInterpretation != value)
                {
                    _MainFISHInterpretation = value;
                    OnPropertyChanged("MainFISHInterpretation");
                }
            }
        }


        private long _SupervisedById;
        public long SupervisedById
        {
            get { return _SupervisedById; }
            set
            {
                if (_SupervisedById != value)
                {
                    _SupervisedById = value;
                    OnPropertyChanged("SupervisedById");
                }
            }
        }

        private long _PGDIndicationID;
        public long PGDIndicationID
        {
            get { return _PGDIndicationID; }
            set
            {
                if (_PGDIndicationID != value)
                {
                    _PGDIndicationID = value;
                    OnPropertyChanged("PGDIndicationID");
                }
            }
        }

        private string _PGDIndicationDetails;
        public string PGDIndicationDetails
        {
            get { return _PGDIndicationDetails; }
            set
            {
                if (_PGDIndicationDetails != value)
                {
                    _PGDIndicationDetails = value;
                    OnPropertyChanged("PGDIndicationDetails");
                }
            }
        }

        private long _ReferringFacilityID;
        public long ReferringFacilityID
        {
            get { return _ReferringFacilityID; }
            set
            {
                if (_ReferringFacilityID != value)
                {
                    _ReferringFacilityID = value;
                    OnPropertyChanged("ReferringFacilityID");
                }
            }
        }

        private string _PGDResult;
        public string PGDResult
        {
            get { return _PGDResult; }
            set
            {
                if (_PGDResult != value)
                {
                    _PGDResult = value;
                    OnPropertyChanged("PGDResult");
                }
            }
        }


        private long _PGDPGSProcedureID;
        public long PGDPGSProcedureID
        {
            get { return _PGDPGSProcedureID; }
            set
            {
                if (_PGDPGSProcedureID != value)
                {
                    _PGDPGSProcedureID = value;
                    OnPropertyChanged("PGDPGSProcedureID");
                }
            }
        }

        #endregion
        #endregion
    }

    public class clsPGDFISHVO : IValueObject, INotifyPropertyChanged 
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

        private long _LabDayUnitID;
        public long LabDayUnitID
        {
            get { return _LabDayUnitID; }
            set
            {
                if (_LabDayUnitID != value)
                {
                    _LabDayUnitID = value;
                    OnPropertyChanged("LabDayUnitID");
                }
            }
        }
        private long _LabDayNo;
        public long LabDayNo
        {
            get { return _LabDayNo; }
            set
            {
                if (_LabDayNo != value)
                {
                    _LabDayNo = value;
                    OnPropertyChanged("LabDayNo");
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

        private long _SerialEmbNumber;
        public long SerialEmbNumber
        {
            get { return _SerialEmbNumber; }
            set
            {
                if (_SerialEmbNumber != value)
                {
                    _SerialEmbNumber = value;
                    OnPropertyChanged("SerialEmbNumber");
                }
            }
        }
        private long _ChromosomeStudiedID;
        public long ChromosomeStudiedID
        {
            get { return _ChromosomeStudiedID; }
            set
            {
                if (_ChromosomeStudiedID != value)
                {
                    _ChromosomeStudiedID = value;
                    OnPropertyChanged("ChromosomeStudiedID");
                }
            }
        }

        private long _TestOrderedID;
        public long TestOrderedID
        {
            get { return _TestOrderedID; }
            set
            {
                if (_TestOrderedID != value)
                {
                    _TestOrderedID = value;
                    OnPropertyChanged("TestOrderedID");
                }
            }
        }
        private string _NoOfCellCounted;
        public string NoOfCellCounted
        {
            get { return _NoOfCellCounted; }
            set
            {
                if (_NoOfCellCounted != value)
                {
                    _NoOfCellCounted = value;
                    OnPropertyChanged("NoOfCellCounted");
                }
            }
        }
        private string _Result;
        public string Result
        {
            get { return _Result; }
            set
            {
                if (_Result != value)
                {
                    _Result = value;
                    OnPropertyChanged("Result");
                }
            }
        }

        private List<MasterListItem> _ChromosomeStudiedIdList = new List<MasterListItem>();
       public List<MasterListItem> ChromosomeStudiedIdList
       {
           get
           {
               return _ChromosomeStudiedIdList;
           }
           set
           {
               _ChromosomeStudiedIdList = value;
           }
       }

       private MasterListItem _SelectedChromosomeStudiedId = new MasterListItem { ID = 0, Description = "--Select--" };
       public MasterListItem SelectedChromosomeStudiedId
       {
           get
           {
               return _SelectedChromosomeStudiedId;
           }
           set
           {
               _SelectedChromosomeStudiedId = value;
           }
       }

       private List<MasterListItem> _TestOrderedIdList = new List<MasterListItem>();
       public List<MasterListItem> TestOrderedIdList
       {
           get
           {
               return _TestOrderedIdList;
           }
           set
           {
               _TestOrderedIdList = value;
           }
       }

       private MasterListItem _SelectedTestOrderedId = new MasterListItem { ID = 0, Description = "--Select--" };
       public MasterListItem SelectedTestOrderedId
       {
           get
           {
               return _SelectedTestOrderedId;
           }
           set
           {
               _SelectedTestOrderedId = value;
           }
       }
        #endregion
    }
    public class clsPGDKaryotypingVO : IValueObject, INotifyPropertyChanged
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

        private long _LabDayUnitID;
        public long LabDayUnitID
        {
            get { return _LabDayUnitID; }
            set
            {
                if (_LabDayUnitID != value)
                {
                    _LabDayUnitID = value;
                    OnPropertyChanged("LabDayUnitID");
                }
            }
        }
        private long _LabDayNo;
        public long LabDayNo
        {
            get { return _LabDayNo; }
            set
            {
                if (_LabDayNo != value)
                {
                    _LabDayNo = value;
                    OnPropertyChanged("LabDayNo");
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

        private long _SerialEmbNumber;
        public long SerialEmbNumber
        {
            get { return _SerialEmbNumber; }
            set
            {
                if (_SerialEmbNumber != value)
                {
                    _SerialEmbNumber = value;
                    OnPropertyChanged("SerialEmbNumber");
                }
            }
        }
        private long _ChromosomeStudiedID;
        public long ChromosomeStudiedID
        {
            get { return _ChromosomeStudiedID; }
            set
            {
                if (_ChromosomeStudiedID != value)
                {
                    _ChromosomeStudiedID = value;
                    OnPropertyChanged("ChromosomeStudiedID");
                }
            }
        }
        private long _CultureTypeID;
        public long CultureTypeID
        {
            get { return _CultureTypeID; }
            set
            {
                if (_CultureTypeID != value)
                {
                    _CultureTypeID = value;
                    OnPropertyChanged("CultureTypeID");
                }
            }
        }
        private long _BindingTechnique;
        public long BindingTechnique
        {
            get { return _BindingTechnique; }
            set
            {
                if (_BindingTechnique != value)
                {
                    _BindingTechnique = value;
                    OnPropertyChanged("BindingTechnique");
                }
            }
        }
        private string _MetaphaseCounted;
        public string MetaphaseCounted
        {
            get { return _MetaphaseCounted; }
            set
            {
                if (_MetaphaseCounted != value)
                {
                    _MetaphaseCounted = value;
                    OnPropertyChanged("MetaphaseCounted");
                }
            }
        }
        private string _MetaphaseAnalysed;
        public string MetaphaseAnalysed
        {
            get { return _MetaphaseAnalysed; }
            set
            {
                if (_MetaphaseAnalysed != value)
                {
                    _MetaphaseAnalysed = value;
                    OnPropertyChanged("MetaphaseAnalysed");
                }
            }
        }
        private string _MetaphaseKaryotype;
        public string MetaphaseKaryotype
        {
            get { return _MetaphaseKaryotype; }
            set
            {
                if (_MetaphaseKaryotype != value)
                {
                    _MetaphaseKaryotype = value;
                    OnPropertyChanged("MetaphaseKaryotype");
                }
            }
        }
        private string _Result;
        public string Result
        {
            get { return _Result; }
            set
            {
                if (_Result != value)
                {
                    _Result = value;
                    OnPropertyChanged("Result");
                }
            }
        }
        private List<MasterListItem> _ChromosomeStudiedIdList = new List<MasterListItem>();
        public List<MasterListItem> ChromosomeStudiedIdList
        {
            get
            {
                return _ChromosomeStudiedIdList;
            }
            set
            {
                _ChromosomeStudiedIdList = value;
            }
        }

        private MasterListItem _SelectedChromosomeStudiedId = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedChromosomeStudiedId
        {
            get
            {
                return _SelectedChromosomeStudiedId;
            }
            set
            {
                _SelectedChromosomeStudiedId = value;
            }
        }
        #endregion
    }
}
