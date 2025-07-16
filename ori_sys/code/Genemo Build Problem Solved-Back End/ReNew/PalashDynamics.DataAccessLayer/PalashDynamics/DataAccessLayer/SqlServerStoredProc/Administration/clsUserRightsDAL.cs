namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Administration
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration.UserRights;
    using System;
    using System.Data;
    using System.Data.Common;

    public class clsUserRightsDAL : clsBaseUserRightsDAL
    {
        private Database dbServer;

        public clsUserRightsDAL()
        {
            try
            {
                if (this.dbServer == null)
                {
                    this.dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject AddCreditLimit(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsSetCreditLimitBizActionVO nvo = valueObject as clsSetCreditLimitBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_SetCreditLimit");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.objUserRight.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.objUserRight.UserID);
                this.dbServer.AddInParameter(storedProcCommand, "IsIpd", DbType.Boolean, nvo.objUserRight.IsIpd);
                this.dbServer.AddInParameter(storedProcCommand, "IsOpd", DbType.Boolean, nvo.objUserRight.IsOpd);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "IpdAuthLvlID", DbType.Int64, nvo.objUserRight.IpdAuthLvl);
                this.dbServer.AddInParameter(storedProcCommand, "OpdAuthLvlID", DbType.Int64, nvo.objUserRight.OpdAuthLvl);
                this.dbServer.AddInParameter(storedProcCommand, "IpdBillingPercentage", DbType.Decimal, nvo.objUserRight.IpdBillingPercentage);
                this.dbServer.AddInParameter(storedProcCommand, "IpdSettlePercentage", DbType.Decimal, nvo.objUserRight.IpdSettlePercentage);
                this.dbServer.AddInParameter(storedProcCommand, "IpdBillingAmmount", DbType.Decimal, nvo.objUserRight.IpdBillingAmmount);
                this.dbServer.AddInParameter(storedProcCommand, "IpdSettleAmmount", DbType.Decimal, nvo.objUserRight.IpdSettleAmmount);
                this.dbServer.AddInParameter(storedProcCommand, "IpdBillAuthLvlID", DbType.Int64, nvo.objUserRight.IpdBillAuthLvlID);
                this.dbServer.AddInParameter(storedProcCommand, "OpdBillingPercentage", DbType.Decimal, nvo.objUserRight.OpdBillingPercentage);
                this.dbServer.AddInParameter(storedProcCommand, "OpdSettlePercentage", DbType.Decimal, nvo.objUserRight.OpdSettlePercentage);
                this.dbServer.AddInParameter(storedProcCommand, "OpdBillingAmmount", DbType.Decimal, nvo.objUserRight.OpdBillingAmmount);
                this.dbServer.AddInParameter(storedProcCommand, "OpdSettleAmmount", DbType.Decimal, nvo.objUserRight.OpdSettleAmmount);
                this.dbServer.AddInParameter(storedProcCommand, "OpdBillAuthLvlID", DbType.Int64, nvo.objUserRight.OpdBillAuthLvlID);
                this.dbServer.AddInParameter(storedProcCommand, "IsCrossAppointment", DbType.Boolean, nvo.objUserRight.IsCrossAppointment);
                this.dbServer.AddInParameter(storedProcCommand, "IsDailyCollection", DbType.Boolean, nvo.objUserRight.IsDailyCollection);
                this.dbServer.AddInParameter(storedProcCommand, "IsDirectIndent", DbType.Boolean, nvo.objUserRight.IsDirectIndent);
                this.dbServer.AddInParameter(storedProcCommand, "IsInterClinicIndent", DbType.Boolean, nvo.objUserRight.IsInterClinicIndent);
                this.dbServer.AddInParameter(storedProcCommand, "IsDirectPurchase", DbType.Boolean, nvo.objUserRight.IsDirectPurchase);
                this.dbServer.AddInParameter(storedProcCommand, "MaxPurchaseAmtPerTrans", DbType.Decimal, nvo.objUserRight.MaxPurchaseAmtPerTrans);
                this.dbServer.AddInParameter(storedProcCommand, "FrequencyPerMonth", DbType.Int64, nvo.objUserRight.FrequencyPerMonth);
                this.dbServer.AddInParameter(storedProcCommand, "IsCentralPurchase", DbType.Boolean, nvo.objUserRight.IsCentarlPurchase);
                this.dbServer.AddInParameter(storedProcCommand, "POApprovalLvlID", DbType.Int64, nvo.objUserRight.POApprovalLvlID);
                this.dbServer.AddInParameter(storedProcCommand, "IsMRPAdjustmentAuth", DbType.Int64, nvo.objUserRight.IsMRPAdjustmentAuth);
                this.dbServer.AddInParameter(storedProcCommand, "MRPAdjustmentAuthLvlID", DbType.Int64, nvo.objUserRight.MRPAdjustmentAuthLvlID);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "Isfinalized", DbType.Boolean, nvo.objUserRight.Isfinalized);
                this.dbServer.AddInParameter(storedProcCommand, "IsEditAfterFinalized", DbType.Boolean, nvo.objUserRight.IsEditAfterFinalized);
                this.dbServer.AddInParameter(storedProcCommand, "PatientAdvRefundAmmount", DbType.Decimal, nvo.objUserRight.PatientAdvRefundAmmount);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyAdvRefundAmmount", DbType.Decimal, nvo.objUserRight.CompanyAdvRefundAmmount);
                this.dbServer.AddInParameter(storedProcCommand, "PatientAdvRefundAuthLvlID", DbType.Int64, nvo.objUserRight.PatientAdvRefundAuthLvlID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyAdvRefundAuthLvlID", DbType.Int64, nvo.objUserRight.CompanyAdvRefundAuthLvlID);
                this.dbServer.AddInParameter(storedProcCommand, "IsRefundSerAfterSampleCollection", DbType.Boolean, nvo.objUserRight.IsRefundSerAfterSampleCollection);
                this.dbServer.AddInParameter(storedProcCommand, "IsRCEditOnFreeze", DbType.Boolean, nvo.objUserRight.IsRCEditOnFreeze);
                int num = this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.objUserRight.ResultStatus = num;
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetUserRights(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUserRightsBizActionVO nvo = valueObject as clsGetUserRightsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUserRights");
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.objUserRight.UserID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsUserRightsVO svo = new clsUserRightsVO {
                            IsIpd = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsIpd"])),
                            IpdAuthLvl = Convert.ToInt64(DALHelper.HandleDBNull(reader["IpdAuthLvlID"])),
                            IsOpd = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOpd"])),
                            OpdAuthLvl = Convert.ToInt64(DALHelper.HandleDBNull(reader["OpdAuthLvlID"])),
                            IpdBillingPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IpdBillingPercentage"])),
                            IpdSettlePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IpdSettlePercentage"])),
                            IpdBillingAmmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IpdBillingAmmount"])),
                            IpdSettleAmmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IpdSettleAmmount"])),
                            OpdBillingPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["OpdBillingPercentage"])),
                            OpdSettlePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["OpdSettlePercentage"])),
                            OpdBillingAmmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["OpdBillingAmmount"])),
                            OpdSettleAmmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["OpdSettleAmmount"])),
                            IpdBillAuthLvlID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IPDAuthLvtForConcessionID"])),
                            IsCrossAppointment = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCrossAppointment"])),
                            IsDailyCollection = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDailyCollectionReport"])),
                            IsDirectIndent = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDirectIndent"])),
                            IsInterClinicIndent = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsInterClinicIndent"])),
                            OPDAuthLvtForConcessionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OPDAuthLvtForConcessionID"])),
                            AuthLevelForRefundOPD = Convert.ToString(DALHelper.HandleBoolDBNull(reader["AuthLevelForRefundOPD"])),
                            AuthLevelForConcenssionOPD = Convert.ToString(DALHelper.HandleBoolDBNull(reader["AuthLevelForConcenssionOPD"])),
                            AuthLevelForRefundIPD = Convert.ToString(DALHelper.HandleBoolDBNull(reader["AuthLevelForRefundIPD"])),
                            AuthLevelForConcenssionIPD = Convert.ToString(DALHelper.HandleBoolDBNull(reader["AuthLevelForConcenssionIPD"])),
                            IsDirectPurchase = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsDirectPurchase"])),
                            MaxPurchaseAmtPerTrans = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MaxPurchaseAmtPerTrans"])),
                            FrequencyPerMonth = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrequencyPerMonth"])),
                            IsCentarlPurchase = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsCentralPurchase"])),
                            POApprovalLvlID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POApprovalLvlID"])),
                            POApprovalLvl = Convert.ToString(DALHelper.HandleDBNull(reader["POApprovalLvl"])),
                            IsMRPAdjustmentAuth = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsMRPAdjustmentAuth"])),
                            MRPAdjustmentAuthLvlID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MRPAdjustmentAuthLvlID"])),
                            Isfinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfinalized"])),
                            IsEditAfterFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEditAfterFinalized"])),
                            PatientAdvRefundAmmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PatientAdvRefundAmount"])),
                            PatientAdvRefundAuthLvlID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientAdvAuthLvtForRefundID"])),
                            IsRefundSerAfterSampleCollection = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefundSerAfterSampleCollection"])),
                            IsRCEditOnFreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRCEditOnFreeze"]))
                        };
                        nvo.objUserRight = svo;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GRNCountWithRightsAndFrequency(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUserGRNCountWithRightsAndFrequencyBizActionVO nvo = valueObject as clsGetUserGRNCountWithRightsAndFrequencyBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("Get_GRNCountWithRightsAndFrequency");
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.objUserRight.UserID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsUserRightsVO svo = new clsUserRightsVO {
                            UserGRNCountForMonth = Convert.ToInt64(DALHelper.HandleDBNull(reader["UserGRNCountForMonth"])),
                            IsDirectPurchase = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsDirectPurchase"])),
                            MaxPurchaseAmtPerTrans = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MaxPurchaseAmtPerTrans"])),
                            FrequencyPerMonth = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrequencyPerMonth"]))
                        };
                        nvo.objUserRight = svo;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

