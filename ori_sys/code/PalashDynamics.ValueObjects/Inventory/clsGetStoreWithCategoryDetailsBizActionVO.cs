using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetStoreWithCategoryDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetStoreWithCategoryDetailsBizAction";
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

        private List<long> objItemCategoryMaster = new List<long>();
        public List<long> ItemMatserCategoryDetails
        {
            get
            {
                return objItemCategoryMaster;
            }
            set
            {
                objItemCategoryMaster = value;

            }
        }

        public Boolean IsForAll { get; set; }
        public Boolean IsForCategories { get; set; }
     
        public long StoreId { get; set; }
        public long UnitID { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
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


    public class clsFillStoreMasterListBizActionVO : IBizActionValueObject 
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsFillStoreMasterListBizAction";
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

        public long UnitID { get; set; }

        //StoreType - 0 for departmentant store
        //StoreType - 1 for Quarantine store
        //StoreType - 2 for both

        public int StoreType { get; set; }
        private List<clsStoreVO> objStoreMaster = new List<clsStoreVO>();
        public List<clsStoreVO> StoreMasterDetails
        {
            get
            {
                return objStoreMaster;
            }
            set
            {
                objStoreMaster = value;

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
