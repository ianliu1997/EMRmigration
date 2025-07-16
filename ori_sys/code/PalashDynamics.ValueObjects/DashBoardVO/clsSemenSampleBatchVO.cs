using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsSemenSampleBatchVO : IValueObject, INotifyPropertyChanged
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
        private long _LabID;
        public long LabID
        {
            get { return _LabID; }
            set
            {
                if (_LabID != value)
                {
                    _LabID = value;
                    OnPropertyChanged("LabID");
                }
            }
        }
        private long _ReceivedByID;
        public long ReceivedByID
        {
            get { return _ReceivedByID; }
            set
            {
                if (_ReceivedByID != value)
                {
                    _ReceivedByID = value;
                    OnPropertyChanged("ReceivedByID");
                }
            }
        }
        private DateTime _ReceivedDate;
        public DateTime ReceivedDate
        {
            get { return _ReceivedDate; }
            set
            {
                if (_ReceivedDate != value)
                {
                    _ReceivedDate = value;
                    OnPropertyChanged("ReceivedDate");
                }
            }
        }
        private string _InvoiceNo;
        public string InvoiceNo
        {
            get { return _InvoiceNo; }
            set
            {
                if (_InvoiceNo != value)
                {
                    _InvoiceNo = value;
                    OnPropertyChanged("InvoiceNo");
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
        private int _NoOfVails;
        public int NoOfVails
        {
            get { return _NoOfVails; }
            set
            {
                if (_NoOfVails != value)
                {
                    _NoOfVails = value;
                    OnPropertyChanged("NoOfVails");
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
        private float _Volume;
        public float Volume
        {
            get { return _Volume; }
            set
            {
                if (_Volume != value)
                {
                    _Volume = value;
                    OnPropertyChanged("Volume");
                }
            }
        }
        private float _AvailableVolume;
        public float AvailableVolume
        {
            get { return _AvailableVolume; }
            set
            {
                if (_AvailableVolume != value)
                {
                    _AvailableVolume = value;
                    OnPropertyChanged("AvailableVolume");
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
    public class clsBatchAndSpemFreezingVO : IValueObject, INotifyPropertyChanged
    {

        #region Property Declaration Section
        private bool _IsThaw;
        public bool IsThaw
        {
            get { return _IsThaw; }
            set
            {
                if (_IsThaw != value)
                {
                    _IsThaw = value;
                    OnPropertyChanged("IsThaw");
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
        private long _TankID;
        public long TankID
        {
            get { return _TankID; }
            set
            {
                if (_TankID != value)
                {
                    _TankID = value;
                    OnPropertyChanged("TankID");
                }
            }
        }
        private string _Canister;
        public string Canister
        {
            get { return _Canister; }
            set
            {
                if (_Canister != value)
                {
                    _Canister = value;
                    OnPropertyChanged("Canister");
                }
            }
        }
        private long _CanisterID;
        public long CanisterID
        {
            get { return _CanisterID; }
            set
            {
                if (_CanisterID != value)
                {
                    _CanisterID = value;
                    OnPropertyChanged("CanisterID");
                }
            }
        }
        private string _Cane;
        public string Cane
        {
            get { return _Cane; }
            set
            {
                if (_Cane != value)
                {
                    _Cane = value;
                    OnPropertyChanged("Cane");
                }
            }
        }
        private long _CaneID;
        public long CaneID
        {
            get { return _CaneID; }
            set
            {
                if (_CaneID != value)
                {
                    _CaneID = value;
                    OnPropertyChanged("CaneID");
                }
            }
        }
        private string _GlobletSize;
        public string GlobletSize
        {
            get { return _GlobletSize; }
            set
            {
                if (_GlobletSize != value)
                {
                    _GlobletSize = value;
                    OnPropertyChanged("GlobletSize");
                }
            }
        }
        private long _GlobletSizeID;
        public long GlobletSizeID
        {
            get { return _GlobletSizeID; }
            set
            {
                if (_GlobletSizeID != value)
                {
                    _GlobletSizeID = value;
                    OnPropertyChanged("GlobletSizeID");
                }
            }
        }
        private string _GlobletShape;
        public string GlobletShape
        {
            get { return _GlobletShape; }
            set
            {
                if (_GlobletShape != value)
                {
                    _GlobletShape = value;
                    OnPropertyChanged("GlobletShape");
                }
            }
        }
        private long _GlobletShapeID;
        public long GlobletShapeID
        {
            get { return _GlobletShapeID; }
            set
            {
                if (_GlobletShapeID != value)
                {
                    _GlobletShapeID = value;
                    OnPropertyChanged("GlobletShapeID");
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
        private long _StrawID;
        public long StrawID
        {
            get { return _StrawID; }
            set
            {
                if (_StrawID != value)
                {
                    _StrawID = value;
                    OnPropertyChanged("StrawID");
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
        private long _SpremNo;
        public long SpremNo
        {
            get { return _SpremNo; }
            set
            {
                if (_SpremNo != value)
                {
                    _SpremNo = value;
                    OnPropertyChanged("SpremNo");
                }
            }
        }
        private long _SpermFreezingDetailsID;
        public long SpermFreezingDetailsID
        {
            get { return _SpermFreezingDetailsID; }
            set
            {
                if (_SpermFreezingDetailsID != value)
                {
                    _SpermFreezingDetailsID = value;
                    OnPropertyChanged("SpermFreezingDetailsID");
                }
            }
        }
        private long _SpermFreezingDetailsUnitID;
        public long SpermFreezingDetailsUnitID
        {
            get { return _SpermFreezingDetailsUnitID; }
            set
            {
                if (_SpermFreezingDetailsUnitID != value)
                {
                    _SpermFreezingDetailsUnitID = value;
                    OnPropertyChanged("SpermFreezingDetailsUnitID");
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
        private string _Viscosity;
        public string Viscosity
        {
            get { return _Viscosity; }
            set
            {
                if (_Viscosity != value)
                {
                    _Viscosity = value;
                    OnPropertyChanged("Viscosity");
                }
            }
        }

        private long _ViscosityID;
        public long ViscosityID
        {
            get { return _ViscosityID; }
            set
            {
                if (_ViscosityID != value)
                {
                    _ViscosityID = value;
                    OnPropertyChanged("ViscosityID");
                }
            }
        }

        private long _CollectionMethodID;
        public long CollectionMethodID
        {
            get { return _CollectionMethodID; }
            set
            {
                if (_CollectionMethodID != value)
                {
                    _CollectionMethodID = value;
                    OnPropertyChanged("CollectionMethodID");
                }
            }
        }

        private string _CollectionProblem;
        public string CollectionProblem
        {
            get { return _CollectionProblem; }
            set
            {
                if (_CollectionProblem != value)
                {
                    _CollectionProblem = value;
                    OnPropertyChanged("CollectionProblem");
                }
            }
        }
        private string _FreezingOther;
        public string FreezingOther
        {
            get { return _FreezingOther; }
            set
            {
                if (_FreezingOther != value)
                {
                    _FreezingOther = value;
                    OnPropertyChanged("FreezingOther");
                }
            }
        }

        private string _FreezingComments;
        public string FreezingComments
        {
            get { return _FreezingComments; }
            set
            {
                if (_FreezingComments != value)
                {
                    _FreezingComments = value;
                    OnPropertyChanged("FreezingComments");
                }
            }
        }
        private float _Volume;
        public float Volume
        {
            get { return _Volume; }
            set
            {
                if (_Volume != value)
                {
                    _Volume = value;
                    OnPropertyChanged("Volume");
                }
            }
        }

        private string _Abstience;
        public string Abstience
        {
            get { return _Abstience; }
            set
            {
                if (_Abstience != value)
                {
                    _Abstience = value;
                    OnPropertyChanged("Abstience");
                }
            }
        }

        private long _TotalSpremCount;
        public long TotalSpremCount
        {
            get { return _TotalSpremCount; }
            set
            {
                if (_TotalSpremCount != value)
                {
                    _TotalSpremCount = value;
                    OnPropertyChanged("TotalSpremCount");
                }
            }
        }

        private decimal _Motility;
        public decimal Motility
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

        private decimal _GradeA;
        public decimal GradeA
        {
            get { return _GradeA; }
            set
            {
                if (_GradeA != value)
                {
                    _GradeA = value;
                    OnPropertyChanged("GradeA");
                }
            }
        }

        private decimal _GradeB;
        public decimal GradeB
        {
            get { return _GradeB; }
            set
            {
                if (_GradeB != value)
                {
                    _GradeB = value;
                    OnPropertyChanged("GradeB");
                }
            }
        }

        private decimal _GradeC;
        public decimal GradeC
        {
            get { return _GradeC; }
            set
            {
                if (_GradeC != value)
                {
                    _GradeC = value;
                    OnPropertyChanged("GradeC");
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
        private DateTime _SpremFreezingTime;
        public DateTime SpremFreezingTime
        {
            get { return _SpremFreezingTime; }
            set
            {
                if (_SpremFreezingTime != value)
                {
                    _SpremFreezingTime = value;
                    OnPropertyChanged("SpremFreezingTime");
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
        private long _LabID;
        public long LabID
        {
            get { return _LabID; }
            set
            {
                if (_LabID != value)
                {
                    _LabID = value;
                    OnPropertyChanged("LabID");
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
        private long _ReceivedByID;
        public long ReceivedByID
        {
            get { return _ReceivedByID; }
            set
            {
                if (_ReceivedByID != value)
                {
                    _ReceivedByID = value;
                    OnPropertyChanged("ReceivedByID");
                }
            }
        }
        private string _ReceivedByName;
        public string ReceivedByName
        {
            get { return _ReceivedByName; }
            set
            {
                if (_ReceivedByName != value)
                {
                    _ReceivedByName = value;
                    OnPropertyChanged("ReceivedByName");
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

        private DateTime _SpremFreezingDate;
        public DateTime SpremFreezingDate
        {
            get { return _SpremFreezingDate; }
            set
            {
                if (_SpremFreezingDate != value)
                {
                    _SpremFreezingDate = value;
                    OnPropertyChanged("SpremFreezingDate");
                }
            }
        }
        private DateTime _ReceivedDate;
        public DateTime ReceivedDate
        {
            get { return _ReceivedDate; }
            set
            {
                if (_ReceivedDate != value)
                {
                    _ReceivedDate = value;
                    OnPropertyChanged("ReceivedDate");
                }
            }
        }


        private string _InvoiceNo;
        public string InvoiceNo
        {
            get { return _InvoiceNo; }
            set
            {
                if (_InvoiceNo != value)
                {
                    _InvoiceNo = value;
                    OnPropertyChanged("InvoiceNo");
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
        private int _NoOfVails;
        public int NoOfVails
        {
            get { return _NoOfVails; }
            set
            {
                if (_NoOfVails != value)
                {
                    _NoOfVails = value;
                    OnPropertyChanged("NoOfVails");
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

        private float _AvailableVolume;
        public float AvailableVolume
        {
            get { return _AvailableVolume; }
            set
            {
                if (_AvailableVolume != value)
                {
                    _AvailableVolume = value;
                    OnPropertyChanged("AvailableVolume");
                }
            }
        }
        private long _SpermFreezingID;
        public long SpermFreezingID
        {
            get { return _SpermFreezingID; }
            set
            {
                if (_SpermFreezingID != value)
                {
                    _SpermFreezingID = value;
                    OnPropertyChanged("SpermFreezingID");
                }
            }
        }
        private long _SpermFreezingUnitID;
        public long SpermFreezingUnitID
        {
            get { return _SpermFreezingUnitID; }
            set
            {
                if (_SpermFreezingUnitID != value)
                {
                    _SpermFreezingUnitID = value;
                    OnPropertyChanged("SpermFreezingUnitID");
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
    public class clsReceiveOocyteVO : IValueObject, INotifyPropertyChanged
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
        private DateTime _DonorOPUDate;
        public DateTime DonorOPUDate
        {
            get { return _DonorOPUDate; }
            set
            {
                if (_DonorOPUDate != value)
                {
                    _DonorOPUDate = value;
                    OnPropertyChanged("DonorOPUDate");
                }
            }
        }
        private long _DonorOPUID;
        public long DonorOPUID
        {
            get { return _DonorOPUID; }
            set
            {
                if (_DonorOPUID != value)
                {
                    _DonorOPUID = value;
                    OnPropertyChanged("DonorOPUID");
                }
            }
        }

        private long _DonorOPUUnitID;
        public long DonorOPUUnitID
        {
            get { return _DonorOPUUnitID; }
            set
            {
                if (_DonorOPUUnitID != value)
                {
                    _DonorOPUUnitID = value;
                    OnPropertyChanged("DonorOPUUnitID");
                }
            }
        }
        private long _DonorOocyteRetrived;
        public long DonorOocyteRetrived
        {
            get { return _DonorOocyteRetrived; }
            set
            {
                if (_DonorOocyteRetrived != value)
                {
                    _DonorOocyteRetrived = value;
                    OnPropertyChanged("DonorOocyteRetrived");
                }
            }
        }
        private long _DonorBalancedOocyte;
        public long DonorBalancedOocyte
        {
            get { return _DonorBalancedOocyte; }
            set
            {
                if (_DonorBalancedOocyte != value)
                {
                    _DonorBalancedOocyte = value;
                    OnPropertyChanged("DonorBalancedOocyte");
                }
            }
        }
        private long _OocyteConsumed;
        public long OocyteConsumed
        {
            get { return _OocyteConsumed; }
            set
            {
                if (_OocyteConsumed != value)
                {
                    _OocyteConsumed = value;
                    OnPropertyChanged("OocyteConsumed");
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

        private bool _IsDonorCycle;
        public bool IsDonorCycle
        {
            get { return _IsDonorCycle; }
            set
            {
                if (_IsDonorCycle != value)
                {
                    _IsDonorCycle = value;
                    OnPropertyChanged("IsDonorCycle");
                }
            }
        }

        private bool _IsReceiveOocyte;
        public bool IsReceiveOocyte
        {
            get
            {
                return _IsReceiveOocyte;
            }
            set
            {
                _IsReceiveOocyte = value;
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
