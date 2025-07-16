using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
   public class clsAddOpeningBalanceBizActionVO:IBizActionValueObject
    {
        #region  IBizActionValueObject



        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsAddOpeningBalanceBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion



        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private clsItemMasterVO objItemMaster = null;
        public clsItemMasterVO ItemMatserDetails
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


        public List<clsItemMasterVO> ItemList { get; set; }

        public List<clsOpeningBalVO> OpeningBalanceList { get; set; }
        public clsOpeningBalVO  OpeningBalVO { get; set; }
    }
}
