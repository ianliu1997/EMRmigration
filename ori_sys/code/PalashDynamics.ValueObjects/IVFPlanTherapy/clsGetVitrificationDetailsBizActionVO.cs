using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Windows.Media;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Patient;
namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsGetVitrificationDetailsBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetVitrificationDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        public long CoupleID { get; set; }
        public long CoupleUintID { get; set; }
        public bool IsEdit { get; set; }
        public long ID { get; set; }
        public long UnitID { get; set; }
        public int FromID { get; set; }
      
        private clsGetVitrificationVO _Vitrification = new clsGetVitrificationVO();
        public clsGetVitrificationVO Vitrification
        {
            get
            {
                return _Vitrification;
            }
            set
            {
                _Vitrification = value;
            }
        }

        private List<clsFemaleMediaDetailsVO> _MediaDetails = new List<clsFemaleMediaDetailsVO>();
        public List<clsFemaleMediaDetailsVO> MediaDetails
        {
            get
            {
                return _MediaDetails;
            }
            set
            {
                _MediaDetails = value;
            }
        }
       
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
                
            }
        }
    }
    
    public class clsGetVitrificationVO : IValueObject, INotifyPropertyChanged
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
        public string VitrificationNo { get; set; }
        public DateTime? VitrificationDate { get; set; }
        public DateTime? PickupDate  { get; set; }
        public bool ConsentForm  { get; set; }
        public bool IsFreezed { get; set; }
        public bool IsOnlyVitrification { get; set; }        
        public bool ConsentFormYes { get; set; }
        public bool ConsentFormNo { get; set; }
        public string Impression { get; set; }

        private List<clsGetVitrificationDetailsVO> _VitrificationDetails = new List<clsGetVitrificationDetailsVO>();
        public List<clsGetVitrificationDetailsVO> VitrificationDetails
        {
            get
            {
                return _VitrificationDetails;
            }
            set
            {
                _VitrificationDetails = value;
            }
        }

        private List<clsThawingDetailsVO> _ThawingDetails = new List<clsThawingDetailsVO>();
        public List<clsThawingDetailsVO> ThawingDetails
        {
            get
            {
                return _ThawingDetails;
            }
            set
            {
                _ThawingDetails = value;
            }
        }
               
        private List<FileUpload> _FUSetting = new List<FileUpload>();
        public List<FileUpload> FUSetting
        {
            get
            {
                return _FUSetting;
            }
            set
            {
                _FUSetting = value;
            }
        }
    }

    public class clsGetVitrificationDetailsVO : IValueObject, INotifyPropertyChanged
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

        public Int64 ID { get; set; }
        public string EmbNo { get; set; }

        //By Anjali................
        public string SerialOccyteNo { get; set; }
        //.................................
        public string LeafNo { get; set; }
        public string PatientUnitName { get; set; }
       
        public string EmbDays { get; set; }
        public string ColorCode { get; set; }

        //public System.Windows.Media.Color SelectesColor;

        private System.Windows.Media.Color _SelectesColor = new System.Windows.Media.Color() { A = 255, B = 0, G = 0, R = 128 };
        public System.Windows.Media.Color SelectesColor
        {
            get
            {
                return _SelectesColor;
            }
            set
            {
                _SelectesColor = value;
                ColorCode = _SelectesColor.ToString();
                
            }
        }
        public string ProtocolType { get; set; }
        public Int64 ProtocolTypeID { get; set; }
        public Int64 LabDayID { get; set; }

        public string CanID { get; set; }
        public string ConistorNo { get; set; }
        public string SOOcytes { get; set; }
        public Int64 SOOcytesID { get; set; }
        public string OSCode { get; set; }
        public Int64 SOSemenID { get; set; }
        public string SOSemen { get; set; }
        public string SSCode { get; set; }

        public DateTime? TransferDate { get; set; }
        public string TransferDay { get; set; }
        public string CellStange { get; set; }
        public long CellStangeID { get; set; }
        public string Grade { get; set; }
        public Int64 GradeID { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }

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

        public string StrawId { get; set; }
        public string GobletShapeId { get; set; }
        public string GobletSizeId { get; set; }
        public string CanisterId { get; set; }
        public string TankId { get; set; }

        public long VitrificationNo { get; set; }
        public DateTime VitrificationDate { get; set; }
        public string EmbStatus { get; set; }
        public bool DoneThawing { get; set; }

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

        private string _FirstName = "";
        //[Required(ErrorMessage = "First Name Required")]
        //[StringLength(50, ErrorMessage = "First Name must be in between 1 to 50 Characters")]
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        private string _MiddleName = "";
        public string MiddleName
        {
            get
            {
                return _MiddleName;
            }
            set
            {
                if (_MiddleName != value)
                {
                    _MiddleName = value;
                    OnPropertyChanged("MiddleName");
                }
            }
        }

        private string _LastName = "";
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

        //

        private List<clsFemaleMediaDetailsVO> _MediaDetails = new List<clsFemaleMediaDetailsVO>();
        public List<clsFemaleMediaDetailsVO> MediaDetails
        {
            get
            {
                return _MediaDetails;
            }
            set
            {
                _MediaDetails = value;
            }
        }

    }

    public class clsGetVitrificationForCryoBankBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetVitrificationForCryoBankBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        //public long CoupleID { get; set; }
        public long CoupleUintID { get; set; }
        public bool IsEdit { get; set; }
        public long ID { get; set; }
        public long UnitID { get; set; }
        public int FromID { get; set; }
        public long PatientID { get; set; }


        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }

        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string FamilyName { get; set; }
        public string MRNo { get; set; }
        public string CtcNo { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        private clsGetVitrificationVO _Vitrification = new clsGetVitrificationVO();
        public clsGetVitrificationVO Vitrification
        {
            get
            {
                return _Vitrification;
            }
            set
            {
                _Vitrification = value;
            }
        }
        
        private List<clsFemaleMediaDetailsVO> _MediaDetails = new List<clsFemaleMediaDetailsVO>();
        public List<clsFemaleMediaDetailsVO> MediaDetails
        {
            get
            {
                return _MediaDetails;
            }
            set
            {
                _MediaDetails = value;
            }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;

            }
        }
    }
}
