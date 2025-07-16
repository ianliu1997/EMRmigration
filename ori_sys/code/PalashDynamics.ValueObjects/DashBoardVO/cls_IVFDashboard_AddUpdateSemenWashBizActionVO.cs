using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class cls_IVFDashboard_AddUpdateSemenWashBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_IVFDashboard_AddUpdateSemenWashBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private cls_IVFDashboard_SemenWashVO _SemensWashDetails = new cls_IVFDashboard_SemenWashVO();
        public cls_IVFDashboard_SemenWashVO SemensWashDetails
        {
            get
            {
                return _SemensWashDetails;
            }
            set
            {
                _SemensWashDetails = value;
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

    public class cls_IVFDashboard_AddUpdateSemenUsedBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_IVFDashboard_AddUpdateSemenUsedBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private cls_IVFDashboard_SemenWashVO _SemensWashDetails = new cls_IVFDashboard_SemenWashVO();
        public cls_IVFDashboard_SemenWashVO SemensWashDetails
        {
            get
            {
                return _SemensWashDetails;
            }
            set
            {
                _SemensWashDetails = value;
            }
        }

        public List<cls_IVFDashboard_SemenWashVO> ListSemenUsed { get; set; }

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

    public class cls_GetIVFDashboard_SemenWashBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_GetIVFDashboard_SemenWashBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private cls_IVFDashboard_SemenWashVO _SemensExaminationDetails = new cls_IVFDashboard_SemenWashVO();
        public cls_IVFDashboard_SemenWashVO SemensExaminationDetails
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

        private List<cls_IVFDashboard_SemenWashVO> _List = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<cls_IVFDashboard_SemenWashVO> List
        {
            get { return _List; }
            set { _List = value; }
        }

    }

    public class cls_GetIVFDashboard_SemenDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_GetIVFDashboard_SemenDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private cls_IVFDashboard_SemenWashVO _SemensExaminationDetails = new cls_IVFDashboard_SemenWashVO();
        public cls_IVFDashboard_SemenWashVO SemensExaminationDetails
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

        public long PlanTherapyID { get; set; }
        public long PlanTherapyUnitID { get; set; }

        public long UnitID { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public bool IsPagingLoad { get; set; }

        private List<cls_IVFDashboard_SemenWashVO> _List = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<cls_IVFDashboard_SemenWashVO> List
        {
            get { return _List; }
            set { _List = value; }
        }

    }

    public class cls_GetIVFDashboard_SemenUsedBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_GetIVFDashboard_SemenUsedBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private cls_IVFDashboard_SemenWashVO _SemensExaminationDetails = new cls_IVFDashboard_SemenWashVO();
        public cls_IVFDashboard_SemenWashVO SemensExaminationDetails
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

        public long PlanTherapyID { get; set; }
        public long PlanTherapyUnitID { get; set; }


        public long UnitID { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public bool IsPagingLoad { get; set; }

        private List<cls_IVFDashboard_SemenWashVO> _List = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<cls_IVFDashboard_SemenWashVO> ListSaved
        {
            get { return _List; }
            set { _List = value; }
        }

    }

    public class cls_GetIVFDashboard_NewIUIDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_GetIVFDashboard_NewIUIDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private cls_IVFDashboard_SemenWashVO _SemensExaminationDetails = new cls_IVFDashboard_SemenWashVO();
        public cls_IVFDashboard_SemenWashVO SemensExaminationDetails
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

       

        private List<cls_IVFDashboard_SemenWashVO> _List = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<cls_IVFDashboard_SemenWashVO> List
        {
            get { return _List; }
            set { _List = value; }
        }

    }

}
