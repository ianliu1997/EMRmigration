using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.ValueObjects.DashBoardVO
{

    public class clsGetDetailsOfReceivedOocyteBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsGetDetailsOfReceivedOocyteBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsReceiveOocyteVO _Details = new clsReceiveOocyteVO();
        public clsReceiveOocyteVO Details
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
    }

    //added by neena for getting donated oocyte and embryo
    public class clsGetDetailsOfReceivedOocyteEmbryoBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsGetDetailsOfReceivedOocyteEmbryoBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsReceiveOocyteVO _Details = new clsReceiveOocyteVO();
        public clsReceiveOocyteVO Details
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
    }
    //

}
