using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Collections.ObjectModel;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsSpermVitrifiactionVO : IValueObject, INotifyPropertyChanged
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

        private List<clsSpermVitrificationDetailsVO> _VitrificationDetails = new List<clsSpermVitrificationDetailsVO>();
        public List<clsSpermVitrificationDetailsVO> VitrificationDetails
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


        private List<clsSpermThawingDetailsVO> _ThawingDetails = new List<clsSpermThawingDetailsVO>();
        public List<clsSpermThawingDetailsVO> ThawingDetails
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


    }

    public class clsSpermVitrificationDetailsVO
    {
        public DateTime? FreezingDate { get; set; }
        public DateTime? FreezingTime { get; set; }
        public string ColorCode { get; set; }
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
        public string CanID { get; set; }
        public string CanisterNo { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public string VitrificationNo { get; set; }

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

        public string Volume { get; set; }
        public string SpermCount { get; set; }
        public string Motillity { get; set; }        
    }
       
    public class clsGetSpermVitrificationBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetSpermVitrificationBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        public long CoupleID { get; set; }
        public long CoupleUintID { get; set; }
        public long ID { get; set; }
        public long UnitID { get; set; }
        public bool IsEdit { get; set; }
        public long LabPersonID { get; set; }

        public long PatientId { get; set; }
        public long PatientUnitId { get; set; }
        public bool IsDonor { get; set; }

        private clsSpermVitrifiactionVO _Vitrification = new clsSpermVitrifiactionVO();
        public clsSpermVitrifiactionVO Vitrification
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

    public class clsSpermThawingVO : IValueObject, INotifyPropertyChanged
    {
        #region IValueObject Members
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
        public long ID { get; set; }
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

        private List<clsSpermThawingDetailsVO> _VitrificationDetails = new List<clsSpermThawingDetailsVO>();
        public List<clsSpermThawingDetailsVO> VitrificationDetails
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

    }

    public class clsSpermThawingDetailsVO : IValueObject, INotifyPropertyChanged
    {
        #region IValueObject Members
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

        public long ID { get; set; }
        public long UnitId { get; set; }
        public string ColorCode { get; set; }
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
        public string Status { get; set; }
        public string Comments { get; set; }
        public long VitrificationNo { get; set; }
        public DateTime VitrificationDate { get; set; }
        public DateTime VitrificationTime { get; set; }
        public string Volume { get; set; }
        public string SpermCount { get; set; }
        public string Motillity { get; set; }
        //public long PostThawPlan { get; set; }
        public DateTime? ThawingDate { get; set; }
        public DateTime? ThawingTime { get; set; }
        public bool IsFreezed { get; set; }
        
        public string PostThawPlan { get; set; }
        public long PostThawPlanId { get; set; }
        
        private List<MasterListItem> _PlanIdList = new List<MasterListItem>();
        public List<MasterListItem> PlanIdList
        {
            get
            {
                return _PlanIdList;
            }
            set
            {
                _PlanIdList = value;
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


        public string LabInchargeName { get; set; }
        public long LabInchargeId { get; set; }

        private List<MasterListItem> _InchargeIdList = new List<MasterListItem>();
        public List<MasterListItem> InchargeIdList
        {
            get
            {
                return _InchargeIdList;
            }
            set
            {
                _InchargeIdList = value;
            }
        }

        private MasterListItem _SelectedIncharge = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedIncharge
        {
            get
            {
                return _SelectedIncharge;
            }
            set
            {
                _SelectedIncharge = value;
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

    }

    public class clsGetSpermThawingBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetSpermThawingBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        public long CoupleID { get; set; }
        public long CoupleUintID { get; set; }
        public long ID { get; set; }
        public long UnitID { get; set; }

        private clsSpermThawingVO _Vitrification = new clsSpermThawingVO();
        public clsSpermThawingVO Vitrification
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

    public class clsAddUpdateSpermThawingBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddUpdateSpermThawingBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        public long CoupleID { get; set; }
        public long CoupleUintID { get; set; }
        public long ID { get; set; }
        public long UintID { get; set; }
        public DateTime? FreezingDate { get; set; }
        public DateTime? FreezingTime { get; set; }
        public string Impression { get; set; }
        public long PlanTherapyID { get; set; }
        public long PlanTherapyUnitID { get; set; }
        public long DonorID { get; set; }
        public long DonorUnitID { get; set; }
        private bool _IsNewForm = false;
        public bool IsNewForm
        {
            get
            {
                return _IsNewForm;
            }
            set
            {
                _IsNewForm = value;
            }
        }

        public long PatientId { get; set; }

        //private clsSpermThawingVO _Thawing = new clsSpermThawingVO();
        //public clsSpermThawingVO Thawing
        //{
        //    get
        //    {
        //        return _Thawing;
        //    }
        //    set
        //    {
        //        _Thawing = value;
        //    }
        //}

        private List<clsSpermThawingDetailsVO> _Thawing = new List<clsSpermThawingDetailsVO>();
        public List<clsSpermThawingDetailsVO> Thawing
        {
            get
            {
                return _Thawing;
            }
            set
            {
                _Thawing = value;
            }
        }

        private List<cls_NewThawingDetailsVO> _ThawingList = new List<cls_NewThawingDetailsVO>();
        public List<cls_NewThawingDetailsVO> ThawingList
        {
            get{return _ThawingList;}
            set{_ThawingList = value;}
        }

        private ObservableCollection<cls_NewGetSpremThawingBizActionVO> _ThawDeList = new ObservableCollection<cls_NewGetSpremThawingBizActionVO>();
        public ObservableCollection<cls_NewGetSpremThawingBizActionVO> ThawDeList
        {
            get { return _ThawDeList; }
            set { _ThawDeList = value; }
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
