using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetScrapSalesDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetScrapSalesDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        public long StoreID { get; set; }

        public long UnitID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public long ItemScrapSaleId { get; set; }
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
       

        private List<clsSrcapVO> objItemMaster = new List<clsSrcapVO>();
        public List<clsSrcapVO> MasterDetail
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



    }
}
