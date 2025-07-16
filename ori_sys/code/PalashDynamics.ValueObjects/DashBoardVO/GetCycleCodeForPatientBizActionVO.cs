using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class GetCycleCodeForPatientBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.GetCycleCodeForPatientBizActionBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion
        public long CoupleID { get; set; }
        public string CycleCode { get; set; }
        public long CoupleUnitID { get; set; }
        private List<string> _List = null;
        public List<string> List
        {
            get
            { return _List; }

            set
            { _List = value; }
        }
        
    }
    public class GetCycleDetailsFromCycleCodeBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.GetCycleDetailsFromCycleCodeBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion
        public long CoupleID { get; set; }
        public string CycleCode { get; set; }
        public long CoupleUnitID { get; set; }

 
        private clsPlanTherapyVO _TherapyDetails = new clsPlanTherapyVO();
        public clsPlanTherapyVO TherapyDetails
        {
            get
            {
                return _TherapyDetails;
            }
            set
            {
                _TherapyDetails = value;
            }
        }

    }
}
