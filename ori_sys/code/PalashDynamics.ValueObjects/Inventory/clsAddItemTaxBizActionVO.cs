using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsAddItemTaxBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject



        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsAddItemTaxBizAction";
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


        public clsItemTaxVO ItemTax { get; set; }
        public List<clsItemTaxVO> ItemTaxList { get; set; }
        public List<clsItemMasterVO> ItemList { get; set; }
        //Added By Pallavi
        public List<long> TaxIds { get; set; }
    }

    public class clsAddUpdateItemClinicDetailBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsAddUpdateItemClinicDetailBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion
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
        public long StoreClinicID { get; set; }
        public clsItemTaxVO ItemTax { get; set; }
        public List<clsItemTaxVO> ItemTaxList { get; set; }
        public List<clsItemMasterVO> ItemList { get; set; }
        public List<long> TaxIds { get; set; }

    }
}
