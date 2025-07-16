using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsAddExpiryItemReturnBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsAddExpiryItemReturnBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        public clsExpiredItemReturnVO objExpiryItem { get; set; }

        public List<clsExpiredItemReturnDetailVO> objExpiryItemDetailList { get; set; }


        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
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

        public bool IsForApproveClick { get; set; }
    }

    public class clsUpdateExpiryForApproveRejectVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsUpdateExpiryForApproveReject";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        public Boolean IsEditMode { get; set; }
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

        public clsExpiredItemReturnVO objExpiryItem { get; set; }
        public List<clsExpiredItemReturnDetailVO> objExpiryItemDetailsList { get; set; }
    }

}
