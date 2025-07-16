using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetExpiredItemListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetExpiredItemListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public long? StoreID { get; set; }
        public long? SupplierID { get; set; }
        public long? ExpiredID { get; set; }
        public long? UnitID { get; set; }

        //public bool CheckStatusType { get; set; }
        //public int IndentStatus { get; set; }

        public List<clsExpiredItemReturnDetailVO> ExpiredItemList { get; set; }

        public clsExpiredItemReturnDetailVO objExpiredItemsVO { get; set; }

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

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public bool IsFromDOS { get; set; }  // Set true from ScrapSale.xaml.cs
    }

    public class clsGetExpiredListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetExpiredListBizAction";
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

        public long StoreId { get; set; }


        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long UnitID { get; set; }
        public long SupplierID { get; set; }
        public long StoreID { get; set; }

        public List<clsExpiredItemReturnVO> ExpiredList { get; set; }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }



    }

    public class clsGetExpiredListForExpiredReturnBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetExpiredListForExpiredReturnBizAction";
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

        public long ConsumptionID { get; set; }
        public long UnitID { get; set; }

        public List<clsExpiredItemReturnDetailVO> ItemList { get; set; }




    }

    public class clsGetExpiredReturnDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetExpiredReturnDetailsBizAction";
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

        public long ExpiredID { get; set; }
        public long UnitID { get; set; }

        public clsExpiredItemReturnVO ExpiredMainList { get; set; }
        public List<clsExpiredItemReturnDetailVO> ExpiredItemList { get; set; }

    }

}
