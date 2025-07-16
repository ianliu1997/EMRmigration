using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;
using PalashDynamics.ValueObjects.Master.DoctorPayment;
using PalashDynamics.ValueObjects.Administration.DoctorShareRange;

namespace PalashDynamics.BusinessLayer.Master.DocPayment
{
    internal class clsGetDoctorPaymentBizAction : BizAction
    {
        private clsGetDoctorPaymentBizAction()
        {
        }

        private static clsGetDoctorPaymentBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDoctorPaymentBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetDoctorPaymentBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetDoctorPaymentBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                    obj = (clsGetDoctorPaymentBizActionVO)objBaseDoctorDAL.GetDoctorPaymentList(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;

        }
    }

    //R
    internal class clsGetDoctorPaymentChildBizAction : BizAction
    {
        private clsGetDoctorPaymentChildBizAction()
        {
        }

        private static clsGetDoctorPaymentChildBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDoctorPaymentChildBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetDoctorPaymentChildBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetDoctorPaymentChildBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();

                    obj = (clsGetDoctorPaymentChildBizActionVO)objBaseDoctorDAL.GetDoctorPaymentChildDetailsList(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;

        }
    }


    //R
    internal class clsGetDoctorSharePaymentBizAction : BizAction
    {
        private clsGetDoctorSharePaymentBizAction()
        {
        }

        private static clsGetDoctorSharePaymentBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDoctorSharePaymentBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetDoctorPaymentShareDetailsBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetDoctorPaymentShareDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();

                    obj = (clsGetDoctorPaymentShareDetailsBizActionVO)objBaseDoctorDAL.GetDoctorPaymentDetailsList(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;

        }
    }

    //R
    internal class clsGetDoctorPaymentDetailListBizAction : BizAction
    {
        private clsGetDoctorPaymentDetailListBizAction()
        {
        }

        private static clsGetDoctorPaymentDetailListBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDoctorPaymentDetailListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetDoctorPaymentDetailListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetDoctorPaymentDetailListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                    obj = (clsGetDoctorPaymentDetailListBizActionVO)objBaseDoctorDAL.GetDoctorPaymentFrontDetailsList(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;

        }
    }

    //internal class clsGetDoctorShareRangeListBizAction : BizAction
    //{
    //    private clsGetDoctorShareRangeListBizAction()
    //    {
    //    }

    //    private static clsGetDoctorShareRangeListBizAction _Instance = null;
    //    public static BizAction GetInstance()
    //    {
    //        if (_Instance == null)
    //            _Instance = new clsGetDoctorShareRangeListBizAction();

    //        return _Instance;
    //    }

    //    protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
    //    {
    //        bool CurrentMethodExecutionStatus = true;

    //        clsGetDoctorShareRangeListBizActionVO obj = null;
    //        int ResultStatus = 0;
    //        try
    //        {
    //            obj = (clsGetDoctorShareRangeListBizActionVO)valueObject;
    //            if (obj != null)
    //            {
    //                clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
    //                obj = (clsGetDoctorShareRangeListBizActionVO)objBaseDoctorDAL.GetDoctorShareRangeList(obj, objUserVO);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            CurrentMethodExecutionStatus = false;
    //        }
    //        finally
    //        {

    //        }
    //        return obj;

    //    }
    //}

}
