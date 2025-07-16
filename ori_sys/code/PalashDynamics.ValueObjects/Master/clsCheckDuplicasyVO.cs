using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
    public class clsCheckDuplicasyVO : IValueObject
    {
        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public long ItemID { get; set; }

        public long StoreID { get; set; }

        public InventoryTransactionType TransactionTypeID { get; set; }

        public string BatchCode { get; set; }

        public string ItemName { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public bool IsBatchRequired { get; set; }

        public float CostPrice { get; set; }

        public float MRP { get; set; }

        public bool IsFromOpeningBalance { get; set; }

        public bool IsFree { get; set; }  // For Free Items

       // public bool Active { get; set; }
    }
}
