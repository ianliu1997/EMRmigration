using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetStockDetailsForOpeningBalanceBizActionVO:IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetStockDetailsForOpeningBalanceBizAction";
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


        private clsOpeningBalVO _objOpeningBalance = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsOpeningBalVO OpeningBalance
        {
            get { return _objOpeningBalance; }
            set { _objOpeningBalance = value; }
        }

        public List<clsOpeningBalVO> ItemList { get; set; }
    }
}
