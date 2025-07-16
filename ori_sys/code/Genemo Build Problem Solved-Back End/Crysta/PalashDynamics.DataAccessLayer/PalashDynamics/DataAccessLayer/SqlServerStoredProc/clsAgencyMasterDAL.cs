namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration.Agency;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.Administration.Agency;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsAgencyMasterDAL : clsBaseAgencyMasterDAL
    {
        private Database dbServer;

        private clsAgencyMasterDAL()
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

        public override IValueObject AddAgencyCliniLink(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            clsAddAgencyClinicLinkBizActionVO nvo = (clsAddAgencyClinicLinkBizActionVO) valueObject;
            clsAgencyClinicLinkVO agencyClinicLinkDetails = nvo.AgencyClinicLinkDetails;
            List<clsAgencyMasterVO> agencyMasterList = nvo.AgencyMasterList;
            if (agencyClinicLinkDetails.IsModifyLink)
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteAgencyClinicLinkDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, agencyClinicLinkDetails.UnitID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            try
            {
                foreach (clsAgencyMasterVO rvo in agencyMasterList)
                {
                    connection = this.dbServer.CreateConnection();
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddAgencyClinicLinking");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, rvo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "ApplicableUnitID", DbType.Int64, agencyClinicLinkDetails.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "IsDefault", DbType.String, rvo.IsDefault);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            return nvo;
        }

        public override IValueObject AddAgencyMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddAgencyMasterBizActionVO nvo = (clsAddAgencyMasterBizActionVO) valueObject;
            clsAgencyMasterVO agencyMasterDetails = nvo.AgencyMasterDetails;
            if (nvo.IsStatusChanged)
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateAgencyMasterStatus");
                this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, agencyMasterDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Int64, agencyMasterDetails.Status);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            else
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateAgencyMaster");
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, agencyMasterDetails.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, agencyMasterDetails.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, agencyMasterDetails.Code);
                    this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, agencyMasterDetails.Description);
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, agencyMasterDetails.Address1);
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine2", DbType.String, agencyMasterDetails.Address2);
                    this.dbServer.AddInParameter(storedProcCommand, "AddressLine3", DbType.String, agencyMasterDetails.Address3);
                    this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, agencyMasterDetails.City);
                    this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, agencyMasterDetails.State);
                    this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, agencyMasterDetails.Country);
                    this.dbServer.AddInParameter(storedProcCommand, "CityId", DbType.Int64, agencyMasterDetails.CityId);
                    this.dbServer.AddInParameter(storedProcCommand, "StateId", DbType.Int64, agencyMasterDetails.StateId);
                    this.dbServer.AddInParameter(storedProcCommand, "CountryId", DbType.Int64, agencyMasterDetails.CountryId);
                    this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, agencyMasterDetails.Pincode);
                    this.dbServer.AddInParameter(storedProcCommand, "ContactPerson1Name", DbType.String, agencyMasterDetails.ContactPerson1Name);
                    this.dbServer.AddInParameter(storedProcCommand, "ContactPerson1MobileNo", DbType.String, agencyMasterDetails.ContactPerson1MobileNo);
                    this.dbServer.AddInParameter(storedProcCommand, "ContactPerson1EmailId", DbType.String, agencyMasterDetails.ContactPerson1Email);
                    this.dbServer.AddInParameter(storedProcCommand, "ContactPerson2Name", DbType.String, agencyMasterDetails.ContactPerson2Name);
                    this.dbServer.AddInParameter(storedProcCommand, "ContactPerson2MobileNo", DbType.String, agencyMasterDetails.ContactPerson2MobileNo);
                    this.dbServer.AddInParameter(storedProcCommand, "ContactPerson2EmailId", DbType.String, agencyMasterDetails.ContactPerson2Email);
                    this.dbServer.AddInParameter(storedProcCommand, "PhoneNo", DbType.String, agencyMasterDetails.PhoneNo);
                    this.dbServer.AddInParameter(storedProcCommand, "Fax", DbType.String, agencyMasterDetails.Fax);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    agencyMasterDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_DeleteAgencyMasterDetails");
                    this.dbServer.AddInParameter(command, "AgencyID", DbType.Int64, agencyMasterDetails.ID);
                    this.dbServer.ExecuteNonQuery(command);
                    if ((agencyMasterDetails.SpecializationList != null) && (agencyMasterDetails.SpecializationList.Count > 0))
                    {
                        foreach (clsSpecializationVO nvo2 in agencyMasterDetails.SpecializationList)
                        {
                            DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddAgencyMasterDetails");
                            this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command4, "AgencyID", DbType.Int64, agencyMasterDetails.ID);
                            this.dbServer.AddInParameter(command4, "SpecializationID", DbType.Int64, nvo2.SpecializationId);
                            this.dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, objUserVO.ID);
                            this.dbServer.AddInParameter(command4, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command4, "ID", DbType.Int64, 0);
                            this.dbServer.ExecuteNonQuery(command4);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return nvo;
        }

        public override IValueObject AddAgentMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddAgencyMasterBizActionVO nvo = (clsAddAgencyMasterBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("usp_AgentMaster");
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.AgentDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Action", DbType.String, "SaveUpdateAgent");
                this.dbServer.AddInParameter(storedProcCommand, "Name", DbType.String, nvo.AgentDetails.Name);
                this.dbServer.AddInParameter(storedProcCommand, "DOB", DbType.DateTime, nvo.AgentDetails.DOB);
                this.dbServer.AddInParameter(storedProcCommand, "OccupationID", DbType.Int32, nvo.AgentDetails.OccupationID);
                this.dbServer.AddInParameter(storedProcCommand, "IsMarried", DbType.Boolean, nvo.AgentDetails.IsMarried);
                this.dbServer.AddInParameter(storedProcCommand, "YearsOfMerrage", DbType.Int32, nvo.AgentDetails.YearsOfMerrage);
                this.dbServer.AddInParameter(storedProcCommand, "SpouseName", DbType.String, nvo.AgentDetails.SpouseName);
                this.dbServer.AddInParameter(storedProcCommand, "SpouseDOB", DbType.DateTime, nvo.AgentDetails.SpouseDOB);
                this.dbServer.AddInParameter(storedProcCommand, "PrevioulyEggDonation", DbType.Boolean, nvo.AgentDetails.PrevioulyEggDonation);
                this.dbServer.AddInParameter(storedProcCommand, "PreviousSurogacyDone", DbType.Boolean, nvo.AgentDetails.PreviousSurogacyDone);
                this.dbServer.AddInParameter(storedProcCommand, "NoofDonationTime", DbType.Int32, nvo.AgentDetails.NoofDonationTime);
                this.dbServer.AddInParameter(storedProcCommand, "NoofSurogacyDone", DbType.Int32, nvo.AgentDetails.NoofSurogacyDone);
                this.dbServer.AddInParameter(storedProcCommand, "MobCountryCode", DbType.String, nvo.AgentDetails.MobCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "MobileNo", DbType.String, nvo.AgentDetails.MobileNo);
                this.dbServer.AddInParameter(storedProcCommand, "AltMobCountryCode", DbType.String, nvo.AgentDetails.AltMobCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "AltMobileNo", DbType.String, nvo.AgentDetails.AltMobileNo);
                this.dbServer.AddInParameter(storedProcCommand, "LLAreaCode", DbType.String, nvo.AgentDetails.LLAreaCode);
                this.dbServer.AddInParameter(storedProcCommand, "LandlineNo", DbType.String, nvo.AgentDetails.LandlineNo);
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, nvo.AgentDetails.AddressLine1);
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine2", DbType.String, nvo.AgentDetails.AddressLine2);
                this.dbServer.AddInParameter(storedProcCommand, "Street", DbType.String, nvo.AgentDetails.Street);
                this.dbServer.AddInParameter(storedProcCommand, "LandMark", DbType.String, nvo.AgentDetails.LandMark);
                this.dbServer.AddInParameter(storedProcCommand, "CountryID", DbType.Int32, nvo.AgentDetails.CountryID);
                this.dbServer.AddInParameter(storedProcCommand, "StateID", DbType.Int32, nvo.AgentDetails.StateID);
                this.dbServer.AddInParameter(storedProcCommand, "CityID", DbType.Int32, nvo.AgentDetails.CityID);
                this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, nvo.AgentDetails.Area);
                this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, nvo.AgentDetails.Pincode);
                this.dbServer.AddInParameter(storedProcCommand, "PanNo", DbType.String, nvo.AgentDetails.PanNo);
                this.dbServer.AddInParameter(storedProcCommand, "AadharNo", DbType.String, nvo.AgentDetails.AadharNo);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.AgentDetails.ID = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                if ((nvo.AgentYearList != null) && (nvo.AgentYearList.Count > 0))
                {
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand("delete from M_AgentMaster_YearClinic where  AgentID=" + nvo.AgentDetails.ID + " and IsDonation=1");
                    this.dbServer.ExecuteNonQuery(sqlStringCommand);
                    foreach (YearClinic clinic in nvo.AgentYearList)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("usp_AgentMaster");
                        this.dbServer.AddInParameter(command3, "Action", DbType.String, "SaveClinicYear");
                        this.dbServer.AddInParameter(command3, "ID", DbType.Int32, nvo.AgentDetails.ID);
                        this.dbServer.AddInParameter(command3, "YCID", DbType.Int64, clinic.YCID);
                        this.dbServer.AddInParameter(command3, "Year", DbType.String, clinic.Year);
                        this.dbServer.AddInParameter(command3, "Clinic", DbType.String, clinic.Clinic);
                        this.dbServer.AddInParameter(command3, "IsDonation", DbType.Boolean, clinic.IsDonation);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        this.dbServer.AddParameter(command3, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                        this.dbServer.ExecuteNonQuery(command3);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                    }
                }
                if ((nvo.AgentYearListSurrogacy != null) && (nvo.AgentYearListSurrogacy.Count > 0))
                {
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand("delete from M_AgentMaster_YearClinic where  AgentID=" + nvo.AgentDetails.ID + " and IsDonation=0");
                    this.dbServer.ExecuteNonQuery(sqlStringCommand);
                    foreach (YearClinic clinic2 in nvo.AgentYearListSurrogacy)
                    {
                        DbCommand command5 = this.dbServer.GetStoredProcCommand("usp_AgentMaster");
                        this.dbServer.AddInParameter(command5, "Action", DbType.String, "SaveClinicYear");
                        this.dbServer.AddInParameter(command5, "ID", DbType.Int32, nvo.AgentDetails.ID);
                        this.dbServer.AddInParameter(command5, "YCID", DbType.Int64, clinic2.YCID);
                        this.dbServer.AddInParameter(command5, "Year", DbType.String, clinic2.Year);
                        this.dbServer.AddInParameter(command5, "Clinic", DbType.String, clinic2.Clinic);
                        this.dbServer.AddInParameter(command5, "IsDonation", DbType.Boolean, clinic2.IsDonation);
                        this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        this.dbServer.AddParameter(command5, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                        this.dbServer.ExecuteNonQuery(command5);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command5, "ResultStatus");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddServiceAgencyLink(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddServiceAgencyLinkBizActionVO nvo = (clsAddServiceAgencyLinkBizActionVO) valueObject;
            clsAgencyMasterVO objServiceAgencyDetails = nvo.objServiceAgencyDetails;
            if (nvo.IsModify)
            {
                DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_DeleteAgencyServiceLink");
                try
                {
                    this.dbServer.AddInParameter(command, "AgencyID", DbType.Int64, objServiceAgencyDetails.ID);
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, objServiceAgencyDetails.ApplicableUnitID);
                    this.dbServer.ExecuteNonQuery(command);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                        command.Connection.Dispose();
                    }
                }
            }
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddAgencySelectedServcieLinking");
            try
            {
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, objServiceAgencyDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.String, objServiceAgencyDetails.ServiceIDList);
                this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.String, objServiceAgencyDetails.RateList);
                this.dbServer.AddInParameter(storedProcCommand, "ApplicableUnitID", DbType.Int64, objServiceAgencyDetails.ApplicableUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Discount", DbType.Decimal, objServiceAgencyDetails.Discount);
                this.dbServer.AddInParameter(storedProcCommand, "IsDefaultAgency", DbType.String, objServiceAgencyDetails.IsDefaultList);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (storedProcCommand != null)
                {
                    storedProcCommand.Dispose();
                    storedProcCommand.Connection.Dispose();
                }
            }
            return nvo;
        }

        public override IValueObject GetAgencyclinicLinkList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetAgencyClinicLinkBizActionVO nvo = (clsGetAgencyClinicLinkBizActionVO) valueObject;
            clsAgencyClinicLinkVO agencyClinicLinkDetails = nvo.AgencyClinicLinkDetails;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAgencyClinicLinkList");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, agencyClinicLinkDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                nvo.AgencyClinicLinkList = new List<clsAgencyClinicLinkVO>();
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.AgencyClinicLinkList == null)
                    {
                        nvo.AgencyClinicLinkList = new List<clsAgencyClinicLinkVO>();
                    }
                    while (reader.Read())
                    {
                        clsAgencyClinicLinkVO item = new clsAgencyClinicLinkVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader["ClinicName"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            IsDefault = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDefault"])),
                            ApplicableUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableUnitID"]))
                        };
                        nvo.AgencyClinicLinkList.Add(item);
                        clsAgencyMasterVO rvo = new clsAgencyMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            ApplicableUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableUnitID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            IsDefault = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDefault"]))
                        };
                        nvo.AgencyMasterList.Add(rvo);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetAgencyMasterList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetAgencyMasterListBizActionVO nvo = (clsGetAgencyMasterListBizActionVO) valueObject;
            clsAgencyMasterVO agencyMasterDetails = nvo.AgencyMasterDetails;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAgencyMasterList");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.AgencyMasterDetails.Description);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                nvo.AgencyMasterList = new List<clsAgencyMasterVO>();
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.AgencyMasterList == null)
                    {
                        nvo.AgencyMasterList = new List<clsAgencyMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsAgencyMasterVO item = new clsAgencyMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Address1 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine1"])),
                            Address2 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine2"])),
                            Address3 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine3"])),
                            Fax = Convert.ToString(DALHelper.HandleDBNull(reader["Fax"])),
                            City = Convert.ToString(DALHelper.HandleDBNull(reader["City"])),
                            State = Convert.ToString(DALHelper.HandleDBNull(reader["State"])),
                            Country = Convert.ToString(DALHelper.HandleDBNull(reader["Country"])),
                            Pincode = Convert.ToString(DALHelper.HandleDBNull(reader["Pincode"])),
                            PhoneNo = Convert.ToString(DALHelper.HandleDBNull(reader["PhoneNo"])),
                            CountryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CountryId"])),
                            StateId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StateId"])),
                            CityId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CityId"])),
                            ContactPerson1Name = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson1Name"])),
                            ContactPerson1MobileNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson1MobileNo"])),
                            ContactPerson1Email = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson1EmailId"])),
                            ContactPerson2Name = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson2Name"])),
                            ContactPerson2MobileNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson2MobileNo"])),
                            ContactPerson2Email = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson2EmailId"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.AgencyMasterList.Add(item);
                    }
                }
                reader.NextResult();
                if (agencyMasterDetails.SelectedSpecializationList == null)
                {
                    agencyMasterDetails.SelectedSpecializationList = new List<clsSpecializationVO>();
                }
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.Close();
                        nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                        break;
                    }
                    clsSpecializationVO item = new clsSpecializationVO {
                        SpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"])),
                        SpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"])),
                        AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"])),
                        ClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                        Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                    };
                    nvo.AgencyMasterDetails.SelectedSpecializationList.Add(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAgentDetilsByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetAgencyMasterListBizActionVO nvo = (clsGetAgencyMasterListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("usp_AgentMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Action", DbType.String, "GetAgentByID");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int32, nvo.AgentDetails.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.AgentDetails.ID = Convert.ToInt32(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.AgentDetails.Name = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        nvo.AgentDetails.DOB = DALHelper.HandleDate(reader["DOB"]);
                        nvo.AgentDetails.OccupationID = Convert.ToInt32(DALHelper.HandleDBNull(reader["OccupationID"]));
                        nvo.AgentDetails.IsMarried = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsMarried"]));
                        nvo.AgentDetails.YearsOfMerrage = Convert.ToInt32(DALHelper.HandleDBNull(reader["YearsOfMerrage"]));
                        nvo.AgentDetails.SpouseName = Convert.ToString(DALHelper.HandleDBNull(reader["SpouseName"]));
                        nvo.AgentDetails.SpouseDOB = DALHelper.HandleDate(reader["SpouseDOB"]);
                        nvo.AgentDetails.PrevioulyEggDonation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PrevioulyEggDonation"]));
                        nvo.AgentDetails.PreviousSurogacyDone = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PreviousSurogacyDone"]));
                        nvo.AgentDetails.NoofDonationTime = Convert.ToInt32(DALHelper.HandleDBNull(reader["NoofDonationTime"]));
                        nvo.AgentDetails.NoofSurogacyDone = Convert.ToInt32(DALHelper.HandleDBNull(reader["NoofSurogacyDone"]));
                        nvo.AgentDetails.MobCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobCountryCode"]));
                        nvo.AgentDetails.MobileNo = Convert.ToString(DALHelper.HandleDBNull(reader["MobileNo"]));
                        nvo.AgentDetails.AltMobCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["AltMobCountryCode"]));
                        nvo.AgentDetails.AltMobileNo = Convert.ToString(DALHelper.HandleDBNull(reader["AltMobileNo"]));
                        nvo.AgentDetails.AddressLine1 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine1"]));
                        nvo.AgentDetails.AddressLine2 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine2"]));
                        nvo.AgentDetails.Street = Convert.ToString(DALHelper.HandleDBNull(reader["Street"]));
                        nvo.AgentDetails.LandMark = Convert.ToString(DALHelper.HandleDBNull(reader["LandMark"]));
                        nvo.AgentDetails.CountryID = Convert.ToInt32(DALHelper.HandleDBNull(reader["CountryID"]));
                        nvo.AgentDetails.StateID = Convert.ToInt32(DALHelper.HandleDBNull(reader["StateID"]));
                        nvo.AgentDetails.CityID = Convert.ToInt32(DALHelper.HandleDBNull(reader["CityID"]));
                        nvo.AgentDetails.Area = Convert.ToString(DALHelper.HandleDBNull(reader["Area"]));
                        nvo.AgentDetails.Pincode = Convert.ToString(DALHelper.HandleDBNull(reader["Pincode"]));
                        nvo.AgentDetails.LLAreaCode = Convert.ToString(DALHelper.HandleDBNull(reader["LLAreaCode"]));
                        nvo.AgentDetails.LandlineNo = Convert.ToString(DALHelper.HandleDBNull(reader["LandlineNo"]));
                        nvo.AgentDetails.PanNo = Convert.ToString(DALHelper.HandleDBNull(reader["PanNo"]));
                        nvo.AgentDetails.AadharNo = Convert.ToString(DALHelper.HandleDBNull(reader["AadharNo"]));
                        nvo.AgentDetails.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                    }
                }
                reader.NextResult();
                if (nvo.AgentYearList == null)
                {
                    nvo.AgentYearList = new List<YearClinic>();
                }
                int num = 0;
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.NextResult();
                        if (nvo.AgentYearListSurrogacy == null)
                        {
                            nvo.AgentYearListSurrogacy = new List<YearClinic>();
                        }
                        num = 0;
                        while (true)
                        {
                            if (!reader.Read())
                            {
                                reader.Close();
                                break;
                            }
                            num++;
                            YearClinic clinic2 = new YearClinic {
                                SrNo = num.ToString(),
                                YCID = Convert.ToInt32(DALHelper.HandleDBNull(reader["YCID"])),
                                Year = Convert.ToString(DALHelper.HandleDBNull(reader["Year"])),
                                Clinic = Convert.ToString(DALHelper.HandleDBNull(reader["Clinic"])),
                                IsDonation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonation"]))
                            };
                            nvo.AgentYearListSurrogacy.Add(clinic2);
                        }
                        break;
                    }
                    num++;
                    YearClinic item = new YearClinic {
                        SrNo = num.ToString(),
                        YCID = Convert.ToInt32(DALHelper.HandleDBNull(reader["YCID"])),
                        Year = Convert.ToString(DALHelper.HandleDBNull(reader["Year"])),
                        Clinic = Convert.ToString(DALHelper.HandleDBNull(reader["Clinic"])),
                        IsDonation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonation"]))
                    };
                    nvo.AgentYearList.Add(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAgentMasterList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetAgencyMasterListBizActionVO nvo = (clsGetAgencyMasterListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("usp_AgentMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Action", DbType.String, "GetAgentList");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                nvo.AgentDetailsList = new List<AgentVO>();
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.AgentDetailsList == null)
                    {
                        nvo.AgentDetailsList = new List<AgentVO>();
                    }
                    while (reader.Read())
                    {
                        AgentVO item = new AgentVO {
                            ID = Convert.ToInt32(DALHelper.HandleDBNull(reader["ID"])),
                            Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"])),
                            SpouseName = Convert.ToString(DALHelper.HandleDBNull(reader["SpouseName"])),
                            MobileNo = Convert.ToString(DALHelper.HandleDBNull(reader["MobileNo"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.AgentDetailsList.Add(item);
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

        public override IValueObject GetClinicMasterList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetClinicMasterListBizActionVO nvo = (clsGetClinicMasterListBizActionVO) valueObject;
            clsAgencyMasterVO clinicDetails = nvo.ClinicDetails;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetClinicMasterList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, clinicDetails.ID);
                nvo.AgencyClinicLinkList = new List<clsAgencyClinicLinkVO>();
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
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetSelectedServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceListBizActionVO nvo = (clsGetServiceListBizActionVO) valueObject;
            clsServiceMasterVO serviceDetails = nvo.ServiceDetails;
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAgencyServiceLink");
            try
            {
                this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, serviceDetails.AgencyID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, serviceDetails.UnitID);
                nvo.ServiceList = new List<clsServiceMasterVO>();
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceList == null)
                    {
                        nvo.ServiceList = new List<clsServiceMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            IsDefaultAgency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDefaultAgency"]))
                        };
                        nvo.ServiceList.Add(item);
                    }
                }
                reader.NextResult();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (storedProcCommand != null)
                {
                    storedProcCommand.Dispose();
                    storedProcCommand.Connection.Dispose();
                }
            }
            return nvo;
        }

        public override IValueObject GetServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceListBizActionVO nvo = (clsGetServiceListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAgencyServiceList");
                this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, nvo.AgencyID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SpecializationID", DbType.Int64, nvo.SpecializationID);
                this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationID", DbType.Int64, nvo.SubSpecializationID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceName", DbType.String, nvo.ServiceName + "%");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                nvo.ServiceList = new List<clsServiceMasterVO>();
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceList == null)
                    {
                        nvo.ServiceList = new List<clsServiceMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])),
                            Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"])),
                            SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]))
                        };
                        nvo.ServiceList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetServiceToAgencyAssigned(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceToAgencyBizActionVO nvo = (clsGetServiceToAgencyBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAgencyServiceLinkingDetails");
                this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, nvo.AgencyID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ServicID", DbType.String, nvo.ServiceID);
                nvo.ServiceList = new List<clsServiceMasterVO>();
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceList == null)
                    {
                        nvo.ServiceList = new List<clsServiceMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsServiceMasterVO rvo1 = new clsServiceMasterVO();
                        nvo.ServiceDetails.AssignedAgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["id"]));
                    }
                }
                reader.NextResult();
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetServiceToAgencyAssignedCheck(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceToAgencyAssignedBizActionVO nvo = (clsGetServiceToAgencyAssignedBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAgencyServiceLinkingCheckDetails");
                this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, nvo.AgencyID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                nvo.ServiceList = new List<clsServiceMasterVO>();
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceList == null)
                    {
                        nvo.ServiceList = new List<clsServiceMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsServiceMasterVO rvo1 = new clsServiceMasterVO();
                        nvo.ServiceDetails.AssignedAgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                    }
                }
                reader.NextResult();
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject UpdateStatusAgent(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddAgencyMasterBizActionVO nvo = valueObject as clsAddAgencyMasterBizActionVO;
            try
            {
                AgentVO agentDetails = nvo.AgentDetails;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("usp_AgentMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Action", DbType.String, "UpdateStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, agentDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, agentDetails.Status);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

