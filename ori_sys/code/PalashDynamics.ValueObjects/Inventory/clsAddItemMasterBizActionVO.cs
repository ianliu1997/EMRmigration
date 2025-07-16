using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
  public  class clsAddItemMasterBizActionVO:IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
                {
                    return "PalashDynamics.BusinessLayer.Inventory.clsAddItemMasterBizAction";
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


         public List<clsItemMasterVO> ItemList { get; set; }

         public long ItemID { get; set; }

    }
}
