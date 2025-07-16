using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.Administration.UserRights;
using PalashDynamics.ValueObjects;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Administration
{
    public class clsUserRightsDAL : clsBaseUserRightsDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        #endregion

        public clsUserRightsDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //public override IValueObject GetAppConfig1(IValueObject BizActionObj, clsUserVO objUserVO)
        //{
        //    return BizActionObj;
        //}


        public override IValueObject AddCreditLimit(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsSetCreditLimitBizActionVO BizActionObj = valueObject as clsSetCreditLimitBizActionVO;
            try
            {
                //clsUserRightsVO ObjUserRightsVO = valueObject as clsUserRightsVO;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_SetCreditLimit");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.objUserRight.UnitID);
                dbServer.AddInParameter(command, "UserID", DbType.Int64, BizActionObj.objUserRight.UserID);
                
                dbServer.AddInParameter(command, "IsIpd", DbType.Boolean, BizActionObj.objUserRight.IsIpd);
                dbServer.AddInParameter(command, "IsOpd", DbType.Boolean, BizActionObj.objUserRight.IsOpd);

                //dbServer.AddInParameter(command, "CreditLimit", DbType.Int64, BizActionObj.objUserRight.CreditLimit);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                
                dbServer.AddInParameter(command, "IpdAuthLvlID", DbType.Int64, BizActionObj.objUserRight.IpdAuthLvl);
                dbServer.AddInParameter(command, "OpdAuthLvlID", DbType.Int64, BizActionObj.objUserRight.OpdAuthLvl);
                dbServer.AddInParameter(command, "IpdBillingPercentage", DbType.Decimal, BizActionObj.objUserRight.IpdBillingPercentage);
                dbServer.AddInParameter(command, "IpdSettlePercentage", DbType.Decimal, BizActionObj.objUserRight.IpdSettlePercentage);
                dbServer.AddInParameter(command, "IpdBillingAmmount", DbType.Decimal, BizActionObj.objUserRight.IpdBillingAmmount);
                dbServer.AddInParameter(command, "IpdSettleAmmount", DbType.Decimal, BizActionObj.objUserRight.IpdSettleAmmount);
                dbServer.AddInParameter(command, "IpdBillAuthLvlID", DbType.Int64, BizActionObj.objUserRight.IpdBillAuthLvlID);
                dbServer.AddInParameter(command, "OpdBillingPercentage", DbType.Decimal, BizActionObj.objUserRight.OpdBillingPercentage);
                dbServer.AddInParameter(command, "OpdSettlePercentage", DbType.Decimal, BizActionObj.objUserRight.OpdSettlePercentage);
                dbServer.AddInParameter(command, "OpdBillingAmmount", DbType.Decimal, BizActionObj.objUserRight.OpdBillingAmmount);
                dbServer.AddInParameter(command, "OpdSettleAmmount", DbType.Decimal, BizActionObj.objUserRight.OpdSettleAmmount);
                dbServer.AddInParameter(command, "OpdBillAuthLvlID", DbType.Int64, BizActionObj.objUserRight.OpdBillAuthLvlID);
                dbServer.AddInParameter(command, "IsCrossAppointment", DbType.Boolean, BizActionObj.objUserRight.IsCrossAppointment);
                dbServer.AddInParameter(command, "IsDailyCollection", DbType.Boolean, BizActionObj.objUserRight.IsDailyCollection); //***//

                dbServer.AddInParameter(command, "IsDirectIndent", DbType.Boolean, BizActionObj.objUserRight.IsDirectIndent); //***//
                dbServer.AddInParameter(command, "IsInterClinicIndent", DbType.Boolean, BizActionObj.objUserRight.IsInterClinicIndent); //***//
                dbServer.AddInParameter(command, "IsDirectPurchase", DbType.Boolean, BizActionObj.objUserRight.IsDirectPurchase);
                dbServer.AddInParameter(command, "MaxPurchaseAmtPerTrans", DbType.Decimal, BizActionObj.objUserRight.MaxPurchaseAmtPerTrans);
                dbServer.AddInParameter(command, "FrequencyPerMonth", DbType.Int64, BizActionObj.objUserRight.FrequencyPerMonth);
                dbServer.AddInParameter(command, "IsCentralPurchase", DbType.Boolean, BizActionObj.objUserRight.IsCentarlPurchase); // added by Ashish Z.
                dbServer.AddInParameter(command, "POApprovalLvlID", DbType.Int64, BizActionObj.objUserRight.POApprovalLvlID); // added by Ashish Z. on 04042016
                dbServer.AddInParameter(command, "IsMRPAdjustmentAuth", DbType.Int64, BizActionObj.objUserRight.IsMRPAdjustmentAuth); 
                dbServer.AddInParameter(command, "MRPAdjustmentAuthLvlID", DbType.Int64, BizActionObj.objUserRight.MRPAdjustmentAuthLvlID); 
                dbServer.AddParameter(command,   "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, int.MaxValue);
                dbServer.AddInParameter(command, "Isfinalized", DbType.Boolean, BizActionObj.objUserRight.Isfinalized); // added by Yogesh K. 17 may 16
                dbServer.AddInParameter(command, "IsEditAfterFinalized", DbType.Boolean, BizActionObj.objUserRight.IsEditAfterFinalized); // added by Rohinee 10 01 2017

                dbServer.AddInParameter(command, "PatientAdvRefundAmmount", DbType.Decimal, BizActionObj.objUserRight.PatientAdvRefundAmmount); // added by Bhushan 30052017
                dbServer.AddInParameter(command, "CompanyAdvRefundAmmount", DbType.Decimal, BizActionObj.objUserRight.CompanyAdvRefundAmmount); // added by Bhushan 30052017
                dbServer.AddInParameter(command, "PatientAdvRefundAuthLvlID", DbType.Int64, BizActionObj.objUserRight.PatientAdvRefundAuthLvlID); // added by Bhushan 30052017
                dbServer.AddInParameter(command, "CompanyAdvRefundAuthLvlID", DbType.Int64, BizActionObj.objUserRight.CompanyAdvRefundAuthLvlID); // added by Bhushan 30052017
                dbServer.AddInParameter(command, "IsRefundSerAfterSampleCollection", DbType.Boolean, BizActionObj.objUserRight.IsRefundSerAfterSampleCollection);


                dbServer.AddInParameter(command, "IsRCEditOnFreeze", DbType.Boolean, BizActionObj.objUserRight.IsRCEditOnFreeze); // added by Prashant Channe 16/10/2018

                int intStatus2 = dbServer.ExecuteNonQuery(command);
                BizActionObj.objUserRight.ResultStatus = intStatus2;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }

            return BizActionObj;
        }


        public override IValueObject GetUserRights(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUserRightsBizActionVO BizActionObj = valueObject as clsGetUserRightsBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetUserRights");               
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "UserID", DbType.Int64, BizActionObj.objUserRight.UserID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                   // if (BizActionObj.objListUserRights == null)
                       // BizActionObj.objListUserRights = new List<clsUserRightsVO>();
                    while (reader.Read())
                    {
                        clsUserRightsVO UserVO = new clsUserRightsVO();
                        UserVO.IsIpd = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsIpd"]));
                        UserVO.IpdAuthLvl = Convert.ToInt64(DALHelper.HandleDBNull(reader["IpdAuthLvlID"]));
                        UserVO.IsOpd = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOpd"]));
                        UserVO.OpdAuthLvl = Convert.ToInt64(DALHelper.HandleDBNull(reader["OpdAuthLvlID"]));
                        UserVO.IpdBillingPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IpdBillingPercentage"]));
                        UserVO.IpdSettlePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IpdSettlePercentage"]));
                        UserVO.IpdBillingAmmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IpdBillingAmmount"]));
                        UserVO.IpdSettleAmmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IpdSettleAmmount"]));
                        UserVO.OpdBillingPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["OpdBillingPercentage"]));
                        UserVO.OpdSettlePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["OpdSettlePercentage"]));
                        UserVO.OpdBillingAmmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["OpdBillingAmmount"]));
                        UserVO.OpdSettleAmmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["OpdSettleAmmount"]));
                        UserVO.IpdBillAuthLvlID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IPDAuthLvtForConcessionID"]));
                        UserVO.IsCrossAppointment = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCrossAppointment"]));

                        UserVO.IsDailyCollection = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDailyCollectionReport"])); //***//
                        UserVO.IsDirectIndent = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDirectIndent"])); //***// 
                        UserVO.IsInterClinicIndent = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsInterClinicIndent"])); //***//
                        
                        //By Anjali.................................

                        UserVO.OPDAuthLvtForConcessionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OPDAuthLvtForConcessionID"]));
                        UserVO.AuthLevelForRefundOPD = Convert.ToString(DALHelper.HandleBoolDBNull(reader["AuthLevelForRefundOPD"]));
                        UserVO.AuthLevelForConcenssionOPD = Convert.ToString(DALHelper.HandleBoolDBNull(reader["AuthLevelForConcenssionOPD"]));
                        UserVO.AuthLevelForRefundIPD = Convert.ToString(DALHelper.HandleBoolDBNull(reader["AuthLevelForRefundIPD"]));
                        UserVO.AuthLevelForConcenssionIPD = Convert.ToString(DALHelper.HandleBoolDBNull(reader["AuthLevelForConcenssionIPD"]));
                        //................................................

                        //  UserVO.IsActive = (bool)(DALHelper.HandleDBNull(reader["Status"]));
                        // Added By CDS
                        UserVO.IsDirectPurchase = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsDirectPurchase"]));
                        UserVO.MaxPurchaseAmtPerTrans = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MaxPurchaseAmtPerTrans"]));
                        UserVO.FrequencyPerMonth = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrequencyPerMonth"]));
                        UserVO.IsCentarlPurchase = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsCentralPurchase"]));
                        UserVO.POApprovalLvlID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POApprovalLvlID"]));
                        UserVO.POApprovalLvl = Convert.ToString(DALHelper.HandleDBNull(reader["POApprovalLvl"]));
                        UserVO.IsMRPAdjustmentAuth = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsMRPAdjustmentAuth"]));
                        UserVO.MRPAdjustmentAuthLvlID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MRPAdjustmentAuthLvlID"]));
                        UserVO.Isfinalized=Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfinalized"])); //Added By Yogesh K 17 5 16
                        UserVO.IsEditAfterFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEditAfterFinalized"])); //Added By Rohinee 10 01 17

                        UserVO.PatientAdvRefundAmmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PatientAdvRefundAmount"])); //Added By bhushanp 31052017
                        UserVO.PatientAdvRefundAuthLvlID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientAdvAuthLvtForRefundID"])); //Added By bhushanp 31052017
                        UserVO.IsRefundSerAfterSampleCollection = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefundSerAfterSampleCollection"]));


                        UserVO.IsRCEditOnFreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRCEditOnFreeze"]));    //Added by Prashant Channe 16/10/2018

                        BizActionObj.objUserRight = UserVO;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }

            return BizActionObj;
        }

        // Added By CDS 04/01/2016

        public override IValueObject GRNCountWithRightsAndFrequency(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUserGRNCountWithRightsAndFrequencyBizActionVO BizActionObj = valueObject as clsGetUserGRNCountWithRightsAndFrequencyBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("Get_GRNCountWithRightsAndFrequency");

                dbServer.AddInParameter(command, "UserID", DbType.Int64, BizActionObj.objUserRight.UserID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);                

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    // if (BizActionObj.objListUserRights == null)
                    // BizActionObj.objListUserRights = new List<clsUserRightsVO>();
                    while (reader.Read())
                    {
                        clsUserRightsVO UserVO = new clsUserRightsVO();

                        //UserVO.IsIpd = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsIpd"]));
                        //UserVO.IpdAuthLvl = Convert.ToInt64(DALHelper.HandleDBNull(reader["IpdAuthLvlID"]));
                        //UserVO.IsOpd = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOpd"]));
                        //UserVO.OpdAuthLvl = Convert.ToInt64(DALHelper.HandleDBNull(reader["OpdAuthLvlID"]));
                        //UserVO.IpdBillingPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IpdBillingPercentage"]));
                        //UserVO.IpdSettlePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IpdSettlePercentage"]));
                        //UserVO.IpdBillingAmmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IpdBillingAmmount"]));
                        //UserVO.IpdSettleAmmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IpdSettleAmmount"]));
                        //UserVO.OpdBillingPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["OpdBillingPercentage"]));
                        //UserVO.OpdSettlePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["OpdSettlePercentage"]));
                        //UserVO.OpdBillingAmmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["OpdBillingAmmount"]));
                        //UserVO.OpdSettleAmmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["OpdSettleAmmount"]));
                        //UserVO.IpdBillAuthLvlID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IPDAuthLvtForConcessionID"]));
                        //UserVO.IsCrossAppointment = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCrossAppointment"]));
                        ////By Anjali.................................
                        //UserVO.OPDAuthLvtForConcessionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OPDAuthLvtForConcessionID"]));
                        //UserVO.AuthLevelForRefundOPD = Convert.ToString(DALHelper.HandleBoolDBNull(reader["AuthLevelForRefundOPD"]));
                        //UserVO.AuthLevelForConcenssionOPD = Convert.ToString(DALHelper.HandleBoolDBNull(reader["AuthLevelForConcenssionOPD"]));
                        //UserVO.AuthLevelForRefundIPD = Convert.ToString(DALHelper.HandleBoolDBNull(reader["AuthLevelForRefundIPD"]));
                        //UserVO.AuthLevelForConcenssionIPD = Convert.ToString(DALHelper.HandleBoolDBNull(reader["AuthLevelForConcenssionIPD"]));
                        //................................................
                        //  UserVO.IsActive = (bool)(DALHelper.HandleDBNull(reader["Status"]));

                        // Added By CDS

                        UserVO.UserGRNCountForMonth = Convert.ToInt64(DALHelper.HandleDBNull(reader["UserGRNCountForMonth"]));
                        UserVO.IsDirectPurchase = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsDirectPurchase"]));
                        UserVO.MaxPurchaseAmtPerTrans = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MaxPurchaseAmtPerTrans"]));
                        UserVO.FrequencyPerMonth = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrequencyPerMonth"]));

                        //UserGRNCountForMonth	IsDirectPurchase	MaxPurchaseAmtPerTrans	FrequencyPerMonth

                        BizActionObj.objUserRight = UserVO;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }

            return BizActionObj;
        }
    }
}
