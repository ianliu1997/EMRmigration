using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
  public  class clsGetItemTaxBizActionVO:IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemTaxBizAction";
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

        private Boolean _CheckForTaxExistatnce;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public Boolean CheckForTaxExistatnce
        {
            get { return _CheckForTaxExistatnce; }
            set { _CheckForTaxExistatnce = value; }
        }

        private Boolean _IsTaxAdded;
        public Boolean IsTaxAdded
        {
            get { return _IsTaxAdded; }
            set { _IsTaxAdded = value; }
        }
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

        public clsItemTaxVO ItemTaxDetails { get; set; }
        public List<clsItemTaxVO> ItemTaxList { get; set; }

        public List<clsItemMasterVO> ItemList { get; set; }
    }

  public class clsGetAllItemTaxDetailBizActionVO : IBizActionValueObject
  {
      public string GetBizAction()
      {
          return "PalashDynamics.BusinessLayer.Inventory.clsGetAllItemTaxDetailBizAction";
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

      public long StoreID { get; set; }
      public long ApplicableFor { get; set; }

      public List<clsItemTaxVO> ItemTaxList;

  }

  public class clsGetItemClinicDetailBizActionVO : IBizActionValueObject
  {
      public string GetBizAction()
      {
          return "PalashDynamics.BusinessLayer.Inventory.clsGetItemClinicDetailBizAction";
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

      public long ItemClinicId { get; set; }
      public List<clsItemTaxVO> ItemTaxList;

  }
}
