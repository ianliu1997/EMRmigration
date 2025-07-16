using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsGetThawingDetailsBizActionVO: IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetThawingDetailsBizAction";
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


        private clsThawingVO _Thawing = new clsThawingVO();
        public clsThawingVO Thawing
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

    public class clsThawingVO
    {
        
        public DateTime? Date { get; set; }
        public long LabPerseonID { get; set; }
        public string Impression { get; set; }

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

    public class clsThawingDetailsVO
    {
        public Int64 ID { get; set; }
        public Int64 VitrificationID { get; set; }

        public string EmbNo { get; set; }
        public DateTime? Date { get; set; }
        public long CellStangeID { get; set; }
        public Int64 GradeID { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public Boolean Plan { get; set; }
        //By Anjali..........
        public string SerialOccyteNo { get; set; }
        //.....................


        private List<MasterListItem> _CellStage = new List<MasterListItem>();
        public List<MasterListItem> CellStage 
        {
            get
            {
                return _CellStage;
            }
            set
            {
                _CellStage = value;
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

        private List<MasterListItem> _Grade = new List<MasterListItem>();
        public List<MasterListItem> Grade
        {
            get
            {
                return _Grade;
            }
            set
            {
                _Grade = value;
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

}


