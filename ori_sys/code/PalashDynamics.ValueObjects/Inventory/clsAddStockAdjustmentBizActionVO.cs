using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
   public class clsAddStockAdjustmentBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return " PalashDynamics.BusinessLayer.Inventory.clsAddStockAdjustmentBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        public DateTime DateTime { get; set; }
        public long StockAdjustmentId { get; set; }
        public long StoreId { get; set; }
        public long UnitID { get; set; }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        /// 


        private clsAdjustmentStockMainVO objMain = new clsAdjustmentStockMainVO();
        public clsAdjustmentStockMainVO objMainStock
        {
            get
            {
                return objMain;
            }
            set
            {
                objMain = value;

            }
        }


        private clsAdjustmentStockVO objItems = new clsAdjustmentStockVO();
        public clsAdjustmentStockVO StockAdustmentItem
        {
            get
            {
                return objItems;
            }
            set
            {
                objItems = value;

            }
        }

        private List<clsAdjustmentStockVO> objItemMaster = new List<clsAdjustmentStockVO>();
        public List<clsAdjustmentStockVO> StockAdustmentItems
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


   public class clsAddMRPAdjustmentBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return " PalashDynamics.BusinessLayer.Inventory.clsAddMRPAdjustmentBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        public DateTime DateTime { get; set; }
        public long StockAdjustmentId { get; set; }
        public long StoreId { get; set; }
        public string ItemName { get; set; }


        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private clsMRPAdjustmentMainVO objMRPAdjustmentMain = new clsMRPAdjustmentMainVO();
        public clsMRPAdjustmentMainVO MRPAdjustmentMainVO
        {
            get
            {
                return objMRPAdjustmentMain;
            }
            set
            {
                objMRPAdjustmentMain = value;

            }
        }
        
    
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private clsMRPAdjustmentVO objItems = new clsMRPAdjustmentVO();
        public clsMRPAdjustmentVO MRPAdjustmentItem
        {
            get
            {
                return objItems;
            }
            set
            {
                objItems = value;

            }
        }

        private List<clsMRPAdjustmentVO> objItemMaster = new List<clsMRPAdjustmentVO>();
        public List<clsMRPAdjustmentVO> MRPAdjustmentItems
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

        public int AddCriteria { get; set; }  // 1=Add , 2=Approve

    }

    //By Anjali.............................
   public class clsUpdateStockAdjustmentBizActionVO : IBizActionValueObject
   {
       #region  IBizActionValueObject
       public string GetBizAction()
       {
           return " PalashDynamics.BusinessLayer.Inventory.clsUpdateStockAdjustmentBizAction";
       }

       public string ToXml()
       {
           return this.ToXml();
       }


       #endregion
       public DateTime DateTime { get; set; }
       public long StockAdjustmentId { get; set; }
       public long StoreId { get; set; }
       public long UnitID { get; set; }
       public bool IsForApproval { get; set; }

       /// <summary>
       /// This property contains Item master details.
       /// </summary>
       /// 


       private clsAdjustmentStockMainVO objMain = new clsAdjustmentStockMainVO();
       public clsAdjustmentStockMainVO objMainStock
       {
           get
           {
               return objMain;
           }
           set
           {
               objMain = value;

           }
       }


       private clsAdjustmentStockVO objItems = new clsAdjustmentStockVO();
       public clsAdjustmentStockVO StockAdustmentItem
       {
           get
           {
               return objItems;
           }
           set
           {
               objItems = value;

           }
       }

       private List<clsAdjustmentStockVO> objItemMaster = new List<clsAdjustmentStockVO>();
       public List<clsAdjustmentStockVO> StockAdustmentItems
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
    //.........................................
}
