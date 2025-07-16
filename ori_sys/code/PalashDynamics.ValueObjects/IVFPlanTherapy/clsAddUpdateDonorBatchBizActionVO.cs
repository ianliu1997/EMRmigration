using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.DashBoardVO;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsAddUpdateDonorBatchBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddUpdateDonorBatchBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        private clsSemenSampleBatchVO _BatchDetails = new clsSemenSampleBatchVO();
        public clsSemenSampleBatchVO BatchDetails
        {
            get
            {
                return _BatchDetails;
            }
            set
            {
                _BatchDetails = value;
            }
        }
        private cls_NewSpremFreezingMainVO _FreezingObj = new cls_NewSpremFreezingMainVO();
        public cls_NewSpremFreezingMainVO FreezingObj
        {
            get
            {
                return _FreezingObj;
            }
            set
            {
                _FreezingObj = value;
            }
        }
        private List<clsNew_SpremFreezingVO> _FreezingDetailsList = new List<clsNew_SpremFreezingVO>();
        public List<clsNew_SpremFreezingVO> FreezingDetailsList
        {
            get
            {
                return _FreezingDetailsList;
            }
            set
            {
                _FreezingDetailsList = value;
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
