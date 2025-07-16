using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class cls_IVFDashboard_AddSemenBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_IVFDashboard_AddSemenBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        
        private cls_IVFDashboard_SemenVO _SemensExaminationDetails = new cls_IVFDashboard_SemenVO();
        public cls_IVFDashboard_SemenVO SemensExaminationDetails
        {
            get
            {
                return _SemensExaminationDetails;
            }
            set
            {
                _SemensExaminationDetails = value;
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

    public class cls_GetIVFDashboard_SemenBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_GetIVFDashboard_SemenBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private cls_IVFDashboard_SemenVO _SemensExaminationDetails = new cls_IVFDashboard_SemenVO();
        public cls_IVFDashboard_SemenVO SemensExaminationDetails
        {
            get
            {
                return _SemensExaminationDetails;
            }
            set
            {
                _SemensExaminationDetails = value;
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

        public long PatientUnitID { get; set; }
        public long PatientID { get; set; }
        public long UnitID { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public bool IsPagingLoad { get; set; } 

        private List<cls_IVFDashboard_SemenVO> _List = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<cls_IVFDashboard_SemenVO> List
        {
            get { return _List; }
            set { _List = value; }
        }

    }
}
