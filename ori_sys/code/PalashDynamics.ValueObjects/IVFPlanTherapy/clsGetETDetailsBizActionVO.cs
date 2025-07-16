using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsGetETDetailsBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetETDetailsBizAction";
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


        private ETVO _ET = new ETVO();
        public ETVO ET
        {
            get
            {
                return _ET;
            }
            set
            {
                _ET = value;
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
    public class ETVO
    {
            public long ID { get; set; }
            public long UnitID { get; set; }
            public long CoupleID { get; set; }
            public long CoupleUnitID { get; set; }
            public DateTime? Date { get; set; }
            public long EmbryologistID { get; set; }
            public long AssEmbryologistID { get; set; }
            public long AnasthesistID { get; set; }
            public long AssAnasthesistID { get; set; }
            
            /* Added By sudhir on 13/09/2013*/
            public long EndometriumThickness { get; set; }
            public long ETPattern { get; set; }
            public string ColorDopper { get; set; }
        //By Anjali...
            public Boolean IsUterinePI { get; set; }
            public Boolean IsUterineRI { get; set; }
            public Boolean IsUterineSD{ get; set; }
            public Boolean IsEndometerialPI { get; set; }
            public Boolean IsEndometerialRI { get; set; }
            public Boolean IsEndometerialSD { get; set; }

            public Boolean IsTreatmentUnderGA { get; set; }
            public long CatheterTypeID { get; set; } 
            public bool Difficulty { get; set; }
            public long DifficultyTypeID { get; set; }
            public Boolean IsFreezed { get; set; } 
            public bool Status { get; set; }
            public long CreatedUnitID { get; set; }
            public long AddedBy { get; set; }
            public string AddedOn { get; set; }
            public DateTime AddedDateTime { get; set; }
            public string AddedWindowsLoginName { get; set; }
            public string Impression { get; set; }
            public bool IsOnlyET { get; set; }
            public bool TenaculumUsed { get; set; }
            public string DistanceFromFundus { get; set; }
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

            private List<ETDetailsVO> _ETDetails = new List<ETDetailsVO>();
            public List<ETDetailsVO> ETDetails
            {
                get
                {
                    return _ETDetails;
                }
                set
                {
                    _ETDetails = value;
                }
            }

            public string SrcSemenCode { get; set; }
            public long SrcSemenID { get; set; }

            public string SrcOoctyCode { get; set; }
            public long SrcOoctyID { get; set; }

    }

    public class ETDetailsVO :  INotifyPropertyChanged
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


            public long ID { get; set; }
            public long ETUnitID { get; set; }
            public long ETID { get; set; }
            public long LabDayID { get; set; }
            public string EmbNO { get; set; }
        //By Anjali................
            public string SerialOccyteNo { get; set; }
        //.......................

            public DateTime? TransferDate { get; set; }
            public string TransferDay { get; set; }
            public string Grade { get; set; }
            public long GradeID { get; set; }
            public string Score { get; set; }
            public string FertilizationStage { get; set; }
            public long FertilizationStageID { get; set; }
            public string EmbStatus { get; set; }
            public bool Status { get; set; }
            public long PlanID { get; set; }
            //public string  FileName { get; set; }
            public byte[] FileContents{ get; set; }

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
	
            private List<MasterListItem> _FertilizationList = new List<MasterListItem>();
            public List<MasterListItem> FertilizationList
            {
                get
                {
                    return _FertilizationList;
                }
                set
                {
                    _FertilizationList = value;
                }
            }

            private MasterListItem _SelectedFertilizationStage = new MasterListItem { ID = 0, Description = "--Select--" };
            public MasterListItem SelectedFertilizationStage
            {
                get
                {
                    return _SelectedFertilizationStage;
                }
                set
                {
                    _SelectedFertilizationStage = value;
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

            private MasterListItem _SelectedPlan = new MasterListItem { ID = 0, Description = "--Select--" };
            public MasterListItem SelectedPlan
            {
                get
                {
                    return _SelectedPlan;
                }
                set
                {
                    _SelectedPlan = value;
                }
            }
    }

}
