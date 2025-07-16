using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory.PurchaseOrder
{
    public class clsGetPurchaseOrderDetailsBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.PurchaseOrder.clsGetPurchaseOrderDetailsBizAction";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
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
        public bool IsPagingEnabled { get; set; }

        public Int32 StartIndex { get; set; }

        public Int32 MinRows { get; set; }
        //Added By Somnath
        public int TotalRowCount { get; set; }
        //End
        public long SearchID { get; set; }
        public long UnitID { get; set; }

        public clsPurchaseOrderVO PurchaseOrder { get; set; }

        public Boolean FilterPendingQuantity { get; set; }
        public List<clsPurchaseOrderDetailVO> PurchaseOrderList { get; set; }
        public List<clsPurchaseOrderDetailVO> PoIndentList { get; set; }
        public List<clsPurchaseOrderTerms> POTerms { get; set; }
        public bool IsFromGRNAgainstPOSearchWindow = false;   // added by Ashish Z. for PoSerch

        public bool ISPOLastPurchasePrice { get; set; } //***//19
    }
}
