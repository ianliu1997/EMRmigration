namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CRM;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.CRM;
    using PalashDynamics.ValueObjects.CRM.LoyaltyProgram;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsLoyaltyProgramDAL : clsBaseLoyaltyProgramDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsLoyaltyProgramDAL()
        {
            try
            {
                if (this.dbServer == null)
                {
                    this.dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                if (this.logManager == null)
                {
                    this.logManager = LogManager.GetInstance();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject AddLoyaltyProgram(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            clsAddLoyaltyProgramBizActionVO nvo = (clsAddLoyaltyProgramBizActionVO) valueObject;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsLoyaltyProgramVO loyaltyProgramDetails = nvo.LoyaltyProgramDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddLoyaltyProgram");
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, loyaltyProgramDetails.LoyaltyProgramName.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, loyaltyProgramDetails.EffectiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, loyaltyProgramDetails.ExpiryDate);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, loyaltyProgramDetails.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFamily", DbType.Boolean, loyaltyProgramDetails.IsFamily);
                if (loyaltyProgramDetails.Remark != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, loyaltyProgramDetails.Remark.Trim());
                }
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, loyaltyProgramDetails.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.LoyaltyProgramDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                foreach (clsLoyaltyClinicVO cvo in loyaltyProgramDetails.ClinicList)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddLoyaltyProgramClinicDetails");
                    this.dbServer.AddInParameter(command2, "LoyaltyProgramID", DbType.Int64, loyaltyProgramDetails.ID);
                    this.dbServer.AddInParameter(command2, "LoyaltyProgramUnitID", DbType.Int64, cvo.LoyaltyUnitID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, cvo.Status);
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, cvo.ID);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    cvo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                }
                foreach (clsLoyaltyProgramPatientCategoryVO yvo in loyaltyProgramDetails.CategoryList)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddLoyaltyProgramPatientCategoryDetails");
                    this.dbServer.AddInParameter(command3, "LoyaltyProgramID", DbType.Int64, loyaltyProgramDetails.ID);
                    this.dbServer.AddInParameter(command3, "PatientCategoryID", DbType.Int64, yvo.PatientCategoryID);
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, yvo.Status);
                    this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, yvo.ID);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    yvo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                }
                foreach (clsLoyaltyProgramFamilyDetails details in loyaltyProgramDetails.FamilyList)
                {
                    DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddLoyaltyProgramFamilyDetails");
                    this.dbServer.AddInParameter(command4, "LoyaltyProgramID", DbType.Int64, loyaltyProgramDetails.ID);
                    this.dbServer.AddInParameter(command4, "RelationID", DbType.Int64, details.RelationID);
                    this.dbServer.AddInParameter(command4, "TariffID", DbType.Int64, details.TariffID);
                    this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command4, "Status", DbType.Boolean, details.Status);
                    this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                    this.dbServer.ExecuteNonQuery(command4, transaction);
                    details.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                }
                foreach (clsLoyaltyAttachmentVO tvo in loyaltyProgramDetails.AttachmentList)
                {
                    DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_AddLoyaltyProgramAttachmentDetails");
                    this.dbServer.AddInParameter(command5, "LoyaltyProgramID", DbType.Int64, loyaltyProgramDetails.ID);
                    this.dbServer.AddInParameter(command5, "AttachmentFileName", DbType.String, tvo.AttachmentFileName);
                    this.dbServer.AddInParameter(command5, "Attachment", DbType.Binary, tvo.Attachment);
                    this.dbServer.AddInParameter(command5, "DocumentName", DbType.String, tvo.DocumentName);
                    this.dbServer.AddInParameter(command5, "Status", DbType.Boolean, tvo.Status);
                    this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command5, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command5, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command5, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command5, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command5, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                    this.dbServer.ExecuteNonQuery(command5, transaction);
                    tvo.ID = (long) this.dbServer.GetParameterValue(command5, "ID");
                }
                transaction.Commit();
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                nvo.LoyaltyProgramDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject FillCardTypeCombo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsFillCardTypeComboBizActionVO nvo = (clsFillCardTypeComboBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillCardTypeCombo");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString(), (bool) reader["Status"]));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject FillFamilyTariffUsingRelationID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsFillFamilyTariffUsingRelationIDBizActionVO nvo = (clsFillFamilyTariffUsingRelationIDBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillTariffUsingRelation");
                this.dbServer.AddInParameter(storedProcCommand, "RelationID", DbType.Int64, nvo.RelationID);
                this.dbServer.AddInParameter(storedProcCommand, "LoyaltyProgramID", DbType.Int64, nvo.LoyaltyProgramID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.Details == null)
                        {
                            nvo.Details = new clsLoyaltyProgramFamilyDetails();
                        }
                        nvo.Details.LoyaltyProgramID = (long) reader["LoyaltyProgramID"];
                        nvo.Details.RelationID = (long) reader["RelationID"];
                        nvo.Details.TariffID = (long) reader["TariffID"];
                        nvo.Details.Tariff = (string) reader["Tariff"];
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAttachmentDetailsByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLoyaltyProgramAttachmentByIdBizActionVO nvo = (clsGetLoyaltyProgramAttachmentByIdBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetLoyaltyAttachmentbyID");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.AttachmentDetails == null)
                        {
                            nvo.AttachmentDetails = new clsLoyaltyAttachmentVO();
                        }
                        nvo.AttachmentDetails.ID = (long) reader["ID"];
                        nvo.AttachmentDetails.LoyaltyProgramID = (long) reader["LoyaltyProgramID"];
                        nvo.AttachmentDetails.DocumentName = (string) reader["DocumentName"];
                        nvo.AttachmentDetails.AttachmentFileName = (string) reader["AttachmentFileName"];
                        nvo.AttachmentDetails.Attachment = (byte[]) DALHelper.HandleDBNull(reader["Attachment"]);
                        nvo.AttachmentDetails.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetCategoryList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLoyaltyPatientCategoryBizActionVO nvo = (clsGetLoyaltyPatientCategoryBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetLoyaltyPatientCategory");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.CategoryList == null)
                    {
                        nvo.CategoryList = new List<clsLoyaltyProgramPatientCategoryVO>();
                    }
                    while (reader.Read())
                    {
                        clsLoyaltyProgramPatientCategoryVO item = new clsLoyaltyProgramPatientCategoryVO {
                            PatientCategoryID = (long) reader["ID"],
                            Description = reader["Description"].ToString(),
                            Status = (bool) reader["Status"]
                        };
                        nvo.CategoryList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetClinicList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLoyaltyClinicBizActionVO nvo = (clsGetLoyaltyClinicBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetLoyaltyClinics");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ClinicList == null)
                    {
                        nvo.ClinicList = new List<clsLoyaltyClinicVO>();
                    }
                    while (reader.Read())
                    {
                        clsLoyaltyClinicVO item = new clsLoyaltyClinicVO {
                            LoyaltyUnitID = (long) reader["ID"],
                            LoyaltyUnitDescription = reader["Description"].ToString(),
                            Status = (bool) reader["Status"]
                        };
                        nvo.ClinicList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetFamilyDetailsByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLoyaltyProgramFamilyByIdBizActionVO nvo = (clsGetLoyaltyProgramFamilyByIdBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetLoyaltyFamilyDetailsbyID");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.FamilyDetails == null)
                        {
                            nvo.FamilyDetails = new clsLoyaltyProgramFamilyDetails();
                        }
                        nvo.FamilyDetails.ID = (long) reader["ID"];
                        nvo.FamilyDetails.Tariff = (string) reader["Tariff"];
                        nvo.FamilyDetails.Relation = (string) reader["Relation"];
                        nvo.FamilyDetails.TariffID = (long) reader["TariffID"];
                        nvo.FamilyDetails.RelationID = (long) reader["RelationID"];
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetLoyaltyProgram(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLoyaltyProgramListBizActionVO nvo = (clsGetLoyaltyProgramListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetLoyaltyProgramList");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                if ((nvo.LoyaltyProgramName != null) && (nvo.LoyaltyProgramName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.LoyaltyProgramName + "%");
                }
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.LoyaltyProgramList == null)
                    {
                        nvo.LoyaltyProgramList = new List<clsLoyaltyProgramVO>();
                    }
                    while (reader.Read())
                    {
                        clsLoyaltyProgramVO item = new clsLoyaltyProgramVO {
                            ID = (long) reader["ID"],
                            LoyaltyProgramName = (string) reader["Description"],
                            EffectiveDate = new DateTime?((DateTime) reader["EffectiveDate"]),
                            ExpiryDate = new DateTime?((DateTime) reader["ExpiryDate"]),
                            Status = (bool) reader["Status"],
                            TariffID = (long) reader["TariffID"],
                            Tariff = (string) reader["Tariff"]
                        };
                        nvo.LoyaltyProgramList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetLoyaltyProgramByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLoyaltyProgramByIDBizActionVO nvo = (clsGetLoyaltyProgramByIDBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetLoyaltyProgramByID");
                this.dbServer.AddInParameter(storedProcCommand, "LoyaltyProgramID", DbType.Int64, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.LoyaltyProgramDetails == null)
                        {
                            nvo.LoyaltyProgramDetails = new clsLoyaltyProgramVO();
                        }
                        nvo.LoyaltyProgramDetails.ID = (long) reader["ID"];
                        nvo.LoyaltyProgramDetails.LoyaltyProgramName = (string) reader["Description"];
                        nvo.LoyaltyProgramDetails.EffectiveDate = new DateTime?((DateTime) reader["EffectiveDate"]);
                        nvo.LoyaltyProgramDetails.ExpiryDate = new DateTime?((DateTime) reader["ExpiryDate"]);
                        nvo.LoyaltyProgramDetails.Remark = (string) DALHelper.HandleDBNull(reader["Remark"]);
                        nvo.LoyaltyProgramDetails.Status = (bool) reader["Status"];
                        nvo.LoyaltyProgramDetails.PatientCategory = (string) reader["Description"];
                        nvo.LoyaltyProgramDetails.TariffID = (long) reader["TariffID"];
                        nvo.LoyaltyProgramDetails.Tariff = (string) reader["Tariff"];
                        nvo.LoyaltyProgramDetails.IsFamily = (bool) reader["IsFamily"];
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    nvo.LoyaltyProgramDetails.CategoryList = new List<clsLoyaltyProgramPatientCategoryVO>();
                    while (reader.Read())
                    {
                        clsLoyaltyProgramPatientCategoryVO item = new clsLoyaltyProgramPatientCategoryVO {
                            LoyaltyProgramID = (long) DALHelper.HandleDBNull(reader["LoyaltyProgramID"]),
                            PatientCategoryID = (long) DALHelper.HandleDBNull(reader["PatientCategoryID"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.LoyaltyProgramDetails.CategoryList.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    nvo.LoyaltyProgramDetails.ClinicList = new List<clsLoyaltyClinicVO>();
                    while (reader.Read())
                    {
                        clsLoyaltyClinicVO item = new clsLoyaltyClinicVO {
                            LoyaltyProgramID = (long) DALHelper.HandleDBNull(reader["LoyaltyProgramID"]),
                            LoyaltyUnitID = (long) DALHelper.HandleDBNull(reader["LoyaltyProgramUnitID"]),
                            LoyaltyUnitDescription = (string) DALHelper.HandleDBNull(reader["Description"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.LoyaltyProgramDetails.ClinicList.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    nvo.LoyaltyProgramDetails.FamilyList = new List<clsLoyaltyProgramFamilyDetails>();
                    while (reader.Read())
                    {
                        clsLoyaltyProgramFamilyDetails item = new clsLoyaltyProgramFamilyDetails {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            LoyaltyProgramID = (long) DALHelper.HandleDBNull(reader["LoyaltyProgramID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            RelationID = (long) DALHelper.HandleDBNull(reader["RelationID"]),
                            Relation = (string) DALHelper.HandleDBNull(reader["Relation"]),
                            TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"]),
                            Tariff = (string) DALHelper.HandleDBNull(reader["Tariff"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.LoyaltyProgramDetails.FamilyList.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    nvo.LoyaltyProgramDetails.AttachmentList = new List<clsLoyaltyAttachmentVO>();
                    while (reader.Read())
                    {
                        clsLoyaltyAttachmentVO item = new clsLoyaltyAttachmentVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            LoyaltyProgramID = (long) DALHelper.HandleDBNull(reader["LoyaltyProgramID"]),
                            DocumentName = (string) DALHelper.HandleDBNull(reader["DocumentName"]),
                            Attachment = (byte[]) DALHelper.HandleDBNull(reader["Attachment"]),
                            AttachmentFileName = (string) DALHelper.HandleDBNull(reader["AttachmentFileName"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.LoyaltyProgramDetails.AttachmentList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetLoyaltyProgramTariffByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLoyaltyProgramTariffByIDBizActionVO nvo = (clsGetLoyaltyProgramTariffByIDBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillTariff");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.Details == null)
                        {
                            nvo.Details = new clsLoyaltyProgramVO();
                        }
                        nvo.Details.ID = (long) reader["ID"];
                        nvo.Details.TariffID = (long) reader["TariffID"];
                        nvo.Details.Tariff = (string) reader["Description"];
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetRelationMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRelationMasterListBizActionVO nvo = (clsGetRelationMasterListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRelationList");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.FamilyList == null)
                    {
                        nvo.FamilyList = new List<clsLoyaltyProgramFamilyDetails>();
                    }
                    while (reader.Read())
                    {
                        clsLoyaltyProgramFamilyDetails item = new clsLoyaltyProgramFamilyDetails {
                            RelationID = (long) reader["ID"],
                            RelationCode = reader["Code"].ToString(),
                            RelationDescription = reader["Description"].ToString(),
                            Status = (bool) reader["Status"]
                        };
                        nvo.FamilyList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateLoyaltyProgram(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            clsUpdateLoyaltyProgramBizActionVO nvo = (clsUpdateLoyaltyProgramBizActionVO) valueObject;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsLoyaltyProgramVO loyaltyProgram = nvo.LoyaltyProgram;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateLoyaltyProgram");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, loyaltyProgram.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, loyaltyProgram.LoyaltyProgramName.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, loyaltyProgram.EffectiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, loyaltyProgram.ExpiryDate);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, loyaltyProgram.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFamily", DbType.Boolean, loyaltyProgram.IsFamily);
                if (loyaltyProgram.Remark != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, loyaltyProgram.Remark.Trim());
                }
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                if ((loyaltyProgram.ClinicList != null) && (loyaltyProgram.ClinicList.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLoyaltyProgramClinicDetails");
                    this.dbServer.AddInParameter(command2, "LoyaltyProgramID", DbType.Int64, loyaltyProgram.ID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                if ((loyaltyProgram.CategoryList != null) && (loyaltyProgram.CategoryList.Count > 0))
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLoyaltyProgramPatientCategoryDetails");
                    this.dbServer.AddInParameter(command3, "LoyaltyProgramID", DbType.Int64, loyaltyProgram.ID);
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                }
                foreach (clsLoyaltyClinicVO cvo in loyaltyProgram.ClinicList)
                {
                    DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddLoyaltyProgramClinicDetails");
                    this.dbServer.AddInParameter(command4, "LoyaltyProgramID", DbType.Int64, loyaltyProgram.ID);
                    this.dbServer.AddInParameter(command4, "LoyaltyProgramUnitID", DbType.Int64, cvo.LoyaltyUnitID);
                    this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command4, "Status", DbType.Boolean, cvo.Status);
                    this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, cvo.ID);
                    this.dbServer.ExecuteNonQuery(command4, transaction);
                    cvo.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                }
                foreach (clsLoyaltyProgramPatientCategoryVO yvo in loyaltyProgram.CategoryList)
                {
                    DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_AddLoyaltyProgramPatientCategoryDetails");
                    this.dbServer.AddInParameter(command5, "LoyaltyProgramID", DbType.Int64, loyaltyProgram.ID);
                    this.dbServer.AddInParameter(command5, "PatientCategoryID", DbType.Int64, yvo.PatientCategoryID);
                    this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command5, "Status", DbType.Boolean, yvo.Status);
                    this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, yvo.ID);
                    this.dbServer.ExecuteNonQuery(command5, transaction);
                    yvo.ID = (long) this.dbServer.GetParameterValue(command5, "ID");
                }
                if ((loyaltyProgram.FamilyList != null) && (loyaltyProgram.FamilyList.Count > 0))
                {
                    DbCommand command6 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLoyaltyProgramFamilyDetails");
                    this.dbServer.AddInParameter(command6, "LoyaltyProgramID", DbType.Int64, loyaltyProgram.ID);
                    this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.ExecuteNonQuery(command6, transaction);
                }
                foreach (clsLoyaltyProgramFamilyDetails details in loyaltyProgram.FamilyList)
                {
                    DbCommand command7 = this.dbServer.GetStoredProcCommand("CIMS_AddLoyaltyProgramFamilyDetails");
                    this.dbServer.AddInParameter(command7, "LoyaltyProgramID", DbType.Int64, loyaltyProgram.ID);
                    this.dbServer.AddInParameter(command7, "RelationID", DbType.Int64, details.RelationID);
                    this.dbServer.AddInParameter(command7, "TariffID", DbType.Int64, details.TariffID);
                    this.dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command7, "Status", DbType.Boolean, details.Status);
                    this.dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                    this.dbServer.ExecuteNonQuery(command7, transaction);
                    details.ID = (long) this.dbServer.GetParameterValue(command7, "ID");
                }
                if ((loyaltyProgram.AttachmentList != null) && (loyaltyProgram.AttachmentList.Count > 0))
                {
                    DbCommand command8 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLoyaltyProgramAttachmentDetailsDetails");
                    this.dbServer.AddInParameter(command8, "LoyaltyProgramID", DbType.Int64, loyaltyProgram.ID);
                    this.dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.ExecuteNonQuery(command8, transaction);
                }
                foreach (clsLoyaltyAttachmentVO tvo in loyaltyProgram.AttachmentList)
                {
                    DbCommand command9 = this.dbServer.GetStoredProcCommand("CIMS_AddLoyaltyProgramAttachmentDetails");
                    this.dbServer.AddInParameter(command9, "LoyaltyProgramID", DbType.Int64, loyaltyProgram.ID);
                    this.dbServer.AddInParameter(command9, "AttachmentFileName", DbType.String, tvo.AttachmentFileName);
                    this.dbServer.AddInParameter(command9, "Attachment", DbType.Binary, tvo.Attachment);
                    this.dbServer.AddInParameter(command9, "DocumentName", DbType.String, tvo.DocumentName);
                    this.dbServer.AddInParameter(command9, "Status", DbType.Boolean, tvo.Status);
                    this.dbServer.AddInParameter(command9, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command9, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command9, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command9, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command9, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command9, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command9, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                    this.dbServer.ExecuteNonQuery(command9, transaction);
                    tvo.ID = (long) this.dbServer.GetParameterValue(command9, "ID");
                }
                transaction.Commit();
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                nvo.LoyaltyProgram = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }
    }
}

