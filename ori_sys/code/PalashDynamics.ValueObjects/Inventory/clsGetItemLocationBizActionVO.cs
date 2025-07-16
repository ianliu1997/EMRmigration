using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetItemLocationBizActionVO: IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return " PalashDynamics.BusinessLayer.Inventory.clsGetItemLocationBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

       private clsItemLocationVO _ItemLocationDetails = null;
        
        public clsItemLocationVO ItemLocationDetails
        {
            get { return _ItemLocationDetails; }
            set { _ItemLocationDetails = value; }
        }
        
        private List<clsItemMasterVO> _StoreList = new List<clsItemMasterVO>();
        public List<clsItemMasterVO> StoreList
        {
            get
            {
                return _StoreList;
            }
            set
            {
                _StoreList = value;

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

        private List<long> _StoreDetails;
        public List<long> StoreDetails
        {
            get { return _StoreDetails; }
            set { _StoreDetails = value; }           
        }

        
    }

}

