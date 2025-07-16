using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master.CompanyPayment;
using PalashDynamics.DataAccessLayer;

namespace PalashDynamics.BusinessLayer.Master
{
    internal class clsGetCompanyInvoiceDetailsBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private clsGetCompanyInvoiceDetailsBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

        }
        private static clsGetCompanyInvoiceDetailsBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetCompanyInvoiceDetailsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            //clsGetDoctorShareDetailsBizActionVO
            clsGetCompanyInvoiceDetailsBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetCompanyInvoiceDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseCompanyPaymentDAL objBaseCompanyDAL = clsBaseCompanyPaymentDAL.GetInstance();
                    obj = (clsGetCompanyInvoiceDetailsBizActionVO)objBaseCompanyDAL.GetCompanyInvoice(obj, objUserVO);
                }
            }
            catch (Exception)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;

        }



    }

    internal class clsGetCompanyPaymentDetailsBizAction : BizAction
    {


        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private clsGetCompanyPaymentDetailsBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

        }
        private static clsGetCompanyPaymentDetailsBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetCompanyPaymentDetailsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetCompanyPaymentDetailsBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetCompanyPaymentDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseCompanyPaymentDAL objBaseCompanyDAL = clsBaseCompanyPaymentDAL.GetInstance();
                    obj = (clsGetCompanyPaymentDetailsBizActionVO)objBaseCompanyDAL.GetCompanyPaymentDetail(obj, objUserVO);
                }
            }
            catch (Exception)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;

        }



    }


    internal class clsGetCompanyInvoiceForModify : BizAction
    {


        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private clsGetCompanyInvoiceForModify()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

        }
        private static clsGetCompanyInvoiceForModify _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetCompanyInvoiceForModify();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetCompanyInvoiceForModifyVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetCompanyInvoiceForModifyVO)valueObject;
                if (obj != null)
                {
                    clsBaseCompanyPaymentDAL objBaseCompanyDAL = clsBaseCompanyPaymentDAL.GetInstance();
                    obj = (clsGetCompanyInvoiceForModifyVO)objBaseCompanyDAL.GetCompanyInvoiceDetail(obj, objUserVO);
                }
            }
            catch (Exception)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;

        }



    }


    internal class clsAddCompanyInvoiceBizAction : BizAction
    {


        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private clsAddCompanyInvoiceBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

        }
        private static clsAddCompanyInvoiceBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddCompanyInvoiceBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            //clsGetDoctorShareDetailsBizActionVO
            clsAddCompanyInvoiceBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddCompanyInvoiceBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseCompanyPaymentDAL objBaseCompanyDAL = clsBaseCompanyPaymentDAL.GetInstance();
                    obj = (clsAddCompanyInvoiceBizActionVO)objBaseCompanyDAL.AddInvoice(obj, objUserVO);
                }
            }
            catch (Exception)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;

        }



    }
    internal class clsGetBillAgainstInvoiceBizAction : BizAction
    {


        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private clsGetBillAgainstInvoiceBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

        }
        private static clsGetBillAgainstInvoiceBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetBillAgainstInvoiceBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            //clsGetDoctorShareDetailsBizActionVO
            clsGetBillAgainstInvoiceBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetBillAgainstInvoiceBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseCompanyPaymentDAL objBaseCompanyDAL = clsBaseCompanyPaymentDAL.GetInstance();
                    obj = (clsGetBillAgainstInvoiceBizActionVO)objBaseCompanyDAL.GetInvoiceSearchList(obj, objUserVO);
                }
            }
            catch (Exception)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;

        }



    }


    internal class clsModifyCompanyInvoiceBizAction : BizAction
    {


        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private clsModifyCompanyInvoiceBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

        }
        private static clsModifyCompanyInvoiceBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsModifyCompanyInvoiceBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            //clsGetDoctorShareDetailsBizActionVO
            clsModifyCompanyInvoiceBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsModifyCompanyInvoiceBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseCompanyPaymentDAL objBaseCompanyDAL = clsBaseCompanyPaymentDAL.GetInstance();
                    obj = (clsModifyCompanyInvoiceBizActionVO)objBaseCompanyDAL.ModifyInvoice(obj, objUserVO);
                }
            }
            catch (Exception)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;

        }



    }


    internal class clsDeleteCompanyInvoiceBillBizAction : BizAction
    {


        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private clsDeleteCompanyInvoiceBillBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

        }
        private static clsDeleteCompanyInvoiceBillBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsDeleteCompanyInvoiceBillBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            //clsGetDoctorShareDetailsBizActionVO
            clsDeleteCompanyInvoiceBillBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsDeleteCompanyInvoiceBillBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseCompanyPaymentDAL objBaseCompanyDAL = clsBaseCompanyPaymentDAL.GetInstance();
                    obj = (clsDeleteCompanyInvoiceBillBizActionVO)objBaseCompanyDAL.DeleteInvoiceBill(obj, objUserVO);
                }
            }
            catch (Exception)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;

        }



    }


}
