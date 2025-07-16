using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
     public class clsAddUpdateStoreDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsAddUpdateStoreDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsStoreVO> objItemMaster = new List<clsStoreVO>();
        public List<clsStoreVO> ItemMatserDetails
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
        public long ID { get; set; }
        public long StoreID { get; set; }
        public bool Status { get; set; }
        private List<long> objItemCategoryID = new List<long>();
        public List<long> ItemCategoryID
        {
            get
            {
                return objItemCategoryID;
            }
            set
            {
                objItemCategoryID = value;

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
        private Nullable<bool> _ApplyallItems;
        public Nullable<bool> ApplyallItems
        {
            get
            {
                return _ApplyallItems;
            }
            set 
            {
                _ApplyallItems = value;
            }
        }

        public bool IsForStatusUpdate = false;
        
    }

     public class clsUpdateCentralStoreDetailsBizActionVO:IBizActionValueObject
     {
         #region IBizActionValueObject
         public string GetBizAction()
         {
             return "PalashDynamics.BusinessLayer.Inventory.clsUpdateCentralStoreDetailsBizAction";
         }
         
         public string ToXml()
         {
             return this.ToXml();
         }
         #endregion

         private clsStoreVO ObjStoreMaster = new clsStoreVO();
         public clsStoreVO StoreMasterDetails
         {
             get
             {
                 return ObjStoreMaster;
             }
             set
             {
                 ObjStoreMaster = value; 
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

    public class clsStoreVO
    {

        public string ToXml()
        {
            return this.ToXml();
        }


        //By Anjali........................................
        public string QuarantineDescription { get; set; }
        public bool IsQuarantineStore { get; set; }
        public long ID { get; set; }
        public string Description { get; set; }
        public long ParentStoreID { get; set; }

        //......................................................
        public long StoreId { get; set; }
        public long ClinicId { get; set; }
        public String ClinicName { get; set; }
        public string StoreName { get; set; }
        public string Code { get; set; }
        public Boolean OpeningBalance { get; set; }
        public Boolean Indent { get; set; }
        public Boolean Issue { get; set; }
        public Boolean ItemReturn { get; set; }
        public Boolean GoodsReceivedNote { get; set; }
        public Boolean GRNReturn { get; set; }
        public Boolean ItemsSale { get; set; }
        public Boolean ItemSaleReturn { get; set; }
        public Boolean ExpiryItemReturn { get; set; }
        public Boolean ReceiveIssue { get; set; }
        public Boolean ReceiveItemReturn { get; set; }
        public Boolean isCentralStore { get; set; }
        public string ItemCatagoryID { get; set; }
        public Boolean Status { get; set; }

       
        public long CategoryID { get; set; }

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
        public bool ForItemStock { get; set; }
        public long CostCenterCodeID { get; set; }
        public long StateID { get; set; }
        private long _Parent;
        public long Parent
        {
            get
            {
                return _Parent;
            }
            set
            {
                _Parent = value;
            }
        }

        private string _ParentName;
        public string ParentName
        {
            get
            {
                return _ParentName;
            }
            set
            {
                _ParentName = value;
            }
        }

        
        public override string ToString()
        {
            if (ForItemStock == true)
            {
                return this.StoreName;
            }
            else
            { return this.ClinicName + " - " + this.StoreName; }
        }
    }
}
