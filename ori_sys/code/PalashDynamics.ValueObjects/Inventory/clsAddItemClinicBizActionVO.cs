using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
 public   class clsAddItemClinicBizActionVO:IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsAddItemClinicBizAction";
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
        private clsItemStoreVO objItemStore = null;
        public clsItemStoreVO ItemStoreDetails
        {
            get
            {
                return objItemStore;
            }
            set
            {
                objItemStore = value;

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
        public int ResultStatus { get; set; }


        public List<clsItemMasterVO> ItemList { get; set; }

        public clsItemStoreVO ItemStore { get; set; }
        public bool IsForDelete = false;
        public long DeleteTaxID { get; set; }


        private List<clsItemTaxVO> _ItemTaxList = new List<clsItemTaxVO>();
        public List<clsItemTaxVO> ItemTaxList
        {
            get
            {
                return _ItemTaxList;
            }
            set
            {
                _ItemTaxList = value;
            }
        }

     //***//19-------------------------
        private List<clsItemMasterVO> _ItemLinkStoreList = new List<clsItemMasterVO>(); 
        public List<clsItemMasterVO> ItemLinkStoreList
        {
            get
            {
                return _ItemLinkStoreList;
            }
            set
            {
                _ItemLinkStoreList = value;
            }
        }

        


        public bool ISAddMultipleStoreTax { get; set; }
        public bool ISMultipleStoreTax { get; set; }
        public long MultipleStoreTaxID { get; set; } 
//-----------------------------------------
        //public bool ApplyToAllStore =false;//{ get; set; }
        //public List<long> lstStoreIds { get; set; }
    }
}
