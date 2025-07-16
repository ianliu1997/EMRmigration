using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetScrapSalesItemsDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetScrapSalesItemsDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        public long ItemScrapSaleId { get; set; }
        public long UnitID { get; set; }
        /// <summary>
        /// This property contains Item master details.
        /// </summary>


        private List<clsSrcapDetailsVO> objItemMaster = new List<clsSrcapDetailsVO>();
        public List<clsSrcapDetailsVO> MasterDetail
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

    public class clsGetDOSReturnDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetDOSReturnDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

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

        public long DosID { get; set; }
        public long UnitID { get; set; }

        public clsSrcapVO DOSMainList { get; set; }
        public List<clsSrcapDetailsVO> DOSItemList { get; set; }

    }

}
