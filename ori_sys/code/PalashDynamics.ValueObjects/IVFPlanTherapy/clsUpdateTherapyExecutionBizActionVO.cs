using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{

    public class clsUpdateTherapyExecutionBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsUpdateTherapyExecutionBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        public long FollicularID { get; set; }
        public long TherapyID { get; set; }

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
