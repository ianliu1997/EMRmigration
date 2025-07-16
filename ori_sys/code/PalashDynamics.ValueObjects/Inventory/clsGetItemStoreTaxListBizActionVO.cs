using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetItemStoreTaxListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemStoreTaxListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

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
        private clsItemTaxVO objItemTaxDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsItemTaxVO StoreItemTaxDetails
        {
            get { return objItemTaxDetails; }
            set { objItemTaxDetails = value; }
        }
        private List<clsItemTaxVO> _StoreItemTaxList = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsItemTaxVO> StoreItemTaxList
        {
            get { return _StoreItemTaxList; }
            set { _StoreItemTaxList = value; }
        }

        public bool IsForAllStore { get; set; }  // to get tax list for all store

        public bool ISGetAllStoreTax { get; set; } //***//19
       

    }
}
