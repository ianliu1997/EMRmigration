using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    public abstract class clsBaseIndentDAL
    {
        static private clsBaseIndentDAL _instance = null;

        public static clsBaseIndentDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsIndentDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIndentDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }


        /// <summary>
        /// </summary>
        /// <param name="valueObject"></param>
        /// <returns></returns>
        public abstract IValueObject GetIndentList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetIndentDetailList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetIndentListByStoreId(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddIndent(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject GetIndentListForDashBoard(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateIndentOnlyForFreeze(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateIndent(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateIndentForChangeAndApprove(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CloseIndentFromDashBoard(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject ClosePurchaseRequisitionFromDashBoard(IValueObject valueObject, clsUserVO UserVo);  // x-x-x-x-x-x-x-x Function for Bulk PR Close

        public abstract IValueObject UpdateIndentRemarkandCancelIndent(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateIndentRemarkandRejectIndent(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject ApproveDirect(IValueObject valueObject, clsUserVO UserVo); //***//


    }
}
