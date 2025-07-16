
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
   
    public class clsAddReceivedItemAgainstReturnBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsAddReceivedItemAgainstReturnBizAction";
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


        public clsReceivedItemAgainstReturnVO ReceivedItemAgainstReturnDetails { get; set; }


    }
}
