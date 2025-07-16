using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
  public   class clsGetStoreStatusBizActionVO:IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetStoreStatusBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
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

       
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>

        private clsItemMasterVO objItemMater = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsItemMasterVO ItemDetails
        {
            get { return objItemMater; }
            set { objItemMater = value; }
        }

       

        public List<clsItemMasterVO> ItemList { get; set; }
    }
}
