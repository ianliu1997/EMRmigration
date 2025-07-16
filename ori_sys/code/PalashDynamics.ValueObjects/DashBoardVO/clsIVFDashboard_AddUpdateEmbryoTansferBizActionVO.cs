using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_AddUpdateEmbryoTansferBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddUpdateEmbryoTansferBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_EmbryoTransferVO _ETDetails = new clsIVFDashboard_EmbryoTransferVO();
        public clsIVFDashboard_EmbryoTransferVO ETDetails
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

        private List<clsIVFDashboard_EmbryoTransferDetailsVO> _ETDetailsList = new List<clsIVFDashboard_EmbryoTransferDetailsVO>();
        public List<clsIVFDashboard_EmbryoTransferDetailsVO> ETDetailsList
        {
            get
            {
                return _ETDetailsList;
            }
            set
            {
                _ETDetailsList = value;
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

    public class clsIVFDashboard_GetEmbryoTansferBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetEmbryoTansferBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_EmbryoTransferVO _ETDetails = new clsIVFDashboard_EmbryoTransferVO();
        public clsIVFDashboard_EmbryoTransferVO ETDetails
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

        private List<clsIVFDashboard_EmbryoTransferDetailsVO> _ETDetailsList = new List<clsIVFDashboard_EmbryoTransferDetailsVO>();
        public List<clsIVFDashboard_EmbryoTransferDetailsVO> ETDetailsList
        {
            get
            {
                return _ETDetailsList;
            }
            set
            {
                _ETDetailsList = value;
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
