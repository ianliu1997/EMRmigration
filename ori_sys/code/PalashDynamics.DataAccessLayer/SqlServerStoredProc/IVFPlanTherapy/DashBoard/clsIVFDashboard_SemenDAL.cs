using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects;
using System.Data.Common;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    class clsIVFDashboard_SemenDAL : clsBaseIVFDashboard_SemenDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion
        private clsIVFDashboard_SemenDAL()
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

        // Semen Examination 
        public override IValueObject AddUpdateSemenExaminationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            cls_IVFDashboard_AddSemenBizActionVO BizActionObj = valueObject as cls_IVFDashboard_AddSemenBizActionVO;

            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientSemenExamination");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.SemensExaminationDetails.UnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.SemensExaminationDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.SemensExaminationDetails.PatientUnitID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.SemensExaminationDetails.VisitID);
                dbServer.AddInParameter(command, "CollectionDate", DbType.DateTime, BizActionObj.SemensExaminationDetails.CollectionDate);
                dbServer.AddInParameter(command, "TimeRecSampLab", DbType.DateTime, BizActionObj.SemensExaminationDetails.TimeRecSampLab);
                dbServer.AddInParameter(command, "MethodOfCollection", DbType.Int64, BizActionObj.SemensExaminationDetails.MethodOfCollectionID);
                dbServer.AddInParameter(command, "Abstinence", DbType.Int64, BizActionObj.SemensExaminationDetails.AbstinenceID);
                dbServer.AddInParameter(command, "Complete", DbType.Boolean, BizActionObj.SemensExaminationDetails.Complete);
                dbServer.AddInParameter(command, "CollecteAtCentre", DbType.Boolean, BizActionObj.SemensExaminationDetails.CollecteAtCentre);
                dbServer.AddInParameter(command, "Color", DbType.Int64, BizActionObj.SemensExaminationDetails.ColorID);
                dbServer.AddInParameter(command, "Quantity", DbType.Single, BizActionObj.SemensExaminationDetails.Quantity);
                dbServer.AddInParameter(command, "PH", DbType.Single, BizActionObj.SemensExaminationDetails.PH);
                dbServer.AddInParameter(command, "LiquificationTime", DbType.String, BizActionObj.SemensExaminationDetails.LiquificationTime);
                dbServer.AddInParameter(command, "Viscosity", DbType.Boolean, BizActionObj.SemensExaminationDetails.Viscosity);
                dbServer.AddInParameter(command, "RangeViscosityID", DbType.Int64, BizActionObj.SemensExaminationDetails.RangeViscosityID);
                dbServer.AddInParameter(command, "Odour", DbType.Boolean, BizActionObj.SemensExaminationDetails.Odour);
                dbServer.AddInParameter(command, "SpermCount", DbType.Single, BizActionObj.SemensExaminationDetails.SpermCount);
                dbServer.AddInParameter(command, "TotalSpermCount", DbType.Single, BizActionObj.SemensExaminationDetails.TotalSpermCount);
                dbServer.AddInParameter(command, "Motility", DbType.Single, BizActionObj.SemensExaminationDetails.Motility);
                dbServer.AddInParameter(command, "NonMotility", DbType.Single, BizActionObj.SemensExaminationDetails.NonMotility);
                dbServer.AddInParameter(command, "TotalMotility", DbType.Single, BizActionObj.SemensExaminationDetails.TotalMotility);
                dbServer.AddInParameter(command, "MotilityGradeI", DbType.Single, BizActionObj.SemensExaminationDetails.MotilityGradeI);
                dbServer.AddInParameter(command, "MotilityGradeII", DbType.Single, BizActionObj.SemensExaminationDetails.MotilityGradeII);
                dbServer.AddInParameter(command, "MotilityGradeIII", DbType.Single, BizActionObj.SemensExaminationDetails.MotilityGradeIII);
                dbServer.AddInParameter(command, "MotilityGradeIV", DbType.Single, BizActionObj.SemensExaminationDetails.MotilityGradeIV);
                dbServer.AddInParameter(command, "Amorphus", DbType.Single, BizActionObj.SemensExaminationDetails.Amorphus);
                dbServer.AddInParameter(command, "NeckAppendages", DbType.Single, BizActionObj.SemensExaminationDetails.NeckAppendages);
                dbServer.AddInParameter(command, "Pyriform", DbType.Single, BizActionObj.SemensExaminationDetails.Pyriform);
                dbServer.AddInParameter(command, "Macrocefalic", DbType.Single, BizActionObj.SemensExaminationDetails.Macrocefalic);
                dbServer.AddInParameter(command, "Microcefalic", DbType.Single, BizActionObj.SemensExaminationDetails.Microcefalic);
                dbServer.AddInParameter(command, "BrockenNeck", DbType.Single, BizActionObj.SemensExaminationDetails.BrockenNeck);
                dbServer.AddInParameter(command, "RoundHead", DbType.Single, BizActionObj.SemensExaminationDetails.RoundHead);
                dbServer.AddInParameter(command, "DoubleHead", DbType.Single, BizActionObj.SemensExaminationDetails.DoubleHead);
                dbServer.AddInParameter(command, "Total", DbType.Single, BizActionObj.SemensExaminationDetails.Total);
                dbServer.AddInParameter(command, "MorphologicalAbnormilities", DbType.Single, BizActionObj.SemensExaminationDetails.MorphologicalAbnormilities);
                dbServer.AddInParameter(command, "NormalMorphology", DbType.Single, BizActionObj.SemensExaminationDetails.NormalMorphology);
                dbServer.AddInParameter(command, "Comment", DbType.String, BizActionObj.SemensExaminationDetails.Comment);               
                dbServer.AddInParameter(command, "CytoplasmicDroplet", DbType.Single, BizActionObj.SemensExaminationDetails.CytoplasmicDroplet);
                dbServer.AddInParameter(command, "Others", DbType.Single, BizActionObj.SemensExaminationDetails.Others);
                dbServer.AddInParameter(command, "MidPieceTotal", DbType.Single, BizActionObj.SemensExaminationDetails.MidPieceTotal);
                dbServer.AddInParameter(command, "CoiledTail", DbType.Single, BizActionObj.SemensExaminationDetails.CoiledTail);
                dbServer.AddInParameter(command, "ShortTail", DbType.Single, BizActionObj.SemensExaminationDetails.ShortTail);
                dbServer.AddInParameter(command, "HairpinTail", DbType.Single, BizActionObj.SemensExaminationDetails.HairpinTail);
                dbServer.AddInParameter(command, "DoubleTail", DbType.Single, BizActionObj.SemensExaminationDetails.DoubleTail);
                dbServer.AddInParameter(command, "TailOthers", DbType.Single, BizActionObj.SemensExaminationDetails.TailOthers);
                dbServer.AddInParameter(command, "TailTotal", DbType.Single, BizActionObj.SemensExaminationDetails.TailTotal);
                dbServer.AddInParameter(command, "HeadToHead", DbType.String, BizActionObj.SemensExaminationDetails.HeadToHead);
                dbServer.AddInParameter(command, "TailToTail", DbType.String, BizActionObj.SemensExaminationDetails.TailToTail);
                dbServer.AddInParameter(command, "HeadToTail", DbType.String, BizActionObj.SemensExaminationDetails.HeadToTail);
                dbServer.AddInParameter(command, "SpermToOther", DbType.String, BizActionObj.SemensExaminationDetails.SpermToOther);
                dbServer.AddInParameter(command, "PusCells", DbType.String, BizActionObj.SemensExaminationDetails.PusCells);
                dbServer.AddInParameter(command, "RoundCells", DbType.String, BizActionObj.SemensExaminationDetails.RoundCells);
                dbServer.AddInParameter(command, "EpithelialCells", DbType.String, BizActionObj.SemensExaminationDetails.EpithelialCells);
                dbServer.AddInParameter(command, "Infections", DbType.String, BizActionObj.SemensExaminationDetails.Infections);
                dbServer.AddInParameter(command, "OtherFindings", DbType.String, BizActionObj.SemensExaminationDetails.OtherFindings);
                dbServer.AddInParameter(command, "InterpretationsID", DbType.Int64, BizActionObj.SemensExaminationDetails.InterpretationsID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.SemensExaminationDetails.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, BizActionObj.SemensExaminationDetails.CreatedUnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, BizActionObj.SemensExaminationDetails.UpdatedUnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, BizActionObj.SemensExaminationDetails.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, BizActionObj.SemensExaminationDetails.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, BizActionObj.SemensExaminationDetails.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, BizActionObj.SemensExaminationDetails.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, BizActionObj.SemensExaminationDetails.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, BizActionObj.SemensExaminationDetails.UpdatedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, BizActionObj.SemensExaminationDetails.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, BizActionObj.SemensExaminationDetails.UpdatedWindowsLoginName);
                dbServer.AddInParameter(command, "Synchronized", DbType.Boolean, BizActionObj.SemensExaminationDetails.Synchronized);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, BizActionObj.SemensExaminationDetails.EmbryologistID);
                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientVO.GeneralDetails.PatientID);
                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.PGDHistoryDetails.ID);
                dbServer.AddParameter(command, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.SemensExaminationDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                //added by neena
                dbServer.AddInParameter(command, "Comments", DbType.String, BizActionObj.SemensExaminationDetails.AllComments);
                dbServer.AddInParameter(command, "Sperm5thPercentile", DbType.Single, BizActionObj.SemensExaminationDetails.Sperm5thPercentile);
                dbServer.AddInParameter(command, "Sperm75thPercentile", DbType.Single, BizActionObj.SemensExaminationDetails.Sperm75thPercentile);
                dbServer.AddInParameter(command, "Ejaculate5thPercentile", DbType.Single, BizActionObj.SemensExaminationDetails.Ejaculate5thPercentile);
                dbServer.AddInParameter(command, "Ejaculate75thPercentile", DbType.Single, BizActionObj.SemensExaminationDetails.Ejaculate75thPercentile);
                dbServer.AddInParameter(command, "TotalMotility5thPercentile", DbType.Single, BizActionObj.SemensExaminationDetails.TotalMotility5thPercentile);
                dbServer.AddInParameter(command, "TotalMotility75thPercentile", DbType.Single, BizActionObj.SemensExaminationDetails.TotalMotility75thPercentile);
                dbServer.AddInParameter(command, "RapidProgressive", DbType.Single, BizActionObj.SemensExaminationDetails.RapidProgressive);
                dbServer.AddInParameter(command, "SlowProgressive", DbType.Single, BizActionObj.SemensExaminationDetails.SlowProgressive);
                dbServer.AddInParameter(command, "SpermMorphology5thPercentile", DbType.Single, BizActionObj.SemensExaminationDetails.SpermMorphology5thPercentile);
                dbServer.AddInParameter(command, "SpermMorphology75thPercentile", DbType.Single, BizActionObj.SemensExaminationDetails.SpermMorphology75thPercentile);
                dbServer.AddInParameter(command, "NormalFormsComments", DbType.String, BizActionObj.SemensExaminationDetails.NormalFormsComments);
                dbServer.AddInParameter(command, "OverAllDefectsComments", DbType.String, BizActionObj.SemensExaminationDetails.OverAllDefectsComments);
                dbServer.AddInParameter(command, "HeadDefectsComments", DbType.String, BizActionObj.SemensExaminationDetails.HeadDefectsComments);
                dbServer.AddInParameter(command, "MidPieceNeckDefectsComments", DbType.String, BizActionObj.SemensExaminationDetails.MidPieceNeckDefectsComments);
                dbServer.AddInParameter(command, "TailDefectsComments", DbType.String, BizActionObj.SemensExaminationDetails.TailDefectsComments);
                dbServer.AddInParameter(command, "ExcessiveResidualComments", DbType.String, BizActionObj.SemensExaminationDetails.ExcessiveResidualComments);
                dbServer.AddInParameter(command, "SpermMorphologySubNormal", DbType.String, BizActionObj.SemensExaminationDetails.SpermMorphologySubNormal);
                dbServer.AddInParameter(command, "Spillage", DbType.String, BizActionObj.SemensExaminationDetails.Spillage);
                dbServer.AddInParameter(command, "Fructose", DbType.String, BizActionObj.SemensExaminationDetails.Fructose);
                dbServer.AddInParameter(command, "Live", DbType.Single, BizActionObj.SemensExaminationDetails.Live);
                dbServer.AddInParameter(command, "Dead", DbType.Single, BizActionObj.SemensExaminationDetails.Dead);
                dbServer.AddInParameter(command, "TotalAdvanceMotility", DbType.Single, BizActionObj.SemensExaminationDetails.TotalAdvanceMotility);
                dbServer.AddInParameter(command, "TotalAdvance5thPercentile", DbType.Single, BizActionObj.SemensExaminationDetails.TotalAdvance5thPercentile);
                dbServer.AddInParameter(command, "TotalAdvance75thPercentile", DbType.Single, BizActionObj.SemensExaminationDetails.TotalAdvance75thPercentile);
                //

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.SemensExaminationDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.SemensExaminationDetails = null;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;

        }


        public override IValueObject GetSemenExaminationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();  CIMS_GetSemenExaminationDetails

            cls_GetIVFDashboard_SemenBizActionVO BizActionObj = valueObject as cls_GetIVFDashboard_SemenBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSemenExaminationDetails");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<cls_IVFDashboard_SemenVO>();
                    while (reader.Read())
                    {
                        cls_IVFDashboard_SemenVO Details = new cls_IVFDashboard_SemenVO();

                        Details.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        Details.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        Details.CollectionDate = (DateTime?)DALHelper.HandleDate(reader["CollectionDate"]);
                        Details.MethodOfCollection = (string)DALHelper.HandleDBNull(reader["MethodOfCollection"]);

                        Details.MethodOfCollectionID = (long)DALHelper.HandleDBNull(reader["MethodOfCollectionID"]);
                        Details.TimeRecSampLab = (DateTime?)DALHelper.HandleDBNull(reader["TimeRecSampLab"]);
                        Details.AbstinenceID = (long)DALHelper.HandleDBNull(reader["Abstinence"]);
                        Details.Complete = (Boolean)DALHelper.HandleDBNull(reader["Complete"]);
                        Details.CollecteAtCentre = (Boolean)DALHelper.HandleDBNull(reader["CollecteAtCentre"]);
                        Details.ColorID = (long)DALHelper.HandleDBNull(reader["Color"]);
                        Details.Quantity = (float)(Double)(Double)DALHelper.HandleDBNull(reader["Quantity"]);
                        Details.PH = (float)(Double)DALHelper.HandleDBNull(reader["PH"]);
                        Details.LiquificationTime = (string)DALHelper.HandleDBNull(reader["LiquificationTime"]);
                        Details.Viscosity = (Boolean)DALHelper.HandleDBNull(reader["Viscosity"]);
                        Details.Odour = (Boolean)DALHelper.HandleDBNull(reader["Odour"]);

                        Details.SpermCount = (float)(Double)DALHelper.HandleDBNull(reader["SpermCount"]);
                        //Details.Total = (float)(Double)DALHelper.HandleDBNull(reader["TotalSpermCount"]);
                        Details.TotalSpermCount = (float)(Double)DALHelper.HandleDBNull(reader["TotalSpermCount"]);
                        Details.Motility = (float)(Double)DALHelper.HandleDBNull(reader["Motility"]);
                        Details.NonMotility = (float)(Double)DALHelper.HandleDBNull(reader["NonMotility"]);
                        Details.TotalMotility = (float)(Double)DALHelper.HandleDBNull(reader["TotalMotility"]);
                        Details.MotilityGradeI = (float)(Double)DALHelper.HandleDBNull(reader["MotilityGradeI"]);
                        Details.MotilityGradeII = (float)(Double)DALHelper.HandleDBNull(reader["MotilityGradeII"]);
                        Details.MotilityGradeIII = (float)(Double)DALHelper.HandleDBNull(reader["MotilityGradeIII"]);
                        Details.MotilityGradeIV = (float)(Double)DALHelper.HandleDBNull(reader["MotilityGradeIV"]);
                        Details.Amorphus = (float)(Double)DALHelper.HandleDBNull(reader["Amorphus"]);
                        Details.NeckAppendages = (float)(Double)DALHelper.HandleDBNull(reader["NeckAppendages"]);
                        Details.Pyriform = (float)(Double)DALHelper.HandleDBNull(reader["Pyriform"]);
                        Details.Macrocefalic = (float)(Double)DALHelper.HandleDBNull(reader["Macrocefalic"]);
                        Details.Microcefalic = (float)(Double)DALHelper.HandleDBNull(reader["Microcefalic"]);
                        Details.BrockenNeck = (float)(Double)DALHelper.HandleDBNull(reader["BrockenNeck"]);
                        Details.RoundHead = (float)(Double)DALHelper.HandleDBNull(reader["RoundHead"]);
                        Details.DoubleHead = (float)(Double)DALHelper.HandleDBNull(reader["DoubleHead"]);
                        Details.Total = (float)(Double)DALHelper.HandleDBNull(reader["Total"]);
                        Details.MorphologicalAbnormilities = (float)(Double)DALHelper.HandleDBNull(reader["MorphologicalAbnormilities"]);
                        Details.NormalMorphology = (float)(Double)DALHelper.HandleDBNull(reader["NormalMorphology"]);
                        Details.Comment = (string)DALHelper.HandleDBNull(reader["Comment"]);
                        Details.CytoplasmicDroplet = (float)(Double)DALHelper.HandleDBNull(reader["CytoplasmicDroplet"]);
                        Details.Others = (float)(Double)DALHelper.HandleDBNull(reader["Others"]);
                        Details.MidPieceTotal = (float)(Double)DALHelper.HandleDBNull(reader["MidPieceTotal"]);
                        Details.CoiledTail = (float)(Double)DALHelper.HandleDBNull(reader["CoiledTail"]);
                        Details.ShortTail = (float)(Double)DALHelper.HandleDBNull(reader["ShortTail"]);
                        Details.HairpinTail = (float)(Double)DALHelper.HandleDBNull(reader["HairpinTail"]);
                        Details.DoubleTail = (float)(Double)DALHelper.HandleDBNull(reader["DoubleTail"]);
                        Details.TailOthers = (float)(Double)DALHelper.HandleDBNull(reader["TailOthers"]);
                        Details.TailTotal = (float)(Double)DALHelper.HandleDBNull(reader["TailTotal"]);
                        Details.HeadToHead = (string)DALHelper.HandleDBNull(reader["HeadToHead"]);
                        Details.TailToTail = (string)DALHelper.HandleDBNull(reader["TailToTail"]);
                        Details.HeadToTail = (string)DALHelper.HandleDBNull(reader["HeadToTail"]);
                        Details.SpermToOther = (string)DALHelper.HandleDBNull(reader["SpermToOther"]);
                        Details.PusCells = (string)DALHelper.HandleDBNull(reader["PusCells"]);
                        Details.RoundCells = (string)DALHelper.HandleDBNull(reader["RoundCells"]);
                        Details.EpithelialCells = (string)DALHelper.HandleDBNull(reader["EpithelialCells"]);
                        Details.Infections = (string)DALHelper.HandleDBNull(reader["Infections"]);
                        Details.OtherFindings = (string)DALHelper.HandleDBNull(reader["OtherFindings"]);
                        //Details.Interpretations = (string)DALHelper.HandleDBNull(reader["Interpretations"]);
                        Details.InterpretationsID = (long)DALHelper.HandleDBNull(reader["InterpretationsID"]);
                        Details.RangeViscosityID = (long)DALHelper.HandleDBNull(reader["RangeViscosityID"]);
                        Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));                        
                        Details.VolumeRange = "<1.5 - Low, 2.0 - Normal, >2.0 - High";
                        Details.PHRange = "<7.0 - Acidic, >=7.2 - Normal";
                        Details.PusCellsRange = "<04 - Not Significant";
                        Details.MorphologyAbnormilityRange = "<4% - Abnormal forms";
                        Details.NormalMorphologyRange = ">=4 Normal forms";
                        Details.SpermConcentrationRange = ">15 mill/ml - Normal";
                        Details.EjaculateRange = ">= 39 millions - Normal";
                        Details.TotalMotilityRange = ">40% - Normal";
                        //added by neena
                        Details.AllComments = (string)DALHelper.HandleDBNull(reader["Comments"]);
                        Details.Sperm5thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["Sperm5thPercentile"]);
                        Details.Sperm75thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["Sperm75thPercentile"]);
                        Details.Ejaculate5thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["Ejaculate5thPercentile"]);
                        Details.Ejaculate75thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["Ejaculate75thPercentile"]);
                        Details.TotalMotility5thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["TotalMotility5thPercentile"]);
                        Details.TotalMotility75thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["TotalMotility75thPercentile"]);
                        Details.RapidProgressive = (float)(Double)DALHelper.HandleDBNull(reader["RapidProgressive"]);
                        Details.SlowProgressive = (float)(Double)DALHelper.HandleDBNull(reader["SlowProgressive"]);
                        Details.SpermMorphology5thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["SpermMorphology5thPercentile"]);
                        Details.SpermMorphology75thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["SpermMorphology75thPercentile"]);
                        Details.NormalFormsComments = (string)DALHelper.HandleDBNull(reader["NormalFormsComments"]);
                        Details.OverAllDefectsComments = (string)DALHelper.HandleDBNull(reader["OverAllDefectsComments"]);
                        Details.HeadDefectsComments = (string)DALHelper.HandleDBNull(reader["HeadDefectsComments"]);
                        Details.MidPieceNeckDefectsComments = (string)DALHelper.HandleDBNull(reader["MidPieceNeckDefectsComments"]);
                        Details.TailDefectsComments = (string)DALHelper.HandleDBNull(reader["TailDefectsComments"]);
                        Details.ExcessiveResidualComments = (string)DALHelper.HandleDBNull(reader["ExcessiveResidualComments"]);
                        Details.SpermMorphologySubNormal = (string)DALHelper.HandleDBNull(reader["SpermMorphologySubNormal"]);
                        Details.Spillage = (string)DALHelper.HandleDBNull(reader["Spillage"]);
                        Details.Fructose = (string)DALHelper.HandleDBNull(reader["Fructose"]);
                        Details.Live = (float)(Double)DALHelper.HandleDBNull(reader["Live"]);
                        Details.Dead = (float)(Double)DALHelper.HandleDBNull(reader["Dead"]);
                        Details.TotalAdvanceMotility = (float)(Double)DALHelper.HandleDBNull(reader["TotalAdvanceMotility"]);
                        Details.TotalAdvance5thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["TotalAdvance5thPercentile"]);
                        Details.TotalAdvance75thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["TotalAdvance75thPercentile"]);
                        //

                        BizActionObj.List.Add(Details);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception e)
            {
            }
            return BizActionObj;

        }

        // Semen Wash

        public override IValueObject AddUpdateSemenWashDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            cls_IVFDashboard_AddUpdateSemenWashBizActionVO BizActionObj = valueObject as cls_IVFDashboard_AddUpdateSemenWashBizActionVO;

            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateSemenWash");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.SemensWashDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.SemensWashDetails.PatientUnitID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.SemensWashDetails.VisitID);

                dbServer.AddInParameter(command, "SpermTypeID", DbType.Int64, BizActionObj.SemensWashDetails.SpermTypeID);
                dbServer.AddInParameter(command, "SampleCode", DbType.String, BizActionObj.SemensWashDetails.SampleCode);
                dbServer.AddInParameter(command, "SampleLinkID", DbType.String, BizActionObj.SemensWashDetails.SampleLinkID);

                dbServer.AddInParameter(command, "CollectionDate", DbType.DateTime, BizActionObj.SemensWashDetails.CollectionDate);
                //dbServer.AddInParameter(command, "TimeRecSampLab", DbType.DateTime, BizActionObj.SemensWashDetails.TimeRecSampLab);
                dbServer.AddInParameter(command, "MethodOfCollection", DbType.Int64, BizActionObj.SemensWashDetails.MethodOfCollectionID);
                dbServer.AddInParameter(command, "Abstinence", DbType.Int64, BizActionObj.SemensWashDetails.AbstinenceID);

                dbServer.AddInParameter(command, "IUIDate", DbType.DateTime, BizActionObj.SemensWashDetails.IUIDate);
                dbServer.AddInParameter(command, "InSeminatedByID", DbType.Int64, BizActionObj.SemensWashDetails.InSeminatedByID);
                dbServer.AddInParameter(command, "InSeminationLocationID", DbType.Int64, BizActionObj.SemensWashDetails.InSeminationLocationID);
                dbServer.AddInParameter(command, "WitnessByID", DbType.Int64, BizActionObj.SemensWashDetails.WitnessByID);
                dbServer.AddInParameter(command, "InSeminationMethodID", DbType.Int64, BizActionObj.SemensWashDetails.InSeminationMethodID);
                dbServer.AddInParameter(command, "ThawDate", DbType.DateTime, BizActionObj.SemensWashDetails.ThawDate);
                dbServer.AddInParameter(command, "SampleID", DbType.String, BizActionObj.SemensWashDetails.SampleID);
                dbServer.AddInParameter(command, "DonorID", DbType.Int64, BizActionObj.SemensWashDetails.DonorID);
                dbServer.AddInParameter(command, "DonorUnitID", DbType.Int64, BizActionObj.SemensWashDetails.DonorUnitID);
                dbServer.AddInParameter(command, "ISFromIUI", DbType.Boolean, BizActionObj.SemensWashDetails.ISFromIUI);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.SemensWashDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.SemensWashDetails.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "BatchID", DbType.Int64, BizActionObj.SemensWashDetails.BatchID);
                dbServer.AddInParameter(command, "BatchUnitID", DbType.Int64, BizActionObj.SemensWashDetails.BatchUnitID);
                dbServer.AddInParameter(command, "BatchCode", DbType.String, BizActionObj.SemensWashDetails.BatchCode);



                dbServer.AddInParameter(command, "CollecteAtCentre", DbType.Boolean, BizActionObj.SemensWashDetails.CollecteAtCentre);
                dbServer.AddInParameter(command, "IsFrozenSample", DbType.Boolean, BizActionObj.SemensWashDetails.IsFrozenSample);

                dbServer.AddInParameter(command, "Color", DbType.Int64, BizActionObj.SemensWashDetails.ColorID);
                dbServer.AddInParameter(command, "Quantity", DbType.Single, BizActionObj.SemensWashDetails.Quantity);
                dbServer.AddInParameter(command, "PH", DbType.Single, BizActionObj.SemensWashDetails.PH);
                dbServer.AddInParameter(command, "LiquificationTime", DbType.String, BizActionObj.SemensWashDetails.LiquificationTime);
                dbServer.AddInParameter(command, "Viscosity", DbType.Boolean, BizActionObj.SemensWashDetails.Viscosity);
                dbServer.AddInParameter(command, "RangeViscosityID", DbType.Int64, BizActionObj.SemensWashDetails.RangeViscosityID);
                dbServer.AddInParameter(command, "Odour", DbType.Boolean, BizActionObj.SemensWashDetails.Odour);

                dbServer.AddInParameter(command, "PreSpermCount", DbType.Single, BizActionObj.SemensWashDetails.PreSpermCount);
                dbServer.AddInParameter(command, "PreTotalSpermCount", DbType.Single, BizActionObj.SemensWashDetails.PreTotalSpermCount);
                dbServer.AddInParameter(command, "PreMotility", DbType.Single, BizActionObj.SemensWashDetails.PreMotility);
                dbServer.AddInParameter(command, "PreNonMotility", DbType.Single, BizActionObj.SemensWashDetails.PreNonMotility);
                dbServer.AddInParameter(command, "PreTotalMotility", DbType.Single, BizActionObj.SemensWashDetails.PreTotalMotility);
                dbServer.AddInParameter(command, "PreMotilityGradeI", DbType.Single, BizActionObj.SemensWashDetails.PreMotilityGradeI);
                dbServer.AddInParameter(command, "PreMotilityGradeII", DbType.Single, BizActionObj.SemensWashDetails.PreMotilityGradeII);
                dbServer.AddInParameter(command, "PreMotilityGradeIII", DbType.Single, BizActionObj.SemensWashDetails.PreMotilityGradeIII);
                dbServer.AddInParameter(command, "PreMotilityGradeIV", DbType.Single, BizActionObj.SemensWashDetails.PreMotilityGradeIV);
                dbServer.AddInParameter(command, "PreNormalMorphology", DbType.Single, BizActionObj.SemensWashDetails.PreNormalMorphology);



                dbServer.AddInParameter(command, "PostSpermCount", DbType.Single, BizActionObj.SemensWashDetails.PostSpermCount);
                dbServer.AddInParameter(command, "PostTotalSpermCount", DbType.Single, BizActionObj.SemensWashDetails.PostTotalSpermCount);
                dbServer.AddInParameter(command, "PostMotility", DbType.Single, BizActionObj.SemensWashDetails.PostMotility);
                dbServer.AddInParameter(command, "PostNonMotility", DbType.Single, BizActionObj.SemensWashDetails.PostNonMotility);
                dbServer.AddInParameter(command, "PostTotalMotility", DbType.Single, BizActionObj.SemensWashDetails.PostTotalMotility);
                dbServer.AddInParameter(command, "PostMotilityGradeI", DbType.Single, BizActionObj.SemensWashDetails.PostMotilityGradeI);
                dbServer.AddInParameter(command, "PostMotilityGradeII", DbType.Single, BizActionObj.SemensWashDetails.PostMotilityGradeII);
                dbServer.AddInParameter(command, "PostMotilityGradeIII", DbType.Single, BizActionObj.SemensWashDetails.PostMotilityGradeIII);
                dbServer.AddInParameter(command, "PostMotilityGradeIV", DbType.Single, BizActionObj.SemensWashDetails.PostMotilityGradeIV);
                dbServer.AddInParameter(command, "PostNormalMorphology", DbType.Single, BizActionObj.SemensWashDetails.PostNormalMorphology);

                dbServer.AddInParameter(command, "PusCells", DbType.String, BizActionObj.SemensWashDetails.PusCells);
                dbServer.AddInParameter(command, "RoundCells", DbType.String, BizActionObj.SemensWashDetails.RoundCells);
                dbServer.AddInParameter(command, "EpithelialCells", DbType.String, BizActionObj.SemensWashDetails.EpithelialCells);
                dbServer.AddInParameter(command, "CheckedByDoctorID", DbType.Int64, BizActionObj.SemensWashDetails.CheckedByDoctorID);
                dbServer.AddInParameter(command, "OtherCells", DbType.String, BizActionObj.SemensWashDetails.AnyOtherCells);

                dbServer.AddInParameter(command, "Comment", DbType.String, BizActionObj.SemensWashDetails.Comment);
                dbServer.AddInParameter(command, "CommentID", DbType.String, BizActionObj.SemensWashDetails.CommentID);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.SemensWashDetails.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "Synchronized", DbType.Boolean, BizActionObj.SemensWashDetails.Synchronized);

                // Added as per requirement of Milann

                dbServer.AddInParameter(command, "PreAmount", DbType.Single, BizActionObj.SemensWashDetails.PreAmount);
                dbServer.AddInParameter(command, "PostAmount", DbType.Single, BizActionObj.SemensWashDetails.PostAmount);
                dbServer.AddInParameter(command, "PreProgMotility", DbType.Single, BizActionObj.SemensWashDetails.PreProgMotility);
                dbServer.AddInParameter(command, "PostProgMotility", DbType.Single, BizActionObj.SemensWashDetails.PostProgMotility);
                dbServer.AddInParameter(command, "PreNonProgressive", DbType.Single, BizActionObj.SemensWashDetails.PreNonProgressive);
                dbServer.AddInParameter(command, "PostNonProgressive", DbType.Single, BizActionObj.SemensWashDetails.PostNonProgressive);
                dbServer.AddInParameter(command, "PreNonMotile", DbType.Single, BizActionObj.SemensWashDetails.PreNonMotile);
                dbServer.AddInParameter(command, "PostNonMotile", DbType.Single, BizActionObj.SemensWashDetails.PostNonMotile);
                dbServer.AddInParameter(command, "PreMotileSpermCount", DbType.Single, BizActionObj.SemensWashDetails.PreMotileSpermCount);
                dbServer.AddInParameter(command, "PostMotileSpermCount", DbType.Single, BizActionObj.SemensWashDetails.PostMotileSpermCount);

                dbServer.AddInParameter(command, "PreNormalForms", DbType.Single, BizActionObj.SemensWashDetails.PreNormalForms);
                dbServer.AddInParameter(command, "PostNormalForms", DbType.Single, BizActionObj.SemensWashDetails.PostNormalForms);
                dbServer.AddInParameter(command, "PreperationDate", DbType.DateTime, BizActionObj.SemensWashDetails.PreperationDate);
                dbServer.AddInParameter(command, "InseminatedAmount", DbType.String, BizActionObj.SemensWashDetails.Inseminated);
                dbServer.AddInParameter(command, "MotileSperm", DbType.String, BizActionObj.SemensWashDetails.MotileSperm);
                // add by devidas 9/5/2017  for andrologist
                dbServer.AddInParameter(command, "Spillage", DbType.String, BizActionObj.SemensWashDetails.Spillage);
                dbServer.AddInParameter(command, "MediaUsed", DbType.String, BizActionObj.SemensWashDetails.MediaUsed);
                dbServer.AddInParameter(command, "SemenProcessingMethodID", DbType.Int64, BizActionObj.SemensWashDetails.SemenProcessingMethodID);
                dbServer.AddInParameter(command, "HIV", DbType.Single, BizActionObj.SemensWashDetails.HIV);
                dbServer.AddInParameter(command, "VDRL", DbType.Single, BizActionObj.SemensWashDetails.VDRL);
                dbServer.AddInParameter(command, "HBSAG", DbType.Single, BizActionObj.SemensWashDetails.HBSAG);
                dbServer.AddInParameter(command, "HCV", DbType.Single, BizActionObj.SemensWashDetails.HCV);
                dbServer.AddInParameter(command, "TransacationTypeID", DbType.Single, BizActionObj.SemensWashDetails.TransacationTypeID);
                dbServer.AddInParameter(command, "BloodGroupID", DbType.Single, BizActionObj.SemensWashDetails.BloodGroupID);
                dbServer.AddInParameter(command, "SpermRecoveredFrom", DbType.String, BizActionObj.SemensWashDetails.SpermRecoveredFrom);
                //added by neena
                dbServer.AddInParameter(command, "Comments", DbType.String, BizActionObj.SemensWashDetails.AllComments);
                dbServer.AddInParameter(command, "Sperm5thPercentile", DbType.Single, BizActionObj.SemensWashDetails.Sperm5thPercentile);
                dbServer.AddInParameter(command, "Sperm75thPercentile", DbType.Single, BizActionObj.SemensWashDetails.Sperm75thPercentile);
                dbServer.AddInParameter(command, "Ejaculate5thPercentile", DbType.Single, BizActionObj.SemensWashDetails.Ejaculate5thPercentile);
                dbServer.AddInParameter(command, "Ejaculate75thPercentile", DbType.Single, BizActionObj.SemensWashDetails.Ejaculate75thPercentile);
                dbServer.AddInParameter(command, "TotalMotility5thPercentile", DbType.Single, BizActionObj.SemensWashDetails.TotalMotility5thPercentile);
                dbServer.AddInParameter(command, "TotalMotility75thPercentile", DbType.Single, BizActionObj.SemensWashDetails.TotalMotility75thPercentile);
                //

                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, BizActionObj.SemensWashDetails.IsFreezed);

                dbServer.AddParameter(command, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.SemensWashDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.SemensWashDetails = null;


                throw ex;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;

        }

        public override IValueObject GetSemenWashDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();

            //cls_GetIVFDashboard_SemenBizActionVO BizActionObj = valueObject as cls_GetIVFDashboard_SemenBizActionVO;
            cls_GetIVFDashboard_SemenWashBizActionVO BizActionObj = valueObject as cls_GetIVFDashboard_SemenWashBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSemenWashDetails");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<cls_IVFDashboard_SemenWashVO>();
                    while (reader.Read())
                    {
                        cls_IVFDashboard_SemenWashVO Details = new cls_IVFDashboard_SemenWashVO();

                        Details.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        Details.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        Details.CollectionDate = (DateTime?)DALHelper.HandleDate(reader["CollectionDate"]);
                        Details.MethodOfCollection = (string)DALHelper.HandleDBNull(reader["MethodOfCollection"]);
                        Details.MethodOfCollectionID = (long)DALHelper.HandleDBNull(reader["MethodOfCollectionID"]);
                        Details.TimeRecSampLab = (DateTime?)DALHelper.HandleDBNull(reader["TimeRecSampLab"]);
                        Details.AbstinenceID = (long)DALHelper.HandleDBNull(reader["Abstinence"]);
                        Details.IsFrozenSample = (Boolean)DALHelper.HandleDBNull(reader["IsFrozenSample"]);
                        Details.CollecteAtCentre = (Boolean)DALHelper.HandleDBNull(reader["CollecteAtCentre"]);
                        Details.ColorID = (long)DALHelper.HandleDBNull(reader["Color"]);
                        Details.Quantity = (float)(Double)(Double)DALHelper.HandleDBNull(reader["Quantity"]);
                        Details.PH = (float)(Double)DALHelper.HandleDBNull(reader["PH"]);
                        Details.LiquificationTime = (string)DALHelper.HandleDBNull(reader["LiquificationTime"]);
                        Details.Viscosity = (Boolean)DALHelper.HandleDBNull(reader["Viscosity"]);
                        Details.Odour = (Boolean)DALHelper.HandleDBNull(reader["Odour"]);

                        Details.CheckedByDoctorID = (long)DALHelper.HandleDBNull(reader["CheckedByDoctorID"]);

                        Details.PreSpermCount = (float)(Double)DALHelper.HandleDBNull(reader["PreSpermCount"]);
                        Details.PreTotalSpermCount = (float)(Double)DALHelper.HandleDBNull(reader["PreTotalSpermCount"]);
                        Details.PreMotility = (float)(Double)DALHelper.HandleDBNull(reader["PreMotility"]);
                        Details.PreNonMotility = (float)(Double)DALHelper.HandleDBNull(reader["PreNonMotility"]);
                        Details.PreTotalMotility = (float)(Double)DALHelper.HandleDBNull(reader["PreTotalMotility"]);
                        Details.PreMotilityGradeI = (float)(Double)DALHelper.HandleDBNull(reader["PreMotilityGradeI"]);
                        Details.PreMotilityGradeII = (float)(Double)DALHelper.HandleDBNull(reader["PreMotilityGradeII"]);
                        Details.PreMotilityGradeIII = (float)(Double)DALHelper.HandleDBNull(reader["PreMotilityGradeIII"]);
                        Details.PreMotilityGradeIV = (float)(Double)DALHelper.HandleDBNull(reader["PreMotilityGradeIV"]);
                        Details.PreNormalMorphology = (float)(Double)DALHelper.HandleDBNull(reader["PreNormalMorphology"]);

                        //Details.Comment = (string)DALHelper.HandleDBNull(reader["Comment"]); 
                        Details.TypeOfSperm = Convert.ToString(DALHelper.HandleDBNull(reader["TypeOfSperm"]));
                        Details.SpermTypeID = (long)DALHelper.HandleDBNull(reader["TypeOfSpermID"]);

                        Details.SampleCode = Convert.ToString(DALHelper.HandleDBNull(reader["SampleCode"]));
                        Details.SampleLinkID = Convert.ToString(DALHelper.HandleDBNull(reader["SampleLinkID"]));

                        Details.CommentID = (long)DALHelper.HandleDBNull(reader["CommentID"]);
                        Details.RangeViscosityID = (long)DALHelper.HandleDBNull(reader["RangeViscosityID"]);

                        Details.PostSpermCount = (float)(Double)DALHelper.HandleDBNull(reader["PostSpermCount"]);
                        Details.PostTotalSpermCount = (float)(Double)DALHelper.HandleDBNull(reader["PostTotalSpermCount"]);
                        Details.PostMotility = (float)(Double)DALHelper.HandleDBNull(reader["PostMotility"]);
                        Details.PostNonMotility = (float)(Double)DALHelper.HandleDBNull(reader["PostNonMotility"]);
                        Details.PostTotalMotility = (float)(Double)DALHelper.HandleDBNull(reader["PostTotalMotility"]);
                        Details.PostMotilityGradeI = (float)(Double)DALHelper.HandleDBNull(reader["PostMotilityGradeI"]);
                        Details.PostMotilityGradeII = (float)(Double)DALHelper.HandleDBNull(reader["PostMotilityGradeII"]);
                        Details.PostMotilityGradeIII = (float)(Double)DALHelper.HandleDBNull(reader["PostMotilityGradeIII"]);
                        Details.PostMotilityGradeIV = (float)(Double)DALHelper.HandleDBNull(reader["PostMotilityGradeIV"]);
                        Details.PostNormalMorphology = (float)(Double)DALHelper.HandleDBNull(reader["PostNormalMorphology"]);

                        Details.PusCells = (string)DALHelper.HandleDBNull(reader["PusCells"]);
                        Details.RoundCells = (string)DALHelper.HandleDBNull(reader["RoundCells"]);
                        Details.EpithelialCells = (string)DALHelper.HandleDBNull(reader["EpithelialCells"]);
                        Details.AnyOtherCells = (string)DALHelper.HandleDBNull(reader["OtherCells"]);
                        Details.Spillage = (string)DALHelper.HandleDBNull(reader["Spillage"]);
                        Details.MediaUsed = (string)DALHelper.HandleDBNull(reader["MediaUsed"]);
                        Details.SemenProcessingMethodID = (long)DALHelper.HandleDBNull(reader["SemenProcessingMethodID"]);
                        Details.HIV = Convert.ToInt64(DALHelper.HandleDBNull(reader["HIV"]));
                        Details.HCV = Convert.ToInt64(DALHelper.HandleDBNull(reader["HCV"]));
                        Details.VDRL = Convert.ToInt64(DALHelper.HandleDBNull(reader["VDRL"]));
                        Details.HBSAG = Convert.ToInt64(DALHelper.HandleDBNull(reader["HBSAG"]));
                        Details.SpermRecoveredFrom = Convert.ToString(DALHelper.HandleDBNull(reader["SpermRecoverdFrom"]));
                        Details.TransacationTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionTypeId"]));
                        Details.BloodGroupID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BloodGroupID"]));
                        Details.Interpretations = Convert.ToString(DALHelper.HandleDBNull(reader["Interpretations"]));
                        Details.Andrologist = Convert.ToString(DALHelper.HandleDBNull(reader["Andrologist"]));

                        Details.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        //Details.Infections = (string)DALHelper.HandleDBNull(reader["Infections"]);
                        //Details.OtherFindings = (string)DALHelper.HandleDBNull(reader["OtherFindings"]);
                        //Details.Interpretations = (string)DALHelper.HandleDBNull(reader["Interpretations"]);
                        Details.IUIDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["IUIDate"]));
                        Details.PreperationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["PreperationDate"]));
                        Details.InSeminatedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InSeminatedByID"]));
                        Details.WitnessByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WitnessByID"]));
                        Details.Inseminated = Convert.ToString(DALHelper.HandleDBNull(reader["InseminatedAmount"]));
                        Details.PostAmount = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostAmount"])));
                        Details.PostProgMotility = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostProgMotility"])));
                        Details.PostNormalForms = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostNormalForms"])));
                        Details.PreAmount = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreAmount"])));
                        Details.PreProgMotility = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreProgMotility"])));
                        Details.PreNormalForms = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreNormalForms"])));
                        Details.PreNonProgressive = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreNonProgressive"])));
                        Details.PostNonProgressive = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostNonProgressive"])));
                        Details.PreNonMotile = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreNonMotile"])));
                        Details.PostNonMotile = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostNonMotile"])));
                        Details.InSeminatedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InSeminatedByID"]));
                        Details.WitnessByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WitnessByID"]));
                        Details.Comment = Convert.ToString(DALHelper.HandleDBNull(reader["IUIComment"]));
                        //added by neena
                        Details.AllComments = (string)DALHelper.HandleDBNull(reader["Comments"]);
                        Details.Sperm5thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["Sperm5thPercentile"]);
                        Details.Sperm75thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["Sperm75thPercentile"]);
                        Details.Ejaculate5thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["Ejaculate5thPercentile"]);
                        Details.Ejaculate75thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["Ejaculate75thPercentile"]);
                        Details.TotalMotility5thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["TotalMotility5thPercentile"]);
                        Details.TotalMotility75thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["TotalMotility75thPercentile"]);
                        //
                        BizActionObj.List.Add(Details);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception e)
            {
            }
            return BizActionObj;

        }

        public override IValueObject GetNewIUIDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_GetIVFDashboard_NewIUIDetailsBizActionVO BizActionObj = valueObject as cls_GetIVFDashboard_NewIUIDetailsBizActionVO;

            try
            {

                // DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVFDashBoard_GetNewIUIDetails");
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVFDashBoard_GetNewIUIDetails_New_1");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.SemensExaminationDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.SemensExaminationDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.SemensExaminationDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.SemensExaminationDetails.PlanTherapyUnitID);



                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizActionObj.SemensExaminationDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"])); //added by neena
                        BizActionObj.SemensExaminationDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Unitid"])); //added by neena
                        BizActionObj.SemensExaminationDetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        BizActionObj.SemensExaminationDetails.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        BizActionObj.SemensExaminationDetails.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        BizActionObj.SemensExaminationDetails.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        BizActionObj.SemensExaminationDetails.DonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorID"]));
                        BizActionObj.SemensExaminationDetails.DonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorUnitID"]));
                        BizActionObj.SemensExaminationDetails.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"]));
                        BizActionObj.SemensExaminationDetails.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        BizActionObj.SemensExaminationDetails.BloodGroup = Convert.ToString(DALHelper.HandleDBNull(reader["BloodGroup"]));
                        BizActionObj.SemensExaminationDetails.SampleID = Convert.ToString(DALHelper.HandleDBNull(reader["SampleID"]));
                        BizActionObj.SemensExaminationDetails.Hair = Convert.ToString(DALHelper.HandleDBNull(reader["Hair"]));
                        BizActionObj.SemensExaminationDetails.Skin = Convert.ToString(DALHelper.HandleDBNull(reader["Skin"]));
                        BizActionObj.SemensExaminationDetails.Eye = Convert.ToString(DALHelper.HandleDBNull(reader["Eye"]));
                        BizActionObj.SemensExaminationDetails.BoneStructure = Convert.ToString(DALHelper.HandleDBNull(reader["BoneStructure"]));
                        BizActionObj.SemensExaminationDetails.Height = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["Height"])));
                        BizActionObj.SemensExaminationDetails.Lab = Convert.ToString(DALHelper.HandleDBNull(reader["Lab"]));
                        BizActionObj.SemensExaminationDetails.ThawDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawDate"]));
                        BizActionObj.SemensExaminationDetails.IUIDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["IUIDate"]));
                        BizActionObj.SemensExaminationDetails.InSeminatedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InSeminatedByID"]));
                        BizActionObj.SemensExaminationDetails.WitnessByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WitnessByID"]));
                        BizActionObj.SemensExaminationDetails.InSeminationLocationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InSeminationLocationID"]));
                        BizActionObj.SemensExaminationDetails.InSeminationMethodID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InSeminationMethodID"]));
                        BizActionObj.SemensExaminationDetails.CollectionDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["CollectionDate"]));


                        BizActionObj.SemensExaminationDetails.MethodOfCollectionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MethodOfCollection"]));
                        BizActionObj.SemensExaminationDetails.TimeRecSampLab = Convert.ToDateTime(DALHelper.HandleDBNull(reader["TimeRecSampLab"]));
                        BizActionObj.SemensExaminationDetails.AbstinenceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Abstinence"]));
                        BizActionObj.SemensExaminationDetails.CollecteAtCentre = Convert.ToBoolean(DALHelper.HandleDBNull(reader["CollecteAtCentre"]));
                        BizActionObj.SemensExaminationDetails.ColorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Color"]));
                        BizActionObj.SemensExaminationDetails.Quantity = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])));
                        BizActionObj.SemensExaminationDetails.PH = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PH"])));
                        BizActionObj.SemensExaminationDetails.LiquificationTime = Convert.ToString(DALHelper.HandleDBNull(reader["LiquificationTime"]));
                        BizActionObj.SemensExaminationDetails.Viscosity = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Viscosity"]));
                        BizActionObj.SemensExaminationDetails.Odour = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Odour"]));
                        BizActionObj.SemensExaminationDetails.IsFrozenSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFrozenSample"]));
                        BizActionObj.SemensExaminationDetails.PostMotility = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostMotility"])));
                        BizActionObj.SemensExaminationDetails.PostMotilityGradeI = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostMotilityGradeI"])));
                        BizActionObj.SemensExaminationDetails.PostMotilityGradeII = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostMotilityGradeII"])));
                        BizActionObj.SemensExaminationDetails.PostMotilityGradeIII = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostMotilityGradeIII"])));
                        BizActionObj.SemensExaminationDetails.PostMotilityGradeIV = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostMotilityGradeIV"])));
                        BizActionObj.SemensExaminationDetails.PostNonMotility = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostNonMotility"])));
                        BizActionObj.SemensExaminationDetails.PostNormalMorphology = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostNormalMorphology"])));
                        BizActionObj.SemensExaminationDetails.PostSpermCount = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostSpermCount"])));
                        BizActionObj.SemensExaminationDetails.PostTotalMotility = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostTotalMotility"])));
                        BizActionObj.SemensExaminationDetails.PostTotalSpermCount = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostTotalSpermCount"])));
                        BizActionObj.SemensExaminationDetails.PreMotility = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreMotility"])));
                        BizActionObj.SemensExaminationDetails.PreMotilityGradeI = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreMotilityGradeI"])));
                        BizActionObj.SemensExaminationDetails.PreMotilityGradeII = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreMotilityGradeII"])));
                        BizActionObj.SemensExaminationDetails.PreMotilityGradeIII = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreMotilityGradeIII"])));
                        BizActionObj.SemensExaminationDetails.PreMotilityGradeIV = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreMotilityGradeIV"])));
                        BizActionObj.SemensExaminationDetails.PreNonMotility = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreNonMotility"])));
                        BizActionObj.SemensExaminationDetails.PreNormalMorphology = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreNormalMorphology"])));
                        BizActionObj.SemensExaminationDetails.PreSpermCount = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreSpermCount"])));
                        BizActionObj.SemensExaminationDetails.PreTotalMotility = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreTotalMotility"])));
                        BizActionObj.SemensExaminationDetails.ThawDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawDate"]));
                        BizActionObj.SemensExaminationDetails.TimeRecSampLab = Convert.ToDateTime(DALHelper.HandleDBNull(reader["TimeRecSampLab"]));
                        BizActionObj.SemensExaminationDetails.PreTotalSpermCount = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreTotalSpermCount"])));
                        BizActionObj.SemensExaminationDetails.Comment = Convert.ToString(DALHelper.HandleDBNull(reader["Comment"]));
                        BizActionObj.SemensExaminationDetails.PusCells = Convert.ToString(DALHelper.HandleDBNull(reader["PusCells"]));
                        BizActionObj.SemensExaminationDetails.RoundCells = Convert.ToString(DALHelper.HandleDBNull(reader["RoundCells"]));
                        BizActionObj.SemensExaminationDetails.EpithelialCells = Convert.ToString(DALHelper.HandleDBNull(reader["EpithelialCells"]));
                        BizActionObj.SemensExaminationDetails.AnyOtherCells = Convert.ToString(DALHelper.HandleDBNull(reader["OtherCells"]));
                        BizActionObj.SemensExaminationDetails.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        BizActionObj.SemensExaminationDetails.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        BizActionObj.SemensExaminationDetails.BatchUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchUnitID"]));

                        // Added as per the requirements of Milann

                        BizActionObj.SemensExaminationDetails.PostAmount = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostAmount"])));
                        BizActionObj.SemensExaminationDetails.PostProgMotility = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostProgMotility"])));
                        BizActionObj.SemensExaminationDetails.PostNormalForms = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostNormalForms"])));
                        BizActionObj.SemensExaminationDetails.PreAmount = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreAmount"])));
                        BizActionObj.SemensExaminationDetails.PreProgMotility = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreProgMotility"])));
                        BizActionObj.SemensExaminationDetails.PreNormalForms = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreNormalForms"])));
                        BizActionObj.SemensExaminationDetails.Cyclecode = Convert.ToString(DALHelper.HandleDBNull(reader["Cyclecode"]));
                        BizActionObj.SemensExaminationDetails.CycleDuration = Convert.ToString(DALHelper.HandleDBNull(reader["CycleDuration"]));
                        BizActionObj.SemensExaminationDetails.Inseminated = Convert.ToString(DALHelper.HandleDBNull(reader["InseminatedAmount"]));
                        BizActionObj.SemensExaminationDetails.MotileSperm = Convert.ToString(DALHelper.HandleDBNull(reader["MotileSperm"]));
                        BizActionObj.SemensExaminationDetails.PreperationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["PreperationDate"]));

                        BizActionObj.SemensExaminationDetails.ExternalSimulationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExternalSimulationID"]));
                        BizActionObj.SemensExaminationDetails.PlannedTreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedTreatmentID"]));
                        BizActionObj.SemensExaminationDetails.Cyclecode = Convert.ToString(DALHelper.HandleDBNull(reader["Cyclecode"]));
                        BizActionObj.SemensExaminationDetails.CycleDuration = Convert.ToString(DALHelper.HandleDBNull(reader["CycleDuration"]));
                        BizActionObj.SemensExaminationDetails.MainInductionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainInductionID"]));

                        BizActionObj.SemensExaminationDetails.PreNonProgressive = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreNonProgressive"])));
                        BizActionObj.SemensExaminationDetails.PostNonProgressive = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostNonProgressive"])));
                        BizActionObj.SemensExaminationDetails.PreNonMotile = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreNonMotile"])));
                        BizActionObj.SemensExaminationDetails.PostNonMotile = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostNonMotile"])));
                        BizActionObj.SemensExaminationDetails.PreMotileSpermCount = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PreMotileSpermCount"])));
                        BizActionObj.SemensExaminationDetails.PostMotileSpermCount = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["PostMotileSpermCount"])));
                        BizActionObj.SemensExaminationDetails.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCodeMrno"]));
                        //





                    }
                }
                reader.Close();
            }
            catch (Exception e)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddUpdateSemenUsedDetails(IValueObject valueObject, clsUserVO UserVo)      // For IVF ADM Changes
        {
            //throw new NotImplementedException();
            cls_IVFDashboard_AddUpdateSemenUsedBizActionVO BizActionObj = valueObject as cls_IVFDashboard_AddUpdateSemenUsedBizActionVO;

            cls_IVFDashboard_SemenWashVO objsemenused = new cls_IVFDashboard_SemenWashVO();

            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();

                objsemenused = BizActionObj.ListSemenUsed[0];
                foreach (var item in BizActionObj.ListSemenUsed)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateSemenUsedDetails");
                    dbServer.AddInParameter(command, "ID", DbType.Int64, item.SemenUsedDetailsID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, objsemenused.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command, "TypeOfSpreamID", DbType.Int64, item.SpermTypeID);
                    dbServer.AddInParameter(command, "SemenUsed", DbType.Int64, item.IsUsed);
                    dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, objsemenused.PlanTherapyID);

                    dbServer.AddInParameter(command, "SemenWashID", DbType.Int64, item.ID);
                    //  dbServer.AddInParameter(command, "SemenWashID", DbType.String, item.d);


                    dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, objsemenused.PlanTherapyUnitID);


                    dbServer.AddInParameter(command, "Status", DbType.Boolean, item.Status);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, item.CreatedUnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, item.AddedBy);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, item.AddedOn);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, item.AddedWindowsLoginName);
                    dbServer.AddInParameter(command, "Synchronized", DbType.Boolean, item.Synchronized);



                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(command);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                }

            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.SemensWashDetails = null;


                throw ex;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;

        }

        public override IValueObject GetSemenDetails(IValueObject valueObject, clsUserVO UserVo)        // Added By Yogesh K 14112016
        {
            // throw new NotImplementedException();

            //cls_GetIVFDashboard_SemenBizActionVO BizActionObj = valueObject as cls_GetIVFDashboard_SemenBizActionVO;
            cls_GetIVFDashboard_SemenDetailsBizActionVO BizActionObj = valueObject as cls_GetIVFDashboard_SemenDetailsBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSemenDetails");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);



                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.PlanTherapyUnitID);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<cls_IVFDashboard_SemenWashVO>();
                    while (reader.Read())
                    {
                        cls_IVFDashboard_SemenWashVO Details = new cls_IVFDashboard_SemenWashVO();

                        Details.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        Details.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        Details.SpermTypeID = (long)DALHelper.HandleDBNull(reader["TypeOfSpreamID"]);
                        Details.TypeOfSperm = (string)DALHelper.HandleDBNull(reader["TypeOfSperm"]);
                        //   Details.SemenWashID = (long)DALHelper.HandleDBNull(reader["SemenWashID"]);
                        Details.CollectionDate = (DateTime)DALHelper.HandleDBNull(reader["CollectionDate"]);

                        Details.ISChecked = (bool)DALHelper.HandleBoolDBNull(reader["ISChecked"]);
                        Details.UsedPlanTherapyID = (long)DALHelper.HandleDBNull(reader["UsedPlanTherapyID"]);
                        Details.UsedPlanTherapyUnitID = (long)DALHelper.HandleDBNull(reader["UsedPlanTherapyUnitID"]);
                        //  Details.semenused = (long)DALHelper.HandleDBNull(reader["semenused"]);



                        //Details.Infections = (string)DALHelper.HandleDBNull(reader["Infections"]);
                        //Details.OtherFindings = (string)DALHelper.HandleDBNull(reader["OtherFindings"]);
                        //Details.Interpretations = (string)DALHelper.HandleDBNull(reader["Interpretations"]);
                        BizActionObj.List.Add(Details);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception e)
            {
            }
            return BizActionObj;

        }

        public override IValueObject GetSemenUsedDetails(IValueObject valueObject, clsUserVO UserVo)        // Added By Yogesh K 14112016
        {
            // throw new NotImplementedException();

            //cls_GetIVFDashboard_SemenBizActionVO BizActionObj = valueObject as cls_GetIVFDashboard_SemenBizActionVO;
            cls_GetIVFDashboard_SemenUsedBizActionVO BizActionObj = valueObject as cls_GetIVFDashboard_SemenUsedBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSemenUsedDetails");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);



                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.PlanTherapyUnitID);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ListSaved == null)
                        BizActionObj.ListSaved = new List<cls_IVFDashboard_SemenWashVO>();
                    while (reader.Read())
                    {
                        cls_IVFDashboard_SemenWashVO Details = new cls_IVFDashboard_SemenWashVO();

                        Details.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        Details.SemenUsedDetailsID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        Details.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        Details.TypeOfSperm = (string)DALHelper.HandleDBNull(reader["TypeOfSperm"]);
                        Details.CollectionDate = (DateTime)DALHelper.HandleDBNull(reader["CollectionDate"]);
                        Details.IsUsed = (bool)DALHelper.HandleDBNull(reader["SemenUsed"]);


                        //Details.Infections = (string)DALHelper.HandleDBNull(reader["Infections"]);
                        //Details.OtherFindings = (string)DALHelper.HandleDBNull(reader["OtherFindings"]);
                        //Details.Interpretations = (string)DALHelper.HandleDBNull(reader["Interpretations"]);
                        BizActionObj.ListSaved.Add(Details);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception e)
            {
            }
            return BizActionObj;

        }

    }
}
