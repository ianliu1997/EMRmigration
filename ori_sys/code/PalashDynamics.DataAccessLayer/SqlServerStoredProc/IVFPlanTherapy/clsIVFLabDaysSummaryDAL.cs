using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    class clsIVFLabDaysSummaryDAL : clsBaseIVFLabDaysSummaryDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsIVFLabDaysSummaryDAL()
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

        public override IValueObject AddUpdateLabDaysSummary(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateLabDaysSummaryBizActionVO BizActionObj = valueObject as clsAddUpdateLabDaysSummaryBizActionVO;
            BizActionObj = AddUpdateDetails(BizActionObj, UserVo, null, null);
            return valueObject;
        }

        public override IValueObject AddUpdateLabDaysSummary(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddUpdateLabDaysSummaryBizActionVO BizActionObj = valueObject as clsAddUpdateLabDaysSummaryBizActionVO;
            BizActionObj = AddUpdateDetails(BizActionObj, UserVo, pConnection, pTransaction);
            return valueObject;
        }

        private clsAddUpdateLabDaysSummaryBizActionVO AddUpdateDetails(clsAddUpdateLabDaysSummaryBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            DbConnection con = null;
            DbTransaction trans = null;

            try
            {

                if (pConnection != null) con = pConnection;
                else con = dbServer.CreateConnection();

                if (con.State == ConnectionState.Closed) con.Open();

                if (pTransaction != null) trans = pTransaction;
                else trans = con.BeginTransaction();

                DbCommand command;
                if (BizActionObj.IsUpdate == false)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySummary");
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.LabDaysSummary.ID);

                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_IVF_UpdateLabDaysSummary");
                    dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.LabDaysSummary.ID);

                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizActionObj.LabDaysSummary.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizActionObj.LabDaysSummary.CoupleUnitID);

                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.LabDaysSummary.ProcDate);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, BizActionObj.LabDaysSummary.ProcTime);
                dbServer.AddInParameter(command, "FormID", DbType.Int16, BizActionObj.LabDaysSummary.FormID);
                dbServer.AddInParameter(command, "OocyteID", DbType.Int64, BizActionObj.LabDaysSummary.OocyteID);
                dbServer.AddInParameter(command, "Impressions", DbType.String, BizActionObj.LabDaysSummary.Impressions);
                dbServer.AddInParameter(command, "Priority", DbType.Int32, BizActionObj.LabDaysSummary.Priority);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, BizActionObj.LabDaysSummary.IsFreezed);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                if (BizActionObj.IsUpdate == false)
                {
                    BizActionObj.LabDaysSummary.ID = (long)dbServer.GetParameterValue(command, "ID");
                }
                BizActionObj.SuccessStatus = 0;
                if (pConnection == null) trans.Commit();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                BizActionObj.SuccessStatus = -1;
                if (pConnection == null) trans.Rollback();
                //throw;
            }
            finally
            {
                if (pConnection == null)
                {
                    con.Close();
                    con = null;
                    trans = null;
                }
            }
            return BizActionObj;
        }

        public override IValueObject GetLabDaysSummary(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetLabDaysSummaryBizActionVO BizAction = (valueObject) as clsGetLabDaysSummaryBizActionVO;

            try
            {
                if (BizAction.LabDaysSummary == null)
                    BizAction.LabDaysSummary = new List<clsLabDaysSummaryVO>();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetLabDaysSummary");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizAction.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizAction.CoupleUnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizAction.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizAction.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizAction.MaximumRows);
                //dbServer.AddInParameter(command, "sortExpression", DbType.String, "Code");
                //dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizAction.SearchExpression);

                dbServer.AddParameter(command, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizAction.TotalRows);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);



                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsLabDaysSummaryVO objLabDaySummary = new clsLabDaysSummaryVO();

                        objLabDaySummary.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objLabDaySummary.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        objLabDaySummary.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        objLabDaySummary.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));
                        objLabDaySummary.ProcDate = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objLabDaySummary.ProcTime = Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]));
                        //objLabDaySummary.FormID = (IVFLabWorkForm)(reader["FormID"]);

                        #region Done By SHIKHA

                        int FormType;

                        FormType = Convert.ToInt16(DALHelper.HandleDBNull(reader["FormID"]));
                        objLabDaySummary.FormID = (IVFLabWorkForm)FormType;

                        #endregion

                        //Int16 ob = Convert.ToInt16(DALHelper.HandleDBNull(reader["FormID"]));
                        //if(ob==0)
                        //{
                        //    objLabDaySummary.FormID = IVFLabWorkForm.FemaleLabDay0;
                        //}
                        objLabDaySummary.FormName = objLabDaySummary.FormID.ToString();
                        objLabDaySummary.OocyteID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteID"]));
                        objLabDaySummary.Impressions = Convert.ToString(DALHelper.HandleDBNull(reader["Impressions"]));
                        objLabDaySummary.Priority = Convert.ToInt32(DALHelper.HandleDBNull(reader["Priority"]));
                        objLabDaySummary.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        objLabDaySummary.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizAction.LabDaysSummary.Add(objLabDaySummary);
                    }
                }

                reader.NextResult();
                BizAction.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));

                reader.Close();
            }
            catch (Exception ex)
            {
                BizAction.LabDaysSummary = null;
                throw;
            }
            finally
            {
            }
            return BizAction;
        }

        public override IValueObject GetArtCycleSummary(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetArtCycleSummaryBizActionVO BizAction = (valueObject) as clsGetArtCycleSummaryBizActionVO;

            try
            {
                if (BizAction.ArtCycleSummary == null)
                    BizAction.ArtCycleSummary = new List<clsArtCycleSummaryVO>();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_LabDaySummary");


                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizAction.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizAction.CoupleUnitID);

                //dbServer.AddParameter(command, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizAction.TotalRows);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);


                reader.NextResult();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsArtCycleSummaryVO objArtCycleSummary = new clsArtCycleSummaryVO();

                        objArtCycleSummary.PlanTherapyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyId"]));
                        objArtCycleSummary.TherapyStartDate = Convert.ToDateTime(DALHelper.HandleDate(reader["TherapyStartDate"]));
                        objArtCycleSummary.Oocytes = Convert.ToString(DALHelper.HandleDBNull(reader["Oocytes"]));
                        objArtCycleSummary.Treatment = Convert.ToString(DALHelper.HandleDBNull(reader["Treatment"]));
                        objArtCycleSummary.PronucleusStages = Convert.ToString(DALHelper.HandleDBNull(reader["PronucleusStages"]));
                        objArtCycleSummary.Embryos = Convert.ToString(DALHelper.HandleDBNull(reader["Embryos"]));

                        BizAction.ArtCycleSummary.Add(objArtCycleSummary);

                    }
                }

                //reader.NextResult();
                //BizAction.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));

                reader.Close();
            }
            catch (Exception ex)
            {
                BizAction.ArtCycleSummary = null;
                throw;
            }
            finally
            {
            }

            return BizAction;
        }

        public IVFLabWorkForm GetIVFenumValue(Int16 ob)
        {
            IVFLabWorkForm ob1 = new IVFLabWorkForm();
            ob1 = (IVFLabWorkForm)ob;

            switch (ob)
            {
                case 0:
                    ob1 = IVFLabWorkForm.FemaleLabDay0;
                    break;
                case 1:
                    ob1 = IVFLabWorkForm.FemaleLabDay1;
                    break;
                case 2:
                    ob1 = IVFLabWorkForm.FemaleLabDay2;
                    break;
                case 3:
                    ob1 = IVFLabWorkForm.FemaleLabDay3;
                    break;
                case 4:
                    ob1 = IVFLabWorkForm.FemaleLabDay4;
                    break;
                case 5:
                    ob1 = IVFLabWorkForm.FemaleLabDay5;
                    break;
                case 6:
                    ob1 = IVFLabWorkForm.FemaleLabDay6;
                    break;
                case 7:
                    ob1 = IVFLabWorkForm.EmbryoTransfer;
                    break;
                case 8:
                    ob1 = IVFLabWorkForm.Vitrification;
                    break;
                case 9:
                    ob1 = IVFLabWorkForm.Thawing;
                    break;
                case 10:
                    ob1 = IVFLabWorkForm.MediaCosting;
                    break;
            }
            return ob1;
        }
    }
}
