using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetValuesforScrapSalesbyItemIDBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return " PalashDynamics.BusinessLayer.Inventory.clsGetValuesforScrapSalesbyItemIDBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        public long ItemId { get; set; }
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsSrcapDetailsVO> objItemMaster = new List<clsSrcapDetailsVO>();
        public List<clsSrcapDetailsVO> ItemMatserDetails
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

    
}
