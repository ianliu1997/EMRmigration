using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.DataAccessLayer;

namespace PalashDynamics.BusinessLayer.Master
{
    public class clsGetMasterListBizAction : BizAction
    {

        private static clsGetMasterListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetMasterListBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetMasterListBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                //Typecast the "valueObject" to "clsFillMasterListBizActionVO"
                obj = (clsGetMasterListBizActionVO)valueObject;
                //if (obj.MasterTable == MasterTableNameList.None)
                //    throw new ArgumentException("MasterTable Property Of clsGetMasterListBizActionVO cannot be None", "clsGetMasterListBizActionVO.MasterTable");
                ////Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    if (obj.IsFromPOGRN == true)
                    {
                        //Create the Instance of clsBasePatientDAL
                        clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                        if (obj.IsGST)
                        {
                            obj = (clsGetMasterListBizActionVO)objBaseMasterDAL.GETSupplierList(obj, objUserVO);
                        }
                        else
                        {
                            obj = (clsGetMasterListBizActionVO)objBaseMasterDAL.GetCodeMasterList(obj, objUserVO);
                        }
                    }
                    else
                    {
                        //Create the Instance of clsBasePatientDAL
                        clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                        //Now Call the Insert method of the Patient DAO
                        obj = (clsGetMasterListBizActionVO)objBaseMasterDAL.GetMasterList(obj, objUserVO);
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
                //log error  

            }

