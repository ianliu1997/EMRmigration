namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.DashBoardVO;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    internal class cls_NewSpremThawingDAL : cls_NewBaseSpremThawingDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private cls_NewSpremThawingDAL()
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

        public override IValueObject GetSpermFrezingDetailsForThawing(IValueObject valueObject, clsUserVO UserVO)
        {
            cls_GetSpremFreezingDetilsForThawingBizActionVO nvo = valueObject as cls_GetSpremFreezingDetilsForThawingBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            nvo.SpremThawingDetailsList = new List<cls_NewThawingDetailsVO>();
            try
            {
                DbDataReader reader;
                connection.Open();
                if ((nvo.MalePatientID <= 0L) || (nvo.MalePatientUnitID <= 0L))
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSpermFreezingDetailsForThawing");
                    this.dbServer.AddInParameter(storedProcCommand, "FreezingID", DbType.Int64, nvo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "FreezingUnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "SpremNo", DbType.Int64, nvo.SpremFreezingDetailsVO.SpremNo);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    nvo.SpremFreezingDetailsVO = new clsNew_SpremFreezingVO();
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSpermFreezingDetailsForThawingForASD");
                    this.dbServer.AddInParameter(storedProcCommand, "MalePatientID", DbType.Int64, nvo.MalePatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "MalePatientUnitID", DbType.Int64, nvo.MalePatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.PlanTherapyID);
                    this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.PlanTherapyUnitID);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    nvo.SpremFreezingDetailsVO = new clsNew_SpremFreezingVO();
                }
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsNew_SpremFreezingVO item = new clsNew_SpremFreezingVO {
                            SpremNostr = Convert.ToString(DALHelper.HandleDBNull(reader["ID"])),
                            Volume = Convert.ToDouble(DALHelper.HandleDBNull(reader["Volume"])),
                            SpermCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermCount"])),
                            Motility = Convert.ToDouble(DALHelper.HandleDBNull(reader["Motility"])),
                            GobletColor = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"])),
                            Cane = Convert.ToString(DALHelper.HandleDBNull(reader["Cane"])),
                            Canister = Convert.ToString(DALHelper.HandleDBNull(reader["Canister"])),
                            Straw = Convert.ToString(DALHelper.HandleDBNull(reader["Straw"])),
                            Tank = Convert.ToString(DALHelper.HandleDBNull(reader["Tank"])),
                            GolbletShape = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShape"])),
                            GobletSize = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSize"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            strCollectionMethod = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionMethod"])),
                            CollectionMethod = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionMethodID"])),
                            VitrificationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["SpremFreezingDate"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]))
                        };
                        nvo.SpremFreezingDetails.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cls_NewThawingDetailsVO item = new cls_NewThawingDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ThawingDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawingDate"]))),
                            ThawingTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawingTime"]))),
                            SpremNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermFreezingDetailsID"])),
                            LabPersonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabPersonId"])),
                            PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"])),
                            PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"])),
                            IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])),
                            SemenWashID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SemenWashID"])),
                            Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"])),
                            Volume = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Volume"])),
                            TotalSpearmCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermCount"])),
                            Motility = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Motility"])),
                            DonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorID"])),
                            DonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorUnitID"])),
                            DonorName = Convert.ToString(DALHelper.HandleDBNull(reader["DonorName"])),
                            DonorMrno = Convert.ToString(DALHelper.HandleDBNull(reader["DonorMRNO"]))
                        };
                        nvo.SpremThawingDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.IUIID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IUIID"]));
                    }
                }
            }
            catch (Exception exception)
            {
                connection.Close();
                throw exception;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject GetSpermFrezingDetailsForThawingView(IValueObject valueObject, clsUserVO UserVO)
        {
            cls_GetSpremFreezingDetilsForThawingBizActionVO nvo = valueObject as cls_GetSpremFreezingDetilsForThawingBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            nvo.SpremThawingDetailsList = new List<cls_NewThawingDetailsVO>();
            try
            {
                DbDataReader reader;
                connection.Open();
                if ((nvo.MalePatientID <= 0L) || (nvo.MalePatientUnitID <= 0L))
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSpermFreezingDetailsForThawing");
                    this.dbServer.AddInParameter(storedProcCommand, "FreezingID", DbType.Int64, nvo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "FreezingUnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "SpremNo", DbType.Int64, nvo.SpremFreezingDetailsVO.SpremNo);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    nvo.SpremFreezingDetailsVO = new clsNew_SpremFreezingVO();
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSpermFreezingDetailsForThawingForView");
                    this.dbServer.AddInParameter(storedProcCommand, "MalePatientID", DbType.Int64, nvo.MalePatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "MalePatientUnitID", DbType.Int64, nvo.MalePatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.PlanTherapyID);
                    this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "SemenWashID", DbType.Int64, nvo.SemenWashID);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    nvo.SpremFreezingDetailsVO = new clsNew_SpremFreezingVO();
                }
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsNew_SpremFreezingVO item = new clsNew_SpremFreezingVO {
                            SpremNostr = Convert.ToString(DALHelper.HandleDBNull(reader["ID"])),
                            Volume = Convert.ToDouble(DALHelper.HandleDBNull(reader["Volume"])),
                            SpermCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["TotalSpremCount"])),
                            Motility = Convert.ToDouble(DALHelper.HandleDBNull(reader["Motility"])),
                            GobletColor = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"])),
                            Cane = Convert.ToString(DALHelper.HandleDBNull(reader["Cane"])),
                            Canister = Convert.ToString(DALHelper.HandleDBNull(reader["Canister"])),
                            Straw = Convert.ToString(DALHelper.HandleDBNull(reader["Straw"])),
                            Tank = Convert.ToString(DALHelper.HandleDBNull(reader["Tank"])),
                            GolbletShape = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShape"])),
                            GobletSize = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSize"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            strCollectionMethod = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionMethod"])),
                            CollectionMethod = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionMethodID"])),
                            VitrificationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["SpremFreezingDate"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]))
                        };
                        nvo.SpremFreezingDetails.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cls_NewThawingDetailsVO item = new cls_NewThawingDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ThawingDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawingDate"]))),
                            ThawingTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawingTime"]))),
                            SpremNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermFreezingDetailsID"])),
                            LabPersonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabPersonId"])),
                            PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"])),
                            PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"])),
                            IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])),
                            SemenWashID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SemenWashID"]))
                        };
                        nvo.SpremThawingDetailsList.Add(item);
                    }
                }
            }
            catch (Exception exception)
            {
                connection.Close();
                throw exception;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject GetSpremFreezingforThawingNew(IValueObject ValueObject, clsUserVO UserVO)
        {
            cls_NewGetSpremThawingBizActionVO nvo = ValueObject as cls_NewGetSpremThawingBizActionVO;
            nvo.SpremFreezingDetailsVO = new clsNew_SpremFreezingVO();
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSpremThawing_New");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.MalePatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.String, nvo.MalePatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsNew_SpremFreezingVO item = new clsNew_SpremFreezingVO {
                            SpremNostr = Convert.ToString(DALHelper.HandleDBNull(reader["ID"])),
                            Volume = Convert.ToDouble(DALHelper.HandleDBNull(reader["Volume"])),
                            SpermCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["TotalSpremCount"])),
                            Motility = Convert.ToDouble(DALHelper.HandleDBNull(reader["Motility"])),
                            GobletColor = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"])),
                            Cane = Convert.ToString(DALHelper.HandleDBNull(reader["Cane"])),
                            Canister = Convert.ToString(DALHelper.HandleDBNull(reader["Canister"])),
                            Straw = Convert.ToString(DALHelper.HandleDBNull(reader["Straw"])),
                            Tank = Convert.ToString(DALHelper.HandleDBNull(reader["Tank"])),
                            GolbletShape = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShape"])),
                            GobletSize = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSize"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            strCollectionMethod = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionMethod"])),
                            CollectionMethod = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionMethodID"])),
                            VitrificationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["SpremFreezingDate"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]))
                        };
                        nvo.SpremFreezingDetails.Add(item);
                    }
                }
            }
            catch (Exception exception)
            {
                connection.Close();
                throw exception;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject GetTesaPesaForCode(IValueObject valueObject, clsUserVO UserVO)
        {
            cls_GetSpremFreezingDetilsForThawingBizActionVO nvo = valueObject as cls_GetSpremFreezingDetilsForThawingBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientEMRDataTemplate");
                this.dbServer.AddInParameter(storedProcCommand, "MalePatientID", DbType.Int64, nvo.MalePatientID);
                this.dbServer.AddInParameter(storedProcCommand, "MalePatientUnitID", DbType.Int64, nvo.MalePatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "Value", DbType.String, nvo.DescriptionValue);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                nvo.SpremFreezingDetailsVO = new clsNew_SpremFreezingVO();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsNew_SpremFreezingVO item = new clsNew_SpremFreezingVO {
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["ID"])),
                            VitrificationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["AddedDateTime"])),
                            DescriptionValue = Convert.ToString(DALHelper.HandleDBNull(reader["Value"]))
                        };
                        nvo.SpremFreezingDetails.Add(item);
                    }
                }
            }
            catch (Exception exception)
            {
                connection.Close();
                throw exception;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject GetThawingDetailsList(IValueObject valueObject, clsUserVO UserVO)
        {
            cls_NewGetSpremThawingBizActionVO nvo = (cls_NewGetSpremThawingBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetNEWThawingDetails");
                this.dbServer.AddInParameter(storedProcCommand, "TherapyID", DbType.Int64, nvo.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "TherapyUnitID", DbType.Int64, nvo.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.SpremThawingDetailsList == null)
                    {
                        nvo.SpremThawingDetailsList = new List<cls_NewThawingDetailsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                            break;
                        }
                        cls_NewThawingDetailsVO item = new cls_NewThawingDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            SpremFreezingDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["SpremFreezingDate"]))),
                            SpremFreezingTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["SpremFreezingTime"]))),
                            ThawingDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawingDate"]))),
                            ThawingTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawingTime"]))),
                            DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"])),
                            EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"])),
                            CollectionMethodID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionMethodID"])),
                            CollectionProblem = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionProblem"])),
                            Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"])),
                            ThawComments = Convert.ToString(DALHelper.HandleDBNull(reader["ThawComments"])),
                            GradeA = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeA"])),
                            GradeB = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeB"])),
                            GradeC = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeC"])),
                            Volume = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Volume"])),
                            Motility = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Motility"])),
                            Others = Convert.ToString(DALHelper.HandleDBNull(reader["other"])),
                            Abstience = Convert.ToString(DALHelper.HandleDBNull(reader["Abstience"])),
                            TotalSpearmCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["TotalSpremCount"])),
                            ViscosityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ViscosityID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])),
                            MainID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainID"])),
                            MainUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainUnitID"])),
                            LabPersonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabPersonId"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"])),
                            TherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyUnitID"])),
                            CycleCode = Convert.ToString(DALHelper.HandleDBNull(reader["CycleCode"])),
                            FreezingID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FreezingID"])),
                            FreezingUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FreezingUnitID"])),
                            SpremNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpremNo"])),
                            UsedVolume = Convert.ToSingle(DALHelper.HandleDBNull(reader["UsedVolume"]))
                        };
                        nvo.SpremThawingDetailsList.Add(item);
                    }
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetThawingDetailsListForIUI(IValueObject valueObject, clsUserVO UserVO)
        {
            cls_NewGetSpremThawingBizActionVO nvo = (cls_NewGetSpremThawingBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetThawList");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.MalePatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.MalePatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.SpremThawingDetailsList == null)
                    {
                        nvo.SpremThawingDetailsList = new List<cls_NewThawingDetailsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                            break;
                        }
                        cls_NewThawingDetailsVO item = new cls_NewThawingDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ThawingDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawingDate"]))),
                            ThawingTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawingTime"]))),
                            SpremNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionNo"])),
                            LabPersonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabPersonId"])),
                            PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"])),
                            PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]))
                        };
                        nvo.SpremThawingDetailsList.Add(item);
                    }
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
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

