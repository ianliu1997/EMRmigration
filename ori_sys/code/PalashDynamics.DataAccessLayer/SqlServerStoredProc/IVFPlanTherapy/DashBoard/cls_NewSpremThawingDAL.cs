using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Data;
using System.Data.Common;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    class cls_NewSpremThawingDAL : cls_NewBaseSpremThawingDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion


        private cls_NewSpremThawingDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }

                //Create Instance of the LogManager object 
                if (logManager == null)
                {
                    logManager = LogManager.GetInstance();
                }
                #endregion

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override IValueObject GetSpremFreezingforThawingNew(IValueObject ValueObject, clsUserVO UserVO)
        {
            cls_NewGetSpremThawingBizActionVO Bizaction = (ValueObject) as cls_NewGetSpremThawingBizActionVO;
            Bizaction.SpremFreezingDetailsVO = new clsNew_SpremFreezingVO();
            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSpremThawing_New");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, Bizaction.MalePatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.String, Bizaction.MalePatientUnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsNew_SpremFreezingVO VO = new clsNew_SpremFreezingVO();
                        VO.SpremNostr = Convert.ToString(DALHelper.HandleDBNull(reader["ID"]));
                        VO.Volume = Convert.ToDouble(DALHelper.HandleDBNull(reader["Volume"]));
                        VO.SpermCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["TotalSpremCount"]));
                        VO.Motility = Convert.ToDouble(DALHelper.HandleDBNull(reader["Motility"]));
                        VO.GobletColor = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"]));
                        VO.Cane = Convert.ToString(DALHelper.HandleDBNull(reader["Cane"]));
                        VO.Canister = Convert.ToString(DALHelper.HandleDBNull(reader["Canister"]));
                        VO.Straw = Convert.ToString(DALHelper.HandleDBNull(reader["Straw"]));
                        VO.Tank = Convert.ToString(DALHelper.HandleDBNull(reader["Tank"]));
                        VO.GolbletShape = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShape"]));
                        VO.GobletSize = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSize"]));                     
                        VO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        VO.strCollectionMethod = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionMethod"]));
                        VO.CollectionMethod = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionMethodID"]));
                        VO.VitrificationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["SpremFreezingDate"]));   
                       VO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                       VO.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));

                        Bizaction.SpremFreezingDetails.Add(VO);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return Bizaction;
        }

        public override IValueObject GetThawingDetailsList(IValueObject valueObject, clsUserVO UserVO)
        {
            cls_NewGetSpremThawingBizActionVO BizActionObj = (cls_NewGetSpremThawingBizActionVO)valueObject;
            DbCommand command;
            DbDataReader reader;
            try
            {
                #region
                command = dbServer.GetStoredProcCommand("CIMS_IVF_GetNEWThawingDetails");
                dbServer.AddInParameter(command, "TherapyID", DbType.Int64, BizActionObj.PlanTherapyID);
                dbServer.AddInParameter(command, "TherapyUnitID", DbType.Int64, BizActionObj.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.SpremThawingDetailsList == null)
                        BizActionObj.SpremThawingDetailsList = new List<cls_NewThawingDetailsVO>();
                    while (reader.Read())
                    {
                        cls_NewThawingDetailsVO objThawDetailsVO = new cls_NewThawingDetailsVO();
                        objThawDetailsVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objThawDetailsVO.SpremFreezingDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["SpremFreezingDate"]));
                        objThawDetailsVO.SpremFreezingTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["SpremFreezingTime"]));
                        objThawDetailsVO.ThawingDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawingDate"]));
                        objThawDetailsVO.ThawingTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawingTime"]));
                        objThawDetailsVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        objThawDetailsVO.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        objThawDetailsVO.CollectionMethodID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionMethodID"]));
                        objThawDetailsVO.CollectionProblem = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionProblem"]));
                        objThawDetailsVO.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                        objThawDetailsVO.ThawComments = Convert.ToString(DALHelper.HandleDBNull(reader["ThawComments"]));
                        objThawDetailsVO.GradeA = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeA"]));
                        objThawDetailsVO.GradeB = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeB"]));
                        objThawDetailsVO.GradeC = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeC"]));
                        objThawDetailsVO.Volume = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Volume"]));
                        objThawDetailsVO.Motility = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Motility"]));
                        objThawDetailsVO.Others = Convert.ToString(DALHelper.HandleDBNull(reader["other"]));
                        objThawDetailsVO.Abstience = Convert.ToString(DALHelper.HandleDBNull(reader["Abstience"]));
                        objThawDetailsVO.TotalSpearmCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["TotalSpremCount"]));
                        objThawDetailsVO.ViscosityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ViscosityID"]));
                        objThawDetailsVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objThawDetailsVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        objThawDetailsVO.MainID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainID"]));
                        objThawDetailsVO.MainUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainUnitID"]));
                        objThawDetailsVO.LabPersonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabPersonId"]));
                        objThawDetailsVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objThawDetailsVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        //.........................................................................................................................
                        objThawDetailsVO.TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"]));
                        objThawDetailsVO.TherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyUnitID"]));
                        objThawDetailsVO.CycleCode = Convert.ToString(DALHelper.HandleDBNull(reader["CycleCode"]));
                        objThawDetailsVO.FreezingID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FreezingID"]));
                        objThawDetailsVO.FreezingUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FreezingUnitID"]));
                        objThawDetailsVO.SpremNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpremNo"]));
                        objThawDetailsVO.UsedVolume = Convert.ToSingle(DALHelper.HandleDBNull(reader["UsedVolume"]));
                      //.........................................................................................................................



                        BizActionObj.SpremThawingDetailsList.Add(objThawDetailsVO);
                    }
                    reader.NextResult();
                    var aa = dbServer.GetParameterValue(command, "TotalRows");
                    BizActionObj.TotalRows = Convert.ToInt32(aa);
                }
                if (reader.IsClosed == false)
                {
                    reader.Close();
                }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetTesaPesaForCode(IValueObject valueObject, clsUserVO UserVO)
        {
            cls_GetSpremFreezingDetilsForThawingBizActionVO Bizaction = (valueObject) as cls_GetSpremFreezingDetilsForThawingBizActionVO;

            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientEMRDataTemplate");          //CIMS_GetSpermFreezingDetailsForThawing
                dbServer.AddInParameter(command, "MalePatientID", DbType.Int64, Bizaction.MalePatientID);
                dbServer.AddInParameter(command, "MalePatientUnitID", DbType.Int64, Bizaction.MalePatientUnitID);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, Bizaction.TemplateID);
                dbServer.AddInParameter(command, "Value", DbType.String, Bizaction.DescriptionValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                Bizaction.SpremFreezingDetailsVO = new clsNew_SpremFreezingVO();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        clsNew_SpremFreezingVO VO = new clsNew_SpremFreezingVO();
                        VO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["ID"]));
                        VO.VitrificationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["AddedDateTime"]));
                        VO.DescriptionValue = Convert.ToString(DALHelper.HandleDBNull(reader["Value"]));

                        Bizaction.SpremFreezingDetails.Add(VO);
                    }
                }

            }
            catch (Exception ex)
            {
                con.Close();
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return Bizaction;
        }

        public override IValueObject GetSpermFrezingDetailsForThawing(IValueObject valueObject, clsUserVO UserVO) 
        {
            cls_GetSpremFreezingDetilsForThawingBizActionVO Bizaction = (valueObject) as cls_GetSpremFreezingDetilsForThawingBizActionVO;
            DbDataReader reader;
            DbConnection con = dbServer.CreateConnection();
            Bizaction.SpremThawingDetailsList = new List<cls_NewThawingDetailsVO>();
            try
            {
                con.Open();
                if (Bizaction.MalePatientID > 0 && Bizaction.MalePatientUnitID > 0) //added by neena 
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSpermFreezingDetailsForThawingForASD");          //CIMS_GetSpermFreezingDetailsForThawing
                    dbServer.AddInParameter(command, "MalePatientID", DbType.Int64, Bizaction.MalePatientID);
                    dbServer.AddInParameter(command, "MalePatientUnitID", DbType.Int64, Bizaction.MalePatientUnitID);
                    dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, Bizaction.PlanTherapyID);
                    dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, Bizaction.PlanTherapyUnitID);
                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    Bizaction.SpremFreezingDetailsVO = new clsNew_SpremFreezingVO();
                }
                else
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSpermFreezingDetailsForThawing");
                    dbServer.AddInParameter(command, "FreezingID", DbType.Int64, Bizaction.ID);
                    dbServer.AddInParameter(command, "FreezingUnitID", DbType.Int64, Bizaction.UnitID);
                    dbServer.AddInParameter(command, "SpremNo", DbType.Int64, Bizaction.SpremFreezingDetailsVO.SpremNo);
                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    Bizaction.SpremFreezingDetailsVO = new clsNew_SpremFreezingVO();
                }
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                       
                        clsNew_SpremFreezingVO VO = new clsNew_SpremFreezingVO();
                        VO.SpremNostr = Convert.ToString(DALHelper.HandleDBNull(reader["ID"]));
                        VO.Volume = Convert.ToDouble(DALHelper.HandleDBNull(reader["Volume"]));
                        //VO.SpermCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["TotalSpremCount"]));
                        VO.SpermCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermCount"]));
                        VO.Motility = Convert.ToDouble(DALHelper.HandleDBNull(reader["Motility"]));
                        VO.GobletColor = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"]));
                        VO.Cane = Convert.ToString(DALHelper.HandleDBNull(reader["Cane"]));
                        VO.Canister = Convert.ToString(DALHelper.HandleDBNull(reader["Canister"]));
                        VO.Straw = Convert.ToString(DALHelper.HandleDBNull(reader["Straw"]));
                        VO.Tank = Convert.ToString(DALHelper.HandleDBNull(reader["Tank"]));
                        VO.GolbletShape = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShape"]));
                        VO.GobletSize = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSize"]));
                        VO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        VO.strCollectionMethod = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionMethod"]));
                        VO.CollectionMethod = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionMethodID"]));
                        VO.VitrificationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["SpremFreezingDate"]));
                        VO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        VO.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                        Bizaction.SpremFreezingDetails.Add(VO);
                    }
                }

                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cls_NewThawingDetailsVO objThawDetailsVO = new cls_NewThawingDetailsVO();
                        objThawDetailsVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objThawDetailsVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objThawDetailsVO.ThawingDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawingDate"]));
                        objThawDetailsVO.ThawingTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawingTime"]));
                        //objThawDetailsVO.Doctor = Convert.ToString(DALHelper.HandleDBNull(reader["Doctor"]));
                        //objThawDetailsVO.WitnessBy = Convert.ToString(DALHelper.HandleDBNull(reader["WitnessBy"]));
                        objThawDetailsVO.SpremNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermFreezingDetailsID"]));
                        objThawDetailsVO.LabPersonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabPersonId"]));
                        objThawDetailsVO.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        objThawDetailsVO.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        objThawDetailsVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        objThawDetailsVO.SemenWashID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SemenWashID"]));
                        objThawDetailsVO.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                        //objThawDetailsVO.SpremNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermFreezingDetailsID"]));
                        objThawDetailsVO.Volume = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Volume"]));
                        //VO.SpermCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["TotalSpremCount"]));
                        objThawDetailsVO.TotalSpearmCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermCount"]));
                        objThawDetailsVO.Motility = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Motility"]));
                        objThawDetailsVO.DonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorID"]));
                        objThawDetailsVO.DonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorUnitID"]));
                        objThawDetailsVO.DonorName = Convert.ToString(DALHelper.HandleDBNull(reader["DonorName"]));
                        objThawDetailsVO.DonorMrno = Convert.ToString(DALHelper.HandleDBNull(reader["DonorMRNO"]));                     
                        Bizaction.SpremThawingDetailsList.Add(objThawDetailsVO);

                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Bizaction.IUIID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IUIID"]));
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return Bizaction;
        }

        public override IValueObject GetSpermFrezingDetailsForThawingView(IValueObject valueObject, clsUserVO UserVO)
        {
            cls_GetSpremFreezingDetilsForThawingBizActionVO Bizaction = (valueObject) as cls_GetSpremFreezingDetilsForThawingBizActionVO;
            DbDataReader reader;
            DbConnection con = dbServer.CreateConnection();
            Bizaction.SpremThawingDetailsList = new List<cls_NewThawingDetailsVO>();
            try
            {
                con.Open();
                if (Bizaction.MalePatientID > 0 && Bizaction.MalePatientUnitID > 0) //added by neena 
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSpermFreezingDetailsForThawingForView");          //CIMS_GetSpermFreezingDetailsForThawing
                    dbServer.AddInParameter(command, "MalePatientID", DbType.Int64, Bizaction.MalePatientID);
                    dbServer.AddInParameter(command, "MalePatientUnitID", DbType.Int64, Bizaction.MalePatientUnitID);
                    dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, Bizaction.PlanTherapyID);
                    dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, Bizaction.PlanTherapyUnitID);
                    dbServer.AddInParameter(command, "SemenWashID", DbType.Int64, Bizaction.SemenWashID);
                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    Bizaction.SpremFreezingDetailsVO = new clsNew_SpremFreezingVO();
                }
                else
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSpermFreezingDetailsForThawing");
                    dbServer.AddInParameter(command, "FreezingID", DbType.Int64, Bizaction.ID);
                    dbServer.AddInParameter(command, "FreezingUnitID", DbType.Int64, Bizaction.UnitID);
                    dbServer.AddInParameter(command, "SpremNo", DbType.Int64, Bizaction.SpremFreezingDetailsVO.SpremNo);
                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    Bizaction.SpremFreezingDetailsVO = new clsNew_SpremFreezingVO();
                }
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        clsNew_SpremFreezingVO VO = new clsNew_SpremFreezingVO();
                        VO.SpremNostr = Convert.ToString(DALHelper.HandleDBNull(reader["ID"]));
                        VO.Volume = Convert.ToDouble(DALHelper.HandleDBNull(reader["Volume"]));
                        VO.SpermCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["TotalSpremCount"]));
                        VO.Motility = Convert.ToDouble(DALHelper.HandleDBNull(reader["Motility"]));
                        VO.GobletColor = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"]));
                        VO.Cane = Convert.ToString(DALHelper.HandleDBNull(reader["Cane"]));
                        VO.Canister = Convert.ToString(DALHelper.HandleDBNull(reader["Canister"]));
                        VO.Straw = Convert.ToString(DALHelper.HandleDBNull(reader["Straw"]));
                        VO.Tank = Convert.ToString(DALHelper.HandleDBNull(reader["Tank"]));
                        VO.GolbletShape = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShape"]));
                        VO.GobletSize = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSize"]));
                        VO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        VO.strCollectionMethod = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionMethod"]));
                        VO.CollectionMethod = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionMethodID"]));
                        VO.VitrificationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["SpremFreezingDate"]));
                        VO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        VO.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                        Bizaction.SpremFreezingDetails.Add(VO);
                    }
                }

                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cls_NewThawingDetailsVO objThawDetailsVO = new cls_NewThawingDetailsVO();
                        objThawDetailsVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objThawDetailsVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objThawDetailsVO.ThawingDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawingDate"]));
                        objThawDetailsVO.ThawingTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawingTime"]));
                        //objThawDetailsVO.Doctor = Convert.ToString(DALHelper.HandleDBNull(reader["Doctor"]));
                        //objThawDetailsVO.WitnessBy = Convert.ToString(DALHelper.HandleDBNull(reader["WitnessBy"]));
                        objThawDetailsVO.SpremNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermFreezingDetailsID"]));
                        objThawDetailsVO.LabPersonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabPersonId"]));
                        objThawDetailsVO.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        objThawDetailsVO.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        objThawDetailsVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        objThawDetailsVO.SemenWashID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SemenWashID"]));
                        //objThawDetailsVO.SpremNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermFreezingDetailsID"]));
                        Bizaction.SpremThawingDetailsList.Add(objThawDetailsVO);

                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return Bizaction;
        }

        //added by neena
        public override IValueObject GetThawingDetailsListForIUI(IValueObject valueObject, clsUserVO UserVO)
        {
            cls_NewGetSpremThawingBizActionVO BizActionObj = (cls_NewGetSpremThawingBizActionVO)valueObject;
            DbCommand command;
            DbDataReader reader;
            try
            {   
                command = dbServer.GetStoredProcCommand("CIMS_GetThawList");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.MalePatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.MalePatientUnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.SpremThawingDetailsList == null)
                        BizActionObj.SpremThawingDetailsList = new List<cls_NewThawingDetailsVO>();
                    while (reader.Read())
                    {
                        cls_NewThawingDetailsVO objThawDetailsVO = new cls_NewThawingDetailsVO();
                        objThawDetailsVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objThawDetailsVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objThawDetailsVO.ThawingDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawingDate"]));
                        objThawDetailsVO.ThawingTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawingTime"]));
                        //objThawDetailsVO.Doctor = Convert.ToString(DALHelper.HandleDBNull(reader["Doctor"]));
                        //objThawDetailsVO.WitnessBy = Convert.ToString(DALHelper.HandleDBNull(reader["WitnessBy"]));
                        objThawDetailsVO.SpremNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionNo"]));
                        objThawDetailsVO.LabPersonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabPersonId"]));
                        objThawDetailsVO.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        objThawDetailsVO.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        //objThawDetailsVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));


                        //objThawDetailsVO.SpremFreezingDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["SpremFreezingDate"]));
                        //objThawDetailsVO.SpremFreezingTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["SpremFreezingTime"]));                      
                        //objThawDetailsVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        //objThawDetailsVO.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        //objThawDetailsVO.CollectionMethodID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionMethodID"]));
                        //objThawDetailsVO.CollectionProblem = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionProblem"]));
                        //objThawDetailsVO.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                        //objThawDetailsVO.ThawComments = Convert.ToString(DALHelper.HandleDBNull(reader["ThawComments"]));
                        //objThawDetailsVO.GradeA = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeA"]));
                        //objThawDetailsVO.GradeB = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeB"]));
                        //objThawDetailsVO.GradeC = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeC"]));
                        //objThawDetailsVO.Volume = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Volume"]));
                        //objThawDetailsVO.Motility = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Motility"]));
                        //objThawDetailsVO.Others = Convert.ToString(DALHelper.HandleDBNull(reader["other"]));
                        //objThawDetailsVO.Abstience = Convert.ToString(DALHelper.HandleDBNull(reader["Abstience"]));
                        //objThawDetailsVO.TotalSpearmCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["TotalSpremCount"]));
                        //objThawDetailsVO.ViscosityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ViscosityID"]));
                        //objThawDetailsVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        //objThawDetailsVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        //objThawDetailsVO.MainID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainID"]));
                        //objThawDetailsVO.MainUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainUnitID"]));
                        //objThawDetailsVO.LabPersonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabPersonId"]));
                        //objThawDetailsVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        //objThawDetailsVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        ////.........................................................................................................................
                        //objThawDetailsVO.TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"]));
                        //objThawDetailsVO.TherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyUnitID"]));
                        //objThawDetailsVO.CycleCode = Convert.ToString(DALHelper.HandleDBNull(reader["CycleCode"]));
                        //objThawDetailsVO.FreezingID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FreezingID"]));
                        //objThawDetailsVO.FreezingUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FreezingUnitID"]));
                        //objThawDetailsVO.SpremNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpremNo"]));
                        //objThawDetailsVO.UsedVolume = Convert.ToSingle(DALHelper.HandleDBNull(reader["UsedVolume"]));
                        //.........................................................................................................................



                        BizActionObj.SpremThawingDetailsList.Add(objThawDetailsVO);
                    }
                    reader.NextResult();
                    var aa = dbServer.GetParameterValue(command, "TotalRows");
                    BizActionObj.TotalRows = Convert.ToInt32(aa);
                }
                if (reader.IsClosed == false)
                {
                    reader.Close();
                }
            }

            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }
        //
        
    }
}
