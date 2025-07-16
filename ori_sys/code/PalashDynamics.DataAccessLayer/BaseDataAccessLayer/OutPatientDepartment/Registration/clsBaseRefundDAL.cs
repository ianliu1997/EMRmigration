using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;

  namespace PalashDynamics.DataAccessLayer
    {
        public abstract class clsBaseRefundDAL
        {

            static private clsBaseRefundDAL _instance = null;

            /// <summary>
            /// Returns an instance of the provider type specified in the config file
            /// </summary>
            public static clsBaseRefundDAL GetInstance()
            {
                try
                {
                    if (_instance == null)
                    {
                        //Get the full name of data access layer class from xml file which stores the list of classess.
                        string _DerivedClassName = "clsRefundDAL";
                        string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                        //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                        _instance = (clsBaseRefundDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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
            public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo);
            public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);

            //public abstract IValueObject Get(IValueObject valueObject, clsUserVO UserVo);

           // public abstract IValueObject Update(IValueObject valueObject, clsUserVO UserVo);

            public abstract IValueObject GetList(IValueObject valueObject, clsUserVO UserVo);

            public abstract IValueObject Delete(IValueObject valueObject, clsUserVO UserVo);

            public abstract IValueObject SendApprovalRequest(IValueObject valueObject, clsUserVO UserVo);

            public abstract IValueObject ApproveRefundRequest(IValueObject valueObject, clsUserVO UserVo);

            public abstract IValueObject SendApprovalRequestForBill(IValueObject valueObject, clsUserVO UserVo);

            public abstract IValueObject ApproveConcessionRequest(IValueObject valueObject, clsUserVO UserVo);

            public abstract IValueObject DeleteApprovalRequest(IValueObject valueObject, clsUserVO UserVo);

            public abstract IValueObject SendApprovalRequestForAdvanceRefundDetails(IValueObject valueObject, clsUserVO UserVo);

            public abstract IValueObject ApproveAdvanceRefundRequestDetails(IValueObject valueObject, clsUserVO UserVo);

            public abstract IValueObject GetRefundReceiptList(IValueObject valueObject, clsUserVO UserVo); //Added by AniketK on 30-Jan-2019
            
        }
    }

