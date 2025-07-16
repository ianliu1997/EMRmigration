using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.CompoundDrug;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsAddItemSalesBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.clsAddItemSalesBizAction";
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

        public bool myTransaction { get; set; }

        private clsItemSalesVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsItemSalesVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }


        private List<clsCompoundDrugMasterVO> _CompoundDrugMaster = new List<clsCompoundDrugMasterVO>();
        public List<clsCompoundDrugMasterVO> CompoundDrugMaster
        {
            get { return _CompoundDrugMaster; }
            set { _CompoundDrugMaster = value; }
        }

        private List<clsCompoundDrugDetailVO> _CompoundDrugDetails = new List<clsCompoundDrugDetailVO>();
        public List<clsCompoundDrugDetailVO> CompoundDrugDetails
        {
            get { return _CompoundDrugDetails; }
            set { _CompoundDrugDetails = value; }
        }
    }

    public class clsGetItemSalesCompleteBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemSalesCompleteBizAction";
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


        public long BillID { get; set; }
        public long BillUnitID { get; set; }

        public bool IsBilled { get; set; }
        //Added by Aj Date 1/2/2017
        public long? AdmID { get; set; }
        public long? AdmissionUnitID { get; set; }
        //***//--------
        private clsItemSalesVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsItemSalesVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
    }

}
