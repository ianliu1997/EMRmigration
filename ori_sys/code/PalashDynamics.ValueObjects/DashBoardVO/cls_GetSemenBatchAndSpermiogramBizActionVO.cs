using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class cls_GetSemenBatchAndSpermiogramBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_GetSemenBatchAndSpermiogramBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsBatchAndSpemFreezingVO _Details = new clsBatchAndSpemFreezingVO();
        public clsBatchAndSpemFreezingVO Details
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
        private List<clsBatchAndSpemFreezingVO> _DetailsList = new List<clsBatchAndSpemFreezingVO>();
        public List<clsBatchAndSpemFreezingVO> DetailsList
        {
            get
            {
                return _DetailsList;
            }
            set
            {
                _DetailsList = value;
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
