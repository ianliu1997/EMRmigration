using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetBlockItemsListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return " PalashDynamics.BusinessLayer.Inventory.clsGetBlockItemsListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        /// 

        public int TotalRowCount { get; set; }
        public int MaximumRows { get; set; }

        public Boolean PagingEnabled { get; set; }
        public int StartRowIndex { get; set; }
        public bool IsfromSuspendStockSearchItems = false;
        public bool IsForCheckInTransitItems = false;

        private List<clsBlockItemsVO> objItemList = new List<clsBlockItemsVO>();
        public List<clsBlockItemsVO> ItemDetailsList
        {
            get
            {
                return objItemList;
            }
            set
            {
                objItemList = value;

            }
        }
        private clsBlockItemsVO objItem = new clsBlockItemsVO();
        public clsBlockItemsVO ItemDetails
        {
            get
            {
                return objItem;
            }
            set
            {
                objItem = value;

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

    public class clsAddUpdatePhysicalItemStockBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return " PalashDynamics.BusinessLayer.Inventory.clsAddUpdatePhysicalItemStockBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        /// 

        public int TotalRowCount { get; set; }
        public int MaximumRows { get; set; }

        public Boolean PagingEnabled { get; set; }
        public int StartRowIndex { get; set; }

        private List<clsPhysicalItemsVO> objItemList = new List<clsPhysicalItemsVO>();
        public List<clsPhysicalItemsVO> ItemDetailsList
        {
            get
            {
                return objItemList;
            }
            set
            {
                objItemList = value;

            }
        }
        private clsPhysicalItemsMainVO objItem = new clsPhysicalItemsMainVO();
        public clsPhysicalItemsMainVO ItemDetails
        {
            get
            {
                return objItem;
            }
            set
            {
                objItem = value;

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

    public class clsGetPhysicalItemStockBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return " PalashDynamics.BusinessLayer.Inventory.clsGetPhysicalItemStockBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        /// 

        public int TotalRowCount { get; set; }
        public int MaximumRows { get; set; }

        public Boolean PagingEnabled { get; set; }
        public int StartRowIndex { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long StoreID { get; set; }

        private List<clsPhysicalItemsMainVO> objItemList = new List<clsPhysicalItemsMainVO>();
        public List<clsPhysicalItemsMainVO> ItemDetailsList
        {
            get
            {
                return objItemList;
            }
            set
            {
                objItemList = value;

            }
        }
        private clsPhysicalItemsMainVO objItem = new clsPhysicalItemsMainVO();
        public clsPhysicalItemsMainVO ItemDetails
        {
            get
            {
                return objItem;
            }
            set
            {
                objItem = value;

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

    public class clsGetPhysicalItemStockDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return " PalashDynamics.BusinessLayer.Inventory.clsGetPhysicalItemStockDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        /// 

        //public int TotalRowCount { get; set; }
        //public int MaximumRows { get; set; }

        //public Boolean PagingEnabled { get; set; }
        public long PhysicalItemUnitID { get; set; }
        public long PhysicalItemID { get; set; }

        private List<clsPhysicalItemsVO> objItemList = new List<clsPhysicalItemsVO>();
        public List<clsPhysicalItemsVO> ItemDetailsList
        {
            get
            {
                return objItemList;
            }
            set
            {
                objItemList = value;

            }
        }
        private clsPhysicalItemsVO objItem = new clsPhysicalItemsVO();
        public clsPhysicalItemsVO ItemDetails
        {
            get
            {
                return objItem;
            }
            set
            {
                objItem = value;

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
