using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_AddUpdateMediaDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddUpdateMediaDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_MediaDetailsVO _MediaDetails = new clsIVFDashboard_MediaDetailsVO();
        public clsIVFDashboard_MediaDetailsVO MediaDetails
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
        private List<clsIVFDashboard_MediaDetailsVO> _MediaDetailsList = new List<clsIVFDashboard_MediaDetailsVO>();
        public List<clsIVFDashboard_MediaDetailsVO> MediaDetailsList
        {
            get
            {
                return _MediaDetailsList;
            }
            set
            {
                _MediaDetailsList = value;
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

        // By BHUSHAN
        private ObservableCollection<clsIVFDashboard_MediaDetailsVO> _ObserMediaList = new ObservableCollection<clsIVFDashboard_MediaDetailsVO>();
        public ObservableCollection<clsIVFDashboard_MediaDetailsVO> ObserMediaList
        {
            get
            {
                return _ObserMediaList;
            }
            set
            {
                _ObserMediaList = value;
            }
        }

        public int ResultStatus { get; set; }
    }

    public class clsIVFDashboard_GetMediaDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetMediaDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_MediaDetailsVO _MediaDetails = new clsIVFDashboard_MediaDetailsVO();
        public clsIVFDashboard_MediaDetailsVO MediaDetails
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

        private List<clsIVFDashboard_MediaDetailsVO> _MediaDetailsList = new List<clsIVFDashboard_MediaDetailsVO>();
        public List<clsIVFDashboard_MediaDetailsVO> MediaDetailsList
        {
            get
            {
                return _MediaDetailsList;
            }
            set
            {
                _MediaDetailsList = value;
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

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long PlanTherapyID { get; set; }
        public long PlanTherapyUnitID { get; set; }
        public string ProcedureName { get; set; }
    }
}
