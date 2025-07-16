using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Inventory;

namespace PalashDynamics.ValueObjects.RateContract
{
    public class clsGetRateContractAgainstSupplierAndItemBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.RateContract.clsGetRateContractAgainstSupplierAndItemBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public List<clsRateContractMasterVO> RateContractMasterList { get; set; }
        public List<clsRateContractItemDetailsVO> RateContractItemDetailsList { get; set; }
        public string ItemIDs { get; set; }
        public long SupplierID { get; set; }


        private List<clsRateContractVO> _RateContract = new List<clsRateContractVO>();
        public List<clsRateContractVO> RateContractNew
        {
            get
            {
                return _RateContract;
            }
            set
            {
                _RateContract = value;
            }
        }

        private List<clsRateContractDetailsVO> _RateContractList = new List<clsRateContractDetailsVO>();
        public List<clsRateContractDetailsVO> RateContractListNew
        {
            get
            {
                return _RateContractList;
            }
            set
            {
                _RateContractList = value;
            }
        }

        private List<clsRateContractDetailsVO> _POBestPriceList = new List<clsRateContractDetailsVO>();
        public List<clsRateContractDetailsVO> POBestPriceList
        {
            get
            {
                return _POBestPriceList;
            }
            set
            {
                _POBestPriceList = value;
            }
        }

        private List<clsRateContractDetailsVO> _POLastpurchaseList = new List<clsRateContractDetailsVO>();
        public List<clsRateContractDetailsVO> POLastpurchaseList
        {
            get
            {
                return _POLastpurchaseList;
            }
            set
            {
                _POLastpurchaseList = value;
            }
        }

    }

   
}
