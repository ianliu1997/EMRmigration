using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public  class cls_NewGetSpremThawingBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_NewGetSpremThawingBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        public long MalePatientID { get; set; }
        public long MalePatientUnitID { get; set; }
        public long ID { get; set; }
        public long UintID { get; set; } 
        public bool ISModify { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public bool IsFromIUI { get; set; }
        public bool IsThawDetails { get; set; }
        public long PlanTherapyID { get; set; }
        public long PlanTherapyUnitID { get; set; }
        public long DonorID { get; set; }
        public long DonorUnitID { get; set; }
        public string DonorName { get; set; }
        public string DonorMrno { get; set; }

        public string SpremNo { get; set; }
        public DateTime? ThawingDate { get; set; }
        public DateTime? ThawingTime { get; set; }
        public long TotalSpearmCount { get; set; }
        public double Motility { get; set; }
        public long ProgressionID { get; set; }
        public double AbnormalSperm { get; set; }
        public long RoundCells { get; set; }
        public long AgglutinationID { get; set; }
        public string Comments { get; set; }
        public long DoctorID { get; set; }
        public long WitnessedID { get; set; }

        private bool _SelectFreezed;
        public bool SelectFreezed
        {
            get { return _SelectFreezed; }
            set
            {
                if (value != _SelectFreezed)
                {
                    _SelectFreezed = value;
                    //OnPropertyChanged("SelectFreezed");
                }
            }
        }

        //private long _SpremNo;
        //public long SpremNo
        //{
        //    get { return _SpremNo; }
        //    set
        //    {
        //        if (_SpremNo != value)
        //        {
        //            _SpremNo = value;                    
        //        }
        //    }
        //}

        private long _LabPersonId;
        public long LabPersonId
        {
            get { return _LabPersonId; }
            set
            {
                if (_LabPersonId != value)
                {
                    _LabPersonId = value;
                    
                }
            }
        }

        public long LabInchargeId { get; set; }
        public long VitrificationNo { get; set; }
        public string Volume { get; set; }
        public bool IsFreezed { get; set; }
        public long PostThawPlanId { get; set; }

        private long _SemenWashID;
        public long SemenWashID
        {
            get { return _SemenWashID; }
            set
            {
                if (_SemenWashID != value)
                {
                    _SemenWashID = value;                    
                }
            }
        }

        private cls_NewThawingVO _SpremThawingVO = new cls_NewThawingVO();
        public cls_NewThawingVO SpremThawingVO
        {
            get
            {
                return _SpremThawingVO;
            }
            set
            {
                _SpremThawingVO = value;
            }
        }

        private cls_NewThawingDetailsVO _SpremThawingDetailsVO = new cls_NewThawingDetailsVO();
        public cls_NewThawingDetailsVO SpremThawingDetailsVO
        {
            get
            {
                return _SpremThawingDetailsVO;
            }
            set
            {
                _SpremThawingDetailsVO = value;
            }
        }

        private clsNew_SpremFreezingVO _SpremFreezingDetailsVO = new clsNew_SpremFreezingVO();
        public clsNew_SpremFreezingVO SpremFreezingDetailsVO
        {
            get
            {
                return _SpremFreezingDetailsVO;
            }
            set
            {
                _SpremFreezingDetailsVO = value;
            }
        }

        private ObservableCollection<clsNew_SpremFreezingVO> _SpremFreezingDetails = new ObservableCollection<clsNew_SpremFreezingVO>();
        public ObservableCollection<clsNew_SpremFreezingVO> SpremFreezingDetails
        {
            get { return _SpremFreezingDetails; }
            set { _SpremFreezingDetails = value; }
        }

        private ObservableCollection<cls_NewThawingDetailsVO> _SpremThawingDetails = new ObservableCollection<cls_NewThawingDetailsVO>();
        public ObservableCollection<cls_NewThawingDetailsVO> SpremThawingDetails
        {
            get { return _SpremThawingDetails; }
            set { _SpremThawingDetails = value; }
        }

        private List<cls_NewThawingDetailsVO> _SpremThawingDetailsList = new List<cls_NewThawingDetailsVO>();
        public List<cls_NewThawingDetailsVO> SpremThawingDetailsList
        {
            get { return _SpremThawingDetailsList; }
            set { _SpremThawingDetailsList = value; }
        }

        private List<MasterListItem> _ProgressionList = new List<MasterListItem>();
        public List<MasterListItem> ProgressionListVO
        {
            get
            {
                return _ProgressionList;
            }
            set
            {
                _ProgressionList = value;
            }
        }
        private MasterListItem _selectedProgressionListVO = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem selectedProgressionListVO
        {
            get
            {
                return _selectedProgressionListVO;
            }
            set
            {
                _selectedProgressionListVO = value;
            }
        }


        private List<MasterListItem> _AgglutinationList = new List<MasterListItem>();
        public List<MasterListItem> AgglutinationListVO
        {
            get
            {
                return _AgglutinationList;
            }
            set
            {
                _AgglutinationList = value;
            }
        }
        private MasterListItem _selectedAgglutinationListVO = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem selectedAgglutinationListVO
        {
            get
            {
                return _selectedAgglutinationListVO;
            }
            set
            {
                _selectedAgglutinationListVO = value;
            }
        }

        private List<MasterListItem> _DoctorList = new List<MasterListItem>();
        public List<MasterListItem> DoctorListVO
        {
            get
            {
                return _DoctorList;
            }
            set
            {
                _DoctorList = value;
            }
        }
        private MasterListItem _selectedDoctorListVO = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem selectedDoctorListVO
        {
            get
            {
                return _selectedDoctorListVO;
            }
            set
            {
                _selectedDoctorListVO = value;
            }
        }

        private List<MasterListItem> _WitnessedList = new List<MasterListItem>();
        public List<MasterListItem> WitnessedListVO
        {
            get
            {
                return _WitnessedList;
            }
            set
            {
                _WitnessedList = value;
            }
        }
        private MasterListItem _selectedWitnessedListVO = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem selectedWitnessedListVO
        {
            get
            {
                return _selectedWitnessedListVO;
            }
            set
            {
                _selectedWitnessedListVO = value;
            }
        }

    

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

        private bool _IsEnabled;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set
            {
                if (_IsEnabled != value)
                {
                    _IsEnabled = value;
                    
                }
            }
        }

    }

    public class cls_GetSpremFreezingDetilsForThawingBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_GetSpremFreezingDetilsForThawingBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        public long IUIID { get; set; }
        public long SemenWashID { get; set; }
        public long MalePatientID { get; set; }
        public long MalePatientUnitID { get; set; }

        public bool IsView { get; set; }

        public long ID { get; set; }
        public long UnitID { get; set; }


        public bool IsForTemplate { get; set; }
        public long TemplateID { get; set; }
        public DateTime VitrificationDate { get; set; }
        public string DescriptionValue { get; set; }
        private long _PlanTherapyID;
        public long PlanTherapyID
        {
            get { return _PlanTherapyID; }
            set
            {
                if (_PlanTherapyID != value)
                {
                    _PlanTherapyID = value;
                    
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
                    
                }
            }
        }

      

        private clsNew_SpremFreezingVO _SpremFreezingDetailsVO = new clsNew_SpremFreezingVO();
        public clsNew_SpremFreezingVO SpremFreezingDetailsVO
        {
            get
            {
                return _SpremFreezingDetailsVO;
            }
            set
            {
                _SpremFreezingDetailsVO = value;
            }
        }

        private ObservableCollection<clsNew_SpremFreezingVO> _SpremFreezingDetails = new ObservableCollection<clsNew_SpremFreezingVO>();
        public ObservableCollection<clsNew_SpremFreezingVO> SpremFreezingDetails
        {
            get { return _SpremFreezingDetails; }
            set { _SpremFreezingDetails = value; }
        }

        private ObservableCollection<cls_NewThawingDetailsVO> _SpremThawingDetails = new ObservableCollection<cls_NewThawingDetailsVO>();
        public ObservableCollection<cls_NewThawingDetailsVO> SpremThawingDetails
        {
            get { return _SpremThawingDetails; }
            set { _SpremThawingDetails = value; }
        }

        private List<cls_NewThawingDetailsVO> _SpremThawingDetailsList = new List<cls_NewThawingDetailsVO>();
        public List<cls_NewThawingDetailsVO> SpremThawingDetailsList
        {
            get { return _SpremThawingDetailsList; }
            set { _SpremThawingDetailsList = value; }
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
