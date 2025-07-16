using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;




namespace PalashDynamics.ValueObjects
{
   public class clsGetRadologyPringHeaderFooterImageBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetRadologyPringHeaderFooterImageBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        public long ID { get; set; }
        public long UnitID { get; set; }

        public byte[] HeaderImage { get; set; }
        public byte[] FooterImage { get; set; }


    }
}
