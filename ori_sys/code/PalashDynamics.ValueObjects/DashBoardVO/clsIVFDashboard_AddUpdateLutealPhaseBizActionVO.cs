using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_AddUpdateLutealPhaseBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddUpdateLutealPhaseBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private cls_IVFDashboard_LutualPhaseVO _LutualPhaseDetails = new cls_IVFDashboard_LutualPhaseVO();
        public cls_IVFDashboard_LutualPhaseVO LutualPhaseDetails
        {
            get
            {
                return _LutualPhaseDetails;
            }
            set
            {
                _LutualPhaseDetails = value;
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

    public class clsIVFDashboard_GetLutealPhaseBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetLutealPhaseBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private cls_IVFDashboard_LutualPhaseVO _Details = new cls_IVFDashboard_LutualPhaseVO();
        public cls_IVFDashboard_LutualPhaseVO Details
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
}
