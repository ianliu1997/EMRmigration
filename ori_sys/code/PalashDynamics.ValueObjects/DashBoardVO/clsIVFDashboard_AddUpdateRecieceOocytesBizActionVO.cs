using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_AddUpdateRecieceOocytesBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddUpdateRecieceOocytesBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsReceiveOocyteVO _OPUDetails = new clsReceiveOocyteVO();
        public clsReceiveOocyteVO OPUDetails
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

        private bool _IsEdit;
        public bool IsEdit
        {
            get
            {
                return _IsEdit;
            }
            set
            {
                _IsEdit = value;
            }
        }

        //private clsIVFDashboard_OPUVO _OPUDetails = new clsIVFDashboard_OPUVO();
        //public clsIVFDashboard_OPUVO OPUDetails
        //{
        //    get
        //    {
        //        return _OPUDetails;
        //    }
        //    set
        //    {
        //        _OPUDetails = value;
        //    }
        //}

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
