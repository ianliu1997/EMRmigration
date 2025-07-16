using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class ClsGetSupplierDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.ClsGetSupplierDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<SupplierVO> objItemMaster = new List<SupplierVO>();
        public List<SupplierVO> ItemMatserDetails
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
        //public bool SupplierPaymentMode { get; set; }
        public long SupplierId { get; set; }
        public long UnitID { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long ModeOfPayment { get; set; }
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

        private bool _SupplierPaymentMode=false;
        public bool SupplierPaymentMode
        {
            get
            {
                return _SupplierPaymentMode;
            }
            set
            {
                _SupplierPaymentMode = value;
            }
        }

    }
}
