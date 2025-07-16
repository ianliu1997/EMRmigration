using System;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.PackageNew;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using com.seedhealthcare.hms.CustomExceptions;

namespace PalashDynamics.BusinessLayer.Administration.PackageNew
{
    internal class clsAddPackageSourceTariffCompanyRelationsBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsAddPackageSourceTariffCompanyRelationsBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsAddPackageSourceTariffCompanyRelationsBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddPackageSourceTariffCompanyRelationsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddPackageSourceTariffCompanyRelationsBizActionVO obj = null;
            try
            {
                obj = (clsAddPackageSourceTariffCompanyRelationsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();
                    obj = (clsAddPackageSourceTariffCompanyRelationsBizActionVO)objBaseDAL.AddPackageSourceTariffCompanyLinking(obj, objUserVO);
                }

            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            finally
            {
            }
            return obj;
        }
    }

    internal class clsGetPackageSourceTariffCompanyListBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsGetPackageSourceTariffCompanyListBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsGetPackageSourceTariffCompanyListBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPackageSourceTariffCompanyListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPackageSourceTariffCompanyListBizActionVO obj = null;
            try
            {
                obj = (clsGetPackageSourceTariffCompanyListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();
                    obj = (clsGetPackageSourceTariffCompanyListBizActionVO)objBaseDAL.GetPackageSourceTariffCompanyLinking(obj, objUserVO);
                }

            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            finally
            {
            }
            return obj;
        }
    }

    internal class clsAddPackageRateClinicWiseBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsAddPackageRateClinicWiseBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsAddPackageRateClinicWiseBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddPackageRateClinicWiseBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddPackageRateClinicWiseBizActionVO obj = null;
            try
            {
                obj = (clsAddPackageRateClinicWiseBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();
                    obj = (clsAddPackageRateClinicWiseBizActionVO)objBaseDAL.AddUpdatePackageRateClinicWise(obj, objUserVO);
                }

            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            finally
            {
            }
            return obj;
        }
    }

    internal class clsGetPackageRateClinicWiseBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsGetPackageRateClinicWiseBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsGetPackageRateClinicWiseBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPackageRateClinicWiseBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPackageRateClinicWiseBizActionVO obj = null;
            try
            {
                obj = (clsGetPackageRateClinicWiseBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();
                    obj = (clsGetPackageRateClinicWiseBizActionVO)objBaseDAL.GetPackageRateClinicWise(obj, objUserVO);
                }

            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            finally
            {
            }
            return obj;
        }
    }

    //internal class clsGetPackageSourceTariffCompanyListBizAction : BizAction
    //{


    //}

    //internal class clsAddPackageCompanyTariffBizAction : BizAction
    //{
    //    //This Region Contains Variables Which are Used At Form Level
    //    #region Variables Declaration
    //    //Declare the LogManager object
    //    LogManager logManager = null;
    //    //Declare the BaseOPDPatientMasterDAL object
    //    //Declare the Variable of UserId
    //    long lngUserId = 0;
    //    #endregion

    //    //constructor For Log Error Info
    //    private clsAddPackageCompanyTariffBizAction()
    //    {
    //        //Create Instance of the LogManager object 
    //        #region Logging Code
    //        if (logManager == null)
    //        {
    //            logManager = LogManager.GetInstance();
    //        }
    //        #endregion
    //    }

    //    //The Private declaration
    //    private static clsAddPackageCompanyTariffBizAction _Instance = null;

    //    ///Method Input OPDPatient: none
    //    ///Name:GetInstance       
    //    ///Type:static
    //    ///Direction:None
    //    ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
    //    public static BizAction GetInstance()
    //    {
    //        if (_Instance == null)
    //            _Instance = new clsAddPackageCompanyTariffBizAction();

    //        return _Instance;
    //    }

    //    protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
    //    {
    //        bool CurrentMethodExecutionStatus = true;
    //        clsAddPackageCompanyTariffBizActionVO obj = null;
    //        try
    //        {
    //            obj = (clsAddPackageCompanyTariffBizActionVO)valueObject;
    //            if (obj != null)
    //            {
    //                clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();
    //                obj = (clsAddPackageCompanyTariffBizActionVO)objBaseDAL.AddPackageServiceMaster(obj, objUserVO);
    //            }

    //        }
    //        catch (HmsApplicationException HEx)
    //        {
    //            CurrentMethodExecutionStatus = false;
    //        }
    //        catch (Exception ex)
    //        {
    //            CurrentMethodExecutionStatus = false;
    //            throw;
    //        }
    //        finally
    //        {
    //        }
    //        return obj;
    //    }
    //}
}
