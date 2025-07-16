using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
{
  public abstract  class clsBasePurchaseOrderDAL
    {
      static private clsBasePurchaseOrderDAL _instance = null;

      public static clsBasePurchaseOrderDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "Inventory.clsPurchaseOrderDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBasePurchaseOrderDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }
      public abstract IValueObject CheckContractValidity(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject GetRateContract(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject GetRateContractItemDetail(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject AddRateContract(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject AddPurchaseOrder(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject GetPurchaseOrder(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject GetPurchaseOrderDetails(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject DeletePurchaseOrderItems(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject GetPendingPurchaseOrder(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject FreezPurchaseOrder(IValueObject valueObject, clsUserVO UserVo);
      
      public abstract IValueObject CancelPurchaseOrder(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject UpdatePurchaseOrderForApproval(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject UpdateRemarkForCancellationPO(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject ClosePurchaseOrderManually(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject GetPurchaseOrderToCloseManually(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject GetPurchaseOrderDetailsForGRNAgainstPOSearch(IValueObject valueObject, clsUserVO UserVo);  // added by Ashish Z.

      public abstract IValueObject UpdateForPOCloseDuration(IValueObject valueObject, clsUserVO UserVo);            // Included by Rex Mathew. On 12Oct2018

      public abstract IValueObject GetLastTherrPODetails(IValueObject valueObject, clsUserVO UserVo); //***//19

    }
}
