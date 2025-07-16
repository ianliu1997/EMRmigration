using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PalashDynamics.ValueObjects.EMR;
using System.IO;
using System.Windows.Media.Imaging;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsFemaleLabDay0VO : IValueObject, INotifyPropertyChanged
    {
        #region Property Declaration Section

        public List<IVFTreatment> IVFSetting { get; set; }
        public List<ICSITreatment> ICSISetting { get; set; }
        public List<FileUpload> FUSetting { get; set; }
        public clsLabDaysSummaryVO LabDaySummary { get; set; }
        public clsFemaleSemenDetailsVO SemenDetails { get; set; }
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

        private DateTime? _ProcDate;
        public DateTime? ProcDate
        {
            get { return _ProcDate; }
            set
            {
                if (_ProcDate != value)
                {
                    _ProcDate = value;
                    OnPropertyChanged("ProcDate");
                }
            }
        }

        private DateTime? _ProcTime;
        public DateTime? ProcTime
        {
            get { return _ProcTime; }
            set
            {
                if (_ProcTime != value)
                {
                    _ProcTime = value;
                    OnPropertyChanged("ProcTime");
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

        private long _AssEmbryologistID;
        public long AssEmbryologistID
        {
            get { return _AssEmbryologistID; }
            set
            {
                if (_AssEmbryologistID != value)
                {
                    _AssEmbryologistID = value;
                    OnPropertyChanged("AssEmbryologistID");
                }
            }
        }
                       
        private long _AnesthetistID;
        public long AnesthetistID
        {
            get { return _AnesthetistID; }
            set
            {
                if (_AnesthetistID != value)
                {
                    _AnesthetistID = value;
                    OnPropertyChanged("AnesthetistID");
                }
            }
        }

        private long _AssAnesthetistID;
        public long AssAnesthetistID
        {
            get { return _AssAnesthetistID; }
            set
            {
                if (_AssAnesthetistID != value)
                {
                    _AssAnesthetistID = value;
                    OnPropertyChanged("AssAnesthetistID");
                }
            }
        }

        private long _IVFCycleCount;
        public long IVFCycleCount
        {
            get { return _IVFCycleCount; }
            set
            {
                if (_IVFCycleCount != value)
                {
                    _IVFCycleCount = value;
                    OnPropertyChanged("IVFCycleCount");
                }
            }
        }

        private long _SrcOfNeedleID;
        public long SrcOfNeedleID
        {
            get { return _SrcOfNeedleID; }
            set
            {
                if (_SrcOfNeedleID != value)
                {
                    _SrcOfNeedleID = value;
                    OnPropertyChanged("SrcOfNeedleID");
                }
            }
        }

        private long _NeedleCompanyID;
        public long NeedleCompanyID
        {
            get { return _NeedleCompanyID; }
            set
            {
                if (_NeedleCompanyID != value)
                {
                    _NeedleCompanyID = value;
                    OnPropertyChanged("NeedleCompanyID");
                }
            }
        }

        private long _SrcOfOocyteID;
        public long SrcOfOocyteID
        {
            get { return _SrcOfOocyteID; }
            set
            {
                if (_SrcOfOocyteID != value)
                {
                    _SrcOfOocyteID = value;
                    OnPropertyChanged("SrcOfOocyteID");
                }
            }
        }

        private string _OocyteDonorID;
        public string OocyteDonorID
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

        //By Anjali
        private string _OocyteDonorCode;
        public string OocyteDonorCode
        {
            get { return _OocyteDonorCode; }
            set
            {
                if (_OocyteDonorCode != value)
                {
                    _OocyteDonorCode = value;
                    OnPropertyChanged("OocyteDonorCode");
                }
            }
        }
        
        private long _SrcOfSemenID;
        public long SrcOfSemenID
        {
            get { return _SrcOfSemenID; }
            set
            {
                if (_SrcOfSemenID != value)
                {
                    _SrcOfSemenID = value;
                    OnPropertyChanged("SrcOfSemenID");
                }
            }
        }

        private string _SemenDonorID;
        public string SemenDonorID
        {
            get { return _SemenDonorID; }
            set
            {
                if (_SemenDonorID != value)
                {
                    _SemenDonorID = value;
                    OnPropertyChanged("SemenDonorID");
                }
            }
        }

        //By Anjali
        private string _SemenDonorCode;
        public string SemenDonorCode
        {
            get { return _SemenDonorCode; }
            set
            {
                if (_SemenDonorCode != value)
                {
                    _SemenDonorCode = value;
                    OnPropertyChanged("SemenDonorCode");
                }
            }
        }
        
        private long _TreatmentTypeID;
        public long TreatmentTypeID
        {
            get { return _TreatmentTypeID; }
            set
            {
                if (_TreatmentTypeID != value)
                {
                    _TreatmentTypeID = value;
                    OnPropertyChanged("TreatmentTypeID");
                }
            }
        }        

        private DateTime? _ICSICompletionTime = null;
        public DateTime? ICSICompletionTime
        {
            get { return _ICSICompletionTime; }
            set
            {
                if (_ICSICompletionTime != value)
                {
                    _ICSICompletionTime = value;
                    OnPropertyChanged("ICSICompletionTime");
                }
            }
        }

        private long _SourceOfDenudingNeedle;
        public long SourceOfDenudingNeedle 
        {
            get { return _SourceOfDenudingNeedle; }
            set
            {
                if (_SourceOfDenudingNeedle != value)
                {
                    _SourceOfDenudingNeedle = value;
                    OnPropertyChanged("SourceOfDenudingNeedle");
                }
            }
        }
        
        private DateTime? _FertilizationCheckTime;
        public DateTime? FertilizationCheckTime 
        {
            get { return _FertilizationCheckTime; }
            set
            {
                if (_FertilizationCheckTime != value)
                {
                    _FertilizationCheckTime = value;
                    OnPropertyChanged("FertilizationCheckTime");
                }
            }
        }
        
        private string _OocytePreparationMedia;
        public string OocytePreparationMedia
        {
            get { return _OocytePreparationMedia; }
            set
            {
                if (_OocytePreparationMedia != value)
                {
                    _OocytePreparationMedia = value;
                    OnPropertyChanged("OocytePreparationMedia");
                }
            }
        }

        private string _SpermPreparationMedia;
        public string SpermPreparationMedia
        {
            get { return _SpermPreparationMedia; }
            set
            {
                if (_SpermPreparationMedia != value)
                {
                    _SpermPreparationMedia = value;
                    OnPropertyChanged("SpermPreparationMedia");
                }
            }
        }

        private string _FinalLayering;
        public string FinalLayering
        {
            get { return _FinalLayering; }
            set
            {
                if (_FinalLayering != value)
                {
                    _FinalLayering = value;
                    OnPropertyChanged("FinalLayering");
                }
            }
        }

        private Int32 _Matured;
        public Int32 Matured
        {
            get { return _Matured; }
            set
            {
                if (_Matured != value)
                {
                    _Matured = value;
                    OnPropertyChanged("Matured");
                }
            }
        }

        private Int32 _Immatured;
        public Int32 Immatured
        {
            get { return _Immatured; }
            set
            {
                if (_Immatured != value)
                {
                    _Immatured = value;
                    OnPropertyChanged("Immatured");
                }
            }
        }

        private Int32 _PostMatured;
        public Int32 PostMatured
        {
            get { return _PostMatured; }
            set
            {
                if (_PostMatured != value)
                {
                    _PostMatured = value;
                    OnPropertyChanged("PostMatured");
                }
            }
        }

        private Int32 _Total;
        public Int32 Total
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

        private string _DoneBy;
        public string DoneBy
        {
            get { return _DoneBy; }
            set
            {
                if (_DoneBy != value)
                {
                    _DoneBy = value;
                    OnPropertyChanged("DoneBy");
                }
            }
        }

        private string _FollicularFluid;
        public string FollicularFluid
        {
            get { return _FollicularFluid; }
            set
            {
                if (_FollicularFluid != value)
                {
                    _FollicularFluid = value;
                    OnPropertyChanged("FollicularFluid");
                }
            }
        }

        private Int64 _OPSTypeID;
        public Int64 OPSTypeID
        {
            get { return _OPSTypeID; }
            set
            {
                if (_OPSTypeID != value)
                {
                    _OPSTypeID = value;
                    OnPropertyChanged("OPSTypeID");
                }
            }
        }

        private DateTime? _IVFCompletionTime;
        public DateTime? IVFCompletionTime
        {
            get { return _IVFCompletionTime; }
            set
            {
                if (_IVFCompletionTime != value)
                {
                    _IVFCompletionTime = value;
                    OnPropertyChanged("IVFCompletionTime");
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



    public class IVFTreatment : INotifyPropertyChanged
    {
        public IVFTreatment()
        {
        }

        //IVF
        public MasterListItem Cumulus { get; set; }
        public MasterListItem Grade { get; set; }
        public MasterListItem MOI { get; set; }
        public int Score { get; set; }

        //Common
        public long ID { get; set; }
        public int Index { get; set; }
        public int OocyteNO { get; set; }
        public bool ProceedToDay { get; set; }
        public MasterListItem Plan { get; set; }
        public List<clsFemaleMediaDetailsVO> MediaDetails { get; set; }
        //public MasterListItem Media { get; set; }

        //IVF
        public List<MasterListItem> CumulusSource { get; set; }
        public List<MasterListItem> GradeSource { get; set; }
        public List<MasterListItem> MOISource { get; set; }
        
        //Common
        public List<MasterListItem> PlanSource { get; set; }

        private string _Command;
        public string Command
        {
            get
            {
                return _Command;
            }
            set
            {
                if (_Command != value)
                {
                    _Command = value;
                    OnPropertyChanged("Command");
                }
            }
        }

        private string _FileName;
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                if (_FileName != value)
                {
                    _FileName = value;
                    OnPropertyChanged("FileName");
                }
            }
        }
        public byte[] FileContents { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        #endregion


        //By Anjali.............
        public int SerialOccyteNo { get; set; }
        //........................
    }

    public class ICSITreatment : INotifyPropertyChanged
    {
        public ICSITreatment()
        {
        }

        // ICSI
        public string MBD { get; set; }
        public MasterListItem DOS { get; set; }
        public string Comment { get; set; }
        public MasterListItem PIC { get; set; }
        public string IC { get; set; }

        //Common
        public long ID { get; set; }
        public int Index { get; set; }
        public int OocyteNO { get; set; }
        public bool ProceedToDay { get; set; }
        public MasterListItem Plan { get; set; }
        public List<clsFemaleMediaDetailsVO> MediaDetails { get; set; }
        //public MasterListItem Media { get; set; }

        //ICSI
        public List<MasterListItem> DOSSource { get; set; }
        public List<MasterListItem> PICSource { get; set; }

        //Common
        public List<MasterListItem> PlanSource { get; set; }

        private string _Command;
        public string Command
        {
            get
            {
                return _Command;
            }
            set
            {
                if (_Command != value)
                {
                    _Command = value;
                    OnPropertyChanged("Command");
                }
            }
        }

        private string _FileName;
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                if (_FileName != value)
                {
                    _FileName = value;
                    OnPropertyChanged("FileName");
                }
            }
        }
        public byte[] FileContents { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        #endregion
    }
    
    public class IVFOtherDetails : INotifyPropertyChanged
    {
        public IVFOtherDetails()
        {
        }

        private string _OocytePreparationMedia;
        public string OocytePreparationMedia
        {
            get { return _OocytePreparationMedia; }
            set
            {
                if (_OocytePreparationMedia != value)
                {
                    _OocytePreparationMedia = value;
                    OnPropertyChanged("OocytePreparationMedia");
                }
            }
        }

        private string _SpermPreperationMedia;
        public string SpermPreperationMedia
        {
            get { return _SpermPreperationMedia; }
            set
            {
                if (_SpermPreperationMedia != value)
                {
                    _SpermPreperationMedia = value;
                    OnPropertyChanged("SpermPreperationMedia");
                }
            }
        }

        private string _FinalLayering;
        public string FinalLayering
        {
            get { return _FinalLayering; }
            set
            {
                if (_FinalLayering != value)
                {
                    _FinalLayering = value;
                    OnPropertyChanged("FinalLayering");
                }
            }
        }

        private int _Mature;
        public int Mature
        {
            get { return _Mature; }
            set
            {
                if (_Mature != value)
                {
                    _Mature = value;
                    OnPropertyChanged("Mature");
                }
            }
        }

        private int _Immature;
        public int Immature
        {
            get { return _Immature; }
            set
            {
                if (_Immature != value)
                {
                    _Immature = value;
                    OnPropertyChanged("Immature");
                }
            }
        }

        private int _PostMature;
        public int PostMature
        {
            get { return _PostMature; }
            set
            {
                if (_PostMature != value)
                {
                    _PostMature = value;
                    OnPropertyChanged("PostMature");
                }
            }
        }

        private int _TotalMature;
        public int TotalMature
        {
            get { return _TotalMature; }
            set
            {
                if (_TotalMature != value)
                {
                    _TotalMature = value;
                    OnPropertyChanged("TotalMature");
                }
            }
        }

        private string _DoneBy;
        public string DoneBy
        {
            get { return _DoneBy; }
            set
            {
                if (_DoneBy != value)
                {
                    _DoneBy = value;
                    OnPropertyChanged("DoneBy");
                }
            }
        }
        
        private string _FollocularFluid;
        public string FollocularFluid
        {
            get { return _FollocularFluid; }
            set
            {
                if (_FollocularFluid != value)
                {
                    _FollocularFluid = value;
                    OnPropertyChanged("FollocularFluid");
                }
            }
        }

        private long _OPSTypeID;
        public long OPSTypeID
        {
            get { return _OPSTypeID; }
            set
            {
                if (_OPSTypeID != value)
                {
                    _OPSTypeID = value;
                    OnPropertyChanged("OPSTypeID");
                }
            }
        }

        private DateTime? _IVFCompletionTime = null;
        public DateTime? IVFCompletionTime
        {
            get { return _IVFCompletionTime; }
            set
            {
                if (_IVFCompletionTime != value)
                {
                    _IVFCompletionTime = value;
                    OnPropertyChanged("IVFCompletionTime");
                }
            }
        }
        

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        #endregion
    }

    public class FileUpload : INotifyPropertyChanged
    {
        public FileUpload()
        {
        }

        public long ID { get; set; }
        public long UnitID { get; set; }

        public int Index { get; set; }
        
        public Byte[] Data { get; set; }//Value in Database
        
        //public FileInfo FileInfo { get; set; }

        public string FileName { get; set; }

        private string _Command;
        public string Command
        {
            get
            {
                return _Command;
            }
            set
            {
                if (_Command != value)
                {
                    _Command = value;
                    OnPropertyChanged("Command");
                }
            }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        #endregion
    }

    public class clsAddImageVO : IValueObject, INotifyPropertyChanged
    {
        #region Property Declaration Section

        private string _ImagePath;
        public string ImagePath
        {
            get { return _ImagePath; }
            set
            {
                if (_ImagePath != value)
                {
                    _ImagePath = value;
                    OnPropertyChanged("ImagePath");
                }
            }
        }

        private string _OriginalImagePath;
        public string OriginalImagePath
        {
            get { return _OriginalImagePath; }
            set
            {
                if (_OriginalImagePath != value)
                {
                    _OriginalImagePath = value;
                    OnPropertyChanged("OriginalImagePath");
                }
            }
        }

        //private BitmapImage _BitImagePath;
        //public BitmapImage BitImagePath
        //{
        //    get { return _BitImagePath; }
        //    set
        //    {
        //        if (_BitImagePath != value)
        //        {
        //            _BitImagePath = value;
        //            OnPropertyChanged("BitImagePath");
        //        }
        //    }
        //}

        private byte[] _Photo;
        public byte[] Photo
        {
            get { return _Photo; }
            set
            {
                if (_Photo != value)
                {
                    _Photo = value;
                    OnPropertyChanged("Photo");
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

        private string _SeqNo;
        public string SeqNo
        {
            get { return _SeqNo; }
            set
            {
                if (_SeqNo != value)
                {
                    _SeqNo = value;
                    OnPropertyChanged("SeqNo");
                }
            }
        }

        private string _ServerImageName;
        public string ServerImageName
        {
            get { return _ServerImageName; }
            set
            {
                if (_ServerImageName != value)
                {
                    _ServerImageName = value;
                    OnPropertyChanged("ServerImageName");
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

        private long _Day;
        public long Day
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
