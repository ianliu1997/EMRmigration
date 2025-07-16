using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
namespace PalashDynamics.BusinessLayer.Administration
{
    internal class clsGetTariffServiceBizAction:BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetTariffServiceBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>



        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetTariffServiceBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffServiceBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetTariffServiceBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseServiceMasterDAL objBaseDAL = clsBaseServiceMasterDAL.GetInstance();

                    obj = (clsGetTariffServiceBizActionVO)objBaseDAL.GetTariffService(obj, objUserVO);





                }

            }
            catch (HmsApplicationException HEx)
            {

            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {

            }
            return obj;

        }
    }


    //Added By Manisha
    internal class clsGetTariffServiceListBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetTariffServiceListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>



        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetTariffServiceListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffServiceListBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetTariffServiceListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseServiceMasterDAL objBaseDAL = clsBaseServiceMasterDAL.GetInstance();

                    obj = (clsGetTariffServiceListBizActionVO)objBaseDAL.GetTariffServiceList(obj, objUserVO);





                }

            }
            catch (HmsApplicationException HEx)
            {

            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {

            }
            return obj;

        }
    }


    internal class clsGetTariffServiceListBizActionForPathologyBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetTariffServiceListBizActionForPathologyBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>



        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetTariffServiceListBizActionForPathologyBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffServiceListBizActionForPathologyVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetTariffServiceListBizActionForPathologyVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseServiceMasterDAL objBaseDAL = clsBaseServiceMasterDAL.GetInstance();

                    obj = (clsGetTariffServiceListBizActionForPathologyVO)objBaseDAL.GetTariffServiceListForPathology(obj, objUserVO);





                }

            }
            catch (HmsApplicationException HEx)
            {

            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {

            }
            return obj;

        }
    }


    //Added By Changdeo Sase

    internal class clsGetAdmissionTypeTariffServiceListBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetAdmissionTypeTariffServiceListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetAdmissionTypeTariffServiceListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetAdmissionTypeTariffServiceListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetAdmissionTypeTariffServiceListBizActionVO)valueObject;               
                if (obj != null)
                {
                    clsBaseServiceMasterDAL objBaseDAL = clsBaseServiceMasterDAL.GetInstance();
                    obj = (clsGetAdmissionTypeTariffServiceListBizActionVO)objBaseDAL.GetAdmissionTypeTariffServiceList(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return obj;
        }
    }

    //End Block 

    //Added By Pallavi
    internal class clsGetTariffServiceMasterListBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetTariffServiceMasterListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>



        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetTariffServiceMasterListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffServiceMasterListBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetTariffServiceMasterListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseServiceMasterDAL objBaseDAL = clsBaseServiceMasterDAL.GetInstance();

                    obj = (clsGetTariffServiceMasterListBizActionVO)objBaseDAL.GetTariffServiceMasterList(obj, objUserVO);





                }

            }
            catch (HmsApplicationException HEx)
            {

            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {

            }
            return obj;

        }
    }
}
