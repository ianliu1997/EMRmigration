using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.Inventory;

namespace PalashDynamics.BusinessLayer.Inventory
{
    internal class clsGetBlockItemsListBizAction : BizAction
      {
        
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetBlockItemsListBizAction()     
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }


        private static clsGetBlockItemsListBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetBlockItemsListBizAction();
           
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsGetBlockItemsListBizActionVO obj = null;
            try
            {
                obj = (clsGetBlockItemsListBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseItemMasterDAL objBaseItemDal = clsBaseItemMasterDAL.GetInstance();
                    if (obj.IsfromSuspendStockSearchItems)
                    {
                        obj = (clsGetBlockItemsListBizActionVO)objBaseItemDal.clsItemListForSuspendStockSearch(obj, objUserVO);
                    }
                    else if (obj.IsForCheckInTransitItems)
                    {
                        obj = (clsGetBlockItemsListBizActionVO)objBaseItemDal.IsForCheckInTransitItems(obj, objUserVO);
                    }
                    else
                    {
                        obj = (clsGetBlockItemsListBizActionVO)objBaseItemDal.clsGetBlockItemList(obj, objUserVO);
                    }
                }
            }
            catch (HmsApplicationException HEx)
            {

                CurrentMethodExecutionStatus = false;
                throw;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return obj;
        }

    }

    

    internal class clsAddUpdatePhysicalItemStockBizAction : BizAction
      {
        
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsAddUpdatePhysicalItemStockBizAction()     
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }


        private static clsAddUpdatePhysicalItemStockBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsAddUpdatePhysicalItemStockBizAction();
           
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsAddUpdatePhysicalItemStockBizActionVO obj = null;
            try
            {
                obj = (clsAddUpdatePhysicalItemStockBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseItemMasterDAL objBaseItemDal = clsBaseItemMasterDAL.GetInstance();
                    obj = (clsAddUpdatePhysicalItemStockBizActionVO)objBaseItemDal.AddUpdatePhysicalItemStock(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                CurrentMethodExecutionStatus = false;
                throw;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return obj;
        }

    }

    internal class clsGetPhysicalItemStockBizAction : BizAction
      {
        
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetPhysicalItemStockBizAction()     
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }


        private static clsGetPhysicalItemStockBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetPhysicalItemStockBizAction();
           
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsGetPhysicalItemStockBizActionVO obj = null;
            try
            {
                obj = (clsGetPhysicalItemStockBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseItemMasterDAL objBaseItemDal = clsBaseItemMasterDAL.GetInstance();
                    obj = (clsGetPhysicalItemStockBizActionVO)objBaseItemDal.GetPhysicalItemStock(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                CurrentMethodExecutionStatus = false;
                throw;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return obj;
        }

        //protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        //{
        //    throw new NotImplementedException();
        //}
    }

      internal class clsGetPhysicalItemStockDetailsBizAction : BizAction
      {
        
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetPhysicalItemStockDetailsBizAction()     
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }


        private static clsGetPhysicalItemStockDetailsBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetPhysicalItemStockDetailsBizAction();
           
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsGetPhysicalItemStockDetailsBizActionVO obj = null;
            try
            {
                obj = (clsGetPhysicalItemStockDetailsBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseItemMasterDAL objBaseItemDal = clsBaseItemMasterDAL.GetInstance();
                    obj = (clsGetPhysicalItemStockDetailsBizActionVO)objBaseItemDal.GetPhysicalItemStockDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                CurrentMethodExecutionStatus = false;
                throw;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return obj;
        }

        //protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        //{
        //    throw new NotImplementedException();
        //}
    }


    
}
