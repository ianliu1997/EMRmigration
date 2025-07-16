using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetItemEnquiryBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemEnquiryBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        public long ItemEnquiryId { get; set; }
        public string FilterExpression { get; set; }
        public long SupplierId { get; set; }
      
        public long UnitId { get; set; }
        public long UserID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        /// 

    
        private List<clsItemEnquiryVO> objItemMaster = new List<clsItemEnquiryVO>();
        public List<clsItemEnquiryVO> ItemMatserDetail
        {
            get
            {
                return objItemMaster;
            }
            set
            {
                objItemMaster = value;

            }
        }

       

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

        public long searchStoreID { get; set; }
        public long searchSupplierID { get; set; }
        public DateTime searchFromDate { get; set; }
        public DateTime searchToDate { get; set; }


    }
}
