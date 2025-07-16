namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.CustomExceptions;
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.Administration.RoleMaster;
    using PalashDynamics.ValueObjects.Inventory;
    using PalashDynamics.ValueObjects.Master;
    using PalashDynamics.ValueObjects.Master.DoctorMaster;
    using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    internal class clsMasterDAL : clsBaseMasterDAL
    {
        private Database dbServer;
        public bool chkFlag = true;

        private clsMasterDAL()
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

        private void AddAttachmentDetails(clsEmailTemplateVO objTemplateVO, clsUserVO objUserVO)
        {
            try
            {
                DbCommand storedProcCommand = null;
                for (int i = 0; i < objTemplateVO.AttachmentNos; i++)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddEmailAttachmentDetails");
                    this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                    this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, objTemplateVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "Attachment", DbType.Binary, objTemplateVO.AttachmentDetails[i].Attachment);
                    this.dbServer.AddInParameter(storedProcCommand, "AttachmentFileName", DbType.String, objTemplateVO.AttachmentDetails[i].AttachmentFileName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    objTemplateVO.AttachmentDetails[i].ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
        }

        private void AddCommonParametersForAddUpdateEmailTemplate(DbCommand command, clsEmailTemplateVO objTemplateVO, clsUserVO objUserVO)
        {
            this.dbServer.AddInParameter(command, "Code", DbType.String, objTemplateVO.Code);
            this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
            this.dbServer.AddInParameter(command, "Description", DbType.String, objTemplateVO.Description);
            this.dbServer.AddInParameter(command, "Subject", DbType.String, objTemplateVO.Subject);
            this.dbServer.AddInParameter(command, "Text", DbType.String, objTemplateVO.Text);
            this.dbServer.AddInParameter(command, "EmailFormat", DbType.Boolean, objTemplateVO.EmailFormat);
            this.dbServer.AddInParameter(command, "AttachmentNos", DbType.Int64, objTemplateVO.AttachmentNos);
            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
        }

        private void AddCommonParametersForAddUpdateMethods(DbCommand command, clsUserRoleVO objRoleVO, clsUserVO objUserVO)
        {
            this.dbServer.AddInParameter(command, "Code", DbType.String, objRoleVO.Code);
            this.dbServer.AddInParameter(command, "Description", DbType.String, objRoleVO.Description);
            this.dbServer.AddInParameter(command, "Status", DbType.Boolean, objRoleVO.Status);
            this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            for (int i = 0; i < objRoleVO.MenuList.Count; i++)
            {
                clsMenuVO uvo = objRoleVO.MenuList[i];
                builder.Append(uvo.ID);
                builder2.Append(uvo.Status);
                if (i < (objRoleVO.MenuList.Count - 1))
                {
                    builder.Append(",");
                    builder2.Append(",");
                }
            }
            this.dbServer.AddInParameter(command, "MenuIdList", DbType.String, builder.ToString());
            this.dbServer.AddInParameter(command, "MenuStatusList", DbType.String, builder2.ToString());
            StringBuilder builder3 = new StringBuilder();
            StringBuilder builder4 = new StringBuilder();
            for (int j = 0; j < objRoleVO.DashBoardList.Count; j++)
            {
                clsDashBoardVO dvo = objRoleVO.DashBoardList[j];
                builder3.Append(dvo.ID);
                builder4.Append(dvo.Status);
                if (j < (objRoleVO.DashBoardList.Count - 1))
                {
                    builder3.Append(",");
                    builder4.Append(",");
                }
            }
            this.dbServer.AddInParameter(command, "DashboardIdList", DbType.String, builder3.ToString());
            this.dbServer.AddInParameter(command, "DashboardStatusList", DbType.String, builder4.ToString());
        }

        private void AddCommonParametersForAddUpdateParameter(DbCommand command, clsPathoParameterMasterVO objParameter, clsUserVO objUserVO)
        {
            try
            {
                this.dbServer.AddInParameter(command, "Code", DbType.String, objParameter.Code);
                this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command, "ParameterUnitId", DbType.Int64, objParameter.ParamUnit);
                this.dbServer.AddInParameter(command, "Description", DbType.String, objParameter.ParameterDesc);
                this.dbServer.AddInParameter(command, "PrintName", DbType.String, objParameter.PrintName);
                this.dbServer.AddInParameter(command, "IsNumeric", DbType.Boolean, objParameter.IsNumeric);
                this.dbServer.AddInParameter(command, "Formula", DbType.String, objParameter.Formula);
                this.dbServer.AddInParameter(command, "FormulaID", DbType.String, objParameter.FormulaID);
                this.dbServer.AddInParameter(command, "TechniqueUsed", DbType.String, objParameter.Technique);
                this.dbServer.AddInParameter(command, "DeltaCheckPer", DbType.Double, objParameter.DeltaCheckPer);
                this.dbServer.AddInParameter(command, "ExcutionCalenderParameterID", DbType.String, objParameter.ExcutionCalenderParameterID);
                this.dbServer.AddInParameter(command, "TextValue", DbType.String, objParameter.TextRange);
                this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AddCommonParametersForAddUpdateSMSTemplate(DbCommand command, clsSMSTemplateVO objTemplateVO, clsUserVO objUserVO)
        {
            this.dbServer.AddInParameter(command, "Code", DbType.String, objTemplateVO.Code);
            this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
            this.dbServer.AddInParameter(command, "Description", DbType.String, objTemplateVO.Description);
            this.dbServer.AddInParameter(command, "EnglishText", DbType.String, objTemplateVO.EnglishText);
            this.dbServer.AddInParameter(command, "LocalText", DbType.String, objTemplateVO.LocalText);
            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
        }

        public override IValueObject AddEmailTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddEmailTemplateBizActionVO nvo = valueObject as clsAddEmailTemplateBizActionVO;
            try
            {
                clsEmailTemplateVO emailTemplate = nvo.EmailTemplate;
                DbCommand storedProcCommand = null;
                if (emailTemplate.ID == 0L)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddEmailTemplate");
                    this.dbServer.AddOutParameter(storedProcCommand, "Id", DbType.Int64, 0x7fffffff);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.chkFlag = true;
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateEmailTemplate");
                    this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, emailTemplate.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.chkFlag = false;
                }
                this.AddCommonParametersForAddUpdateEmailTemplate(storedProcCommand, emailTemplate, UserVo);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.EmailTemplate.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                emailTemplate = nvo.EmailTemplate;
                if (this.chkFlag && (emailTemplate.AttachmentNos > 0L))
                {
                    this.AddAttachmentDetails(emailTemplate, UserVo);
                }
                else if (!this.chkFlag && (emailTemplate.AttachmentNos > 0L))
                {
                    this.DeleteAttachmentDetails(emailTemplate, UserVo);
                    this.AddAttachmentDetails(emailTemplate, UserVo);
                }
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        private void AddParameterDefaultDetails(clsPathoParameterMasterVO objParameter, clsUserVO objUserVO, bool NewAdd)
        {
            try
            {
                if (objParameter.DefaultValues.Count > 0)
                {
                    foreach (clsPathoParameterDefaultValueMasterVO rvo in objParameter.DefaultValues)
                    {
                        DbCommand storedProcCommand = null;
                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoParameterDefaultValueMaster");
                        this.dbServer.AddInParameter(storedProcCommand, "ParameterID", DbType.Int64, objParameter.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, rvo.CategoryID);
                        this.dbServer.AddInParameter(storedProcCommand, "Category", DbType.String, rvo.Category);
                        this.dbServer.AddInParameter(storedProcCommand, "IsAgeApplicable", DbType.Boolean, rvo.IsAge);
                        this.dbServer.AddInParameter(storedProcCommand, "AgeFrom", DbType.Double, rvo.AgeFrom);
                        this.dbServer.AddInParameter(storedProcCommand, "AgeTo", DbType.Double, rvo.AgeTo);
                        this.dbServer.AddInParameter(storedProcCommand, "MinValue", DbType.Double, rvo.MinValue);
                        this.dbServer.AddInParameter(storedProcCommand, "MaxValue", DbType.Double, rvo.MaxValue);
                        this.dbServer.AddInParameter(storedProcCommand, "HighReffValue", DbType.Double, rvo.HighReffValue);
                        this.dbServer.AddInParameter(storedProcCommand, "LowReffValue", DbType.Double, rvo.LowReffValue);
                        this.dbServer.AddInParameter(storedProcCommand, "UpperPanicValue", DbType.Double, rvo.UpperPanicValue);
                        this.dbServer.AddInParameter(storedProcCommand, "LowerPanicValue", DbType.Double, rvo.LowerPanicValue);
                        this.dbServer.AddInParameter(storedProcCommand, "LowReflex", DbType.Double, rvo.LowReflexValue);
                        this.dbServer.AddInParameter(storedProcCommand, "HighReflex", DbType.Double, rvo.HighReflexValue);
                        this.dbServer.AddInParameter(storedProcCommand, "DefaultValue", DbType.Double, rvo.DefaultValue);
                        this.dbServer.AddInParameter(storedProcCommand, "AgeValue", DbType.String, rvo.AgeValue);
                        this.dbServer.AddInParameter(storedProcCommand, "MachineID", DbType.Int64, rvo.MachineID);
                        this.dbServer.AddInParameter(storedProcCommand, "Machine", DbType.String, rvo.Machine);
                        this.dbServer.AddInParameter(storedProcCommand, "MinImprobable", DbType.Double, rvo.MinImprobable);
                        this.dbServer.AddInParameter(storedProcCommand, "MaxImprobable", DbType.Double, rvo.MaxImprobable);
                        this.dbServer.AddInParameter(storedProcCommand, "Note", DbType.String, rvo.Note);
                        this.dbServer.AddInParameter(storedProcCommand, "IsReflexTesting", DbType.Boolean, rvo.IsReflexTesting);
                        this.dbServer.AddInParameter(storedProcCommand, "VaryingReferences", DbType.String, rvo.VaryingReferences);
                        this.AddParameterUserDetails(storedProcCommand, objUserVO);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        long num = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                        if (rvo.List != null)
                        {
                            foreach (MasterListItem item in rvo.List)
                            {
                                DbCommand command = null;
                                command = this.dbServer.GetStoredProcCommand("CIMS_AddServiceListForParameter");
                                command.Parameters.Clear();
                                this.dbServer.AddInParameter(command, "Id", DbType.Int64, 0);
                                this.dbServer.AddInParameter(command, "ParameterDefaultID", DbType.Int64, num);
                                this.dbServer.AddInParameter(command, "ServiceID", DbType.Int64, item.ID);
                                this.dbServer.AddInParameter(command, "ParameterID", DbType.Int64, objParameter.ID);
                                this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command, "Status", DbType.Boolean, 1);
                                this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
                                this.dbServer.ExecuteNonQuery(command);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AddParameterHelpValueDetails(clsPathoParameterMasterVO objParameter, clsUserVO objUserVO, bool NewAdd)
        {
            try
            {
                if (objParameter.Items.Count > 0)
                {
                    foreach (clsPathoParameterHelpValueMasterVO rvo in objParameter.Items)
                    {
                        DbCommand storedProcCommand = null;
                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoParameterHelpValueMaster");
                        this.dbServer.AddInParameter(storedProcCommand, "ParameterID", DbType.Int64, objParameter.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "HelpValue", DbType.String, rvo.strHelp);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDefault", DbType.Boolean, rvo.IsDefault);
                        this.dbServer.AddInParameter(storedProcCommand, "IsAbnoramal", DbType.Boolean, rvo.IsAbnoramal);
                        this.AddParameterUserDetails(storedProcCommand, objUserVO);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AddParameterUserDetails(DbCommand command, clsUserVO objUserVO)
        {
            this.dbServer.AddOutParameter(command, "ID", DbType.Int64, 0x7fffffff);
            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
        }

        public override IValueObject AddSMSTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddSMSTemplateBizActionVO nvo = valueObject as clsAddSMSTemplateBizActionVO;
            clsSMSTemplateVO sMSTemplate = nvo.SMSTemplate;
            try
            {
                DbCommand storedProcCommand = null;
                if (sMSTemplate.ID == 0L)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddSMSTemplate");
                    this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateSMSTemplate");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, sMSTemplate.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                }
                this.AddCommonParametersForAddUpdateSMSTemplate(storedProcCommand, sMSTemplate, UserVo);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                nvo.SMSTemplate.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddUpdateCleavageGradeDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateCleavageGradeMasterBizActionVO nvo = valueObject as clsAddUpdateCleavageGradeMasterBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateCleavageGrade");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, nvo.CleavageDetails.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.CleavageDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Name", DbType.String, nvo.CleavageDetails.Name);
                this.dbServer.AddInParameter(storedProcCommand, "ApplyTo", DbType.String, nvo.CleavageDetails.ApplyTo);
                this.dbServer.AddInParameter(storedProcCommand, "Flag", DbType.String, nvo.CleavageDetails.Flag);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.CleavageDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.CleavageDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                nvo.CleavageDetails = null;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject AddUpdateCurrencyMasterList(IValueObject valueObject, clsUserVO userVO)
        {
            clsCurrencyMasterListVO tvo = new clsCurrencyMasterListVO();
            clsAddUpdateCurrencyMasterListBizActionVO nvo = valueObject as clsAddUpdateCurrencyMasterListBizActionVO;
            try
            {
                tvo = nvo.ItemMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateCurrencyMasterList");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, tvo.Id);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, tvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, tvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Symbol", DbType.String, tvo.Symbol);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, tvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, tvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "AddUnitID", DbType.Int64, tvo.AddUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "By", DbType.Int64, tvo.By);
                this.dbServer.AddInParameter(storedProcCommand, "On", DbType.String, tvo.On);
                this.dbServer.AddInParameter(storedProcCommand, "DateTime", DbType.DateTime, tvo.DateTime);
                this.dbServer.AddInParameter(storedProcCommand, "WindowsLoginName", DbType.String, tvo.WindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception exception1)
            {
                if (exception1.Message.Contains("duplicate"))
                {
                    tvo.PrimaryKeyViolationError = true;
                }
                else
                {
                    tvo.GeneralError = true;
                }
            }
            return nvo;
        }

        public override IValueObject AddUpdateMasterList(IValueObject valueObject, clsUserVO userVO)
        {
            clsMasterListVO tvo = new clsMasterListVO();
            clsAddUpdateMasterListBizActionVO nvo = valueObject as clsAddUpdateMasterListBizActionVO;
            try
            {
                tvo = nvo.ItemMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateMasterList");
                this.dbServer.AddInParameter(storedProcCommand, "MasterTableName", DbType.String, tvo.MasterTableName);
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, tvo.Id);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, tvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, tvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, tvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, tvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "AddUnitID", DbType.Int64, tvo.AddUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "By", DbType.Int64, tvo.By);
                this.dbServer.AddInParameter(storedProcCommand, "On", DbType.String, tvo.On);
                this.dbServer.AddInParameter(storedProcCommand, "DateTime", DbType.DateTime, tvo.DateTime);
                this.dbServer.AddInParameter(storedProcCommand, "WindowsLoginName", DbType.String, tvo.WindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception exception1)
            {
                if (exception1.Message.Contains("duplicate"))
                {
                    tvo.PrimaryKeyViolationError = true;
                }
                else
                {
                    tvo.GeneralError = true;
                }
            }
            return nvo;
        }

        public override IValueObject AddUpdateParameter(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdatePathoParameterBizActionVO nvo = valueObject as clsAddUpdatePathoParameterBizActionVO;
            clsPathoParameterMasterVO pathologyParameter = nvo.PathologyParameter;
            bool newAdd = true;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoParameterMaster");
                if (nvo.PathologyParameter.ID == 0L)
                {
                    this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    newAdd = true;
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, pathologyParameter.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                    newAdd = false;
                }
                this.AddCommonParametersForAddUpdateParameter(storedProcCommand, pathologyParameter, objUserVO);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                if (nvo.SuccessStatus == 1)
                {
                    nvo.PathologyParameter.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                    pathologyParameter.ID = nvo.PathologyParameter.ID;
                    if (!newAdd)
                    {
                        this.DeleteHelpAndDefaultValueDetails(pathologyParameter, objUserVO);
                    }
                    if (pathologyParameter.IsNumeric)
                    {
                        this.AddParameterDefaultDetails(pathologyParameter, objUserVO, newAdd);
                    }
                    else
                    {
                        this.AddParameterHelpValueDetails(pathologyParameter, objUserVO, newAdd);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddUpdateSurrogactAgencyDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateSurrogactAgencyMasterBizActionVO nvo = valueObject as clsAddUpdateSurrogactAgencyMasterBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateSurrogactAgency");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Affilatedyear", DbType.DateTime, nvo.AgencyDetails.Affilatedyear);
                this.dbServer.AddInParameter(storedProcCommand, "ReferralTypeID", DbType.Int64, nvo.AgencyDetails.ReferralTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, nvo.AgencyDetails.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.AgencyDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "SourceEmail", DbType.String, nvo.AgencyDetails.SourceEmail);
                this.dbServer.AddInParameter(storedProcCommand, "SourceContactNo", DbType.String, nvo.AgencyDetails.SourceContactNo);
                this.dbServer.AddInParameter(storedProcCommand, "SourceAddress", DbType.String, nvo.AgencyDetails.SourceAddress);
                this.dbServer.AddInParameter(storedProcCommand, "RegistrationNo", DbType.String, nvo.AgencyDetails.RegistrationNo);
                this.dbServer.AddInParameter(storedProcCommand, "AffilatedBy", DbType.String, nvo.AgencyDetails.AffilatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.AgencyDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.AgencyDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                nvo.AgencyDetails = null;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject AddUserRole(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUserRoleBizActionVO nvo = valueObject as clsAddUserRoleBizActionVO;
            try
            {
                clsUserRoleVO details = nvo.Details;
                DbCommand storedProcCommand = null;
                if (details.ID == 0L)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUserRole");
                    this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateUserRole");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, details.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }
                this.AddCommonParametersForAddUpdateMethods(storedProcCommand, details, UserVo);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject CheckDuplicasy(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsCheckDuplicasyBizActionVO nvo = valueObject as clsCheckDuplicasyBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                foreach (clsCheckDuplicasyVO yvo in nvo.lstDuplicasy)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckDuplicasy");
                    this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, yvo.ItemID);
                    this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, yvo.StoreID);
                    this.dbServer.AddInParameter(storedProcCommand, "BatchCode", DbType.String, yvo.BatchCode);
                    this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, yvo.ExpiryDate);
                    this.dbServer.AddInParameter(storedProcCommand, "CostPrice", DbType.Double, yvo.CostPrice);
                    this.dbServer.AddInParameter(storedProcCommand, "MRP", DbType.Double, yvo.MRP);
                    this.dbServer.AddInParameter(storedProcCommand, "TransactionTypeID", DbType.Int16, (short) yvo.TransactionTypeID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsBatchRequired", DbType.Boolean, yvo.IsBatchRequired);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFree", DbType.Boolean, yvo.IsFree);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                    if (nvo.SuccessStatus == 1)
                    {
                        nvo.ItemName = yvo.ItemName;
                        nvo.BatchCode = yvo.BatchCode;
                        nvo.IsBatchRequired = yvo.IsBatchRequired;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        private void DeleteAttachmentDetails(clsEmailTemplateVO objTemplateVO, clsUserVO objUserVO)
        {
            try
            {
                if (!this.chkFlag)
                {
                    DbCommand storedProcCommand = null;
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateEmailAttachmentDetailsStatus");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, objTemplateVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                }
            }
            catch (Exception)
            {
            }
        }

        private void DeleteHelpAndDefaultValueDetails(clsPathoParameterMasterVO objParameter, clsUserVO objUserVO)
        {
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeletePathoParameterHelpValueDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ParameterID", DbType.Int64, objParameter.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                DbCommand command2 = null;
                command2 = this.dbServer.GetStoredProcCommand("CIMS_DeletePathoParameterDefaultValueDetails");
                this.dbServer.AddInParameter(command2, "ParameterID", DbType.Int64, objParameter.ID);
                this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(command2);
                DbCommand command3 = null;
                command3 = this.dbServer.GetStoredProcCommand("CIMS_DeleteServiceParameterDefaultValueDetails");
                this.dbServer.AddInParameter(command3, "ParameterID", DbType.Int64, objParameter.ID);
                this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(command3);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject DeptFromSubSpecilization(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsFillDepartmentBizActionVO nvo = (clsFillDepartmentBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("FillDeptFromSubSp");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = DALHelper.HandleIntegerNull(reader["ID"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            FilterID = (long) DALHelper.HandleDBNull(reader["SubSpecializationID"])
                        };
                        nvo.MasterList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAllDoctorList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorListForComboBizActionVO nvo = (clsGetDoctorListForComboBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillAllDoctors");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        MasterListItem item = new MasterListItem {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            SpecializationID = (long) DALHelper.HandleDBNull(reader["SpecializationID"])
                        };
                        nvo.MasterList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAnesthetist(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetAnesthetistBizActionVO nvo = (clsGetAnesthetistBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillAnesthetistCombobox");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAutoCompleteList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAutoCompleteListVO tvo = (clsGetAutoCompleteListVO) valueObject;
            try
            {
                StringBuilder builder = new StringBuilder();
                if (tvo.Parent != null)
                {
                    if (tvo.IsDecode)
                    {
                        builder.Append(tvo.Parent.Key.ToString() + " like '%" + Security.base64Encode(tvo.Parent.Value) + "%'");
                    }
                    else
                    {
                        builder.Append(tvo.Parent.Key.ToString() + " like '%" + tvo.Parent.Value + "%'");
                    }
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetListForAutoCompleteBox");
                this.dbServer.AddInParameter(storedProcCommand, "TableName", DbType.String, tvo.TableName);
                this.dbServer.AddInParameter(storedProcCommand, "ColumnName", DbType.String, tvo.ColumnName);
                this.dbServer.AddInParameter(storedProcCommand, "FilterExpression", DbType.String, builder.ToString());
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (tvo.List == null)
                    {
                        tvo.List = new List<string>();
                    }
                    switch (tvo.IsDecode)
                    {
                        case false:
                            while (reader.Read())
                            {
                                tvo.List.Add(reader[tvo.ColumnName].ToString());
                            }
                            break;

                        case true:
                            while (reader.Read())
                            {
                                tvo.List.Add(Security.base64Decode(reader[tvo.ColumnName].ToString()));
                            }
                            break;

                        default:
                            break;
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return tvo;
        }

        public override IValueObject GetAutoCompleteList_2colums(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAutoCompleteListVO_2Colums colums = (clsGetAutoCompleteListVO_2Colums) valueObject;
            try
            {
                StringBuilder builder = new StringBuilder();
                if (colums.Parent != null)
                {
                    if (colums.IsDecode)
                    {
                        builder.Append(colums.Parent.Key.ToString() + " like '%" + Security.base64Encode(colums.Parent.Value) + "%'");
                    }
                    else
                    {
                        builder.Append(colums.Parent.Key.ToString() + " like '%" + colums.Parent.Value + "%'");
                    }
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetListForAutoCompleteBox_2coloums");
                this.dbServer.AddInParameter(storedProcCommand, "TableName", DbType.String, colums.TableName);
                this.dbServer.AddInParameter(storedProcCommand, "ColumnName", DbType.String, colums.ColumnName);
                this.dbServer.AddInParameter(storedProcCommand, "FilterExpression", DbType.String, builder.ToString());
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (colums.List == null)
                    {
                        colums.List = new List<string>();
                    }
                    switch (colums.IsDecode)
                    {
                        case false:
                            while (reader.Read())
                            {
                                colums.List.Add(reader[colums.ColumnName].ToString());
                            }
                            break;

                        case true:
                            while (reader.Read())
                            {
                                colums.List.Add(Security.base64Decode(reader[colums.ColumnName].ToString()));
                            }
                            break;

                        default:
                            break;
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return colums;
        }

        public override IValueObject GetBdMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetBdMasterBizActionVO nvo = valueObject as clsGetBdMasterBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillBdMasterCombobox");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetCodeMasterList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetMasterListBizActionVO nvo = (clsGetMasterListBizActionVO) valueObject;
            try
            {
                StringBuilder builder = new StringBuilder();
                if (nvo.IsActive != null)
                {
                    builder.Append("Status = '" + nvo.IsActive.Value + "'");
                }
                if (nvo.Parent != null)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(" And ");
                    }
                    builder.Append(nvo.Parent.Value.ToString() + "='" + nvo.Parent.Key.ToString() + "'");
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMasterList_New");
                this.dbServer.AddInParameter(storedProcCommand, "MasterTableName", DbType.String, nvo.MasterTable.ToString());
                this.dbServer.AddInParameter(storedProcCommand, "FilterExpression", DbType.String, builder.ToString());
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Code"].ToString(), reader["Description"].ToString(), (bool) reader["Status"]));
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                nvo.Error = exception1.Message;
            }
            return nvo;
        }

        public override IValueObject GetColumnListByTableName(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetColumnListByTableNameBizActionVO nvo = (clsGetColumnListByTableNameBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetColumnListByTableName");
                this.dbServer.AddInParameter(storedProcCommand, "MasterTableName", DbType.String, nvo.MasterTable.ToString());
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ColumnList == null)
                    {
                        nvo.ColumnList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.ColumnList.Add(new MasterListItem(Convert.ToInt64(reader["Id"]), reader["ColumnName"].ToString()));
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                nvo.Error = exception1.Message;
            }
            return nvo;
        }

        public override IValueObject GetCurrencyMasterListDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetCurrencyMasterListDetailsBizActionVO nvo = valueObject as clsGetCurrencyMasterListDetailsBizActionVO;
            clsCurrencyMasterListVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCurrencyMasterListDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExperssion);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsCurrencyMasterListVO {
                            Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Symbol = Convert.ToString(DALHelper.HandleDBNull(reader["Symbol"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]))
                        };
                        nvo.ItemMatserDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetDashBoardList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDashBoardListVO tvo = valueObject as clsGetDashBoardListVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDashBoard");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, tvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (tvo.List == null)
                    {
                        tvo.List = new List<clsDashBoardVO>();
                    }
                    while (reader.Read())
                    {
                        clsDashBoardVO item = new clsDashBoardVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        tvo.List.Add(item);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetDataToPrint(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDatatoPrintMasterBizActionVO nvo = valueObject as clsGetDatatoPrintMasterBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDatatoPrintMasterBox");
                this.dbServer.AddInParameter(storedProcCommand, "id", DbType.Int64, nvo.id);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        clsMasterDataVO avo = new clsMasterDataVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])),
                            column0 = Convert.ToString(DALHelper.HandleDBNull(reader["Test"])),
                            column1 = Convert.ToString(DALHelper.HandleDBNull(reader["Test1"])),
                            column2 = Convert.ToString(DALHelper.HandleDBNull(reader["Test2"]))
                        };
                        MasterListItem item = new MasterListItem {
                            ID = avo.ID,
                            Code = avo.Code,
                            Description = avo.Description,
                            Status = true,
                            column0 = avo.column0,
                            column1 = avo.column1,
                            column2 = avo.column2,
                            PurchaseRate = avo.Rate
                        };
                        nvo.MasterList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorDepartmentDetailsBizActionVO nvo = (clsGetDoctorDepartmentDetailsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand;
                if (!nvo.IsServiceWiseDoctorList)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillDoctorCombobox");
                    this.dbServer.AddInParameter(storedProcCommand, "StrClinicID", DbType.String, nvo.StrClinicID);
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillServiceWiseDoctorCombobox");
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceId", DbType.Int64, nvo.ServiceId);
                    this.dbServer.AddInParameter(storedProcCommand, "AllRecord", DbType.Boolean, nvo.AllRecord);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorTypeID", DbType.Int64, nvo.DoctorTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        if (!nvo.IsServiceWiseDoctorList)
                        {
                            nvo.MasterList.Add(new MasterListItem((long) reader["Id"], Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"])), reader["Description"].ToString(), true));
                            continue;
                        }
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"])), reader["Description"].ToString(), true, Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]))));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDoctorListBySpecializationID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorDepartmentDetailsBizActionVO nvo = (clsGetDoctorDepartmentDetailsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorListBySpecializationID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "SpecializationID", DbType.Int64, nvo.SpecializationID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"])), reader["Name"].ToString(), true));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDoctorMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDoctorMasterListBizActionVO nvo = valueObject as clsGetDoctorMasterListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorMaterList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.String, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ComboList == null)
                    {
                        nvo.ComboList = new List<clsComboMasterBizActionVO>();
                    }
                    switch (nvo.IsDecode)
                    {
                        case false:
                            while (reader.Read())
                            {
                                clsComboMasterBizActionVO item = new clsComboMasterBizActionVO {
                                    ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                    Value = (string) DALHelper.HandleDBNull(reader["Value"]),
                                    EmailId = (string) DALHelper.HandleDBNull(reader["EmailId"])
                                };
                                nvo.ComboList.Add(item);
                            }
                            break;

                        case true:
                            while (reader.Read())
                            {
                                clsComboMasterBizActionVO item = new clsComboMasterBizActionVO {
                                    ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                    EmailId = (string) DALHelper.HandleDBNull(reader["EmailId"])
                                };
                                string str = (string) DALHelper.HandleDBNull(reader["Value"]);
                                if (!string.IsNullOrEmpty(str))
                                {
                                    str = "";
                                    foreach (string str2 in str.Split(new char[] { ' ' }))
                                    {
                                        str = str + Security.base64Decode(str2) + " ";
                                    }
                                }
                                item.Value = str.Trim();
                                nvo.ComboList.Add(item);
                            }
                            break;

                        default:
                            break;
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetEmailTemplateDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetEmailTemplateBizActionVO nvo = valueObject as clsGetEmailTemplateBizActionVO;
            try
            {
                clsEmailTemplateVO emailDetails = nvo.EmailDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetEmailTemplateDetails");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    int num = 0;
                    while (reader.Read())
                    {
                        if (nvo.EmailDetails == null)
                        {
                            nvo.EmailDetails = new clsEmailTemplateVO();
                        }
                        nvo.EmailDetails.AttachmentNos = (long) DALHelper.HandleDBNull(reader["AttachmentNos"]);
                        if (num == 0)
                        {
                            nvo.EmailDetails.Code = (string) DALHelper.HandleDBNull(reader["Code"]);
                            nvo.EmailDetails.Description = (string) DALHelper.HandleDBNull(reader["Description"]);
                            nvo.EmailDetails.Subject = (string) DALHelper.HandleDBNull(reader["Subject"]);
                            nvo.EmailDetails.Text = (string) DALHelper.HandleDBNull(reader["Text"]);
                            nvo.EmailDetails.EmailFormat = (bool) DALHelper.HandleDBNull(reader["EmailFormat"]);
                            nvo.EmailDetails.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                            nvo.EmailDetails.AttachmentDetails = new List<clsEmailAttachmentVO>();
                        }
                        if (nvo.EmailDetails.AttachmentNos > 0L)
                        {
                            nvo.EmailDetails.AttachmentDetails.Add(new clsEmailAttachmentVO());
                            nvo.EmailDetails.AttachmentDetails[num].ID = (long) DALHelper.HandleDBNull(reader["DetailsID"]);
                            nvo.EmailDetails.AttachmentDetails[num].Attachment = (byte[]) DALHelper.HandleDBNull(reader["Attachment"]);
                            nvo.EmailDetails.AttachmentDetails[num].AttachmentFileName = (string) DALHelper.HandleDBNull(reader["AttachmentFileName"]);
                            num++;
                        }
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

        public override IValueObject GetEmailTemplateList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetEmailTemplateListBizActionVO nvo = (clsGetEmailTemplateListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetEmailTemplateListForSearch");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "Code");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.EmailList == null)
                    {
                        nvo.EmailList = new List<clsEmailTemplateVO>();
                    }
                    while (reader.Read())
                    {
                        clsEmailTemplateVO item = new clsEmailTemplateVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            Subject = (string) DALHelper.HandleDBNull(reader["Subject"]),
                            Text = (string) DALHelper.HandleDBNull(reader["Text"]),
                            EmailFormat = (bool) DALHelper.HandleDBNull(reader["EmailFormat"])
                        };
                        item.EmailFormatDisp = !item.EmailFormat ? "HTML" : "Text";
                        item.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        nvo.EmailList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetEmbryologist(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetEmbryologistBizActionVO nvo = (clsGetEmbryologistBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillEmbryologistCombobox");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetMarketingExecutivesList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetMarketingExecutivesListVO tvo = valueObject as clsGetMarketingExecutivesListVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMarketingExecutivesList");
                this.dbServer.AddInParameter(storedProcCommand, "IsMarketingExecutives", DbType.String, tvo.IsMarketingExecutives);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.String, tvo.UnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    tvo.MarketingExecutivesList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["MarketingExecutivesName"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]))
                        };
                        tvo.MarketingExecutivesList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return tvo;
        }

        public override IValueObject GetMasterList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetMasterListBizActionVO nvo = (clsGetMasterListBizActionVO) valueObject;
            try
            {
                StringBuilder builder = new StringBuilder();
                if (nvo.IsActive != null)
                {
                    builder.Append("Status = '" + nvo.IsActive.Value + "'");
                }
                if (nvo.Parent != null)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(" And ");
                    }
                    builder.Append(nvo.Parent.Value.ToString() + "='" + nvo.Parent.Key.ToString() + "'");
                }
                DbCommand command = (nvo.MasterTable.ToString() != "M_MenuMaster") ? this.dbServer.GetStoredProcCommand("CIMS_GetMasterList") : this.dbServer.GetStoredProcCommand("CIMS_GetMasterMenu");
                this.dbServer.AddInParameter(command, "MasterTableName", DbType.String, nvo.MasterTable.ToString());
                this.dbServer.AddInParameter(command, "FilterExpression", DbType.String, builder.ToString());
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        if (nvo.MasterTable.ToString() == "M_MenuMaster")
                        {
                            nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString(), DALHelper.HandleDate(reader["Date"])));
                            continue;
                        }
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Code"].ToString(), reader["Description"].ToString(), DALHelper.HandleDate(reader["Date"])));
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                nvo.Error = exception1.Message;
            }
            return nvo;
        }

        public override IValueObject GetMasterListByTableName(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetMasterListByTableNameBizActionVO nvo = (clsGetMasterListByTableNameBizActionVO) valueObject;
            try
            {
                StringBuilder builder = new StringBuilder();
                if (nvo.Parent != null)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(" And ");
                    }
                    builder.Append(nvo.Parent.Value.ToString() + "='" + nvo.Parent.Key.ToString() + "'");
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMasterList");
                this.dbServer.AddInParameter(storedProcCommand, "MasterTableName", DbType.String, nvo.MasterTable.ToString());
                this.dbServer.AddInParameter(storedProcCommand, "FilterExpression", DbType.String, builder.ToString());
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Code"].ToString(), reader["Description"].ToString(), DALHelper.HandleDate(reader["Date"])));
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                nvo.Error = exception1.Message;
            }
            return nvo;
        }

        public override IValueObject GetMasterListByTableNameAndColumnName(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetMasterListByTableNameAndColumnNameBizActionVO nvo = (clsGetMasterListByTableNameAndColumnNameBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMasterListByTableNameAndColumnName");
                this.dbServer.AddInParameter(storedProcCommand, "MasterTableName", DbType.String, nvo.MasterTable.ToString());
                this.dbServer.AddInParameter(storedProcCommand, "ColumnName", DbType.String, nvo.ColumnName.ToString());
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                nvo.Error = exception1.Message;
            }
            return nvo;
        }

        public override IValueObject GetMasterListDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetMasterListDetailsBizActionVO nvo = valueObject as clsGetMasterListDetailsBizActionVO;
            clsMasterListVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMasterListDetails");
                this.dbServer.AddInParameter(storedProcCommand, "MasterTableName", DbType.String, nvo.MasterTableName);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExperssion);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsMasterListVO {
                            Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]))
                        };
                        nvo.ItemMatserDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetMasterListForConsent(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetMasterListConsentBizActionVO nvo = (clsGetMasterListConsentBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMasterList_New");
                this.dbServer.AddInParameter(storedProcCommand, "MasterTableName", DbType.String, nvo.MasterTable.ToString());
                this.dbServer.AddInParameter(storedProcCommand, "FilterExpression", DbType.String, nvo.FilterExpression);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        if ((nvo.MasterTable.ToString() != "M_PathoTestMaster") && (nvo.MasterTable.ToString() != "M_RadTestMaster"))
                        {
                            nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString(), reader["TemplateName"].ToString()));
                        }
                        else
                        {
                            nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString(), (long) reader["CategoryID"]));
                        }
                        if ((nvo.MasterTable.ToString() == "M_PathoTestMaster") && nvo.IsSubTest)
                        {
                            nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString(), (bool) reader["IsSubTest"]));
                        }
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                nvo.Error = exception1.Message;
            }
            return nvo;
        }

        public override IValueObject GetMasterNames(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetMasterNamesBizActionVO nvo = valueObject as clsGetMasterNamesBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMasterNames");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.objPrintMasterList == null)
                    {
                        nvo.objPrintMasterList = new List<clsPrintMasterVO>();
                    }
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        clsPrintMasterVO rvo = new clsPrintMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"])),
                            ViewName = Convert.ToString(DALHelper.HandleDBNull(reader["ViewName"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        MasterListItem item = new MasterListItem {
                            ID = rvo.ID,
                            Code = rvo.ViewName,
                            Description = rvo.Name,
                            Status = rvo.Status
                        };
                        nvo.MasterList.Add(item);
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

        public override IValueObject GetMasterSearchList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetSearchMasterListBizActionVO nvo = valueObject as clsGetSearchMasterListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMasterSearchList");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "MasterTableName", DbType.String, nvo.MasterTable);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, nvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"])
                        };
                        nvo.MasterList.Add(item);
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
            return valueObject;
        }

        public override IValueObject GetOtherThanReferralDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorDepartmentDetailsBizActionVO nvo = (clsGetDoctorDepartmentDetailsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillDoctorComboboxOtherThanReferral");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ReferralID", DbType.Int64, nvo.ReferralID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        if (!nvo.IsServiceWiseDoctorList)
                        {
                            nvo.MasterList.Add(new MasterListItem((long) reader["Id"], Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"])), reader["Description"].ToString(), true));
                            continue;
                        }
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"])), reader["Description"].ToString(), true, Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]))));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetParameterByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPathoParameterByIDBizActionVO nvo = valueObject as clsGetPathoParameterByIDBizActionVO;
            try
            {
                clsPathoParameterMasterVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoParameterByID");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.Details.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.Details == null)
                        {
                            nvo.Details = new clsPathoParameterMasterVO();
                        }
                        nvo.Details.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.Details.Code = (string) DALHelper.HandleDBNull(reader["Code"]);
                        nvo.Details.ParameterDesc = (string) DALHelper.HandleDBNull(reader["Description"]);
                        nvo.Details.PrintName = (string) DALHelper.HandleDBNull(reader["PrintName"]);
                        nvo.Details.ParamUnit = (long) DALHelper.HandleDBNull(reader["ParameterUnitId"]);
                        nvo.Details.IsNumeric = (bool) DALHelper.HandleDBNull(reader["IsNumeric"]);
                        nvo.Details.Formula = (string) DALHelper.HandleDBNull(reader["Formula"]);
                        nvo.Details.FormulaID = (string) DALHelper.HandleDBNull(reader["FormulaID"]);
                        nvo.Details.TextRange = (string) DALHelper.HandleDBNull(reader["TextRange"]);
                        nvo.Details.stringstrParameterUnitName = (string) DALHelper.HandleDBNull(reader["ParameterUnitName"]);
                        nvo.Details.DeltaCheckPer = Convert.ToDouble(DALHelper.HandleDBNull(reader["DeltaCheckPer"]));
                        nvo.Details.Technique = (string) DALHelper.HandleDBNull(reader["TechniqueUsed"]);
                        nvo.Details.ExcutionCalenderParameterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExcutionCalenderParameterID"]));
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    nvo.Details.DefaultValues = new List<clsPathoParameterDefaultValueMasterVO>();
                    while (reader.Read())
                    {
                        clsPathoParameterDefaultValueMasterVO item = new clsPathoParameterDefaultValueMasterVO {
                            ID = (long) DALHelper.HandleDBNull(reader["DefaultValueID"]),
                            Category = (string) DALHelper.HandleDBNull(reader["Category"]),
                            CategoryID = (long) DALHelper.HandleDBNull(reader["CategoryID"]),
                            IsAge = (bool) DALHelper.HandleDBNull(reader["IsAgeApplicable"]),
                            AgeFrom = (double) DALHelper.HandleDBNull(reader["AgeFrom"]),
                            AgeTo = (double) DALHelper.HandleDBNull(reader["AgeTo"]),
                            MinValue = (double) DALHelper.HandleDBNull(reader["MinValue"]),
                            MaxValue = (double) DALHelper.HandleDBNull(reader["MaxValue"]),
                            DefaultValue = (double) DALHelper.HandleDBNull(reader["DefaultValue"]),
                            MinImprobable = Convert.ToDouble(DALHelper.HandleDBNull(reader["MinImprobable"])),
                            MaxImprobable = Convert.ToDouble(DALHelper.HandleDBNull(reader["MaxImprobable"])),
                            MachineID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MachineID"])),
                            Note = Convert.ToString(DALHelper.HandleDBNull(reader["Note"])),
                            Machine = Convert.ToString(DALHelper.HandleDBNull(reader["Machine"])),
                            IsReflexTesting = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsReflexTesting"])),
                            AgeValue = Convert.ToString(DALHelper.HandleDBNull(reader["AgeValue"])),
                            HighReffValue = Convert.ToDouble(DALHelper.HandleDBNull(reader["HighReffValue"])),
                            LowReffValue = Convert.ToDouble(DALHelper.HandleDBNull(reader["LowReffValue"])),
                            UpperPanicValue = Convert.ToDouble(DALHelper.HandleDBNull(reader["UpperPanicValue"])),
                            LowerPanicValue = Convert.ToDouble(DALHelper.HandleDBNull(reader["LowerPanicValue"])),
                            LowReflexValue = Convert.ToDouble(DALHelper.HandleDBNull(reader["LowReflex"])),
                            HighReflexValue = Convert.ToDouble(DALHelper.HandleDBNull(reader["HighReflex"])),
                            VaryingReferences = Convert.ToString(DALHelper.HandleDBNull(reader["VaryingReferences"]))
                        };
                        nvo.Details.DefaultValues.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    nvo.Details.Items = new List<clsPathoParameterHelpValueMasterVO>();
                    while (reader.Read())
                    {
                        clsPathoParameterHelpValueMasterVO item = new clsPathoParameterHelpValueMasterVO {
                            ID = (long) DALHelper.HandleDBNull(reader["HelpID"]),
                            strHelp = (string) DALHelper.HandleDBNull(reader["HelpValue"]),
                            IsDefault = (bool) DALHelper.HandleDBNull(reader["IsDefault"]),
                            IsAbnoramal = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAbnoramal"]))
                        };
                        nvo.Details.Items.Add(item);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetParametersForList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPathoParameterMasterBizActionVO nvo = (clsGetPathoParameterMasterBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathologyParameterListForSearch");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "Code");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AllParameter", DbType.Boolean, nvo.AllParameter);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ParameterList == null)
                    {
                        nvo.ParameterList = new List<clsPathoParameterMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathoParameterMasterVO item = new clsPathoParameterMasterVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            ParameterDesc = (string) DALHelper.HandleDBNull(reader["Description"]),
                            PrintName = (string) DALHelper.HandleDBNull(reader["PrintName"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            stringstrParameterUnitName = (string) DALHelper.HandleDBNull(reader["ParameterUnitName"])
                        };
                        nvo.ParameterList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPathoFasting(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetPathoFastingBizActionVO nvo = valueObject as clsGetPathoFastingBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoFastingDetails");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, nvo.id);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            IsHrs = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsHrs"])),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.MasterList.Add(item);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetPathologist(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPathologistBizActionVO nvo = (clsGetPathologistBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillPathologistCombobox");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPathoParameter(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPathoParameterUnitBizActionVO nvo = (clsGetPathoParameterUnitBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoParameterUnit");
                this.dbServer.AddInParameter(storedProcCommand, "ParamID", DbType.Int64, nvo.ParamID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["ID"], reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetPathoUsers(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPathoUsersBizActionVO nvo = (clsGetPathoUsersBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoUsers");
                this.dbServer.AddInParameter(storedProcCommand, "MenuID", DbType.Int64, nvo.MenuID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPatientMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientMasterListBizActionVO nvo = valueObject as clsGetPatientMasterListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientMaterList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ComboList == null)
                    {
                        nvo.ComboList = new List<clsComboMasterBizActionVO>();
                    }
                    switch (nvo.IsDecode)
                    {
                        case false:
                            while (reader.Read())
                            {
                                clsComboMasterBizActionVO item = new clsComboMasterBizActionVO {
                                    ID = (long) DALHelper.HandleDBNull(reader["PatientID"]),
                                    Value = (string) DALHelper.HandleDBNull(reader["Value"])
                                };
                                nvo.ComboList.Add(item);
                            }
                            break;

                        case true:
                            while (reader.Read())
                            {
                                clsComboMasterBizActionVO item = new clsComboMasterBizActionVO {
                                    ID = (long) DALHelper.HandleDBNull(reader["PatientID"])
                                };
                                string str = (string) DALHelper.HandleDBNull(reader["Value"]);
                                if (string.IsNullOrEmpty(str))
                                {
                                    str = "";
                                }
                                else
                                {
                                    str = "";
                                    foreach (string str2 in str.Split(new char[] { ' ' }))
                                    {
                                        str = str + Security.base64Decode(str2) + " ";
                                    }
                                }
                                item.Value = str.Trim();
                                nvo.ComboList.Add(item);
                            }
                            break;

                        default:
                            break;
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetRadiologist(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetRadiologistBizActionVO nvo = (clsGetRadiologistBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillRadiologistCombobox");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetRefDoctor(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetRefernceDoctorBizActionVO nvo = (clsGetRefernceDoctorBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillRefDoctor");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsFromVisit", DbType.Boolean, nvo.IsFromVisit);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ComboList == null)
                    {
                        nvo.ComboList = new List<clsComboMasterBizActionVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsComboMasterBizActionVO item = new clsComboMasterBizActionVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Value = (string) DALHelper.HandleDBNull(reader["Value"])
                        };
                        nvo.ComboList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetRoleGeneralDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRoleGeneralDetailsBizActionVO nvo = (clsGetRoleGeneralDetailsBizActionVO) valueObject;
            try
            {
                StringBuilder builder = new StringBuilder();
                if ((nvo.Status != null) && nvo.Status.Value)
                {
                    builder.Append("Status = 'True'");
                }
                else if ((nvo.Status != null) && !nvo.Status.Value)
                {
                    builder.Append("Status = 'False'");
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMasterGeneralDetailsList");
                this.dbServer.AddInParameter(storedProcCommand, "MasterTableName", DbType.String, "T_UserRoleMaster");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "Id");
                this.dbServer.AddInParameter(storedProcCommand, "ExtraParameterList", DbType.String, "");
                this.dbServer.AddInParameter(storedProcCommand, "ExtraParameterDeclarationList", DbType.String, "");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.InputSearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.InputPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.InputStartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.InputMaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.InputSortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.RoleGeneralDetailsList == null)
                    {
                        nvo.RoleGeneralDetailsList = new List<clsUserRoleVO>();
                    }
                    while (reader.Read())
                    {
                        clsUserRoleVO item = new clsUserRoleVO {
                            ID = (long) reader["Id"],
                            Code = (string) reader["Code"],
                            Description = reader["Description"].ToString(),
                            Status = (bool) reader["Status"]
                        };
                        item.IsActive = item.Status;
                        nvo.RoleGeneralDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.OutputTotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject GetRoleList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRoleListBizActionVO nvo = (clsGetRoleListBizActionVO) valueObject;
            try
            {
                StringBuilder builder = new StringBuilder();
                if ((nvo.Status != null) && nvo.Status.Value)
                {
                    builder.Append("Status = 'True'");
                }
                else if ((nvo.Status != null) && !nvo.Status.Value)
                {
                    builder.Append("Status = 'False'");
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMasterRoleList");
                this.dbServer.AddInParameter(storedProcCommand, "MasterTableName", DbType.String, "T_UserRoleMaster");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "Id");
                this.dbServer.AddInParameter(storedProcCommand, "FilterExpression", DbType.String, builder.ToString());
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.RoleList.Add(new MasterListItem((long) reader["ID"], reader["Description"].ToString()));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject GetSelectedRoleMenuId(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetSelectedRoleMenuIdBizActionVO nvo = (clsGetSelectedRoleMenuIdBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSelectedRoleDetails");
                this.dbServer.AddInParameter(storedProcCommand, "RoleId", DbType.Int64, nvo.RoleId);
                using (DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand))
                {
                    if (reader.HasRows)
                    {
                        nvo.MenuList = new List<clsMenuVO>();
                        while (reader.Read())
                        {
                            clsMenuVO item = new clsMenuVO {
                                ID = Convert.ToInt64(reader["MenuId"]),
                                Status = new bool?(Convert.ToBoolean(reader["Status"]))
                            };
                            nvo.MenuList.Add(item);
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        nvo.DashBoardList = new List<clsDashBoardVO>();
                        while (reader.Read())
                        {
                            clsDashBoardVO item = new clsDashBoardVO {
                                ID = (long) DALHelper.HandleDBNull(reader["DashBoardID"]),
                                Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                            };
                            nvo.DashBoardList.Add(item);
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject GetServicesByID(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetPathoServicesByIDBizActionVO nvo = valueObject as clsGetPathoServicesByIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetServiceListForParaDefaultvalue");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.String, nvo.ID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    nvo.ServiceDetailsList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["description"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.ServiceDetailsList.Add(item);
                    }
                }
                reader.NextResult();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetSMSTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetSMSTemplateDetailsBizActionVO nvo = valueObject as clsGetSMSTemplateDetailsBizActionVO;
            try
            {
                clsSMSTemplateVO sMSDetails = nvo.SMSDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSMSTemplateDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.SMSDetails == null)
                        {
                            nvo.SMSDetails = new clsSMSTemplateVO();
                        }
                        nvo.ID = (long) DALHelper.HandleDBNull(reader["Id"]);
                        nvo.SMSDetails.Code = (string) DALHelper.HandleDBNull(reader["Code"]);
                        nvo.SMSDetails.Description = (string) DALHelper.HandleDBNull(reader["Description"]);
                        nvo.SMSDetails.EnglishText = (string) DALHelper.HandleDBNull(reader["EnglishText"]);
                        nvo.SMSDetails.LocalText = (string) DALHelper.HandleDBNull(reader["LocalText"]);
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

        public override IValueObject GetSMSTemplateList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetListSMSTemplateListBizActionVO nvo = (clsGetListSMSTemplateListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSMSTemplateListForSearch");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "Code");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.SMSList == null)
                    {
                        nvo.SMSList = new List<clsSMSTemplateVO>();
                    }
                    while (reader.Read())
                    {
                        clsSMSTemplateVO item = new clsSMSTemplateVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            EnglishText = (string) DALHelper.HandleDBNull(reader["EnglishText"]),
                            LocalText = (string) DALHelper.HandleDBNull(reader["LocalText"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.SMSList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetStoreForComboBox(IValueObject valueObject, clsUserVO UserVo)
        {
            clsFillStoreMasterListBizActionVO nvo = valueObject as clsFillStoreMasterListBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStorListForComboBox");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "StoreType", DbType.Int32, nvo.StoreType);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.StoreMasterDetails == null)
                    {
                        nvo.StoreMasterDetails = new List<clsStoreVO>();
                    }
                    while (reader.Read())
                    {
                        clsStoreVO item = new clsStoreVO {
                            StoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreId"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"])),
                            IsQuarantineStore = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsQuarantineStore"])),
                            ParentStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParentStoreID"]))
                        };
                        nvo.StoreMasterDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GETSupplierList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetMasterListBizActionVO nvo = (clsGetMasterListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSupplierList");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Code"].ToString(), reader["Description"].ToString(), (bool) reader["Status"], (long) reader["StateID"]));
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                nvo.Error = exception1.Message;
            }
            return nvo;
        }

        public override IValueObject GetSurrogactAgencyDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetSurrogactAgencyMasterBizActionVO nvo = valueObject as clsGetSurrogactAgencyMasterBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSurrogactAgency");
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.AgencyDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.AgencyDetailsList == null)
                    {
                        nvo.AgencyDetailsList = new List<clsSurrogateAgencyMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsSurrogateAgencyMasterVO item = new clsSurrogateAgencyMasterVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            ReferralTypeID = (long) DALHelper.HandleDBNull(reader["ReferralTypeID"]),
                            ReferralName = (string) DALHelper.HandleDBNull(reader["ReferralName"]),
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            RegistrationNo = (string) DALHelper.HandleDBNull(reader["RegistrationNo"]),
                            SourceEmail = (string) DALHelper.HandleDBNull(reader["SourceEmail"]),
                            SourceContactNo = (string) DALHelper.HandleDBNull(reader["SourceContactNo"]),
                            SourceAddress = (string) DALHelper.HandleDBNull(reader["SourceAddress"]),
                            AffilatedBy = (string) DALHelper.HandleDBNull(reader["AffilatedBy"]),
                            Affilatedyear = DALHelper.HandleDate(reader["Affilatedyear"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.AgencyDetailsList.Add(item);
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
            return valueObject;
        }

        public override IValueObject GetUnitContactNo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUnitContactNoBizActionVO nvo = valueObject as clsGetUnitContactNoBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUnitContactNo");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID ", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.ContactNo = (string) DALHelper.HandleDBNull(reader["ContactNo"]);
                        nvo.SuccessStatus = true;
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

        public override IValueObject GetUserMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetUserMasterListBizActionVO nvo = valueObject as clsGetUserMasterListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUserMaterList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.String, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    switch (nvo.IsDecode)
                    {
                        case false:
                            while (reader.Read())
                            {
                                MasterListItem item = new MasterListItem {
                                    ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                    Description = (string) DALHelper.HandleDBNull(reader["Value"])
                                };
                                nvo.MasterList.Add(item);
                            }
                            break;

                        case true:
                            while (reader.Read())
                            {
                                MasterListItem item = new MasterListItem {
                                    ID = (long) DALHelper.HandleDBNull(reader["ID"])
                                };
                                string str = (string) DALHelper.HandleDBNull(reader["Value"]);
                                if (!string.IsNullOrEmpty(str))
                                {
                                    str = "";
                                    foreach (string str2 in str.Split(new char[] { ' ' }))
                                    {
                                        str = str + Security.base64Decode(str2) + " ";
                                    }
                                }
                                item.Description = str.Trim();
                                nvo.MasterList.Add(item);
                            }
                            break;

                        default:
                            break;
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject UpdateEmailTemplateStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateEmailTemplateStatusBizActionVO nvo = valueObject as clsUpdateEmailTemplateStatusBizActionVO;
            try
            {
                clsEmailTemplateVO emailTempStatus = new clsEmailTemplateVO();
                emailTempStatus = nvo.EmailTempStatus;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateEmailTemplateStatus");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, emailTempStatus.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, emailTempStatus.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.EmailTempStatus.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateParameterStatus(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdatePathoParameterStatusBizActionVO nvo = valueObject as clsUpdatePathoParameterStatusBizActionVO;
            try
            {
                clsPathoParameterMasterVO pathoParameterStatus = new clsPathoParameterMasterVO();
                pathoParameterStatus = nvo.PathoParameterStatus;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePathologyParameterStatus");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, pathoParameterStatus.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, pathoParameterStatus.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.PathoParameterStatus.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        private void UpdateParameterUserDetails(DbCommand command, long Id, clsUserVO objUserVO)
        {
            this.dbServer.AddInParameter(command, "ID", DbType.Int64, Id);
            this.dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
            this.dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
            this.dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
            this.dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
            this.dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
        }

        public override IValueObject UpdateSMSTemplateStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateTemplatestatusBizActionVO nvo = valueObject as clsUpdateTemplatestatusBizActionVO;
            try
            {
                clsSMSTemplateVO sMSTempStatus = new clsSMSTemplateVO();
                sMSTempStatus = nvo.SMSTempStatus;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateSMSTemplateStatus");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, sMSTempStatus.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, sMSTempStatus.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.SMSTempStatus.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateStatus(IValueObject valueObject, clsUserVO UserVO)
        {
            clsUpdateRoleStatusBizActionVO nvo = valueObject as clsUpdateRoleStatusBizActionVO;
            try
            {
                clsUserRoleVO roleStatus = new clsUserRoleVO();
                roleStatus = nvo.RoleStatus;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateRoleStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, roleStatus.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, roleStatus.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.RoleStatus.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject UpdateStatusSurrogactAgencyDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsSurrogateAgencyMasterVO rvo = new clsSurrogateAgencyMasterVO();
            clsUpdateStatusSurrogactAgencyMasterBizActionVO nvo = valueObject as clsUpdateStatusSurrogactAgencyMasterBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                rvo = nvo.AgencyDetailsList[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateStatusSurrogactAgency");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, rvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, rvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, rvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                nvo.AgencyDetails = null;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject UpdateStausMaster(IValueObject valueObject, clsUserVO userVO)
        {
            clsMasterListVO tvo = new clsMasterListVO();
            clsUpdateStatusMasterBizActionVO nvo = valueObject as clsUpdateStatusMasterBizActionVO;
            try
            {
                tvo = nvo.ItemMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateStatusMaster");
                this.dbServer.AddInParameter(storedProcCommand, "MasterTableName", DbType.String, tvo.MasterTableName);
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, tvo.Id);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, tvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "AddUnitID", DbType.Int64, tvo.AddUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "By", DbType.Int64, tvo.By);
                this.dbServer.AddInParameter(storedProcCommand, "On", DbType.String, tvo.On);
                this.dbServer.AddInParameter(storedProcCommand, "DateTime", DbType.DateTime, tvo.DateTime);
                this.dbServer.AddInParameter(storedProcCommand, "WindowsLoginName", DbType.String, tvo.WindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception exception1)
            {
                if (exception1.Message.Contains("duplicate"))
                {
                    tvo.PrimaryKeyViolationError = true;
                }
                else
                {
                    tvo.GeneralError = true;
                }
            }
            return nvo;
        }
    }
}

