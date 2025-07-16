using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetStoreDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetStoreDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsStoreVO> objItemMaster = new List<clsStoreVO>();
        public List<clsStoreVO> ItemMatserDetails
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
        /// This property contains To Store List.
        /// </summary>
        private List<clsStoreVO> objToStoreList = new List<clsStoreVO>();
        public List<clsStoreVO> ToStoreList
        {
            get
            {
                return objToStoreList;
            }
            set
            {
                objToStoreList = value;

            }
        }

        public int StoreType { get; set; }
        public long StoreId { get; set; }
        public long UnitID { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
        public bool ForItemStock = false;  //By Umesh
        public bool IsUserwiseStores = false;
        public long UserID { get; set; }
        public long ItemID { get; set; }
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
