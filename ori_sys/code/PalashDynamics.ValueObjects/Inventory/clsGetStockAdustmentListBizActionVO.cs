using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetStockAdustmentListBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetStockAdustmentListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        public long StockAdjustmentID { get; set; }
       public long StockAdjustmentUnitID { get; set; }

        public long StoreID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public long UnitID { get; set; }
        public bool PagingEnabled { get; set; }

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


        private List<clsAdjustmentStockVO> _objAdjustStock = new List<clsAdjustmentStockVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsAdjustmentStockVO> AdjustStock
        {
            get { return _objAdjustStock; }
            set { _objAdjustStock = value; }
        }

    }

    public class clsGetMRPAdustmentListBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetMRPAdustmentListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        public long StoreID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public long UnitID { get; set; }
        public bool PagingEnabled { get; set; }

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


        private List<clsMRPAdjustmentVO> _MRPAdjustmentList = new List<clsMRPAdjustmentVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsMRPAdjustmentVO> AdjustmentList
        {
            get { return _MRPAdjustmentList; }
            set { _MRPAdjustmentList = value; }
        }

        private List<clsMRPAdjustmentMainVO> _MRPAdjustmentMainList = new List<clsMRPAdjustmentMainVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsMRPAdjustmentMainVO> AdjustmentMainList
        {
            get { return _MRPAdjustmentMainList; }
            set { _MRPAdjustmentMainList = value; }
        }

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

        public int GetListCriteria { get; set; }  //1=Get Main List , 2=Get Details List 

    }

    public class clsGetStockAdustmentListMainBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetStockAdustmentListMainBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        public long StoreID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public long UnitID { get; set; }
        public bool PagingEnabled { get; set; }

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


        private List<clsAdjustmentStockMainVO> _objAdjustStock = new List<clsAdjustmentStockMainVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsAdjustmentStockMainVO> AdjustStock
        {
            get { return _objAdjustStock; }
            set { _objAdjustStock = value; }
        }


    }
}
