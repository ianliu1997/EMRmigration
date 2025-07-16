using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.Agency;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration.Agency;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{

    public class clsAgencyMasterDAL : clsBaseAgencyMasterDAL
    {

        #region Variables Declaration
        private Database dbServer = null;
        #endregion

        private clsAgencyMasterDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
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

        #region Agency Master



        public override IValueObject AddAgencyMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            //rohinee
            clsAddAgencyMasterBizActionVO BizActionObj = (clsAddAgencyMasterBizActionVO)valueObject;
            clsAgencyMasterVO objVO = BizActionObj.AgencyMasterDetails;
            if (BizActionObj.IsStatusChanged == true)
            {
                DbCommand cmd1 = dbServer.GetStoredProcCommand("CIMS_UpdateAgencyMasterStatus");
                dbServer.AddInParameter(cmd1, "AgencyID", DbType.Int64, objVO.ID);
                dbServer.AddInParameter(cmd1, "Status", DbType.Int64, objVO.Status);
                dbServer.ExecuteNonQuery(cmd1);
            }
            else
            {
                try
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateAgencyMaster");

                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objVO.ID);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "Code", DbType.String, objVO.Code);
                    dbServer.AddInParameter(command, "Description", DbType.String, objVO.Description);

                    dbServer.AddInParameter(command, "AddressLine1", DbType.String, objVO.Address1);
                    dbServer.AddInParameter(command, "AddressLine2", DbType.String, objVO.Address2);
                    dbServer.AddInParameter(command, "AddressLine3", DbType.String, objVO.Address3);



                    dbServer.AddInParameter(command, "City", DbType.String, objVO.City);
                    dbServer.AddInParameter(command, "State", DbType.String, objVO.State);

                    dbServer.AddInParameter(command, "Country", DbType.String, objVO.Country);

                    dbServer.AddInParameter(command, "CityId", DbType.Int64, objVO.CityId);
                    dbServer.AddInParameter(command, "StateId", DbType.Int64, objVO.StateId);
                    dbServer.AddInParameter(command, "CountryId", DbType.Int64, objVO.CountryId);

                    dbServer.AddInParameter(command, "Pincode", DbType.String, objVO.Pincode);

                    dbServer.AddInParameter(command, "ContactPerson1Name", DbType.String, objVO.ContactPerson1Name);
                    dbServer.AddInParameter(command, "ContactPerson1MobileNo", DbType.String, objVO.ContactPerson1MobileNo);
                    dbServer.AddInParameter(command, "ContactPerson1EmailId", DbType.String, objVO.ContactPerson1Email);

                    dbServer.AddInParameter(command, "ContactPerson2Name", DbType.String, objVO.ContactPerson2Name);
                    dbServer.AddInParameter(command, "ContactPerson2MobileNo", DbType.String, objVO.ContactPerson2MobileNo);
                    dbServer.AddInParameter(command, "ContactPerson2EmailId", DbType.String, objVO.ContactPerson2Email);

                    dbServer.AddInParameter(command, "PhoneNo", DbType.String, objVO.PhoneNo);
                    dbServer.AddInParameter(command, "Fax", DbType.String, objVO.Fax);


                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    //  dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                    // dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    // dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                    dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                    int intStatus = dbServer.ExecuteNonQuery(command);

                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    objVO.ID = (long)dbServer.GetParameterValue(command, "ID");

                    DbCommand cmd = dbServer.GetStoredProcCommand("CIMS_DeleteAgencyMasterDetails");
                    dbServer.AddInParameter(cmd, "AgencyID", DbType.Int64, objVO.ID);
                    dbServer.ExecuteNonQuery(cmd);

                    if (objVO.SpecializationList != null && objVO.SpecializationList.Count > 0)
                    {
                        foreach (var item in objVO.SpecializationList)
                        {
                            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddAgencyMasterDetails");
                            dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command1, "AgencyID", DbType.Int64, objVO.ID);
                            dbServer.AddInParameter(command1, "SpecializationID", DbType.Int64, item.SpecializationId);
                            dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, objUserVO.ID);
                            dbServer.AddInParameter(command1, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                            //dbServer.AddParameter(command1, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                            int intStatus1 = dbServer.ExecuteNonQuery(command1);
                            //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                            //objVO.ID = (long)dbServer.GetParameterValue(command1, "ID");
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
            }
            return BizActionObj;


        }
        #endregion

        public override IValueObject GetAgencyMasterList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetAgencyMasterListBizActionVO BizActionObj = (clsGetAgencyMasterListBizActionVO)valueObject;
            clsAgencyMasterVO objVO = BizActionObj.AgencyMasterDetails;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAgencyMasterList");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionObj.AgencyMasterDetails.Description);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                BizActionObj.AgencyMasterList = new List<clsAgencyMasterVO>();
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.AgencyMasterList == null)
                        BizActionObj.AgencyMasterList = new List<clsAgencyMasterVO>();
                    while (reader.Read())
                    {
                        clsAgencyMasterVO AgencyVO = new clsAgencyMasterVO();
                        AgencyVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        AgencyVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        AgencyVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        AgencyVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));

                        AgencyVO.Address1 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine1"]));
                        AgencyVO.Address2 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine2"]));
                        AgencyVO.Address3 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine3"]));

                        AgencyVO.Fax = Convert.ToString(DALHelper.HandleDBNull(reader["Fax"]));

                        AgencyVO.City = Convert.ToString(DALHelper.HandleDBNull(reader["City"]));
                        AgencyVO.State = Convert.ToString(DALHelper.HandleDBNull(reader["State"]));
                        AgencyVO.Country = Convert.ToString(DALHelper.HandleDBNull(reader["Country"]));


                        AgencyVO.Pincode = Convert.ToString(DALHelper.HandleDBNull(reader["Pincode"]));
                        AgencyVO.PhoneNo = Convert.ToString(DALHelper.HandleDBNull(reader["PhoneNo"]));

                        AgencyVO.CountryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CountryId"]));
                        AgencyVO.StateId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StateId"]));
                        AgencyVO.CityId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CityId"]));




                        AgencyVO.ContactPerson1Name = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson1Name"]));
                        AgencyVO.ContactPerson1MobileNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson1MobileNo"]));
                        AgencyVO.ContactPerson1Email = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson1EmailId"]));

                        AgencyVO.ContactPerson2Name = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson2Name"]));
                        AgencyVO.ContactPerson2MobileNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson2MobileNo"]));
                        AgencyVO.ContactPerson2Email = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson2EmailId"]));



                        AgencyVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizActionObj.AgencyMasterList.Add(AgencyVO);
                    }
                }
                reader.NextResult();

                if (objVO.SelectedSpecializationList == null)
                    objVO.SelectedSpecializationList = new List<clsSpecializationVO>();
                while (reader.Read())
                {
                    clsSpecializationVO SpecializationVO = new clsSpecializationVO();
                    SpecializationVO.SpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"]));
                    SpecializationVO.SpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
                    SpecializationVO.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"]));
                    SpecializationVO.ClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                    SpecializationVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                    BizActionObj.AgencyMasterDetails.SelectedSpecializationList.Add(SpecializationVO);
                }

                reader.Close();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
            }
            catch (Exception ex1)
            {
                throw;
            }
            finally
            {

            }
            return BizActionObj;
        }


        public override IValueObject GetClinicMasterList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetClinicMasterListBizActionVO BizActionObj = (clsGetClinicMasterListBizActionVO)valueObject;
            clsAgencyMasterVO objVO = BizActionObj.ClinicDetails;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetClinicMasterList");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objVO.ID);

                BizActionObj.AgencyClinicLinkList = new List<clsAgencyClinicLinkVO>();
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                        BizActionObj.MasterList = new List<MasterListItem>();
                    while (reader.Read())
                    {

                        BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));//HandleDBNull(reader["Date"])));
                    }
                }
                reader.NextResult();

                reader.Close();

            }
            catch (Exception ex1)
            {
                throw;
            }
            finally
            {

            }
            return BizActionObj;
        }

        #region AgencyClinic Linking
        public override IValueObject AddAgencyCliniLink(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbConnection con = null;
            DbTransaction trans = null;

            clsAddAgencyClinicLinkBizActionVO BizActionObj = (clsAddAgencyClinicLinkBizActionVO)valueObject;
            clsAgencyClinicLinkVO objVO = BizActionObj.AgencyClinicLinkDetails;
            List<clsAgencyMasterVO> objList = BizActionObj.AgencyMasterList;
            if (objVO.IsModifyLink == true)
            {
                try
                {
                    DbCommand cmd = dbServer.GetStoredProcCommand("CIMS_DeleteAgencyClinicLinkDetails");
                    dbServer.AddInParameter(cmd, "UnitID", DbType.Int64, objVO.UnitID);
                    dbServer.ExecuteNonQuery(cmd);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            try
            {
                foreach (clsAgencyMasterVO item in objList)
                {
                    con = dbServer.CreateConnection();
                    con.Open();
                    trans = con.BeginTransaction();
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddAgencyClinicLinking");
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AgencyID", DbType.Int64, item.ID);

                    dbServer.AddInParameter(command, "ApplicableUnitID", DbType.Int64, objVO.UnitID);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "IsDefault", DbType.String, item.IsDefault);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, 0);

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    trans.Commit();


                }
            }

            catch (Exception ex)
            {
                trans.Rollback();

            }


            finally
            {

            }
            return BizActionObj;

        }

        public override IValueObject GetAgencyclinicLinkList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetAgencyClinicLinkBizActionVO BizActionObj = (clsGetAgencyClinicLinkBizActionVO)valueObject;
            clsAgencyClinicLinkVO objVO = BizActionObj.AgencyClinicLinkDetails;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAgencyClinicLinkList");
                //  dbServer.AddInParameter(command, "ApplicableUnitID", DbType.Int64, objVO.UnitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objVO.UnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                BizActionObj.AgencyClinicLinkList = new List<clsAgencyClinicLinkVO>();
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.AgencyClinicLinkList == null)
                        BizActionObj.AgencyClinicLinkList = new List<clsAgencyClinicLinkVO>();
                    while (reader.Read())
                    {
                        clsAgencyClinicLinkVO AgencyClinicVO = new clsAgencyClinicLinkVO();
                        AgencyClinicVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        AgencyClinicVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        AgencyClinicVO.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"]));
                        AgencyClinicVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        AgencyClinicVO.ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader["ClinicName"]));
                        AgencyClinicVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        AgencyClinicVO.IsDefault = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDefault"]));
                        AgencyClinicVO.ApplicableUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableUnitID"]));
                        BizActionObj.AgencyClinicLinkList.Add(AgencyClinicVO);

                        clsAgencyMasterVO AgencyVO = new clsAgencyMasterVO();
                        AgencyVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"]));
                        AgencyVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        AgencyVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        AgencyVO.ApplicableUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableUnitID"]));
                        AgencyVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        AgencyVO.IsDefault = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDefault"]));
                        BizActionObj.AgencyMasterList.Add(AgencyVO);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return BizActionObj;
        }


        #endregion
        #region AgencyService Linking
        public override IValueObject GetServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceListBizActionVO BizActionObj = (clsGetServiceListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAgencyServiceList");
                dbServer.AddInParameter(command, "AgencyID", DbType.Int64, BizActionObj.AgencyID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, BizActionObj.SpecializationID);
                dbServer.AddInParameter(command, "SubSpecializationID", DbType.Int64, BizActionObj.SubSpecializationID);
                dbServer.AddInParameter(command, "ServiceName", DbType.String, BizActionObj.ServiceName + "%");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceList == null)
                        BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                    while (reader.Read())
                    {
                        clsServiceMasterVO ServiceDetails = new clsServiceMasterVO();
                        ServiceDetails.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ServiceDetails.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        ServiceDetails.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                        ServiceDetails.Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                        ServiceDetails.SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));
                        BizActionObj.ServiceList.Add(ServiceDetails);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return BizActionObj;
        }

        public override IValueObject GetSelectedServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceListBizActionVO BizActionObj = (clsGetServiceListBizActionVO)valueObject;
            clsServiceMasterVO objVO = BizActionObj.ServiceDetails;
            DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAgencyServiceLink");
            try
            {
                dbServer.AddInParameter(command, "AgencyID", DbType.Int64, objVO.AgencyID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objVO.UnitID);
                BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceList == null)
                        BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                    while (reader.Read())
                    {
                        clsServiceMasterVO ServiceDetails = new clsServiceMasterVO();
                        ServiceDetails.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        ServiceDetails.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        ServiceDetails.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                        ServiceDetails.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        ServiceDetails.IsDefaultAgency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDefaultAgency"]));
                        BizActionObj.ServiceList.Add(ServiceDetails);
                    }
                }
                reader.NextResult();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                    command.Connection.Dispose();
                }
            }
            return BizActionObj;
        }

        public override IValueObject AddServiceAgencyLink(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddServiceAgencyLinkBizActionVO BizActionObj = (clsAddServiceAgencyLinkBizActionVO)valueObject;
            clsAgencyMasterVO objAgencyVO = BizActionObj.objServiceAgencyDetails;
            if (BizActionObj.IsModify == true)
            {
                DbCommand cmd = dbServer.GetStoredProcCommand("CIMS_DeleteAgencyServiceLink");
                try
                {
                    dbServer.AddInParameter(cmd, "AgencyID", DbType.Int64, objAgencyVO.ID);
                    dbServer.AddInParameter(cmd, "UnitID", DbType.Int64, objAgencyVO.ApplicableUnitID);
                    int intStatus1 = dbServer.ExecuteNonQuery(cmd);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                        cmd.Connection.Dispose();
                    }
                }

            }
            DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddAgencySelectedServcieLinking");
            try
            {
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AgencyID", DbType.Int64, objAgencyVO.ID);
                dbServer.AddInParameter(command, "ServiceID", DbType.String, objAgencyVO.ServiceIDList);
                dbServer.AddInParameter(command, "Rate", DbType.String, objAgencyVO.RateList);

                dbServer.AddInParameter(command, "ApplicableUnitID", DbType.Int64, objAgencyVO.ApplicableUnitID);


                dbServer.AddInParameter(command, "Discount", DbType.Decimal, objAgencyVO.Discount);
                dbServer.AddInParameter(command, "IsDefaultAgency", DbType.String, objAgencyVO.IsDefaultList);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
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
            return BizActionObj;
        }


        public override IValueObject GetServiceToAgencyAssigned(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceToAgencyBizActionVO BizActionObj = (clsGetServiceToAgencyBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAgencyServiceLinkingDetails");     //sp done by rohini
                dbServer.AddInParameter(command, "AgencyID", DbType.Int64, BizActionObj.AgencyID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "ServicID", DbType.String, BizActionObj.ServiceID);

                BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceList == null)
                        BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                    while (reader.Read())
                    {
                        clsServiceMasterVO ServiceDetails = new clsServiceMasterVO();
                        BizActionObj.ServiceDetails.AssignedAgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["id"]));
                        //ServiceDetails.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));             
                        //BizActionObj.ServiceDetails.Add(ServiceDetails);
                    }
                }
                reader.NextResult();
                //BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return BizActionObj;
        }

        //for service assigned to agency check before delete
        public override IValueObject GetServiceToAgencyAssignedCheck(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceToAgencyAssignedBizActionVO BizActionObj = (clsGetServiceToAgencyAssignedBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAgencyServiceLinkingCheckDetails");     //sp done by rohini
                dbServer.AddInParameter(command, "AgencyID", DbType.Int64, BizActionObj.AgencyID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                //dbServer.AddInParameter(command, "ServicID", DbType.String, BizActionObj.ServiceID);

                BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceList == null)
                        BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                    while (reader.Read())
                    {
                        clsServiceMasterVO ServiceDetails = new clsServiceMasterVO();
                        BizActionObj.ServiceDetails.AssignedAgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                    }
                }
                reader.NextResult();
                //BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return BizActionObj;
        }
        #endregion


        //added by neena
        public override IValueObject AddAgentMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddAgencyMasterBizActionVO BizActionObj = (clsAddAgencyMasterBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("usp_AgentMaster");
                int status = 0;

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.AgentDetails.ID);
                dbServer.AddInParameter(command, "Action", DbType.String, "SaveUpdateAgent");
                dbServer.AddInParameter(command, "Name", DbType.String, BizActionObj.AgentDetails.Name);
                dbServer.AddInParameter(command, "DOB", DbType.DateTime, BizActionObj.AgentDetails.DOB);
                dbServer.AddInParameter(command, "OccupationID", DbType.Int32, BizActionObj.AgentDetails.OccupationID);
                dbServer.AddInParameter(command, "IsMarried", DbType.Boolean, BizActionObj.AgentDetails.IsMarried);
                dbServer.AddInParameter(command, "YearsOfMerrage", DbType.Int32, BizActionObj.AgentDetails.YearsOfMerrage);
                dbServer.AddInParameter(command, "SpouseName", DbType.String, BizActionObj.AgentDetails.SpouseName);
                dbServer.AddInParameter(command, "SpouseDOB", DbType.DateTime, BizActionObj.AgentDetails.SpouseDOB);
                dbServer.AddInParameter(command, "PrevioulyEggDonation", DbType.Boolean, BizActionObj.AgentDetails.PrevioulyEggDonation);
                dbServer.AddInParameter(command, "PreviousSurogacyDone", DbType.Boolean, BizActionObj.AgentDetails.PreviousSurogacyDone);
                dbServer.AddInParameter(command, "NoofDonationTime", DbType.Int32, BizActionObj.AgentDetails.NoofDonationTime);
                dbServer.AddInParameter(command, "NoofSurogacyDone", DbType.Int32, BizActionObj.AgentDetails.NoofSurogacyDone);
                dbServer.AddInParameter(command, "MobCountryCode", DbType.String, BizActionObj.AgentDetails.MobCountryCode);
                dbServer.AddInParameter(command, "MobileNo", DbType.String, BizActionObj.AgentDetails.MobileNo);
                dbServer.AddInParameter(command, "AltMobCountryCode", DbType.String, BizActionObj.AgentDetails.AltMobCountryCode);
                dbServer.AddInParameter(command, "AltMobileNo", DbType.String, BizActionObj.AgentDetails.AltMobileNo);
                dbServer.AddInParameter(command, "LLAreaCode", DbType.String, BizActionObj.AgentDetails.LLAreaCode);
                dbServer.AddInParameter(command, "LandlineNo", DbType.String, BizActionObj.AgentDetails.LandlineNo);
                dbServer.AddInParameter(command, "AddressLine1", DbType.String, BizActionObj.AgentDetails.AddressLine1);
                dbServer.AddInParameter(command, "AddressLine2", DbType.String, BizActionObj.AgentDetails.AddressLine2);
                dbServer.AddInParameter(command, "Street", DbType.String, BizActionObj.AgentDetails.Street);
                dbServer.AddInParameter(command, "LandMark", DbType.String, BizActionObj.AgentDetails.LandMark);
                dbServer.AddInParameter(command, "CountryID", DbType.Int32, BizActionObj.AgentDetails.CountryID);
                dbServer.AddInParameter(command, "StateID", DbType.Int32, BizActionObj.AgentDetails.StateID);
                dbServer.AddInParameter(command, "CityID", DbType.Int32, BizActionObj.AgentDetails.CityID);
                dbServer.AddInParameter(command, "Area", DbType.String, BizActionObj.AgentDetails.Area);
                dbServer.AddInParameter(command, "Pincode", DbType.String, BizActionObj.AgentDetails.Pincode);
                dbServer.AddInParameter(command, "PanNo", DbType.String, BizActionObj.AgentDetails.PanNo);
                dbServer.AddInParameter(command, "AadharNo", DbType.String, BizActionObj.AgentDetails.AadharNo);
                dbServer.AddInParameter(command, "Status", DbType.Boolean,true);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);               
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);              
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);               
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
              
                int intStatus = dbServer.ExecuteNonQuery(command);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.AgentDetails.ID =Convert.ToInt32(dbServer.GetParameterValue(command, "ID"));



                if (BizActionObj.AgentYearList != null && BizActionObj.AgentYearList.Count > 0)
                {
                    DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("delete from M_AgentMaster_YearClinic where  AgentID=" + BizActionObj.AgentDetails.ID + " and IsDonation=1");
                    int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1);

                    foreach (var item in BizActionObj.AgentYearList)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("usp_AgentMaster");
                        dbServer.AddInParameter(command1, "Action", DbType.String, "SaveClinicYear");
                        dbServer.AddInParameter(command1, "ID", DbType.Int32, BizActionObj.AgentDetails.ID);
                        dbServer.AddInParameter(command1, "YCID", DbType.Int64, item.YCID);
                        dbServer.AddInParameter(command1, "Year", DbType.String, item.Year);
                        dbServer.AddInParameter(command1, "Clinic", DbType.String, item.Clinic);
                        dbServer.AddInParameter(command1, "IsDonation", DbType.Boolean, item.IsDonation);                       
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        //dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, objUserVO.ID);
                        //dbServer.AddInParameter(command1, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                        //dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                       
                        dbServer.AddParameter(command1, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                        int intStatus1 = dbServer.ExecuteNonQuery(command1);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        //objVO.ID = (long)dbServer.GetParameterValue(command1, "ID");
                    }
                }

                if (BizActionObj.AgentYearListSurrogacy != null && BizActionObj.AgentYearListSurrogacy.Count > 0)
                {
                    DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("delete from M_AgentMaster_YearClinic where  AgentID=" + BizActionObj.AgentDetails.ID + " and IsDonation=0");
                    int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1);
                    foreach (var item in BizActionObj.AgentYearListSurrogacy)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("usp_AgentMaster");
                        dbServer.AddInParameter(command1, "Action", DbType.String, "SaveClinicYear");
                        dbServer.AddInParameter(command1, "ID", DbType.Int32, BizActionObj.AgentDetails.ID);
                        dbServer.AddInParameter(command1, "YCID", DbType.Int64, item.YCID);
                        dbServer.AddInParameter(command1, "Year", DbType.String, item.Year);
                        dbServer.AddInParameter(command1, "Clinic", DbType.String, item.Clinic);
                        dbServer.AddInParameter(command1, "IsDonation", DbType.Boolean, item.IsDonation);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        //dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, objUserVO.ID);
                        //dbServer.AddInParameter(command1, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                        //dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                        dbServer.AddParameter(command1, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                        int intStatus1 = dbServer.ExecuteNonQuery(command1);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        //objVO.ID = (long)dbServer.GetParameterValue(command1, "ID");
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

        public override IValueObject GetAgentMasterList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetAgencyMasterListBizActionVO BizActionObj = (clsGetAgencyMasterListBizActionVO)valueObject;         
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("usp_AgentMaster");
                dbServer.AddInParameter(command, "Action", DbType.String, "GetAgentList");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                //dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionObj.AgencyMasterDetails.Description);
                //dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                BizActionObj.AgentDetailsList = new List<AgentVO>();
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.AgentDetailsList == null)
                        BizActionObj.AgentDetailsList = new List<AgentVO>();
                    while (reader.Read())
                    {
                        AgentVO ObjAgentVO = new ValueObjects.Administration.Agency.AgentVO();
                        ObjAgentVO.ID =Convert.ToInt32(DALHelper.HandleDBNull(reader["ID"]));
                        ObjAgentVO.Name =Convert.ToString(DALHelper.HandleDBNull(reader["Name"]));
                        //ObjAgentVO.DOB = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DOB"]));
                        //ObjAgentVO.OccupationID = Convert.ToInt32(DALHelper.HandleDBNull(reader["OccupationID"]));
                        //ObjAgentVO.IsMarried = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsMarried"]));
                        //ObjAgentVO.YearsOfMerrage = Convert.ToInt32(DALHelper.HandleDBNull(reader["YearsOfMerrage"]));
                        ObjAgentVO.SpouseName = Convert.ToString(DALHelper.HandleDBNull(reader["SpouseName"]));
                        //ObjAgentVO.SpouseDOB = Convert.ToDateTime(DALHelper.HandleDBNull(reader["SpouseDOB"]));
                        //ObjAgentVO.PrevioulyEggDonation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PrevioulyEggDonation"]));
                        //ObjAgentVO.PreviousSurogacyDone = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PreviousSurogacyDone"]));
                        //ObjAgentVO.NoofDonationTime = Convert.ToInt32(DALHelper.HandleDBNull(reader["NoofDonationTime"]));
                        //ObjAgentVO.NoofSurogacyDone = Convert.ToInt32(DALHelper.HandleDBNull(reader["NoofSurogacyDone"]));
                        //ObjAgentVO.MobCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobCountryCode"]));
                        ObjAgentVO.MobileNo = Convert.ToString(DALHelper.HandleDBNull(reader["MobileNo"]));
                        //ObjAgentVO.AltMobCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["AltMobCountryCode"]));
                        //ObjAgentVO.AltMobileNo = Convert.ToString(DALHelper.HandleDBNull(reader["AltMobileNo"]));
                        //ObjAgentVO.AddressLine1 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine1"]));
                        //ObjAgentVO.AddressLine2 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine2"]));
                        //ObjAgentVO.Street = Convert.ToString(DALHelper.HandleDBNull(reader["Street"]));
                        //ObjAgentVO.LandMark = Convert.ToString(DALHelper.HandleDBNull(reader["LandMark"]));
                        //ObjAgentVO.CountryID = Convert.ToInt32(DALHelper.HandleDBNull(reader["CountryID"]));
                        //ObjAgentVO.StateID = Convert.ToInt32(DALHelper.HandleDBNull(reader["StateID"]));
                        //ObjAgentVO.CityID = Convert.ToInt32(DALHelper.HandleDBNull(reader["CityID"]));
                        //ObjAgentVO.Area = Convert.ToString(DALHelper.HandleDBNull(reader["Area"]));
                        //ObjAgentVO.Pincode = Convert.ToString(DALHelper.HandleDBNull(reader["Pincode"]));
                        //ObjAgentVO.LLAreaCode = Convert.ToString(DALHelper.HandleDBNull(reader["LLAreaCode"]));
                        //ObjAgentVO.LandlineNo = Convert.ToString(DALHelper.HandleDBNull(reader["LandlineNo"]));
                        //ObjAgentVO.PanNo = Convert.ToString(DALHelper.HandleDBNull(reader["PanNo"]));
                        //ObjAgentVO.AadharNo = Convert.ToString(DALHelper.HandleDBNull(reader["AadharNo"]));
                        ObjAgentVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));                                           
                     
                        //AgencyVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));

                        BizActionObj.AgentDetailsList.Add(ObjAgentVO);
                    }
                }
                reader.NextResult();

               
                reader.Close();
                //BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
            }
            catch (Exception ex1)
            {
                throw;
            }
            finally
            {

            }
            return BizActionObj;
        }

        public override IValueObject GetAgentDetilsByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetAgencyMasterListBizActionVO BizActionObj = (clsGetAgencyMasterListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("usp_AgentMaster");
                dbServer.AddInParameter(command, "Action", DbType.String, "GetAgentByID");
                dbServer.AddInParameter(command, "ID", DbType.Int32, BizActionObj.AgentDetails.ID);            
             
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {                    
                    while (reader.Read())
                    {
                        BizActionObj.AgentDetails.ID = Convert.ToInt32(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.AgentDetails.Name = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        BizActionObj.AgentDetails.DOB = (DateTime?)DALHelper.HandleDate(reader["DOB"]);                      
                        BizActionObj.AgentDetails.OccupationID = Convert.ToInt32(DALHelper.HandleDBNull(reader["OccupationID"]));
                        BizActionObj.AgentDetails.IsMarried = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsMarried"]));
                        BizActionObj.AgentDetails.YearsOfMerrage = Convert.ToInt32(DALHelper.HandleDBNull(reader["YearsOfMerrage"]));
                        BizActionObj.AgentDetails.SpouseName = Convert.ToString(DALHelper.HandleDBNull(reader["SpouseName"]));
                        BizActionObj.AgentDetails.SpouseDOB = (DateTime?)DALHelper.HandleDate(reader["SpouseDOB"]); 
                        BizActionObj.AgentDetails.PrevioulyEggDonation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PrevioulyEggDonation"]));
                        BizActionObj.AgentDetails.PreviousSurogacyDone = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PreviousSurogacyDone"]));
                        BizActionObj.AgentDetails.NoofDonationTime = Convert.ToInt32(DALHelper.HandleDBNull(reader["NoofDonationTime"]));
                        BizActionObj.AgentDetails.NoofSurogacyDone = Convert.ToInt32(DALHelper.HandleDBNull(reader["NoofSurogacyDone"]));
                        BizActionObj.AgentDetails.MobCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobCountryCode"]));
                        BizActionObj.AgentDetails.MobileNo = Convert.ToString(DALHelper.HandleDBNull(reader["MobileNo"]));
                        BizActionObj.AgentDetails.AltMobCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["AltMobCountryCode"]));
                        BizActionObj.AgentDetails.AltMobileNo = Convert.ToString(DALHelper.HandleDBNull(reader["AltMobileNo"]));
                        BizActionObj.AgentDetails.AddressLine1 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine1"]));
                        BizActionObj.AgentDetails.AddressLine2 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine2"]));
                        BizActionObj.AgentDetails.Street = Convert.ToString(DALHelper.HandleDBNull(reader["Street"]));
                        BizActionObj.AgentDetails.LandMark = Convert.ToString(DALHelper.HandleDBNull(reader["LandMark"]));
                        BizActionObj.AgentDetails.CountryID = Convert.ToInt32(DALHelper.HandleDBNull(reader["CountryID"]));
                        BizActionObj.AgentDetails.StateID = Convert.ToInt32(DALHelper.HandleDBNull(reader["StateID"]));
                        BizActionObj.AgentDetails.CityID = Convert.ToInt32(DALHelper.HandleDBNull(reader["CityID"]));
                        BizActionObj.AgentDetails.Area = Convert.ToString(DALHelper.HandleDBNull(reader["Area"]));
                        BizActionObj.AgentDetails.Pincode = Convert.ToString(DALHelper.HandleDBNull(reader["Pincode"]));
                        BizActionObj.AgentDetails.LLAreaCode = Convert.ToString(DALHelper.HandleDBNull(reader["LLAreaCode"]));
                        BizActionObj.AgentDetails.LandlineNo = Convert.ToString(DALHelper.HandleDBNull(reader["LandlineNo"]));
                        BizActionObj.AgentDetails.PanNo = Convert.ToString(DALHelper.HandleDBNull(reader["PanNo"]));
                        BizActionObj.AgentDetails.AadharNo = Convert.ToString(DALHelper.HandleDBNull(reader["AadharNo"]));
                        BizActionObj.AgentDetails.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        //AgencyVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));

                        
                    }
                }
                reader.NextResult();

                if (BizActionObj.AgentYearList == null)
                    BizActionObj.AgentYearList = new List<YearClinic>();
                int cnt = 0;
                while (reader.Read())
                {
                    cnt = cnt + 1;
                    YearClinic Obj = new YearClinic();
                    Obj.SrNo = cnt.ToString();
                    Obj.YCID = Convert.ToInt32(DALHelper.HandleDBNull(reader["YCID"]));
                    Obj.Year = Convert.ToString(DALHelper.HandleDBNull(reader["Year"]));
                    Obj.Clinic = Convert.ToString(DALHelper.HandleDBNull(reader["Clinic"]));
                    Obj.IsDonation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonation"]));
                    BizActionObj.AgentYearList.Add(Obj);
                }

                reader.NextResult();
                if (BizActionObj.AgentYearListSurrogacy == null)
                    BizActionObj.AgentYearListSurrogacy = new List<YearClinic>();
                cnt = 0;
                while (reader.Read())
                {
                    cnt = cnt + 1;
                    YearClinic Obj = new YearClinic();
                    Obj.SrNo = cnt.ToString();
                    Obj.YCID = Convert.ToInt32(DALHelper.HandleDBNull(reader["YCID"]));
                    Obj.Year = Convert.ToString(DALHelper.HandleDBNull(reader["Year"]));
                    Obj.Clinic = Convert.ToString(DALHelper.HandleDBNull(reader["Clinic"]));
                    Obj.IsDonation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonation"]));
                    BizActionObj.AgentYearListSurrogacy.Add(Obj);
                }


                reader.Close();
                //BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
            }
            catch (Exception ex1)
            {
                throw;
            }
            finally
            {

            }
            return BizActionObj;
        }


        public override IValueObject UpdateStatusAgent(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddAgencyMasterBizActionVO bizObject = valueObject as clsAddAgencyMasterBizActionVO;
            try
            {
                AgentVO objVO = bizObject.AgentDetails;
                DbCommand command = null;

                command = dbServer.GetStoredProcCommand("usp_AgentMaster");
                dbServer.AddInParameter(command, "Action", DbType.String, "UpdateStatus");
               
                dbServer.AddInParameter(command, "ID", DbType.Int64, objVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);

                //dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                //dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                //dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                //bizObject.DietStatus.ID = (long)dbServer.GetParameterValue(command, "ID");

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //log error  
                //  logManager.LogInfo(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return bizObject;
        }

        //

    }
}