            return valueObject;

        }
    }

    public class clsGetAutoCompleteList : BizAction
    {

        private static clsGetAutoCompleteList _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetAutoCompleteList();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetAutoCompleteListVO obj = null;
            int ResultStatus = 0;

            try
            {
                //Typecast the "valueObject" to "clsFillMasterListBizActionVO"
                obj = (clsGetAutoCompleteListVO)valueObject;
               
                ////Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetAutoCompleteListVO)objBaseMasterDAL.GetAutoCompleteList(obj, objUserVO);
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
                //log error  

            }

            return valueObject;

        }
    }

    public class clsGetAutoCompleteList_2Colums : BizAction
    {

        private static clsGetAutoCompleteList_2Colums _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetAutoCompleteList_2Colums();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetAutoCompleteListVO_2Colums obj = null;
            int ResultStatus = 0;

            try
            {
                //Typecast the "valueObject" to "clsFillMasterListBizActionVO"
                obj = (clsGetAutoCompleteListVO_2Colums)valueObject;

                ////Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetAutoCompleteListVO_2Colums)objBaseMasterDAL.GetAutoCompleteList_2colums(obj, objUserVO);
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
                //log error  

            }

            return valueObject;

        }
    }


    public class clsGetSearchMasterListBizAction : BizAction
    {

        private static clsGetSearchMasterListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetSearchMasterListBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
          
            clsGetSearchMasterListBizActionVO obj = null;
           

            try
            {
                //Typecast the "valueObject" to "clsFillMasterListBizActionVO"
                obj = (clsGetSearchMasterListBizActionVO)valueObject;
                if (obj.MasterTable == MasterTableNameList.None)
                    throw new ArgumentException("MasterTable Property Of clsGetMasterListBizActionVO cannot be None", "clsGetMasterListBizActionVO.MasterTable");
                ////Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetSearchMasterListBizActionVO)objBaseMasterDAL.GetMasterSearchList(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
             
                throw;
            }
            catch (Exception ex)
            {
             
                //log error  

            }

            return valueObject;

        }
    }


    public class clsGetPathoFastingBizAction : BizAction
    {

        private static clsGetPathoFastingBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPathoFastingBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetPathoFastingBizActionVO obj = null;


            try
            {
                //Typecast the "valueObject" to "clsFillMasterListBizActionVO"
                obj = (clsGetPathoFastingBizActionVO)valueObject;               
                ////Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetPathoFastingBizActionVO)objBaseMasterDAL.GetPathoFasting(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                throw;
            }
            catch (Exception ex)
            {

                //log error  

            }

            return valueObject;

        }
    }

   //***//----------------------
    public class clsGetMarketingExecutivesListBizAction : BizAction
    {

        private static clsGetMarketingExecutivesListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetMarketingExecutivesListBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetMarketingExecutivesListVO obj = null;


            try
            {
                //Typecast the "valueObject" to "clsFillMasterListBizActionVO"
                obj = (clsGetMarketingExecutivesListVO)valueObject;
                ////Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetMarketingExecutivesListVO)objBaseMasterDAL.GetMarketingExecutivesList(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                throw;
            }
            catch (Exception ex)
            {

                //log error  

            }

            return valueObject;

        }
    }

  //***//---------------------------------
    public class clsGetMasterListByTableNameBizAction : BizAction
    {

        private static clsGetMasterListByTableNameBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetMasterListByTableNameBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetMasterListByTableNameBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                //Typecast the "valueObject" to "clsFillMasterListBizActionVO"
                obj = (clsGetMasterListByTableNameBizActionVO)valueObject;
                if (obj.MasterTable == "")
                    throw new ArgumentException("MasterTable Property Of clsGetMasterListBizActionVO cannot be None", "clsGetMasterListByTableNameBizActionVO.MasterTable");
                ////Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetMasterListByTableNameBizActionVO)objBaseMasterDAL.GetMasterListByTableName(obj, objUserVO);
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
                //log error  

            }

            return valueObject;

        }
    }

    // Added By Harish
    // Date 1 Aug 2011
    // For Dynamic get Master list from Any Master Table with Dynamic Column Names
    public class clsGetMasterListByTableNameAndColumnNameBizAction : BizAction
    {

        private static clsGetMasterListByTableNameAndColumnNameBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetMasterListByTableNameAndColumnNameBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetMasterListByTableNameAndColumnNameBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                //Typecast the "valueObject" to "clsFillMasterListBizActionVO"
                obj = (clsGetMasterListByTableNameAndColumnNameBizActionVO)valueObject;
                if (obj.MasterTable == "")
                    throw new ArgumentException("MasterTable Property Of clsGetMasterListByTableNameAndColumnNameBizActionVO cannot be None", "clsGetMasterListByTableNameAndColumnNameBizActionVO.MasterTable");
                ////Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetMasterListByTableNameAndColumnNameBizActionVO)objBaseMasterDAL.GetMasterListByTableNameAndColumnName(obj, objUserVO);
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
                //log error  

            }

            return valueObject;

        }
    }

    public class clsGetColumnListByTableNameBizAction : BizAction
    {

        private static clsGetColumnListByTableNameBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetColumnListByTableNameBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetColumnListByTableNameBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                //Typecast the "valueObject" to "clsFillMasterListBizActionVO"
                obj = (clsGetColumnListByTableNameBizActionVO)valueObject;
                if (obj.MasterTable == "")
                    throw new ArgumentException("MasterTable Property Of clsGetColumnListByTableNameBizActionVO cannot be None", "clsGetColumnListByTableNameBizActionVO.MasterTable");
                ////Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetColumnListByTableNameBizActionVO)objBaseMasterDAL.GetColumnListByTableName(obj, objUserVO);
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
                //log error  

            }

            return valueObject;

        }
    }

    public class clsGetMasterListConsentBizAction : BizAction
    {

        private static clsGetMasterListConsentBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetMasterListConsentBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetMasterListConsentBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                //Typecast the "valueObject" to "clsFillMasterListBizActionVO"
                obj = (clsGetMasterListConsentBizActionVO)valueObject;
                //if (obj.MasterTable == MasterTableNameList.None)
                //    throw new ArgumentException("MasterTable Property Of clsGetMasterListBizActionVO cannot be None", "clsGetMasterListBizActionVO.MasterTable");
                ////Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetMasterListConsentBizActionVO)objBaseMasterDAL.GetMasterListForConsent(obj, objUserVO);
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
                //log error  

            }

            return valueObject;

        }
    }

}
