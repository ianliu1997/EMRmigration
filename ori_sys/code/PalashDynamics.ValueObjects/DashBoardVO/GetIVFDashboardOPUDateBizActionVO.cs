using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class GetIVFDashboardOPUDateBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.GetIVFDashboardOPUDateBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        public long CoupleID { get; set; }
        public string CycleCode { get; set; }
        public long CoupleUnitID { get; set; }

        public long UnitID { get; set; }

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }


        public long PlanTherapyID { get; set; }
        public long PlanTherapyUnitID { get; set; }

        private List<clsIVFDashboard_OPUVO> _List = null;
        public List<clsIVFDashboard_OPUVO> List
        {
            get
            { return _List; }

            set
            { _List = value; }
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

