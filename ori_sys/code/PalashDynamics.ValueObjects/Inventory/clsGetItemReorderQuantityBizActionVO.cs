using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
   public class clsGetItemReorderQuantityBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemReorderQuantityBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }


        public Boolean? IsOrderBy { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string BatchCode { get; set; }
        public double AvailableStock { get; set; }
        public double ReorderStock { get; set; }
        public long clinic { get; set; }
        public long store { get; set; }
        public long Unit { get; set; }
        public long UserID { get; set; }
        public long SupplierID { get; set; }

        public long TotalRow { get; set; }
        public long StartRowIndex { get; set; }
        public long NoOfRecords { get; set; }
        public Boolean IsPagingEnabled { get; set; }

        public List<clsItemReorderDetailVO> ItemReorderList { get; set; }
    }
}
