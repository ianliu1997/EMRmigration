using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsAddUpdateSupplierBizActionVO : IBizActionValueObject
    {
         #region  IBizActionValueObject
        public string GetBizAction()
        {
            return " PalashDynamics.BusinessLayer.Inventory.clsAddUpdateSupplierBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<SupplierVO> objItemMaster = new List<SupplierVO>();
        public List<SupplierVO> ItemMatserDetails
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
    
    public class SupplierVO
    {
        public long SupplierId { get; set; }
        public long UnitId { get; set; }
        public string SupplierName { get; set; }
        public string Code { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public long City { get; set; }
        public long District { get; set; }
        public long Country { get; set; }
        public long State { get; set; }
        public long Area { get; set; }
        public long Zone { get; set; }
        public long AddressLocation6ID { get; set; }
        public string Pincode { get; set; }
         
        public string ContactPerson1Name { get; set; }
        public string ContactPerson1MobileNo { get; set; }
        public string ContactPerson1Email { get; set; }
        public string ContactPerson2Name { get; set; }
        public string ContactPerson2MobileNo { get; set; }
        public string ContactPerson2Email { get; set; }
        public string PhoneNo { get; set; }
        public string Fax { get; set; }
        
        public long ModeOfPayment { get; set; }
        public long TermofPayment{ get; set; }
        public long TaxNature { get; set; }
        public long Currency { get; set; }
        
        public string MSTNumber { get; set; }
        public string CSTNumber { get; set; }
        public string VAT { get; set; }
        public string DRUGLicence { get; set; }
        public string ServiceTaxNumber { get; set; }
        public string PANNumber { get; set; }

        //Added by MMBABU
        public long SupplierCategoryId { get; set; }
        public string Depreciation { get; set; }
        public string RatingSystem { get; set; }
        //END

        public Boolean MFlag { get; set; }
        public Boolean Status { get; set; }
        public long CreatedUnitID { get; set; }  
        public long UpdatedUnitID { get; set; }
        public long AddedBy { get; set; }
        public string AddedOn { get; set; }
        public DateTime? AddedDateTime { get; set; }
        public long UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public string AddedWindowsLoginName { get; set; }
        public string UpdateWindowsLoginName { get; set; }

        public Boolean PrimaryKeyViolationError { get; set; }
        public Boolean GeneralError { get; set; }

        public int POAutoCloseDays { get; set; }

        public string GSTINNo { get; set; }

        public bool IsFertilityPoint { get; set; } //***//19
        
       
    }
}
