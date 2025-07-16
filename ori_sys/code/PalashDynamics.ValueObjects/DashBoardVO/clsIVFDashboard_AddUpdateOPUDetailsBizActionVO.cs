using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_AddUpdateOPUDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddUpdateOPUDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_OPUVO _OPUDetails = new clsIVFDashboard_OPUVO();
        public clsIVFDashboard_OPUVO OPUDetails
        {
            get
            {
                return _OPUDetails;
            }
            set
            {
                _OPUDetails = value;
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

        private clsTherapyExecutionVO _TherapyExecutionDetial = new clsTherapyExecutionVO();
        public clsTherapyExecutionVO TherapyExecutionDetial
        {
            get
            {
                return _TherapyExecutionDetial;
            }
            set
            {
                _TherapyExecutionDetial = value;
            }
        }
    }
    public class clsIVFDashboard_GetOPUDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetOPUDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_OPUVO _Details = new clsIVFDashboard_OPUVO();
        public clsIVFDashboard_OPUVO Details
        {
            get
            {
                return _Details;
            }
            set
            {
                _Details = value;
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

    //added by neena dated 28/7/16
    public class clsIVFDashboard_AddUpdateOocyteNumberBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddUpdateOocyteNumberBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_OPUVO _OPUDetails = new clsIVFDashboard_OPUVO();
        public clsIVFDashboard_OPUVO OPUDetails
        {
            get
            {
                return _OPUDetails;
            }
            set
            {
                _OPUDetails = value;
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

        private clsTherapyExecutionVO _TherapyExecutionDetial = new clsTherapyExecutionVO();
        public clsTherapyExecutionVO TherapyExecutionDetial
        {
            get
            {
                return _TherapyExecutionDetial;
            }
            set
            {
                _TherapyExecutionDetial = value;
            }
        }
    }

    public class clsIVFDashboard_GetEmbryologySummaryBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetEmbryologySummaryBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        private clsIVFDashboard_EmbryologySummary _EmbSummary = new clsIVFDashboard_EmbryologySummary();
        public clsIVFDashboard_EmbryologySummary EmbSummary
        {
            get
            {
                return _EmbSummary;
            }
            set
            {
                _EmbSummary = value;
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
    
    //
}


