
//Created Date:22/July/2013
//Created By: Nilesh Raut
//Specification: BizAction VO For Get the Patient EMR Current Medication Detail

//Review By:
//Review Date:

//Modified By:
//Modified Date: 
using System.Collections.Generic;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsGetItemMoleculeNameBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetItemMoleculeNameBizAction";
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

        public long ID { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string ItemName { get; set; }
        public long MoleculeID { get; set; }

        public bool isOtherDrug { get; set; }
        public long StoreID { get; set; }
        public long UnitID { get; set; }

        private List<clsItemMoleculeDetails> objItemMoleculeDetails = new List<clsItemMoleculeDetails>();
        public List<clsItemMoleculeDetails> ItemMoleculeDetailsList
        {
            get { return objItemMoleculeDetails; }
            set { objItemMoleculeDetails = value; }
        }

    }
    public class clsItemMoleculeDetails
    {
        public long ItemID { get; set; }
        public long ItemUnitID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string MoleculeName { get; set; }
        public bool Selected { get; set; }
        public long RouteID { get; set; }
        public string Route { get; set; }
        private double _MRP;
        public double MRP { get { return _MRP; } set { _MRP = value; } }
        public double Quantity { get; set; }

    }
}
