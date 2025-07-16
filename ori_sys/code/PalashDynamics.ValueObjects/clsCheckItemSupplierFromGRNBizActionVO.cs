using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects
{
    public class clsCheckItemSupplierFromGRNBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.clsCheckItemSupplierFromGRNBizAction";
        }
     

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        public long ItemID { get; set; }
        public long SupplierID { get; set; }
        public bool checkSupplier { get; set; }

        #endregion
    }
}
